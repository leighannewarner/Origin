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
	private bool onEnemy = false;
	
	private Transform leftWallDetector;
	private Transform rightWallDetector;
	private bool leftWalled = false;
	private bool rightWalled = false;
	
	public int size = 2;
	public float sizeScaleFactor = 0.5f;
	
	public int health = 100;
	public int damage = 1;
	
	private Animator animator;
	public Transform currentCheckpoint;
	
	void Awake()
	{
		// Setting up references.
		groundDetector = transform.Find("GroundDetector");
		leftWallDetector = transform.Find("LeftWallDetector");
		rightWallDetector = transform.Find("RightWallDetector");
		animator = transform.Find("PlayerAnimations").GetComponent<Animator>();
		Physics2D.IgnoreLayerCollision(10, 11, true);
	}
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Terrain")); 
		onEnemy = Physics2D.Linecast (transform.position, groundDetector.position, 1 << LayerMask.NameToLayer("Enemies"));
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
		
		animator.SetBool ("grounded", grounded);
		if(h > 0) {
			animator.SetInteger("direction", 1);
		} else if (h < 0) {
			animator.SetInteger("direction", -1);
		} else {
			animator.SetInteger("direction", 0);
		}
		
		//Sticks to the wall or applies the force horizontally to make him walk
		if(leftWalled && size < 3 && !grounded) {
			if(h * rigidbody2D.velocity.x < maxSpeed) 			
				rigidbody2D.AddForce(Vector2.up * h * moveForce); 		 		
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed);
		} else if(rightWalled && size < 3 && !grounded) {
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
				if(rigidbody2D.velocity.y > 0) {
					rigidbody2D.AddForce(new Vector2((-1f*jumpForce/2), Mathf.Sqrt(jumpForce)));
				} else {
					rigidbody2D.AddForce(new Vector2((-1f*jumpForce/2), Mathf.Sqrt(jumpForce)));
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y*-1);
				}
				jump = false;
			} else if(rightWalled && !grounded) {
				if(rigidbody2D.velocity.y < 0) {
					rigidbody2D.AddForce(new Vector2((jumpForce), Mathf.Sqrt(jumpForce*4)));
				} else {
					rigidbody2D.AddForce(new Vector2((jumpForce), Mathf.Sqrt(jumpForce*4)));
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y*-1);
				}
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
		if(c.gameObject.name == "Grate" && (size == 3 || c.gameObject.GetComponent<GrateScript>().broken)) {
			c.gameObject.collider2D.enabled = false;
		}
	}
	
	void OnCollisionEnter2D (Collision2D c) {
		//Debug.Log ("Collision enter.");
		if(c.gameObject.tag == "Enemy") {
			if(!onEnemy) {
				health -= (c.gameObject.GetComponent<EnemyScript>()).damage;
				
				if(health <= 0) {
					Destroy (this.gameObject);
				}
				Debug.Log ("take damage!");
			} else {
				(c.gameObject.GetComponent<EnemyScript>()).takeDamage(getDamage());
				Debug.Log ("damage enemy!");
			}
		} else if(c.gameObject.tag == "Respawn") {
			teleportToCheckpoint();
		}
	}
	
	int getDamage() {
		return damage;
	}
	
	public void teleportToCheckpoint() {
		this.transform.position = this.currentCheckpoint.position;
	}
	
	public void setCheckpoint(Transform checkpoint) {
		this.currentCheckpoint = checkpoint;
		
	}
	
}
