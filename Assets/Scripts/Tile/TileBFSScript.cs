using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBFSScript : MonoBehaviour
{

    private bool visited = false;
    private TileBFSScript parent = null;
    private int distance = 0;
    private List<TileBFSScript> adjacencyList = new List<TileBFSScript>();


    public List<TileBFSScript> GetAdjacencyList()
    {
        return adjacencyList;
    }

    public void SetAdjacencyList(List<TileBFSScript> adjacencyList)
    {
        this.adjacencyList = adjacencyList;
    }

    public bool GetVisited()
    {
        return visited;
    }

    public void SetVisited(bool visited)
    {
        this.visited = visited;
    }

    public TileBFSScript GetParent()
    {
        return parent;
    }

    public void SetParent(TileBFSScript parent)
    {
        this.parent = parent;
    }

    public int GetDistance()
    {
        return distance;
    }

    public void SetDistance(int distance)
    {
        this.distance = distance;
    }

    public void Reset()
    {
        adjacencyList.Clear();
        visited = false;
        parent = null;
        distance = 0;
    }

    


    public void CheckTile(Vector3 direction, float jumpHeight, TileBFSScript target)
    {
        Vector3 halfExtents = new Vector3(0.25f, 1 + jumpHeight / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents); //ritorna un array di colliders presenti in una posizione centrata sul primo elemento e con un range pari a halfextents

        foreach (Collider coll in colliders)
        {
            TileStatus tileStatus = coll.GetComponent<TileStatus>();
            TileBFSScript tileBFSScript = coll.GetComponent<TileBFSScript>();
            if (tileStatus != null && tileStatus.IsWalkable())
            {
                RaycastHit hit;
                if (!Physics.Raycast(tileStatus.transform.position, Vector3.up, out hit, 1) || tileStatus == target) //controlla se c'è qualcosa sopra la cella, sparando un raggio verso l'alto con lunghezza 1
                {
                    adjacencyList.Add(tileBFSScript);
                }
            }
        }
    }

    public void FindNeighbors(float jumpHeight, TileBFSScript target)
    {
        //this.GetComponent<TileStatus>().Reset(); //non ho idea perchè ci fosse questa riga, l'ho rimossa e ora funziona :|

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }
}
