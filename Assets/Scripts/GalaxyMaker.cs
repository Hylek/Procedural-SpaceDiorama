using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMaker : MonoBehaviour {

    public GameObject spherePrefab;

	// Use this for initialization
	void Start () {
        GenerateSpiralGalaxy();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, Time.deltaTime / 2, 0);
	}

    private void GenerateSpiralGalaxy()
    {
        int spirals = 6;
        GameObject star;

        // Equation coefficients
        float a = Random.Range(40, 161); // Global size of galaxy
        float b = Random.Range(11.12f, 12.42f); // Bulge-to-arm, bigger value = bigger sweep
        float n = Random.Range(0.706f, 0.802f); // Spiral tightness
        float brightnessDistance = Random.Range(a / 1.2f, a / 2.2f);

        for (int i = 0; i < 360 * spirals; i++)
        {
            float angle = i * Mathf.Deg2Rad;
            float distance = a / Mathf.Log10(b * Mathf.Tan(angle / (2 * n)));
            float angularOffset = Random.Range(-10, 10) * Mathf.Deg2Rad;

            if (distance < Mathf.Abs(a))
            {
                float x = Mathf.Cos(angle + angularOffset) * distance;
                float z = Mathf.Sin(angle + angularOffset) * distance;

                Vector3 finalPosition = transform.position + new Vector3(x, 0, z);

                float starDistance = Vector3.Distance(finalPosition, transform.position);
                star = Instantiate(spherePrefab, finalPosition, new Quaternion(Random.Range(0, 361), Random.Range(0, 361), Random.Range(0, 361), 0), transform);

                if (starDistance > brightnessDistance)
                {
                    int chance = Random.Range(1, 101);
                    if (chance > 80)
                    {
                        star.GetComponent<Renderer>().material.color = new Color32((byte)Random.Range(125, 255), (byte)Random.Range(125, 255), (byte)Random.Range(125, 255), 255);
                        star.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        star.GetComponent<Renderer>().material.SetColor("_EmissionColor", star.GetComponent<Renderer>().material.color);
                    }
                    else
                    {
                        star.GetComponent<Renderer>().material.color = new Color32((byte)Random.Range(50, 150), (byte)Random.Range(50, 150), (byte)Random.Range(50, 150), 255);
                        star.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        star.GetComponent<Renderer>().material.SetColor("_EmissionColor", star.GetComponent<Renderer>().material.color);
                    }
                } 
                else if (starDistance < brightnessDistance)
                {
                    int chance = Random.Range(1, 101);
                    if (chance > 20)
                    {
                        star.GetComponent<Renderer>().material.color = new Color32((byte)Random.Range(125, 255), (byte)Random.Range(125, 255), (byte)Random.Range(125, 255), 255);
                        star.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        star.GetComponent<Renderer>().material.SetColor("_EmissionColor", star.GetComponent<Renderer>().material.color);
                    }
                    else
                    {
                        star.GetComponent<Renderer>().material.color = new Color32((byte)Random.Range(50, 150), (byte)Random.Range(50, 150), (byte)Random.Range(50, 150), 255);
                        star.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        star.GetComponent<Renderer>().material.SetColor("_EmissionColor", star.GetComponent<Renderer>().material.color);
                    }
                }

            }
        }
    }
}
