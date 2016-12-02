using UnityEngine;
using System;
using System.Collections;

//TODO: Placeholder script. We probably don't want doors to blink out of existence when they're opened
public class DoorOpenScript : MonoBehaviour
{
	
	public Door door;

	void OnOpen (Player p, Interactable i)
	{
		gameObject.SetActive (false);
	}

	void Start()
	{
		if ( (door = GetComponent<Door>() ) == null)
		{
			Debug.LogError (name + " doesn't have a Door attached");
		}
		door.RegisterInteract (OnOpen);
	}

	void OnDisable()
	{
		door.UnregisterInteract (OnOpen);
	}
}







