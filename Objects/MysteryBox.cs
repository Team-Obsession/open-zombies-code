using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MysteryBox : MonoBehaviour 
{
	//TODO: create file that has all of the guns/weapons
	//TODO: create array or list of the weapons that we want in the mystery box

	Weapon mysteryWeapon;
	Weapon[] allWeapons;

	public Weapon Roll()
	{
		mysteryWeapon = allWeapons.GetRandomElement();
		return mysteryWeapon;
	}
}
