using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour
{
	public float timer = 1f;

	void Update ()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			Destroy(this.gameObject);
		}
	}
}
