using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float bulletVelocity = 0.025f;
	public float startX = 1.0f;
	public float startY = 1.0f;
	public float targetX;
	public float targetY;

	public float hx = 1.0f;
	public float hy = 1.0f;
	public float tolerance = 0.05f;
	public bool onlyX = false;
	public bool onlyY = false;

	public float slopeX;
	public float slopeY;

	void Awake() {
		startX = this.transform.position.x;
		startY = this.transform.position.y;

		targetX = GameObject.Find("Player").transform.position.x;
		targetY = GameObject.Find("Player").transform.position.y;

		slopeY = Mathf.Abs (((targetY - startY)/(targetX - startX)));

		if(slopeY > 1) {
			bulletVelocity = bulletVelocity/slopeY;
		}

		if (targetX > startX) {
			hx = 1.0f;
		} else {
			hx = -1.0f;
		}

		if (targetY > startY) {
			hy = 1.0f;
		} else {
			hy = -1.0f;
		}
	}

	void FixedUpdate() {

		rigidbody2D.transform.Translate( bulletVelocity * hx, bulletVelocity * hy * slopeY, Time.deltaTime);
	}

	void Start()
	{
		// Limited time to live to avoid any leak
		Destroy(gameObject, 20); // 20sec
	}
	
	void OnTriggerEnter2D (Collider2D c) {
		if(c.gameObject.tag == "Player") {
			Destroy (this.gameObject);
			GameObject.Find("Player").GetComponent<HeroControl>().getHit();
		} else if (c.gameObject.tag == "Terrain" || c.gameObject.tag == "Ceiling" || c.gameObject.tag == "Wall") {
			Destroy (this.gameObject);
		}
	}
}
