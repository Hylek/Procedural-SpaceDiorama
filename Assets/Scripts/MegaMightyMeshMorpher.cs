using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class MegaMightyMeshMorpher : MonoBehaviour
{
    // GameObject to apply noise to
    public GameObject[] targets;

    // Storing the target's current vertices
    private Vector3[] objectVertices;

    // The targets *new* vertices after noise
    private Vector3[] alteredVertices;

    // Noise Parameters
    public float noiseRadius;
    public float noiseFrequency;
    public float noiseStrength;

    // Seed epoch timer
    System.DateTime seedEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    // LibNoise multifractal noise
    public LibNoise.Unity.Generator.RiggedMultifractal noise = new LibNoise.Unity.Generator.RiggedMultifractal();

    private void Start ()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            // Get current epoch time
            int epoch = (int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i;
            Debug.Log("SEED: " + epoch);

            objectVertices = targets[i].GetComponent<MeshFilter>().mesh.vertices;
            alteredVertices = targets[i].GetComponent<MeshFilter>().mesh.vertices;
            noise.Frequency = noiseFrequency;
            noise.Seed = epoch;

            for (int j = 0; j < objectVertices.Length; j++)
            {
                alteredVertices[j] = (objectVertices[j].normalized * (noiseRadius + (float)noise.GetValue(alteredVertices[j].normalized) * noiseStrength));
            }

            targets[i].GetComponent<MeshFilter>().mesh.vertices = alteredVertices;
            targets[i].GetComponent<MeshFilter>().mesh.RecalculateBounds();
            targets[i].GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
	
}
