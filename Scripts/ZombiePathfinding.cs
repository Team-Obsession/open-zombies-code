using UnityEngine;
using System.Collections;

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
		if(col.gameObject.GetComponent<Player>() != null) //we hit a player
		{
			
		}
	}

	void OnTargetChange()
	{
		stoppingDistance = 1.1f * (zombie.Target.GetComponent<CharacterController>().radius + zombie.GetComponent<CapsuleCollider>().radius);
	}

	void Update()
	{
		navAgent.destination = StoppingPoint ( zombie.Target.transform.position, zombie.transform.position, stoppingDistance);

	}

	public Vector3 StoppingPoint (Vector3 from, Vector3 to, float distance)
	{
		Vector3 direction = (to - from).normalized;
		return from + (direction * distance);
	}



}










