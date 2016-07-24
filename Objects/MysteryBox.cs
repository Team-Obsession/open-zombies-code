using UnityEngine;
using System.Collections;
using System;

public class MysteryBox : MonoBehaviour 
{
	//TODO: create file that has all of the guns/weapons
	//TODO: create array or list of the weapons that we want in the mystery box

	Weapon mysteryWeapon;
	Weapon[] placeholder = new Weapon[20];
	System.Random rng = new System.Random();

	public Weapon roll()
	{
		int randomNumber = rng.Next(0,placeholder.Length);
		mysteryWeapon = placeholder[randomNumber];
		return mysteryWeapon;
	}
}
