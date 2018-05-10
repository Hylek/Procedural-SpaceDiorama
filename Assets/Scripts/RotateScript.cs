using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float direction = 0;

	void Update ()
    {
        transform.Rotate(0, 0, direction);
    }
}
