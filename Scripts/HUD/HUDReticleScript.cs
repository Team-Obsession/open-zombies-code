using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class HUDReticleScript : HUDRelatedScript
{
	Canvas canvas;
	RectTransform rectTrans;
	LocalPlayer player;
	PlayerInput input;
	PlayerWeaponHandler pWeapHandler;
	Weapon weap;
	float aimTime = 0.5f;

	private float accuracy = 15f;
	public float Accuracy
	{
		get 	{ return accuracy; }
		set
		{ 
			if (accuracy != value)
			{
				accuracy = value;
				ChangeReticleAccuracy (accuracy);
			}
		}
	}


	void OnInputAim (float timeHeld)
	{
		if (weap.weaponType == WeaponType.Gun)
		{
			Gun gun = (Gun)weap;
			if (timeHeld < aimTime)
			{
				Accuracy = Mathf.Lerp (Accuracy, gun.aimAccuracy, timeHeld / aimTime);
			}
			else
			{
				Accuracy = gun.aimAccuracy;
				canvas.enabled = false;
			}

		}

	}

	void OnInputNotAim (float timeHeld)
	{
		if (canvas.enabled == false)	{	canvas.enabled = true;	}
		if (weap.weaponType == WeaponType.Gun)
		{
			Gun gun = (Gun)weap;
			Accuracy = Mathf.Lerp (Accuracy, gun.hipAccuracy, timeHeld / aimTime);
		}
	}

	void OnWeaponChange (WeaponInstance newWeap)
	{
		weap = newWeap.weapon;
		if (weap.weaponType == WeaponType.Gun)
		{
			Gun gun = (Gun) weap;
			Accuracy = gun.hipAccuracy;
		}
		aimTime = weap.aimTime;
	}

	private void ChangeReticleAccuracy (float accuracy) //accuracy is in degrees
	{
		float newSize = 2 * Screen.height * (accuracy / player.cam.fieldOfView);
		rectTrans.sizeDelta = new Vector2 (newSize, newSize);
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
		if ((canvas = GetComponent<Canvas> ()) == null)
		{
			Debug.LogError (GetType ().ToString () + " on " + name + " does not have a Canvas component attached");
		}

		rectTrans = (RectTransform)transform;
		RegisterCallbacks();


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





