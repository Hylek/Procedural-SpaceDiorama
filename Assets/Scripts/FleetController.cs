using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    public GameObject[] ships;
    public List<GameObject> targetPlanets;
    public SolarSystemScript system;
    public GameObject target;
    public Vector3 cameraTarget;
    int shipCamera = 0;
    public bool viewShips = false;

    private void Start()
    {
        // Add all ships in the fleet into the array
        ships = new GameObject[transform.childCount];
        system = GameObject.Find("_MANAGER").GetComponent<SolarSystemScript>();

        for (int i = 0; i < transform.childCount; i++)
        {
            ships[i] = transform.GetChild(i).gameObject;
        }

        // Loop through all planets to find Temperate ones for colonisation
        foreach (GameObject planet in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if(planet.name == "Temperate")
            {
                targetPlanets.Add(planet);
            }
        }
        target = targetPlanets[Random.Range(0, targetPlanets.Count)];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            shipCamera = Random.Range(0, ships.Length);
            viewShips = true;
        }
        if(viewShips)
        {
            GameObject.Find("Main Camera").transform.position = ships[shipCamera].transform.position + new Vector3(0, 0, -0.5f);
            GameObject.Find("Main Camera").transform.LookAt(ships[shipCamera].transform);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            viewShips = false;
        }
    }
}
