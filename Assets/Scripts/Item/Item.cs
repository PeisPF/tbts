using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract float GetInteractionReach();

    public int actionCost;
    public bool isHovered;
    public bool colorShouldChange;

    private Color color;
    private float emission;

    protected NewPlayerController unitThatTriggered;

    // Use this for initialization
    void Start()
    {
        actionCost = GetActionCost();
        isHovered = false;
        colorShouldChange = false;
        color = Color.black;
        emission = 1.0f;
    }

    public virtual int GetActionCost()
    {
        return 1;
    }
    
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
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color * emission);
            colorShouldChange = false;
        }

    }

    //usato per interazione diretta
    public void Interact(NewPlayerController unitThatTriggered)
    {
        this.unitThatTriggered = unitThatTriggered;
        this.Interact();
    }

    //usato per interazione da trigger
    public abstract void Interact();    

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

}
