using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{

	private static GameController gameController;
	public static GameController Instance()
	{
		if(!gameController)
		{
			gameController = FindObjectOfType(typeof (GameController)) as GameController;
			if(!gameController)
			{
				Debug.LogError("There must be one GameController in the scene");
			}
		}
		return gameController;
	}
	
	public GameObject[] spawnPoints;

	int numPlayers = 1;
	public List<Player> players;


	void Start ()
	{
		players = new List<Player> ();
		players.Count = numPlayers;
		//Create the players
		foreach (Player player in players) 
		{
			player.SpawnAt(spawnPoints.GetRandomElement<Player>().transform);
		}
	}
	
}












