using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDPointsUpdate : HUDRelatedScript
{
	LocalPlayer player;
	Text text;

	void OnPointsUpdate (int newPoints)
	{
		text.text = newPoints.ToString ();
	}

	public override void OnInitialize()
	{
		player = GetComponentInParent<HUD>().Player;
		if (player != null)
		{
			player.RegisterPointsChange (OnPointsUpdate);
		}
		text = GetComponent<Text>();
		OnPointsUpdate (player.Points);
	}

	public override void OnTerminate()
	{
		if (player != null)
		{
			player.UnregisterPointsChange (OnPointsUpdate);
		}
	}
}


