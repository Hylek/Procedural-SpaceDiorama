using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    public GameObject[] ships;
    public List<GameObject> targetPlanets;
    private Vector3 finalForce;

    public SolarSystemScript system;
    public GameObject target;
    private float maxSpeed = 1.1f;
    private Vector3 velocity = Vector3.zero;

    float FAttract_Factor = 10.0f;
    float FRepel_Factor = 0.5f;
    float FAlign_Factor = 40.0f;
    bool isObstacle = false;

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

    private void Update()
    {
        //Vector3 centerOfMass = Vector3.zero;
        //Vector3 vMean = Vector3.zero;
        //float vision = 5.5f;
        //float maxVision = 10.0f;

        //for (int i = 0; i < ships.Length; i++)
        //{
        //    centerOfMass += ships[i].transform.position;
        //    vMean += ships[i].GetComponent<Rigidbody>().velocity.normalized;
        //}
        //Vector3 mMean = centerOfMass / ships.Length;
        //Vector3 dMean = vMean / ships.Length;

        //for (int i = 0; i < ships.Length; i++)
        //{
        //    // Attract Force
        //    Vector3 attractForce = (mMean - ships[i].transform.position) * FAttract_Factor;

        //    // Align Force
        //    Vector3 alignForce = (dMean - ships[i].GetComponent<Rigidbody>().velocity) * FAlign_Factor;

        //    // Fix Orientation
        //    ships[i].transform.LookAt(ships[i].transform.position + dMean);

        //    // Seek Force
        //    Vector3 seekForce = (target.transform.position - ships[i].transform.position).normalized;

        //    // Avoid Force
        //    Vector3 visionFactor = ships[i].transform.position + ships[i].GetComponent<Rigidbody>().velocity.normalized * maxVision;

        //    RaycastHit hit;
        //    Vector3 avoidForce = Vector3.zero;
        //    if (Physics.Raycast(transform.position, dMean * vision, out hit, 50.0f))
        //    {
        //        Debug.DrawRay(ships[i].transform.position, (dMean * vision) * hit.distance, Color.red);
        //        isObstacle = true;
        //    }
        //    else
        //    {
        //        avoidForce = Vector3.zero;
        //    }
        //    Debug.DrawRay(ships[i].transform.position, dMean * vision, Color.white);

        //    // Repel Force
        //    Vector3 repelForce = Vector3.zero;
        //    for (int j = 0; j < ships.Length; j++)
        //    {
        //        if (i == j) continue; // Skip the loop so a ship doesn't calculate forces for itself

        //        Vector3 delta = ships[i].transform.position - ships[j].transform.position;
        //        repelForce += (delta / delta.magnitude) * FRepel_Factor;
        //    }

        //    // Add the forces together
        //    Vector3 finalForce = attractForce + alignForce + repelForce + seekForce + avoidForce;

        //    // Apply the forces to the ships' rigidbodies
        //    ships[i].GetComponent<Rigidbody>().AddForce(finalForce);


        //    if (Vector3.Distance(ships[i].transform.position, target.transform.position) < 20.0f)
        //    {
        //        Vector3 scale = ships[i].transform.localScale;
        //        scale.x -= 0.001f;
        //        scale.y -= 0.001f;
        //        scale.z -= 0.001f;
        //    }

        //}
    }
}
