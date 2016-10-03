using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponHandler : PlayerRelatedScript
{
	public LocalPlayer player;
	public PlayerInput input;
	public PlayerLoadout loadout;
	public Transform weaponPoint;

	WeaponInstance weaponInstance1;
	WeaponInstance weaponInstance2;

	WeaponInstance currentWeapon; //TODO: dual-wield functionality?
	public WeaponInstance CurrentWeapon
	{
		get
		{
			return currentWeapon;
		}
	}

	void OnInputSwitch (float timeHeld)
	{
		if (timeHeld != 0f)	{	return;	}
		currentWeapon = currentWeapon == weaponInstance1 ? weaponInstance2 : weaponInstance1;

		CurrentWeapon.gameObject.SetActive (true);
		(currentWeapon == weaponInstance1 ? weaponInstance2 : weaponInstance1 ).gameObject.SetActive (false);

		OnWeaponChange ();
	}

	public override void OnInitialize()
	{
		player = GetComponent<LocalPlayer>();
		if(input == null && ((input = player.GetComponent<PlayerInput>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find an input from its player");
		}
		input.RegisterInputSwitch (OnInputSwitch);

		weaponInstance1 = loadout.weapon1.Spawn (weaponPoint).GetComponent<WeaponInstance>();
		weaponInstance2 = loadout.weapon2.Spawn (weaponPoint).GetComponent<WeaponInstance>();


		weaponInstance1.player = player;
		weaponInstance1.input = input;
		weaponInstance1.weapHandler = this;
		weaponInstance2.player = player;
		weaponInstance2.input = input;
		weaponInstance2.weapHandler = this;

		weaponInstance1.Initialize ();
		weaponInstance2.Initialize ();

		weaponInstance2.gameObject.SetActive (false);

		currentWeapon = weaponInstance1;

		OnWeaponChange ();
	}

	public override void OnTerminate()
	{
		if (input == null) {	return;		}
		input.UnregisterInputAim (OnInputSwitch);
	}

/*	
	=====================================
	|			CALLBACKS				|
	=====================================
*/

	Action<WeaponInstance> cbWeaponChange;

	void OnWeaponChange ()
	{
		if (cbWeaponChange != null)
		{
			cbWeaponChange (currentWeapon);
		}
	}

	public void RegisterWeaponChange (Action<WeaponInstance> callbackFunc)
	{
		cbWeaponChange += callbackFunc;
	}

	public void UnregisterWeaponChange (Action<WeaponInstance> callbackFunc)
	{
		cbWeaponChange -= callbackFunc;
	}




}





