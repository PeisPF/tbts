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

    private GameObject LoadMap()
    {
        Object obj = Resources.Load("TestMap");
        Debug.Log("loaded obj: " + obj);
        GameObject map = (GameObject)Instantiate(obj);
        map.name = "mappa di test";
        map.transform.position = Vector3.zero;
        return map;
    }

    private void LoadTiles()
    {
        Object obj = Resources.Load("Cube");
        for (float x = -7; x < 7; x++)
        {
            for (float z = -15; z < 17; z++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.position = new Vector3(x, 0, z) + this.transform.position;
            }
        }
    }

	void Start () {
        if (instance == null)
        {
            instance = this;
        }
        GameObject map = LoadMap();
        LoadTiles();
        LoadPlayerUnits(map);
        initialized = true;
    }

    private void LoadPlayerUnits(GameObject map)
    {
        PlayerStartingPointScript[] playerStartingPoints = map.GetComponentsInChildren<PlayerStartingPointScript>();
        Object obj = Resources.Load("Player");
        foreach (PlayerStartingPointScript pos in playerStartingPoints)
        {
            GameObject go = (GameObject)Instantiate(obj);
            go.transform.position = pos.transform.position;
        }
    }
}
