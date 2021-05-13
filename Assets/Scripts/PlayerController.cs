using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Controller {
    public static PlayerController instance;

    public float dodgeForce;
    public float dodgeTime;
    public float dodgeCooldown;

    private IEnumerator currentDodge = null;

    private void Start() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        lifeBar.Set(stats.life, stats.maxLife);
    }

    private void Update() {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if (!attacking) {
            Vector3 movement = dir.normalized * stats.moveSpeed * Time.deltaTime;
            anim.SetBool("IsWalking", movement.magnitude > 0);
            rb.AddForce(movement);

            float rotation;
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
        

        if(Input.GetKeyDown(KeyCode.LeftShift) && currentDodge == null) {
            currentDodge = Dodge(dir.normalized);
            StartCoroutine(currentDodge);
        }
    }

    public IEnumerator Dodge(Vector3 direction) {
        float startTime = Time.time;
        while(startTime + dodgeTime > Time.time) {
            Vector3 movement = direction * dodgeForce * Time.fixedDeltaTime;
            rb.AddForce(movement);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(dodgeCooldown);
        currentDodge = null;
    }

    protected override void Die() {
        SceneManager.LoadScene("Scene1");
    }
}
