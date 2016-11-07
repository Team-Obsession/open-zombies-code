using UnityEngine;
using System.Collections;

public class Shoot_Automatic : IShootable
{

	private GunInstance gi;
	private Gun gun;
	private float timeKeeper = 0f;

	public Shoot_Automatic (GunInstance gi, Gun gun)
	{
		this.gi = gi;
		this.gun = gun;
	}

	public void OnInputShoot (float timeHeld)
	{
		if (Time.time - timeKeeper >= 60.0f / gun.fireRate)
		{
			gi.Shoot (gun.bulletsPerShot);
			timeKeeper = Time.time;
		}
	}
}
