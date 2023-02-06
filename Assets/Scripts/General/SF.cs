using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Static Functions
public static class SF {
	public static void MoveTowards(Rigidbody2D rb, Vector3 targetPos, float speed) {
		rb.AddForce(GetDirectionForce(rb.gameObject.transform.position, targetPos)*speed);
	}

	public static void MoveAway(Rigidbody2D rb, Vector3 targetPos, float speed) {
		rb.AddForce(-GetDirectionForce(rb.gameObject.transform.position, targetPos)*speed);
	}

	public static Vector3 GetDirectionForce(Vector3 sPos, Vector3 tPos) {
		if(sPos == tPos) {return new Vector3(0, 0, 0);}
		Vector3 dif = tPos - sPos;
		return dif.normalized;
	}

	public static bool GetWithinRange(Vector3 pos1, Vector3 pos2, float range) {return Vector3.Distance(pos1, pos2) <= range;}

    public static bool GetPastOrWithinRange(Vector3 sPos, Vector3 tPos, float range, Vector3 dir) {
        if(Vector3.Distance(sPos, tPos) <= range) { return true; } // Within Range
        Vector3 newDir = GetDirectionForce(sPos, tPos);
        return dir.x * newDir.x < 0 || dir.y * newDir.y < 0;
    }

    public static Vector3 Midpoint(Vector3 pos1, Vector3 pos2, float zVal) {
        return new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, zVal);
    }

    public static List<RaycastResult> UIMouseRaycast() {
        PointerEventData pointerData = new PointerEventData(EventSystem.current) {pointerId = -1,};
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }

    public static void LookAt(Transform t, Vector3 target) {
        t.up = target - t.position;
    }
}
