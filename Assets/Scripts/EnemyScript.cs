using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller and behavior.
/// </summary>

public class EnemyScript : MonoBehaviour {
	
	[HideInInspector]
	public bool jump = false;			
	
	public float moveForce = 365f;
	public float maxSpeed = 5f;	
	public float currentMaxSpeed = 2f;
	public float jumpForce = 1000f;
	
	//private Transform groundDetector;		
	//private bool grounded = false;
	private Transform leftWallDetector;
	private Transform rightWallDetector;
	private bool walled = false;
	public bool nearPlayer = false;
	
	public int size = 2;
	public float sizeScaleFactor = 0.5f;

	private float h = 1.0f;

	public int damage = 1;
	public int health = 3;
	//private int attack = 0;
	//private int delay = 1;

	void Awake()
	{
		// Setting up references.
		//groundDetector = transform.Find("GroundDetector");
		leftWallDetector = transform.Find("LeftWallDetector");
		rightWallDetector = transform.Find("RightWallDetector");
		currentMaxSpeed = maxSpeed;
	}
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//grounded = Physics2D.Linecast(transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Terrain"));  
		walled = (Physics2D.Linecast(transform.position, rightWallDetector.position, 1 << LayerMask.NameToLayer("Terrain")) || Physics2D.Linecast(transform.position, leftWallDetector.position, 1 << LayerMask.NameToLayer("Terrain"))); 
	}
	
	void FixedUpdate ()
	{
		if(walled) {
			h *= -1.0f;
		}

		if(h * rigidbody2D.velocity.x < currentMaxSpeed) {			
			rigidbody2D.AddForce(Vector2.right * h * moveForce); 	
		}

		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * currentMaxSpeed, rigidbody2D.velocity.y);
		}

		Debug.Log (currentMaxSpeed);
	}

	public void takeDamage(int d) {
		health -= d;

		if(health <= 0) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D c) {
		if(c.gameObject.tag == "Player") {
			h *= -1.0f;
		}
	}

	void OnCollisionExit2D (Collision2D c) {
		if(c.gameObject.tag == "Player") {
			currentMaxSpeed = maxSpeed;
		}
	}


	public void reverse() {
		h *= -1.0f;
	}
}
