using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
	[SerializeField] private TMP_Text outputDisplay;

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
}
