using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallWeapon : Interactable
{
	[SerializeField]
	private string ammoText;

    [SerializeField]
    private Gun gun; //TODO: generalize to all weapons (refactor WallWeapon to new inheriting class WallGun?)

    [SerializeField]
	private int weaponCost; //in points
	[SerializeField]
	private int ammoCost; //in points


	/// <summary>
	/// Returns the appropriate text, depending on whether the candidate Player already has this Gun	/// </summary>
	/// <param name="candidate">The player who could potentially interact with this Interactable</param>
	public override string InteractText (Player candidate)
    {
		PlayerWeaponHandler pWeapHandler;
		if ( (pWeapHandler = candidate.GetComponent<PlayerWeaponHandler>()) == null)
		{
			Debug.LogError (candidate.name + " doesn't have a PlayerWeaponHandler");
		}

		bool hasWeapon = pWeapHandler.GetWeaponInstance (gun) != null;
		return hasWeapon ? ammoText + " " + gun.name + " (" + ammoCost + " points)" : interactText + " " + gun.name + " (" + weaponCost + " points)";
    }

    /// <summary>
    /// Make a purchase from this WallWeapon. Will give ammo to the player if they already have this Gun,
    /// and give a new instance of this Gun to the Player otherwise, iff this Interactable's prerequisites
    /// have been satisfied and the interacting Player has sufficient points for the transaction.
    /// </summary>
    /// <returns>True if transaction successful, false otherwise</returns>
    /// <param name="candidate">The interacting Player</param>
    public override bool Interact ( Player candidate, float timeHeld )
	{
		//We don't want multiple interactions via the player holding down the Interact button for multiple frames
		if (timeHeld != 0f) {	return false;	}

		//Get the Player's weaponHandler. If it can't be found, not an error
		PlayerWeaponHandler pWeapHandler;
		if ((pWeapHandler = candidate.GetComponent<PlayerWeaponHandler> ()) == null)
		{
			Debug.LogError (candidate.name + " doesn't have a PlayerWeaponHandler");
		}

		//Determine the cost of the transaction, depending on whether or not the Player has this Gun
		bool hasWeapon = pWeapHandler.GetWeaponInstance (gun) != null;
		int cost = hasWeapon ? ammoCost : weaponCost;

		//Stop execution if the player has insufficient points or if this Interactable hasn't been satisfied
		if (candidate.Points < cost || !IsSatisfied)
		{
			return false;
		}

		//If this point is reached, then the transaction can and should take place.

		//Satisfy this Interactable and take points from the player
		SelfSatisfied = true;
		candidate.Points -= hasWeapon ? ammoCost : weaponCost;

		pWeapHandler.PickupWeapon (gun);

		//Callback and return successful
        OnInteract (candidate);
        return true;
    }

}




