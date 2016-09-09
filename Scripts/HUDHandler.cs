using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDHandler : MonoBehaviour
{
	/// <summary>
	/// Call to get the scene's HUDHandler
	/// </summary>
	private static HUDHandler hudHandler;
	public static HUDHandler Instance()
	{
		if(!hudHandler)
		{
			hudHandler = FindObjectOfType(typeof (HUDHandler)) as HUDHandler;
			if(!hudHandler)
			{
				Debug.LogError("There must be one HUDHandler in the scene");
			}
		}
		return hudHandler;
	}

	public GameObject hudPrefab; //The UI that every LocalPlayer will have a copy of

	//Map of each player's HUD
	Dictionary <Player, HUD> playerHUDs;
	Dictionary <Player, PlayerHUDHandler> playerHUDHandlers;

	public void SwitchMenu (Player player, string menuName) //TODO: animate instead of just disabling
	{
		HUD hud = playerHUDs[player];
		foreach (GameObject menu in hud.menus)
		{
			if(menu.name == menuName)
			{
				menu.SetActive (true);
				hud.ActiveMenu = menu;
			}
			else
			{
				menu.SetActive (false);
			}
		}

	}

	void Awake ()
	{
		playerHUDs = new Dictionary <Player, HUD> ();
		playerHUDHandlers = new Dictionary <Player, PlayerHUDHandler> ();
	
		foreach (LocalPlayer player in GameController.Instance ().LocalPlayers)
		{

			//Initialize the HUD
			HUD playerHUD = ((GameObject)Instantiate (hudPrefab)).GetComponent<HUD> (); //Assumes that the prefab has a HUD component
			playerHUD.gameObject.name = hudPrefab.name + player.playerIndex;
			playerHUD.transform.SetParent (this.transform);
			playerHUD.SetPlayer (player);
			playerHUDs.Add (player,	playerHUD);
			playerHUD.gameObject.layer = LayerMask.NameToLayer ("HUD" + player.playerIndex);

			//Initalize the PlayerHUDHandler
			PlayerHUDHandler playerHUDHandler = player.gameObject.AddComponent<PlayerHUDHandler> ();
			playerHUDHandler.player = player;
			playerHUDHandlers.Add (player, playerHUDHandler);

			//Activate the default menu
			SwitchMenu (player, playerHUDs [player].defaultMenu.name);

			//Change the UI's scale for multiple players
			foreach (RectTransform trans in playerHUD.GetComponent<RectTransform>())
			{
				trans.anchorMin = playerHUDHandler.cam.rect.min;
				trans.anchorMax = playerHUDHandler.cam.rect.max;
				trans.pivot = playerHUDHandler.cam.rect.center;
			}

			//Set the Player's parameter(s) accordingly
			player.hudHandler = playerHUDHandler;
		}

	}

	
}
