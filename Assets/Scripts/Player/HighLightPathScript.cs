using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPathScript : UserActionScript
{

    Stack<TileBFSScript> path = new Stack<TileBFSScript>();

    private PlayerStatusScript playerStatusScript;

    private PlayerStatusScript GetPlayerStatusScript()
    {
        /*if (playerStatusScript == null)
        {
            playerStatusScript = this.GetComponent<PlayerStatusScript>();
        }*/
        return playerStatusScript;
    }

    public void SetPlayerStatusScript(PlayerStatusScript playerStatusScript)
    {
        this.playerStatusScript = playerStatusScript;
    }

    //private TileBFSScript pathDestination;

    public void CheckHighlightPath(/*RaycastHit hit*/)
    {
        Debug.Log("CheckHighlightPath");
        TileStatus t = this.GetComponent<TileStatus>();
        if (t.IsSelectable())
        {
            TurnManager.GetCurrentPlayer().GetComponent<PlayerActionScript>().SetPathDestination(t.GetComponent<TileBFSScript>());
            //pathDestination = t.GetComponent<TileBFSScript>();
            HighlightPathTo(t);
        }
    }

    public void HighlightPathTo(TileStatus tile)
    {
        Debug.Log("HighlightPathTo " + tile.name);
        ResetPath();
        DoHighLightPathTo(tile);
    }

    public void ResetPath()
    {
        Debug.Log("Reset Path");
        foreach (TileBFSScript t in path)
        {
            t.GetComponent<TileStatus>().SetPath(false);
        }

        path.Clear();
    }

    public void DoHighLightPathTo(TileStatus tile)
    {
        tile.SetTarget(true);
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(true);

        TileStatus next = tile;
        while (next != null)
        {
            next.SetPath(true);
            next.SetTarget(false);
            path.Push(next.GetComponent<TileBFSScript>());
            if (next.GetComponent<TileBFSScript>().GetParent())
            {
                next = next.GetComponent<TileBFSScript>().GetParent().GetComponent<TileStatus>();
            }
            else
            {
                next = null;
            }

        }
    }

    public override void DoRightClickAction()
    {
        CheckHighlightPath();
    }

    public override void DoLeftClickAction()
    {
        throw new System.NotImplementedException();
    }

    public override void DoDoubleClickAction()
    {
        throw new System.NotImplementedException();
    }
}
