using UnityEngine;

public class ColonySystemGenerator : MonoBehaviour
{
    private GameObject[] habitats;
    private GameObject[] connectors;
    public Vector3[] habPositions;
    public Vector3[] conPositions;
    public GameObject moon;
    public Material defaultMat;
    public float spawnRadius = 10.0f;
    public int objectCount;
    private int colonySize;

	void Start ()
    {
        colonySize = Random.Range(2, 20);
        Init();
        objectCount = 0;
        CreateColony();
    }
	
	void Update ()
    {
		
	}

    //private void CreateHabitat()
    //{
    //    int habitatAmount = Random.Range(2, 11);
    //    habitats = new GameObject[habitatAmount];
    //    habPositions = new Vector3[habitatAmount];
    //    for (int i = 0; i < habitatAmount; i++)
    //    {
    //        habitats[i] = CreateEntity("habitat module " + i, null, ((GameObject)Resources.Load("Dome")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
    //        habitats[i].transform.parent = moon.transform;

    //        GameObject go = habitats[Random.Range(0, habitats.Length)];

    //        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
    //        Vector3 actualPos = new Vector3(spawnPos.x, 0.0f, spawnPos.y);
    //        Vector3 offset = transform.position + actualPos;

    //        RaycastHit hit;
    //        if (Physics.Raycast(offset, Vector3.down, out hit))
    //        {
    //            Vector3 finalPos = hit.point;
    //            Quaternion rot;
    //            rot = Quaternion.LookRotation(hit.normal);
    //            habitats[i].transform.position = finalPos;
    //            habPositions[i] = habitats[i].transform.position;
    //            habitats[i].transform.rotation = rot;
    //        }
    //    }
    //}

    private void Init()
    {
        habitats = new GameObject[colonySize];
        habPositions = new Vector3[colonySize];
        connectors = new GameObject[colonySize];
        conPositions = new Vector3[colonySize];
    }

    private void CreateColony()
    {
        // Create the first habitat module
        if(objectCount == 0)
        {
            CreateHQ();
            objectCount++;
        }

        for(objectCount = objectCount; objectCount < colonySize; objectCount++)
        {
            if (IsNumberOdd() && objectCount > 0)
            {
                CreateConnector();
            }
            else if (!IsNumberOdd() && objectCount > 0)
            {
                CreateHabitat();
            }
        }
    }

    private bool IsNumberOdd()
    {
        if((objectCount & 1) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CreateHQ()
    {
        habitats[objectCount] = CreateEntity("HeadQuarters", null, ((GameObject)Resources.Load("Cube")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
        habitats[objectCount].transform.parent = moon.transform;

        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
        Vector3 actualPos = new Vector3(spawnPos.x, 0.0f, spawnPos.y);
        Vector3 offset = transform.position + actualPos;

        RaycastHit hit;
        if (Physics.Raycast(offset, Vector3.down, out hit))
        {
            Vector3 finalPos = hit.point;
            finalPos.y = finalPos.y + 0.4f;
            Quaternion rot;
            rot = Quaternion.LookRotation(hit.normal);
            habitats[objectCount].transform.position = finalPos;
            habPositions[objectCount] = habitats[objectCount].transform.position;
            habitats[objectCount].transform.rotation = rot;
        }
    }

    private void CreateHabitat()
    {
        habitats[objectCount] = CreateEntity("Habitat Module " + objectCount, null, ((GameObject)Resources.Load("Cube")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
        habitats[objectCount].transform.parent = moon.transform;

        habitats[objectCount].transform.position = connectors[objectCount - 1].transform.position;

        
    }

    private void CreateConnector()
    {
        connectors[objectCount] = CreateEntity("Connector " + objectCount, null, ((GameObject)Resources.Load("Cylinder")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
        connectors[objectCount].transform.parent = moon.transform;

        // THIS IS ONLY FOR THE TEST CYLINDER
        connectors[objectCount].transform.Rotate(new Vector3(90, 0, 0));
        connectors[objectCount].transform.position = new Vector3(habitats[objectCount - 1].transform.position.x, habitats[objectCount - 1].transform.position.y, habitats[objectCount - 1].transform.position.z + 2);
        //connectors[objectCount].transform.localScale = new Vector3(habitats[objectCount - 1].transform.localScale.x, habitats[objectCount - 1].transform.localScale.y, habitats[objectCount - 1].transform.localScale.z);

        //Vector3 origin = new Vector3(connectors[objectCount].transform.position.x - 1, connectors[objectCount].transform.position.y, connectors[objectCount].transform.position.z);
        //RaycastHit hit;
        //if (Physics.Raycast(origin, Vector3.left, out hit))
        //{
        //    Vector3 finalPos = hit.point;
        //    Quaternion finalRot = Quaternion.LookRotation(hit.normal);
        //    connectors[objectCount].transform.position = finalPos;
        //    connectors[objectCount].transform.rotation = finalRot;
        //}

    }
   

    private void CreateLaunchpad()
    {

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
