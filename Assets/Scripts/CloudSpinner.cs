using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpinner : MonoBehaviour
{
	void Update ()
    {
        transform.Rotate(0, -Time.deltaTime * 3.5f, 0);
	}
}
