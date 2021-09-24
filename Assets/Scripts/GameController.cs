using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private Dealer dealer;
	[SerializeField] private TMPro.TMP_Text answer;
	[SerializeField] private IntVariable correctAnswer;
	[SerializeField] private IntVariable skipped;
	[SerializeField] private FloatVariable timeTaken;
	[SerializeField] private string[] countDown = { "GO!", "1", "2", "3" };
	[SerializeField] private Status gameStatus;

	/*
	public delegate void GamePrep();
	public static GamePrep gamePrep;
	public static GamePrep startGame;

	// Dialog Box message , dialogButton Style, custom button text.

	public delegate void DisplayDialog(string dialogMessage, DialogButton.style dialogButtonStyle, string[] buttonText);
	public static event DisplayDialog displayDialog; */

	private void OnDisable()
	{
		Events.dealerStatus -= StatusResponse;
	}

	private void Awake()
	{
		RegisterforEvents();
	}

	private void RegisterforEvents()
	{
		Events.dealerStatus += StatusResponse;      //Register for dealer Status up date Event.

		// Register for StartGame Event
		Events.startGame += StartNewGame;
		Events.startGame += GetTimeStarted;
		Events.startGame += DisplayNextCard;
	}

	private void Start()
	{
		Events.gamePrep.Invoke();

		// Check there is listners for displayDialog events
		if (Events.displayDialog == null)
		{
			Debug.LogError("No listeners for display dialog.");
		}
	}

	private void StatusResponse(Status status)
	{
		switch (status)
		{
			case Status.PreparingDeck:
				Debug.Log("Preparing Deck");
				break;
			case Status.DeckPrepared:
				Debug.Log("Deck prepared");
				break;
			case Status.ShufflingDeck:
				Debug.Log("shuffling Deck");
				Events.displayDialog("Shuffling Deck", DialogButton.style.noButtons, null);
				break;
			case Status.DeckShuffled:
				Debug.Log("Deck shuffled");
				Events.displayDialog("", DialogButton.style.noButtons, null);
				// Start countdown.
				StartCoroutine(CountDown());
				break;
			case Status.DroppedDeck:
				Debug.Log("Ooops somethng went wrong deck dropped");
				DroppedDeck();
				break;
			case Status.EndOfDeck:
				Debug.Log("End of deck");
				GameOver();
				break;
		}
	}

	private IEnumerator CountDown()
	{
		int count = 4;
		while (count > 0)
		{
			// Display dialog box to force dialog rect to reset to cached value.
			Events.displayDialog("", DialogButton.style.noButtons, null);
			Events.displayDialog(countDown[count - 1], DialogButton.style.noButtons, null);
			count--;
			yield return new WaitForSeconds(1.0f);
		}
		Events.displayDialog("", DialogButton.style.noButtons, null);

		if (Events.startGame != null)
		{
			Events.startGame.Invoke();
		}
	}

	private void GetTimeStarted()
	{
		Events.startGame -= GetTimeStarted;
		timeTaken.value = Time.realtimeSinceStartup;
	}

	private void StartNewGame()
	{
		// Initalise values. Setup UI elements.
		Events.startGame -= StartNewGame;
		correctAnswer.value = 0;
		skipped.value = 0;
	}

	public void DisplayNextCard()
	{
		// Unsubscribe from onbuttonClick event
		Events.onConfirmButtonClick -= DisplayNextCard;
		Events.startGame -= DisplayNextCard;
		answer.text = "";
		dealer.DealFlashCard();
	}

	private void FlipFlashCard()
	{
		// Apply the answer to backside of card.
		dealer.Reveal();
	}

	public void Skip()
	{
		DisplayNextCard();
		skipped.value++;
	}

	public void CheckAnswer(string answer)
	{
		FlipFlashCard();
		string[] buttonText = { "Next" };
		if (answer == dealer.CurrentCard.Answer)
		{

			Events.displayDialog("Correct", DialogButton.style.singleButton, buttonText);
			Events.onConfirmButtonClick += DisplayNextCard;
			correctAnswer.value++;
		}
		else
		{
			Events.displayDialog("Wrong", DialogButton.style.singleButton, buttonText);
			Events.onConfirmButtonClick += DisplayNextCard;
		}
	}

	private void CollectTimeTaken()
	{
		float newTime = Time.realtimeSinceStartup;
		timeTaken.value = newTime - timeTaken.value;
	}

	private void DroppedDeck()
	{
		//Oops something went wrong 
		if (Events.displayDialog != null)
		{
			// Invoke dialog box.
			string[] buttonText = { "Selection" };
			Events.displayDialog("Oops! Dealer dropped the deck. Retrun to selection menu.", DialogButton.style.singleButton, buttonText);
			// Register listener for button click.
			Events.onConfirmButtonClick += ReturnMainMenu;
		}
		else
		{
			Debug.Log("No listener for displayDailog");
		}
	}

	// -- Scene Loading Rountines --

	public void GameOver()
	{
		CollectTimeTaken();
		SceneManager.LoadScene("StatsMenu");
	}

	private void ReturnMainMenu()
	{
		Events.onConfirmButtonClick -= ReturnMainMenu;
		SceneManager.LoadScene("MainMenu");
	}

	private void CancelRequest()
	{
		// On cancel button pressed unsubscribe from  confirmButtonClick event
		Events.onConfirmButtonClick -= ReturnMainMenu;
	}

	public void AreYouSure()
	{
		// verify player really wants to retrun to main menu.
		string[] buttonText = { "Quit", "Resume" };
		Events.displayDialog("Are you sure you want to return to Main Menu?", DialogButton.style.dualButtons, buttonText);
		Events.onConfirmButtonClick += ReturnMainMenu;
		Events.onCancelButtonClick += CancelRequest;
	}

}
