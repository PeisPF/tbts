using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticsMove : MonoBehaviour
{
    public bool turn = false;
    HashSet<TileBFSScript> selectableTiles = new HashSet<TileBFSScript>();
    GameObject[] tiles;

    public bool moving = false;
    public bool showingPath = false;

    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;
    public int actionPointsPerTurn = 2;
    public int interactionReach = 1;

    public int remainingActionPoints;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingToEdge = false;
    Vector3 jumpTarget;

    Stack<TileBFSScript> path = new Stack<TileBFSScript>();
    TileBFSScript currentTile;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();//direzione in cui è girato il tizio

    public LayerMask onlyTilesLayerMask;

    public Tile actualTargetTile;

    private bool interactingWithObject = false;


    float halfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        //TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.GetComponent<TileStatus>().SetCurrent(true);
    }

    public TileBFSScript GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        TileBFSScript tile = null;//Physics.Raycast(target.transform.position, -Vector3.up, out hit, onlyTilesLayerMask)
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, onlyTilesLayerMask, onlyTilesLayerMask.value))
        {
            tile = hit.collider.GetComponent<TileBFSScript>();
        }
        else
        {
            Debug.Log("failed to hit a tile under the player");
        }
        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, TileBFSScript target)
    {
        foreach (GameObject tile in tiles)
        {
            TileBFSScript t = tile.GetComponent<TileBFSScript>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public void FindSelectableTiles()
    {
        foreach (TileBFSScript tile in selectableTiles){
            tile.Reset();
            tile.GetComponent<TileStatus>().Reset();
        }
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

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
        ClearSight.tilesToClear = selectableTiles;
    }

    public abstract void SetShowPath(bool value);


    /**
     * Questo è per A*, per adesso lo commento
     **/
    /* 

          protected TileBFSScript FindEndTile(TileBFSScript t)
     {
         Stack<TileBFSScript> tempPath = new Stack<TileBFSScript>();

         TileBFSScript next = t.GetParent();

         while (next != null)
         {
             tempPath.Push(next);
             next = next.GetParent();
         }

         if (tempPath.Count <= move)
         {
             return t.GetParent();
         }

         TileBFSScript endTile = null;
         for (int i = 0; i <= move; i++)
         {
             endTile = tempPath.Pop();
         }
         return endTile;
     }


         protected bool FindPath(Tile target)
     {
         ComputeAdjacencyLists(jumpHeight, target);
         GetCurrentTile();

         List<Tile> openList = new List<Tile>();
         List<Tile> closedList = new List<Tile>();

         openList.Add(currentTile);

         currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
         currentTile.f = currentTile.h; //g vale 0

         while (openList.Count > 0)
         {
             Tile t = findLowestF(openList);
             closedList.Add(t);

             if (t == target)
             {
                 actualTargetTile = FindEndTile(t);
                 MoveToTile(actualTargetTile);
                 return true;
             }

             foreach (Tile tile in t.adjacencyList)
             {
                 if (closedList.Contains(tile))
                 {
                     //do nothing, already processed
                 }
                 else if (openList.Contains(tile))
                 {
                     float tempg = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                     if (tempg < tile.g)
                     {
                         //found a faster way
                         tile.parent = t;
                         tile.g = tempg;
                         tile.f = tile.g + tile.h;
                     }
                 }
                 else
                 {
                     tile.parent = t;
                     tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                     tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                     tile.f = tile.g + tile.h;
                     openList.Add(tile);
                 }
             }

         }
         Debug.Log("Path not found");
         return false;
     }

     private Tile findLowestF(List<Tile> list)
     {
         Tile lowest = list[0];

         foreach (Tile tile in list)
         {
             if (tile.f < lowest.f)
             {
                 lowest = tile;
             }
         }

         list.Remove(lowest);
         return lowest;
     }*/

    public void MoveToTile(TileBFSScript tile)
    {
        path.Clear();
        tile.GetComponent<TileStatus>().SetTarget(true);
        moving = true;
        SetShowPath(false);

        TileBFSScript next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.GetParent();
        }
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

    public void DoReset()
    {
        moving = false;
        SetShowPath(false);
    }

    public void DoHighLightPathTo(TileStatus tile)
    {
        tile.SetTarget(true);
        moving = false;
        SetShowPath(true);

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

    public void HighlightPathTo(TileStatus tile)
    {
        Debug.Log("HighlightPathTo " + tile.name);
        ResetPath();
        DoHighLightPathTo(tile);
    }


    public void Move()
    {
        if (path.Count > 0)
        {
            TileBFSScript t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y; //per non finire sottoterra puntiamo SOPRA (metà altezza dell'omino più metà del tile)

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;
                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }


                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
            DecreaseActionPoints();
        }
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    protected void RemoveSelectableTiles()
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

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingToEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;
        CalculateHeading(target);
        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingToEdge = true;
            //posizione attuale + metà della posizione tra l'arrivo e la destinazione (bordo)
            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingToEdge = true;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;
            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;
        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingToEdge = false;
            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingToEdge = false;
            fallingDown = true;
            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    public void BeginTurn()
    {
        BeginTurn(true);
    }

    public void BeginTurn(bool resetActionPoints)
    {
        if (resetActionPoints || remainingActionPoints==0)
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

    public void InteractWithItem(Item item, int actionIndex)
    {
        float distance = Vector3.Distance(currentTile.transform.position, item.transform.position);
        if (distance<= interactionReach+item.GetInteractionReach())
        {
            interactingWithObject = true;
            //item.Interact(actionIndex); non compila più
            DecreaseActionPoints();
        }
        else
        {
            Debug.Log("item too far away: " + distance);
        }
       
    }

    private void DecreaseActionPoints()
    {
        remainingActionPoints--;
        if (remainingActionPoints == 0)
        {
            TurnManager.EndTurn(); 
        }
    }

    public bool IsInteractingWithObject()
    {
        return interactingWithObject;
    }

    public void SetInteractingWithObject(bool value)
    {
        interactingWithObject = value;
    }
}
