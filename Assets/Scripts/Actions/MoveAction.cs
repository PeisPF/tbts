using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action {

    private bool skippedFirstClick = false;
    private bool found = false;
    HighLightPathScript lastHit;
    protected override bool ShouldResumeCheckOnFog()
    {
        return true;
    }

    public MoveAction(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound ) : base(selectionSound, continuousSound, endActionSound)
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
        if (!found)
        {
            GetPlayerBFSScript().FindSelectableTiles();
            found = true;
        }
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
            if (t != lastHit)
            {
                t.SetPlayerStatusScript(GetPlayerStatusScript()); //per adesso lo passo così
                t.CheckHighlightPath();
                lastHit = t;
            }
            
            
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
