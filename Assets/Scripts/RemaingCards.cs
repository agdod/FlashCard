using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemaingCards : MonoBehaviour
{
	[SerializeField] private IntVariable remainingCards;
	[SerializeField] TMPro.TMP_Text remainingCardTxt;

	private void Update()
	{
		remainingCardTxt.text = remainingCards.value.ToString();
	}
}
