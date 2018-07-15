using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour{
    
    public static GameObject MyInstantiate(Object obj)
    {
        GameObject go = (GameObject)Instantiate(obj);
        Collider[] colliders = go.GetComponents<Collider>();
        foreach (Collider collider in colliders){
            collider.enabled = false;
            collider.enabled = true;
        }
        colliders = go.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
            collider.enabled = true;
        }
        return go;
    }
}
