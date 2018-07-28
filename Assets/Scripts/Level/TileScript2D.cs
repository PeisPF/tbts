using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript2D : MonoBehaviour
{

    private TileType type;

    public TileScript2D()
    {
        this.type = TileType.NONE;
    }

    public void SetTileType(TileType type)
    {
        this.type = type;
    }

    public TileType GetTileType()
    {
        return this.type;
    }
   /* public void SetColor(Color color)
    {
        this.GetComponent<Renderer>().material.color = color;
    }*/
    private void Start()
    {
        //this.color = Color.black;   
    }

    /*public void Update()
    {
        //this.GetComponent<Renderer>().material.color = color;
    }*/

    public void Update()
    {
        switch (this.type)
        {
            case TileType.NONE:
                this.GetComponent<Renderer>().material.color = Color.black;
                break;
            case TileType.FLOOR:
                this.GetComponent<Renderer>().material.color = Color.white;
                break;
            case TileType.WALL:
                this.GetComponent<Renderer>().material.color = Color.gray;
                break;
            case TileType.DOOR_CLOSED:
                this.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case TileType.DOOR_OPEN:
                this.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case TileType.FAILURE:
                this.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }

    public enum TileType
    {
       NONE, WALL, FLOOR, DOOR_CLOSED, DOOR_OPEN, FAILURE
    }

    /*public Color GetColor()
    {
        return this.GetComponent<Renderer>().material.color;
    }*/
}