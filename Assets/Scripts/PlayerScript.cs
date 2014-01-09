﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller and behavior.
/// </summary>

public class PlayerScript : MonoBehaviour {

	[HideInInspector]
	public bool jump = false;			
	
	public float moveForce = 365f;			
	public float maxSpeed = 5f;			
	public float jumpForce = 1000f;			
	
	private Transform groundDetector;		
	private bool grounded = false;	

	public int size = 2;
	public float sizeScaleFactor = 0.5f;
	
	void Awake()
	{
		// Setting up references.
		groundDetector = transform.Find("GroundDetector");
	}
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Terrain"));  
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump")) {
			if(grounded) {
				jump = true;
			}
		}

		if(Input.GetMouseButtonDown(0) && size > 1) {
			decreaseSize();
		} else if(Input.GetMouseButtonDown(1) && size < 3) {
			increaseSize();
		}
	}
	
	void FixedUpdate ()
	{
		Debug.Log("Player " + grounded);
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
		if(h * rigidbody2D.velocity.x < maxSpeed) 			
			rigidbody2D.AddForce(Vector2.right * h * moveForce); 		 		
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		
		// If the player should jump...
		if(jump)
		{	
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}

	void decreaseSize() {
		size -= 1;
		transform.localScale -= new Vector3(sizeScaleFactor,sizeScaleFactor,0);
	}
	
	void increaseSize() {
		size += 1;
		transform.localScale += new Vector3(sizeScaleFactor,sizeScaleFactor,0);

	}
}
