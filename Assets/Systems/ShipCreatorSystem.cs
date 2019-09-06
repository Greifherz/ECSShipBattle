using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;

public class ShipCreatorSystem : ComponentSystem
{
    private const float QUADMESHSIZE = 0.35f;

    private Mesh Mesh;
    private Material Material;

    public EntityArchetype ShipArchetype;
    private NativeArray<Entity> Ships;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Ships.Dispose();
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        LoadResources();

        EntityManager EntityManager = World.Active.EntityManager;

        ShipArchetype = EntityManager.CreateArchetype(
            typeof(ShipComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld)
        );

        Ships = new NativeArray<Entity>(1000, Allocator.Persistent);
        EntityManager.CreateEntity(ShipArchetype, Ships);
        
        SetComponentData(EntityManager,Ships);
        
    }

    public NativeList<Translation> GetShipTranslations(Allocator allocator)
    {
        NativeList<Translation> translations = new NativeList<Translation>(allocator);
        for (int i = 0; i < Ships.Length; i++)
        {
            translations.Add(EntityManager.GetComponentData<Translation>(Ships[i]));
        }
        return translations;
    }

    private void SetComponentData(EntityManager EntityManager,NativeArray<Entity> Ships)
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            EntityManager.SetSharedComponentData(Ships[i], new RenderMesh
            {
                mesh = Mesh,
                material = Material
            });

            EntityManager.SetComponentData(Ships[i], new Translation
            {
                Value = new Unity.Mathematics.float3(Random.Range(-5f, 5f), Random.Range(-4.5f, 4.5f), 0)
            });

            EntityManager.SetComponentData(Ships[i], new ShipComponent
            {
                Speed = 2f,
            });
        }
    }    

    protected override void OnUpdate()
    {
        //Do nothing
    }

    private void LoadResources()
    {        
        Material = Resources.Load<Material>("ShipMaterial");
        Mesh = new Mesh();

        var vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(QUADMESHSIZE, 0, 0),
            new Vector3(0, QUADMESHSIZE, 0),
            new Vector3(QUADMESHSIZE, QUADMESHSIZE, 0)
        };
        Mesh.vertices = vertices;

        var tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        Mesh.triangles = tris;

        var normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        Mesh.normals = normals;

        var uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        Mesh.uv = uv;
    }

}
