using UnityEngine;
using System.Collections;

public class LlamaPlayerDetector : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<LlamaScript>()).nearPlayer = true;

			if(c.transform.position.x > this.transform.position.x) {
				(gameObject.transform.parent.GetComponent<LlamaScript>()).h = 1.0f;
			} else {
				(gameObject.transform.parent.GetComponent<LlamaScript>()).h = -1.0f;
			}
		}
	}
	
	void OnTriggerExit2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<LlamaScript>()).nearPlayer = false;

			if(c.transform.position.x > this.transform.position.x) {
				(gameObject.transform.parent.GetComponent<LlamaScript>()).h = 1.0f;
			} else {
				(gameObject.transform.parent.GetComponent<LlamaScript>()).h = -1.0f;
			}
		}
	}
}
