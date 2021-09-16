using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private Dealer dealer;
	//[SerializeField] private TMPro.TMP_Text question;
	[SerializeField] private TMPro.TMP_Text answer;
	[SerializeField] private UIContoller uIContoller;
	[SerializeField] private IntVariable correctAnswer;
	[SerializeField] private IntVariable skipped;

	private void Start()
	{
		uIContoller.ToggleDisplayFlashCard(true);
		uIContoller.ToggleDisplayKeyPad(true);
		uIContoller.ToggleNext(false);
		uIContoller.DisplayMessage(false);
	}

	public void NewGame()
	{
		correctAnswer.value = 0;
		skipped.value = 0;
	}

	public void DisplayNextCard()
	{
		/*FlashCard flashCard = dealer.DealFlashCard();
		question.text = flashCard.Question;*/
		uIContoller.ToggleNext(false);
		uIContoller.DisplayMessage(false);
		uIContoller.ToggleDisplayKeyPad(true);
		answer.text = "";
		dealer.DealFlashCard();
	}

	private void FlipFlashCard()
	{
		// Animate or rotate the Flash card to "show" the back side.
		// Apply the answer to backside of card.
		dealer.Reveal();
		uIContoller.ToggleNext(true);
	}

	public void Skip()
	{
		DisplayNextCard();
		//FlipFlashCard();
		//uIContoller.DisplayMessage(false);
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

	public void GameOver()
	{
		// End of Deck
		// Display Stats.
		// Play again?
	}

}
