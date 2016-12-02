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
				Debug.LogError("There must be one and only one GameController in the scene");
			}
		}
		return gameController;
	}


	//Variables to be set in the Editor
	public int numLocalPlayers = 1;
	public float roundStartDelay = 5f;
	/// <summary>
	/// The minimum amount of time that will pass after a zombie
	/// spawns that another zombie will spawn at that spawn point.
	/// </summary>
	public float zombieSpawnDelay = 2f;
	public int maxConcurrentZombies = 25;
	public GameObject playerPrefab;
	public GameObject zombiePrefab;
	public GameObject[] playerSpawnPoints; //Don't worry about initializing these, the Editor will handle that
	public GameObject[] zombieSpawnPoints;
	public Weapon defaultWep1;
	public Weapon defaultWep2;

	private bool roundStart = true;

	//Non-editor variables and properties
	private int numNetworkPlayers = 0;
	public int NumPlayers
	{
		get
		{
			return numLocalPlayers + numNetworkPlayers;
		}
	}

	private int round = 0;
	public int Round
	{
		get {	return round;	}
		protected set
		{
			if (value != round)
			{
				round = value;
				StartRound (round);
			}
		}
	}

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

	Dictionary <GameObject, float> zombieSpawnPointTimers;

	List<PlayerRelatedScript> scripts = new List<PlayerRelatedScript> ();
	public void RegisterInitialize (PlayerRelatedScript script)
	{
//		Debug.Log ("Added " + script.GetType ().ToString () + " to scripts to be initialized"); 
		scripts.Add (script);
	}

	public void InitializePlayerScripts ()
	{
		foreach (PlayerRelatedScript script in scripts)
		{
			try
			{
//			Debug.Log ("Initialized " + script.GetType ().ToString ()); 
			script.Initialize ();
			}
			catch (Exception e)
			{
				Debug.LogException (e);
			}
		}
	}


	void Start ()
	{
		localPlayers = new List<LocalPlayer> (numLocalPlayers);
		playerGameObjects = new List<GameObject> (numLocalPlayers);
		for (int i = 0; i < numLocalPlayers; i++)
		{
			GameObject randSpawn = playerSpawnPoints.GetRandomElement<GameObject> ();
			playerGameObjects.Add ((GameObject)Instantiate (playerPrefab, randSpawn.transform.position, randSpawn.transform.rotation));
			localPlayers.Add (playerGameObjects [i].AddComponent<LocalPlayer> ());
			localPlayers [i].prefab = playerPrefab;
			localPlayers [i].playerIndex = i + 1;
			if (i == 0)
			{
				localPlayers [i].cam.gameObject.AddComponent<AudioListener> ();
			}
			localPlayers[i].weaponHandler.loadout = new PlayerLoadout (i + 1, defaultWep1, defaultWep2);
		}

		InitializePlayerScripts ();

		zombies = new List<Zombie>();
		zombieTransforms = new List<Transform>();
		zombieSpawnPointTimers = new Dictionary<GameObject, float> ();
		foreach (GameObject spawn in zombieSpawnPoints)
		{
			zombieSpawnPointTimers.Add (spawn, roundStartDelay);
		}
		StartCoroutine (ZombieSpawner ());
		Round = 1;
	}


	private int zombiesThisRound;
	private int ZombiesThisRound
	{
		get {	return zombiesThisRound;	}
		set
		{
			if (value != zombiesThisRound)
			{
				zombiesLeftThisRound = value;
				zombiesThisRound = value;
			}
		}
	}
	private int zombiesLeftThisRound;
	private int spawnedZombies
	{
		get {	return zombies.Count;	}
	}
	private int zombiesToSpawn;

	void StartRound (int round)
	{
		OnRoundChange (round); //Call the callback
		roundStart = true; // so ZombieSpawner can know
		ZombiesThisRound = 5 * round;
		QueueZombieSpawn (ZombiesThisRound);

	}

	void OnZombieDie (Actor actor)
	{
		Zombie zombie = actor is Zombie ? (Zombie) actor : null;
		if (zombie == null) {	return;		} //Some non-zombie actor died, we don't care
		zombiesLeftThisRound--;
		Zombies.Remove(zombie);
		if(zombiesLeftThisRound <= 0)
		{
			Round++;
			return;
		}
		//TODO: Maybe implement a mechanic where zombies become more hostile as their fellows are killed?
	}

	void QueueZombieSpawn (int numZombies)
	{
		zombiesToSpawn += numZombies;
	}

	void SpawnZombie (Transform trans)
	{
		int index = zombieTransforms.Count;
		zombieTransforms.Add (((GameObject)Instantiate (zombiePrefab, trans.transform.position, trans.transform.rotation)).transform);

		Zombie zombie = zombieTransforms[index].gameObject.AddComponent<Zombie> ();
		zombies.Add (zombie);
		zombie.RegisterDie (OnZombieDie);
		zombie.Health = 50f * Round;
	}



	/// <summary>
	/// Thread responsible for spawning zombies appropriately.
	/// Not a separate class to allow for easy variable access.
	/// </summary>
	IEnumerator ZombieSpawner ()
	{
		while (true)
		{
			if (roundStart)
			{
				yield return new WaitForSeconds (roundStartDelay);
				roundStart = false;
			}
			foreach (GameObject spawn in zombieSpawnPoints)
			{
				zombieSpawnPointTimers[spawn] -= Time.deltaTime; //Change
				if (zombieSpawnPointTimers[spawn] <= 0f && zombiesToSpawn > 0  && spawnedZombies < maxConcurrentZombies)
				{
					SpawnZombie (spawn.transform);
					zombieSpawnPointTimers[spawn] = zombieSpawnDelay;
					zombiesToSpawn--;
					yield return new WaitForSeconds(1f);
				}
			}
			yield return new WaitForEndOfFrame ();
		}
	}


/*	
	=====================================
	|			CALLBACKS				|
	=====================================
*/

	Action cbLocalPlayerCountChange;
	//Action cbNetworkPlayerCountChange;
	Action<int> cbRoundChange;

	void OnLocalPlayerCountChange()
	{
		if (cbLocalPlayerCountChange != null)
		{
			cbLocalPlayerCountChange();
		}
	}

	void OnRoundChange (int newRound)
	{
		if (cbRoundChange != null)
		{
			cbRoundChange (newRound);
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

	/*TODO
	public void RegisterNetworkPlayerCountChange (Action callbackFunc)
	{
		cbNetworkPlayerCountChange += callbackFunc;
	}
	public void UnregisterNetworkPlayerCountChange (Action callbackFunc)
	{
		cbNetworkPlayerCountChange -= callbackFunc;
	}
	*/

	public void RegisterRoundChange (Action<int> callbackFunc)
	{
		cbRoundChange += callbackFunc;
	}
	public void UnregisterRoundChange (Action<int> callbackFunc)
	{
		cbRoundChange -= callbackFunc;
	}
	
}












