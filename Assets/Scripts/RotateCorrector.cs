using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCorrector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Counteract the wrong rotations due to blender being not very nice :(
        transform.Rotate(-90, 0, 90);
    }
}
