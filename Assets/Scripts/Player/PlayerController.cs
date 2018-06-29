using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private bool checkedFogInCurrentPosition = false;

    

    //public LayerMask layerMask;

    float lastClickTime;
    float doubleClickDelay = 0.25f;

    private PlayerStatusScript playerStatusScript;

    private PlayerStatusScript GetPlayerStatusScript()
    {
        if (playerStatusScript == null)
        {
            playerStatusScript = this.GetComponent<PlayerStatusScript>();
        }
        return playerStatusScript;
    }

    private CheckFogScript checkFogScript;

    private CheckFogScript GetCheckFogScript()
    {
        if (checkFogScript == null)
        {
            checkFogScript = this.GetComponent<CheckFogScript>();
        }
        return checkFogScript;
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

    private PlayerActionScript playerActionScript;

    private PlayerActionScript GetPlayerActionScript()
    {
        if (playerActionScript == null)
        {
            playerActionScript = this.GetComponent<PlayerActionScript>();
        }
        return playerActionScript;
    }


    public void SetCheckedFogInCurrentPosition(bool value)
    {
        this.checkedFogInCurrentPosition = value;
    }

    

    // Use this for initialization
    void Start () {
		
	}

    private void CheckFog()
    {
        if (!checkedFogInCurrentPosition)
        {
            GetCheckFogScript().CheckFog();
            checkedFogInCurrentPosition = true;
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward);

        if (TurnManager.GetCurrentPlayer() == this.GetComponent<Collider>())
        {
            CheckFog();



            //Debug.Log("moving: " + GetPlayerStatusScript().IsMoving()+", showingPath: " + GetPlayerStatusScript().IsShowingPath());

            //if the player is not moving and the path is not chosen, highlight selectable tiles and wait for input
            if (!GetPlayerStatusScript().IsMoving()&& !GetPlayerStatusScript().IsShowingPath())
            {
                GetPlayerBFSScript().FindSelectableTiles();
                CheckMouse();
            }

            else
            {
                
                if (GetPlayerStatusScript().IsMoving())
                {

                    //move
                    GetPlayerActionScript().Move();
                    checkedFogInCurrentPosition = false;
                }
                else
                {
                    //wait for input
                    CheckMouse();
                }
            }
        }
    }


    void DoRightMouseButton()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GetPlayerActionScript().layerMask.value))
        {
            Debug.Log("hit " + hit.collider.tag);
            UserActionScript userActionScript = hit.collider.GetComponent<UserActionScript>();
            if (userActionScript != null)
            {
                if (userActionScript is HighLightPathScript)
                {
                    ((HighLightPathScript)userActionScript).SetPlayerStatusScript(GetPlayerStatusScript());
                }
                userActionScript.DoRightClickAction();
            }
        }
    }



    private void CheckMouse()
    {

        if (Input.GetMouseButtonUp(1))
        {
            DoRightMouseButton();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DoLeftMouseButton();
        }
    }

    private void DoLeftMouseButton()
    {
        if (GetPlayerStatusScript().IsShowingPath())
        {
            GetPlayerActionScript().DoAction(true);
        }
        else
        {
            if (Time.time - lastClickTime < doubleClickDelay)
            {
                //double click
                GetPlayerActionScript().DoAction(false);
            }
            else
            {
                //normal click
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GetPlayerActionScript().layerMask.value))
                {
                    Debug.Log("hit " + hit.collider.tag);
                    if (hit.collider.tag == "Player")
                    {
                        GetPlayerActionScript().SwitchTurn(hit);
                    }
                }
            }
            lastClickTime = Time.time;
        }
    }
}
