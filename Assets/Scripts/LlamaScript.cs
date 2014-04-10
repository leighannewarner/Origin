using UnityEngine;
using System.Collections;

public class LlamaScript : MonoBehaviour {

	public float h = 1.0f;
	public bool nearPlayer = false;
	public int shotDelay = 0;
	
	void FixedUpdate () {	
		if (nearPlayer) {
			if(shotDelay <= 0) {
				fire();
				shotDelay = 10;
			} else {
				shotDelay--;
			}
		} else {
			shotDelay = 0;
		}
	}
	
	void fire() {
		shotTransform = Instantiate(shotPrefab) as Transform;
		shotTransform.position = transform.position;
	}
}
