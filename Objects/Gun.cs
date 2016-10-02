using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun", order = 1)]
public class Gun : Weapon 
{
	public int magazineSize = 5, maxExtraMags = 3;
	public float aimTime = 0.2f;
	public bool automatic = true;

}
	
public enum GunClass
{
	LMG, Assault, SniperRifle
}
