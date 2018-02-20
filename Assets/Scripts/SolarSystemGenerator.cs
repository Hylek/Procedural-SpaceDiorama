using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public Material starMaterial;
    public Material defaultMat;
    private Color starColor;
    private GameObject star;
    private GameObject[] planets;
    private GameObject[] moons;
    private int moonColony = -1;

	private void Start ()
    {
        GenerateStar();
        GeneratePlanets();
        GenerateMoons();
	}

	private void Update ()
    {
		if(moonColony == -1)
        {
            moonColony = Random.Range(0, moons.Length);
            Debug.Log("Moon " + moonColony + " Will have the colony!");
        }
	}

    private void GeneratePlanets()
    {
        int planetAmount = Random.Range(5, 11);
        planets = new GameObject[planetAmount];
        for (int i = 0; i < planetAmount; i++)
        {
            float scaleAmount = Random.Range(500.0f, 10000.0f);
            planets[i] = CreateEntity("planet " + i, null, ((GameObject)Resources.Load("LessNuttySphere")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
            planets[i].AddComponent<SphereCollider>();
            planets[i].AddComponent<Orbit>();
            planets[i].GetComponent<Orbit>().target = star;
            planets[i].GetComponent<Orbit>().orbitAngle = new Vector3(Random.Range(0, 45), Random.Range(0, 45), Random.Range(0, 45));
            planets[i].transform.position = new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(-100.0f, 100.0f), Random.Range(-1000.0f, 1000.0f));
            planets[i].transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);

            float distance = Vector3.Distance(planets[i].transform.position, star.transform.position);
            Debug.Log("planet " + i + " distance " + distance);
            float orbitSpeed = (1 / distance) * 5000;
            planets[i].GetComponent<Orbit>().orbitSpeed = orbitSpeed;

            if (!Physics.CheckSphere(planets[i].transform.position, planets[i].transform.localScale.x * 1.2f))
            {
                
            }
        }
    }

    private void GenerateMoons()
    {
        int moonAmount = Random.Range(2, 11);
        moons = new GameObject[moonAmount];
        for(int i = 0; i < moonAmount; i++)
        {
            GameObject planetTarget = planets[Random.Range(0, planets.Length)];
            float orbitSpeed = Random.Range(planetTarget.GetComponent<Orbit>().orbitSpeed * 1.2f, planetTarget.GetComponent<Orbit>().orbitSpeed * 4f);
            float scaleAmount = Random.Range(0.05f, 0.5f);
            moons[i] = CreateEntity("moon " + i, null, ((GameObject)Resources.Load("LessNuttySphere")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
            moons[i].transform.parent = planetTarget.transform;
            moons[i].AddComponent<Orbit>();
            moons[i].GetComponent<Orbit>().target = planetTarget;
            moons[i].GetComponent<Orbit>().orbitSpeed = orbitSpeed;
            moons[i].transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
            moons[i].transform.position = planetTarget.transform.position * Random.Range(1.1f, 1.3f);
            moons[i].GetComponent<Orbit>().orbitAngle = new Vector3(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
        }
    }

    private void GenerateStar()
    {
        float scaleAmount = Random.Range(1000.0f, 5000.0f);
        float starStrength = Random.Range(0.1f, 10.0f);

        starMaterial.color = new Color32((byte)Random.Range(50, 255), (byte)Random.Range(50, 255), (byte)Random.Range(50, 255), 255);
        starMaterial.EnableKeyword("_EMISSION");
        starMaterial.SetColor("_EmissionColor", starMaterial.color);

        Vector3 scale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        star = CreateEntity("star", null, ((GameObject)Resources.Load("LessNuttySphere")).GetComponent<MeshFilter>().sharedMesh, starMaterial);
        star.transform.localScale = scale;
        star.AddComponent<Light>();
        star.GetComponent<Light>().type = LightType.Point;
        star.GetComponent<Light>().range = scaleAmount * starStrength;
        star.GetComponent<Light>().color = starMaterial.color;
    }

    private void GenerationCheck()
    {
        for(int i = 0; i < planets.Length; i++)
        {
            for(int j = 0; j < planets.Length; j++)
            {
                // Approximate the angles of each planet against each other to determine whether they will collide
                // If so, move them out of the way before simulation begins.
            }
        }
    }

    private GameObject CreateEntity(string name, Component collider, Mesh mesh, Material material)
    {
        GameObject entity = new GameObject(name);
        entity.AddComponent<MeshRenderer>();
        entity.AddComponent<MeshFilter>();
        entity.GetComponent<MeshFilter>().mesh = mesh;
        entity.GetComponent<MeshRenderer>().material = material;

        return entity;
    }
}
