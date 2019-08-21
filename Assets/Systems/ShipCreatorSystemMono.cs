using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;

public class ShipCreatorSystemMono : MonoBehaviour
{
    [SerializeField] Mesh Mesh;
    [SerializeField] Material Material;

    protected void Start()
    {
        //EntityManager EntityManager = World.Active.EntityManager;

        //EntityArchetype ShipArchetype = EntityManager.CreateArchetype(
        //    typeof(Translation),
        //    typeof(RenderMesh),
        //    typeof(LocalToWorld),
        //    typeof(ShipComponent)
        //);

        //NativeArray<Entity> EntityArray = new NativeArray<Entity>(100, Allocator.Temp);
        //EntityManager.CreateEntity(ShipArchetype, EntityArray);

        //for (int i = 0; i < EntityArray.Length; i++)
        //{
        //    EntityManager.SetSharedComponentData(EntityArray[i], new RenderMesh
        //    {
        //        mesh = Mesh,
        //        material = Material
        //    });
        //    EntityManager.SetComponentData(EntityArray[i], new Translation
        //    {
        //        Value = new Unity.Mathematics.float3(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f), 0)
        //    });
        //}
    }
}
