using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	private bool didTheThing = false;

	void OnTriggerStay2D (Collider2D c) {
		if(c.gameObject.tag == "Player" && !didTheThing) {
			Debug.Log ("DO THE THING!");
			didTheThing = true;
		}
	}
}
