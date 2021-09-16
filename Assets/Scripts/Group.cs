using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Group : ScriptableObject
{
	[SerializeField] private string identifier;
	[SerializeField] private List<FlashCard> multipliers;

	public List<FlashCard> Multipliers
	{
		get { return multipliers; }
	}
}
