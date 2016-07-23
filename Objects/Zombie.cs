using UnityEngine;
using System.Collections;

public class Zombie : Actor 
{

	Player target;




	void GetTarget()
	{
		target = transform.ClosestOf (GameController.Instance ().playerTransforms).GetComponent<Player>();
	}
	
}












