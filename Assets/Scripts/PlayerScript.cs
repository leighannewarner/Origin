using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller and behavior.
/// </summary>

public class PlayerScript : MonoBehaviour {

	/// <summary>
	/// The speed of the ship
	/// </summary>
	public Vector2 speedVector = new Vector2(50, 50);

	//The movement speed and direction
	private Vector2 movement = new Vector2(0, 0);

	/// <summary>
	/// Starting size
	/// </summary>
	public int size = 2;
	public int speedScaleFactor = 2;
	public float sizeScaleFactor = 0.5f;

	//Jumping
	private bool jumping = false;
	private bool falling = true;
	private int jumpFrame = 0;
	private int jumpCap = 15;

	// Update is called once per frame
	void Update () {
		//Movement
		float inputX = Input.GetAxis("Horizontal");
		movement.x = speedVector.x * inputX;

		/*if(jumping && jumpFrame < jumpCap) {
			movement.y += speedVector.y * 0.05f;
			jumpFrame++;

			if(jumpFrame >= jumpCap) {
				jumping = false;
				movement.y = 0f;
				falling = true;
			}
		}

		if(falling && jumpFrame > 0) {
			movement.y -= speedVector.y * 0.05f;
			jumpFrame--;
			
			if(jumpFrame <= 0) {
				movement.y = 0f;
				falling = false;
			}
		}*/

		if(falling) {
			movement.y -= speedVector.y * 0.005f;
		}

		//User input
		if(Input.GetMouseButtonDown(0) && size > 1) {
			decreaseSize();
		} else if(Input.GetMouseButtonDown(1) && size < 3) {
			increaseSize();
		}

		if(Input.GetKeyUp("space")) {
			jumping = true;
		}
	}

	void FixedUpdate() {
		// Set the movement
		rigidbody2D.velocity = movement;

	}

	void decreaseSize() {
		size -= 1;
		transform.localScale -= new Vector3(sizeScaleFactor,sizeScaleFactor,0);
		
		if (size == 2) {
			speedVector.x = (50/speedScaleFactor)*2;
			speedVector.y = (50/speedScaleFactor)*2;
		} else {
			speedVector.x = (50/speedScaleFactor);
			speedVector.y = (50/speedScaleFactor);
		}
	}

	void increaseSize() {
		size += 1;
		transform.localScale += new Vector3(sizeScaleFactor,sizeScaleFactor,0);
		
		if (size == 2) {
			speedVector.x = (50/speedScaleFactor)*2;
			speedVector.y = (50/speedScaleFactor)*2;
		} else {
			speedVector.x = (50/speedScaleFactor);
			speedVector.y = (50/speedScaleFactor);
		}
	}

	void OnCollisionStay2D (Collision2D c) {
		if(c.gameObject.name == "CollisionTile") {
			falling = false;
			//Debug.Log (c.gameObject.transform.position + " , " + this.gameObject.transform.position);
			Debug.Log ("X: " + c.gameObject.transform.position.x + " , " + (this.gameObject.transform.position.x));
			Debug.Log ("Y: " + c.gameObject.transform.position.y + " , " + (this.gameObject.transform.position.y));
			Debug.Log ("Height: " + this.gameObject.transform.localScale);
		}
	}


	void OnCollisionExit2D (Collision2D c) {
		if(c.gameObject.name == "CollisionTile") {
			falling = true;
		}
	}
}
