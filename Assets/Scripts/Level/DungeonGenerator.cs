using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MapGeneratorScript script = GetComponent<MapGeneratorScript>();
        script.GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
