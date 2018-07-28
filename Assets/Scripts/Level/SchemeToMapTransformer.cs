using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeToMapTransformer : MonoBehaviour
{

    public GameObject[,] tilesMap;
    public Room startingRoom;
    public Room arrivalRoom;
    public GameObject map;
    internal Dictionary<GameObject, Room> lockedDoors;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Transform()
    {
        map = new GameObject();
        map.name = "MAP";
        for (int x = 0; x < tilesMap.GetLength(0); x += 1)
        {
            for (int y = 0; y < tilesMap.GetLength(1); y += 1)
            {
                Transform(tilesMap[x, y], x, y);
            }
        }

        CreateStartingPoints();
        CreateArrivalPoint();
        CleanUp();
        return map;
    }

    private void CreateArrivalPoint()
    {
        Object obj = Resources.Load("chest_open");
        int x = (int)arrivalRoom.GetMedianPoint().x;
        int y = (int)arrivalRoom.GetMedianPoint().z;
        TileScript2D tile = tilesMap[x, y].GetComponent<TileScript2D>();

        GameObject go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = tile.transform.position + new Vector3(0, .5f, 0);
        go.transform.SetParent(map.transform, false);

    }

    private void CreateStartingPoints()
    {
        Object obj = Resources.Load("PlayerStartingPoint");
        int x = (int)startingRoom.GetMedianPoint().x;
        int y = (int)startingRoom.GetMedianPoint().z;

        TileScript2D up = DungeonGenerationUtils.GetUpTile(x, y, tilesMap);
        TileScript2D down = DungeonGenerationUtils.GetDownTile(x, y, tilesMap);
        TileScript2D left = DungeonGenerationUtils.GetLeftTile(x, y, tilesMap);
        TileScript2D right = DungeonGenerationUtils.GetRightTile(x, y, tilesMap);

        GameObject go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = up.transform.position + new Vector3(0, 1f, 0);
        go.transform.SetParent(map.transform, false);

        go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = down.transform.position + new Vector3(0, 1f, 0);
        go.transform.SetParent(map.transform, false);

        go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = left.transform.position + new Vector3(0, 1f, 0);
        go.transform.SetParent(map.transform, false);

        go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = right.transform.position + new Vector3(0, 1f, 0);
        go.transform.SetParent(map.transform, false);

    }

    private void CleanUp()
    {
        for (int x = 0; x < tilesMap.GetLength(0); x += 1)
        {
            for (int y = 0; y < tilesMap.GetLength(1); y += 1)
            {
                Destroy(tilesMap[x, y]);
            }
        }
    }

    private void Transform(GameObject gameObject, int x, int y)
    {
        TileScript2D tile = gameObject.GetComponent<TileScript2D>();
        switch (tile.GetTileType())
        {
            case TileScript2D.TileType.NONE:
                //this.GetComponent<Renderer>().material.color = Color.black;
                break;
            case TileScript2D.TileType.FLOOR:
                CreateTile(gameObject.transform.position);
                break;
            case TileScript2D.TileType.WALL:
                CreateTile(gameObject.transform.position);
                CreateWall(gameObject, x, y);
                break;
            case TileScript2D.TileType.DOOR_CLOSED:
                CreateDoor(gameObject, x, y);
                break;
            case TileScript2D.TileType.DOOR_OPEN:
                CreateTile(gameObject.transform.position);
                CreateDoor(gameObject, x, y);
                break;
        }
    }

    private void CreateDoor(GameObject gameObject, int x, int y)
    {
        TileScript2D up = DungeonGenerationUtils.GetUpTile(x, y, tilesMap);
        TileScript2D down = DungeonGenerationUtils.GetDownTile(x, y, tilesMap);
        float degrees = 90;
        UnityEngine.Object obj = Resources.Load("Door");
        if (IsWall(up) && IsWall(down))
        {
            degrees = 0;
        }

        GameObject go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);
        go.transform.Rotate(Vector3.up, degrees);
        go.transform.SetParent(map.transform, false);
        if (lockedDoors.ContainsKey(gameObject))
        {
            LockDoorAndCreateKey(go, gameObject);
        }

    }

    private void LockDoorAndCreateKey(GameObject door, GameObject tile)
    {
        door.GetComponentInChildren<DoorScript>().allowedLocalOpen = false;
        CreateKeyInRoom(lockedDoors[tile]);
    }

    private void CreateKeyInRoom(Room room)
    {
        Debug.Log("Creating key in room " + room);//TODO: qui dentro va creata davvero la chiave
    }

    private void CreateWall(GameObject gameObject, int x, int y)
    {
        TileScript2D up = DungeonGenerationUtils.GetUpTile(x, y, tilesMap);
        TileScript2D down = DungeonGenerationUtils.GetDownTile(x, y, tilesMap);
        TileScript2D left = DungeonGenerationUtils.GetLeftTile(x, y, tilesMap);
        TileScript2D right = DungeonGenerationUtils.GetRightTile(x, y, tilesMap);
        UnityEngine.Object obj = null;
        float degrees = 0;
        if (IsNullOrFloor(up) && IsNullOrFloor(down))
        {
            obj = Resources.Load("WallCubeH");
        }
        if (IsNullOrFloor(right) && IsNullOrFloor(left))
        {
            obj = Resources.Load("WallCubeH");
            degrees = 90;
        }
        if (IsWall(up) && IsWall(left))
        {
            obj = Resources.Load("WallAngle");
            degrees = 270;
        }

        if (IsWall(up) && IsWall(right))
        {
            obj = Resources.Load("WallAngle");
            degrees = 180;
        }

        if (IsWall(down) && IsWall(left))
        {
            obj = Resources.Load("WallAngle");
            degrees = 0;
        }

        if (IsWall(down) && IsWall(right))
        {
            obj = Resources.Load("WallAngle");
            degrees = 90;
        }


        if (obj != null)
        {
            GameObject go = (GameObject)Utils.MyInstantiate(obj);
            go.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);
            go.transform.Rotate(Vector3.up, degrees);
            go.transform.SetParent(map.transform, false);

        }
    }

    private static bool IsWall(TileScript2D tile)
    {
        return (tile != null && tile.GetTileType() == TileScript2D.TileType.WALL);
    }

    private static bool IsNullOrFloor(TileScript2D tile)
    {
        return (tile == null || tile.GetTileType() == TileScript2D.TileType.FLOOR || tile.GetTileType() == TileScript2D.TileType.NONE || tile.GetTileType() == TileScript2D.TileType.DOOR_CLOSED || tile.GetTileType() == TileScript2D.TileType.DOOR_OPEN);
    }

    private void CreateTile(Vector3 position)
    {
        UnityEngine.Object obj = Resources.Load("Cube");
        GameObject go = (GameObject)Utils.MyInstantiate(obj);
        go.transform.position = position;
        go.transform.SetParent(map.transform, false);
    }


    
}
