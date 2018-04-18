using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperateController : MonoBehaviour
{
    public FleetBuilder shipBuilder;
    GameObject[] fighterArray;
    public int fighterCount = 1;
    bool isSpawning = false;
    public float minTime = 3.0f;
    public float maxTime = 10.0f;

    void Start ()
    {
        shipBuilder = GameObject.Find("_MANAGER").GetComponent<FleetBuilder>();
        fighterArray = new GameObject[fighterCount];
	}
	
	void Update ()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnFighter(Random.Range(minTime, maxTime)));
        }
    }

    IEnumerator SpawnFighter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        shipBuilder.CreateFighter(fighterCount, transform.position, fighterArray);
        shipBuilder.CreateFighter(fighterCount, transform.position, fighterArray);
        isSpawning = false;
    }
}
