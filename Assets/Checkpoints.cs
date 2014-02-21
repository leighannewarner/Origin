using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {

	void onTriggerEnter(Collider2D c) {
		Debug.Log ("Trigger enter.");
		try {
			(c.gameObject.GetComponent<PlayerScript>()).setCheckpoint(c.gameObject.transform);
			Debug.Log ("Done!");
		} catch(MissingComponentException) {
			Debug.Log ("Missing component!");
		}
	}

	void onColliderEnter(Collision2D c) {
		Debug.Log ("Collider enter.");
		try {
			(c.gameObject.GetComponent<PlayerScript>()).setCheckpoint(c.gameObject.transform);
			Debug.Log ("Done!");
		} catch(MissingComponentException) {
			Debug.Log ("Missing component!");
		}
	}
}
