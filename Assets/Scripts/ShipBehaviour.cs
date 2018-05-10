using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    float Range_FAttract = 15.0f;
    float Range_FRepel = 20.0f;
    float Range_FAlign = 50.0f;
    float FAttract_Vmax = 1.0f;
    float FAttract_Factor = 3.0f;
    float FRepel_Factor = 0.3f;
    float FAlign_Factor = 50.0f;

    public FleetController fleetManager;
    Vector3 force = Vector3.zero;

    void Start()
    {
        fleetManager = transform.GetComponentInParent<FleetController>();
    }

    private void Update()
    {
        ApplyForces(fleetManager.ships);
        GetComponent<Rigidbody>().AddForce(force);

        if(Vector3.Distance(transform.position, fleetManager.target.transform.position) < 5.0f)
        {
            Debug.Log("Fleet has arrived!");
            Destroy(transform.parent.gameObject);
        }
    }

    public void ApplyForces(GameObject[] ships)
    {
       force = RepelForce(ships) + AttractForce(ships) + AlignForce(ships) + SeekForce(ships);
    }

    Vector3 AttractForce(GameObject[] ships)
    {
        Vector3 centerOfMass = new Vector3(0, 0, 0);
        float neighbourCount = 0;
        Vector3 attractForce = new Vector3(0, 0, 0);

        for (int i = 0; i < ships.Length; i++)
        {
            if (this == ships[i]) continue;

            Vector3 sep = transform.position - ships[i].transform.position;

            float d = sep.magnitude;

            if (d < Range_FAttract)
            {
                centerOfMass += ships[i].transform.position;
                neighbourCount++;
            }
        }
        if (neighbourCount > 0)
        {
            centerOfMass /= neighbourCount;
            Vector3 direction = (centerOfMass - transform.position).normalized;
            Vector3 desired = direction * FAttract_Vmax;
            attractForce += (desired - GetComponent<Rigidbody>().velocity) * FAttract_Factor;
        }
        return attractForce;
    }

    Vector3 AlignForce(GameObject[] ships)
    {
        Vector3 direction = new Vector3(0, 0, 0);
        float neighbourCount = 0;
        Vector3 alignForce = new Vector3(0, 0, 0);

        for (int i = 0; i < ships.Length; i++)
        {
            if (this == ships[i]) continue;

            Vector3 sep = transform.position - ships[i].transform.position;

            float d = sep.magnitude;

            if (d < Range_FAlign)
            {
                direction += ships[i].GetComponent<Rigidbody>().velocity;
                neighbourCount++;
            }
        }
        if (neighbourCount > 0)
        {
            direction /= neighbourCount;
            Vector3 desired = direction;
            alignForce += (desired - GetComponent<Rigidbody>().velocity) * FAlign_Factor;

            // Commented code is accurate but causes extreme jittering among ships, used code is much smoother but slightly less accurate in some cases
            transform.LookAt(/*transform.position + desired.normalized */ fleetManager.target.transform);
        }
        return alignForce;
    }

    Vector3 RepelForce(GameObject[] ships)
    {
        float neighbourCount = 0;
        Vector3 repelForce = Vector3.zero;

        for (int i = 0; i < ships.Length; i++)
        {
            if (this == ships[i]) continue;

            Vector3 sep = transform.position - ships[i].transform.position;

            float d = sep.magnitude;

            if (d < Range_FRepel)
            {
                Vector3 delta = (transform.position - ships[i].transform.position);
                float length = delta.magnitude;
                if(length == 0)
                {
                    continue;
                }
                repelForce += (delta / Mathf.Pow(delta.magnitude, 2.0f));  
                neighbourCount++;
            }
        }
        if (neighbourCount > 0)
        {
           repelForce *= FRepel_Factor;
        }
        return repelForce;
    }

    Vector3 SeekForce(GameObject[] ships)
    {
        Vector3 seekForce = Vector3.zero;
        for (int i = 0; i < ships.Length; i++)
        {
            seekForce = (fleetManager.target.transform.position - ships[i].transform.position).normalized * 2.0f;
        }
        return seekForce;
    }
}
