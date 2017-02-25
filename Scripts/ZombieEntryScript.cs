using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEntryScript : MonoBehaviour
{

	[SerializeField]
	private GameObject plankPrefab;
	[SerializeField]
	private Transform windowCenter;

	private ZombieEntry entry;
	private List<GameObject> planks = new List<GameObject> ();
	private int numPlanks;


	void OnRepairLevelChange (int newLevel)
	{
		int delta = newLevel - numPlanks;
		SpawnPlanks (delta);
		numPlanks = newLevel;
	}

	void SpawnPlanks (int delta)
	{
		if (delta > 0)
		{
			for (int i = 0; i < delta; i++)
			{
				planks.Add (	(GameObject) Instantiate (
									plankPrefab,
									windowCenter.position,
									windowCenter.rotation * Quaternion.Euler (new Vector3 (0f, 0f, UnityEngine.Random.value * 180f)),
									this.transform
								));
			}
		}
		else if (delta < 0)
		{ //TODO: Object pooling
			for (int i = 0; i < -1 * delta; i++)
			{
				GameObject destroyee = planks.GetRandomElement ();
				planks.Remove(destroyee);
				Destroy (destroyee);
			}
		}
	}

	void Start ()
	{
		if ( (entry = GetComponent<ZombieEntry> ()) == null)
		{
			Debug.LogError (this.name + " on " + gameObject.name + " couldn't find a ZombieEntry component");
		}
		numPlanks = entry.CurrentRepairLevel;
		SpawnPlanks (numPlanks);
		RegisterCallbacks ();
	}

	void OnDisable ()
	{
		UnregisterCallbacks ();
	}

	void RegisterCallbacks ()
	{
		entry.RegisterRepairLevelChange (OnRepairLevelChange);
	}
	void UnregisterCallbacks ()
	{
		entry.UnregisterRepairLevelChange (OnRepairLevelChange);
	}
}







