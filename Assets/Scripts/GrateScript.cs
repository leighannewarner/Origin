using UnityEngine;
using System.Collections;

public class GrateScript : MonoBehaviour {

	public bool broken = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void breakGrate() {
		broken = true;
		Destroy(this.collider2D);
	}

	void OnColliderEnter2D (Collision2D c) {
		if(c.gameObject.tag == "Player") {
			if((c.gameObject.GetComponent<HeroControl>()).size >= 2) {
				breakGrate();
			} else {
				Debug.Log(c.gameObject.GetComponent<HeroControl>().size);
			}
		}
	}
}
