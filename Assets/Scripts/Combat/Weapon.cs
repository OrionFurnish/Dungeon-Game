using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float damage;
    public float knockback;

    public virtual void OnTriggerEnter2D(Collider2D collision) {
        Controller controller = collision.GetComponent<Controller>();
        if(controller != null) {
            controller.TakeDamage(damage);
            controller.TakeKnockback(knockback, transform.position);
        }
    }
}
