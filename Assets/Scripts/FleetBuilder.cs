using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetBuilder : MonoBehaviour
{
    public GameObject fighterBase;

    // Fleet arrays
    public GameObject[] fighters;

    public Mesh[] fighterParts;
    public Material fighterMaterial;

	void Start ()
    {
        CreateFighters(Random.Range(1, 1));
    }
	
	void Update ()
    {
		
	}

    private void CreateFighters(int fighterCount)
    {
        fighters = new GameObject[fighterCount];
        for(int i = 0; i < fighterCount; i++)
        {
            // Should this fighter have a tail? 0 for null, 1 for yes, and 2 for no.
            int hasTail = 0;
            // What engine should this fighter use? 0 for default, 1 for standard, 2 for large
            int engineType = 0;
            // How many weapon should this fighter have? Between 0 and 5
            int weaponCount = 0;

            Debug.Log("Tail: " + hasTail);

            // Create fighter base
            fighters[i] = Instantiate(fighterBase, transform.position, transform.rotation);
            fighters[i].name = "Fighter " + i;

            // Decide tail, engine and weapon count
            hasTail = Random.Range(1, 3);
            engineType = Random.Range(1, 3);
            weaponCount = Random.Range(0, 6);

            // Assign the tail to the correct node, if we have a tail it also means we can support a large undercarried gun (Node gun5)
            if(hasTail == 1)
            {
                fighters[i].transform.GetChild(5).GetComponent<MeshFilter>().mesh = fighterParts[7];
                fighters[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                fighters[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];
            }
            // If no tail is assigned, move onto adding wings
            if(hasTail == 2)
            {
                fighters[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                fighters[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];
            }
            if(engineType == 1)
            {
                fighters[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = fighterParts[1];
                fighters[i].transform.GetChild(3).GetComponent<MeshFilter>().mesh = fighterParts[1];
            }
            if (engineType == 2)
            {
                // Add the big engine to the 1st engine node and destroy the 2nd node as we do not need it, then move the 1st node into the center.
                fighters[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = fighterParts[2];
                fighters[i].transform.GetChild(2).transform.position = new Vector3(fighters[i].transform.GetChild(2).transform.position.x, fighters[i].transform.GetChild(2).transform.position.y, fighters[i].transform.GetChild(2).transform.position.z + .6f);
                fighters[i].transform.GetChild(2).transform.localScale = new Vector3(fighters[i].transform.GetChild(2).transform.localScale.x, fighters[i].transform.GetChild(2).transform.localScale.y + 0.6f, fighters[i].transform.GetChild(2).transform.localScale.z);

                Destroy(fighters[i].transform.GetChild(3).gameObject);
            }
            // If we are not considering a undercarrige weapon then add them randomly
            if(weaponCount < 5)
            {
                // Add guns for wing 1
                fighters[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                fighters[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];

                // Add guns for wing 2
                fighters[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                fighters[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
            }
            if(weaponCount == 5)
            {
                // Add guns for wing 1
                fighters[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                fighters[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];

                // Add guns for wing 2
                fighters[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                fighters[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];

                // Add undercarrige weapon
                fighters[i].transform.GetChild(4).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
            }
        }
    }
}
