using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {
    public Transform lifeBarPrefab;
    Transform worldSpaceCanvas;

    private void Start() {
        worldSpaceCanvas = GameObject.Find("World Space Canvas").transform;
        lifeBar = Instantiate(lifeBarPrefab, worldSpaceCanvas).GetComponent<BarControl>();
        lifeBar.GetComponent<Follow>().target = transform;
        lifeBar.Set(stats.life, stats.maxLife);
    }

    /** Use in FixedUpdate */
    public void Move(Vector3 target) {
        SF.MoveTowards(rb, target, stats.moveSpeed*Time.fixedDeltaTime);
    }

    public void MoveAway(Vector3 target) {
        SF.MoveAway(rb, target, stats.moveSpeed * Time.fixedDeltaTime);
    }
}
