using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public Stats stats;
    protected Rigidbody2D rb;
    protected Animator anim;
    public BarControl lifeBar;
    public bool attacking;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage) {
        stats.life -= damage;
        lifeBar.Set(stats.life, stats.maxLife);
        if (stats.life <= 0) {
            Die();
        }
    }

    public void TakeKnockback(float knockback, Vector3 origin) {
        rb.AddForce(SF.GetDirectionForce(origin, transform.position)*knockback);
    }

    private void Die() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        if (lifeBar != null) { Destroy(lifeBar.gameObject); }
    }
}
