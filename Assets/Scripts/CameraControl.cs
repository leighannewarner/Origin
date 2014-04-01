using UnityEngine;
using System.Collections;

//EXTREMELY simple camera control script. Simply follows player
public class CameraControl : MonoBehaviour {

	//How fast does the camera approach the players position
	//0: Camera will not move (Takes forever to reach the player)
	//...
	//0.5: Camera will traverse half the distance between itself and the player per frame.
	//..
	//1: Camera will reach the player instintaniously.
	public float CameraSpeed=0.1f;
	Vector3 Vel;
	GameObject Player;
	// Use this for initialization
	void Start (){
		Player=GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 delta=Player.transform.position-transform.position;
		Vel=delta*CameraSpeed;
		transform.position+=new Vector3(Vel.x,Vel.y,0);
	}
}
