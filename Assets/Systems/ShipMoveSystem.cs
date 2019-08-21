using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using System;
using Unity.Burst;
using UnityEngine.Jobs;

public class ShipMoveSystem : JobComponentSystem
{    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveJob job = new MoveJob
        {
            DeltaTime = Time.deltaTime
        };

        return job.Schedule(this,inputDeps);
    }

    public List<float3> MovePos = new List<float3>();

    [BurstCompile]
    struct MoveJob : IJobForEach<ShipComponent,Translation>
    {
        public float DeltaTime;
        
        public void Execute([ReadOnly] ref ShipComponent shipComponent, ref Translation shipTranslation)
        {
            var okDistance = shipComponent.Speed * 1.5f * (1f / 18f);
            
            var dir = shipComponent.Target - shipTranslation.Value;

            var newPos = (math.normalize(dir) * shipComponent.Speed * DeltaTime);

            //shipTranslation.Value += newPos;
        }
    }
}
