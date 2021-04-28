using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    public Vector2 enteringOffset;
    [HideInInspector] public Vector2Int targetIndex;

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            GameManager.instance.EnterRoom(targetIndex.x, targetIndex.y);
        }
    }
}
