using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitActionScript : UserActionScript {
    //qua dentro va spostata tutta la logica a comune tra giocatori e NPC
    public int remainingActionPoints;
    public int actionPointsPerTurn = 2;

    public int GetRemainingActionPoints()
    {
        return remainingActionPoints;
    }

    public void BeginTurn()
    {
        BeginTurn(true);
    }

    public void ResetActionPoints()
    {
        remainingActionPoints = actionPointsPerTurn;
    }

    public void BeginTurn(bool resetActionPoints)
    {
        if (resetActionPoints || remainingActionPoints == 0)
        {
            ResetActionPoints();
        }
        Debug.Log("unit " + this.name + " begins turn with " + remainingActionPoints + "action points ");
    }

    public void EndTurn()
    {
    
    }

    public abstract void DoReset();
}
