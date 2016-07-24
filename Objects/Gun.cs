using UnityEngine;
using System;
using System.Collections;

public class Gun : Weapon 
{
	public int magazineSize = 5, maxExcessAmmo;
	private int currentExcessAmmo;

	private int bulletsInMag;

	public int BulletsInMag
	{
		get {	return bulletsInMag;	}
		protected set
		{
			if (value != bulletsInMag)
			{
				bulletsInMag = value;
				OnBulletCountChange();
			}
		}
	}

	public Gun()
	{
		bulletsInMag = magazineSize;
	}
		
	/// <summary>
	/// Reload this gun.
	/// </summary>
	public void Reload()
	{
		if(currentExcessAmmo > 0)
		{
			if(currentExcessAmmo >= magazineSize)
			{
				BulletsInMag = magazineSize;
				currentExcessAmmo -= magazineSize;
			}
			else
			{
				BulletsInMag = currentExcessAmmo;
				currentExcessAmmo = 0;
			}
		}
		OnReload();
	}

	/// <summary>
	/// Shoot up to the specified number of bullets.
	/// Returns the actual number of bullets shot.
	/// </summary>
	/// <param name="numBullets">Number bullets.</param>
	public int Shoot(int numBullets)
	{
		int bulletsShot = 0;
		if (BulletsInMag > 0)
		{
			if(BulletsInMag >= numBullets)
			{
				BulletsInMag -= numBullets;
				bulletsShot = numBullets;
			}
			else
			{
				BulletsInMag = 0;
				bulletsShot = numBullets - BulletsInMag;
			}
		}
		return bulletsShot;
	}

	protected Action cbBulletCountChange;
	protected Action cbMagazineEmpty; 
	protected Action cbReload;

	private void OnBulletCountChange()
	{
		if(cbBulletCountChange != null)
		{
			cbBulletCountChange();
		}
	}

	private void OnMagazineEmpty()
	{
		if(cbMagazineEmpty != null)
		{
			cbMagazineEmpty();
		}

	}		

	private void OnReload()
	{
		if(cbReload != null)
		{
			cbReload();
		}
	}

	public void RegisterBulletCountChange(Action callbackFunc)
	{
		cbBulletCountChange += callbackFunc;
	}

	public void UnregisterBulletCountChange(Action callbackFunc)
	{
		cbBulletCountChange -= callbackFunc;
	}

	public void RegisterMagazineEmpty(Action callbackFunc)
	{
		cbMagazineEmpty += callbackFunc;
	}

	public void UnregisterMagazineEmpty(Action callbackFunc)
	{
		cbMagazineEmpty -= callbackFunc;
	}

	public void RegisterReload(Action callbackFunc)
	{
		cbReload += callbackFunc;
	}

	public void UnregisterReload(Action callbackFunc)
	{
		cbReload -= callbackFunc;
	}
}
	
public enum GunClass
{
	LMG, Assault, SniperRifle
}
