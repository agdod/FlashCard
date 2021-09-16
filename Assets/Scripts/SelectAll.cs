using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAll : MonoBehaviour
{
	[SerializeField] private List<SelectOptions> selectOption;



	public void SelectAllGroups()
	{
		for (int x = 0; x < selectOption.Count; x++)
		{
			selectOption[x].Selected();
		}
	}

	public void CancelAll()
	{
		for (int x = 0; x < selectOption.Count; x++)
		{
			selectOption[x].DeSelected();
		}
	}
}
