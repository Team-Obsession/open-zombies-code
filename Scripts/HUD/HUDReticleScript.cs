using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDReticleScript : HUDRelatedScript
{
	LocalPlayer player;
	PlayerInput input;
	Image reticle;

	void OnInputAim (float timeHeld)
	{
		reticle.enabled = false;
	}

	void OnInputNotAim (float timeHeld)
	{
		reticle.enabled = true;
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
		if (reticle == null && ((reticle = GetComponent<Image>()) == null))
		{
			Debug.LogError (GetType ().ToString () + " on " + name + " couldn't find an Image");
		}
		input.RegisterInputAim (OnInputAim);
		input.RegisterInputNotAim (OnInputNotAim);
		
	}

	public override void OnTerminate ()
	{
		if (input != null)
		{
			input.UnregisterInputAim (OnInputAim);
			input.UnregisterInputNotAim (OnInputNotAim);
		}
	}
}





