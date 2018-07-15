using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionWithCallback : Action {
    private bool isOver;

    public ActionWithCallback(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound) : base(selectionSound, continuousSound, endActionSound)
    {
        isOver = false;
    }

    public void SetOver()
    {
        this.isOver = true;
    }

    public override bool IsOver()
    {
        return this.isOver;
    }
}
