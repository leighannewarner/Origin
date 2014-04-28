using UnityEngine;
using System.Collections;

public class GateScript : MonoBehaviour {
	
	public bool open = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void openGate() {
		open = true;
		Destroy(this.collider2D);
		(GetComponent<SpriteRenderer>()).enabled = false;
	}
	
}
