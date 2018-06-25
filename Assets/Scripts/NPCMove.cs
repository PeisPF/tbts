/**
 * 
 * Classe commentata perchè per adesso non serve
 * 
 * */

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    /*GameObject target;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }
        else
        {
            if (!moving)
            {
                FindNearestTarget();
                CalculatePath();
                FindSelectableTiles();
                actualTargetTile.target = true;
            }

            else
            {
                Move();
            }
        }
    }

    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject item in targets)
        {
            float d = Vector3.Distance(transform.position, item.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = item;
            }
        }


        target = nearest;
    }

    public override void SetShowPath(bool value)
    {
        //throw new System.NotImplementedException();
    }
}
*/