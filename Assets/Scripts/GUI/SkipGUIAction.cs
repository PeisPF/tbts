using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipGUIAction : GUIActionScript
{
    protected override Action GetNewAction()
    {
        return new SkipAction(selectionSound, continuousSound, endActionSound);
    }
}
