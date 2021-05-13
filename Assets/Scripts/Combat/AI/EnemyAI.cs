using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float targetDistanceMin, targetDistanceMax, attackDistance;
    public float attackChance;
    public float attackChargeTime;
    public Color chargeColor;

    protected EnemyController controller;
    protected SpriteRenderer sr;
    protected Color baseColor;

    private void Start() {
        controller = GetComponent<EnemyController>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
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
        controller.LookAt(PlayerController.instance.transform.position);
        // Start Charge
        controller.attacking = true;
        sr.color = chargeColor;
        yield return new WaitForSeconds(attackChargeTime);
        // Attack
        PerformAttack();
        while(controller.attacking) {
            yield return null;
        }
        sr.color = baseColor;
    }

    protected virtual void PerformAttack() {
        controller.anim.SetTrigger("Attack");
    }
}
