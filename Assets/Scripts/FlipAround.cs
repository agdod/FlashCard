using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAround : MonoBehaviour
{
	// Little script to convert the times tables
	// converting from
	// 2 * 6 ==> 6 * 2
	// 2 * 7 ==> 7 * 2

	[SerializeField] private List<Group> multiplyGroup;
	[SerializeField] private List<FlashCard> multiplication;
	[SerializeField] private int groupCount;

	private void Start()
	{
		groupCount = multiplyGroup.Count;
		for (int x = 0; x < groupCount; x++)
		{
			int count = multiplyGroup[x].Multipliers.Count;
			for (int y = 0; y < count; y++)
			{
				Debug.Log((y + 2 )+ " * " + (x + 2) + " = " + (y + 2) * (x + 2));
				int group = y + 2;
				int multiple = x + 2;
				multiplyGroup[x].Multipliers[y].Question = group.ToString() + " * " + multiple.ToString();
			}
		}
	}
}
