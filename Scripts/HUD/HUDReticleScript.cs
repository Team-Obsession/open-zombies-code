using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDReticleScript : HUDRelatedScript
{
	[SerializeField]
	Sprite image; //pivot assumed to be right
	Canvas canvas;

	LocalPlayer player;
	PlayerInput input;
	PlayerWeaponHandler pWeapHandler;
	Weapon weap;
	float aimTime = 0.5f;

	Vector3 aimScale = Vector3.one;
	Vector3 hipScale = Vector3.one;


	void OnInputAim (float timeHeld)
	{
		if (weap.weaponType == WeaponType.Gun)
		{

		}

	}

	void OnInputNotAim (float timeHeld)
	{
		
	}

	void OnWeaponChange (WeaponInstance newWeap)
	{
		weap = newWeap.weapon;
		if (weap.weaponType == WeaponType.Gun)
		{
			Gun gun = (Gun) weap;

		}
		aimTime = weap.aimTime;
	}

	public override void OnInitialize ()
	{
		player = GetComponentInParent<HUD>().Player;
		if (player == null && ((player = GetComponentInParent<HUD>().Player) == null))
		{
			Debug.LogError (GetType ().ToString () + " on " + gameObject.name + " couldn't find a LocalPlayer");
		}
		if (input == null && ((input = player.input) == null))
		{
			Debug.LogError (GetType ().ToString () + " on " + name + " couldn't get a PlayerInput from its LocalPlayer");
		}
		if ((pWeapHandler = player.GetComponent<PlayerWeaponHandler> ()) == null)
		{
			Debug.LogError (GetType ().ToString () + " on " + name + " couldn't get a PlayerWeaponHandler from its LocalPlayer");
		}
		if (canvas == null && ((canvas = GetComponent<Canvas>()) == null))
		{
			Debug.LogError (GetType ().ToString () + " on " + name + " couldn't find an Image");
		}
		RegisterCallbacks();
	}

	private void CreateReticle()
	{
		
	}

	private void ChangeReticleAccuracy (float accuracy) //accuracy is in degrees
	{

	}

	public override void OnTerminate ()
	{
		UnregisterCallbacks ();
	}

	public void RegisterCallbacks ()
	{
		input.RegisterInputAim (OnInputAim);
		input.RegisterInputNotAim (OnInputNotAim);
		pWeapHandler.RegisterWeaponChange (OnWeaponChange);
	}

	public void UnregisterCallbacks ()
	{
		if (input != null)
		{
			input.UnregisterInputAim (OnInputAim);
			input.UnregisterInputNotAim (OnInputNotAim);
		}
		if (pWeapHandler != null)
		{
			pWeapHandler.UnregisterWeaponChange (OnWeaponChange);
		}
	}
}





