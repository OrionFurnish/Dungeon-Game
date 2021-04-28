using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour {
    public Transform ground;
    public Transform wallBottom;
    public Transform wallLeft;
    public Transform wallRight;
    public Transform wallTop;
    public Transform wallBottomLeft;
    public Transform wallBottomRight;
    public Transform wallTopLeft;
    public Transform wallTopRight;
    public Transform exitTop;
    public Transform exitBottom;
    public Transform exitLeft;
    public Transform exitRight;
    public Transform player;

    private GameObject level;
    private Transform groundParent;
    private Transform entityParent;

    public void Load(Room room) {
        if (level != null) { Destroy(level); }
        level = new GameObject();
        level.name = "Level";
        level.transform.SetParent(transform);
        groundParent = new GameObject().transform;
        groundParent.name = "Ground";
        groundParent.SetParent(level.transform);
        entityParent = new GameObject().transform;
        entityParent.name = "Entities";
        entityParent.SetParent(level.transform);

        // Ground
        Transform groundTrans = Instantiate(ground, groundParent);
        groundTrans.localScale = new Vector3(room.size, room.size, 1f);
        groundTrans.transform.position = Vector3.zero;

        // Wall Tops
        for(float i = -room.size / 2f; i < room.size / 2f; i++) {
            Transform trans;
            if(i != -.5f || room.northExit == null) {
                trans = Instantiate(wallTop, groundParent);
            } else { // Generate Exit
                trans = Instantiate(exitTop, groundParent);
                trans.GetComponent<Exit>().targetIndex = room.northExit.position;
            }
            trans.position = new Vector3(i + .5f, room.size / 2f + .5f, 0f);
        }

        // Wall Bottoms
        for (float i = -room.size / 2f; i < room.size / 2f; i++) {
            Transform trans;
            if (i != -.5f || room.southExit == null) {
                trans = Instantiate(wallBottom, groundParent);
            } else { // Generate Exit
                trans = Instantiate(exitBottom, groundParent);
                trans.GetComponent<Exit>().targetIndex = room.southExit.position;
            }
            trans.position = new Vector3(i + .5f, -room.size / 2f - .5f, 0f);
        }

        // Wall Left
        for (float i = -room.size / 2f; i < room.size / 2f; i++) {
            Transform trans;
            if (i != -.5f || room.westExit == null) {
                trans = Instantiate(wallLeft, groundParent);
            } else { // Generate Exit
                trans = Instantiate(exitLeft, groundParent);
                trans.GetComponent<Exit>().targetIndex = room.westExit.position;
            }
            trans.position = new Vector3(-room.size / 2f - .5f, i + .5f, 0f);
        }

        // Wall Right
        for (float i = -room.size / 2f; i < room.size / 2f; i++) {
            Transform trans;
            if (i != -.5f || room.eastExit == null) {
                trans = Instantiate(wallRight, groundParent);
            } else { // Generate Exit
                trans = Instantiate(exitRight, groundParent);
                trans.GetComponent<Exit>().targetIndex = room.eastExit.position;
            }
            trans.position = new Vector3(room.size / 2f + .5f, i + .5f, 0f);
        }

        // Wall Corners
        Transform wallCorner = Instantiate(wallBottomLeft, groundParent);
        wallCorner.position = new Vector3(-room.size / 2f - .5f, -room.size / 2f - .5f, 0f);
        wallCorner = Instantiate(wallBottomRight, groundParent);
        wallCorner.position = new Vector3(room.size / 2f + .5f, -room.size / 2f - .5f, 0f);
        wallCorner = Instantiate(wallTopLeft, groundParent);
        wallCorner.position = new Vector3(-room.size / 2f - .5f, room.size / 2f + .5f, 0f);
        wallCorner = Instantiate(wallTopRight, groundParent);
        wallCorner.position = new Vector3(room.size / 2f + .5f, room.size / 2f + .5f, 0f);


        // Place Player
        player.transform.position = new Vector3(0f, 0f, 0f);

        // Load Layout
        LoadLayout(room.layout);
    }

    private void LoadLayout(EntityData[] layout) {
        foreach(EntityData entity in layout) {
            entity.Instantiate(entityParent);
        }
    }

    public void DestroyRoom() {
        if (level != null) {
            DestroyImmediate(level);
        }
    }


}
