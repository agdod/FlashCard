using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private Dealer dealer;
	[SerializeField] private TMPro.TMP_Text answer;
	[SerializeField] private UIContoller uIContoller;
	[SerializeField] private IntVariable correctAnswer;
	[SerializeField] private IntVariable skipped;
	[SerializeField] private FloatVariable timeTaken;
	[SerializeField] private BoolVariable initaliseDealer;
	[SerializeField] private string[] countDown = { "GO!", "1", "2", "3" };

	public delegate void GamePrep();
	public static GamePrep gamePrep;

	private void Start()
	{
		Dealer.lastCard += GameOver;    // Register listener for event.
		gamePrep += PrepareNewGame;
		uIContoller.DisplayMessage(false);
		gamePrep.Invoke();
	}

	private void PrepareNewGame()
	{
		gamePrep -= PrepareNewGame;     // Unregister from event.
		StartCoroutine(PreGameSetup());
	}

	IEnumerator PreGameSetup()
	{
		// Display UI message whie deck is being shuffled.
		uIContoller.ToggleDisplayFlashCard(false);
		uIContoller.ToggleDisplayKeyPad(false);
		uIContoller.ToggleNext(false);
		uIContoller.DisplayCountDown(false);
		while (!dealer.IsShuffled)
		{
			uIContoller.DisplayMessage("Shuffling deck....");
			Debug.Log("in corountine shuffling deck");
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine(CountDown());
	}

	private void StartNewGame()
	{
		// Initalise values. Setup UI elements.
		correctAnswer.value = 0;
		skipped.value = 0;
		timeTaken.value = Time.realtimeSinceStartup;
		uIContoller.DisplayCountDown(false);
		uIContoller.ToggleDisplayFlashCard(true);
		DisplayNextCard();
	}

	public void DisplayNextCard()
	{
		uIContoller.ToggleNext(false);
		uIContoller.DisplayMessage(false);
		uIContoller.ToggleDisplayKeyPad(true);
		answer.text = "";
		dealer.DealFlashCard();
	}

	private void FlipFlashCard()
	{
		// Apply the answer to backside of card.
		dealer.Reveal();
		uIContoller.ToggleNext(true);
	}

	public void Skip()
	{
		DisplayNextCard();
		skipped.value++;
	}

	public void CheckAnswer(string answer)
	{
		FlipFlashCard();
		uIContoller.ToggleDisplayKeyPad(false);
		if (answer == dealer.CurrentCard.Answer)
		{
			uIContoller.DisplayMessage("Correct");
			correctAnswer.value++;
		}
		else
		{
			uIContoller.DisplayMessage("Wrong Answer");
		}
	}

	private void CollectTimeTaken()
	{
		float newTime = Time.realtimeSinceStartup;
		timeTaken.value = newTime - timeTaken.value;
	}

	public void GameOver()
	{
		Dealer.lastCard -= GameOver;    // Unregister from event.
		CollectTimeTaken();
		SceneManager.LoadScene("StatsMenu");
	}

	private IEnumerator CountDown()
	{
		int count = 4;
		while (count > 0)
		{
			uIContoller.DisplayCountDown(countDown[count - 1]);
			count--;
			yield return new WaitForSeconds(1.0f);
		}
		StartNewGame();
	}
}
