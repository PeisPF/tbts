using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        //SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        //SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
