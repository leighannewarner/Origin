﻿using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	public string nextLevel = "Level1";
	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			Application.LoadLevel(nextLevel);
		}
	}
}
