using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {
    public Transform lifeBarPrefab;
    Transform worldSpaceCanvas;

    protected override void Awake() {
        base.Awake();
        worldSpaceCanvas = GameObject.Find("World Space Canvas").transform;
        lifeBar = Instantiate(lifeBarPrefab, worldSpaceCanvas).GetComponent<BarControl>();
        lifeBar.GetComponent<Follow>().target = transform;
        lifeBar.Set(stats.life, stats.maxLife);
        GameManager.instance.AddEnemy(this);
    }

    /** Use in FixedUpdate */
    public void Move(Vector3 target) {
        SF.MoveTowards(rb, target, stats.moveSpeed*Time.fixedDeltaTime);
    }

    public void MoveAway(Vector3 target) {
        SF.MoveAway(rb, target, stats.moveSpeed * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 target) {
        transform.up = target - transform.position;
    }

    protected override void Die() {
        GameManager.instance.RemoveEnemy(this);
        base.Die();
    }
}
