using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private Dealer dealer;
	[SerializeField] private TMPro.TMP_Text answer;
	//[SerializeField] private UIContoller uIContoller;
	[SerializeField] private IntVariable correctAnswer;
	[SerializeField] private IntVariable skipped;
	[SerializeField] private FloatVariable timeTaken;
	[SerializeField] private string[] countDown = { "GO!", "1", "2", "3" };
	[SerializeField] private Status gameStatus;

	public delegate void GamePrep();
	public static GamePrep gamePrep;

	public delegate void DisplayDialog(string message, string buttonText);
	public static event DisplayDialog displayDialog;

	private void OnDisable()
	{
		Dealer.dealerStatus -= StatusResponse;
	}

	private void Start()
	{
		Dealer.dealerStatus += StatusResponse;      //Register for dealer Status up date Event.

		gamePrep += PrepareNewGame;
		gamePrep.Invoke();

		// Check there is listners for displayDialog events
		if (displayDialog == null)
		{
			Debug.LogError("No listeners for display dialog.");
		}
	}

	private void PrepareNewGame()
	{
		gamePrep -= PrepareNewGame;                 // Unregister from event.
		PreGameSetup();
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
				displayDialog("Shuffling Deck", "");
				break;
			case Status.DeckShuffled:
				Debug.Log("Deck shuffled");
				displayDialog("", "");
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

	private void PreGameSetup()
	{
		/*
		uIContoller.ToggleDisplayFlashCard(false);
		uIContoller.ToggleDisplayKeyPad(false);
		uIContoller.ToggleNext(false);
		uIContoller.DisplayCountDown(false);
		*/
	}

	private IEnumerator CountDown()
	{
		int count = 4;
		while (count > 0)
		{
			// Display dialog box to force dialog rect to reset to cached value.
			displayDialog("", "");
			displayDialog(countDown[count - 1], "");
			count--;
			yield return new WaitForSeconds(1.0f);
		}
		displayDialog("", "");
		StartNewGame();
	}

	private void StartNewGame()
	{
		// Initalise values. Setup UI elements.
		correctAnswer.value = 0;
		skipped.value = 0;
		timeTaken.value = Time.realtimeSinceStartup;
		//uIContoller.ToggleDisplayFlashCard(true);
		DisplayNextCard();
	}

	public void DisplayNextCard()
	{
		// Unsubscribe from onbuttonClick event
		DialogBox.onButtonClick -= DisplayNextCard;
		//uIContoller.ToggleDisplayKeyPad(true);
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
		if (answer == dealer.CurrentCard.Answer)
		{
			displayDialog("Correct", "Next");
			DialogBox.onButtonClick += DisplayNextCard;
			correctAnswer.value++;
		}
		else
		{
			displayDialog("Wrong", "Next");
			DialogBox.onButtonClick += DisplayNextCard;
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
		if (displayDialog != null)
		{
			// Invoke dialog box.
			displayDialog("Oops! Dealer dropped the deck. Retrun to selection menu.", "Selection");
			// Register listener for button click.
			DialogBox.onButtonClick += ReturnMainMenu;
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
		DialogBox.onButtonClick -= ReturnMainMenu;
		SceneManager.LoadScene("MainMenu");
	}



}
