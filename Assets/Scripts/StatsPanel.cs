using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text rightAnswers_Text;
	[SerializeField] private TMPro.TMP_Text totalCards_Text;
	[SerializeField] private TMPro.TMP_Text skippedCards_Text;
	[SerializeField] private TMPro.TMP_Text percentage;
	[SerializeField] private TMPro.TMP_Text timeTaken_Text;

	// Scriptable Objects varibles
	[SerializeField] private IntVariable rightAnswers;
	[SerializeField] private IntVariable totalCards;
	[SerializeField] private IntVariable skipped;
	[SerializeField] private FloatVariable timeTaken;

	private void Start()
	{
		float right = rightAnswers.value;
		float total = totalCards.value;

		rightAnswers_Text.text = right.ToString();
		totalCards_Text.text = total.ToString();
		skippedCards_Text.text = skipped.value.ToString();
		Debug.Log("right,total " + right + ":" + total);

		float percent;
		percent = (right / total) * 100;
		Debug.Log(percent + ":");
		percentage.text = percent.ToString();


	}
}
