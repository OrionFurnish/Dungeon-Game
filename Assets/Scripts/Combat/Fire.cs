using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Weapon {
    public float duration;
    public float damageTimer;

    private float lastDamage;

    private void Start() {
        Invoke("DestroySelf", duration);
    }

    public void OnTriggerStay2D(Collider2D collision) {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null) {
            controller.TakeDamage(damage);
            controller.TakeKnockback(knockback, transform.position);
        }
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
