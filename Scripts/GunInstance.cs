using UnityEngine;
using System;
using System.Collections;

public class GunInstance : WeaponInstance
{
	public Transform graphic;
	public Transform hipPoint;
	public Transform aimPoint;
	public GameObject shootPoint;
	public GameObject hitPrefab;
	public Animation anim;

	PauseHandler pauseHandler;
	Gun gun;
	float timeKeeper;

	private int magazineSize, maxExtraMags, bulletsInMag, excessAmmo;


	public int BulletsInMag
	{
		get {	return bulletsInMag;	}
		protected set
		{
			if (value != bulletsInMag)
			{
				bulletsInMag = value;
				OnBulletCountChange();
			}
		}
	}

	public int ExtraAmmo
	{
		get		{	return excessAmmo;	}
	}

	public int MaxAmmo
	{
		get		{	return maxExtraMags * (magazineSize + 1);	}
	}

	void OnInputShoot (float timeHeld)
	{
		if(timeHeld == 0f)
		{
			Shoot(1);
			timeKeeper = Time.time;
		}
		else if (gun.automatic)
		{
			if(Time.time - timeKeeper >= 60.0f / gun.fireRate)
			{
				Shoot(1);
				timeKeeper = Time.time;
			}
		}
	}

	void OnInputAim (float timeHeld)
	{
		graphic.position = Vector3.Lerp (graphic.position, aimPoint.position, timeHeld / gun.aimTime);
		graphic.rotation = Quaternion.Lerp (graphic.rotation, aimPoint.rotation, timeHeld / gun.aimTime);
	}

	void OnInputNotAim (float timeHeld)
	{
		graphic.position = Vector3.Lerp (graphic.position, hipPoint.position, timeHeld / gun.aimTime);
		graphic.rotation = Quaternion.Lerp (graphic.rotation, hipPoint.rotation, timeHeld / gun.aimTime);
	}

	void OnInputReload (float timeHeld)
	{
		if (timeHeld == 0f)
		{
			Reload ();
		}
	}

	/// <summary>
	/// Reload this gun.
	/// </summary>
	public void Reload()
	{
		if(BulletsInMag == magazineSize)
		{
			return;
		}
		if(excessAmmo > 0)
		{
			if(excessAmmo >= magazineSize - BulletsInMag)
			{
				excessAmmo -= magazineSize - BulletsInMag;
				BulletsInMag = magazineSize;
			}
			else
			{
				BulletsInMag = excessAmmo;
				excessAmmo = 0;
			}
		}
		OnReload();
	}

	/// <summary>
	/// Shoot up to the specified number of bullets.
	/// Returns the actual number of bullets shot.
	/// </summary>
	/// <param name="numBullets">Number bullets.</param>
	public int Shoot (int numBullets)
	{
		int bulletsShot = 0;
		if (BulletsInMag > 0)
		{
			if(BulletsInMag >= numBullets)
			{
				BulletsInMag -= numBullets;
				bulletsShot = numBullets;
			}
			else
			{
				BulletsInMag = 0;
				bulletsShot = numBullets - BulletsInMag;
			}
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
		return bulletsShot;
	}

	void OnPauseStateChange (bool newState)
	{
		if (newState)
		{
			UnregisterCallbacks ();
		}
		else
		{
			RegisterCallbacks ();
		}
	}

	public override void OnInitialize ()
	{
		if (gun == null)
		{
			gun = (Gun) weapon;
		}
		if (player == null)
		{
			player = GetComponentInParent<LocalPlayer>();
		}
		if(input == null && ((input = player.GetComponent<PlayerInput>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find an input from its player");
		}
		if(anim == null && ((anim = GetComponent<Animation>()) == null))
		{
			Debug.LogError ("No Animation Component on " + gameObject.name);
		}
		if(pauseHandler == null && ((pauseHandler = PauseHandler.Instance) == null))
		{
			Debug.LogError ("No PauseHandler found by " + gameObject.name);
		}

		RegisterCallbacks ();
		pauseHandler.RegisterPauseStateChange (OnPauseStateChange);

		if (!hasInitialized)
		{
			magazineSize = gun.magazineSize;
			maxExtraMags = gun.maxExtraMags;
			BulletsInMag = magazineSize;
			excessAmmo = maxExtraMags * magazineSize;
		}

	}


	public override void OnTerminate()
	{
		pauseHandler.RegisterPauseStateChange (OnPauseStateChange);
		if (player == null || input == null || weapHandler == null)	{ 	return;		}
		UnregisterCallbacks ();
	}

	void RegisterCallbacks ()
	{
		input.RegisterInputShoot (OnInputShoot);
		input.RegisterInputReload (OnInputReload);
		input.RegisterInputAim (OnInputAim);
		input.RegisterInputNotAim (OnInputNotAim);
	}

	void UnregisterCallbacks ()
	{
		input.UnregisterInputShoot (OnInputShoot);
		input.UnregisterInputReload (OnInputReload);
		input.UnregisterInputAim (OnInputAim);
		input.UnregisterInputNotAim (OnInputNotAim);
	}



/*	
	=====================================
	|			CALLBACKS				|
	=====================================
*/

	protected Action cbShoot;
	protected Action<WeaponInstance> cbBulletCountChange;
	protected Action cbMagazineEmpty; 
	protected Action cbReload;


	private void OnShoot()
	{
		if(cbShoot != null)
		{
			cbShoot();
		}
	}

	private void OnBulletCountChange()
	{
		if(cbBulletCountChange != null)
		{
			cbBulletCountChange (this);
		}
	}

	private void OnMagazineEmpty()
	{
		if(cbMagazineEmpty != null)
		{
			cbMagazineEmpty();
		}

	}		

	private void OnReload()
	{
		if(cbReload != null)
		{
			cbReload();
		}
	}

	public void RegisterShoot (Action callbackFunc)
	{
		cbShoot += callbackFunc;
	}
	public void UnregisterShoot (Action callbackFunc)
	{
		cbShoot -= callbackFunc;
	}

	public void RegisterBulletCountChange (Action<WeaponInstance> callbackFunc)
	{
		cbBulletCountChange += callbackFunc;
	}
	public void UnregisterBulletCountChange (Action<WeaponInstance> callbackFunc)
	{
		cbBulletCountChange -= callbackFunc;
	}

	public void RegisterMagazineEmpty (Action callbackFunc)
	{
		cbMagazineEmpty += callbackFunc;
	}
	public void UnregisterMagazineEmpty (Action callbackFunc)
	{
		cbMagazineEmpty -= callbackFunc;
	}

	public void RegisterReload (Action callbackFunc)
	{
		cbReload += callbackFunc;
	}
	public void UnregisterReload (Action callbackFunc)
	{
		cbReload -= callbackFunc;
	}
}
