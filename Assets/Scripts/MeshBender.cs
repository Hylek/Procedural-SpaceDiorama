using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class MeshBender : MonoBehaviour {

    private Mesh gameMesh;
    Vector3[] vertices;
    Vector3[] verticesN;
    public float radius = 1f;
    public LibNoise.Unity.Generator.RiggedMultifractal noise = new LibNoise.Unity.Generator.RiggedMultifractal();
    public float frequency = 10f;
    public float noisemod = 1f;
    private MeshCollider meshCollider;
    public GameObject prefab;
    System.DateTime seedTimer = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    public GameObject[] roids;

    void Start ()
    {
        int epochTime = (int)(System.DateTime.UtcNow - seedTimer).TotalSeconds;
        Debug.Log(epochTime);

        //for(int i = 0; i < 100; i++)
        //{
        //    Instantiate(roids[i], transform.position * 2, transform.rotation);
        //}

        gameMesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        Vector3[] vertices = gameMesh.vertices;
        Vector3[] verticesN = gameMesh.vertices;
        noise.Frequency = frequency;
        noise.Seed = epochTime;

        for (int i = 0; i < vertices.Length; i++)
        {
            verticesN[i] = (vertices[i].normalized * (radius + (float)noise.GetValue(verticesN[i].normalized) * noisemod));
        }
        gameMesh.vertices = verticesN;
        gameMesh.RecalculateNormals();
        gameMesh.RecalculateBounds();
        meshCollider.sharedMesh = gameMesh;
    }
}
