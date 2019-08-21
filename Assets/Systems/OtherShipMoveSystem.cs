using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class OtherShipMoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref ShipComponent shipComponent, ref Translation translation) =>
        {
            var okDistance = shipComponent.Speed * 1.5f * (1f / 18f);

            var dir = shipComponent.Target - translation.Value;

            var newPos = (math.normalize(dir) * shipComponent.Speed * Time.deltaTime);

            //translation.Value += newPos;
        });
    }
}