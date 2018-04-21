using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
 //   public SolarSystemScript system;
 //   private GameObject target;
 //   private List<GameObject> temperatePlanets;
 //   private Transform orientation;
 //   private Vector3 startPos;
 //   private float maxForce = 0.15f;
 //   private float maxSpeed = 0.15f;
 //   private Vector3 velocity = Vector3.zero;
 //   private Vector3 offset = new Vector3(-90, 0, 90);

	//void Start ()
 //   {
 //       startPos = transform.position;
 //       system = GameObject.Find("_MANAGER").GetComponent<SolarSystemScript>();
 //       target = system.planets[Random.Range(0, system.planets.Length)];

 //       foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
 //       {
 //           if (go.name == "Temperate")
 //               temperatePlanets.Add(go);
 //       }
 //   }
	
	//void Update ()
 //   {
 //       //float step = 1.5f * Time.deltaTime;

 //       // Look at the target destination
 //       //transform.LookAt(target.transform.position);

 //       // Counteract the wrong rotations due to blender being wank
 //       //transform.Rotate(-90, 0, 90);

 //       //float distance = Vector3.Distance(transform.position, target.transform.position);
 //       //if(distance < 1.5f)
 //       //{
 //       //    Destroy(gameObject);
 //       //}

 //       //// Vector3.MoveTowards(transform.position, target.transform.position, step);

 //       //Vector3 desired = (target.transform.position - transform.position).normalized * maxForce;
 //       //Vector3 direction = desired - velocity;
 //       //direction = Vector3.ClampMagnitude(direction, maxForce);
 //       //direction = direction / GetComponent<Rigidbody>().mass;
 //       //velocity = Vector3.ClampMagnitude(velocity + direction, maxSpeed);
 //       //transform.position = transform.position + velocity;
 //   }
}
