using UnityEngine;
using System;
using System.Collections;

public class Actor : MonoBehaviour 
{

	private float health = 100f;
	public float Health
	{ 
		get {	return health;	}
		protected set
		{
			if (value != health)
			{
				health = value;
				OnHealthChange();
				if(health <= 0f)
				{
					OnDie();
				}
			}
		}
	}
	private float moveSpeed = 1f;
	public float MoveSpeed
	{
		get {	return moveSpeed;	}
		set
		{
			if(value != moveSpeed)
			{
				moveSpeed = value;
				OnMoveSpeedChange();
			}
		}
	}
	public GameObject prefab;

	//Just to have a zero argument constructor
	public Actor (){}
	//The actual constructor to use when creating a new Actor
	public Actor (float health, float moveSpeed)
	{
		this.health = health;
		this.moveSpeed = moveSpeed;
	}

	public GameObject SpawnAt (Transform trans)
	{
		//TODO: Don't just use instantiate, maybe use some object pooling or whatnot
		return (GameObject)Instantiate(prefab, trans.position, trans.rotation);
	}

	public void TakeDamage (float amount, Actor sender)
	{
		if(amount != 0f)
		{
			Health -= amount;
		}
	}
	public void TakeDamage (float amount, Actor sender, ref ActorHitInfo hitInfo)
	{
		hitInfo.hitActor = this;
		hitInfo.didDamage = true;
		hitInfo.didDie = Health - amount > 0;
		if(amount != 0f)
		{
			Health -= amount;
		}
	}

	public void ChangeMoveSpeed (float newSpeed)
	{
		if ( newSpeed != MoveSpeed)
		{
			MoveSpeed = newSpeed;
		}
	}

	/* 
		Callbacks to notify other objects when an event occurs.

		Uses less overhead than the alternative of polling a variable every so often (such as every frame).

		Let's say that you are the GameController and you want to know when something dies. Rather than poll
		EVERY Actor EVERY frame you can just be notified when an Actor dies. It's the difference between a 
		naggy wife and a wife who waits for her husband to tell her that he took out the trash.

		To register a callback, call an actor's registerwhatevercallback function with the function you want
		to be called as the parameter (DON'T INCLUDE PARENTHESIS IN THE ARGUMENT).
		EX: call "RegisterDie (PlayDeathAnimation);" in the animation script.
	*/
	protected Action cbDie;
	protected Action cbHealthChange;
	protected Action cbMoveSpeedChange;

	private void OnDie()
	{
		if(cbDie != null)
		{
			cbDie();
		}
		Debug.Log (gameObject.name + " has died");
		//TODO: Implement a death handler script where the actor could play an animation or whatever before being destroyed.
	}

	private void OnHealthChange ()
	{
		if(cbHealthChange != null)
		{
			cbHealthChange();
		}
	}

	private void OnMoveSpeedChange ()
	{
		if(cbMoveSpeedChange != null)
		{
			cbMoveSpeedChange();
		}
	}


	/// <summary>
	/// Call to have callbackFunc called when this Actor's health reaches zero.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void RegisterDie(Action callbackFunc)
	{
		cbDie += callbackFunc;
	}

	/// <summary>
	/// Call to unregister callbackFunc from the Die callback.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void UnregisterDie (Action callbackFunc)
	{
		cbDie -= callbackFunc;
	}

	/// <summary>
	/// Call to have callbackFunc called when this Actor's health changes.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void RegisterHealthChange (Action callbackFunc)
	{
		cbHealthChange += callbackFunc;
	}

	/// <summary>
	/// Call to unregister callbackFunc from the HealthChange callback.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void UnregisterHealthChange (Action callbackFunc)
	{
		cbHealthChange -= callbackFunc;
	}

	/// <summary>
	/// Call to have callbackFunc called when this Actor's move speed changes.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void RegisterMoveSpeedChange (Action callbackFunc)
	{
		cbMoveSpeedChange += callbackFunc;
	}

	/// <summary>
	/// Call to unregister callbackFunc from the MoveSpeedChange callback.
	/// </summary>
	/// <param name="callbackFunc">Callback func.</param>
	public void UnregisterMoveSpeedChange (Action callbackFunc)
	{
		cbMoveSpeedChange -= callbackFunc;
	}

}






