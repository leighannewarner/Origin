using UnityEngine;
using System.Collections;

public class GrateScript : MonoBehaviour {

	public bool broken = false;
	public Sprite broken_sprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void breakGrate() {
		broken = true;
		Destroy(this.collider2D);
		(GetComponent<SpriteRenderer>()).sprite = broken_sprite;
	}
	
}
