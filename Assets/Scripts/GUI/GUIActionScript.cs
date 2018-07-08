using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GUIActionScript : MonoBehaviour {

    protected abstract Action GetNewAction();
    public AudioSource audioSource;

    public AudioSource selectionSound;
    public AudioSource continuousSound;
    public AudioSource endActionSound;


	public void SetAction()
    {
        audioSource.Play();
        TurnManager.GetCurrentPlayer().GetComponent<NewPlayerController>().SetCurrentAction(GetNewAction());
    }
}
