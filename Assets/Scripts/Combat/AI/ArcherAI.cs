using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : EnemyAI {
    public Transform projectile;

    protected override void PerformAttack() {
        Transform t = Instantiate(projectile, transform.parent);
        t.GetComponent<Projectile>().target = PlayerController.instance.transform;
        t.position = transform.position;
        SF.LookAt(t, PlayerController.instance.transform.position);
        controller.attacking = false;
    }
}
