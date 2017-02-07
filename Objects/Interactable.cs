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
	public abstract bool Interact (Player candidate, float timeHeld);

	/// <summary>
	/// Method which returns the appropriate text to be displayed when a player could interact with this Interactable
	/// </summary>
	/// <param name="candidate">The player who could potentially interact with this Interactable</param>
	public abstract string InteractText (Player candidate);

	//TODO: send out network event when player successfully interacts with an Interactable
	public void OnInteract (Player interactPlayer)
	{
		if (cbInteract == null)	{	return;	}
			cbInteract (interactPlayer, this);
	}

	public void OnInteractableUpdate ()
	{
		if (cbUpdate == null)	{	return;	}
		cbUpdate (this);
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
		


	/*	
	=====================================
	|			CALLBACKS				|
	=====================================
	*/

	private Action<Player, Interactable> cbInteract;
	private Action<Interactable> cbUpdate;

	public void RegisterInteract (Action<Player, Interactable> callbackFunc)
	{
		cbInteract += callbackFunc;
	}
	public void UnregisterInteract (Action<Player, Interactable> callbackFunc)
	{
		cbInteract -= callbackFunc;
	}

	public void RegisterUpdate (Action<Interactable> callbackFunc)
	{
		cbUpdate += callbackFunc;
	}
	public void UnregisterUpdate (Action<Interactable> callbackFunc)
	{
		cbUpdate -= callbackFunc;
	}
}







