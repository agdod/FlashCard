using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
	[System.Serializable]
	public class Tables
	{
		[SerializeField] private string identifier;
		[SerializeField] private List<FlashCard> multipliers;

		public List<FlashCard> Multipliers
		{
			get { return multipliers; }
		}
	}

	[SerializeField] private TMPro.TMP_Text flashCardText;
	[SerializeField] private List<Tables> timesTables;
	[SerializeField] private int totalCardCount = 0;

	private List<FlashCard> flashCardStack;
	private int index = -1; // pointer for current flash card in stack
	private bool isShuffled;

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

	private void Start()
	{
		CountTotalFlashCards();
		PrepareDeck();
		ShuffleFlashCards();
		DealFlashCard();
	}

	private void CountTotalFlashCards()
	{
		int tablesCount = timesTables.Count;
		for (int x = 0; x < tablesCount; x++)
		{
			totalCardCount += timesTables[x].Multipliers.Count;
		}
	}

	private void PrepareDeck()
	{
		// Deck is prepared and cached for use, so deck doesnt need to be repopluated if player wants to try again.
		// Initalise the List for the FlashCardStack
		flashCardStack = new List<FlashCard>(totalCardCount);

		// Cycle trhough Tables and FlashCard list, filling Stack of Flash Cards		
		Tables tables;
		int timesTableCount = timesTables.Count;

		for (int tableCount = 0; tableCount < timesTableCount; tableCount++)
		{
			// Cache the current timesTables
			tables = timesTables[tableCount];

			for (int cardCount = 0; cardCount < tables.Multipliers.Count; cardCount++)
			{
				// Add current Flash Card to the flash card Stack.
				flashCardStack.Add(tables.Multipliers[cardCount]);
			}
		}
		isShuffled = false;
	}

	private void ShuffleFlashCards()
	{
		// Assign a array from 0 to total card count.
		// Split array in half
		// Pick random location in first half, swap with random postion in second half.
		// repeat unitl all first half has been swapped.

		int deckMidPoint = totalCardCount / 2;

		int lowerrHalfSwap;
		int upperHalfSwap;
		int upperRange;
		int lowerRange;

		FlashCard tempSwap;

		// Check for odd number of cards, if odd number of cards mid way point is odd card out.
		// upper half will increase by one. Middle card will not get swapped.
		if (totalCardCount % 2 != 0)
		{
			upperRange = deckMidPoint + 1;
			lowerRange = deckMidPoint - 1;
			// Shuffle middle card
			flashCardStack[deckMidPoint] = flashCardStack[Random.Range(0, totalCardCount)];
		}
		else
		{
			upperRange = deckMidPoint;
			lowerRange = deckMidPoint - 1;
		}

		for (int x = 0; x < deckMidPoint; x++)
		{
			lowerrHalfSwap = Random.Range(0, lowerRange + 1);
			upperHalfSwap = Random.Range(upperRange + 1, totalCardCount);
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

		index++;
		if (index > flashCardStack.Count)
		{
			Debug.Log("End of game reshuffle and start again.");
		}
		currentCard = flashCardStack[index];
		flashCardText.text = currentCard.Question;
	}

	public void Reveal()
	{
		flashCardText.text = currentCard.Answer;
	}
}


