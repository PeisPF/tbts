using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    private float minEmission;
    private float maxEmission;
    private Color baseColor;


    public void SetEmission(float minEmission, float maxEmission, Color baseColor)
    {
        this.minEmission = minEmission;
        this.maxEmission = maxEmission;
        this.baseColor = baseColor;
    }

	// Use this for initialization
	void Start () {
        SetEmission(0.2f, 0.2f, Color.black);
    }

    // Update is called once per frame
    void Update () {
        {
            UpdateMaterial();
        }
        
	}

    private void UpdateMaterial()
    {
        float emission;
        if (minEmission < maxEmission)
        {
            emission = minEmission + Mathf.PingPong(Time.time / 5, maxEmission - minEmission);
        }
        else
        {
            emission = minEmission;
        }
        
        GetComponent<Renderer>().material.SetColor("_EmissionColor", baseColor * emission);
    }
}
