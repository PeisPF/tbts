using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPathScript : UserActionScript
{
    private PlayerStatusScript playerStatusScript;

    private Stack<TileBFSScript> GetPath()
    {
        return playerStatusScript.GetSelectedPath();
    }

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
        //Debug.Log("SetPlayerStatusScript called, path.Count: "+path.Count);
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
        Debug.Log("HighlightPathTo " + tile.name+", path current size: "+ GetPath().Count);
        ResetPath();
        DoHighLightPathTo(tile);
    }

    public void ResetPath()
    {
        Debug.Log("Reset Path, currently holding "+ GetPath().Count);
        foreach (TileBFSScript t in GetPath())
        {
            Debug.Log("removing path from " + t.name);
            t.GetComponent<TileStatus>().SetPath(false);
        }

        GetPath().Clear();
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

            GetPath().Push(next.GetComponent<TileBFSScript>());
            if (next.GetComponent<TileBFSScript>().GetParent())
            {
                next = next.GetComponent<TileBFSScript>().GetParent().GetComponent<TileStatus>();
            }
            else
            {
                next = null;
            }

        }
        //Debug.Log("path.Count: "+path.Count);
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
