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
    private float okDistance;

    protected override void OnCreate()
    {

    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        ShipCreatorSystem = EntityManager.World.GetExistingSystem<ShipCreatorSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var otherShips = ShipCreatorSystem.GetShipTranslations(Allocator.TempJob);

        var job = new TargettingJob()
        {
            OtherShips = otherShips,
            DeltaTime = Time.deltaTime
        };

        var handle = job.Schedule(this);

        handle.Complete();

        otherShips.Dispose();        

        return handle;
    }

    [BurstCompile]
    struct TargettingJob : IJobForEach<ShipComponent, Translation>
    {
        [ReadOnlyAttribute] public NativeList<Translation> OtherShips;
        [ReadOnlyAttribute] public float DeltaTime;

        public void Execute(ref ShipComponent shipC, [ReadOnly] ref Translation shipTranslation)
        {
            var okDistance = shipC.Speed * 1.5f * (1f / 18f);
            var closestDistance = float.MaxValue;

            float3 target = shipTranslation.Value;

            for (int j = 0; j < OtherShips.Length; j++)
            {
                var otherShipTranslation = OtherShips[j];
                var tripleBool = shipTranslation.Value == otherShipTranslation.Value;
                if (tripleBool.x && tripleBool.y && tripleBool.z) continue;

                var distance = math.distance(shipTranslation.Value, otherShipTranslation.Value);


                if (distance < closestDistance && distance > okDistance)
                {
                    closestDistance = distance;
                    target = otherShipTranslation.Value;
                }
            }

            var dir = target - shipTranslation.Value;// + new float3(0.1f, 0, 0);

            var toSelf = target == shipTranslation.Value;
            if (toSelf.x && toSelf.y && toSelf.z) return;

            var newPos = (math.normalize(dir) * shipC.Speed * DeltaTime);

            shipTranslation.Value += newPos;

        }
    }
}
