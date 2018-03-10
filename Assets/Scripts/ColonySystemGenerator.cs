using UnityEngine;

public class ColonySystemGenerator : MonoBehaviour
{
    private GameObject[] habitats;
    private GameObject[] connectors;
    public Vector3[] habPositions;
    public Vector3[] conPositions;
    public GameObject moon;
    public Material defaultMat;
    public float spawnRadius = 1.0f;
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
            //Debug.DrawRay(new Vector3(habitats[objectCount].transform.position.x, habitats[objectCount].transform.position.y - 1, habitats[objectCount].transform.position.z), Vector3.down);
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

        Vector3 offset = transform.position;

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

        //habitats[objectCount].transform.position = connectors[objectCount - 1].transform.position;
        habitats[objectCount].transform.position = new Vector3(connectors[objectCount - 1].transform.position.x, connectors[objectCount - 1].transform.position.y, connectors[objectCount - 1].transform.position.z + 2/*Random.Range(1, 4)*/);
          
       Vector3 origin = new Vector3(habitats[objectCount].transform.position.x, habitats[objectCount].transform.position.y - 0.01f, habitats[objectCount].transform.position.z);
       RaycastHit hit;
       if (Physics.Raycast(origin, Vector3.down, out hit))
       {
           Vector3 finalPos = hit.point;
            finalPos.y = finalPos.y + 0.1f;
           Quaternion finalRot = Quaternion.LookRotation(hit.normal);
            habitats[objectCount].transform.position = finalPos;
            habitats[objectCount].transform.rotation = finalRot;
       }
    }

    private void CreateConnector()
    {
        connectors[objectCount] = CreateEntity("Connector " + objectCount, null, ((GameObject)Resources.Load("Cylinder")).GetComponent<MeshFilter>().sharedMesh, defaultMat);
        connectors[objectCount].transform.parent = moon.transform;

        // THIS IS ONLY FOR THE TEST CYLINDER
        connectors[objectCount].transform.localScale = new Vector3(0.005f, 0.02f, 0.005f);
        connectors[objectCount].transform.Rotate(new Vector3(90, 0, 0));
        connectors[objectCount].transform.position = SelectDirection();
        //connectors[objectCount].transform.localScale = new Vector3(habitats[objectCount - 1].transform.localScale.x, habitats[objectCount - 1].transform.localScale.y, habitats[objectCount - 1].transform.localScale.z);

        Vector3 origin = new Vector3(connectors[objectCount].transform.position.x, connectors[objectCount].transform.position.y - 0.01f, connectors[objectCount].transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit))
        {
            Vector3 finalPos = hit.point;
            Quaternion finalRot = Quaternion.LookRotation(hit.normal);
            connectors[objectCount].transform.position = finalPos;
            connectors[objectCount].transform.rotation = finalRot;
        }

    }

    private Vector3 SelectDirection()
    {
        Vector3 forward = new Vector3(habitats[objectCount - 1].transform.position.x, habitats[objectCount - 1].transform.position.y, habitats[objectCount - 1].transform.position.z + 2);
        Vector3 left = new Vector3(habitats[objectCount - 1].transform.position.x - 2, habitats[objectCount - 1].transform.position.y, habitats[objectCount - 1].transform.position.z);
        Vector3 right = new Vector3(habitats[objectCount - 1].transform.position.x + 2, habitats[objectCount - 1].transform.position.y, habitats[objectCount - 1].transform.position.z);
        Vector3 chosenDir = new Vector3(0,0,0);
        int random = Random.Range(1, 4);
        switch(random)
        {
            case 1:
                chosenDir = forward; break;
            case 2:
                chosenDir = left; break;
            case 3:
                chosenDir = right; break;
        }
        return chosenDir;
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
