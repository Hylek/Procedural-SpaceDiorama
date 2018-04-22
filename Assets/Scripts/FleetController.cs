using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    public GameObject[] ships;
    public List<GameObject> targetPlanets;

    public SolarSystemScript system;
    private GameObject target;
    private float maxSpeed = 1.1f;
    private Vector3 velocity = Vector3.zero;

    float FAttract_Factor = 18.0f;
    float FRepel_Factor = 5.0f;
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
        target = targetPlanets[Random.Range(0, targetPlanets.Count + 1)];
    }

    private void Update()
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 vMean = Vector3.zero;
        float vision = 5.5f;
        float maxVision = 10.0f;

        for (int i = 0; i < ships.Length; i++)
        {                   
            centerOfMass += ships[i].transform.position;
            vMean += ships[i].GetComponent<Rigidbody>().velocity.normalized;
        }
        Vector3 mMean = centerOfMass / ships.Length;
        Vector3 dMean = vMean / ships.Length;

        for (int i = 0; i < ships.Length; i++)
        {
            // Attract
            Vector3 attract = FAttract_Factor * (mMean - ships[i].transform.position);

            // Align
            Vector3 align = FAlign_Factor * (dMean - ships[i].GetComponent<Rigidbody>().velocity);

            // Seek
            Vector3 seek = (target.transform.position - ships[i].transform.position).normalized;

            // Avoid
            Vector3 vVision = ships[i].transform.position + ships[i].GetComponent<Rigidbody>().velocity.normalized * maxVision;

            RaycastHit hit;
            Vector3 avoid = Vector3.zero;
            if (Physics.Raycast(transform.position, dMean * vision, out hit, 50.0f))
            {
                Debug.DrawRay(ships[i].transform.position, (dMean * vision) * hit.distance, Color.red);
                Debug.Log("Hit: " + hit.transform.name);
                isObstacle = true;
            }
            else
            {
                avoid = Vector3.zero;
            }
            Debug.DrawRay(ships[i].transform.position, dMean * vision, Color.white);

            // repel
            Vector3 repel = Vector3.zero;
            for (int j = 0; j < ships.Length; j++)
            {
                if (i != j)
                {
                    Vector3 delta = ships[j].transform.position - ships[i].transform.position;
                    repel += delta * (-FRepel_Factor / (Mathf.Pow(delta.magnitude, 2)));
                }
            }
            ships[i].GetComponent<Rigidbody>().AddForce((attract + align + repel + seek) * maxSpeed);
            ships[i].transform.LookAt(ships[i].transform.position + dMean);

            if (Vector3.Distance(ships[i].transform.position, target.transform.position) < 10.0f)
            {
                Vector3 scale = ships[i].transform.localScale;
                scale.x -= 0.001f;
                scale.y -= 0.001f;
                scale.z -= 0.001f;
            }
        }

    }
}
