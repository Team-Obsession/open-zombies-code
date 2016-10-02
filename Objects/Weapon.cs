using UnityEngine;
using System.Collections;

public class Weapon : ScriptableObject 
{
	public GameObject prefab;
	public WeaponType weaponType;
	public string weaponName;
	public float movementScalar = 1f, fireRate = 480f, reloadSpeed = 1f, damage = 50f;

	public GameObject Spawn (Transform parent)
	{
		return (GameObject) Instantiate (prefab, parent.position, parent.rotation, parent);
	}
}

public enum WeaponType
{
	Gun, Melee
}