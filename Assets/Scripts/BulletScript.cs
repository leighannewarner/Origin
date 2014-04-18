using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float bulletVelocity = 0.1f;
	public float h = 1.0f;

	void FixedUpdate() {
		rigidbody2D.transform.Translate(bulletVelocity * h, 0, Time.deltaTime);
	}

	void Start()
	{
		// Limited time to live to avoid any leak
		Destroy(gameObject, 30); // 20sec
	}
	
	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player" || c.gameObject.tag == "Terrain") {
			Application.LoadLevel("MainMenu");
		}
	}
}
