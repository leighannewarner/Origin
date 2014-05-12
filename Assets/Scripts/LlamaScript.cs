using UnityEngine;
using System.Collections;

public class LlamaScript : MonoBehaviour {

	public float h = 1.0f;
	public bool nearPlayer = false;
	public int shotDelay = 0;
	public GameObject bullet;
	public float speed = 0.01f;
	GameObject shotTransform;
	private Animator animator;
	private GameObject player;
	private bool attacking;

	Transform GroundCheckRight; //Right Ground detector
	Transform GroundCheckLeft;  //Left Ground Detector
	Transform RightCheck;       //Right Wall Detector
	Transform LeftCheck;		//Left Wall Detector
	Transform CeilingCheck;		//Ceiling Detector.

	bool RightEdge;
	bool LeftEdge;
	int delay = 0;

	void Awake() {
		animator = transform.Find("LlamaAnimations").GetComponent<Animator>();
		player = GameObject.Find("Player");
		Transform GroundCheckRight=transform.Find("Detector.RightGround");
		Transform GroundCheckLeft=transform.Find("Detector.LeftGround");
		Transform RightCheck=transform.Find("Detector.Right");
		Transform LeftCheck=transform.Find("Detector.Left");

	}

	void FixedUpdate () {	
		animator.transform.localScale = new Vector3((Mathf.Abs(transform.localScale.x) * h * -1f),transform.localScale.y,transform.localScale.z);

		if (!attacking) {
			rigidbody2D.transform.Translate( speed * h, 0f, Time.deltaTime);
		}
		// Should pace all the time
		// If near the player...
		// Shoot the player
		// then Go towards the player


		if (nearPlayer) {
			if(shotDelay <= 58) {
				fire();
			} else if (shotDelay > 0){
				animator.SetBool ("attacking", false);
				attacking = false;
				shotDelay--;
			} else {
				shotDelay--;
			}


			//Move towards player
			if (shotDelay >= 0) {
				if (player.transform.position.x < this.transform.position.x) {
					h = -1.0f;
					delay = 0;
				} else {
					h = 1.0f;
					delay = 0;
				}
			}

		} else {
			animator.SetBool ("attacking", false);
			attacking = false;
			shotDelay = 0;

			LeftEdge = !(Physics2D.Linecast(transform.position-new Vector3(0.5f,0,0),transform.Find("Detector.LeftGround").position,1<<LayerMask.NameToLayer("Terrain")));
			RightEdge = !(Physics2D.Linecast(transform.position-new Vector3(-0.5f,0,0),transform.Find("Detector.RightGround").position,1<<LayerMask.NameToLayer("Terrain")));
			
			if( LeftEdge && delay <= 0) {
				h = -1.0f;
				delay = 10;
			} else if ( RightEdge && delay <= 0) {
				h = 1.0f;
				delay = 10;
			}
		}

		Debug.Log("Delay: " + delay);
		delay--;
	}

	void OnCollisionStay2D(Collision2D c) {
		if(c.gameObject.tag == "Wall" && !nearPlayer && shotDelay <= 0 && !LeftEdge && !RightEdge && delay <= 0) {
			Debug.Log ("hitting the wall");
			h *= -1.0f;
			delay = 10;
		} else if (c.gameObject.tag == "Player") {
			Application.LoadLevel("MainMenu");
		}
	}

	void fire() {
		animator.SetBool ("attacking", true);
		attacking = true;
		shotTransform = ((Instantiate(bullet, new Vector2(transform.position.x+0.05f, transform.position.y+0.8f), Quaternion.identity)) as GameObject);
		shotTransform.GetComponent<BulletScript>().targetX = player.transform.position.x;
		shotTransform.GetComponent<BulletScript>().targetY = player.transform.position.y;
		shotDelay = 100;
	}

}
