using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : Prerequisite
{
	public void OnEnable()
	{
		if (IsSatisfied)
		{
			Register (true);
		}
		RegisterSatisfiedChange (Register);
	}

	public void OnDisable()
	{
		UnregisterSatisfiedChange (Register);
	}


	private void Register ( bool satisfied )
	{
		if (satisfied)
		{
			GameController.Instance ().RegisterPlayerSpawnPoint (this);
		}
		else
		{
			GameController.Instance ().UnregisterPlayerSpawnPoint (this);
		}
	}
}







