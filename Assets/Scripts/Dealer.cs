using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
	//[SerializeField] private GameController gameController;
	[SerializeField] private TMPro.TMP_Text flashCardText;
	[SerializeField] private List<Group> timesTables;
	[SerializeField] private IntVariable totalCardCount;
	[SerializeField] private BoolVariable initaliseDealer;

	private List<FlashCard> flashCardStack;
	private int index = -1; // pointer for current flash card in stack
	private bool isShuffled;

	public delegate void Notify();
	public static Notify lastCard;

	private FlashCard currentCard;

	public FlashCard CurrentCard
	{
		get { return currentCard; }
	}

	public bool IsShuffled
	{
		get { return isShuffled; }
	}

	// Get total amount of flash cards
	// Get Amount of Flash Card in each Table
	// Shuffle cards
	// Draw card from shuffled "deck"


	private void Awake()
	{
		// Check for Dealer Satus 
		// If initialsie is true, new selection is picked.
		// If false, use old selection just reshuffle deck.

		if (initaliseDealer.value == true)
		{
			GameController.gamePrep += InitailiseDealer;
		}
		else
		{
			GameController.gamePrep += ReshuffleDeck;
		}
	}

	public void InitailiseDealer()
	{
		GameController.gamePrep -= InitailiseDealer; // Unregister the Event.
		CountTotalFlashCards();
		PrepareDeck();
		ShuffleFlashCards();
	}

	private void CountTotalFlashCards()
	{
		int tablesCount = timesTables.Count;
		totalCardCount.value = 0;
		for (int x = 0; x < tablesCount; x++)
		{
			if (timesTables[x].IsActive == true)
			{
				totalCardCount.value += timesTables[x].Multipliers.Count;
			}
		}
	}

	private void ReshuffleDeck()
	{
		GameController.gamePrep -= ReshuffleDeck;

		// Repopluate the FlashCardStack
		PrepareDeck();
		ShuffleFlashCards();
	}

	private void PrepareDeck()
	{
		// Initalise the List for the FlashCardStack
		flashCardStack = new List<FlashCard>(totalCardCount.value);
		// Cycle trhough Tables and FlashCard list, filling Stack of Flash Cards		
		Group tables;
		int timesTableCount = timesTables.Count;

		for (int tableCount = 0; tableCount < timesTableCount; tableCount++)
		{
			// Cache the current timesTables
			tables = timesTables[tableCount];
			if (tables.IsActive == true)
			{
				for (int cardCount = 0; cardCount < tables.Multipliers.Count; cardCount++)
				{
					// Add current Flash Card to the flash card Stack.
					flashCardStack.Add(tables.Multipliers[cardCount]);
				}
			}
		}
		isShuffled = false;
	}

	public void ShuffleFlashCards()
	{
		// Split FlashCard Stack List in half
		// Pick random location in first half, swap with random postion in second half.
		// repeat unitl all first half has been swapped.

		GameController.gamePrep -= ShuffleFlashCards;   // Unregister the Event from the Gamecontroller Class.

		int deckMidPoint = totalCardCount.value / 2;

		int lowerrHalfSwap;
		int upperHalfSwap;
		int upperRange;
		int lowerRange;

		FlashCard tempSwap;

		// Check for odd number of cards, if odd number of cards mid way point is odd card out.
		// upper half will increase by one. Middle card will not get swapped.
		if (totalCardCount.value % 2 != 0)
		{
			upperRange = deckMidPoint + 1;
			lowerRange = deckMidPoint - 1;
			// Shuffle middle card
			flashCardStack[deckMidPoint] = flashCardStack[Random.Range(0, totalCardCount.value)];
		}
		else
		{
			upperRange = deckMidPoint;
			lowerRange = deckMidPoint - 1;
		}

		for (int x = 0; x < deckMidPoint; x++)
		{
			lowerrHalfSwap = Random.Range(0, lowerRange + 1);
			upperHalfSwap = Random.Range(upperRange + 1, totalCardCount.value);
			tempSwap = flashCardStack[lowerrHalfSwap];
			flashCardStack[lowerrHalfSwap] = flashCardStack[upperHalfSwap];
			flashCardStack[upperHalfSwap] = tempSwap;
		}
		isShuffled = true;
	}

	public void DealFlashCard()
	{
		// increment pointer 
		// Get new card, and return

		// Check that current card isnt last card.
		if (index < flashCardStack.Count - 1)
		{
			index++;
			currentCard = flashCardStack[index];
			flashCardText.text = currentCard.Question;
		}
		else
		{
			// GameOver
			if (lastCard != null)
			{
				lastCard.Invoke();
			}
		}

	}

	public void Reveal()
	{
		flashCardText.text = currentCard.Answer;
	}
}


