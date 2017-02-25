using UnityEngine;
using System.Collections;

public class ZombieDeath : MonoBehaviour
{

	private Zombie zombie; //this zombie

	void OnDie (Actor actor)
	{
		Destroy(this.gameObject);
	}

	void Start()
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
