using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnGUIAction : GUIActionScript
{
    protected override Action GetNewAction()
    {
        UnitFrameGUIScript script = this.transform.parent.GetComponent<UnitFrameGUIScript>();
        PlayerStatusScript statusScript = script.GetPlayer();
        PlayerActionScript actionScript = statusScript.GetComponent<PlayerActionScript>();

        return new SwitchTurnAction(selectionSound, continuousSound, endActionSound, actionScript);
    }
}
