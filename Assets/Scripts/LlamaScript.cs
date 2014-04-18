using UnityEngine;
using System.Collections;

public class LlamaScript : MonoBehaviour {

	public float h = 1.0f;
	public bool nearPlayer = false;
	public int shotDelay = 0;
	public GameObject bullet;
	GameObject shotTransform;
	private Animator animator;

	void Awake() {
		animator = transform.Find("LlamaAnimations").GetComponent<Animator>();
	}

	void FixedUpdate () {	
		animator.transform.localScale = new Vector3((Mathf.Abs(transform.localScale.x) * h * -1f),transform.localScale.y,transform.localScale.z);

		if (nearPlayer) {
			if(shotDelay <= 20) {
				fire();
			} else if (shotDelay > 0){
				animator.SetBool ("attacking", false);
				shotDelay--;
			} else {
				shotDelay--;
			}
		} else {
			animator.SetBool ("attacking", false);
			shotDelay = 0;
		}
	}
	
	void fire() {
		animator.SetBool ("attacking", true);
		shotTransform = ((Instantiate(bullet, new Vector2(transform.position.x, transform.position.y+0.6f), Quaternion.identity)) as GameObject);
		shotTransform.GetComponent<BulletScript>().h = h;
		shotDelay = 55;
	}

}
