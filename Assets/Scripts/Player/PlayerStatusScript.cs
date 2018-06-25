using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusScript : UnitStatusScript
{
    bool showingPath = false;

    public bool IsShowingPath()
    {
        return showingPath;
    }

    public void SetShowingPath(bool showingPath)
    {
        this.showingPath = showingPath;
    }
}
