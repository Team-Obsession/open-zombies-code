using UnityEngine;
using System;
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

	public int numPlayers = 2;
	public int numLocalPlayers = 2;
	public GameObject playerPrefab;
	public GameObject zombiePrefab;

	private List<LocalPlayer> localPlayers;
	public List<LocalPlayer> LocalPlayers 
	{
		get {return localPlayers;} 
		protected set
		{
			if(value != localPlayers)
			{
				localPlayers = value;
			}
		}
	}

	private List<GameObject> playerGameObjects;
	public List<GameObject> PlayerGameObjects {		get {return playerGameObjects;} protected set {playerGameObjects = value;}	}

	private List<Zombie> zombies;
	public List<Zombie> Zombies {	get {return zombies;} protected set {zombies = value;}	}

	private List<Transform> zombieTransforms;
	public List<Transform> ZombieTransforms {	get {return zombieTransforms;} protected set {zombieTransforms = value;}	}


	void OnEnable ()
	{
		localPlayers = new List<LocalPlayer> (numPlayers);
		playerGameObjects = new List<GameObject> (numPlayers);
		for (int i = 0; i < numLocalPlayers; i++)
		{
			GameObject randSpawn = playerSpawnPoints.GetRandomElement<GameObject> ();
			playerGameObjects.Add ((GameObject)Instantiate (playerPrefab, randSpawn.transform.position, randSpawn.transform.rotation));
			localPlayers.Add (playerGameObjects [i].AddComponent<LocalPlayer> ());
			localPlayers [i].prefab = playerPrefab;
			localPlayers [i].playerIndex = i + 1;
			localPlayers [i].hudHandler = localPlayers [i].gameObject.AddComponent<PlayerHUDHandler> ();
			if (i == 0)
			{
				localPlayers [i].cam.gameObject.AddComponent<AudioListener> ();
			}
		}
	}

	Action cbLocalPlayerCountChange;
	//Action cbNetworkPlayerCountChange;

	void OnLocalPlayerCountChange()
	{
		if (cbLocalPlayerCountChange != null)
		{
			cbLocalPlayerCountChange();
		}
	}


	public void RegisterLocalPlayerCountChange (Action callbackFunc)
	{
		cbLocalPlayerCountChange += callbackFunc;
	}
	public void UnregisterLocalPlayerCountChange (Action callbackFunc)
	{
		cbLocalPlayerCountChange -= callbackFunc;
	}

	/*
	public void RegisterNetworkPlayerCountChange (Action callbackFunc)
	{
		cbNetworkPlayerCountChange += callbackFunc;
	}
	public void UnregisterNetworkPlayerCountChange (Action callbackFunc)
	{
		cbNetworkPlayerCountChange -= callbackFunc;
	}
	*/
	
}












