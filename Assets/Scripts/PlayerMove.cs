using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{

    float lastClickTime;
    float doubleClickDelay = 0.25f;
    private bool showingPath = false;
    private bool openingADoor = false;

    public GUIElement actionPanel;

    private Tile pathDestination;

    public LayerMask layerMask;

    public LayerMask fogAndWalls;

    public LayerMask lineOfSightObstructing;

    private Transform rayCastTarget;

    private bool checkedFogInCurrentPosition = false;



    // Use this for initialization
    void Start()
    {
        Init();
        rayCastTarget = new GameObject().transform;
    }

    public override void SetShowPath(bool value)
    {
        showingPath = value;
    }

    private void CheckFog()
    {
        List<RaycastHit> actualHits = new List<RaycastHit>();
        rayCastTarget.position = this.transform.position + new Vector3(0.1f, 0, 0);
        for (int i =0; i < 360; i+=1)
        {
            rayCastTarget.RotateAround(this.transform.position, new Vector3(0, 1, 0), ((float)i));
            actualHits.AddRange(RayCastUtils.RaycastTo(this.transform.position, rayCastTarget.position, fogAndWalls, lineOfSightObstructing, float.PositiveInfinity));
            //Debug.Log("rotated to "+rayCastTarget.position);
        }
        //casto un raggio anche verso il giocatore, per eliminare la nebbia su di lui
        rayCastTarget.position = this.transform.position + new Vector3(0, 2, 0);
        actualHits.AddRange(RayCastUtils.RaycastTo(rayCastTarget.position, this.transform.position, fogAndWalls, lineOfSightObstructing, 1));
        //Debug.Log("CheckFog hit " + actualHits.Count + " items");
        foreach (RaycastHit hit in actualHits){
            Destroy(hit.collider.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }
        else
        {
            if (!checkedFogInCurrentPosition)
            {
                CheckFog();
                checkedFogInCurrentPosition = true;
            }
            if (!moving && !showingPath)
            {
                CheckMouse();
                FindSelectableTiles();
            }

            else
            {
                if (!showingPath)
                {
                    Move();
                    checkedFogInCurrentPosition = false;
                }
                else
                {
                    CheckMouse();
                }
            }
        }
    }

    void CheckHighlightPath(RaycastHit hit)
    {
        Tile t = hit.collider.GetComponent<Tile>();
        if (t.selectable)
        {
            pathDestination = t;
            HighlightPathTo(t);
        }

    }

    void ShowActionsToolTip(RaycastHit hit)
    {

    }

    Ray getActualRay(Vector3 tap_position)
    {
        Ray result = Camera.main.ScreenPointToRay(tap_position);
        return result; 
    }

    void DoAction(bool pathLit)
    {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = getActualRay(Input.mousePosition);

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
                    openingADoor = true;
                    DoInteract(hit, 0);
                }
            }
            else
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.path)
                    {
                        MoveToTile(pathDestination);
                    }
                }
            }
        }
        checkedFogInCurrentPosition = false;
    }

    private void SwitchTurn(RaycastHit hit)
    {
        PlayerMove p = hit.collider.GetComponent<PlayerMove>();
        if (p != this)
        {
            ResetPath();
            TurnManager.SwitchTurn(p);
        }
        else
        {
        }

    }

    void DoInteract(RaycastHit hit, int index)
    {
        Item t = hit.collider.GetComponent<Item>();
        InteractWithItem(t, index);
    }


    void DoMove(RaycastHit hit)
    {
        Tile t = hit.collider.GetComponent<Tile>();
        if ((t.path && showingPath) || (t.selectable && !showingPath))
        {
            MoveToTile(t);
        }
    }

    void DoRightMouseButton()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = getActualRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask.value))
        {
            Debug.Log("hit " + hit.collider.tag);
            if (hit.collider.tag == "Tile")
            {
                CheckHighlightPath(hit);
            }
            else if (hit.collider.tag == "Items")
            {
                if (!showingPath)
                {
                    ResetPath();
                    ShowActionsToolTip(hit);
                }
            }
        }
    }



    void CheckMouse()
    {

        if (Input.GetMouseButtonUp(1))
        {
            DoRightMouseButton();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (showingPath)
            {
                DoAction(true);
            }
            else
            {
                if (Time.time - lastClickTime < doubleClickDelay)
                {
                    //double click
                    DoAction(false);
                }
                else
                {
                    //normal click
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Ray ray = getActualRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask.value))
                    {
                        Debug.Log("hit " + hit.collider.tag);
                        if (hit.collider.tag == "Player")
                        {
                            SwitchTurn(hit);
                        }
                    }
                }
                lastClickTime = Time.time;
            }
        }
    }

    public bool isMoving() {
        return moving;
    }

    public bool isOpeningADoor() {
        return openingADoor;
    }

    public void setOpeningADoor(bool x)
    {
        openingADoor = x;
    }

}
