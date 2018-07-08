using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : UnitActionScript
{
    private readonly float TARGET_DISTANCE_ERROR_THRESHDOLD = 0.05f;
    private readonly float MOVE_SPEED = 2;

    float halfHeight = 0;
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();//direzione in cui è girato il tizio

    private Action[] availableActions;

    public Action[] GetAvailableAction()
    {
        return this.availableActions;
    }
    public void SetAvailableActions(Action[] availableActions)
    {
        this.availableActions = availableActions;
    }

    public float jumpVelocity = 4.5f;
    private float interactionReach;


    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingToEdge = false;
    Vector3 jumpTarget;

    public LayerMask layerMask;

    public LayerMask moveLayerMask;
    public LayerMask interactLayerMask;

    private TileBFSScript pathDestination;

    public TileBFSScript GetPathDestionation()
    {
        return this.pathDestination;
    }

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

    private PlayerController playerController;

    public void SetInteractionReach(float interactionReach)
    {
        this.interactionReach = interactionReach;
    }

    public float GetInteractionReach()
    {
        return this.interactionReach;
    }

    private PlayerController GetPlayerController()
    {
        if (playerController == null)
        {
            playerController = this.GetComponent<PlayerController>();
        }
        return playerController;
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

    public bool Move()
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
            //EndAction(true);
            Debug.Log("Move ended");
            return true;
        }
        return false;
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

    /*private void EndAction(bool consumeAction)
    {
        GetPlayerBFSScript().RemoveSelectableTiles();
        GetPlayerStatusScript().SetMoving(false);
        GetPlayerStatusScript().SetShowingPath(false);
        if (consumeAction)
        {
            DecreaseActionPoints();
        }
    }*/

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
                //EndAction(false);
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
    }

    public bool MoveToTile(TileBFSScript tile)
    {
        //Debug.Log("MoveToTile(" + tile + ") called");
        GetPlayerBFSScript().GetPath().Clear();
        Debug.Log("status " + tile.GetComponent<TileStatus>());
        tile.GetComponent<TileStatus>().SetTarget(true);
        GetPlayerStatusScript().SetMoving(true);
        GetPlayerStatusScript().SetShowingPath(true);

        TileBFSScript next = tile;
        while (next != null)
        {
            GetPlayerBFSScript().GetPath().Push(next);
            next = next.GetParent();
        }
        return true;
    }

    public void SwitchTurn(RaycastHit hit)
    {
        PlayerActionScript p = hit.collider.GetComponent<PlayerActionScript>();
        if (p != this)
        {
            //EndAction(false);
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

    public bool IsInteractionPossible(Item item)
    {
        Vector3 currentTilePosition = GetPlayerBFSScript().GetCurrentTile().transform.position;
        Vector3 itemPosition = item.transform.position;
        float distanceFromItem = Vector3.Distance(currentTilePosition, itemPosition);
        if (distanceFromItem <= GetInteractionReach() + item.GetInteractionReach())
        {
            if (item.isActionPossible(this))
            {
                return true;
            }
        }
        return false;

    }

    public bool InteractWithItem(Item item, int actionIndex)
    {
        Vector3 currentTilePosition = GetPlayerBFSScript().GetCurrentTile().transform.position;
        Vector3 itemPosition = item.transform.position;
        float distanceFromItem = Vector3.Distance(currentTilePosition, itemPosition);
        if (IsInteractionPossible(item))
        {
            GetPlayerStatusScript().SetInteractingWithObject(true);
            TurnPlayerTo(itemPosition);
            item.Interact(actionIndex, this.GetPlayerController());
            return true;
        }
        else
        {
            Debug.Log("item too far away: " + distanceFromItem);
        }
        return false;
    }


    private void TurnPlayerTo(Vector3 itemPosition)
    {
        Vector3 lookPos = itemPosition - this.transform.position;
        lookPos.y = 0;
        this.heading = lookPos;
        this.transform.forward = lookPos;
    }



    public void ShowActionsToolTip(RaycastHit hit)
    {

    }

    public void DecreaseActionPoints(int amount)
    {
        remainingActionPoints -= amount;
        if (remainingActionPoints == 0)
        {
            TurnManager.EndTurn();
        }
    }
    /*private void DecreaseActionPoints()
    {
        DecreaseActionPoints(1);
    }*/

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
        Debug.Log("Calculate heading to " + target);
        Vector3 orientationVector = target - this.transform.position;
        heading = orientationVector;
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

    public bool actionIsPossible(Item item)
    {
        bool possible = true;
        if (item.GetType().ToString() == "DoorScript")
        {
            DoorScript door = (DoorScript)item;
            if (door.open && Vector3.Distance(door.fulcrum.position, this.transform.position) < 0.6f)
            {
                possible = false;
            }
            else { possible = true; }
        }

        return possible;
    }
}
