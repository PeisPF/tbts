using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInitializer : MonoBehaviour {
    public bool initialized;
    private static FogInitializer instance;

    public static FogInitializer GetInstance()
    {
        return instance;
    }
    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        Object obj = Resources.Load("Fog_cube");
        for (float x = -7; x < 7; x++)
        {
            for (float z = -15; z < 17; z++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.position = new Vector3(x, 1.55f, z) + this.transform.position;
                go.transform.SetParent(this.transform, false);
            }
        }
        initialized = true;
    }
}
