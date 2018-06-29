using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBFSScript : MonoBehaviour {
    HashSet<TileBFSScript> selectableTiles = new HashSet<TileBFSScript>();
    GameObject[] tiles;
    Stack<TileBFSScript> path = new Stack<TileBFSScript>();

    public TileBFSScript currentTile;
    public LayerMask onlyTilesLayerMask;


    public int move = 5;
    public float jumpHeight = 2;

    private void Start()
    {
        Init();
    }

    public TileBFSScript GetCurrentTile()
    {
        return currentTile;
    }

    public Stack<TileBFSScript> GetPath()
    {
        return path;
    }

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        //Debug.Log("found "+tiles.Length+" tiles");
        //halfHeight = GetComponent<Collider>().bounds.extents.y;
        //TurnManager.AddUnit(this);
    }

    public void ComputeAdjacencyLists(float jumpHeight, TileBFSScript target)
    {
        foreach (GameObject tile in tiles)
        {
            
            TileBFSScript t = tile.GetComponent<TileBFSScript>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public void SelectCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.GetComponent<TileStatus>().SetCurrent(true);
    }

    public TileBFSScript GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        TileBFSScript tile = null;//Physics.Raycast(target.transform.position, -Vector3.up, out hit, onlyTilesLayerMask)
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 2f, onlyTilesLayerMask.value))
        {
            tile = hit.collider.GetComponent<TileBFSScript>();
        }
        else
        {
            Debug.Log("failed to hit a tile under the player");
        }
        return tile;
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


    public void FindSelectableTiles()
    {
        //selectableTiles.Clear();
        foreach (TileBFSScript tile in selectableTiles)
        {
            tile.Reset();
            tile.GetComponent<TileStatus>().Reset();
        }
        ComputeAdjacencyLists(jumpHeight, null);
        SelectCurrentTile();

        Queue<TileBFSScript> process = new Queue<TileBFSScript>();
        process.Enqueue(currentTile);
        currentTile.SetVisited(true);
        while (process.Count > 0)
        {
            TileBFSScript t = process.Dequeue();

            selectableTiles.Add(t);
            t.GetComponent<TileStatus>().SetSelectable(true);

            if (t.GetDistance() < move)
            {
                //Debug.Log("tile "+t.name+" has "+t.GetAdjacencyList().Count+" adjacent tiles");
                foreach (TileBFSScript tile in t.GetAdjacencyList())
                {
                    if (!tile.GetVisited())
                    {
                        
                        tile.SetParent(t);
                        tile.SetVisited(true);
                        tile.SetDistance(t.GetDistance() + 1);
                        process.Enqueue(tile);
                    }
                }
            }
        }
        Debug.Log("FindSelectableTiles returns: "+selectableTiles.Count+" tiles");
        ClearSight.tilesToClear = selectableTiles;
    }


    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.GetComponent<TileStatus>().SetCurrent(false);
            currentTile = null;
        }
        foreach (TileBFSScript t in selectableTiles)
        {
            t.GetComponent<TileStatus>().Reset(true);
            t.Reset();
        }
        selectableTiles.Clear();
    }

    
}
