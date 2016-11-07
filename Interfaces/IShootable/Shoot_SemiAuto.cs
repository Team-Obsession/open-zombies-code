using UnityEngine;
using System.Collections;

public class Shoot_SemiAuto : IShootable
{

	private GunInstance gi;
	private Gun gun;
	private float timeKeeper = 0f;

	public Shoot_SemiAuto (GunInstance gi, Gun gun)
	{
		this.gi = gi;
		this.gun = gun;
	}

	public void OnInputShoot (float timeHeld)
	{
		if (timeHeld == 0f && Time.time - timeKeeper >= 60.0f / gun.fireRate)
		{
			gi.Shoot (gun.bulletsPerShot);
			timeKeeper = Time.time;
		}
	}
}