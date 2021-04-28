using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    [HideInInspector] public GameObject prefab;
}

[System.Serializable]
public class EntityData {
    public Vector2 position;

    public GameObject prefab;

    public EntityData(Vector2 position, GameObject prefab) {
        this.position = position;
        this.prefab = prefab;
    }

    public EntityData GetCopy() {
        EntityData entity = new EntityData(position, prefab);
        return entity;
    }

    public void Instantiate(Transform parent) {
        Transform t = GameObject.Instantiate(prefab, parent).transform;
        t.GetComponent<Entity>().prefab = prefab;
        t.position = position;
    }
}
