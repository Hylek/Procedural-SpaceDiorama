using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class noisetest : MonoBehaviour {

    System.DateTime seedEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    // Use this for initialization
    void Start ()
    {
        GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds, 0.2f, 6, 0.1f, 3, 0.1f, 6, 0.3f, false);
    }

    private Texture2D GenerateAtmosphere(int epoch, float bFrequency, int bOctave, float bPersistence, float bLacunarity, float pFrequency, int pOctave, float pPersistence, bool useBothNoises)
    {
        int textureHeight = 256;
        int textureWidth = 512;
        Texture2D clouds = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
        Noise2D noise;

        // Create new LibNoise Perlin Noise to make the atmosphere and set values
        Perlin pNoise = new Perlin();
        pNoise.Frequency = pFrequency;
        pNoise.OctaveCount = pOctave;
        pNoise.Persistence = pPersistence;
        pNoise.Seed = epoch;

        Billow bNoise = new Billow();
        bNoise.Frequency = bFrequency;
        bNoise.OctaveCount = bOctave;
        bNoise.Persistence = bPersistence;
        bNoise.Lacunarity = bLacunarity;
        bNoise.Quality = LibNoise.Unity.QualityMode.High;
        bNoise.Seed = epoch;


        RiggedMultifractal rNoise = new RiggedMultifractal();
        rNoise.Seed = epoch;
        rNoise.OctaveCount = bOctave;
        rNoise.Frequency = bFrequency;

        ModuleBase noiseModule;
        if (useBothNoises)
        {
            noiseModule = new Add(bNoise, pNoise);
        }
        else
        {
            noiseModule = bNoise;
        }

        noise = new Noise2D(textureWidth, textureHeight, noiseModule);
        noise.GenerateSpherical(-90.0, 90.0, -180.0, 180.0);
        clouds = noise.GetTexture(LibNoise.Unity.Gradient.Grayscale);

        // Perform colour change
        Color[] pixels = clouds.GetPixels(0, 0, clouds.width, clouds.height, 0);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(pixels[i].r, pixels[i].g, pixels[i].b, pixels[i].grayscale);
        }
        clouds.SetPixels(0, 0, clouds.width, clouds.height, pixels, 0);
        clouds.Apply();

        return clouds;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
