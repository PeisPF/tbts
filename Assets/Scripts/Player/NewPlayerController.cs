using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour {

    private Action currentAction;

    public void SetCurrentAction(Action action)
    {
        this.currentAction = action;
    }

    private PlayerStatusScript playerStatusScript;

    private PlayerStatusScript GetPlayerStatusScript()
    {
        if (playerStatusScript == null)
        {
            playerStatusScript = this.GetComponent<PlayerStatusScript>();
        }
        return playerStatusScript;
    }

    private CheckFogScript checkFogScript;

    private CheckFogScript GetCheckFogScript()
    {
        if (checkFogScript == null)
        {
            checkFogScript = this.GetComponent<CheckFogScript>();
        }
        return checkFogScript;
    }

    private PlayerBFSScript playerBFSScript;

    private PlayerBFSScript GetPlayerBFSScript()
    {
        if (playerBFSScript == null)
        {
            playerBFSScript = this.GetComponent<PlayerBFSScript>();
        }
        return playerBFSScript;
    }

    private PlayerActionScript playerActionScript;

    private PlayerActionScript GetPlayerActionScript()
    {
        if (playerActionScript == null)
        {
            playerActionScript = this.GetComponent<PlayerActionScript>();
        }
        return playerActionScript;
    }


    void Update()
    {
        GetCheckFogScript().CheckFog();
        if (TurnManager.GetCurrentPlayer() == this.GetComponent<Collider>())
        {
            if (currentAction != null)
            {
                if (currentAction.DoAction())
                {
                    currentAction = null;
                }
            }
        }
    }

}
