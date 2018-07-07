using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGUIAction : GUIActionScript {
    protected override Action GetNewAction()
    {
        return new InteractAction();
    }
}
