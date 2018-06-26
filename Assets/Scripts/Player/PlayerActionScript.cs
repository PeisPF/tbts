using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : UnitActionScript {

    float halfHeight = 0;
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();//direzione in cui è girato il tizio
    
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;
    public int interactionReach = 1;


    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingToEdge = false;
    Vector3 jumpTarget;

    public LayerMask layerMask;

    private TileBFSScript pathDestination;

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
        this.GetComponent<PlayerStatusScript>().SetMoving(false);
        this.GetComponent<PlayerStatusScript>().SetShowingPath(false);
        //SetShowPath(false);
    }

    public void Move()
    {
        if (this.GetComponent<PlayerBFSScript>().GetPath().Count > 0)
        {
            TileBFSScript t = this.GetComponent<PlayerBFSScript>().GetPath().Peek();
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
                this.GetComponent<PlayerBFSScript>().GetPath().Pop();
            }
        }
        else
        {
            this.GetComponent<PlayerBFSScript>().RemoveSelectableTiles();
            this.GetComponent<PlayerStatusScript>().SetMoving(false);
            DecreaseActionPoints();
        }
    }

    public void DoAction(bool pathLit)
    {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask.value))
        {
            Debug.Log("hit " + hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                SwitchTurn(hit);
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
        this.GetComponent<PlayerBFSScript>().GetPath().Clear();
        Debug.Log("status "+ tile.GetComponent<TileStatus>());
        tile.GetComponent<TileStatus>().SetTarget(true);
        this.GetComponent<PlayerStatusScript>().SetMoving(true);
        this.GetComponent<PlayerStatusScript>().SetShowingPath(true);

        TileBFSScript next = tile;
        while (next != null)
        {
            this.GetComponent<PlayerBFSScript>().GetPath().Push(next);
            next = next.GetParent();
        }
    }

    public void SwitchTurn(RaycastHit hit)
    {
        PlayerActionScript p = hit.collider.GetComponent<PlayerActionScript>();
        if (p != this)
        {
            this.GetComponent<PlayerBFSScript>().ResetPath();
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
        TileStatus t = hit.collider.GetComponent<TileStatus>();
        if ((t.IsPath() && this.GetComponent<PlayerStatusScript>().IsShowingPath()) || (t.IsSelectable() && !this.GetComponent<PlayerStatusScript>().IsShowingPath()))
        {
            MoveToTile(t.GetComponent<TileBFSScript>());
        }
    }

    public void InteractWithItem(Item item, int actionIndex)
    {
        float distance = Vector3.Distance(this.GetComponent<PlayerBFSScript>().GetCurrentTile().transform.position, item.transform.position);
        if (distance <= interactionReach + item.GetInteractionReach())
        {
            this.GetComponent<PlayerStatusScript>().SetInteractingWithObject(true);
            item.Interact(actionIndex);
            DecreaseActionPoints();
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



    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

}
