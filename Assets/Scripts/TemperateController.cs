using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperateController : MonoBehaviour
{
    public FleetBuilder shipBuilder;
    public GameObject[] fighterArray;
    public GameObject[] colonyArray;
    public int fighterCount = 1;
    public int colonyCount = 2;
    bool isSpawning = false;
    public float minTime = 3.0f;
    public float maxTime = 10.0f;

    void Start ()
    {
        shipBuilder = GameObject.Find("_MANAGER").GetComponent<FleetBuilder>();
        fighterArray = new GameObject[fighterCount];
        colonyArray = new GameObject[colonyCount];
	}
	
	void Update ()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            //StartCoroutine(SpawnFleet(Random.Range(minTime, maxTime)));
        }
    }

    IEnumerator SpawnFleet(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        shipBuilder.CreateFighter(fighterCount, transform.position, fighterArray);

        shipBuilder.CreateColonyShip(colonyCount, transform.position, colonyArray);
        isSpawning = false;
    }
}
