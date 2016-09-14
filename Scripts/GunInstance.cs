using UnityEngine;
using System;
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
		RaycastHit[] hits;
		hits = Physics.RaycastAll (new Ray (player.cam.transform.position, player.cam.transform.forward));
		if (hits.Length != 0)
		{
			//Sort by distance (squared distance, that is)
			Array.Sort(hits, Extensions.CompareRayCastHitByDistance);
			bool hasHitObstruction = false;
			for (int i = 0; i < hits.Length; i++)
			{
				Actor hitActor = hits[i].transform.GetComponent<Actor>();
				if (hitActor == null)
				{
					hasHitObstruction = true;
					Quaternion effectRotation = new Quaternion();
					effectRotation.SetLookRotation (hits[i].normal);
					Instantiate (hitPrefab, hits[i].point, effectRotation);
				}
				else if (!hasHitObstruction)
				{
					hitActor.TakeDamage (gun.damage * Mathf.Max(1f - i * 0.2f, 0f), player);
					player.Points += 10;
				}

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
