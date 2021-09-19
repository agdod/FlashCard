using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[SerializeField] private BoolVariable initDealer;

	private void Start()
	{
		float right = rightAnswers.value;
		float total = totalCards.value;

		rightAnswers_Text.text = right.ToString();
		totalCards_Text.text = total.ToString();
		skippedCards_Text.text = skipped.value.ToString();
		float percent;
		percent = (right / total) * 100;
		percentage.text = percent.ToString("###");
		DecodeTime();
	}

	private void DecodeTime()
	{
		System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeTaken.value);
		timeTaken_Text.text = timeSpan.ToString(@"mm\:ss");
	}

	public void SelectionClicked()
	{
		initDealer.value = true;
		SceneManager.LoadScene("MainMenu");
	}

	public void ReplayClicked()
	{
		initDealer.value = false;
		SceneManager.LoadScene("Multiplication");
	}
}
