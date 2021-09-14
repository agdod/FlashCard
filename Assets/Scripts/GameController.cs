using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private Dealer dealer;
	//[SerializeField] private TMPro.TMP_Text question;
	[SerializeField] private TMPro.TMP_Text answer;
	[SerializeField] private UIContoller uIContoller;

	private void Start()
	{
		uIContoller.ToggleDisplayFlashCard(true);
		uIContoller.ToggleDisplayKeyPad(true);
		uIContoller.ToggleNext(false);
	}

	public void DisplayNextCard()
	{
		/*FlashCard flashCard = dealer.DealFlashCard();
		question.text = flashCard.Question;*/
		uIContoller.ToggleNext(false);
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

	public void CheckAnswer(string answer)
	{
		FlipFlashCard();
		if (answer == dealer.CurrentCard.Answer)
		{
			Debug.Log("Yaay you was right!");
		}
		else
		{
			Debug.Log("wrong answer!!!");
		}
	}

}
