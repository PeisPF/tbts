using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPathScript : MonoBehaviour
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
        ResetPath();
        DoHighLightPathTo(tile);
    }

    public void ResetPath()
    {
        foreach (TileBFSScript t in GetPath())
        {
            t.GetComponent<TileStatus>().SetPath(false);
        }

        GetPath().Clear();
    }

    public void DoHighLightPathTo(TileStatus tile)
    {
        tile.SetTarget(true);
        GetPlayerStatusScript().SetMoving(false);
        //GetPlayerStatusScript().SetShowingPath(true);

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
}
