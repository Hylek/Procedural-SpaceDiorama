using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


    // Variables
    public FleetController fCon;
    public GameObject target;
    public float camDistance;
    public float camHeight;

    // Use this for initialization
    void Start ()
    {
        fCon = GameObject.Find("FleetController").GetComponent<FleetController>();
        target = fCon.ships[Random.Range(0, fCon.ships.Length)];

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = target.transform.position;
        pos.z -= camDistance;
        pos.y += camHeight;
        transform.position = pos;
    }
}
