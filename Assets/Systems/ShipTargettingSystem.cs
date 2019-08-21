using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

public class ShipTargettingSystem : JobComponentSystem
{
    private ShipCreatorSystem ShipCreatorSystem;
    private EntityQuery Query;
    private float okDistance;

    protected override void OnCreate()
    {
        Query = GetEntityQuery(typeof(ShipComponent), ComponentType.ReadOnly<Translation>());
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        ShipCreatorSystem = EntityManager.World.GetExistingSystem<ShipCreatorSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var shipComponentType = GetArchetypeChunkComponentType<ShipComponent>();
        var shipTranslationType = GetArchetypeChunkComponentType<Translation>(true);
        
        var job = new TargettingJob()
        {
            ShipComponentType = shipComponentType,
            ShipTranslationType = shipTranslationType
        };

        return job.Schedule(Query, inputDeps);
    }

    [BurstCompile]
    struct TargettingJob : IJobChunk
    {
        public ArchetypeChunkComponentType<ShipComponent> ShipComponentType;
        [ReadOnly] public ArchetypeChunkComponentType<Translation> ShipTranslationType;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkShips = chunk.GetNativeArray(ShipComponentType);
            var chunkShipPositions = chunk.GetNativeArray(ShipTranslationType);

            for (var i = 0; i < chunk.Count; i++)
            {
                var ship = chunkShips[i];
                var shipTranslation = chunkShipPositions[i];
                var okDistance = ship.Speed * 1.5f * (1f / 18f);

                var closestDistance = float.MaxValue;

                for (int j = 0; j < chunk.Count ; j++)
                {
                    if (j == i) continue;
                    var otherShip = chunkShips[j];
                    var otherShipTranslation = chunkShipPositions[j];

                    var distance = math.distance(shipTranslation.Value, otherShipTranslation.Value);

                    if (distance < closestDistance && distance > okDistance)
                    {
                        closestDistance = distance;
                        ship.Target = otherShipTranslation.Value;
                    }
                }
            }
        }
    }
}
