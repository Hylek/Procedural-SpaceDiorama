using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class SolarSystemScript : MonoBehaviour
{

    public GameObject galaxyPrefab;
    public GameObject planePrefab;
    public Material astertoidMaterial;

    public GameObject[] galaxyStars;
    public GameObject[] asteroids;
    public GameObject spherePrefab;

    public GameObject star;
    public GameObject planetPrefab;
    public GameObject[] planets;
    public Material starMaterial;
    public int planetCount;
    private bool starterPlanet = false;

    System.DateTime seedEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    int terra = 0;
    public int temperatePlanetCount = 0;


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
            planetCount = Random.Range(6, 9);
        } else if (scaleAmount > 10.0f)
        {
            Debug.Log("Big Star!");
            starMaterial.color = new Color32((byte)Random.Range(150, 255), (byte)Random.Range(50, 200), (byte)Random.Range(25, 100), 255);
            starMaterial.EnableKeyword("_EMISSION");
            starMaterial.SetColor("_EmissionColor", starMaterial.color);
            planetCount = Random.Range(10, 16);
        }
        planets = new GameObject[planetCount];

        Vector3 scale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        star = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
        star.GetComponent<Renderer>().material = starMaterial;
        star.name = "star";
        star.transform.localScale = scale;

        // Create a light for the star to project light around the diorama
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

            minDistance += Random.Range(5.0f, 25.0f);
            maxDistance += Random.Range(10.0f, 35.0f);
        }
    }

    private void ModifyPlanets()
    {
        float distance;
        for (int i = 0; i < planetCount; i++)
        {
            distance = Vector3.Distance(planets[i].transform.position, star.transform.position);
            planets[i].GetComponent<Orbit>().orbitSpeed = (1f / distance) * 10.0f;

            Debug.Log("Planet " + i + " Distance from star " + distance);

            // Closest planets
            if(distance < 50) 
            {
                // Planets closest should be smaller
                float newScale = Random.Range(0.5f, 2.5f);
                planets[i].transform.localScale = new Vector3(newScale, newScale, newScale);

                // Planets closest should have little to no atmosphere and more barren colours
                int chance = Random.Range(1, 101);
                if (chance > 10.0f)
                {
                    Destroy(planets[i].transform.GetChild(0).gameObject);

                    planets[i].GetComponent<Renderer>().material.mainTexture = GenerateBarrenSurface((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.4f, 0.8f), Random.Range(1.5f, 3.6f), Random.Range(6, 7));
                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    planets[i].name = "Barren " + i;
                }
                else if (chance < 10.0f)
                {
                    // Very small chance a planet this close to the star may be a temperate planet
                    planets[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(1, 3), Random.Range(2, 5), Random.Range(0.1f, 0.4f), 1, 1, 1, true, false);
                    planets[i].GetComponent<Renderer>().material.mainTexture = GenerateTerrain((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.2f, 1.5f), Random.Range(5, 7), Random.Range(2.5f, 5.6f), Random.Range(0.1f, 0.25f));

                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    planets[i].name = "Temperate";
                    temperatePlanetCount++;
                }
            }

            // Golden zone planets
            if (distance > 50 && distance <= 150)
            {
                int chance = Random.Range(1, 101);
                if(chance > 15.0f)
                {
                    if(!starterPlanet)
                    {
                        planets[i].AddComponent<TemperateController>();
                        starterPlanet = true;
                    }

                    float newScale = Random.Range(0.5f, 2.5f);
                    planets[i].transform.localScale = new Vector3(newScale, newScale, newScale);

                    planets[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, 3, 6, 0.3f, 1, 1, 1, true, false);
                    planets[i].GetComponent<Renderer>().material.mainTexture = GenerateTerrain((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.2f, 1.5f), Random.Range(5, 7), Random.Range(2.5f, 5.6f), Random.Range(0.1f, 0.25f));
                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    planets[i].name = "Temperate";
                    temperatePlanetCount++;

                    if (terra == 0 && chance > 30.0f)
                    {
                        planets[i].name = "Terra";
                        terra = 1;
                    }
                }
                else // a Gas giant
                {
                    float newScale = Random.Range(1.5f, 4.5f);
                    planets[i].transform.localScale = new Vector3(newScale, newScale, newScale);

                    // Gas giants should be generally larger with no terrain texture and a thicc atmosphere of a random colour
                    planets[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.25f, 1), Random.Range(1, 2), Random.Range(0.25f, 0.3f), 0, 0, 0, false, true);
                    planets[i].name = "Gas " + i;
                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    int ringChance = Random.Range(1, 101);

                    if (ringChance > 50.0f)
                    {
                        GenerateAsteroidRing(planets[i].transform.position, planets[i]);
                    }
                }

            }
            // Planets furthest are always to be Gas giants or barren planets
            if (distance > 150)
            {
                int chance = Random.Range(1, 101);
          
                // 60% chance it will be gas
                if(chance > 40.0f)
                {
                    float newScale = Random.Range(4.5f, 10.5f);
                    planets[i].transform.localScale = new Vector3(newScale, newScale, newScale);
                    planets[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GenerateAtmosphere((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.25f, 1), Random.Range(1, 2), Random.Range(0.25f, 0.3f), 0, 0, 0, false, true);
                    planets[i].name = "Gas " + i;
                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    int ringChance = Random.Range(1, 101);

                    if(ringChance > 50.0f)
                    {
                        GenerateAsteroidRing(planets[i].transform.position, planets[i]);
                    }
                }
                // 40% chance it will be barren
                else if (chance < 40.0f)
                {
                    Destroy(planets[i].transform.GetChild(0).gameObject);
                    planets[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                    planets[i].GetComponent<Renderer>().material.mainTexture = GenerateBarrenSurface((int)(System.DateTime.UtcNow - seedEpoch).TotalSeconds + i, Random.Range(0.4f, 0.8f), Random.Range(1.5f, 3.6f), Random.Range(6, 7));
                    planets[i].name = "Barren " + i;
                }

            }
        }
    }

    private void GenerateAsteroidRing(Vector3 position, GameObject parent)
    {
        int asteroidCount = Random.Range(40, 101);
        asteroids = new GameObject[asteroidCount];
        for (int i = 0; i < asteroidCount; i++)
        {
            // Use a random angle and distance from star to place the planets in the solar system
            float angle = Random.Range(0, 361.0f);
            float distance = Random.Range(5.0f, 14.0f);

            // Get the position
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

            Vector3 finalPosition = position + new Vector3(x, 0, z);

            GameObject temp;
            temp = Instantiate(spherePrefab, finalPosition, Quaternion.identity, parent.transform);
            temp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            temp.GetComponent<Renderer>().material = astertoidMaterial;
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

    private Texture2D GenerateBarrenSurface(int epoch, float frequency, float lacunarity, int octave)
    {
        int textureHeight = 256;
        int textureWidth = 512;
        Texture2D barren = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);

        // Feed in noise values
        RiggedMultifractal rm = new RiggedMultifractal();
        rm.Frequency = frequency;
        rm.Lacunarity = lacunarity;
        rm.OctaveCount = octave;
        rm.Seed = epoch;

        ModuleBase noiseModule = rm;
        Noise2D noise = new Noise2D(textureWidth, textureHeight, noiseModule);
        noise.GenerateSpherical(-90.0, 90.0, -180.0, 180.0);
        barren = noise.GetTexture(LibNoise.Unity.Gradient.Grayscale);

        Color[] pixels = barren.GetPixels(0, 0, barren.width, barren.height, 0);
        float colourAStrength = Random.Range(0.2f, 1.6f);
        float colourBStrength = Random.Range(1.1f, 2.1f);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(new Color(0.14f, 0.12f, 0.11f) * colourAStrength, new Color(0.32f, 0.23f, 0.05f) * colourBStrength, Mathf.Round(pixels[i].grayscale * 1000f) / 1000f);
        }
        barren.SetPixels(0, 0, barren.width, barren.height, pixels, 0);
        barren.Apply();

        return barren;
    }

    private Texture2D GenerateAtmosphere(int epoch, float bFrequency, int bOctave, float bPersistence, float pFrequency, int pOctave, float pPersistence, bool useBothNoises, bool gasGiant)
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
        bNoise.Seed = epoch;

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

        Color colourA = new Color(Random.Range(0, 0.35f), Random.Range(0.2f, 0.8f), Random.Range(0.3f, 1.1f));
        Color colourB = new Color(Random.Range(0.1f, 0.45f), Random.Range(0.1f, 0.6f), Random.Range(0.2f, 0.7f));
        for (int i = 0; i < pixels.Length; i++)
        {
            if(gasGiant)
            {
                pixels[i] = Color.Lerp(colourA, colourB * 2, Mathf.Round(pixels[i].grayscale * 1000f) / 1000f);
            }
            else
            {
                pixels[i] = new Color(pixels[i].r, pixels[i].g, pixels[i].b, pixels[i].grayscale);
            }
        }
        clouds.SetPixels(0, 0, clouds.width, clouds.height, pixels, 0);
        clouds.Apply();

        return clouds;
    }

    private Texture2D GenerateTerrain(int epoch, float frequency, int octaves, float lacunarity, float persistence)
    {
        int textureHeight = 256;
        int textureWidth = 512;
        Texture2D terrain = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
        Noise2D noise;

        // Create new LibNoise Perlin Noise to make the atmosphere and set values
        Perlin pNoise = new Perlin();
        pNoise.Frequency = frequency;
        pNoise.OctaveCount = octaves;
        pNoise.Lacunarity = lacunarity;
        pNoise.Persistence = persistence;
        pNoise.Seed = epoch;

        ModuleBase noiseModule = pNoise;
        noise = new Noise2D(textureWidth, textureHeight, noiseModule);
        noise.GenerateSpherical(-90.0, 90.0, -180.0, 180.0);
        terrain = noise.GetTexture(LibNoise.Unity.Gradient.Grayscale);

        // Perform colour change
        Color[] pixels = terrain.GetPixels(0, 0, terrain.width, terrain.height, 0);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(Color.green * 1.2f, Color.blue, Mathf.Round(pixels[i].grayscale * 1000f) / 1000f);
        }
        terrain.SetPixels(0, 0, terrain.width, terrain.height, pixels, 0);
        terrain.Apply();

        return terrain;
    }

}
