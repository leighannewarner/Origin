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
	public int damage = 1;
	public bool isPlayer = false;

	void OnCollisionEnter2D (Collision2D c) {
		Debug.Log (c.gameObject.tag);
	}
}
