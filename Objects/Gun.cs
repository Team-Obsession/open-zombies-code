using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun", order = 1)]
public class Gun : Weapon 
{
	public FireType fireType; //i.e. semi-auto, bolt-action
	public int magazineSize = 5, maxExtraMags = 3, bulletsPerShot = 1;
	public float aimTime = 0.2f;

}
	
public enum GunClass
{
	LMG, Assault, SniperRifle
}
