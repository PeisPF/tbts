using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private bool generated = false;
    private int attempts = 0;
    private int MAX_ATTEMPTS = 10;

    // Use this for initialization
    void Start()
    {
        MapGeneratorScript script = GetComponent<MapGeneratorScript>();
        while (!generated && attempts < MAX_ATTEMPTS)
        {
            try
            {
                attempts++;
                script.GenerateMap();
                generated = true;
                Debug.Log("successfully created map in " + attempts+ " attempts");
            }
            catch (Exception e)
            {
                Debug.Log("failed attempt number " + attempts);
                script.CleanUp();
                Debug.Log(e);
            }

        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
