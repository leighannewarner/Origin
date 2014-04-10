using UnityEngine;
using System.Collections;

public class DogePlayerDetectorScript : MonoBehaviour {

	void Update() {
		//Debug.Log (".");
	}

	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<DogeScript>()).nearPlayer = true;
			(gameObject.transform.parent.GetComponent<DogeScript>()).windUp = 50;
			if(c.transform.position.x > this.transform.position.x) {
				(gameObject.transform.parent.GetComponent<DogeScript>()).h = 1.0f;
			} else {
				(gameObject.transform.parent.GetComponent<DogeScript>()).h = -1.0f;
			}
		}
	}

	void OnTriggerExit2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<DogeScript>()).nearPlayer = false;
			(gameObject.transform.parent.GetComponent<DogeScript>()).windUp = 0;
			(gameObject.transform.parent.GetComponent<DogeScript>()).backItUp = 0;
		}
	}
}
