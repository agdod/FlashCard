using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContoller : MonoBehaviour
{
	[SerializeField] private Canvas keyPadCanvas;
	[SerializeField] private Canvas flashCardCanvas;
	[SerializeField] private Canvas UICanvas;
	[SerializeField] private GameObject nextButton;
	[SerializeField] private TMPro.TMP_Text uiMessage;
	[SerializeField] private TMPro.TMP_Text countDownText;

	public void ToggleDisplayKeyPad(bool isActive)
	{
		keyPadCanvas.gameObject.SetActive(isActive);
	}

	public void ToggleDisplayFlashCard(bool isActive)
	{
		flashCardCanvas.gameObject.SetActive(isActive);
	}

	public void ToggleUICanvas(bool isActive)
	{
		UICanvas.gameObject.SetActive(isActive);
	}

	public void ToggleNext(bool isActive)
	{
		nextButton.SetActive(isActive);
	}

	public void DisplayMessage(string message)
	{
		uiMessage.text = message;
		uiMessage.gameObject.SetActive(true);
	}

	public void DisplayMessage(bool isActive)
	{
		uiMessage.gameObject.SetActive(isActive);
	}

	public void DisplayCountDown(string countDown)
	{
		countDownText.gameObject.SetActive(true);
		countDownText.text = countDown;
	}

	public void DisplayCountDown(bool isActive)
	{
		countDownText.gameObject.SetActive(isActive);
	}



}
