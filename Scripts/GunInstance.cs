using UnityEngine;
using System.Collections;

public class GunInstance : WeaponInstance
{
	public GameObject shootPoint;
	public GameObject hitPrefab;
	public Animation animation;
	public Player player;
	public PlayerWeaponHandler weapHandler;
	public Gun gun;

	PlayerInput input;
	float timeKeeper;

	void OnInputShoot (float timeHeld)
	{
		if(timeHeld == 0f && input.TimeHeldShoot >= 60.0f / gun.fireRate)
		{
			Shoot();
			timeKeeper = Time.time;
		}
		else
		{
			if(Time.time - timeKeeper >= 60.0f / gun.fireRate)
			{
				Shoot();
				timeKeeper = Time.time;
			}
		}
	}

	void Shoot ()
	{
		RaycastHit hit;
		if (Physics.Raycast (new Ray (shootPoint.transform.position, shootPoint.transform.forward), out hit))
		{
			Quaternion effectRotation = new Quaternion();
			effectRotation.SetLookRotation (hit.normal);
			Instantiate (hitPrefab, hit.point, effectRotation);
			Actor hitActor = hit.transform.GetComponent<Actor>();
			if (hitActor != null)
			{
				hitActor.TakeDamage (gun.damage, player);
			}
		}
		animation.Stop ();
		animation.Play ();
	}


	void OnEnable()
	{
		if((input = weapHandler.GetComponent<PlayerInput>()) == null)
		{
			Debug.LogError (gameObject.name + " couldn't find an input from its player");
		}
		if(animation == null && ((animation = GetComponent<Animation>()) == null))
		{
			Debug.LogError ("No Animation Component on " + gameObject.name);
		}

		gun.RegisterShoot (Shoot);
		input.RegisterInputShoot (OnInputShoot);
	}

	void OnDisable()
	{
		gun.UnregisterShoot (Shoot);
		input.UnregisterInputShoot (OnInputShoot);
	}
}
