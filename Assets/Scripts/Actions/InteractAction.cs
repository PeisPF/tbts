using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : Action
{
    private bool skippedFirstClick = false;
    Item target;

    public InteractAction(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound) : base(selectionSound, continuousSound, endActionSound)
    {
    }

    protected override bool DoActualAction()
    {
        return base.DoActualAction() && GetPlayerActionScript().InteractWithItem(target);
    }

    protected override bool EndAction()
    {
        Debug.Log("resetting color to black");
        ResetCurrentTargetColor();
        return base.EndAction() &&true;
    }

    private void ResetCurrentTargetColor()
    {
        if (target != null)
        {
            target.ChangeColor(Color.black, 1f);
        }
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
                return SetTarget();
            }
        }
        else
        {
            skippedFirstClick = true; //purtroppo altrimenti l'azione mi viene triggerata al click sulla GUI
        }
        return false;
    }

    private bool SetTarget()
    {
        ResetCurrentTargetColor();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GetPlayerActionScript().interactLayerMask.value))
        {
            target = hit.collider.GetComponent<Item>();
            if (GetPlayerActionScript().IsInteractionPossible(target))
            {
                target.ChangeColor(Color.green, 0.5f);
                return true;
            }
            else
            {
                target.ChangeColor(Color.red, 0.5f);
            }
            
        }
        return false;
    }
}
