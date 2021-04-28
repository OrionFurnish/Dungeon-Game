using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform target;
    public Vector3 barOffset;

    private void Start() {
        transform.position = target.transform.position + barOffset;
    }

    void Update() {
        transform.position = target.transform.position + barOffset;
    }
}
