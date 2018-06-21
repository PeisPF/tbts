using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitializer : MonoBehaviour {
    public bool initialized;
    private static MapInitializer instance;

    public static MapInitializer GetInstance()
    {
        return instance;
    }
	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance = this;
        }
        Object obj = Resources.Load("Cube");
        for (float x = -7; x < 7; x++)
        {
            for (float z = -15; z < 17; z++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.position = new Vector3(x, 0, z) +this.transform.position;
                go.transform.parent = this.transform;
            }
        }
        initialized = true;
    }
	
}
