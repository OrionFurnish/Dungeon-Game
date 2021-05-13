using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public Stats stats;
    protected Rigidbody2D rb;
    public Animator anim;
    public BarControl lifeBar;
    public bool attacking;
    public float attackCooldown;

    protected virtual void Awake() {
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

    public void Heal(float amount) {
        stats.life += amount;
        if(stats.life > stats.maxLife) {
            stats.life = stats.maxLife;
        }
        lifeBar.Set(stats.life, stats.maxLife);
    }

    public void TakeKnockback(float knockback, Vector3 origin) {
        rb.AddForce(SF.GetDirectionForce(origin, transform.position)*knockback);
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        if (lifeBar != null) { Destroy(lifeBar.gameObject); }
    }

    public void AttackCooldown() {
        StartCoroutine(AttackCooldown(attackCooldown));
    }

    IEnumerator AttackCooldown(float time) {
        anim.SetBool("CanAttack", false);
        yield return new WaitForSeconds(time);
        anim.SetBool("CanAttack", true);
    }
}
