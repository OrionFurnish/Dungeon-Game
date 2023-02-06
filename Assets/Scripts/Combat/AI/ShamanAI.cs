using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanAI : ArcherAI {
    public float attack2Chance;
    public float attack2ChargeTime;
    public Transform attack2Prefab;
    public float attack2SpawnDelay;
    public float attack2SpawnIncrement;
    public float healChargeTime;
    public float healChance;
    public float healDistance;
    public float healAmount;

    protected override IEnumerator ChooseAction() {
        while (gameObject.activeSelf) {
            IEnumerator currentAction = null;
            float r = Random.Range(0f, 100f);
            if (r <= attackChance) {
                currentAction = Attack();
            }
            else if (r <= attackChance + attack2Chance) {
                currentAction = Attack2();
            }
            else if (r <= attackChance + attack2Chance + healChance) {
                currentAction = Heal();
            }

            if (currentAction != null) {
                yield return StartCoroutine(currentAction);
            } else {
                MaintainDistance();
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public IEnumerator Heal() {
        EnemyController healTarget = GetHealTarget();
        while(healTarget != null && !SF.GetWithinRange(transform.position, healTarget.transform.position, healDistance)) {
            controller.Move(healTarget.transform.position);
            yield return new WaitForFixedUpdate();
        } if(healTarget != null) {
            controller.anim.SetBool("Charging", true);
            yield return new WaitForSeconds(attackChargeTime);
            if (healTarget != null) {
                // Heal
                healTarget.Heal(healAmount);
            }
            controller.anim.SetBool("Charging", false);
        }
    }

    private EnemyController GetHealTarget() {
        EnemyController target = null;
        float targetLifeRemaining = 1f;
        foreach(EnemyController possibleTarget in transform.parent.GetComponentsInChildren<EnemyController>()) {
            if (possibleTarget != controller) {
                float lifeRemaining = possibleTarget.stats.life / possibleTarget.stats.maxLife;
                if(lifeRemaining < targetLifeRemaining) {
                    target = possibleTarget;
                    targetLifeRemaining = lifeRemaining;
                }
            }
        }
        if(targetLifeRemaining < .75f) {
            return target;
        } else {
            return null;
        }
    }

    public IEnumerator Attack2() {
        controller.anim.SetBool("Charging", true);
        yield return new WaitForSeconds(attack2ChargeTime);
        controller.anim.SetBool("Charging", false);

        int wallLayerMask = LayerMask.GetMask("Wall");
        RaycastHit2D southHit = Physics2D.Raycast(transform.position, Vector2.down, 20f, wallLayerMask);
        RaycastHit2D northHit = Physics2D.Raycast(transform.position, Vector2.up, 20f, wallLayerMask);
        RaycastHit2D eastHit = Physics2D.Raycast(transform.position, Vector2.right, 20f, wallLayerMask);
        RaycastHit2D westHit = Physics2D.Raycast(transform.position, Vector2.left, 20f, wallLayerMask);

        Vector3 southIncrement = Vector2.down * attack2SpawnIncrement;
        Vector3 northIncrement = Vector2.up * attack2SpawnIncrement;
        Vector3 eastIncrement = Vector2.right * attack2SpawnIncrement;
        Vector3 westIncrement = Vector2.left * attack2SpawnIncrement;

        Vector3 currentSouth = transform.position + southIncrement;
        Vector3 currentNorth = transform.position + northIncrement;
        Vector3 currentEast = transform.position + eastIncrement;
        Vector3 currentWest = transform.position + westIncrement;

        while (currentSouth.y > southHit.point.y || currentNorth.y < northHit.point.y || currentEast.x < eastHit.point.x || currentWest.x > westHit.point.x) {
            // Spawn South
            if (currentSouth.y > southHit.point.y) {
                CreateAttack2(currentSouth);
                currentSouth += southIncrement;
            }
            // Spawn North
            if (currentNorth.y < northHit.point.y) {
                CreateAttack2(currentNorth);
                currentNorth += northIncrement;
            }
            // Spawn East
            if (currentEast.x < eastHit.point.x) {
                CreateAttack2(currentEast);
                currentEast += eastIncrement;
            }
            // Spawn West
            if (currentWest.x > westHit.point.x) {
                CreateAttack2(currentWest);
                currentWest += westIncrement;
            }

            yield return new WaitForSeconds(attack2SpawnDelay);
        }
    }

    private void CreateAttack2(Vector3 position) {
        Transform t = Instantiate(attack2Prefab, transform.parent);
        t.transform.position = position;
    }
}
