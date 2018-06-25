using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPathScript : MonoBehaviour {

    Stack<TileBFSScript> path = new Stack<TileBFSScript>();

    private TileBFSScript pathDestination;

    public void CheckHighlightPath(RaycastHit hit)
    {
        Debug.Log("CheckHighlightPath");
        TileStatus t = hit.collider.GetComponent<TileStatus>();
        if (t.IsSelectable())
        {
            pathDestination = t.GetComponent<TileBFSScript>();
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
        this.GetComponent<PlayerStatusScript>().SetMoving(false);
        this.GetComponent<PlayerStatusScript>().SetShowingPath(true);

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



}
