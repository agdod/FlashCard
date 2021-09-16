using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Group : ScriptableObject
{
	[SerializeField] private string identifier;
	[SerializeField] private bool isActive;
	[SerializeField] private List<FlashCard> multipliers;

	private void OnEnable()
	{
		isActive = false;
	}

	public bool IsActive
	{
		get { return isActive; }
		set { isActive = value; }
	}

	public List<FlashCard> Multipliers
	{
		get { return multipliers; }
	}

}
