using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapon/Melee", order = 2)]
public class Melee : Weapon
{
	private bool canAttack = true;
	public bool CanAttack
	{
		get {	return canAttack;	}
		protected set
		{
			if (value != canAttack)
			{
				canAttack = value;
				OnAttack();
			}
		}
	}

	protected Action cbAttack;

	private void OnAttack()
	{
		if(cbAttack != null)
		{
			cbAttack();
		}
	}

	public void RegisterAttack(Action callbackFunc)
	{
		cbAttack += callbackFunc;
	}
	public void UnregisterAttack(Action callbackFunc)
	{
		cbAttack -= callbackFunc;
	}
}
