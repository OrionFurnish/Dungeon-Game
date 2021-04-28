using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarControl : MonoBehaviour {
    public Image barImage;
    private float cur = 1, max = 1;

    private void Awake() {
        Set(cur, max);
    }

    public void Set(float cur, float max) {
        this.cur = cur;
        this.max = max;
        UpdateBar();
    }

    private void UpdateBar() {
        float percent;
        if (cur >= 0) { percent = cur / max; }
        else { percent = 0; }
        barImage.fillAmount = percent;
    }
}
