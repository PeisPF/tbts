using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGUIAction : GUIActionScript
{
    protected override Action GetNewAction()
    {
        return new MoveAction(selectionSound, continuousSound, endActionSound);
    }
}
