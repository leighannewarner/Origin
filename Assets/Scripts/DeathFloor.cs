﻿using UnityEngine;
using System.Collections;

public class DeathFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy (col.gameObject);

		if(col.gameObject.tag == "Player") {
			Application.LoadLevel("MainMenu");
		}
	}
}