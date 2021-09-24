using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogButton
{
	// Serves to identify button type, and styling - ie single, dual.

	public enum style { noButtons, singleButton, dualButtons };
	public style buttonStyle;
	public Button button;
	public TMPro.TMP_Text buttoneText;
}

public class DialogBox : MonoBehaviour
{
	//[SerializeField] private GameObject dialogBox;
	[SerializeField] private GameObject dialogBox;
	[SerializeField] private GameObject dialogMask;
	[SerializeField] private List<DialogButton> dialogButtons;
	[SerializeField] private TMPro.TMP_Text dialogText;
	//[SerializeField] private TMPro.TMP_Text buttonText;
	[SerializeField] private RectTransform rectTransform;

	private Vector2 cachedRectValues;
	private bool rectValuesCached = false;
	/*
	public delegate void OnButtonClick();
	public static event OnButtonClick onConfirmButtonClick;
	public static event OnButtonClick onCancelButtonClick;*/

	[SerializeField] int charsPerLine = 25;
	[SerializeField] int lineHeight = 30;

	private void Awake()
	{
		Events.displayDialog += DisplayDialogBox;
		//MainMenu.displayDialog += DisplayDialogBox;

		rectTransform = GetComponent<RectTransform>();
		dialogMask.gameObject.SetActive(false);
		CacheValues();
	}

	private void CacheValues()
	{
		cachedRectValues = rectTransform.sizeDelta;
		rectValuesCached = true;
	}

	private string LineWrap(string message)
	{
		string formatMessage = "";
		int charsInPreviousLines = 0;
		int lineCount = 1;
		string[] words = message.Split(' ');

		for (int x = 0; x < words.Length; x++)
		{
			// Adjust length of message to account for additional <br>, 
			int amountLineBreakChars = ((lineCount - 1) * 4);
			int adjustedLength = formatMessage.Length - amountLineBreakChars;

			// Check adding current word doesnt go over 20 chars in line
			if ((adjustedLength + words[x].Length) < (charsPerLine + charsInPreviousLines))
			{
				formatMessage += words[x] + " ";
			}
			else
			{
				lineCount++;
				// Keep track of total amout of chars in previous Lines (not all lines will have exactly 20 chars!)
				charsInPreviousLines = adjustedLength;
				formatMessage += "<BR>" + words[x] + " ";
			}
		}
		//Return the formatted message with line count appended.
		formatMessage += ":" + lineCount;

		return formatMessage;
	}

	private void ResizeDialogBox(string message)
	{
		string formattedMessage = LineWrap(message);

		// Grab the lineCount from the string and convert to Int
		int lineCount = System.Convert.ToInt16(formattedMessage.Substring(formattedMessage.IndexOf(":") + 1));
		// Strip the line count from the string.
		formattedMessage = formattedMessage.Substring(0, formattedMessage.IndexOf(":"));

		float dialogHeight = rectTransform.rect.height;
		dialogHeight += lineCount * lineHeight;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, dialogHeight);
		dialogText.text = formattedMessage;
	}

	private void DisplayDialogBoxButtons(DialogButton.style dialogBtnStyle, string[] buttonTxt)
	{
		// Buttons are controlled by button style <enum> 
		//		noBtns		- diable buttons for dislog box.
		//		singleBtn	- Allows for custom txt, (also Ok) for a single central button
		//		dualBtns	- Displays two button evently spaced from the center, with custom text

		// Cycle through the dialogButtons, compare the type of each button with the type of button passed in via the Event
		// If button type matches activate button and display the text passed in via Event.

		switch (dialogBtnStyle)
		{
			case DialogButton.style.noButtons:
				// Display all DialogButtons
				foreach (DialogButton item in dialogButtons)
				{
					item.button.gameObject.SetActive(false);
				}
				break;
			case DialogButton.style.singleButton:
				// Enable Only Dialog Button, and display text.
				foreach (DialogButton item in dialogButtons)
				{
					if (item.buttonStyle == DialogButton.style.singleButton)
					{
						item.button.gameObject.SetActive(true);
						item.buttoneText.text = buttonTxt[0];
					}
					else
					{
						item.button.gameObject.SetActive(false);
					}
				}
				break;
			case DialogButton.style.dualButtons:
				// Enable Ok adnd Cancel  Buttons.
				for (int x = 0; x < dialogButtons.Count; x++)
				{
					if (dialogButtons[x].buttonStyle == DialogButton.style.dualButtons)
					{
						dialogButtons[x].button.gameObject.SetActive(true);
						dialogButtons[x].buttoneText.text = buttonTxt[x];
					}
					else
					{
						dialogButtons[x].button.gameObject.SetActive(false);
					}
				}
				break;
			default:
				// Default to hiding buttons if invalid option
				Debug.Log("Invalid options. Buttons Hiden.");
				foreach (DialogButton item in dialogButtons)
				{
					item.button.gameObject.SetActive(false);
				}
				break;
		}
	}

	private void DisplayDialogBox(string dialogMessage, DialogButton.style dialogBtnStyle, string[] buttonTxt)
	{
		// Display dialog box with text and appoint button.

		if (dialogMessage.Length == 0)
		{
			// No message to display, disable the dialog box
			dialogMask.SetActive(false);
		}
		else
		{
			dialogMask.SetActive(true);
			// Resize dialog box and display message.
			ResizeDialogBox(dialogMessage);
			DisplayDialogBoxButtons(dialogBtnStyle, buttonTxt);
		}
	}

	private void DisplayMessage(string message, string buttonTxt)
	{
		// Allow Display message to display 2 buttons,
		// using buttonTxt as a string to hold the values for the buttonText

		if (message.Length == 0)
		{
			// No message to display disable dialog box and mask
			dialogMask.SetActive(false);
		}
		else
		{
			dialogMask.SetActive(true);
			ResizeDialogBox(message);
		}
	}

	public void ButtonClick()
	{
		dialogMask.SetActive(false);
		if (Events.onConfirmButtonClick != null)
		{
			Events.onConfirmButtonClick.Invoke();
		}
	}

	public void CancelButtonClick()
	{
		dialogMask.SetActive(false);
		if (Events.onCancelButtonClick != null)
		{
			Events.onCancelButtonClick.Invoke();
		}
	}

	private void OnDisable()
	{
		//Reset the sizing of the dialog box.
		if (rectValuesCached)
		{
			rectTransform.sizeDelta = cachedRectValues;
		}
	}

	private void OnDestroy()
	{
		Events.displayDialog -= DisplayDialogBox;
		//MainMenu.displayDialog -= DisplayDialogBox;
	}
}
