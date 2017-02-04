using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractHandler : PlayerRelatedScript
{

	private LocalPlayer player;
	private PlayerInput input;
	private List<Interactable> interactCandidates = new List<Interactable>();

	protected override void OnInitialize ()
	{

		if ( (player = GetComponent<LocalPlayer>() ) == null)
		{
			Debug.LogError (this.name + " doesn't have a LocalPlayer attached");
		}
		if ( (input = GetComponent<PlayerInput>() ) == null)
		{
			Debug.LogError (this.name + " doesn't have a PlayerInput attached");
		}
		input.RegisterInputInteract (OnInputInteract);
	}

	protected override void OnTerminate ()
	{
		input.UnregisterInputInteract (OnInputInteract);
	}

	public void InteractableEnter (Interactable obj)
	{
		interactCandidates.Insert (0, obj);
		OnInteractableEnter (obj);
	}

	public void InteractableExit (Interactable obj)
	{
		interactCandidates.Remove (obj);
		OnInteractableExit (obj);
	}

	void OnInputInteract (float timeHeld)
	{
		if (interactCandidates.Count > 0)
		{
			interactCandidates[0].Interact (player, timeHeld);
			OnInteract (interactCandidates [0]);
		}
	}

	void OnInteractableEnter (Interactable obj)
	{
		if (cbInteractableEnter == null) {	return;		}
		cbInteractableEnter (obj);
	}

	void OnInteractableExit (Interactable obj)
	{
		if (cbInteractableExit == null) {	return;		}
		cbInteractableExit (obj);
	}

	void OnInteract (Interactable obj)
	{
		if (cbInteract == null) {	return;		}
		cbInteract (obj);
	}

	/*	
	=====================================
	|			CALLBACKS				|
	=====================================
	*/

	private Action<Interactable> cbInteractableEnter;
	private Action<Interactable> cbInteractableExit;
	private Action<Interactable> cbInteract;

	public void RegisterInteractableEnter (Action<Interactable> callbackFunc)
	{
		cbInteractableEnter += callbackFunc;
	}
	public void UnregisterInteractableEnter (Action<Interactable> callbackFunc)
	{
		cbInteractableEnter -= callbackFunc;
	}

	public void RegisterInteractableExit (Action<Interactable> callbackFunc)
	{
		cbInteractableExit += callbackFunc;
	}
	public void UnregisterInteractableExit (Action<Interactable> callbackFunc)
	{
		cbInteractableExit -= callbackFunc;
	}

	public void RegisterInteract (Action<Interactable> callbackFunc)
	{
		cbInteract += callbackFunc;
	}
	public void UnregisterInteract (Action<Interactable> callbackFunc)
	{
		cbInteract -= callbackFunc;
	}

}







