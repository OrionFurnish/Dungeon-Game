using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    public static PlayerController instance;
    public float attackCooldown;

    private void Start() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        lifeBar.Set(stats.life, stats.maxLife);
    }

    private void Update() {
        if(!attacking) {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            Vector3 movement = dir.normalized * stats.moveSpeed * Time.deltaTime;
            rb.AddForce(movement);

            float rotation = 0;
            if (movement.x > 0) {
                rotation = 270;
                if (movement.y > 0) {
                    rotation += 45;
                }
                else if (movement.y < 0) {
                    rotation -= 45;
                }
                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
            else if (movement.x < 0) {
                rotation = 90;
                if (movement.y > 0) {
                    rotation -= 45;
                }
                else if (movement.y < 0) {
                    rotation += 45;
                }
                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
            else if (movement.y > 0) {
                rotation = 0;
                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
            else if (movement.y < 0) {
                rotation = 180;
                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetTrigger("Attack");
        }
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
