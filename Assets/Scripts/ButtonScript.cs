using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	private bool didTheThing = false;
	public GameObject gate;

	void OnTriggerStay2D (Collider2D c) {
		if((c.gameObject.tag == "Player") && !didTheThing) {
			transform.localScale = new Vector3(0.25f, 0.07f, 0f);
			(gate.gameObject.GetComponent<GateScript>()).openGate();
			didTheThing = true;
		}
	}
}
