using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MysteryBox : Interactable 
{
	//TODO: create file that has all of the guns/weapons
	//TODO: create array or list of the weapons that we want in the mystery box


	//commented to reduce annoying Console warning
	Weapon mysteryWeapon;
	Weapon[] allWeapons;

	public int cost;

	//False when not currently being used
	private bool state = false;


	public override bool Interact (Player candidate, float timeHeld)
	{
		if (timeHeld != 0f) 	{	return false;	}
		if (state || candidate.Points < cost || !PrerequisitesSatisfied()) { return false; }
		state = true;
		IsSatisfied = true;
		candidate.Points -= cost;
		OnInteract(candidate);
		mysteryWeapon = allWeapons.GetRandomElement();
		return true;
	}

	public override string InteractText (Player candidate)
	{
		return interactText + cost;
	}

	//When the player interacts again
	//pWeapHandler.PickupWeapon(mysteryWeapon);
}
