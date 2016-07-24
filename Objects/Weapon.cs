using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject 
{
	public GameObject prefab;
	public WeaponType weaponType;
	public string weaponName;
	public float movementScalar = 1f, fireRate = 480f, reloadSpeed = 1f, damage = 50f;
}

public enum WeaponType
{
	Gun, Melee
}