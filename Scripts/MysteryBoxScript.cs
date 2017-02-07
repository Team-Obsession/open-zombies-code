using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the weapon ***VISUAL*** when the mystery box is rolled
/// </summary>
public class MysteryBoxScript : MonoBehaviour
{
	private MysteryBox myBox;
	private Transform weaponSpawn;
	private float availableTime;

	private GameObject spawnedWeapon;

	public void OnRoll (Weapon weap)
	{
		//Spawn Weapon
		spawnedWeapon = (GameObject) Instantiate(weap.prefab, weaponSpawn.position, weaponSpawn.rotation);
	}

	public void OnPickedUp ()
	{
		//Destroy
		Destroy(spawnedWeapon);
	}

	public void OnTimeOut ()
	{
		//Destroy
		if (!spawnedWeapon) 	{	return;		}
		Destroy(spawnedWeapon);
	}

	public void OnEnable()
	{
		if ( (myBox = GetComponent<MysteryBox>() ) == null)
		{
			Debug.LogError (name + " doesn't have a MysteryBox component attached");
		}
		RegisterCallbacks ();
		weaponSpawn = myBox.WeaponSpawn;
	}

	public void OnDisable()
	{
		UnregisterCallbacks ();
		weaponSpawn = null;
	}

	private void RegisterCallbacks()
	{
		myBox.RegisterRoll (OnRoll);
		myBox.RegisterPickedUp (OnPickedUp);
		myBox.RegisterTimeOut (OnTimeOut);
	}

	private void UnregisterCallbacks()
	{
		myBox.UnregisterRoll (OnRoll);
		myBox.UnregisterPickedUp (OnPickedUp);
		myBox.UnregisterTimeOut (OnTimeOut);
	}
}
