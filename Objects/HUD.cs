using UnityEngine;
using System.Collections;

/// <summary>
/// To be on the root panel for each of the player's UI. Will track which menus are available
/// and hold useful information for HUD stuff
/// </summary>
public class HUD : MonoBehaviour
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

	public GameObject[] menus;
	public GameObject defaultMenu;


	private GameObject activeMenu;
	public GameObject ActiveMenu
	{
		get {	return activeMenu;	}
		set
		{
			if (value != activeMenu)
			{
				activeMenu = value;
				//TODO: callbacks
			}
		}
	}
		
	LocalPlayer player;
	public LocalPlayer GetPlayer()	{	return player;	}
	public void SetPlayer (LocalPlayer player)	{	this.player = player;	}

	/*TODO: Redo to allow for multi-monitor edge cases (i.e. split vertically for two monitors and two players instead of horizontally)
				Maybe this could be a neat GUI selector
	*/
	public static Rect TwoPlayerIndexToViewportRect (int playerIndex)
	{
		
		switch (playerIndex)
		{
			case 1:
			{
				return topHalf;
			}
			case 2:
			{
				return bottomHalf;
			}
			default:
			{
				Debug.LogError ("playerIndex outside of [1,2]: " + playerIndex);
				return leftHalf;
			}
		}
	}

	public static Rect ThreePlayerIndexToViewportRect(int playerIndex)
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
				Debug.LogError ("playerIndex outside of [1,3]: " + playerIndex);
				return leftHalf; //Because I couldn't return null
			}
		}
	}

	public static Rect FourPlayerIndexToViewportRect(int playerIndex)
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
				Debug.LogError ("playerIndex outside of [1,4]: " + playerIndex);
				return leftHalf;; //Because I couldn't return null
			}
		}
	}
}
