using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MysteryBox : Interactable 
{
	//TODO: create file that has all of the guns/weapons
	//TODO: create array or list of the weapons that we want in the mystery box

	[SerializeField]
	private int cost;

	[SerializeField]
	private float availableTime = 5f;
	public float AvailableTime {	get {	return availableTime;	}	}

	[SerializeField]
	private Transform weaponSpawn;
	public Transform WeaponSpawn	{	get {	return weaponSpawn;	}	}

	[SerializeField]
	private List<Weapon> potentialWeapons;


	private Player weaponOwner;
	private Weapon mysteryWeapon;

	//False when not currently being used
	private bool state = false;


	public override bool Interact (Player candidate, float timeHeld)
	{
		if (timeHeld != 0f) 	{	return false;	}
		if (state && candidate == weaponOwner)
		{ //Weapon already spawned and should be available for pickup

			//Pick up weapon
			PlayerWeaponHandler pWeapHandler;
			if ( (pWeapHandler = candidate.GetComponent<PlayerWeaponHandler>()) == null)
			{
				Debug.LogError (candidate.name + " doesn't have a PlayerWeaponHandler");
			}
			pWeapHandler.PickupWeapon(mysteryWeapon);

			//State setting and callback
			state = false;
			weaponOwner = null;
			OnPickedUp ();
			return true;
		}
		else if (state == false && candidate.Points >= cost && PrerequisitesSatisfied())
		{ //Weapon is not spawned and should be
			state = true;
			IsSatisfied = true;
			candidate.Points -= cost;
			mysteryWeapon = potentialWeapons.GetRandomElement<Weapon>();
			weaponOwner = candidate;

			OnInteract(candidate);
			OnRoll (mysteryWeapon);
			StartCoroutine (Extensions.CallAfterSeconds (OnTimeOut, availableTime));
			return true;
		}
		else
		{
			return false;
		}
	}

	public override string InteractText (Player candidate)
	{
		if (!state) {	return interactText + cost; }
		return "Pick up " + mysteryWeapon.weaponName;
	}


	public void OnRoll (Weapon weap)
	{
		if (cbRoll == null) {	return;	}
		cbRoll (weap);
	}

	public void OnPickedUp()
	{
		state = false;
		weaponOwner = null;
		if (cbPickedUp == null) {	return;	}
		cbPickedUp ();
	}
		
	public void OnTimeOut()
	{
		state = false;
		weaponOwner = null;
		if (cbTimeOut == null) {	return;	}
		cbTimeOut ();
	}

	/*	
	=====================================
	|			CALLBACKS				|
	=====================================
	*/

	private Action<Weapon> cbRoll;
	private Action cbPickedUp; 
	private Action cbTimeOut;

	public void RegisterRoll (Action<Weapon> callbackFunc)
	{
		cbRoll += callbackFunc;
	}
	public void UnregisterRoll (Action<Weapon> callbackFunc)
	{
		cbRoll -= callbackFunc;
	}

	public void RegisterPickedUp (Action callbackFunc)
	{
		cbPickedUp += callbackFunc;
	}
	public void UnregisterPickedUp (Action callbackFunc)
	{
		cbPickedUp -= callbackFunc;
	}

	public void RegisterTimeOut (Action callbackFunc)
	{
		cbTimeOut += callbackFunc;
	}
	public void UnregisterTimeOut (Action callbackFunc)
	{
		cbTimeOut -= callbackFunc;
	}


}
