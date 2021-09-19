using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptions : MonoBehaviour
{
	[SerializeField] private bool isSelected;
	[SerializeField] private Group grouping;
	[SerializeField] private Color selectedColor = new Color(245, 64, 64);
	[SerializeField] private Color normalColor = new Color(250, 234, 96);
	private Image image;

	//public delegate void OnClicked(bool onSelected);
	//public static event OnClicked onClicked;

	public Group Grouping
	{
		get { return grouping; }
	}

	private void Awake()
	{
		image = GetComponent<Image>();
		MainMenu.onSelected += Selected;
		MainMenu.onDeselected += Deselect;
	}

	public void Selected()
	{

		image.color = selectedColor;
		isSelected = true;
		grouping.IsActive = true;
	}

	public void Deselect()
	{
		image.color = normalColor;
		isSelected = false;
		grouping.IsActive = false;
	}

	public void OnButtonSelected()
	{
		if (isSelected == true)
		{
			Deselect();
		}
		else
		{
			Selected();
		}
	}
}
