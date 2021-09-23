using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Status
{
	CountingCards,
	CardsCounted,
	PreparingDeck,
	DeckPrepared,
	RecountDeck,
	ShufflingDeck,
	DeckShuffled,
	EndOfDeck,
	DroppedDeck
}

public class Dealer : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text flashCardText;
	[SerializeField] private List<Group> timesTables;
	[SerializeField] private IntVariable totalCardCount;
	[SerializeField] private BoolVariable initaliseDealer;
	[SerializeField] private IntVariable remaingCards;
	[SerializeField] private bool onClicked = false;

	private List<FlashCard> flashCardStack;
	private int index = -1; // pointer for current flash card in stack
	private bool isShuffled;

	[SerializeField] private Status status;

	public delegate void DealerStatus(Status status);
	public static event DealerStatus dealerStatus;

	private FlashCard currentCard;

	public FlashCard CurrentCard
	{
		get { return currentCard; }
	}

	// Get total amount of flash cards
	// Get Amount of Flash Card in each Table
	// Shuffle cards
	// Draw card from shuffled "deck"

	private void OnEnable()
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
	}

	private void CountTotalFlashCards()
	{
		int cardCount = 0;
		int tablesCount = timesTables.Count;

		if (dealerStatus != null)
		{
			status = Status.CardsCounted;
			dealerStatus(Status.CountingCards);
		}

		totalCardCount.value = 0;
		for (int x = 0; x < tablesCount; x++)
		{
			if (timesTables[x].IsActive == true)
			{
				cardCount += timesTables[x].Multipliers.Count;
			}
		}
		totalCardCount.value = cardCount;
		remaingCards.value = cardCount;

		if (dealerStatus != null)
		{
			status = Status.CardsCounted;
			dealerStatus(Status.CardsCounted);
		}
	}

	private void ReshuffleDeck()
	{
		GameController.gamePrep -= ReshuffleDeck;

		// Repopluate the FlashCardStack
		PrepareDeck();
	}

	private void PrepareDeck()
	{
		if (dealerStatus != null)
		{
			status = Status.PreparingDeck;
			dealerStatus(Status.PreparingDeck);
		}
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
		if (flashCardStack.Count != 0)
		{
			// check prepped deck == cardcount total

			if (flashCardStack.Count != totalCardCount.value)
			{
				// Card count not correct re initialise Dealer
				status = Status.RecountDeck;
				Debug.Log("Status : " + status);
				InitailiseDealer();

			}
			else
			{
				if (dealerStatus != null)
				{
					status = Status.DeckPrepared;
					dealerStatus(Status.DeckPrepared);
				}
				ShuffleFlashCards();
			}

		}
		else
		{
			// Something went wrong deck is zero - unprepared.
			if (dealerStatus != null)
			{
				status = Status.DroppedDeck;
				dealerStatus(Status.DroppedDeck);
			}
		}
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

			// Random position value to swap middle card wiht
			int tempMiddleSwap = Random.Range(0, totalCardCount.value);
			// Cache the middle card.
			FlashCard tempMiddleCardSwap = flashCardStack[deckMidPoint];
			// Move the random positon to the middle card postion
			flashCardStack[deckMidPoint] = flashCardStack[tempMiddleSwap];
			// Move the cached middlecard to the random postion.
			flashCardStack[tempMiddleSwap] = tempMiddleCardSwap;
		}
		else
		{
			upperRange = deckMidPoint;
			lowerRange = deckMidPoint - 1;
		}

		for (int x = 0; x < deckMidPoint; x++)
		{
			if (dealerStatus != null)
			{
				status = Status.ShufflingDeck;
				dealerStatus(Status.ShufflingDeck);
			}
			lowerrHalfSwap = Random.Range(0, lowerRange + 1);
			upperHalfSwap = Random.Range(upperRange + 1, totalCardCount.value);
			tempSwap = flashCardStack[lowerrHalfSwap];
			flashCardStack[lowerrHalfSwap] = flashCardStack[upperHalfSwap];
			flashCardStack[upperHalfSwap] = tempSwap;
		}
		isShuffled = true;
		if (dealerStatus != null)
		{
			status = Status.DeckShuffled;
			dealerStatus(Status.DeckShuffled);
		}
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
			remaingCards.value--;
		}
		else
		{
			if (dealerStatus != null)
			{
				status = Status.EndOfDeck;
				dealerStatus(Status.EndOfDeck);
			}
		}
	}

	public void Reveal()
	{
		flashCardText.text = currentCard.Answer;
	}
}


