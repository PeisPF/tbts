using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : TriggerItemScript {
    //public float interactionReach = 1.0f;
    public Transform fulcrum;

    public override float GetInteractionReach()
    {
        return 1.1f;
    }

    public override void ShowAsDisactivated()
    {
        transform.RotateAround(fulcrum.position, transform.right.normalized, 60);
    }

    public override void ShowAsActivated()
    {
        /*Debug.Log("forward: "+transform.forward);
        Debug.Log("up: " + transform.up);
        Debug.Log("right: " + transform.right);*/
        
        transform.RotateAround(fulcrum.position, transform.right.normalized, -60);
    }

    void Start()
    {
        transform.RotateAround(fulcrum.position, transform.right.normalized, 30);
    }

    public override bool isActionPossible(PlayerActionScript player)
    {
        return true;
    }
}
