using UnityEngine;
using System;
using System.Collections;

public class PauseHandler : MonoBehaviour
{
	private static PauseHandler pauseHandler;
	public static PauseHandler Instance
	{
		get
		{
			if (!pauseHandler)
			{
				pauseHandler = FindObjectOfType (typeof(PauseHandler)) as PauseHandler;
				if (!pauseHandler)
				{
					Debug.LogError ("There is not a PauseHandler in the scene");
				}
			}
			return pauseHandler;
		}
	}


	private bool isPaused = false;
	public bool PauseState
	{
		get {	return isPaused;	}
		set
		{
			if (value != isPaused)
			{
				isPaused = value;
				OnPauseStateChange();
			}
		}
	}

	public bool TogglePause ()
	{
		PauseState = !PauseState;
		return isPaused;
	}


	Action<bool> cbPauseStateChange;

	void OnPauseStateChange ()
	{
		if (cbPauseStateChange != null)
		{
			cbPauseStateChange (isPaused);
		}
		Time.timeScale = isPaused ? 0f : 1f;
	}

	public void RegisterPauseStateChange (Action<bool> callbackFunc)
	{
		cbPauseStateChange += callbackFunc;
	}
	public void UnregisterPauseStateChange (Action<bool> callbackFunc)
	{
		cbPauseStateChange -= callbackFunc;
	}

}





