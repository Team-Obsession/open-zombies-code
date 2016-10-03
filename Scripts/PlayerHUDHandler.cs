using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the player's input, querying HUDHandler to switch menus, and sizing the player's camera
/// </summary>
public class PlayerHUDHandler : HUDRelatedScript
{

	public List<GameObject> menuObjects;
	public LocalPlayer player;
	public Camera cam;

	private GameController gameController;
	private int index;


	void OnPauseStateChange (bool newState)
	{
		HUDHandler.Instance.SwitchMenu (player, newState ? "Pause Canvas" : "HUD Canvas" );
	}

	public override void OnInitialize()
	{
		gameController = GameController.Instance ();
		player = GetComponent<LocalPlayer>();
		this.index = player.playerIndex;
		this.cam = player.cam;

		PauseHandler.Instance.RegisterPauseStateChange (OnPauseStateChange);

		//Set up the camera viewport
		switch (gameController.numLocalPlayers)
		{
			case 1:
			{
				cam.rect = HUD.fullscreen;
				break;
			}
			case 2:
			{
				cam.rect = HUD.TwoPlayerIndexToViewportRect (index);
				break;
			}
			case 3:
			{
				cam.rect = HUD.ThreePlayerIndexToViewportRect (index);
				break;
			}
			case 4:
			{
				cam.rect = HUD.FourPlayerIndexToViewportRect (index);
				break;
			}
		}

		cam.cullingMask = cam.cullingMask | LayerMask.NameToLayer ("HUD" + player.playerIndex);
	}

	public override void OnTerminate ()
	{
		PauseHandler.Instance.UnregisterPauseStateChange (OnPauseStateChange);
	}

}
