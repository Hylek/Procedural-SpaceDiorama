using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FleetController : MonoBehaviour
{
    public GameObject[] ships;

    static float Range_FAttract = 5.0f;
    static float Range_FRepel = 3f;
    static float Range_FAlign = 2.5f;

    static float FAttract_Factor = 3.0f;
    static float FRepel_Factor = 1.0f;
    static float FAlign_Factor = 10.0f;
    static float FAttract_Vmax = 2.0f;


    Vector3 force;

    void Start ()
    {
        // Add all ships in the fleet into the array
        ships = new GameObject[transform.childCount];
		for(int i = 0; i < transform.childCount; i++)
        {
            ships[i] = transform.GetChild(i).gameObject;
        }
	}

    private void Update()
    {
        
    }

    void ApplyForces()
    {

    }

    void AttractForce()
    {
        Vector3 centerOfMass = Vector3.zero;
        float neighbourCount = 0;
        Vector3 attractForce = Vector3.zero;

        for(int i = 0; i < ships.Length; i++)
        {
            for(int j = i + 1; j < ships.Length; j++)
            {
                Vector3 sep = ships[i].transform.position - ships[j].transform.position;
                Vector3 pos = ships[j].transform.position;

                float d = sep.magnitude;

                if(d < Range_FAttract)
                {
                    centerOfMass += ships[j].transform.position;
                    neighbourCount++;
                }
            }
        }
    }


	
}
