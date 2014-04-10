using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float bulletVelocity = 50f;

	void FixedUpdate() {
		move.direction = this.transform.right;
	}

	void Start()
	{
		// Limited time to live to avoid any leak
		Destroy(gameObject, 20); // 20sec
	}
	
	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player" || c.gameObject.tag == "Terrain") {
			Destroy(gameObject);
		}
	}
}
