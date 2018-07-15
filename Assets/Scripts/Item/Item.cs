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

    private Color color;
    private float emission;

    protected PlayerController unitThatTriggered;

    // Use this for initialization
    void Start()
    {
        actions = InitActions();
        actionCosts = InitActionCosts();
        isHovered = false;
        colorShouldChange = false;
        color = Color.black;
        emission = 1.0f;
    }

    public abstract int[] InitActionCosts();
    public abstract string[] InitActions();

    public void ChangeColor(Color color, float emission)
    {
        Debug.Log("setting color to: " + color);
        this.colorShouldChange = true;
        this.color = color;
        this.emission = emission;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (colorShouldChange)
        {
            /*Debug.Log("changing color on " + this.name);
            Color color = Color.black;
            float emission = 1f;
            if (this.isHovered)
            {
                emission = 0.5f;
                color = Color.yellow;
            }*/
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color * emission);
            colorShouldChange = false;
        }

    }

    public void Interact(int index, PlayerController unitThatTriggered, ActionWithCallback action)
    {
        this.unitThatTriggered = unitThatTriggered;
        if (index == 0)
        {
            Interact1(action);
        }
        else if (index == 1)
        {
            Interact2(action);
        }
        else if (index == 2)
        {
            Interact3(action);
        }
        else if (index == 3)
        {
            Interact4(action);
        }
        else if (index == 4)
        {
            Interact5(action);
        }
    }

    public virtual void Interact1(ActionWithCallback action)
    {
        throw new NotImplementedException();
    }
    public virtual void Interact2(ActionWithCallback action)
    {
        throw new NotImplementedException();
    }
    public virtual void Interact3(ActionWithCallback action)
    {
        throw new NotImplementedException();
    }
    public virtual void Interact4(ActionWithCallback action)
    {
        throw new NotImplementedException();
    }
    public virtual void Interact5(ActionWithCallback action)
    {
        throw new NotImplementedException();
    }

    public virtual bool isActionPossible(PlayerActionScript player)
    {
        throw new NotImplementedException();
    }

    public bool ShouldChangeColor()
    {
        Action currentAction = TurnManager.GetCurrentPlayer().GetComponent<NewPlayerController>().GetCurrentAction();
        return currentAction is InteractAction && !currentAction.IsSelectionEnded();
    }

    public void OnMouseEnter()
    {
        this.colorShouldChange = ShouldChangeColor();
        this.color = Color.yellow;
        this.emission = 0.5f;
    }

    public void OnMouseExit()
    {
        this.colorShouldChange = ShouldChangeColor();
        this.color = Color.black;
        this.emission = 1f;
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
