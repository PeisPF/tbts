using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {
    private float nextActionTime = 0.0f;
    // Use this for initialization
    void Start () {
		
	}

    void Flicker()
    {
        this.GetComponent<Light>().range = Random.Range(4.5f, 5.5f);
        this.GetComponent<Light>().intensity = Random.Range(9.0f, 11.0f);
        Color color = this.GetComponent<Light>().color;
        color.g = Random.Range(120f/360, 160f/360);
        this.GetComponent<Light>().color = color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > nextActionTime)
        {

            float period = Random.Range(0.05f, 0.2f);
            nextActionTime += period;
            Flicker();
        }
       

    }
}
