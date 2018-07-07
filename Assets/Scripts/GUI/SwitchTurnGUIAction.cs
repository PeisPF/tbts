using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnGUIAction : GUIActionScript
{
    protected override Action GetNewAction()
    {
        UnitFrameGUIScript script = this.transform.parent.GetComponent<UnitFrameGUIScript>();
        Debug.Log("script: " + script);
        PlayerStatusScript statusScript = script.GetPlayer();
        Debug.Log("statusScript: " + statusScript);
        PlayerActionScript actionScript = statusScript.GetComponent<PlayerActionScript>();
        Debug.Log("actionScript: " + actionScript);

        return new SwitchTurnAction(actionScript);
    }
}
