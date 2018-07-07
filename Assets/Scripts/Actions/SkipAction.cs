using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAction : Action {
    protected override bool DoActualAction()
    {
        return true;
    }

    protected override bool EndAction()
    {
        return true;
    }

    protected override bool SelectionPhase()
    {
        return true;
    }

    protected override bool StartAction()
    {
        return true;
    }

    
}
