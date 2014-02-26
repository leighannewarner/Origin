using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {

	void OnTriggerStay2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			try {
				Debug.Log (c.gameObject.transform);
				(c.gameObject.GetComponent<PlayerScript>()).setCheckpoint(this.gameObject.transform);
			} catch(MissingComponentException) {
				Debug.Log ("Missing component!");
			}
		}
	}
}
