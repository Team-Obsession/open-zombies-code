using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods 
{
	public static T GetRandomElement<T> (this T[] array)
	{
		return array[Random.Range (0, array.Length - 1)];
	}

	//public static Transform ClosestTo ()
	
}












