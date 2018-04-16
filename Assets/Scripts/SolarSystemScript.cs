﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class SolarSystemScript : MonoBehaviour
{

    public GameObject galaxyPrefab;

    public GameObject[] galaxyStars;
    public GameObject spherePrefab;

    public GameObject star;
    public GameObject planetPrefab;
    public GameObject[] planets;
    public Material starMaterial;
    public int planetCount;

    public Noise2D atmosphere;
    System.DateTime seedEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);


    void Start ()
    {
        CreateStar();
        CreatePlanets();
        ModifyPlanets();
        GenerateDistantGalaxies();

    }

    private void CreateStar()
    {
        float scaleAmount = Random.Range(4.0f, 20.0f);
        float starStrength = Random.Range(0.1f, 10.0f);

        // If the star is small, we use a cooler colour set. If the star is large, we use hotter colours like with red giants
        // The planets size also affects how many planets it supports
        if (scaleAmount < 10.0f)
        {
            Debug.Log("Small Star!");
            starMaterial.color = new Color32((byte)Random.Range(10, 150), (byte)Random.Range(150, 255), (byte)Random.Range(150, 255), 255);
            starMaterial.EnableKeyword("_EMISSION");
            starMaterial.SetColor("_EmissionColor", starMaterial.color);
            planetCount = Random.Range(3, 9);
        } else if (scaleAmount > 10.0f)
        {
            Debug.Log("Big Star!");
            starMaterial.color = new Color32((byte)Random.Range(150, 255), (byte)Random.Range(50, 200), (byte)Random.Range(25, 100), 255);
            starMaterial.EnableKeyword("_EMISSION");
            starMaterial.SetColor("_EmissionColor", starMaterial.color);
            planetCount = Random.Range(5, 16);
        }
        planets = new GameObject[planetCount];

        Vector3 scale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        star = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
        star.GetComponent<Renderer>().material = starMaterial;
        star.name = "star";
        star.transform.localScale = scale;
        star.AddComponent<Light>();
        star.GetComponent<Light>().type = LightType.Point;
        star.GetComponent<Light>().range = scaleAmount * starStrength * 2;
        star.GetComponent<Light>().color = starMaterial.color;
    }

    private void CreatePlanets()
    {
        Vector3[] positions = new Vector3[planetCount];
        float minDistance = 10.0f;
        float maxDistance = 25.0f;
    
        for (int i = 0; i < planetCount; i++)
        {
            // Use a random angle and distance from star to place the planets in the solar system
            float angle = Random.Range(0, 361.0f);
            float distance = Random.Range(minDistance, maxDistance);

            // Get the position
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

            positions[i] = new Vector3(x, 0, z);
            planets[i] = Instantiate(planetPrefab, positions[i], Quaternion.identity);
            planets[i].GetComponent<Orbit>().target = star;
            planets[i].name = "Planet " + i;

            minDistance += Random.Range(5.0f, 10.0f);
            maxDistance += Random.Range(5.0f, 10.0f);

            // Use epochtime to generate a seed, meaning each one is guarenteed to be different, rather than Random.range
            planets[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i);
            planets[i].GetComponent<Renderer>().material.mainTexture = GenerateTemperateTerrain((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i);
        }
    }

    private void ModifyPlanets()
    {
        float distance;
        for (int i = 0; i < planetCount; i++)
        {
            distance = Vector3.Distance(planets[i].transform.position, star.transform.position);
            planets[i].GetComponent<Orbit>().orbitSpeed = (1f / distance) * 100.0f;
        }
    }

    private void GenerateAsteroidRing()
    {
        int starCount = Random.Range(80, 201);
        galaxyStars = new GameObject[starCount];
        for (int i = 0; i < starCount; i++)
        {
            // Use a random angle and distance from star to place the planets in the solar system
            float angle = Random.Range(0, 361.0f);
            float distance = Random.Range(10.0f, 21.0f);

            // Get the position
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

            Vector3 finalPosition = transform.position + new Vector3(x, 0, z);

            Instantiate(spherePrefab, finalPosition, Quaternion.identity, transform);
        }
    }

    private void GenerateDistantGalaxies()
    {
        GameObject galaxy;
        for (int i = 0; i < 10; i++)
        {
            Vector3 position = Random.onUnitSphere * (GetComponent<SphereCollider>().radius) + Vector3.zero;
            galaxy = Instantiate(galaxyPrefab, position, Quaternion.identity);
            galaxy.transform.eulerAngles = new Vector3(Random.Range(0, 361), Random.Range(0, 361), Random.Range(0, 361));
        }
    }

    private void PlanetMeshMorpher()
    {

    }

    private Texture2D GenerateAtmosphere(int epoch)
    {
        int textureHeight = 256;
        int textureWidth = 512;
        Texture2D clouds = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);

        // Create new LibNoise Perlin Noise to make the atmosphere and set values
        Perlin pNoise = new Perlin();
        pNoise.Frequency = 2;
        pNoise.OctaveCount = 2;
        pNoise.Persistence = 0.3f;
        pNoise.Seed = epoch;

        Billow bNoise = new Billow();
        bNoise.Frequency = 3;
        bNoise.OctaveCount = 6;
        bNoise.Persistence = 0.3f;
        bNoise.Seed = epoch;

        ModuleBase noiseModule = new Add(bNoise, pNoise);
        atmosphere = new Noise2D(textureWidth, textureHeight, noiseModule);
        atmosphere.GenerateSpherical(-90.0, 90.0, -180.0, 180.0);
        clouds = atmosphere.GetTexture(LibNoise.Unity.Gradient.Grayscale);

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

    private Texture2D GenerateTerrain(int epoch/*, Color colorA, Color colorB*/)
    {
        int textureHeight = 256;
        int textureWidth = 512;
        Texture2D terrain = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);

        // Create new LibNoise Perlin Noise to make the atmosphere and set values
        Perlin pNoise = new Perlin();
        pNoise.Frequency = 2;
        pNoise.OctaveCount = 2;
        pNoise.Lacunarity = 2;
        pNoise.Seed = epoch;

        //Billow bNoise = new Billow();
        //bNoise.Frequency = 3;
        //bNoise.OctaveCount = 6;
        //bNoise.Persistence = 0.3f;
        //bNoise.Seed = epoch;

        ModuleBase noiseModule = pNoise; //new Add(bNoise, pNoise);
        atmosphere = new Noise2D(textureWidth, textureHeight, noiseModule);
        atmosphere.GenerateSpherical(-90.0, 90.0, -180.0, 180.0);
        terrain = atmosphere.GetTexture(LibNoise.Unity.Gradient.Grayscale);

        // Perform colour change
        Color[] pixels = terrain.GetPixels(0, 0, terrain.width, terrain.height, 0);
        Color targetOcean = new Color(Random.Range(0.2f, 0.45f), Random.Range(0.2f, 0.45f), Random.Range(0.2f, 0.45f));
        for (int i = 0; i < pixels.Length; i++)
        {
            if(pixels[i].grayscale > targetOcean.grayscale)
            {
                pixels[i] = new Color(0, 0.25f, 0.55f, 1);
            }
            else
            {
                pixels[i] = new Color(0, 0.85f, 0.1f, 1);
            }
            //pixels[i] = new Color(pixels[i].r, pixels[i].g, pixels[i].b, pixels[i].grayscale);
        }
        terrain.SetPixels(0, 0, terrain.width, terrain.height, pixels, 0);
        terrain.Apply();

        return terrain;
    }
}
