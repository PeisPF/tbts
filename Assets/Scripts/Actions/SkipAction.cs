using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAction : Action {
    public SkipAction(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound) : base(selectionSound, continuousSound, endActionSound)
    {
    }

    protected override bool SelectionPhase()
    {
        return true;
    }


    
}
