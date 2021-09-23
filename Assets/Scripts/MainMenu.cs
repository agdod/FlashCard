using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	[SerializeField] private BoolVariable initaliseDealer;
	[SerializeField] private List<char> isOneActive;        //Used to keep tally is at least one grouping is selected
	[SerializeField] private GameObject dialogBox;

	public delegate void ActionClick();
	public static event ActionClick onSelected;
	public static event ActionClick onDeselected;

	public delegate void DisplayDialog(string dialogMessage, DialogButton.style dialogBtn, string[] buttonTxt);
	public static event DisplayDialog displayDialog;

	private void OnEnable()
	{
		SelectOptions.onActive += PlusOne;
		SelectOptions.onDeactive += MinusOne;
	}

	public void Start()
	{
		// Reset all Selected Groups
		CancelAll();
	}

	private void PlusOne()
	{
		// Adds an element to the isOneActive List
		isOneActive.Add('*');
	}

	private void MinusOne()
	{
		// Removes an element from the isOneActive List
		isOneActive.Remove('*');
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
		// Check at least one selection has been made
		// Else raise dialg warning.
		if (isOneActive.Count > 0)
		{
			initaliseDealer.value = true;
			SceneManager.LoadScene("Multiplication");
		}
		else
		{
			Debug.Log(" raise the dialog box from 1st scene.");
			// Rasie dialog warning.
			string[] buttonText = { "Ok" };
			if (displayDialog != null)
			{
				displayDialog("Select at least one group.", DialogButton.style.singleButton, buttonText);
			}
		}
	}

	private void OnDisable()
	{
		//Unregister from the Events
		SelectOptions.onActive -= PlusOne;
		SelectOptions.onDeactive -= MinusOne;
	}
}
