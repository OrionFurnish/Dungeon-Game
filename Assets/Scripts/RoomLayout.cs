using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Layout", menuName ="Custom/Create Room Layout", order = 0)]
public class RoomLayout : ScriptableObject {
    public EntityData[] entities;
    public int size;

    public EntityData[] GetCopy() {
        EntityData[] newEntities = new EntityData[entities.Length];
        for(int i = 0; i < entities.Length; i++) {
            newEntities[i] = entities[i].GetCopy();
        }
        return newEntities;
    }

    public RoomLayout() {
        entities = new EntityData[0];
        size = 10;
    }
}