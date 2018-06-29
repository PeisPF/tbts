using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : UnitActionScript {
    private readonly float TARGET_DISTANCE_ERROR_THRESHDOLD = 0.05f;
    private readonly float MOVE_SPEED = 2;

    float halfHeight = 0;
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();//direzione in cui è girato il tizio
    

    public float jumpVelocity = 4.5f;
    public int interactionReach = 1;


    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingToEdge = false;
    Vector3 jumpTarget;

    public LayerMask layerMask;

    private TileBFSScript pathDestination;

    private PlayerStatusScript playerStatusScript;

    private PlayerStatusScript GetPlayerStatusScript()
    {
        if (playerStatusScript == null)
        {
            playerStatusScript = this.GetComponent<PlayerStatusScript>();
        }
        return playerStatusScript;
    }

    private PlayerBFSScript playerBFSScript;


    private PlayerBFSScript GetPlayerBFSScript()
    {
        if (playerBFSScript == null)
        {
            playerBFSScript = this.GetComponent<PlayerBFSScript>();
        }
        return playerBFSScript;
    }


    public void SetPathDestination(TileBFSScript dest)
    {
        this.pathDestination = dest;
    }

    private void Start()
    {
        Init();   
    }
    protected void Init()
    {
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        TurnManager.AddUnit(this);
    }

    public override void DoReset()
    {
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(false);
        //SetShowPath(false);
    }

    public void Move()
    {
        //
        Debug.Log("Move() called");
        if (GetPlayerBFSScript().GetPath().Count > 0)
        {
            TileBFSScript nextTileBFS = GetPlayerBFSScript().GetPath().Peek();
            Vector3 nextTilePosition = nextTileBFS.transform.position;
            Collider nextTileCollider = nextTileBFS.GetComponent<Collider>();

            nextTilePosition.y = CalculateFloorLevel(nextTilePosition, nextTileCollider); 

            if (IsDistantFromTargetAbove(nextTilePosition, TARGET_DISTANCE_ERROR_THRESHDOLD))
            {
                bool jump = this.transform.position.y != nextTilePosition.y;
                if (jump)
                {
                    Jump(nextTilePosition);
                }
                else
                {
                    CalculateHeading(nextTilePosition);
                    SetHorizontalVelocity();
                }


                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                TerminateMoveInExactPosition(nextTilePosition);
            }
        }
        else
        {
            EndAction(true);
        }
    }

    private void TerminateMoveInExactPosition(Vector3 nextTilePosition)
    {
        transform.position = nextTilePosition;
        GetPlayerBFSScript().GetPath().Pop();
    }

    private float CalculateFloorLevel(Vector3 nextTilePosition, Collider nextTileCollider)
    {
        //per non finire sottoterra puntiamo SOPRA (metà altezza dell'omino più metà del tile)
        return nextTilePosition.y + this.halfHeight + nextTileCollider.bounds.extents.y;
    }

    private bool IsDistantFromTargetAbove(Vector3 target, float threshold)
    {
        return Vector3.Distance(transform.position, target) >= threshold;
    }

    private void EndAction(bool consumeAction)
    {
        GetPlayerBFSScript().RemoveSelectableTiles();
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(false);
        if (consumeAction)
        {
            DecreaseActionPoints();
        }
    }

    public void DoAction(bool pathLit)
    {
        //Debug.Log("DoAction("+ pathLit + ") called");
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask.value))
        {
            Debug.Log("hit " + hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                SwitchTurn(hit);
                EndAction(false);
            }
            else if (!pathLit)
            {
                if (hit.collider.tag == "Tile")
                {
                    DoMove(hit);
                }
                else if (hit.collider.tag == "Items")
                {
                    DoInteract(hit, 0);
                }
            }
            else
            {
                if (hit.collider.tag == "Tile")
                {
                    TileStatus t = hit.collider.GetComponent<TileStatus>();
                    if (t.IsPath())
                    {
                        MoveToTile(pathDestination);
                    }
                }
            }
        }
        //checkedFogInCurrentPosition = false;
    }

    public void MoveToTile(TileBFSScript tile)
    {
        //Debug.Log("MoveToTile(" + tile + ") called");
        GetPlayerBFSScript().GetPath().Clear();
        Debug.Log("status "+ tile.GetComponent<TileStatus>());
        tile.GetComponent<TileStatus>().SetTarget(true);
        GetPlayerStatusScript().SetMoving(true);
        GetPlayerStatusScript().SetShowingPath(true);

        TileBFSScript next = tile;
        while (next != null)
        {
            GetPlayerBFSScript().GetPath().Push(next);
            next = next.GetParent();
        }
    }

    public void SwitchTurn(RaycastHit hit)
    {
        PlayerActionScript p = hit.collider.GetComponent<PlayerActionScript>();
        if (p != this)
        {
            EndAction(false);
            TurnManager.SwitchTurn(p);
        }
    }

    void DoInteract(RaycastHit hit, int index)
    {
        Item t = hit.collider.GetComponent<Item>();
        InteractWithItem(t, index);
    }


    void DoMove(RaycastHit hit)
    {
        //Debug.Log("DoMove(" + hit + ") called");
        TileStatus t = hit.collider.GetComponent<TileStatus>();
        if ((t.IsPath() && GetPlayerStatusScript().IsShowingPath()) || (t.IsSelectable() && !GetPlayerStatusScript().IsShowingPath()))
        {
            MoveToTile(t.GetComponent<TileBFSScript>());
        }
    }

    public void InteractWithItem(Item item, int actionIndex)
    {
        float distance = Vector3.Distance(GetPlayerBFSScript().GetCurrentTile().transform.position, item.transform.position);
        if (distance <= interactionReach + item.GetInteractionReach())
        {
            GetPlayerStatusScript().SetInteractingWithObject(true);
            item.Interact(actionIndex);
            EndAction(true);
        }
        else
        {
            Debug.Log("item too far away: " + distance);
        }
        
    }



    public void ShowActionsToolTip(RaycastHit hit)
    {

    }


    private void DecreaseActionPoints()
    {
        remainingActionPoints--;
        if (remainingActionPoints == 0)
        {
            TurnManager.EndTurn();
        }
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

            velocity = heading * MOVE_SPEED / 3.0f;

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



    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * MOVE_SPEED;
    }

    public override void DoRightClickAction()
    {
        throw new System.NotImplementedException();
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
