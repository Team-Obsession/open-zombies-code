using UnityEngine;
using System.Collections;
using System;

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

}
