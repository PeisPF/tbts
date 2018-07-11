using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGUIAction : GUIActionScript
{
    public CPC_CameraPath path;

    protected override Action GetNewAction()
    {
        return new MoveAction(selectionSound, continuousSound, endActionSound, path);
    }
}
