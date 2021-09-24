using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContoller : MonoBehaviour
{
	[SerializeField] private Canvas keyPadCanvas;
	[SerializeField] private Canvas flashCardCanvas;
	[SerializeField] private Canvas UICanvas;
	[SerializeField] private GameObject nextButton;

	private void Awake()
	{
		Events.gamePrep += PreGameSetup;        //Subscribe to Events
		Events.startGame += StartNewGame;
	}

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

	private void PreGameSetup()
	{
		Events.gamePrep -= PreGameSetup;        //Unsubscribe from Event.
														// Hide UI elements while prepaing deck/game.);
		ToggleDisplayFlashCard(false);
		ToggleDisplayKeyPad(false);
		//DisplayCountDown(false);
	}

	private void StartNewGame()
	{
		Events.startGame -= StartNewGame;       // Unsubscribe from Event.
														// Display UI elements
		ToggleDisplayFlashCard(true);
		ToggleDisplayKeyPad(true);
	}

}
