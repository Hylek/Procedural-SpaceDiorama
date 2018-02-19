using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public Material starMaterial;
    public Material defaultMat;
    private Color starColor;
    private GameObject star;
    private GameObject[] planets;

	private void Start ()
    {
        GenerateStar();
        GeneratePlanets();
	}

	private void Update ()
    {
		
	}

    private void GeneratePlanets()
    {
        int planetAmount = Random.Range(10, 20);
        planets = new GameObject[planetAmount];
        for (int i = 0; i < planetAmount; i++)
        {
            float orbitSpeed = Random.Range(1.0f, 50.0f);
            float scaleAmount = Random.Range(500.0f, 10000.0f);
            planets[i] = CreateEntity("planet " + i, null, ((GameObject)Resources.Load("LessNuttySphere")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
            planets[i].AddComponent<SphereCollider>();
            planets[i].AddComponent<Orbit>();
            planets[i].GetComponent<Orbit>().target = star;
            planets[i].GetComponent<Orbit>().orbitSpeed = orbitSpeed;
            planets[i].GetComponent<Orbit>().orbitAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            planets[i].transform.position = new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f));
            planets[i].transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);

            if (!Physics.CheckSphere(planets[i].transform.position, planets[i].transform.localScale.x * 1.2f))
            {
                
            }
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
