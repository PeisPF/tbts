using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

   /* private float minEmission;
    private float maxEmission;
    private Color baseColor;
    */

    // Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        {
            this.GetComponent<TileMaterialScript>().MakeMaterialGlow();
        }
        
	}

    
}
