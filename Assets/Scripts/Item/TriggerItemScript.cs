using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerItemScript : Item {

    public Item itemTriggered; //l'oggetto che viene attivato

    public bool activated = false;
    public bool moving = false;


    public override void Interact()
    {
        Debug.Log("interacted with " + this.name);
        activated = !activated;
        moving = true;
        itemTriggered.Interact();
    }

    public abstract void ShowAsDisactivated();
    public abstract void ShowAsActivated();
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (moving)
        {
            if (activated)
            {
                ShowAsActivated();
            }
            else
            {
                ShowAsDisactivated();
            }
        }
        moving = false;
        

    }
}
