using UnityEngine;
using System.Collections;

public class PlayerDetectorScript : MonoBehaviour {

	void Update() {
		//Debug.Log (".");
	}

	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<EnemyScript>()).nearPlayer = true;
			(gameObject.transform.parent.GetComponent<EnemyScript>()).maxSpeed *= 2;
		}
	}

	void OnTriggerExit2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			(gameObject.transform.parent.GetComponent<EnemyScript>()).nearPlayer = false;
			(gameObject.transform.parent.GetComponent<EnemyScript>()).maxSpeed /= 2;
		}
	}
}
