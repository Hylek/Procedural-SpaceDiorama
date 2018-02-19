using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject target;
    public float orbitSpeed;
    public Vector3 orbitAngle;

    private Transform targetPosition;

    private void Start()
    {
        // Get the target object's transform
        targetPosition = target.transform;
    }

    private void FixedUpdate()
    {
        // Rotate the object around the central object
        transform.RotateAround(targetPosition.position, orbitAngle, orbitSpeed * Time.deltaTime);
    }
}
