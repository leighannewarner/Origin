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
			// Center in X, 2/3 of the height in Y
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
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Level1");
		}

		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
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
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Level2");
		}

		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
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
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Level3");
		}

		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			((Screen.width/2) - (buttonWidth/2) + buttonWidth),
			((Screen.height/2) - (buttonHeight/2) + buttonHeight),
			buttonWidth,
			buttonHeight
			),
			"Level 4"
			)
			)
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Level4");
		}
	}
}