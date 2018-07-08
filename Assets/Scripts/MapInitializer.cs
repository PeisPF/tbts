using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapInitializer : MonoBehaviour
{
    public bool initialized;
    private static MapInitializer instance;
    public Transform unitFrames;

    public static MapInitializer GetInstance()
    {
        return instance;
    }
    // Use this for initialization

    private GameObject LoadMap()
    {
        UnityEngine.Object obj = Resources.Load("TestMap");
        Debug.Log("loaded obj: " + obj);
        GameObject map = (GameObject)Instantiate(obj);
        map.name = "mappa di test";
        map.transform.position = Vector3.zero;
        return map;
    }

    private void LoadTiles()
    {
        UnityEngine.Object obj = Resources.Load("Cube");
        for (float x = -7; x < 7; x++)
        {
            for (float z = -15; z < 17; z++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.position = new Vector3(x, 0, z) + this.transform.position;
            }
        }
    }

    void Start()
    {
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
        UnityEngine.Object obj = Resources.Load("Unit/Player");
        int i = 0;
        foreach (PlayerStartingPointScript pos in playerStartingPoints)
        {
            UnitJSONData data = ReadDataFromJSON(i);
            GameObject go = InstantiatePlayer(obj, pos, data);
            InstantiateGUIItem(go);
            i++;
        }
    }

    private UnitJSONData ReadDataFromJSON(int i)
    {
        string path = "Assets/Resources/Text/Units/" + i + ".json";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string jsonString = reader.ReadToEnd();
        Debug.Log(jsonString);
        reader.Close();
        UnitJSONData data = JsonUtility.FromJson<UnitJSONData>(jsonString);
        return data;
    }

    private void InstantiateGUIItem(GameObject playerObject)
    {
        UnityEngine.Object obj = Resources.Load("UI/UnitPanel");
        GameObject go = (GameObject)Instantiate(obj);
        go.GetComponent<UnitFrameGUIScript>().SetPlayer(playerObject.GetComponent<PlayerStatusScript>());
        go.transform.SetParent(unitFrames.transform, false);
    }

    private static GameObject InstantiatePlayer(UnityEngine.Object player, PlayerStartingPointScript pos, UnitJSONData data)
    {
        GameObject go = (GameObject)Instantiate(player);
        go.transform.position = pos.transform.position;
        go.GetComponent<PlayerActionScript>().SetActionPointsPerTurn(data.actionPointsPerTurn);
        go.GetComponent<PlayerActionScript>().SetInteractionReach(data.interactionReach);
        go.GetComponent<PlayerBFSScript>().SetMove(data.move);
        return go;
    }
}
