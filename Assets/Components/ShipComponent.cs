using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ShipComponent : IComponentData
{
    public float Speed;
    public float3 Target;
}
