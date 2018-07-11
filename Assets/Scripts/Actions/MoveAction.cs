using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action {

    private bool skippedFirstClick = false;

    protected override float GetCameraMovementDuration()
    {
        //Debug.Log("path length: " + TurnManager.GetCurrentPlayer().GetComponent<PlayerBFSScript>().GetPath().Count);
        return 5;
    }

    public MoveAction(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound, CPC_CameraPath path) : base(selectionSound, continuousSound, endActionSound, path)
    {
    }

    protected override bool StartAction()
    {
        return base.StartAction() && GetPlayerActionScript().MoveToTile(GetPlayerActionScript().GetPathDestionation());
    }

    protected override bool DoActualAction()
    {
        return base.DoActualAction() && GetPlayerActionScript().Move();
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

    protected override bool EndAction()
    {
        GetPlayerBFSScript().RemoveSelectableTiles();
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(false);
        return base.EndAction()&&true;
    }

}
