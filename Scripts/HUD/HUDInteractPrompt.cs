using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HUDInteractPrompt : HUDRelatedScript
{
	private LocalPlayer player;
	private PlayerInteractHandler pHandler;
	private List<Interactable> interactCandidates = new List<Interactable>();

	private Text text;

	public void OnInteractableEnter (Interactable candidate)
	{
		interactCandidates.Insert (0, candidate);
		UpdateText();
	}

	public void OnInteractableExit (Interactable candidate)
	{
		interactCandidates.Remove (candidate);
		UpdateText();
	}

	public void OnInteract (Interactable obj)
	{
		UpdateText ();
	}

	void UpdateText()
	{
		if (interactCandidates.Count == 0)
		{
			text.enabled = false;
			return;
		}
		text.enabled = true;
		text.text = interactCandidates[0].InteractText (player);
	}

	public override void OnInitialize ()
	{
		player = GetComponentInParent<HUD>().Player;
		if ( (pHandler = player.GetComponent<PlayerInteractHandler>() ) == null)
		{
			Debug.LogError (player.name + " doesn't have a PlayerInteractHandler attached");
			this.enabled = false;
			return;
		}
		if ( (text = GetComponent<Text>() ) == null)
		{
			Debug.LogError (name + " doesn't have a Text attached");
		}
		RegisterCallbacks ();
		UpdateText ();
	}

	public override void OnTerminate ()
	{
		UnregisterCallbacks ();
	}

	private void RegisterCallbacks ()
	{
		pHandler.RegisterInteractableEnter (OnInteractableEnter);
		pHandler.RegisterInteractableExit (OnInteractableExit);
		pHandler.RegisterInteract (OnInteract);
	}

	private void UnregisterCallbacks ()
	{
		pHandler.UnregisterInteractableEnter (OnInteractableEnter);
		pHandler.UnregisterInteractableExit (OnInteractableExit);
		pHandler.UnregisterInteract (OnInteract);
	}
}







