using UnityEngine;
using System;
using System.Collections;

public class Zombie : Actor 
{

	private Player target;
	public Player Target
	{
		get {	return target;	}
		protected set
		{
			if(value != target)
			{
				target = value;
				OnTargetChange ();
			}
		}
	}



	public void GetTarget()
	{
		target = transform.ClosestOf (GameController.Instance ().PlayerGameObjects).GetComponent<Player>();
	}

	protected Action cbTargetChange;

	private void OnTargetChange()
	{
		if(cbTargetChange != null)
		{
			cbTargetChange ();
		}
	}

	public void RegisterTargetChange(Action callbackFunc)
	{
		cbTargetChange += callbackFunc;
	}

	public void UnregisterTargetChange(Action callbackFunc)
	{
		cbTargetChange -= callbackFunc;
	}

}












