using UnityEngine;
using System.Collections;

public class DogeScript : MonoBehaviour {

	public bool nearPlayer = false;
	public float maxSpeed = 100f;
	public float currentMaxSpeed = 100f;
	public float moveForce = 100f;
	public int windUp = 0;
	public int backItUp = 0;
	public float h = 1.0f;

	void Awake()
	{
		currentMaxSpeed = maxSpeed;
	}

	void FixedUpdate ()
	{	
		if (nearPlayer) {
			if(windUp > 0) {
				backItUp = 0;
				windUp--;
			} else if (backItUp > 0) {
				Debug.Log("Back it up!");
				backItUp--;
				rigidbody2D.AddForce(Vector2.right * moveForce * h * -1.0f);
				if(backItUp == 0) {
					currentMaxSpeed = maxSpeed;
				}
			} else {
				if(Mathf.Abs(rigidbody2D.velocity.x) < currentMaxSpeed) {			
					rigidbody2D.AddForce(Vector2.right * moveForce * h); 	
				}
				
				if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
					rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * currentMaxSpeed * h, rigidbody2D.velocity.y);
				}
			}
		} else {
			rigidbody2D.velocity = Vector2.zero;
		}
	}


	public void takeDamage(int d) {

	}
	
	void OnCollisionEnter2D (Collision2D c) {
		if(c.gameObject.tag == "Player") {
			windUp = 0;
			backItUp = 10;
			currentMaxSpeed = (currentMaxSpeed/2);
		}
	}
	
	void OnCollisionExit2D (Collision2D c) {

	}
}