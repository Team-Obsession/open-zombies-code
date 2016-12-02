using UnityEngine;
using System;
using System.Collections;

public class Door : Interactable
{
	public int cost; //in points

	private bool state = false; //false is closed

	/// <summary>
	/// Open the door if it is closed and the player has the sufficient points
	/// </summary>
	/// <returns>
	/// true if the door was successfully opened, false otherwise
	/// </returns>
	/// <param name="spender">The interacting player</param>
	public override bool Interact (Player candidate)
	{
		if (state || candidate.Points < cost || !PrerequisitesSatisfied()) {	return false;	}
		state = true;
		IsSatisfied = true;
		candidate.Points -= cost;
		OnInteract (candidate);
		return true;
	}

	public override string InteractText (Player candidate)
	{
		return interactText;
	}


	
}







