using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnAction : Action
{

    private bool skippedFirstClick = false;

    protected override bool DoActualAction()
    {
        throw new System.NotImplementedException();
    }

    protected override bool SelectionPhase()
    {
        return CheckMouseForSelection();
    }

    private bool CheckMouseForSelection()
    {
        if (skippedFirstClick)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //SetDestination();
                return true;
            }
            else
            {
                HighlightUnitsInSameTeam();
            }
        }
        else
        {
            skippedFirstClick = true; //purtroppo altrimenti l'azione mi viene triggerata al click sulla GUI
        }
        return false;
    }

    private void HighlightUnitsInSameTeam()
    {
        throw new NotImplementedException();
    }

    protected override bool StartAction()
    {
        throw new System.NotImplementedException();
    }

    protected override bool EndAction()
    {
        throw new NotImplementedException();
    }
}
