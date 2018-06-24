using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Item
{

    public bool open = false;
    public bool moving = false;

    public bool allowedLocalOpen = true;

    //private Vector3 axisOpen = new Vector3(0, 0, -0.5f);

    public Transform fulcrum;

    private Vector3 originalTransformPosition;

    // Use this for initialization
    void Start()
    {
        originalTransformPosition = transform.position;
    }

    float endrot = 0;
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            float rot = Time.deltaTime * 180;
            if (endrot + rot > 90)
            {
                rot = 90 - endrot;
            }
            endrot += rot;
            if (open)
            {
                transform.RotateAround(fulcrum.position, new Vector3(0.0f, -1.0f, 0.0f), rot);
                if (endrot >= 90)
                {
                    moving = false;
                    open = false;
                    endrot = 0;
                }
            }
            else
            {
                transform.RotateAround(fulcrum.position, new Vector3(0.0f, 1.0f, 0.0f), rot);
                if (endrot >= 90)
                {
                    moving = false;
                    open = true;
                    endrot = 0;
                }
            }
        }
        if (TurnManager.GetCurrentPlayer()!=null && TurnManager.GetCurrentPlayer().GetComponent<PlayerMove>() != null)
        {
            TurnManager.GetCurrentPlayer().GetComponent<PlayerMove>().SetCheckedFogInCurrentPosition(false);
        }
    }

    public override string[] InitActions()
    {
        //successivamente leggeremo da un file di configurazione
        return new string[] { "Open/Close", "Action2" };
    }
    public override int[] InitActionCosts()
    {
        //successivamente leggeremo da un file di configurazione
        return new int[] { 1, 20 };
    }


    public override void Interact1()
    {
        this.moving = true;
    }

    public override float GetInteractionReach()
    {
        if (allowedLocalOpen)
        {
            return 1.0f;
        }
        else
        {
            return 0f;
        }
    }
}
