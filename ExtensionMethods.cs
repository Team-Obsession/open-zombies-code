using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods 
{
	public static T GetRandomElement<T> (this T[] array)
	{
		return array[Random.Range (0, array.Length - 1)];
	}

	public static Transform ClosestOf (this Transform reference, List<Transform> prospects)
	{
		Transform closest = prospects[0];
		float lastSqrDist = float.MaxValue;
		float thisSqrDist;
		for(int i = 0; i < prospects.Count; i++)
		{
			thisSqrDist =  (prospects[i].position - reference.position).sqrMagnitude;
			if(thisSqrDist < lastSqrDist)
			{
				closest = prospects[i];
			}
			lastSqrDist = thisSqrDist;
		}
		return closest;
	}
	
}












