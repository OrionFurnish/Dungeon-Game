using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour {
    public Transform mapTile;
    public Transform mapRow;
    public Color playerLocationColor;

    private Transform[,] mapDisplay;
    private Color baseColor;
    private Vector2Int currentLocation;

    public void GenerateMapDisplay(Room[,] map) {
        mapDisplay = new Transform[map.GetLength(0), map.GetLength(1)];

        for (int y = 0; y < map.GetLength(1); y++) {
            Transform currentRow = Instantiate(mapRow, transform);
            currentRow.SetSiblingIndex(0);
            for (int x = 0; x < map.GetLength(0); x++) {
                Transform currentTile = Instantiate(mapTile, currentRow).GetChild(0);

                if(map[x, y].southExit == null) {
                    currentTile.GetChild(0).gameObject.SetActive(false);
                }

                if (map[x, y].eastExit == null) {
                    currentTile.GetChild(1).gameObject.SetActive(false);
                }

                if (map[x, y].westExit == null) {
                    currentTile.GetChild(2).gameObject.SetActive(false);
                }

                if (map[x, y].northExit == null) {
                    currentTile.GetChild(3).gameObject.SetActive(false);
                }

                mapDisplay[x, y] = currentTile;
                currentTile.gameObject.SetActive(false);
            }
        }

        RevealRoom(map.GetLength(0)-1, map.GetLength(1)-1); // Reveal target tile
        baseColor = mapDisplay[0, 0].GetComponent<Image>().color;
    }

    public void RevealRoom(int x, int y) {
        mapDisplay[x, y].gameObject.SetActive(true);
    }

    public void EnterRoom(int x, int y) {
        RevealRoom(x, y);
        SetDisplayColor(x, y, playerLocationColor);
        if (currentLocation != null && currentLocation != new Vector2Int(x, y)) {
            SetDisplayColor(currentLocation.x, currentLocation.y, baseColor);
        }
        currentLocation = new Vector2Int(x, y);
    }

    private void SetDisplayColor(int x, int y, Color color) {
        mapDisplay[x, y].GetComponent<Image>().color = color;
        foreach (Transform child in mapDisplay[x, y]) {
            child.GetComponent<Image>().color = color;
        }
    }
}
