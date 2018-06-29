using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitActionScript : UserActionScript {
    //qua dentro va spostata tutta la logica a comune tra giocatori e NPC
    public int remainingActionPoints;
    private bool turn;
    public int actionPointsPerTurn = 2;

    public void BeginTurn()
    {
        BeginTurn(true);
    }

    public void BeginTurn(bool resetActionPoints)
    {
        if (resetActionPoints || remainingActionPoints == 0)
        {
            remainingActionPoints = actionPointsPerTurn;
        }
        Debug.Log("unit " + this.name + " begins turn with " + remainingActionPoints + "action points ");
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }

    public abstract void DoReset();
}
