using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {
    private Room[,] map;
    private int newPathChance;
    private int stopDirectionChance;
    private int size;
    private RoomLayout startingRoom;
    private RoomLayout[] roomLayouts;

    private Queue<System.Action> extraDirectionQueue;

    private delegate void GenerateMethod(int x, int y);

    public MapGenerator(int size, int newPathChance, int stopDirectionChance, RoomLayout startingRoom, RoomLayout[] roomLayouts) {
        this.size = size;
        this.newPathChance = newPathChance;
        this.stopDirectionChance = stopDirectionChance;
        this.startingRoom = startingRoom;
        this.roomLayouts = roomLayouts;
    }

    public Room[,] GenerateMap() {
        extraDirectionQueue = new Queue<System.Action>();
        map = new Room[size, size];
        int x = 0, y = 0;
        map[x, y] = CreateStartRoom(x, y);
        GenerateRooms(x, y);
        while(extraDirectionQueue.Count > 0) {
            extraDirectionQueue.Dequeue().Invoke();
        }
        return map;
    }

    private void GenerateRooms(int x, int y) {
        List<GenerateMethod> directionList = new List<GenerateMethod>();
        directionList.Add(GenerateEast);
        directionList.Add(GenerateWest);
        directionList.Add(GenerateNorth);
        directionList.Add(GenerateSouth);

        while(directionList.Count > 0) {
            int index = Random.Range(0, directionList.Count);
            if(Random.Range(0, 100) < stopDirectionChance) {
                GenerateMethod generateMethod = directionList[index];
                extraDirectionQueue.Enqueue(() => generateMethod.Invoke(x, y));
            } else {
                directionList[index].Invoke(x, y);
            }
            
            directionList.RemoveAt(index);
        }
    }

    private void GenerateEast(int x, int y) {
        if (InBounds(x + 1, y)) {
            if(map[x + 1, y] == null) {
                map[x + 1, y] = CreateRoom(x + 1, y);
                map[x, y].eastExit = map[x + 1, y];
                map[x + 1, y].westExit = map[x, y];
                GenerateRooms(x + 1, y);
            } else if(Random.Range(0, 100) < newPathChance) {
                map[x, y].eastExit = map[x + 1, y];
                map[x + 1, y].westExit = map[x, y];
            }
        }
    }

    private void GenerateWest(int x, int y) {
        if (InBounds(x - 1, y)) {
            if (map[x - 1, y] == null) {
                map[x - 1, y] = CreateRoom(x - 1, y);
                map[x, y].westExit = map[x - 1, y];
                map[x - 1, y].eastExit = map[x, y];
                GenerateRooms(x - 1, y);
            } else if (Random.Range(0, 100) < newPathChance) {
                map[x, y].westExit = map[x - 1, y];
                map[x - 1, y].eastExit = map[x, y];
            }
        }
    }

    private void GenerateNorth(int x, int y) {
        if (InBounds(x, y + 1)) {
            if (map[x, y + 1] == null) {
                map[x, y + 1] = CreateRoom(x, y + 1);
                map[x, y].northExit = map[x, y + 1];
                map[x, y + 1].southExit = map[x, y];
                GenerateRooms(x, y + 1);
            } else if (Random.Range(0, 100) < newPathChance) {
                map[x, y].northExit = map[x, y + 1];
                map[x, y + 1].southExit = map[x, y];
            }
        }
    }

    private void GenerateSouth(int x, int y) {
        if (InBounds(x, y - 1)) {
            if (map[x, y - 1] == null) {
                map[x, y - 1] = CreateRoom(x, y - 1);
                map[x, y].southExit = map[x, y - 1];
                map[x, y - 1].northExit = map[x, y];
                GenerateRooms(x, y - 1);
            } else if (Random.Range(0, 100) < newPathChance) {
                map[x, y].southExit = map[x, y - 1];
                map[x, y - 1].northExit = map[x, y];
            }
        }
    }

    private Room CreateRoom(int x, int y) {
        RoomLayout layout = roomLayouts[Random.Range(0, roomLayouts.Length)];
        return CreateRoom(x, y, layout);
    }

    private Room CreateStartRoom(int x, int y) {
        return CreateRoom(x, y, startingRoom);
    }

    private Room CreateRoom(int x, int y, RoomLayout layout) {
        Room room = new Room(x, y);
        room.layout = layout.GetCopy();
        room.size = layout.size;
        if(x == size-1 && y == size-1) {
            room.finalRoom = true;
        }
        return room;
    }

    private bool InBounds(int x, int y) {
        return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
    }
}

public class Room {
    public Vector2Int position;
    public Room northExit;
    public Room eastExit;
    public Room southExit;
    public Room westExit;
    public int size;
    public bool finalRoom = false;

    public EntityData[] layout;

    public Room(int x, int y) {
        this.position = new Vector2Int(x, y);
    }
}
