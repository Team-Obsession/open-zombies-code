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
	
	public GameObject[] playerSpawnPoints; //Don't worry about initializing these, the Editor will handle that
	public GameObject[] zombieSpawnPoints;

	public int numPlayers = 1;
	public GameObject playerPrefab;

	private List<Player> players;
	public List<Player> Players {	get{return players;} protected set {players = value;}	}

	private List<GameObject> playerGameObjects;
	public List<GameObject> PlayerGameObjects {		get {return playerGameObjects;} protected set {playerGameObjects = value;}	}

	private List<Zombie> zombies;
	public List<Zombie> Zombies {	get {return zombies;} protected set {zombies = value;}	}

	private List<Transform> zombieTransforms;
	public List<Transform> ZombieTransforms {	get{return zombieTransforms;} protected set {zombieTransforms = value;}	}


	void Awake ()
	{
		players = new List<Player> (numPlayers);
		playerGameObjects = new List<GameObject> (numPlayers);
		for(int i = 0; i < numPlayers; i++)
		{
			GameObject randSpawn = playerSpawnPoints.GetRandomElement<GameObject> ();
			playerGameObjects.Add( (GameObject) Instantiate (playerPrefab, randSpawn.transform.position, randSpawn.transform.rotation) );
			players.Add (	playerGameObjects[i].AddComponent<Player>()		);
			players[i].prefab = playerPrefab;
		}
	}
	
}












