using UnityEngine;
using System.Collections;

public class LeftGroundDetector : MonoBehaviour {

	void OnCollisionExit2D (Collision2D c) {
		if (c.gameObject.tag == "Terrain") {
			Debug.Log ("Left edge.");
		}
	}
}
