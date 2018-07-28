using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerationUtils
{

    public static GameObject GetUp(int x, int y, GameObject[,] tilesMap)
    {
        if (y - 1 > 0)
        {
            return tilesMap[x, y - 1];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetDown(int x, int y, GameObject[,] tilesMap)
    {
        if (y + 1 < tilesMap.GetLength(1))
        {
            return tilesMap[x, y + 1];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetLeft(int x, int y, GameObject[,] tilesMap)
    {
        if (x - 1 > 0)
        {
            return tilesMap[x - 1, y];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetRight(int x, int y, GameObject[,] tilesMap)
    {
        if (x + 1 < tilesMap.GetLength(0))
        {
            return tilesMap[x + 1, y];
        }
        else
        {
            return null;
        }
    }


    public static TileScript2D GetUpTile(int x, int y, GameObject[,] tilesMap)
    {
        if (GetUp(x, y, tilesMap) != null)
        {
            return GetUp(x, y, tilesMap).GetComponent<TileScript2D>();
        }
        return null;
    }

    public static TileScript2D GetDownTile(int x, int y, GameObject[,] tilesMap)
    {
        if (GetDown(x, y, tilesMap) != null)
        {
            return GetDown(x, y, tilesMap).GetComponent<TileScript2D>();
        }
        return null;
    }

    public static TileScript2D GetLeftTile(int x, int y, GameObject[,] tilesMap)
    {
        if (GetLeft(x, y, tilesMap) != null)
        {
            return GetLeft(x, y, tilesMap).GetComponent<TileScript2D>();
        }
        return null;
    }

    public static TileScript2D GetRightTile(int x, int y, GameObject[,] tilesMap)
    {
        if (GetRight(x, y, tilesMap))
        {
            return GetRight(x, y, tilesMap).GetComponent<TileScript2D>();
        }
        return null;
    }

}
