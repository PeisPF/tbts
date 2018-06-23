﻿using System;
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

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public override void SetShowPath(bool value)
    {
        showingPath = value;
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
       /* Debug.DrawLine(result.origin, result.direction*20000, Color.white, 20f);
        Vector3 modifiedTarget = Camera.main.ScreenToViewportPoint(new Vector3(result.direction.x,result.direction.y, 0.5f- transform.position.z));

        Debug.DrawLine(result.origin, modifiedTarget * 20000, Color.red, 20f);*/
        return result; //impreciso, più o meno di mezzo tile
       
        
        /*  Debug.Log("tap position: " + tap_position);
          Vector3 relativePosition =    Camera.main.transform.position- tap_position;
          Debug.Log("touch position modified: " + relativePosition);
          Debug.DrawRay(Camera.main.transform.position, relativePosition, Color.white, 20);
          return new Ray(Camera.main.transform.position, relativePosition);
          */

        /*  Debug.Log("tap position: "+tap_position);
          Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(tap_position.x, tap_position.y, TurnManager.GetCurrentPlayer().transform.position.z - transform.position.z));
          Debug.Log("touch position modified: " + touchPos);
          Ray ray = new Ray(Camera.main.transform.position, (touchPos - Camera.main.transform.position).normalized);
          return ray;*/
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
