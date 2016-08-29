using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun", order = 1)]
public class Gun : Weapon 
{
	public int magazineSize = 5, maxExtraMags = 3;
	private int excessAmmo;

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

	public int MaxAmmo()
	{
		return maxExtraMags * (magazineSize + 1);
	}

	public int CurrentAmmo()
	{
		return BulletsInMag + excessAmmo;
	}

	public Gun()
	{
		bulletsInMag = magazineSize;
		excessAmmo = maxExtraMags * magazineSize;
	}
		
	/// <summary>
	/// Reload this gun.
	/// </summary>
	public void Reload()
	{
		if(BulletsInMag == magazineSize)
		{
			return;
		}
		if(excessAmmo > 0)
		{
			if(excessAmmo >= magazineSize - BulletsInMag)
			{
				excessAmmo -= magazineSize - BulletsInMag;
				BulletsInMag = magazineSize;
			}
			else
			{
				BulletsInMag = excessAmmo;
				excessAmmo = 0;
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
		OnShoot ();
		return bulletsShot;
	}

	protected Action cbShoot;
	protected Action cbBulletCountChange;
	protected Action cbMagazineEmpty; 
	protected Action cbReload;

	private void OnShoot()
	{
		if(cbShoot != null)
		{
			cbShoot();
		}
	}

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

	public void RegisterShoot(Action callbackFunc)
	{
		cbShoot += callbackFunc;
	}
	public void UnregisterShoot(Action callbackFunc)
	{
		cbShoot -= callbackFunc;
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
