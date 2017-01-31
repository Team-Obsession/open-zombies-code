using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : Prerequisite
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
		GameController.Instance ().RegisterPlayerSpawnPoint (this);
	}
}







