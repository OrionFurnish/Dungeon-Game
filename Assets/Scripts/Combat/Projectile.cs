using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Weapon {
    public float speed;
    public bool canHitEnemies;
    public Color deflectedColor;
    public float homing;
    public float duration;
    [HideInInspector] public Transform target;

    private Rigidbody2D rb;
    private Vector3 dir;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        dir = SF.GetDirectionForce(transform.position, target.position);
        Invoke("DestroySelf", duration);
    }

    private void FixedUpdate() {
        if(homing > 0 && target != null) {
            dir += SF.GetDirectionForce(transform.position, target.position) * homing;
            dir.Normalize();
        }
            
        SF.MoveTowards(rb, dir*1000, speed*Time.fixedDeltaTime);
    }

    public override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player Attack")) {
            // Player Deflects Attack
            dir = -dir;
            speed *= 1.2f;
            GetComponent<SpriteRenderer>().color = deflectedColor;
            canHitEnemies = true;
            homing = 0f;
        }
        else if(!collision.gameObject.CompareTag("Enemy") || canHitEnemies) {
            base.OnTriggerEnter2D(collision);
            Destroy(gameObject);
        }
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
