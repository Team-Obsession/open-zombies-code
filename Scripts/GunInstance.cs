using UnityEngine;
using System.Collections;

public class GunInstance : WeaponInstance
{
	public GameObject shootPoint;
	public GameObject hitPrefab;
	public Animation anim;
	public LocalPlayer player;
	public PlayerWeaponHandler weapHandler;
	public Gun gun;

	PlayerInput input;
	float timeKeeper;

	private bool paused = false;

	void OnInputPause()
	{
		paused = !paused;
		if (paused)
		{
			input.UnregisterInputShoot (OnInputShoot);
		}
		else
		{
			input.RegisterInputShoot (OnInputShoot);
		}
	}

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
		if (Physics.Raycast (new Ray (player.cam.transform.position, player.cam.transform.forward), out hit))
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
		anim.Stop ();
		anim.Play ();
	}


	void Start()
	{
		//TODO: Placeholder, implement procedural gun adding
		input = weapHandler.input;
		if(input == null && ((input = player.GetComponent<PlayerInput>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find an input from its player");
		}
		if(anim == null && ((anim = GetComponent<Animation>()) == null))
		{
			Debug.LogError ("No Animation Component on " + gameObject.name);
		}

		player = input.Player;

		gun.RegisterShoot (Shoot);
		input.RegisterInputPause (OnInputPause);
		input.RegisterInputShoot (OnInputShoot);
	}

	void OnDisable()
	{
		gun.UnregisterShoot (Shoot);
		input.UnregisterInputPause (OnInputPause);
		input.UnregisterInputShoot (OnInputShoot);
	}
}
