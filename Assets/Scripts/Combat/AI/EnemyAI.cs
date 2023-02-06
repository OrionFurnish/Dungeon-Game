using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float targetDistanceMin, targetDistanceMax, attackDistance;
    public float attackChance;
    public float attackChargeTime;

    protected EnemyController controller;

    private void Start() {
        controller = GetComponent<EnemyController>();
        StartCoroutine(ChooseAction());
    }

    protected virtual IEnumerator ChooseAction() {
        while (gameObject.activeSelf) {
            IEnumerator currentAction = null;
            float r = Random.Range(0f, 100f);
            if (r <= attackChance) {
                currentAction = Attack();
            }

            if (currentAction != null) {
                yield return StartCoroutine(currentAction);
            } else {
                MaintainDistance();
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void MaintainDistance() {
        if (!SF.GetWithinRange(transform.position, PlayerController.instance.transform.position, targetDistanceMax)) {
            controller.Move(PlayerController.instance.transform.position);
        }
        else if (SF.GetWithinRange(transform.position, PlayerController.instance.transform.position, targetDistanceMin)) {
            controller.MoveAway(PlayerController.instance.transform.position);
        }
    }

    public IEnumerator Attack() {
        float startTime = Time.time;
        yield return new WaitForFixedUpdate();
        while (!SF.GetWithinRange(transform.position, PlayerController.instance.transform.position, attackDistance)) {
            controller.Move(PlayerController.instance.transform.position);
            yield return new WaitForFixedUpdate();
        }
        // Start Charge
        controller.attacking = true;
        controller.anim.SetBool("Charging", true);
        yield return new WaitForSeconds(attackChargeTime);
        controller.anim.SetBool("Charging", false);
        // Attack
        PerformAttack();
        while(controller.attacking) {
            yield return null;
        }
    }

    protected virtual void PerformAttack() {
        controller.anim.SetTrigger("Attack");
    }
}
