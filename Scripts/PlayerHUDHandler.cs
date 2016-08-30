using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerHUDHandler : MonoBehaviour
{
	public static Rect fullscreen = new Rect (0f, 0f, 1f, 1f);
	public static Rect topHalf = new Rect(0f, 0.5f, 1f, 0.5f);
	public static Rect bottomHalf = new Rect (0f, 0f, 1f, 0.5f);
	public static Rect leftHalf = new Rect (0f, 0f, 0.5f, 1f);
	public static Rect rightHalf = new Rect (0.5f, 0f, 0.5f, 1f);
	public static Rect topLeftQuad = new Rect (0f, 0.5f, 0.5f, 0.5f);
	public static Rect topRightQuad = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
	public static Rect bottomLeftQuad = new Rect (0f, 0f, 0.5f, 0.5f);
	public static Rect bottomRightQuad = new Rect (0.5f, 0f, 0.5f, 0.5f);

	public List<GameObject> menuObjects;
	public Dictionary<string, GameObject> menus;
	public LocalPlayer player;
	public Camera cam;

	private GameController gameController;
	private int index;

	/// <summary>
	/// Switchs the menu to menuName
	/// </summary>
	/// <param name="menuName">Menu name.</param>
	public void SwitchMenu (string menuName) //TODO: animate instead of just disabling
	{
		foreach (KeyValuePair <string, GameObject> item in menus)
		{
			item.Value.SetActive (false);
		}
		menus[menuName].SetActive (true);
	}

	void OnEnable()
	{
		gameController = GameController.Instance ();
		//Get a player without a PlayerHUDHandler
		foreach(LocalPlayer player in gameController.LocalPlayers)
		{
			if(player.hudHandler == null)
			{
				player.hudHandler = this;
				this.player = player;
				this.index = player.playerIndex;
				this.cam = player.cam;
			}
		}

		//Create the Dictionary from the List
		foreach(GameObject obj in menuObjects)
		{
			menus.Add(obj.name, obj);
		}

		//Set up the camera viewport
		switch (gameController.numLocalPlayers)
		{
			case 1:
			{
				cam.rect = fullscreen;

				break;
			}
			case 2:
			{
				cam.rect = new Rect(0f, (index == 1 ? 0.5f : 0f), 1f, 0.5f);

				break;
			}
			case 3:
			{
				cam.rect = ThreePlayerIndexToViewportRect (index);
				break;
			}
			case 4:
			{
				cam.rect = FourPlayerIndexToViewportRect (index);
				break;
			}
		}
	}

	/*TODO: Redo to allow for multi-monitor edge cases (i.e. split vertically for two monitors and two players instead of horizontally)
				Maybe this could be a neat GUI selector
	*/
	public Rect ThreePlayerIndexToViewportRect(int playerIndex)
	{
		
		switch (playerIndex)
		{
			case 1:
			{
				return topLeftQuad;
			}
			case 2:
			{
				return topRightQuad;
			}
			case 3:
			{
				return bottomHalf;
			}
			default:
			{
				Debug.LogError (player.name + "'s playerIndex outside of [1,3]: " + playerIndex);
				return new Rect(0f, 0.5f, 0.5f, 0.5f); //Because I couldn't return null
			}
		}
	}

	public Rect FourPlayerIndexToViewportRect(int playerIndex)
	{
		
		switch (playerIndex)
		{
			case 1:
			{
				return topLeftQuad;
			}
			case 2:
			{
				return topRightQuad;
			}
			case 3:
			{
				return bottomLeftQuad;
			}
			case 4:
			{
				return bottomRightQuad;
			}
			default:
			{
				Debug.LogError (player.name + "'s playerIndex outside of [1,4]: " + playerIndex);
				return new Rect(0f, 0.5f, 0.5f, 0.5f); //Because I couldn't return null
			}
		}
	}
}
