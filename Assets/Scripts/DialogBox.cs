using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
	//[SerializeField] private GameObject dialogBox;
	[SerializeField] private GameObject dialogBox;
	[SerializeField] private GameObject dialogMask;
	[SerializeField] private Button dialogButton;
	[SerializeField] private TMPro.TMP_Text dialogText;
	[SerializeField] private TMPro.TMP_Text buttonText;
	[SerializeField] private RectTransform rectTransform;

	private Vector2 cachedRectValues;
	private bool rectValuesCached = false;

	public delegate void OnButtonClick();
	public static event OnButtonClick onButtonClick;

	[SerializeField] int charsPerLine = 25;
	[SerializeField] int lineHeight = 30;

	private void Awake()
	{
		GameController.displayDialog += DisplayMessage;
		MainMenu.displayDialog += DisplayMessage;

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

	private void DisplayMessage(string message, string buttonTxt)
	{
		if (message.Length == 0)
		{
			// No message to display disable dialog box and mask
			dialogMask.SetActive(false);
		}
		else
		{
			dialogMask.SetActive(true);
			ResizeDialogBox(message);
			if (buttonTxt != "")
			{
				dialogButton.gameObject.SetActive(true);
				buttonText.text = buttonTxt;
			}
			else
			{
				dialogButton.gameObject.SetActive(false);
			}
		}
	}

	public void ButtonClick()
	{
		dialogMask.SetActive(false);
		if (onButtonClick != null)
		{
			onButtonClick.Invoke();
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
		GameController.displayDialog -= DisplayMessage;
		MainMenu.displayDialog -= DisplayMessage;
	}
}
