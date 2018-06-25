using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private bool checkedFogInCurrentPosition = false;

    private PlayerStatusScript playerStatus;

    //public LayerMask layerMask;

    float lastClickTime;
    float doubleClickDelay = 0.25f;



    public void SetCheckedFogInCurrentPosition(bool value)
    {
        this.checkedFogInCurrentPosition = value;
    }

    private PlayerStatusScript GetPlayerStatusScript()
    {
        if (playerStatus == null)
        {
            playerStatus = this.GetComponent<PlayerStatusScript>();
        }
        return playerStatus;
    }

    // Use this for initialization
    void Start () {
		
	}

    


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (TurnManager.GetCurrentPlayer() == this.GetComponent<Collider>())
        {
            if (!checkedFogInCurrentPosition)
            {
                this.GetComponent<CheckFogScript>().CheckFog();
                checkedFogInCurrentPosition = true;
            }

            if (!GetPlayerStatusScript().IsMoving()&& !GetPlayerStatusScript().IsShowingPath())
            {
                CheckMouse();
                //BFSUtils.FindSelectableTiles(allTiles, selectableTiles, currentTile, jumpHeight, move);
                this.GetComponent<PlayerBFSScript>().FindSelectableTiles();
            }

            else
            {
                if (!GetPlayerStatusScript().IsShowingPath())
                {
                    this.GetComponent<PlayerActionScript>().Move();
                    checkedFogInCurrentPosition = false;
                }
                else
                {
                    CheckMouse();
                }
            }
        }
    }


    void DoRightMouseButton()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, this.GetComponent<PlayerActionScript>().layerMask.value))
        {
            Debug.Log("hit " + hit.collider.tag);
            if (hit.collider.tag == "Tile")
            {
                this.GetComponent<HighLightPathScript>().CheckHighlightPath(hit);
            }
            else if (hit.collider.tag == "Items")
            {
                if (!GetPlayerStatusScript().IsShowingPath())
                {
                    this.GetComponent<PlayerBFSScript>().ResetPath();
                    this.GetComponent<PlayerActionScript>().ShowActionsToolTip(hit);
                }
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
            if (GetPlayerStatusScript().IsShowingPath())
            {
                this.GetComponent<PlayerActionScript>().DoAction(true);
            }
            else
            {
                if (Time.time - lastClickTime < doubleClickDelay)
                {
                    //double click
                    this.GetComponent<PlayerActionScript>().DoAction(false);
                }
                else
                {
                    //normal click
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, float.PositiveInfinity, this.GetComponent<PlayerActionScript>().layerMask.value))
                    {
                        Debug.Log("hit " + hit.collider.tag);
                        if (hit.collider.tag == "Player")
                        {
                            this.GetComponent<PlayerActionScript>().SwitchTurn(hit);
                        }
                    }
                }
                lastClickTime = Time.time;
            }
        }
    }

}
