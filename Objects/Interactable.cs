using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(Collider))]
public abstract class Interactable : Prerequisite
{
	[SerializeField]
	protected string interactText = "Interact";

	private bool isInteractable = true;
	public bool IsInteractable
	{
		get { 	return isInteractable;	}
		protected set
		{
			isInteractable = value;
		}
	}

	/// <summary>
	/// Interact with this Interactable. Should call OnInteract in its body and check to determine if the prerequisites are satisfied
	/// </summary>
	/// <returns>
	/// True if successful, false otherwise
	/// </returns>
	/// <param name="candidate">The interacting Player</param>
	public abstract bool Interact (Player candidate);

	//TODO: send out network event when player successfully interacts with an Interactable
	public void OnInteract (Player interactPlayer)
	{
		if (cbInteract != null)
		{
			cbInteract (interactPlayer, this);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// Only a local player because interacting will be broadcasted over the network,
		// meaning that duplicate events could be a problem if we look at all Players
		LocalPlayer candidate;
		if ( (candidate = other.GetComponent<LocalPlayer>()) == null)		{	return;		}
		PlayerInteractHandler handler;
		if ( (handler = candidate.GetComponent<PlayerInteractHandler>()) == null)	{	return;		}
		handler.InteractableEnter (this);
	}

	void OnTriggerExit (Collider other)
	{
		LocalPlayer candidate;
		if ( (candidate = other.GetComponent<LocalPlayer>()) == null)		{	return;		}
		PlayerInteractHandler handler;
		if ( (handler = candidate.GetComponent<PlayerInteractHandler>()) == null)	{	return;		}
		handler.InteractableExit (this);
	}

	public abstract string InteractText (Player candidate);


	/*	
	=====================================
	|			CALLBACKS				|
	=====================================
	*/

	private Action<Player, Interactable> cbInteract;

	public void RegisterInteract (Action<Player, Interactable> callbackFunc)
	{
		cbInteract += callbackFunc;
	}
	public void UnregisterInteract (Action<Player, Interactable> callbackFunc)
	{
		cbInteract -= callbackFunc;
	}
}







