using UnityEngine;

public class ColonySystemGenerator : MonoBehaviour
{
    private GameObject[] habitats;
    public Vector3[] habPositions;
    public GameObject moon;
    public Material defaultMat;
    public float spawnRadius = 10.0f;
    public int objectCount = 5;

	void Start ()
    {
        CreateHabitats();
    }
	
	void Update ()
    {
		
	}

    private void CreateHabitats()
    {
        int habitatAmount = Random.Range(2, 11);
        habitats = new GameObject[habitatAmount];
        habPositions = new Vector3[habitatAmount];
        for (int i = 0; i < habitatAmount; i++)
        {
            habitats[i] = CreateEntity("habitat module " + i, null, ((GameObject)Resources.Load("Dome")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
            habitats[i].transform.parent = moon.transform;

            GameObject go = habitats[Random.Range(0, habitats.Length)];

            Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
            Vector3 actualPos = new Vector3(spawnPos.x, 0.0f, spawnPos.y);
            Vector3 offset = transform.position + actualPos;

            RaycastHit hit;
            if (Physics.Raycast(offset, Vector3.down, out hit))
            {
                Vector3 finalPos = hit.point;
                Quaternion rot;
                rot = Quaternion.LookRotation(hit.normal);
                habitats[i].transform.position = finalPos;
                habPositions[i] = habitats[i].transform.position;
                habitats[i].transform.rotation = rot; 
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
