using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public abstract float GetInteractionReach();

    public string[] actions;
    public int[] actionCosts;

	// Use this for initialization
	void Start () {
        actions = InitActions();
        actionCosts = InitActionCosts();
	}

    public abstract int[] InitActionCosts();
    public abstract string[] InitActions();

	// Update is called once per frame
	void Update () {
		
	}

    public void Interact(int index)
    {
        if (index == 0)
        {
            Interact1();
        }
        else if (index ==1)
        {
            Interact2();
        }
        else if (index == 2)
        {
            Interact3();
        }
        else if (index == 3)
        {
            Interact4();
        }
        else if (index == 4)
        {
            Interact5();
        }
    }

    public virtual void Interact1()
    {
        throw new NotImplementedException();
    }
    public virtual void Interact2()
    {
        throw new NotImplementedException();
    }
    public virtual void Interact3()
    {
        throw new NotImplementedException();
    }
    public virtual void Interact4()
    {
        throw new NotImplementedException();
    }
    public virtual void Interact5()
    {
        throw new NotImplementedException();
    }
}
