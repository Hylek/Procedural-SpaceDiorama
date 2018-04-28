using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetBuilder : MonoBehaviour
{
    public GameObject fighterBase;
    public GameObject colonyBase;
    public GameObject fleetController;

    // Fleet arrays
    public GameObject[] shipArray;
    public List<GameObject> fleets;

    public Mesh[] fighterParts;
    public Mesh[] colonyShipParts;

    public GameObject[] colonyShipsTest;

    public Material fighterMaterial;
    public int fleetIndex = -1; 

    private void Start()
    {
        fleetIndex = -1;
        colonyShipsTest = new GameObject[1];
    }

    public void SpawnFleet(Vector3 position)
    {
        int shipCount = Random.Range(10, 21);
        shipArray = new GameObject[shipCount];
        fleets.Add(Instantiate(fleetController, position + new Vector3(10, 0, 10), transform.rotation));
        fleetIndex++;
        fleets[fleetIndex].name = "FleetController";

        for (int i = 0; i < shipCount; i++)
        {
            Vector3 spawnPoint = fleets[fleetIndex].transform.position + Random.insideUnitSphere * fleets[fleetIndex].GetComponent<SphereCollider>().radius;
            if (IsNumberOdd(i))
            {
                CreateFighter(1, spawnPoint, shipArray);
            }
            else if(!IsNumberOdd(i))
            {
                CreateColonyShip(1, spawnPoint, shipArray);
            }
        }
    }

    public void CreateFighter(int fighterCount, Vector3 spawnPosition, GameObject[] shipArray)
    {
        for(int i = 0; i < fighterCount; i++)
        {
            // Should this fighter have a tail? 0 for null, 1 for yes, and 2 for no.
            int tailChance = 0;
            // What engine should this fighter use? 0 for default, 1 for standard, 2 for large
            int engineType = 0;
            // How many weapon should this fighter have? Between 0 and 5
            int weaponCount = 0;
            // Undercarrige weapon
            int underGun = 0;

            // Create fighter base
            shipArray[i] = Instantiate(fighterBase, spawnPosition, transform.rotation, fleets[fleetIndex].transform);
            shipArray[i].AddComponent<ShipBehaviour>();
            shipArray[i].AddComponent<RotateCorrector>();

            //shipArray[i].AddComponent<ShipScript>();
            shipArray[i].name = "Fighter " + i;

            // Decide tail, engine and weapon count
            tailChance = Random.Range(0, 101);
            engineType = Random.Range(0, 101);
            weaponCount = Random.Range(0, 8);

            // Assign the tail to the correct node, if we have a tail it also means we can support a large undercarried gun (Node gun5) 40% chance
            if(tailChance > 60.0f)
            {
                shipArray[i].transform.GetChild(5).GetComponent<MeshFilter>().mesh = fighterParts[7];
                shipArray[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                shipArray[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];

                underGun = Random.Range(1, 101);
            }
            // If no tail is assigned, move onto adding wings 60%
            if(tailChance < 60.0f)
            {
                shipArray[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                shipArray[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];
            }
            if(engineType > 50.0f)
            {
                shipArray[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = fighterParts[1];
                shipArray[i].transform.GetChild(3).GetComponent<MeshFilter>().mesh = fighterParts[1];
            }
            if (engineType < 50.0f)
            {
                // Add the big engine to the 1st engine node and destroy the 2nd node as we do not need it, then move the 1st node into the center.
                shipArray[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = fighterParts[2];
                shipArray[i].transform.GetChild(2).transform.position = new Vector3(shipArray[i].transform.GetChild(2).transform.position.x, shipArray[i].transform.GetChild(2).transform.position.y - .006f, shipArray[i].transform.GetChild(2).transform.position.z);
                shipArray[i].transform.GetChild(2).transform.localScale = new Vector3(shipArray[i].transform.GetChild(2).transform.localScale.x, shipArray[i].transform.GetChild(2).transform.localScale.y + 0.6f, shipArray[i].transform.GetChild(2).transform.localScale.z);

                Destroy(shipArray[i].transform.GetChild(3).gameObject);
            }
            // If we are not considering a undercarrige weapon then add them randomly
            if(weaponCount < 5)
            {
                // Add guns for wing 1
                shipArray[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                shipArray[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // Add guns for wing 2
                shipArray[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                shipArray[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
            }
            if(weaponCount > 5)
            {
                // Add guns for wing 1
                shipArray[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                shipArray[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // Add guns for wing 2
                shipArray[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                shipArray[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // 80% chance to have an undergun if you have a tail
                if(underGun > 20.0f && tailChance > 60.0f)
                {
                    // Add undercarrige weapon
                    shipArray[i].transform.GetChild(4).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                }
            }
        }
    }

    public void CreateColonyShip(int shipCount, Vector3 spawnPosition, GameObject[] shipArray)
    {
        for(int i = 0; i < shipCount; i++)
        {
            // Set total module count
            int moduleCount = Random.Range(4, 16);
            //Debug.Log(i);

            // Start with creating a base and chaning the name
            shipArray[i] = Instantiate(colonyBase, spawnPosition, transform.rotation, fleets[fleetIndex].transform);
            shipArray[i].AddComponent<Rigidbody>();
            shipArray[i].GetComponent<Rigidbody>().useGravity = false;
            shipArray[i].name = "Colony Ship " + i;
            shipArray[i].AddComponent<ShipBehaviour>();
            Vector3 offset = shipArray[i].transform.position;
            offset.z = offset.z - 2;

            // Changing the base to a front colony mesh module
            int frontChance = Random.Range(0, 101);
            if(frontChance > 50.0f)
            {
                shipArray[i].GetComponent<MeshFilter>().mesh = colonyShipParts[0];
            }
            else
            {
                shipArray[i].GetComponent<MeshFilter>().mesh = colonyShipParts[1];
            }

            bool isLast = false;
            // Loop through the module count to add modules to the ship
            for (int j = 0; j < moduleCount; j++)
            {
                // Generic probability variable
                int chance = Random.Range(0, 101);

                if(j >= moduleCount - 1)
                {
                    isLast = true;
                }

                if (isLast)
                {
                    //Debug.Log("Add an engine");
                    CreateRear(offset, i, shipArray);
                }

                // If we are just starting the build, the 2nd section must always be generic
                if (shipArray[i].transform.childCount <= 0)
                {
                    CreateConnector(offset, i, shipArray);
                    offset.z = offset.z - 2;
                }
                else if (chance > 60 && !isLast && IsNumberOdd(j))
                {
                    CreateHabitatModule(offset, i, shipArray);
                    offset.z = offset.z - 2;
                }
                else if(IsNumberOdd(j) && chance < 40 && !isLast)
                {
                    CreateModule(offset, i, shipArray);
                    offset.z = offset.z - 2;
                }
                else if(!IsNumberOdd(j) && !isLast)
                {
                    //Debug.Log("Create Connector");
                    CreateConnector(offset, i, shipArray);
                    offset.z = offset.z - 2;
                }
            }
            // Scale down the ship because I stupidly made the models too big :(
            shipArray[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    private void CreateConnector(Vector3 offset, int i, GameObject[] shipArray)
    {
        // Add the generic section and update the offset.
        GameObject temp;
        temp = Instantiate(colonyBase, offset, transform.rotation, shipArray[i].transform);
        temp.transform.localScale = new Vector3(temp.transform.localScale.x / 2.5f, temp.transform.localScale.y / 2.5f, temp.transform.localScale.z);
    }

    private void CreateModule(Vector3 offset, int i, GameObject[] shipArray)
    {
        // Creates a standard module
        GameObject temp;
        temp = Instantiate(colonyBase, offset, transform.rotation, shipArray[i].transform);
        temp.transform.localScale = new Vector3(temp.transform.localScale.x, temp.transform.localScale.y, temp.transform.localScale.z);
    }

    private void CreateHabitatModule(Vector3 offset, int i, GameObject[] shipArray)
    {
        // Create a larger habitat module
        GameObject temp;
        temp = Instantiate(colonyBase, offset, transform.rotation, shipArray[i].transform);
        temp.GetComponent<MeshFilter>().mesh = colonyShipParts[2];
        Vector3 position = temp.transform.position;
        GameObject temp2; 
        temp2 = Instantiate(colonyBase, position, transform.rotation, shipArray[i].transform);
        temp2.GetComponent<MeshFilter>().mesh = colonyShipParts[5];
        temp2.transform.localScale = temp2.transform.localScale * 2.5f;
        temp2.AddComponent<RotateScript>();
        int chance = Random.Range(0, 101);
        if(chance > 50)
        {
            temp2.GetComponent<RotateScript>().direction = Time.deltaTime * 2.5f;
        }
        else
        {
            temp2.GetComponent<RotateScript>().direction = -Time.deltaTime * 2.5f;
        }

    }

    private void CreateRear(Vector3 offset, int i, GameObject[] shipArray)
    {
        GameObject temp;
        temp = Instantiate(colonyBase, offset, transform.rotation, shipArray[i].transform);
        temp.GetComponent<MeshFilter>().mesh = colonyShipParts[6];
    }

    private bool IsNumberOdd(int number)
    {
        if ((number & 1) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
