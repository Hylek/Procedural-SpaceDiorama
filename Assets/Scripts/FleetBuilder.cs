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
            int tailChance = 0;
            // What engine should this fighter use? 0 for default, 1 for standard, 2 for large
            int engineType = 0;
            // How many weapon should this fighter have? Between 0 and 5
            int weaponCount = 0;
            // Undercarrige weapon
            int underGun = 0;

            Debug.Log("Tail: " + tailChance);

            // Create fighter base
            fighters[i] = Instantiate(fighterBase, transform.position, transform.rotation);
            fighters[i].name = "Fighter " + i;

            // Decide tail, engine and weapon count
            tailChance = Random.Range(1, 101);
            engineType = Random.Range(1, 101);
            weaponCount = Random.Range(0, 6);
            

            // Assign the tail to the correct node, if we have a tail it also means we can support a large undercarried gun (Node gun5) 40% chance
            if(tailChance > 60.0f)
            {
                fighters[i].transform.GetChild(5).GetComponent<MeshFilter>().mesh = fighterParts[7];
                fighters[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                fighters[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];

                underGun = Random.Range(1, 101);
            }
            // If no tail is assigned, move onto adding wings 60%
            if(tailChance < 60.0f)
            {
                fighters[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[6];
                fighters[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[6];
            }
            if(engineType > 50.0f)
            {
                fighters[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = fighterParts[1];
                fighters[i].transform.GetChild(3).GetComponent<MeshFilter>().mesh = fighterParts[1];
            }
            if (engineType < 50.0f)
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
                fighters[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                fighters[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // Add guns for wing 2
                fighters[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                fighters[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
            }
            if(weaponCount == 5)
            {
                // Add guns for wing 1
                fighters[i].transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                fighters[i].transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // Add guns for wing 2
                fighters[i].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];
                fighters[i].transform.GetChild(1).GetChild(1).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 6)];

                // 80% chance to have an undergun if you have a tail
                if(underGun > 20.0f)
                {
                    // Add undercarrige weapon
                    fighters[i].transform.GetChild(4).GetComponent<MeshFilter>().mesh = fighterParts[Random.Range(3, 5)];
                }
            }
        }
    }
}
