using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action {

    private bool skippedFirstClick = false;

    protected override bool StartAction()
    {
        return GetPlayerActionScript().MoveToTile(GetPlayerActionScript().GetPathDestionation());
    }

    protected override bool DoActualAction()
    {
        return GetPlayerActionScript().Move();
    }


    protected override bool SelectionPhase()
    {
        GetPlayerBFSScript().FindSelectableTiles();
        return CheckMouseForSelection();
    }

    private bool CheckMouseForSelection()
    {
        if (skippedFirstClick)
        {
            if (Input.GetMouseButtonUp(0))
            {
                SetDestination();
                return true;
            }
            else
            {
                HighLightPath();
            }
        }
        else
        {
            skippedFirstClick = true; //purtroppo altrimenti l'azione mi viene triggerata al click sulla GUI
        }
        return false;
    }

    private void HighLightPath()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GetPlayerActionScript().moveLayerMask.value))
        {
            HighLightPathScript t = hit.collider.GetComponent<HighLightPathScript>();
            t.SetPlayerStatusScript(GetPlayerStatusScript()); //per adesso lo passo così
            t.CheckHighlightPath();
        }
    }

    private void SetDestination()
    {
        this.selectionEnded = true;
    }




    //metodi e variabili di utilità per recuperare gli altri script attivi sull'unità

    private PlayerActionScript playerActionScript;
    private PlayerStatusScript playerStatusScript;
    private PlayerBFSScript playerBFSScript;

    private PlayerActionScript GetPlayerActionScript()
    {
        if (this.playerActionScript == null)
        {
            this.playerActionScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerActionScript>();
        }
        return this.playerActionScript;
    }
     

    private PlayerBFSScript GetPlayerBFSScript()
    {
        if (this.playerBFSScript == null)
        {
            this.playerBFSScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerBFSScript>();
        }
        return this.playerBFSScript;
    }

    private PlayerStatusScript GetPlayerStatusScript()
    {
        if (this.playerStatusScript == null)
        {
            this.playerStatusScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerStatusScript>();
        }
        return this.playerStatusScript;
    }

    protected override bool EndAction()
    {
        //TODO: ripulire i selectable tiles
        return true;
    }
}
