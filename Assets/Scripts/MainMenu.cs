using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private List<SelectOptions> selectOption;
	[SerializeField] private BoolVariable initaliseDealer;

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

	public void StartClicked()
	{
		initaliseDealer.value = true;
		SceneManager.LoadScene("Multiplication");
	}

}
