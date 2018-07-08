﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected bool selectionEnded;

    protected bool actionStarted;
    protected bool actionEnded;

    private AudioSource selectionSound;
    private AudioSource continuousSound;
    private AudioSource endActionSound;

    public Action(AudioSource selectionSound, AudioSource continuousSound, AudioSource endActionSound)
    {
        this.selectionSound = selectionSound;
        this.continuousSound = continuousSound;
        this.endActionSound = endActionSound;
    }

    protected virtual int GetCost()
    {
        return 1;
    }

    public bool IsSelectionEnded()
    {
        return selectionEnded;
    }

    protected abstract bool SelectionPhase(); //displays selection on screen

    protected virtual bool StartAction()
    {
        PlaySound(selectionSound);
        return true;
    }//performs the setup of the action

    private void StopSound(AudioSource sound)
    {
        if (sound!=null && sound.isPlaying)
        {
            sound.Stop();
        }
    }

    private void PlaySound(AudioSource sound)
    {
        if (sound != null && !sound.isPlaying)
        {
            sound.Play();
        }
    }

    protected virtual bool DoActualAction()
    {
        PlaySound(continuousSound);
        return true;
    }//plays the action continous phase

    protected virtual bool EndAction()
    {
        StopSound(continuousSound);
        PlaySound(endActionSound);
        return true;
    }//performs the cleanup after the action

    //method returns true when logic is over
    public bool DoAction()
    {
        if (!CheckForCancel())
        {
            if (!selectionEnded)
            {
                selectionEnded = SelectionPhase();
            }
            else
            {
                if (!actionStarted)
                {
                    actionStarted = StartAction();
                }
                else
                {
                    if (!actionEnded)
                    {
                        actionEnded = DoActualAction();
                    }
                    else
                    {
                        ConsumeActionPoints();
                        EndAction();
                        return true;
                    }
                }
            }
        }
        else {
            EndAction();
            return true;
        }
        
        return false;
    }

    protected void ConsumeActionPoints()
    {
        Debug.Log("Consuming " + GetCost() + " action points");
        GetPlayerActionScript().DecreaseActionPoints(GetCost());
    }

    private bool CheckForCancel()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cancelling action");
            return true;
        }
        return false;
    }



    //metodi e variabili di utilità per recuperare gli altri script attivi sull'unità

    private PlayerActionScript playerActionScript;
    private PlayerStatusScript playerStatusScript;
    private PlayerBFSScript playerBFSScript;

    protected PlayerActionScript GetPlayerActionScript()
    {
        if (this.playerActionScript == null)
        {
            this.playerActionScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerActionScript>();
        }
        return this.playerActionScript;
    }


    protected PlayerBFSScript GetPlayerBFSScript()
    {
        if (this.playerBFSScript == null)
        {
            this.playerBFSScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerBFSScript>();
        }
        return this.playerBFSScript;
    }

    protected PlayerStatusScript GetPlayerStatusScript()
    {
        if (this.playerStatusScript == null)
        {
            this.playerStatusScript = TurnManager.GetCurrentPlayer().GetComponent<PlayerStatusScript>();
        }
        return this.playerStatusScript;
    }



}
