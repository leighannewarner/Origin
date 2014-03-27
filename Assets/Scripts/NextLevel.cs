using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D c) {
		if(c.gameObject.tag == "Player") {
			Application.LoadLevel("Level1");
		}
	}
}
