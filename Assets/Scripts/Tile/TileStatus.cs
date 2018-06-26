using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour {

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool path = false;


    public bool IsCurrent()
    {
        return current;
    }

    public bool IsWalkable()
    {
        return walkable;
    }

    public bool IsTarget()
    {
        return target;
    }

    public bool IsSelectable()
    {
        return selectable;
    }

    public bool IsPath()
    {
        return path;
    }

    


    private TileMaterialScript materialScript;

    public void Reset()
    {
        Reset(false);
    }

    public void Reset(bool alsoPath)
    {
        //Debug.Log("Reset: " + alsoPath);
        current = false;
        target = false;
        selectable = false;
        if (alsoPath)
        {
            path = false;
        }
        UpdateMaterial();
    }

    private TileMaterialScript GetTileMaterialScript()
    {
        if (this.GetComponent<TileMaterialScript>() != null)
        {
            materialScript = this.GetComponent<TileMaterialScript>();
        }
        return materialScript;
    }

    private void Start()
    {
        //this.materialScript = ;   
    }

    public void SetWalkable(bool walkable)
    {
        this.walkable = walkable;
        UpdateMaterial();
    }

    public void SetCurrent(bool current)
    {
        this.current = current;
        UpdateMaterial();
    }

    public void SetTarget(bool target)
    {
        //Debug.Log("Set target to: " + this.name);
        this.target = target;
        UpdateMaterial();
    }

    public void SetSelectable(bool selectable)
    {
        this.selectable = selectable;
        UpdateMaterial();
    }

    public void SetPath (bool path)
    {
        this.path = path;
        UpdateMaterial();
    }


    private void UpdateMaterial()
    {
        GetTileMaterialScript().SetMaterial();
    }





}
