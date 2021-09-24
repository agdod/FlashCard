using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Events 
{
	// Dialog Box Events
	public delegate void DisplayDialog(string dialogMessage, DialogButton.style dialogButtonStyle, string[] buttonText);
	public static DisplayDialog displayDialog;
	public delegate void OnButtonClick();
	public static OnButtonClick onConfirmButtonClick;
	public static OnButtonClick onCancelButtonClick;

	// Dealer Events
	public delegate void DealerStatus(Status status);
	public static DealerStatus dealerStatus;

	// GameController Events
	public delegate void GamePrep();
	public static GamePrep gamePrep;
	public static GamePrep startGame;

	// Select Options
	public delegate void OnClicked();
	public static OnClicked onActive;
	public static OnClicked onDeactive;

	// MainMenu
	public delegate void ActionClick();
	public static ActionClick onSelected;
	public static ActionClick onDeselected;
}
