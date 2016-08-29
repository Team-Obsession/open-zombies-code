using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponHandler : MonoBehaviour
{
	public PlayerInput input;
	public List<WeaponInstance> weapons;

	Player player;
	WeaponInstance currentWeapon; //TODO: add dual-wield functionality

	void OnEnable()
	{
		if (input == null)
		{
			if ((input = GetComponent<PlayerInput>()) == null)
			{
				Debug.LogError ("No PlayerInput componenet on " + this.gameObject.name);
			}
		}
		player = input.Player;
	}	
}
