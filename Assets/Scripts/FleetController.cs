using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    public GameObject[] ships;
    public List<GameObject> targetPlanets;
    public SolarSystemScript system;
    public GameObject target;

    private void Start()
    {
        // Add all ships in the fleet into the array
        ships = new GameObject[transform.childCount];
        system = GameObject.Find("_MANAGER").GetComponent<SolarSystemScript>();

        for (int i = 0; i < transform.childCount; i++)
        {
            ships[i] = transform.GetChild(i).gameObject;
        }

        foreach (GameObject planet in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if(planet.name == "Temperate")
            {
                targetPlanets.Add(planet);
            }
        }
        target = targetPlanets[Random.Range(0, targetPlanets.Count)];
    }
}
