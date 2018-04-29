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
        Test();
    }

    // This is used to test the ship building algorithm
    void Test()
    {
        shipBuilder.TestFleet(transform.position);
    }

    void Spawn()
    {
        shipBuilder.SpawnFleet(transform.position);
        shipBuilder.SpawnFleet(transform.position);
    }


}
