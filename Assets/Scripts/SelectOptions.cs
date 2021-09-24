using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptions : MonoBehaviour
{
	[SerializeField] private Group grouping;
	[SerializeField] private Color selectedColor = new Color(245, 64, 64);
	[SerializeField] private Color normalColor = new Color(250, 234, 96);
	private Image image;

	/*
	public delegate void OnClicked();
	public static event OnClicked onActive;
	public static event OnClicked onDeactive;*/

	public Group Grouping
	{
		get { return grouping; }
	}

	private void Awake()
	{
		image = GetComponent<Image>();

		// Register with the Events for select deselecting all.

		Events.onSelected += Selected;
		Events.onDeselected += Deselect;
	}

	public void Selected()
	{
		// Only if group isnt already active
		if (grouping.IsActive != true)
		{
			image.color = selectedColor;
			grouping.IsActive = true;

			if (Events.onActive != null)
			{
				Events.onActive();
			}
		}
	}

	public void Deselect()
	{
		// Only if Group isnt already deactivated
		if (grouping.IsActive != false)
		{
			image.color = normalColor;
			grouping.IsActive = false;

			if (Events.onDeactive != null)
			{
				Events.onDeactive();
			}
		}
	}

	public void OnButtonSelected()
	{
		if (grouping.IsActive == true)
		{
			Deselect();
		}
		else
		{
			Selected();
		}
	}

	private void OnDisable()
	{
		// Unregister from the Event method.
		Events.onSelected -= Selected;
		Events.onDeselected -= Deselect;
	}
}
