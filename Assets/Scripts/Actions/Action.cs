using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected bool selectionEnded;

    protected bool actionStarted;
    protected bool actionEnded;

    protected abstract bool SelectionPhase(); //displays selection on screen

    protected abstract bool StartAction(); //performs the setup of the action

    protected abstract bool DoActualAction(); //plays the action continous phase

    protected abstract bool EndAction(); //performs the cleanup after the action

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
                        EndAction();
                        return true;
                    }
                }
            }
        }
        else {
            selectionEnded= true;
            actionEnded = true;
        }
        
        return false;
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



}
