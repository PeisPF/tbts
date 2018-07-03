using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnClick : MonoBehaviour {

    public int indexOfNextScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            if (indexOfNextScene == -1)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(indexOfNextScene);
            }
        }
            
    }
}
