using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFloat : MonoBehaviour {
    TextMeshProUGUI text;
    public float floatSpeed;
    public float fadeSpeed;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
        transform.position += new Vector3(0f, 20f, 0f);
    }

    private void Update() {
        transform.position += new Vector3(0f, 0, floatSpeed * Time.deltaTime);
        text.color -= new Color(0f, 0f, 0f, fadeSpeed*Time.deltaTime);
        if (text.color.a <= 0) {
            Destroy(gameObject);
        }
    }
}
