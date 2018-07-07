﻿using System;
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

    protected override bool EndAction()
    {
        GetPlayerBFSScript().RemoveSelectableTiles();
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(false);
        return true;
    }

}