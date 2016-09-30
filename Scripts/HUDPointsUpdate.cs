using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDPointsUpdate : MonoBehaviour
{
	LocalPlayer player;
	Text text;

	void OnPointsUpdate (int newPoints)
	{
		text.text = newPoints.ToString ();
	}

	void Start()
	{
		player = GetComponentInParent<HUD>().GetPlayer ();
		if (player != null)
		{
			player.RegisterPointsChange (OnPointsUpdate);
		}
		text = GetComponent<Text>();
		OnPointsUpdate (player.Points);
	}

	void OnDisable()
	{
		player.UnregisterPointsChange (OnPointsUpdate);
	}
}


