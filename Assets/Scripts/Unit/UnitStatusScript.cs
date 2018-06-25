using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusScript : MonoBehaviour {
    private bool moving = false;
    private bool interactingWithObject = false;

    public bool IsMoving()
    {
        return moving;
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    public void SetInteractingWithObject(bool value)
    {
        this.interactingWithObject = value;
    }
}
