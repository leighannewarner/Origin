using UnityEngine;
using System.Collections;

/*Hero Control Script
Notes:
This script is NOT optimized. At all. FixedUpdate() Could probably use a good onceover to more efficiently order
the checks and assign forces. Additionally, Update should be changed to use the NoAlloc version of Physics2D.Linecst,
since the memory allocation and garbage collection is expensive. Additionally, setting each EdgeDetector of all platforms to
different layers, and linecasting specific detectors against only those layers, may also provide an avenue of optimization.
*/
public class HeroControl : MonoBehaviour {

	
	//Animation Controls
	private Animator animator;

	public float MaxHSpeed; //Maximum Horizontal Move Speed
	public float MaxVSpeed; //Maximum Vertical Move Speed
	public float MoveVMag;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
	public float MoveHMag;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
	public float JumpHMag;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
	public float JumpVMag;  //Vertical Magnitude of force applied when jumping off the ground. Scaled value is used when dismounting from the ceiling. 

	public float MaxHSpeed_const; //Maximum Horizontal Move Speed
	public float MaxVSpeed_const; //Maximum Vertical Move Speed
	public float MoveVMag_const;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
	public float MoveHMag_const;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
	public float JumpHMag_const;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
	public float JumpVMag_const;

	public float InAirHScale;

	
	//Detector Positions
	Transform GroundCheckRight; //Right Ground detector
	Transform GroundCheckLeft;  //Left Ground Detector
	Transform RightCheck;       //Right Wall Detector
	Transform LeftCheck;		//Left Wall Detector
	Transform CeilingCheck;		//Ceiling Detector.

	Vector2 MoveForce;          //Variable for holding controller Axis data. Here for memory allocation reasions
	
	bool TriggerJump;           //Indicates whether the player should jump in the next FixedUpdate call.
	bool TouchGround;           //Indicates wheather the player is touching the ground
	bool TouchWallToLeft;       //Indicates whether the player is touching a wall on its left side
	bool TouchWallToRight;      //Indicates whether the player is touching a wall on its right side
	bool TouchCeiling;          //Inidcates whether the player is touching the ceiling.
	bool WallStick;             //Inidcates whether the player should be stuck to a wall surface
	bool CeilingStick;          //Indicates whether the player should be stuck to the ceiling
	int size = 1;                  //Backing for Size property
	public float sizeScaleFactor = 0.5f;
	public float speedScaleFactor = 0.75f;
	
	//Shortcut for checking if the player is in freefall (Not touching any walls and is in the air)
	bool FreeFall
	{
		get{
		return !(TouchWallToRight||TouchWallToLeft||TouchCeiling||TouchGround);
		}
	}
	//Shortcut for checking if the player is stuck to the wall to its right.
	bool StuckOnWallToRight
	{
		get{
			return TouchWallToRight&&WallStick;
		}
	}
	//Shortchut for checking if the player is stuck to the wall to it's left
	bool StuckOnWallToLeft
	{
		get{
			return TouchWallToLeft&&WallStick;
		}
	}
	//Shortcut for checking if player is in the air.
	bool IsInAir
	{
		get{
			return !TouchGround;
		}
	}
	//Shortcut for checking if a jump can be triggered.
	bool CanJump
	{
		get{
			return (TouchGround||TouchCeiling||TouchWallToLeft||TouchWallToRight)&&(!TriggerJump);
		}
	}
	void Start () {
		//Setup Script Recources.
		MoveForce=new Vector2();
		GroundCheckRight=transform.Find("Detector.GroundRight");
		GroundCheckLeft=transform.Find("Detector.GroundLeft");
		RightCheck=transform.Find("Detector.Right");
		LeftCheck=transform.Find("Detector.Left");
		CeilingCheck=transform.Find("Detector.Ceiling");
		animator = transform.Find("PlayerAnimations").GetComponent<Animator>();

		MaxHSpeed_const = MaxHSpeed; //Maximum Horizontal Move Speed
		MaxVSpeed_const = MaxVSpeed; //Maximum Vertical Move Speed
		MoveVMag_const = MoveVMag;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
		MoveHMag_const = MoveHMag;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
		JumpHMag_const = JumpHMag;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
		JumpVMag_const = JumpVMag;
	}
	
	// Update is called once per frame
	void Update () {
		//Get Controller Data.
		float x=Input.GetAxis("Horizontal");
		float y=Input.GetAxis("Vertical");
		//Fix Controller data to only have values in the set {-1,0,1}
		x=Mathf.Abs(x)>0.01f?Mathf.Sign(x):0;
		y=Mathf.Abs(y)>0.01f?Mathf.Sign(y):0;
		//Set Controller Data for use in FixedUpdate.
		MoveForce.Set(x,y);
		//Update Player State
		TouchGround=Physics2D.Linecast(transform.position-new Vector3(0.5f,0,0),GroundCheckLeft.position,1<<LayerMask.NameToLayer("Terrain"))||
					Physics2D.Linecast(transform.position-new Vector3(-0.5f,0,0),GroundCheckRight.position,1<<LayerMask.NameToLayer("Terrain"));
		TouchWallToLeft=Physics2D.Linecast(transform.position,LeftCheck.position,1<<LayerMask.NameToLayer("Terrain"));
		TouchWallToRight=Physics2D.Linecast(transform.position,RightCheck.position,1<<LayerMask.NameToLayer("Terrain"));
		TouchCeiling=Physics2D.Linecast(transform.position,CeilingCheck.position,1<<LayerMask.NameToLayer("Terrain"));
		if(CanJump){
			TriggerJump=Input.GetButtonDown("Jump");
		}
		//Handle Size Switches.
		if(Input.GetButtonDown("Fire1"))
		{
			increaseSize();
		}
		if(Input.GetButtonDown("Fire2"))
		{
			decreaseSize();
		}
	}
	void FixedUpdate()
	{
		
		//Get Controller Data for Fixed Frame
		float x=Input.GetAxis("Horizontal");
		float y=Input.GetAxis("Vertical");
		
		//Manipulate data into correct range.
		x=Mathf.Abs(x)>0.01f?Mathf.Sign(x):0;
		y=Mathf.Abs(y)>0.01f?Mathf.Sign(y):0;
		MoveForce.Set(x,y);
		
		//Player Force Assignment. This portion of the script is probably extrememly inefficient, and should probably be reweritten for
		//both clarity and optimization reasons. 
		if((!WallStick))
		{
			if(CeilingStick){
				//If we're sticking to the ceiling, pretend gravity is reversed so friction works. 
				rigidbody2D.AddForce(Vector2.up*9.81f);
			}
			if(MoveForce.x*rigidbody2D.velocity.x<MaxHSpeed)
			{
				if(FreeFall){
					//If we're freefalling, apply restricted horizontal movement. 
					rigidbody2D.AddForce(Vector2.right * MoveForce.x * MoveHMag*InAirHScale);
					
				}
				else{
				if(size==2){
					//If we're too big to stick to walls
					if(TouchWallToRight&&(x>0)){
						//if we're trying to go right and we're touching a right wall, don't assign any force so friction doesn't act to 
						//"stick" us to the wall.
					}
					else if(TouchWallToLeft&&(x<0)){
						//if we're trying to go left and we're touching a left wall, dont assign any force so frction doesn't act to
						//"stick" us to the wall.
					}
					else
					{
							//otherwise, move normaly
							rigidbody2D.AddForce(Vector2.right * MoveForce.x * MoveHMag);
					}
				}
				else{
						//we're a normal size, so move normally.
						rigidbody2D.AddForce(Vector2.right * MoveForce.x * MoveHMag);
				}
				}
			}
			
			
		}
		else
		{
			if(MoveForce.y*rigidbody2D.velocity.y<MaxVSpeed)
			{
				
				rigidbody2D.AddForce(Vector2.up * MoveForce.y * MoveVMag);
				
			}
			if(StuckOnWallToLeft){
				rigidbody2D.AddForce(Vector2.right*-1*9.81f);
			}
			else if(StuckOnWallToRight){
				rigidbody2D.AddForce(Vector2.right*9.81f);
			}
		}
		//clip horizontal speed.
		if(Mathf.Abs(rigidbody2D.velocity.x) > MaxHSpeed)
		{
			
			rigidbody2D.velocity.Set(Mathf.Sign(rigidbody2D.velocity.x) * MaxHSpeed, rigidbody2D.velocity.y);
		}
		//clip vertical speed.
		if(Mathf.Abs(rigidbody2D.velocity.y)>MaxVSpeed)
		{
		
			rigidbody2D.velocity.Set(rigidbody2D.velocity.x,Mathf.Sign(rigidbody2D.velocity.y)*MaxVSpeed);
		}
			
    		if(TriggerJump)
		{
			if(TouchGround){
				//we're jumping up from the ground. Jump normally.
				rigidbody2D.AddForce(Vector2.up*JumpVMag);
			}
			if(StuckOnWallToLeft)
			{	
				//we're jumping to the right off a wall.
					rigidbody2D.AddForce(new Vector2(JumpHMag,JumpVMag*0.25f));
			}
			if(StuckOnWallToRight)
			{
				//we're jumping off the left off a wall.
				rigidbody2D.AddForce(new Vector2(-JumpHMag,JumpVMag*0.25f));
			}
			if(CeilingStick)
			{
				//we're dropping off the ceiling.
				rigidbody2D.AddForce(Vector2.up*-1*JumpVMag*0.2f);
			}
			TriggerJump=false;
		}
		//Fix a bug where graviy sometimes doesn't reset.
		if(FreeFall&&(rigidbody2D.gravityScale!=1))
		{
		
			rigidbody2D.gravityScale=1;
		}

		//Animation Controllers
		animator.SetBool ("grounded", !FreeFall);
		if(Input.GetAxis("Horizontal") > 0) {
			animator.SetInteger("direction", 1);
		} else if (Input.GetAxis("Horizontal") < 0) {
			animator.SetInteger("direction", -1);
		} else {
			animator.SetInteger("direction", 0);
		}

		//Rotate the sprite
		if(TouchGround || FreeFall && transform.rotation.eulerAngles.x != 0) {
			transform.Rotate (0, 0, 0);
			Debug.Log ("Rotate to 0");
		} else if(WallStick && TouchWallToLeft && transform.rotation.eulerAngles.x != 90) {
			//transform.Rotate(90, 0, 0);
			//Debug.Log ("Rotate to 90");
		}

	}
	void OnCollisionEnter2D(Collision2D col)
	{	
		//should probably turn this into a switch statement somehow.
		if(col.collider.tag=="Wall")
		{
			if(!TouchGround)
			{
				if(size!=2){
					//if we've just collided with a wall and we're not touching the ground and we're a size
					//that allows it, stick to the wall.
					rigidbody2D.velocity.Set (0,0);
 					rigidbody2D.gravityScale=0;
 					WallStick=true;
 				}			
			}

		}
		else if(col.collider.tag=="Ceiling"){
			if(!TouchGround){
				if(size!=2){
					//if we've just collided with the ceiling and we're not touching the ground and we're a size
					//that allows it, stick to the ceiling
					rigidbody2D.velocity.Set (0,0);
					rigidbody2D.gravityScale=0;
					CeilingStick=true;
				}
				
			}
		}
	}
	void OnCollisionStay2D(Collision2D col)
	{
		if(TouchGround)
		{
			//if we're ever sticking on a ceiling or wall and then touch the ground, revert back to normal
			//ground movement. 
			rigidbody2D.gravityScale=1;
			WallStick=false;
			CeilingStick=false;
		}
	}
	//This needs to be fixed before multi-platform walls can be included
	void OnCollisionExit2D(Collision2D col)
	{
		if(col.collider.tag=="Wall")
		{
			//if we've stopped colliding with a wall, stop sticking to it.
			//this is WRONG if we're still colliding with any other ceilings, but this can be 
			//designed around by ensuring the level colliders are single-platform-per-surface
			rigidbody2D.gravityScale=1;
			WallStick=false;
			
		}
		else if(col.collider.tag=="Ceiling")
		{
			//if we've stopped colliding with a ceiling, stop sticking to it.
			//this is WRONG if we're still colliding with any other ceilings, but this can be 
			//designed around by ensuring the level colliders are single-platform-per-surface
			rigidbody2D.gravityScale=1;
			CeilingStick=false;
		}
	}

	void decreaseSize() {
		if(size > 0) {
			size -= 1;
			transform.localScale -= new Vector3(sizeScaleFactor,sizeScaleFactor,0);
			setSpeedAndJumpParams();
		}
	}
	
	void increaseSize() {
		if(size < 2) {
			size += 1;
			transform.localScale += new Vector3(sizeScaleFactor,sizeScaleFactor,0);
			MaxHSpeed += MaxHSpeed * sizeScaleFactor; //Maximum Horizontal Move Speed
			MaxVSpeed += MaxVSpeed * sizeScaleFactor; //Maximum Vertical Move Speed
			MoveVMag += MoveVMag * sizeScaleFactor;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
			MoveHMag += MoveHMag * sizeScaleFactor;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
			JumpHMag += JumpHMag * sizeScaleFactor;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
			JumpVMag += JumpVMag * sizeScaleFactor;
			setSpeedAndJumpParams();
		}
	
		//reset gravity
		if(size == 2) {
			rigidbody2D.gravityScale=1;
			WallStick=false;			
			CeilingStick=false;
		}
	}

	void setSpeedAndJumpParams() {
		if(size == 0) {
			MaxHSpeed -= MaxHSpeed_const * speedScaleFactor; //Maximum Horizontal Move Speed
			MaxVSpeed -= MaxVSpeed_const * speedScaleFactor; //Maximum Vertical Move Speed
			MoveVMag -= MoveVMag_const * speedScaleFactor;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
			MoveHMag -= MoveHMag_const * speedScaleFactor;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
			JumpHMag -= JumpHMag_const * speedScaleFactor;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
			JumpVMag -= JumpVMag_const * speedScaleFactor;
			Debug.Log (size + ": " + MaxHSpeed + " " + MaxVSpeed + " " + MoveVMag + " " + MoveHMag + " " + JumpHMag + " " + JumpVMag);
		} else if(size == 1) {
			MaxHSpeed = MaxHSpeed_const; //Maximum Horizontal Move Speed
			MaxVSpeed = MaxVSpeed_const; //Maximum Vertical Move Speed
			MoveVMag = MoveVMag_const;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
			MoveHMag = MoveHMag_const;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
			JumpHMag = JumpHMag_const;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
			JumpVMag = JumpVMag_const;
			Debug.Log (size + ": " + MaxHSpeed + " " + MaxVSpeed + " " + MoveVMag + " " + MoveHMag + " " + JumpHMag + " " + JumpVMag);
		} else {
			MaxHSpeed -= MaxHSpeed_const * speedScaleFactor; //Maximum Horizontal Move Speed
			MaxVSpeed -= MaxVSpeed_const * speedScaleFactor; //Maximum Vertical Move Speed
			MoveVMag -= MoveVMag_const * speedScaleFactor;  //Magnitude of force applied when moving vertically (Up or Down a wall, not jumping)
			MoveHMag -= MoveHMag_const * speedScaleFactor;  //Magnitude of force applied when moving horizontally (left or right on ground or ceiling, scaled while in air)
			JumpHMag -= JumpHMag_const * speedScaleFactor;  //Horizontal Magnitude of force applied when jumping off a wall. Not used when Jumping off a vertical surface.
			JumpVMag -= JumpVMag_const * speedScaleFactor;
			Debug.Log (size + ": " + MaxHSpeed + " " + MaxVSpeed + " " + MoveVMag + " " + MoveHMag + " " + JumpHMag + " " + JumpVMag);
		}
	}

}
