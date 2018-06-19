using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Item {

    public bool open = false;
    public bool moving = false;

    private Vector3 axisOpen = new Vector3(0,0,-0.5f);

    private Vector3 originalTransformPosition;

	// Use this for initialization
	void Start () {
        originalTransformPosition = transform.position;
	}
	

	// Update is called once per frame
	void Update () {
		if (moving)
        {
            if (open)
            {
                transform.RotateAround(originalTransformPosition + axisOpen, new Vector3(0.0f, -1.0f, 0.0f), 90);
                moving = false;
                open = false;
            }
            else
            {
                transform.RotateAround(originalTransformPosition + axisOpen, new Vector3(0.0f, 1.0f, 0.0f), 90);
                moving = false;
                open = true;
            }
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
        return 1.0f;
    }
}
