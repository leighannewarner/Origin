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

	// Update is called once per frame
	void Update () {
		//Movement
		float inputX = Input.GetAxis("Horizontal");
		movement.x = speedVector.x * inputX;

		//Growing and shrinking
		if(Input.GetMouseButtonDown(0) && size > 1) {
			decreaseSize();
			Debug.Log (speedVector);
		} else if(Input.GetMouseButtonDown(1) && size < 3) {
			increaseSize();
			Debug.Log (speedVector);
		}

		if(Input.GetKeyUp("space")) {
			Debug.Log ("Space key.");
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
}
