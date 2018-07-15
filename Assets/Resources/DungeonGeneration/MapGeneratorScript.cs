using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour
{

    public int WIDTH;
    public int HEIGTH;
    public GameObject map;

    public int THRESHOLD;

    public int MIN_ROOM_SIZE;

    public GameObject[,] tilesMap;

    private System.Random random;

    public int MAX_OFFSET;
    public int MIN_OFFSET;

    public LayerMask onlyTilesLayerMask;

    public int STARTING_POINT_DEPTH;
    public int ARRIVAL_POINT_DEPTH;

    private List<Room> rooms;

    // Use this for initialization
    void Start()
    {
        random = new System.Random();
        tilesMap = new GameObject[WIDTH, HEIGTH];
        rooms = new List<Room>();
        InitMap();
        Area root = new Area(null, 0, 0, WIDTH, HEIGTH, tilesMap);
        BisectOrCreateRoom(root);
        ConnectChildren(root);
        CreateQuest(root);
    }

    
    private void CreateQuest(Area root)
    {
        CreateItemInArea(root.GetChildren().ToArray()[0], STARTING_POINT_DEPTH, "DungeonGeneration/StartingPoint");
        CreateItemInArea(root.GetChildren().ToArray()[1], ARRIVAL_POINT_DEPTH, "DungeonGeneration/ArrivalPoint");
    }

    private void CreateItemInArea(Area area, int threshold, string itemName)
    {
        bool instantiated = false;
        if (random.Next(100) > threshold || area.GetRoom() != null) //o ho superato la soglia, o sono in una foglia
        {
            if (area.GetChildren() != null && area.GetChildren().Count > 0)//non sono in una foglia, ma provo a vedere se uno dei figli lo è
            {
                if (area.GetChildren().ToArray()[0].GetRoom() != null && area.GetChildren().ToArray()[1].GetRoom() != null)
                {
                    if (random.Next(100) > 50)
                    {
                        InstantiateItemInRoom(area.GetChildren().ToArray()[0].GetRoom(), itemName);
                    }
                    else
                    {
                        InstantiateItemInRoom(area.GetChildren().ToArray()[1].GetRoom(), itemName);
                    }
                    instantiated = true;
                }
                else if (area.GetChildren().ToArray()[0].GetRoom() != null)
                {
                    InstantiateItemInRoom(area.GetChildren().ToArray()[0].GetRoom(), itemName);
                    instantiated = true;
                }
                else if (area.GetChildren().ToArray()[1].GetRoom() != null)
                {
                    InstantiateItemInRoom(area.GetChildren().ToArray()[1].GetRoom(), itemName);
                    instantiated = true;
                }
            }
            else //sono in una foglia, c'è sicuramente una stanza
            {
                InstantiateItemInRoom(area.GetRoom(), itemName);
                instantiated = true;
            }
        }
        if (!instantiated)
        {
            if (random.Next(100) > 50)
            {
                CreateItemInArea(area.GetChildren().ToArray()[0], threshold, itemName);
            }
            else
            {
                CreateItemInArea(area.GetChildren().ToArray()[1], threshold, itemName);
            }
        }
    }

    private void InstantiateItemInRoom(Room room, string itemName)
    {
        UnityEngine.Object obj = Resources.Load(itemName);
        GameObject item = (GameObject)Instantiate(obj);
        item.transform.position = room.GetMedianPoint() + new Vector3(0, 1, 0);
    }

    private void ConnectChildren(Area area)
    {
        Area area1 = area.GetChildren().ToArray()[0];
        Area area2 = area.GetChildren().ToArray()[1];

        Room room1 = area.GetChildren().ToArray()[0].GetRoom();
        Room room2 = area.GetChildren().ToArray()[1].GetRoom();
        if (room1 == null)
        {
            ConnectChildren(area1);
            room1 = area1;
        }
        if (area2.GetRoom() == null)
        {
            ConnectChildren(area2);
            room2 = area2;
        }
        area.AddTiles(room1.GetTiles());
        area.AddTiles(room2.GetTiles());
        List<GameObject> corridor = CreateCorridorBetween(room1, room2);
        area.AddTiles(corridor);
    }


    private List<GameObject> CreateCorridorBetween(Room room1, Room room2)
    {
        GameObject[] closestTiles = GetClosestTiles(room1, room2);
        List<GameObject> corridor = ConnectTiles(closestTiles[0], closestTiles[1]);
    return corridor;
    }

    private List<GameObject> ConnectTiles(GameObject tile1, GameObject tile2)
    {
        List<GameObject> result = new List<GameObject>();
        if (random.Next(100) > 50)
        {
            tile1.GetComponent<TileScript2D>().SetType(TileScript2D.TileType.DOOR_OPEN);
            tile2.GetComponent<TileScript2D>().SetType(TileScript2D.TileType.FLOOR);
        }
        else
        {
            tile1.GetComponent<TileScript2D>().SetType(TileScript2D.TileType.FLOOR);
            tile2.GetComponent<TileScript2D>().SetType(TileScript2D.TileType.DOOR_OPEN);
        }


        ConnectTilesAndAddToResult(tile1, tile2, result, TileScript2D.TileType.FLOOR);
        List<GameObject> wallTiles = CreateWalls(tile1, tile2);
        result.AddRange(wallTiles);
        return result;
    }

    private void ConnectTilesAndAddToResult(GameObject tile1, GameObject tile2, List<GameObject> result, TileScript2D.TileType type)
    {
        RaycastHit[] tilesHits = RayCastUtils.RaycastTo(tile1.transform, tile2.transform, onlyTilesLayerMask);
        foreach (RaycastHit hit in tilesHits)
        {
            GameObject tile = hit.collider.gameObject;
            if (tile != tile1 && tile != tile2)
            {
                result.Add(tile);
                hit.collider.GetComponent<TileScript2D>().SetType(type);
            }
        }
    }

    private List<GameObject> CreateWalls(GameObject tile1, GameObject tile2)
    {
        List<GameObject> result = new List<GameObject>();

        if (tile1.transform.position.x == tile2.transform.position.x)
        {
            GameObject wallTile1 = tilesMap[(int)tile1.transform.position.x - 1, (int)tile1.transform.position.z];
            GameObject wallTile2 = tilesMap[(int)tile2.transform.position.x - 1, (int)tile2.transform.position.z];
            ConnectTilesAndAddToResult(wallTile1, wallTile2, result, TileScript2D.TileType.WALL);
            wallTile1 = tilesMap[(int)tile1.transform.position.x + 1, (int)tile1.transform.position.z];
            wallTile2 = tilesMap[(int)tile2.transform.position.x + 1, (int)tile2.transform.position.z];
            ConnectTilesAndAddToResult(wallTile1, wallTile2, result, TileScript2D.TileType.WALL);
        }
        else if (tile1.transform.position.z == tile2.transform.position.z)
        {
            GameObject wallTile1 = tilesMap[(int)tile1.transform.position.x, (int)tile1.transform.position.z - 1];
            GameObject wallTile2 = tilesMap[(int)tile2.transform.position.x, (int)tile2.transform.position.z - 1];
            ConnectTilesAndAddToResult(wallTile1, wallTile2, result, TileScript2D.TileType.WALL);
            wallTile1 = tilesMap[(int)tile1.transform.position.x, (int)tile1.transform.position.z + 1];
            wallTile2 = tilesMap[(int)tile2.transform.position.x, (int)tile2.transform.position.z + 1];
            ConnectTilesAndAddToResult(wallTile1, wallTile2, result, TileScript2D.TileType.WALL);
        }
        else
        {
            Debug.Log("CAZZO, non sono paralleli");
        }


        return result;
    }

    private GameObject[] GetClosestTiles(Room room1, Room room2)
    {
        //GameObject[] result = new GameObject[2];
        float minDistance = float.PositiveInfinity;
        List<GameObject[]> candidates = new List<GameObject[]>();
        foreach (GameObject tile in room1.GetTiles())
        {
            foreach (GameObject tile2 in room2.GetTiles())
            {
                if (AreOnAxis(tile, tile2) && AreNotCorners(tile, tile2, room1, room2))
                {
                    float distance = Vector3.Distance(tile.transform.position, tile2.transform.position);
                    if (distance <= minDistance)
                    {
                        if (distance < minDistance)
                        {
                            candidates = new List<GameObject[]>();
                        }
                        GameObject[] candidate = new GameObject[2];
                        candidate[0] = tile;
                        candidate[1] = tile2;
                        candidates.Add(candidate);
                        minDistance = distance;
                    }
                }
            }
        }
        return candidates[random.Next(candidates.Count)];
    }

    /*private bool IsCorner(GameObject tile, Area area)
    {
        if (area.GetRoom() == null)
        {
            List<Area> children = area.GetChildren();
            return IsCorner(tile.gameObject, area.GetChildren().ToArray()[0]) || IsCorner(tile.gameObject, area.GetChildren().ToArray()[1]);
        }
        else
        {
            return area.GetRoom().GetTiles().Contains(tile) && ((tile.transform.position.x == area.GetRoom().GetX() || tile.transform.position.x == area.GetRoom().GetX() + area.GetRoom().GetSizeX()) || (tile.transform.position.z == area.GetRoom().GetY() || tile.transform.position.z == area.GetRoom().GetY() + area.GetRoom().GetSizeY()));
        }
    }*/

    private bool AreNotCorners(GameObject tile1, GameObject tile2, Room room1, Room room2)
    {
        return !room1.IsCorner(tile1) && !room2.IsCorner(tile2);
    }

    private bool AreOnAxis(GameObject tile, GameObject tile2)
    {
        return tile.transform.position.x == tile2.transform.position.x || tile.transform.position.z == tile2.transform.position.z;
    }

    private void Update()
    {

    }

    private void BisectOrCreateRoom(Area area)
    {
        if (area.IsSmallerThan(THRESHOLD))
        {
            Room room = CreateRoom(area);
            rooms.Add(room);
        }
        else
        {
            //Debug.Log("Splitting area: " + area);
            bool bisectOnX = false;
            if (!area.IsSmallerOnXThan(THRESHOLD) && !area.IsSmallerOnYThan(THRESHOLD))
            {
                int n = random.Next(100);
                if (n >= 50)
                {
                    bisectOnX = true;
                }
            }
            else
            {
                if (area.IsSmallerOnXThan(THRESHOLD))
                {
                    bisectOnX = false;
                }
                else
                {
                    bisectOnX = true;
                }

            }
            Area child1;
            Area child2;
            int minAreaSize = MIN_ROOM_SIZE + MIN_OFFSET * 2;
            if (bisectOnX)
            {   //se è 30, deve essere tra 6 e 24
                int bisectCoord = minAreaSize + random.Next(area.GetSizeX() - minAreaSize * 2);
                child1 = new Area(area, area.GetX(), area.GetY(), bisectCoord, area.GetSizeY(), tilesMap);
                child2 = new Area(area, area.GetX() + bisectCoord, area.GetY(), area.GetSizeX() - bisectCoord, area.GetSizeY(), tilesMap);
            }
            else
            {
                int bisectCoord = minAreaSize + random.Next(area.GetSizeY() - minAreaSize * 2);
                child1 = new Area(area, area.GetX(), area.GetY(), area.GetSizeX(), bisectCoord, tilesMap);
                child2 = new Area(area, area.GetX(), area.GetY() + bisectCoord, area.GetSizeX(), area.GetSizeY() - bisectCoord, tilesMap);
            }
            area.AddChild(child1);
            area.AddChild(child2);
            BisectOrCreateRoom(child1);
            BisectOrCreateRoom(child2);
        }
    }

    private bool IsOnRoomsEdge(int x, int y, Room room)
    {
        return x == room.GetX() || x == room.GetX() + room.GetSizeX() - 1 || y == room.GetY() || y == room.GetY() + room.GetSizeY() - 1;
    }

    private Room CreateRoom(Area area)
    {
        int actualMaxOffsetX = Math.Min(MAX_OFFSET, (area.GetSizeX() - MIN_ROOM_SIZE) / 2);
        int actualMaxOffsetY = Math.Min(MAX_OFFSET, (area.GetSizeY() - MIN_ROOM_SIZE) / 2);
        int offsetX = MIN_OFFSET + random.Next(Math.Min(area.GetSizeX() - actualMaxOffsetX, actualMaxOffsetX));
        int offsetY = MIN_OFFSET + random.Next(Math.Min(area.GetSizeY() - actualMaxOffsetY, actualMaxOffsetY));
        int offsetXEnd = MIN_OFFSET + random.Next(Math.Min(area.GetSizeX() - (actualMaxOffsetX + offsetX), actualMaxOffsetX));
        int offsetYEnd = MIN_OFFSET + random.Next(Math.Min(area.GetSizeY() - (actualMaxOffsetY + offsetX), actualMaxOffsetY));

        //Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        // Debug.Log("creating room in area: " + area+",using color: " +color);
        Room room = new Room(area.GetX() + offsetX, area.GetY() + offsetY, area.GetSizeX() - offsetX - offsetXEnd, area.GetSizeY() - offsetY - offsetYEnd, tilesMap);
        area.SetRoom(room);
        for (int x = room.GetX(); x < room.GetX() + room.GetSizeX(); x++)
        {
            for (int y = room.GetY(); y < room.GetY() + room.GetSizeY(); y++)
            {
                TileScript2D tileScript = tilesMap[x, y].GetComponent<TileScript2D>();
                if (IsOnRoomsEdge(x, y, room))
                {
                    tileScript.SetType(TileScript2D.TileType.WALL);
                }
                else
                {
                    tileScript.SetType(TileScript2D.TileType.FLOOR);
                }


                //tiles[x, y].GetComponent<TileScript2D>().SetColor(color);
                //Debug.Log("tile at [" + x + "," + y + "] colored with color: " + tiles[x,y].GetComponent<TileScript2D>().GetColor());
            }
        }
        return room;
    }

    private void InitMap()
    {
        UnityEngine.Object obj = Resources.Load("DungeonGeneration/2DTile");

        for (int x = 0; x < WIDTH; x++)
        {
            for (int z = 0; z < HEIGTH; z++)
            {
                GameObject item = (GameObject)Instantiate(obj);
                item.transform.position = new Vector3(x, 0, z);
                item.transform.SetParent(map.transform);
                tilesMap[x, z] = item;
                //Debug.Log("tiles[" + (x + WIDTH / 2) + "," + (z + HEIGTH / 2) + "]");
            }
        }
    }
}

class Room
{
    private int x;
    private int y;
    private int sizeX;
    private int sizeY;
    private bool connected;

    private int index;

    protected List<GameObject> tiles;

    public override string ToString()
    {
        return "Room: x=" + x + "(" + sizeX + "), y=" + y + "(" + sizeY + ")";
    }

    public void SetConnected()
    {
        this.connected = true;
    }

    public bool IsConnected()
    {
        return this.connected;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetSizeX()
    {
        return sizeX;
    }

    public int GetSizeY()
    {
        return sizeY;
    }

    public Vector3 GetMedianPoint()
    {
        return new Vector3(x + (sizeX / 2), 1, y + (sizeY / 2));
    }

    public Room(int x, int y, int sizeX, int sizeY, GameObject[,] tilesMap)
    {
        this.x = x;
        this.y = y;
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.connected = false;

        InitTiles(tilesMap);
    }

    public void AddTile(GameObject tile)
    {
        this.tiles.Add(tile);
    }

    public void AddTiles(List<GameObject> tiles)
    {
        this.tiles.AddRange(tiles);
    }

    protected virtual void InitTiles(GameObject[,] tilesMap)
    {
        this.tiles = new List<GameObject>();
        for (int x = this.x; x < this.x + this.sizeX; x++)
        {
            for (int y = this.y; y < this.y + this.sizeY; y++)
            {
                this.tiles.Add(tilesMap[x, y]);
            }
        }
    }

    internal bool IsSmallerThan(int threshold)
    {
        return this.IsSmallerOnXThan(threshold) && this.IsSmallerOnYThan(threshold);
    }

    public bool IsSmallerOnYThan(int threshold)
    {
        return sizeY < threshold;
    }

    public bool IsSmallerOnXThan(int threshold)
    {
        return sizeX < threshold;
    }

    internal List<GameObject> GetTiles()
    {
        return this.tiles;
    }

    public virtual bool IsCorner(GameObject tile)
    {
        return ((tile.transform.position.x == GetX() || tile.transform.position.x == GetX() + GetSizeX() - 1) && (tile.transform.position.z == GetY() || tile.transform.position.z == GetY() + GetSizeY() - 1));
    }
}

class Area : Room
{
    private Area parent;
    private List<Area> children;
    private Room room;

    public Area(Area parent, int x, int y, int sizeX, int sizeY, GameObject[,] tilesMap) : base(x, y, sizeX, sizeY, tilesMap)
    {
        this.parent = parent;
        this.children = new List<Area>();
    }

    public override bool IsCorner(GameObject tile)
    {
        if (this.room != null)
        {
            return room.IsCorner(tile);
        }
        else
        {
            foreach (Area child in children)
            {
                if (child.IsCorner(tile))
                {
                    return true;
                }
            }
            return false;
        }
    }

    protected override void InitTiles(GameObject[,] tilesMap)
    {
        //un'area, a differenza di una stanza, non ha inizialmente alcun tile
        this.tiles = new List<GameObject>();
    }
    public Room GetRoom()
    {
        return this.room;
    }
    public void SetRoom(Room room)
    {
        this.room = room;
    }
    public List<Area> GetChildren()
    {
        return this.children;
    }
    public void AddChild(Area child)
    {
        this.children.Add(child);
    }

    public override string ToString()
    {
        return "Area: x=" + GetX() + "(" + GetSizeX() + "), y=" + GetY() + "(" + GetSizeY() + ")";
    }

    internal Area GetParent()
    {
        return this.parent;
    }
}
