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
	IShootable fireType;

	private int magazineSize, bulletsInMag, extraAmmo;
	private float currentAccuracy = 0f;
	private bool reloading = false, cocking = false;


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
		get	{	 return extraAmmo;	 }
		protected set
		{
			if (value != extraAmmo)
			{
				extraAmmo = Mathf.Min (value, gun.MaxExtraAmmo);
				OnBulletCountChange ();
			}
		}
	}




	/**
	*	look in fireType for the handling of shoot input
	*/

	void OnInputAim (float timeHeld)
	{
		currentAccuracy = Mathf.Lerp (currentAccuracy, gun.aimAccuracy, timeHeld / gun.aimTime);
		graphic.position = Vector3.Lerp (graphic.position, aimPoint.position, timeHeld / gun.aimTime);
		graphic.rotation = Quaternion.Lerp (graphic.rotation, aimPoint.rotation, timeHeld / gun.aimTime);
	}

	void OnInputNotAim (float timeHeld)
	{
		currentAccuracy = Mathf.Lerp (currentAccuracy, gun.hipAccuracy, timeHeld / gun.aimTime);
		graphic.position = Vector3.Lerp (graphic.position, hipPoint.position, timeHeld / gun.aimTime);
		graphic.rotation = Quaternion.Lerp (graphic.rotation, hipPoint.rotation, timeHeld / gun.aimTime);
	}

	void OnInputReload (float timeHeld)
	{
		if (timeHeld == 0f)
		{
			reloading = true;
			StartCoroutine (Extensions.CallAfterSeconds(Reload, gun.reloadSpeed));
		}
	}

	void OnInputShoot (float timeHeld)
	{
		if (reloading)		{		return;		}
		//Check to see if we can shoot
		if (BulletsInMag == 0)
		{
			OnInputReload (timeHeld);
			return;
		}
		//Shoot according to our FireType
		fireType.OnInputShoot (timeHeld);
	}

	/// <summary>
	/// Reload this gun.
	/// </summary>
	public void Reload()
	{
		if (ExtraAmmo > 0)
		{
			int bulletsReloaded = ExtraAmmo < magazineSize - BulletsInMag ? ExtraAmmo : magazineSize - bulletsInMag;
			ExtraAmmo -= bulletsReloaded;
			BulletsInMag += bulletsReloaded;
		}
		reloading = false;
		OnReload();
	}

	/// <summary>
	/// Shoot up to the specified number of bullets.
	/// Returns the actual number of bullets shot.
	/// </summary>
	/// <param name="numBullets">Number bullets.</param>
	public int Shoot ( int numBullets )
	{
		int bulletsShot = Mathf.Min (BulletsInMag, numBullets);
		RaycastHit[] hits;
		for (int j = 0; j < bulletsShot * gun.bulletsPerShot; j++)
		{
			hits = Physics.RaycastAll (new Ray (player.cam.transform.position, player.cam.transform.forward)/*Extensions.RotateRay (player.cam.transform, UnityEngine.Random.value * currentAccuracy, UnityEngine.Random.value * 360f)*/);
			if (hits.Length != 0)
			{
				//Sort by distance (squared distance, that is)
				Array.Sort (hits, Extensions.CompareRayCastHitByDistance);
				bool hasHitObstruction = false;
				int triggerColliderCount = 0;
				for (int i = 0; i < hits.Length; i++)
				{
					if (hits [i].collider.isTrigger)
					{
						triggerColliderCount++;
						continue;
					}
					Actor hitActor = hits [i].transform.GetComponent<Actor> ();
					if (hitActor == null /*&& hits[i].collider.isTrigger == false*/)
					{
						hasHitObstruction = true;
						Quaternion effectRotation = new Quaternion ();
						effectRotation.SetLookRotation (hits [i].normal);
						Instantiate (hitPrefab, hits [i].point, effectRotation);
					}
					else if (!hasHitObstruction)
					{
						hitActor.TakeDamage (gun.damage * Mathf.Max (1f - (i - triggerColliderCount) * 0.2f, 0f), player);
						player.Points += 10;
					}
				}
			}
		}
		BulletsInMag -= bulletsShot;
		if (bulletsShot > 0)
		{
			anim.Stop ();
			anim.Play ();
		}
		return bulletsShot;
	}

	/// <summary>
	/// Adds the specified number of bullets to this GunInstance's extraAmmo, but extraAmmo will not exceed (magazineSize * maxExtraMags)
	/// </summary>
	/// <param name="bullets">Number of bullets to be added</param>
	public void AddAmmo (int bullets)
	{
		ExtraAmmo += bullets;
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
		if (graphic == null)
		{
			graphic = GetComponentInChildren<Renderer>().transform;
		}

		SetFireType ();

		RegisterCallbacks ();
		pauseHandler.RegisterPauseStateChange (OnPauseStateChange);

		if (!hasInitialized)
		{
			magazineSize = gun.magazineSize;
			BulletsInMag = magazineSize;
			extraAmmo = gun.InitialExtraAmmo;
			currentAccuracy = gun.hipAccuracy;
		}

	}

	public override void OnTerminate()
	{
		pauseHandler.UnregisterPauseStateChange (OnPauseStateChange);
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

	public void SetFireType ()
	{
		switch (gun.fireType)
		{
			case FireType.Automatic:
				fireType = new Shoot_Automatic (this, gun);
			break;

			case FireType.SemiAuto :
				fireType = new Shoot_SemiAuto (this, gun);
			break;

			case FireType.BoltAction :
				fireType = new Shoot_BoltAction (this, gun);
			break;

			default :
				fireType = new Shoot_SemiAuto (this, gun);
			break;
		}
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

	/// <summary>
	/// Calls the cbBulletCountchange callback with this GunInstance as the parameter. Call this anytime 
	/// </summary>
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
