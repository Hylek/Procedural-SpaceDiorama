using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCorrector : MonoBehaviour
{
	void Update ()
    {
        // Counteract the wrong rotations due to blender being not very nice :(
        transform.Rotate(-90, 0, 90);
    }
}
