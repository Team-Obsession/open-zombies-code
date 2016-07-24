using UnityEngine;
using System.Collections;

public class Weapon : ScriptableObject 
{
	public GameObject prefab;
	public WeaponType weaponType;
	public string weaponName;
	public float movementScalar, fireRate, reloadSpeed;
}

public enum WeaponType
{
	Gun, Melee
}