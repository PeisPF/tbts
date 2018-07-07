using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    static Dictionary<string, List<UnitActionScript>> units = new Dictionary<string, List<UnitActionScript>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<UnitActionScript> turnTeam = new Queue<UnitActionScript>();

	// Use this for initialization
	void Start () {
	}

    public static Collider GetCurrentPlayer()
    {
        if (turnTeam!=null && turnTeam.Count > 0)
        {
            UnitActionScript tact = turnTeam.Peek();
            if (tact != null)
            {
                return tact.GetComponentInParent<Collider>();
            }
        }
        return null;
    }
	
	// Update is called once per frame
	void Update () {
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        List<UnitActionScript> teamList = units[turnKey.Peek()];

        foreach (UnitActionScript unit in teamList)
        {
            unit.ResetActionPoints();
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void SwitchTurn(PlayerActionScript player)
    {
        UnitActionScript unit = turnTeam.Peek();
        if (turnTeam.Contains(player))
        {
            UnitActionScript[] tempTurns = turnTeam.ToArray();
            turnTeam = new Queue<UnitActionScript>();
            turnTeam.Enqueue(player);
            foreach (UnitActionScript tact in tempTurns)
            {
               // Debug.Log("checking: "+tact+" != "+player+" ->"+ (tact != player));
                if (tact != player)
                {
                    turnTeam.Enqueue(tact);
                    PrintTurnQueue();
                }
            }
            int temp = unit.remainingActionPoints;

            unit.EndTurn();
            unit.DoReset();
            unit.remainingActionPoints = temp;
            StartTurn(false);
        }
        else
        {
            Debug.Log("selected player can't move in this turn");
        }
        PrintTurnQueue();
    }

    private static void PrintTurnQueue()
    {
        string debugString = "action queue ";
        foreach (UnitActionScript item in turnTeam.ToArray())
        {
            debugString += item.name + ",";
        }
        Debug.Log(debugString);
    }

    public static void StartTurn()
    {
        StartTurn(true);
    }

    public static void StartTurn(bool resetActionPoints)
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn(resetActionPoints);
        }
    }

    public static void EndTurn()
    {
        UnitActionScript unit = turnTeam.Dequeue();
        unit.EndTurn();
        if (turnTeam.Count > 0)
        {
            StartTurn(false);
        }
        else
        {
            Debug.Log("Turn ended");
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }

    }


    public static void AddUnit(UnitActionScript unit)
    {
        List<UnitActionScript> list;
        if (!units.ContainsKey(unit.tag))
        {
            list = new List<UnitActionScript>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                Debug.Log("adding unit " + unit.name);
                turnKey.Enqueue(unit.tag);
                Debug.Log("turnKey has " +turnKey.Count+" elements");
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }

    public static void RemoveUnit(UnitActionScript unit)
    {
        //implementarla se un'unità può essere rimossa dal gioco
    }
}
