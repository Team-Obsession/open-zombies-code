using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDAmmoUpdate : HUDRelatedScript
{
	LocalPlayer player;
	PlayerWeaponHandler weapHandler;
	Text text;
	WeaponInstance currentWeapon;

	void OnAmmoUpdate (WeaponInstance weapon)
	{
		switch (weapon.weapon.weaponType)
		{
			case WeaponType.Gun :
				GunInstance temp = (GunInstance) weapon;
				text.text = temp.BulletsInMag + " / " + temp.ExtraAmmo;
				break;

			case WeaponType.Melee :
				text.text = "\u221E"; //Infinity
				break;

			default :
				break;
		}
	}

	void OnWeaponChange (WeaponInstance newCurrentWeapon)
	{
		WeaponInstance oldWeapon = currentWeapon;
		currentWeapon = newCurrentWeapon;
		if (oldWeapon != null)
		{
			switch (oldWeapon.weapon.weaponType)
			{
				case WeaponType.Gun:
					((GunInstance)newCurrentWeapon).UnregisterBulletCountChange (OnAmmoUpdate);
					break;

				case WeaponType.Melee:
				//Just in case we think of something to put here
					break;

				default :
					break;
			}
		}

		switch (currentWeapon.weapon.weaponType)
		{
			case WeaponType.Gun :
			((GunInstance)newCurrentWeapon).RegisterBulletCountChange (OnAmmoUpdate);
			break;

			case WeaponType.Melee :
			break;

			default :
			break;
		}

		OnAmmoUpdate (currentWeapon);
	}

	public override void OnInitialize()
	{
		player = GetComponentInParent<HUD>().Player;
		weapHandler = player.weaponHandler;
		text = GetComponent<Text>();
		weapHandler.RegisterWeaponChange (OnWeaponChange);
	}

	public override void OnTerminate()
	{
		if (weapHandler != null)
		{
			weapHandler.UnregisterWeaponChange (OnWeaponChange);
		}
	}
}





