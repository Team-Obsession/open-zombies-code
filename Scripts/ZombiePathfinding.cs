using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombiePathfinding : MonoBehaviour 
{
	public Zombie zombie;
	public NavMeshAgent navAgent;
	public float stopTime = 1f;

	void OnCollisionEnter (Collision col)
	{
		Player player = col.gameObject.GetComponent<Player>();
		if (player != null) //we hit a player
		{
			player.TakeDamage (50f, zombie);
		}
	}

	void OnTargetChange()
	{
		//Do stuff here
	}

	void OnSpeedChange (float newSpeed)
	{
		navAgent.speed = newSpeed;
	}

	void Update()
	{
		navAgent.destination = zombie.Target.position;
	}

	void Start()
	{
		if (zombie == null && ((zombie = GetComponent<Zombie>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a Zombie component");
		}
		if (navAgent == null && ((navAgent = GetComponent<NavMeshAgent>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a NavMeshAgent component");
		}
		zombie.GetTarget ();
		OnSpeedChange (zombie.MinSpeed);
		OnTargetChange ();

	}

	void RegisterCallbacks ()
	{
		zombie.RegisterTargetChange (OnTargetChange);
		zombie.RegisterMoveSpeedChange (OnSpeedChange);
	}

	public Vector3 StoppingPoint (Vector3 from, Vector3 to, float distance)
	{
		Vector3 direction = (to - from).normalized;
		return from + (direction * distance);
	}

}










