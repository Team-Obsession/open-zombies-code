using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Extensions 
{
	public static T GetRandomElement<T> (this T[] array)
	{
		return array[UnityEngine.Random.Range (0, array.Length)];
	}

	public static T GetRandomElement<T> ( this List<T> list )
	{
		if (list.Count == 0)	{	return default (T);	}
		int rand = UnityEngine.Random.Range (0, list.Count);
		return list[rand];
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

	public static int CompareRayCastHitByDistance (RaycastHit first, RaycastHit second)
	{
		if (first.distance == second.distance) {	return 0;	}
		return first.distance < second.distance ? -1 : 1;
	}


	public static void SetLayer (this Transform trans, int layer)
	{
		trans.gameObject.layer = layer;
		foreach(Transform child in trans)
		{
			child.SetLayer(layer);
		}
			
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

	/// <summary>
	/// Deprecated?
	/// </summary>
	/// <param name="seconds">timer</param>
	static public  IEnumerator WaitForSeconds (float seconds)
	{
		yield return new WaitForSeconds (seconds);
	}

	static public IEnumerator CallAfterSeconds (Action action, float timer)
	{
		if (action == null)
		{
			Debug.LogError ("Action " + action + " is empty");
			yield break;
		}
		yield return new WaitForSeconds (timer);
		action ();
	}

	static public  IEnumerator WaitFrame ()
	{
		yield return new WaitForEndOfFrame ();
	}

	/// <summary>
	/// Compute a Ray which has some angular difference from an input Transform
	/// </summary>
	/// <returns>A new ray whose base is input.position, and points degAway degrees away from input.up and is rotated
	/// degForward degrees around input.forward</returns>
	/// <param name="input">The input Transform</param>
	/// <param name="degAway">The number of degrees between the output Ray around input's forward vector</param>
	/// <param name="degForward">The number of degrees between the output Ray around input's forward vector</param>
	public static Ray RotateRay (this Transform input, float degAway, float degForward)
	{
		Quaternion rotation = input.transform.rotation * Quaternion.Euler (0f, 0f, degForward) * Quaternion.Euler (0f, degAway, 0f);
		return new Ray (input.position, rotation * Vector3.forward);
	}
	
}













