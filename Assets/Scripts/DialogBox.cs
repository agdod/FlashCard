using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
	//[SerializeField] private GameObject dialogBox;
	[SerializeField] private TMPro.TMP_Text dialogText;
	[SerializeField] private TMPro.TMP_Text buttonText;

	public delegate void OnButtonClick();
	public static event OnButtonClick onButtonClick;

	private void Awake()
	{
		GameController.displayDialog += DisplayMessage;
		this.gameObject.SetActive(false);
	}

	private void DisplayMessage(string message, string buttonTxt)
	{
		this.gameObject.SetActive(true);
		dialogText.text = message;
		buttonText.text = buttonTxt;
	}

	public void ButtonClick()
	{
		this.gameObject.SetActive(false);
		if (onButtonClick != null)
		{
			onButtonClick.Invoke();
		}
	}

	private void OnDisable()
	{
		GameController.displayDialog -= DisplayMessage;
	}
}
