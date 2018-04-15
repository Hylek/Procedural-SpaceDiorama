using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpinner : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, -Time.deltaTime * 2.5f, 0);
	}
}
