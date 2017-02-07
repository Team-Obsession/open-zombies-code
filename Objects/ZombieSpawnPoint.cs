using UnityEngine;
using System.Collections;

public class ZombieSpawnPoint : Prerequisite
{
	public void OnEnable()
	{
		RegisterSatisfied (Register);
	}

	public void OnDisable()
	{
		UnregisterSatisfied (Register);
	}


	private void Register ()
	{
		GameController.Instance ().RegisterZombieSpawnPoint (this);
	}
}
