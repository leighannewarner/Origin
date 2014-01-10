using UnityEngine;

/// <summary>
/// Handle hitpoints and damages
/// </summary>
public class HealthScript : MonoBehaviour
{
	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int hp = 2;
	
	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	void Update() {
		Debug.Log ("This script is running!");
	}
	
	void OnCollisionStay2D (Collision2D c) {
		Debug.Log (c.gameObject.tag);
	}
}
