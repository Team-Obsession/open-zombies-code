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
		if (prospects.Capacity == 0)
		{
			Debug.LogError ("Prospects is an uninstantiated list");
			return null;
		}
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

	public static Transform ClosestOf (this Transform reference, List<GameObject> prospects)
	{
		if (prospects.Capacity == 0)
		{
			Debug.LogError ("Prospects is an uninstantiated list");
			return null;
		}
		Transform closest = prospects[0].transform;
		float lastSqrDist = float.MaxValue;
		float thisSqrDist;
		for(int i = 0; i < prospects.Count; i++)
		{
			thisSqrDist =  (prospects[i].transform.position - reference.transform.position).sqrMagnitude;
			if(thisSqrDist < lastSqrDist)
			{
				closest = prospects[i].transform;
			}
			lastSqrDist = thisSqrDist;
		}
		return closest;
	}

	public static Vector2 gradientNormalize2D (this Vector2 input)
	{	
		if(input == Vector2.zero)
		{
			return Vector2.zero;
		}
		float angle = Mathf.Atan(input.y / input.x);
		float extMag = 0f;
		float sinAngle = Mathf.Sin(angle);
		float cosAngle = Mathf.Cos(angle);
		
		if( Mathf.Abs(sinAngle) <= Mathf.Abs(cosAngle))
		{
			extMag = 1f / Mathf.Abs(cosAngle);
		}
		else
		{
			extMag = 1f / Mathf.Abs(sinAngle);
		}
		Vector2 output = new Vector2(input.x / extMag, input.y / extMag);	
		return output;
	}
	
}












