using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			Application.LoadLevel("MainMenu");
		}
	}
}