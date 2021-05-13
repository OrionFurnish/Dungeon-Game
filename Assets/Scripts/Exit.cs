using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {
    public static Vector3 enteringDirection;
    public Vector3 enteringOffset;
    public Sprite closedIcon;
    [HideInInspector] public Vector2Int targetIndex;

    private bool closed = false;
    private SpriteRenderer sr;
    private Sprite openIcon;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        openIcon = sr.sprite;
        GameManager.instance.AddExit(this);
        if(GameManager.instance.GetEnemyCount() > 0) {
            CloseExit();
        }
        if(enteringDirection == enteringOffset) {
            PlayerController.instance.transform.position = transform.position + enteringOffset;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(!closed && collision.gameObject.CompareTag("Player")) {
            if (targetIndex.x != -1) {
                GameManager.instance.EnterRoom(targetIndex.x, targetIndex.y);
                enteringDirection = -enteringOffset;
            } else {
                SceneManager.LoadScene("Victory");
            }   
        }
    }

    public void CloseExit() {
        closed = true;
        sr.sprite = closedIcon;
    }

    public void OpenExit() {
        closed = false;
        sr.sprite = openIcon;
    }
}
