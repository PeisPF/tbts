using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticsMove : MonoBehaviour
{
    public bool turn = false;
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    public bool moving = false;
    //public bool showingPath = false;

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

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();//direzione in cui è girato il tizio

    public LayerMask onlyTilesLayerMask;

    public Tile actualTargetTile;


    float halfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;//Physics.Raycast(target.transform.position, -Vector3.up, out hit, onlyTilesLayerMask)
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, onlyTilesLayerMask, onlyTilesLayerMask.value))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        else
        {
            Debug.Log("failed to hit a tile under the player");
        }
        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.visited = true;
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = t.distance + 1;
                        process.Enqueue(tile);
                    }
                }
            }
        }
        ClearSight.tilesToClear = selectableTiles;
    }

    public abstract void SetShowPath(bool value);

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;

        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
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
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;
        SetShowPath(false);

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void ResetPath()
    {
        foreach (Tile t in path)
        {
            t.path = false;
        }

        path.Clear();
    }

    public void DoReset()
    {
        moving = false;
        SetShowPath(false);
    }

    public void DoHighLightPathTo(Tile tile)
    {
        tile.target = true;
        moving = false;
        SetShowPath(true);

        Tile next = tile;
        while (next != null)
        {
            next.path = true;
            next.target = false;
            path.Push(next);
            next = next.parent;
        }
    }

    public void HighlightPathTo(Tile tile)
    {
        ResetPath();
        DoHighLightPathTo(tile);
    }


    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
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
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile t in selectableTiles)
        {
            t.Reset(true);
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
            item.Interact(actionIndex);
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
            TurnManager.EndTurn(); //modificare qui se vogliamo aggiungere altre azioni
        }
    }
}
