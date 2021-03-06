﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusScript : UnitStatusScript
{
    private Stack<TileBFSScript> selectedPath = new Stack<TileBFSScript>();

    bool showingPath = false;

    public bool IsShowingPath()
    {
        return showingPath;
    }

    public void SetShowingPath(bool showingPath)
    {
        this.showingPath = showingPath;
    }

    public Stack<TileBFSScript> GetSelectedPath()
    {
        return selectedPath;
    }
}
