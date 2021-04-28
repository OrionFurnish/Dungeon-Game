using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float targetDistanceMin, targetDistanceMax;

    private EnemyController controller;

    private void Start() {
        controller = GetComponent<EnemyController>();
        StartCoroutine(ChooseAction());
    }

    IEnumerator ChooseAction() {
        while (gameObject.activeSelf) {
            IEnumerator currentAction = MaintainDistance();
            yield return StartCoroutine(currentAction);
        }
    }

    public IEnumerator MaintainDistance() {
        while (!SF.GetWithinRange(transform.position, PlayerController.instance.transform.position, targetDistanceMax)) {
            controller.Move(PlayerController.instance.transform.position);
            yield return new WaitForFixedUpdate();
        }
        while (SF.GetWithinRange(transform.position, PlayerController.instance.transform.position, targetDistanceMin)) {
            controller.MoveAway(PlayerController.instance.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Attack() {
        yield return null;
    }

    public IEnumerator LungeAttack() {
        yield return null;
    }

    public IEnumerator Flee() {
        yield return null;
    }
}
