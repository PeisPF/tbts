using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInitializer : MonoBehaviour {
    public bool initialized;
    
    // Use this for initialization
    public void GenerateFog(int xmin, int xmax, int ymin, int ymax)
    {
        Object obj = Resources.Load("Fog_cube");
        for (float x = xmin; x < xmax; x++)
        {
            for (float z = ymin; z < ymax; z++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.position = new Vector3(x, 1.55f, z) + this.transform.position;
                go.transform.SetParent(this.transform, false);
            }
        }
        initialized = true;
    }
}
