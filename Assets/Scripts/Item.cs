using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public abstract float GetInteractionReach();

    public string[] actions;
    public int[] actionCosts;
    public bool isHovered;

    // Use this for initialization
    void Start () {
        actions = InitActions();
        actionCosts = InitActionCosts();
        isHovered = false;
	}

    public abstract int[] InitActionCosts();
    public abstract string[] InitActions();

	// Update is called once per frame
	void Update () {
        
        if(this.isHovered)
        {
            
            float emission = 0.5f;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow * emission);
        }
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

    public void OnMouseOver(){
        Debug.Log("onmouseover" + this.name);
        this.isHovered = true;
    }
}
