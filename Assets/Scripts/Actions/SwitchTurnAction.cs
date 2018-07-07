using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnAction : Action
{
    private PlayerActionScript target;

    protected override int GetCost()
    {
        return 0;
    }

    public SwitchTurnAction(PlayerActionScript target)
    {
        this.target = target;
    }

    protected override bool DoActualAction()
    {
        TurnManager.SwitchTurn(target);
        return true;
    }

    protected override bool SelectionPhase()
    {
        return true;
    }

    protected override bool StartAction()
    {
        return true;
    }

    protected override bool EndAction()
    {
        return true;
    }
}
