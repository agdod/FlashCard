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

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void Selected()
	{
		image.color = selectedColor;
		isSelected = true;
		grouping.IsActive = true;
	}

	public void DeSelected()
	{
		image.color = normalColor;
		isSelected = false;
		grouping.IsActive = false;
	}

	public void OnButtonSelected()
	{
		if (isSelected == true)
		{
			DeSelected();
		}
		else
		{
			Selected();
		}
	}
}
