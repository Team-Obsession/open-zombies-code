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
	/// <param name="candidate">The interacting player</param>
	public override bool Interact (Player candidate, float timeHeld)
	{
		if (timeHeld != 0f) {	return false;	}
		if (state || candidate.Points < cost || !IsSatisfied) {	return false;	}
		state = true;
		SelfSatisfied = true;
		candidate.Points -= cost;
		OnInteract (candidate);
		OnInteractableUpdate ();
		return true;
	}

	public override string InteractText (Player candidate)
	{
		return interactText +  " (" + cost + " points)";
	}


	
}







