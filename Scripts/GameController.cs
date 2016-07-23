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
	public List<Transform> playerTransforms;
	public List<Zombie> zombies;
	public List<Transform> zombieTransforms;


	void Start ()
	{
		players = new List<Player> ();
		players.Capacity = numPlayers;
		//Create the players
		for( int i = 0; i < players.Count; i++)
		{
			playerTransforms[i] = players[i].SpawnAt (spawnPoints.GetRandomElement<GameObject> ().transform).transform;
		}
	}
	
}












