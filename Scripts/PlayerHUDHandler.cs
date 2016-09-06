using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the player's input, querying HUDHandler to switch menus, and sizing the player's camera
/// </summary>
public class PlayerHUDHandler : MonoBehaviour
{
	

	public List<GameObject> menuObjects;
	public LocalPlayer player;
	public Camera cam;

	PlayerInput input;

	private GameController gameController;
	private int index;

	private bool isPaused = false;

	void OnInputPause()
	{
		isPaused = !isPaused;
		HUDHandler.Instance ().SwitchMenu (player, isPaused ? "Pause Canvas" : "HUD Canvas" );
	}

	void OnEnable()
	{
		gameController = GameController.Instance ();
		player = GetComponent<LocalPlayer>();
		this.index = player.playerIndex;
		this.cam = player.cam;

		input = player.input;
		input.RegisterInputPause (OnInputPause);

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


}
