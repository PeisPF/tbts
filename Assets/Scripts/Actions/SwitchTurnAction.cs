using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnAction : Action
{
    private PlayerActionScript target;

    public override int GetCost()
    {
        return 0;
    }

    public SwitchTurnAction(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound, PlayerActionScript target) : base(selectionSound, continuousSound, endActionSound)
    {
        this.target = target;
    }

    protected override bool DoActualAction()
    {
        TurnManager.SwitchTurn(target);
        return base.DoActualAction();
    }

    protected override bool SelectionPhase()
    {
        return true;
    }

}
