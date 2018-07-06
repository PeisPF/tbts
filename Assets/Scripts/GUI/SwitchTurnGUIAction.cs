using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnGUIAction : GUIActionScript
{
    protected override Action GetNewAction()
    {
        return new SwitchTurnAction();
    }
}
