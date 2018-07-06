using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GUIActionScript : MonoBehaviour {

    protected abstract Action GetNewAction();

	public void SetAction()
    {
        TurnManager.GetCurrentPlayer().GetComponent<NewPlayerController>().SetCurrentAction(GetNewAction());
    }
}
