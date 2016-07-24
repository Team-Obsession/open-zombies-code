using UnityEngine;
using System.Collections;

public class Gun : Weapon 
{
	string weaponName, weaponClass;

	public Gun(string weaponName, string weaponClass)
	{
		this.weaponName = weaponName;
		this.weaponClass = weaponClass;
	}

}
