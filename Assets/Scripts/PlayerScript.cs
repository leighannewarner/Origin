using UnityEngine;
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
	public float speedx = 0f;
	public float speedy = 0f;
	
	private Transform groundDetector;
	private bool grounded = false;	

	private Transform leftWallDetector;
	private Transform rightWallDetector;
	private bool leftWalled = false;
	private bool rightWalled = false;

	public int size = 2;
	public float sizeScaleFactor = 0.5f;

	public int health = 100;
	
	void Awake()
	{
		// Setting up references.
		groundDetector = transform.Find("GroundDetector");
		leftWallDetector = transform.Find("LeftWallDetector");
		rightWallDetector = transform.Find("RightWallDetector");
	}
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Terrain")); 
		leftWalled = Physics2D.Linecast(transform.position, rightWallDetector.position, 1 << LayerMask.NameToLayer("Terrain"));
		rightWalled = Physics2D.Linecast(transform.position, leftWallDetector.position, 1 << LayerMask.NameToLayer("Terrain")); 
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump")) {
			if(grounded || leftWalled || rightWalled) {
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
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		if(leftWalled && size < 3) {
			if(h * rigidbody2D.velocity.x < maxSpeed) 			
				rigidbody2D.AddForce(Vector2.up * h * moveForce); 		 		
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed);
		} else if(rightWalled && size < 3) {
			if(Mathf.Abs(h * rigidbody2D.velocity.x) < maxSpeed) 			
				rigidbody2D.AddForce(Vector2.up * h * moveForce * -1f); 		 		
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed *-1);
		} else {
			if(h * rigidbody2D.velocity.x < maxSpeed) 			
				rigidbody2D.AddForce(Vector2.right * h * moveForce); 		 		
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}

		// If the player should jump...
		if(jump)
		{	
			if(leftWalled && !grounded) {
				rigidbody2D.AddForce(new Vector2(-1f*Mathf.Sqrt(jumpForce*4), Mathf.Sqrt(jumpForce*4)));
				jump = false;
			} else if(rightWalled && !grounded) {
				rigidbody2D.AddForce(new Vector2(Mathf.Sqrt(jumpForce*4), Mathf.Sqrt(jumpForce*4)));
				jump = false;
			} else {
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				jump = false;
			}
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

	void OnCollisionStay2D (Collision2D c) {
		if(c.gameObject.name == "Grate" && size == 3) {
			c.gameObject.collider2D.enabled = false;
		}
		//Debug.Log ("Collision");
	}

	void OnCollisionEnter2D (Collision2D c) {
		if(c.gameObject.tag == "Enemy") {
			health--;
			Debug.Log (health);

			if(health == 0) {
				Destroy (this.gameObject);
			}
		}
	}
}
