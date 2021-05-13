using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO:
 *  Add New Sprites and Animations
 *  Save that rooms have been cleared of enemies
 *  Create a title screen
 *  Fix Shaman Fire attack and Projectile Attacks
 *  Add way to restore life
 *  Create / Rebalance rooms and dungeon size
 *  Add regular goblin enemy
 * */

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public RoomLoader roomLoader;
    public MapDisplay mapDisplay;
    public int mapSize;
    public int newPathChance;
    public int stopDirectionChance;
    public RoomLayout startingRoom;
    public RoomLayout[] roomLayouts;
    
    private Room[,] map;
    private Vector2Int currentLocation;
    private List<EnemyController> enemies;
    private List<Exit> exits;

    private void Awake() {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }

        enemies = new List<EnemyController>();
        exits = new List<Exit>();
    }

    void Start() {
        MapGenerator mapGenerator = new MapGenerator(mapSize, newPathChance, stopDirectionChance, startingRoom, roomLayouts);
        map = mapGenerator.GenerateMap();
        mapDisplay.GenerateMapDisplay(map);
        EnterRoom(0, 0);
    }

    public void EnterRoom(int x, int y) {
        enemies.Clear();
        exits.Clear();
        currentLocation = new Vector2Int(x, y);
        mapDisplay.EnterRoom(x, y);
        roomLoader.Load(map[x, y]);
    }

    public void AddEnemy(EnemyController enemy) {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy) {
        enemies.Remove(enemy);
        if(enemies.Count == 0) {
            foreach(Exit exit in exits) {
                exit.OpenExit();
            }
        }
    }

    public void AddExit(Exit exit) {
        exits.Add(exit);
    }

    public int GetEnemyCount() {
        return enemies.Count;
    }
}
