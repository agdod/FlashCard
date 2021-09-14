using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class FlashCard :ScriptableObject
{
	[SerializeField] private Image questionImage;
	[SerializeField] private Image answerImage;
	[SerializeField] private string question;
	[SerializeField] private string answer;

	public Image QuestionImage
	{
		get { return questionImage; }
	}

	public Image AnswerImage
	{
		get { return answerImage; }
	}

	public string Question
	{
		get { return question; }
	}

	public string Answer
	{
		get { return answer; }
	}
}
