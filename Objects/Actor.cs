using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour 
{
	public float health = 100.0f;
	public float moveSpeed = 1f;
	public GameObject prefab;

	//Just to have a zero argument constructor
	public Actor ()
	{
	}

	public Actor (float health, float moveSpeed)
	{
		this.health = health;
		this.moveSpeed = moveSpeed;
	}

	public void SpawnAt( Transform trans)
	{
		//TODO: Don't just use instantiate, maybe use some object pooling or whatnot
		Instantiate(prefab, trans.position, trans.rotation);
	}

	public void Die()
	{
		//TODO: Don't just destroy the object, deactivate it and put it back into the object pool
		Destroy(this.gameObject);
	}

	
}












