using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
	[SerializeField] private TMP_Text outputDisplay;
	[SerializeField] private GameController gameController;

	public void InputKeyPress(string keyValue)
	{
		outputDisplay.text += keyValue.ToString();
	}

	public void DeleteKeyPress()
	{
		// Delete key pressed
		// Trucate the string by one
		string tempText = outputDisplay.text;
		int truncateText = tempText.Length;
		outputDisplay.text = tempText.Substring(0, truncateText - 1);
	}

	public void EnterPressed()
	{
		// Enter key is pressed
		// Check there is answer
		if (outputDisplay.text != null | outputDisplay.text.Length > 0)
		{
			// Answer has been entered, submit answer
			gameController.CheckAnswer(outputDisplay.text);
		}
	}

	public void SkipPressed()
	{
		// Reveal answer
		outputDisplay.text = "";
		gameController.Skip();
	}
}
