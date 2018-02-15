using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject target;
    public float orbitSpeed;

    private Transform targetPosition;

    private void Start()
    {
        // Get the target object's transform
        targetPosition = target.transform;
    }

    private void FixedUpdate()
    {
        // Rotate the object around the central object
        transform.RotateAround(targetPosition.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
