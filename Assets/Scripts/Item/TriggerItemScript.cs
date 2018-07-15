using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerItemScript : Item {

    public Item itemTriggered; //l'oggetto che viene attivato
    public int triggeredItemInteractionIndex; //l'indice nella lista di azioni possibili per l'oggetto attivato

    public bool activated = false;
    public bool moving = false;

    public override string[] InitActions()
    {
        //successivamente leggeremo da un file di configurazione
        return new string[] { "Use"};
    }
    public override int[] InitActionCosts()
    {
        //successivamente leggeremo da un file di configurazione
        return new int[] { 1 };
    }

    public override void Interact1(ActionWithCallback action)
    {
        Debug.Log("interacted with " + this.name);
        activated = !activated;
        moving = true;
        itemTriggered.Interact(triggeredItemInteractionIndex, null, action);
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
