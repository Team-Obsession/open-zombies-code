using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun", order = 1)]
public class Gun : Weapon 
{
	public FireType fireType; //i.e. semi-auto, bolt-action
	public int magazineSize = 10, defaultExtraMags = 3, maxExtraMags = 10, bulletsPerShot = 1;
	public float aimTime = 0.2f;

	public int InitialExtraAmmo
	{
		get		{	return defaultExtraMags * magazineSize;		}
	}

	public int InitialAmmo
	{
		get		{	return defaultExtraMags * (magazineSize + 1);	}
	}

	public int MaxExtraAmmo
	{
		get		{	return maxExtraMags * magazineSize;		}
	}

	public int MaxAmmo
	{
		get		{	return maxExtraMags * (magazineSize + 1);	}
	}

}
	
public enum GunClass
{
	LMG, Assault, SniperRifle
}
