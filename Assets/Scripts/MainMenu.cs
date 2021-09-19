using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private BoolVariable initaliseDealer;

	public delegate void ActionClick();
	public static event ActionClick onSelected;
	public static event ActionClick onDeselected;

	public void Start()
	{
		// Reset all Selected Groups
		CancelAll();
	}

	public void SelectAllGroups()
	{
		if (onSelected != null)
		{
			onSelected.Invoke();
		}
	}

	public void CancelAll()
	{
		if (onDeselected != null)
		{
			onDeselected.Invoke();
		}
	}

	public void StartClicked()
	{
		// -- TO DO --
		// Check at least one selection has been made
		// Check each group unitl one is found selected.
		// Else raise dialg warning.

		initaliseDealer.value = true;
		SceneManager.LoadScene("Multiplication");

	}


}
