using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public RoomLoader roomLoader;
    public MapDisplay mapDisplay;
    public int mapSize;
    public int newPathChance;
    public int stopDirectionChance;
    public RoomLayout[] roomLayouts;

    private Room[,] map;
    private Vector2Int currentLocation;

    private void Awake() {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start() {
        MapGenerator mapGenerator = new MapGenerator(mapSize, newPathChance, stopDirectionChance, roomLayouts);
        map = mapGenerator.GenerateMap();
        mapDisplay.GenerateMapDisplay(map);
        EnterRoom(0, 0);
    }

    public void EnterRoom(int x, int y) {
        currentLocation = new Vector2Int(x, y);
        mapDisplay.EnterRoom(x, y);
        roomLoader.Load(map[x, y]);
    }
}
