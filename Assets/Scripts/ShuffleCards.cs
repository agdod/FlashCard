using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCards : MonoBehaviour
{
	[System.Serializable]
	public class Tables
	{
		[SerializeField] private string identifier;
		[SerializeField] private List<FlashCard> multipliers;

		public List<FlashCard> Multipiers
		{
			get { return multipliers; }
		}
	}

	[SerializeField] private List<Tables> timesTables;
	[SerializeField] private int totalCardCount = 0;
	private List<int> shuffledDeck;

	// Get total amount of flash cards
	// Get Amount of Flash Card in each Table
	// Shuffle cards
	// Draw card from shuffled "deck"

	private void Start()
	{
		CountTotalFlashCards();
		ShuffleFlashCards();
	}

	private void CountTotalFlashCards()
	{
		int tablesCount = timesTables.Count;
		for (int x = 0; x < tablesCount; x++)
		{
			totalCardCount += timesTables[x].Multipiers.Count;
		}
	}

	private void ShuffleFlashCards()
	{
		// Assign a array from 0 to total card count.
		// Split array in half
		// Pick random location in first half, swap with random postion in second half.
		// repeat unitl all first half has been swapped.

		// initalise the list
		shuffledDeck = new List<int>(totalCardCount);
		for (int x = 0; x < totalCardCount; x++)
		{
			shuffledDeck.Add(x + 1);
		}

		int deckMidPoint = totalCardCount / 2;
		int lowerrHalfSwap;
		int upperHalfSwap;
		int upperRange;
		int lowerRange;
		int tempSwap;

		// Check for odd number of cards, if odd number of cards mid way point is odd card out.
		// upper half will increase by one. Middle card will not get swapped.
		if (totalCardCount % 2 != 0)
		{
			upperRange = deckMidPoint + 1;
			lowerRange = deckMidPoint - 1;
		}
		else
		{
			upperRange = deckMidPoint;
			lowerRange = deckMidPoint - 1;
		}

		for (int x = 0; x < deckMidPoint; x++)
		{
			lowerrHalfSwap = Random.Range(0, lowerRange);
			upperHalfSwap = Random.Range(upperRange, totalCardCount);
			tempSwap = shuffledDeck[lowerrHalfSwap];
			shuffledDeck[lowerrHalfSwap] = shuffledDeck[upperHalfSwap];
			shuffledDeck[upperHalfSwap] = tempSwap;
		}
	}
}


