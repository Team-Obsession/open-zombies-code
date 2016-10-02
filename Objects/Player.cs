using UnityEngine;
using System;
using System.Collections;

public class Player : Actor 
{
	public PlayerLoadout loadout;
	public float baseMoveSpeed = 5f; //in m/s
	public float sprintScalar = 1.2f;
	public float baseJumpForce = 4f;
	public float extraJumpTime = 1f; //The amount of time you can affect your jump by holding the button
	public float turnSpeed = 1f; //In rotations per second, where the input's absolute value is 1
	public float airControlScalar = 0.75f;
	public float groundControlDamp = 5f; //Higher values yield more responsive movement
	public float airControlDamp = 3f;

	private int points = 500;
	public int Points
	{
		get {	return points;		}
		set
		{
			if (value != points)
			{
				points = value;
				OnPointsChange ();
			}
		}
	}

	Action<int> cbPointsChange;

	void OnPointsChange()
	{
		if (cbPointsChange != null)
		{
			cbPointsChange(points);
		}
	}

	public void RegisterPointsChange (Action<int> callbackFunc)
	{
		cbPointsChange += callbackFunc;
	}
	public void UnregisterPointsChange (Action<int> callbackFunc)
	{
		cbPointsChange -= callbackFunc;
	}
}












