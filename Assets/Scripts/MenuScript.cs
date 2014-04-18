using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
{
	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		// Draw a button to start the game
		if (
			GUI.Button(
			new Rect(
			((Screen.width/2) - (buttonWidth/2) - buttonWidth),
			((Screen.height/2) - (buttonHeight/2) - buttonHeight),
			buttonWidth,
			buttonHeight
			),
			"Level 1!"
			)
			)
		{
			Application.LoadLevel("Level1");
			GUILayer.Destroy(this);
		}

		if (
			GUI.Button(
			new Rect(
			((Screen.width/2) - (buttonWidth/2) + buttonWidth),
			((Screen.height/2) - (buttonHeight/2) - buttonHeight),
			buttonWidth,
			buttonHeight
			),
			"Level 2!"
			)
			)
		{
			Application.LoadLevel("Level2");
			GUILayer.Destroy(this);
		}

		if (
			GUI.Button(
			new Rect(
			((Screen.width/2) - (buttonWidth/2)  - buttonWidth),
			((Screen.height/2) - (buttonHeight/2) + buttonHeight),
			buttonWidth,
			buttonHeight
			),
			"Level 3!"
			)
			)
		{
			Application.LoadLevel("Level3");
			GUILayer.Destroy(this);
		}

	}
}