using UnityEngine;
using System.Collections;

public class ZombieDeath : MonoBehaviour
{
	public Zombie zombie; //this zombie

	void OnDie()
	{
		Destroy(this.gameObject);
	}


	void OnEnable()
	{
		if ((zombie == null) && ((zombie = GetComponent<Zombie>()) == null))
		{
			Debug.LogError ("No Zombie component on " + gameObject.name);
		}
		zombie.RegisterDie (OnDie);
	}

	void OnDisable()
	{
		zombie.UnregisterDie (OnDie);
	}
}
