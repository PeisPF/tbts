using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : UserActionScript
{
    public abstract float GetInteractionReach();

    public string[] actions;
    public int[] actionCosts;
    public bool isHovered;
    public bool colorShouldChange;

    protected PlayerController unitThatTriggered;

    // Use this for initialization
    void Start()
    {
        actions = InitActions();
        actionCosts = InitActionCosts();
        isHovered = false;
        colorShouldChange = false;
    }

    public abstract int[] InitActionCosts();
    public abstract string[] InitActions();


    // Update is called once per frame
    protected virtual void Update()
    {
        if (colorShouldChange)
        {
            Debug.Log("changing color on " + this.name);
            Color color = Color.black;
            float emission = 1f;
            if (this.isHovered)
            {
                emission = 0.5f;
                color = Color.yellow;
            }
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color * emission);
            colorShouldChange = false;
        }

    }

    public void Interact(int index, PlayerController unitThatTriggered)
    {
        this.unitThatTriggered = unitThatTriggered;
        if (index == 0)
        {
            Interact1();
        }
        else if (index == 1)
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

    public virtual bool isActionPossible(PlayerActionScript player)
    {
        throw new NotImplementedException();
    }

    public void OnMouseEnter()
    {
        this.colorShouldChange = true;
        this.isHovered = true;
    }

    public void OnMouseExit()
    {
        this.colorShouldChange = true;
        this.isHovered = false;
    }

    public void ShowActionsToolTip()
    {

    }

    public override void DoRightClickAction()
    {
        ShowActionsToolTip();
    }

    public override void DoLeftClickAction()
    {
        throw new NotImplementedException();
    }

    public override void DoDoubleClickAction()
    {
        throw new NotImplementedException();
    }
}
