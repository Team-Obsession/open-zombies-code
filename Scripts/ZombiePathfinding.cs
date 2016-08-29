using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(NavMeshAgent))]
public class ZombiePathfinding : MonoBehaviour 
{
	public Zombie zombie;
	public NavMeshAgent navAgent;
	public float stopTime = 1f;

	float stoppingDistance = 1f;

	void Start()
	{
		if(zombie == null)
		{
			zombie = GetComponent<Zombie> ();
			if(zombie == null)
			{
				Debug.LogError ("Some zombie GameObject doesn't have a Zombie component");
			}
		}
		if(navAgent == null)
		{
			navAgent = GetComponent<NavMeshAgent>();
			if(navAgent == null)
			{
				Debug.LogError ("Some zombie GameObject doesn't have a NavMeshAgent component");
			}
		}
		zombie.RegisterTargetChange (OnTargetChange);
		zombie.GetTarget ();
		OnTargetChange ();

	}

	void OnCollisionEnter(Collision col)
	{
		Player player = col.gameObject.GetComponent<Player>();
		if(player != null) //we hit a player
		{
			player.TakeDamage (0f, zombie);
		}
	}

	void OnTargetChange()
	{
		stoppingDistance = 1.1f * (zombie.Target.GetComponent<CapsuleCollider>().radius + zombie.GetComponent<CapsuleCollider>().radius);
		navAgent.stoppingDistance = stoppingDistance;
	}

	void Update()
	{
		navAgent.destination = zombie.Target.transform.position;

	}

	public Vector3 StoppingPoint (Vector3 from, Vector3 to, float distance)
	{
		Vector3 direction = (to - from).normalized;
		return from + (direction * distance);
	}



}










