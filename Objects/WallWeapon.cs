using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallWeapon : Interactable
{
	
    [SerializeField]
    private Gun gun;

    [SerializeField]
	private int weaponCost; //in points
	[SerializeField]
	private int ammoCost; //in points

	[SerializeField]
	private string ammoText = "ammo text"; //in points

	public override string InteractText (Player candidate)
    {
		PlayerWeaponHandler pWeapHandler;
		if ( (pWeapHandler = candidate.GetComponent<PlayerWeaponHandler>()) == null)
		{
			Debug.LogError (candidate.name + " doesn't have a PlayerWeaponHandler");
		}

		bool hasWeapon = pWeapHandler.GetWeaponInstance (gun) != null;
        return hasWeapon ? ammoText : interactText;
    }

    public override bool Interact (Player candidate)
    {
    	PlayerWeaponHandler pWeapHandler;
		if ( (pWeapHandler = candidate.GetComponent<PlayerWeaponHandler>()) == null)
		{
			Debug.LogError (candidate.name + " doesn't have a PlayerWeaponHandler");
		}

		bool hasWeapon = pWeapHandler.GetWeaponInstance (gun) != null;
    	int cost = hasWeapon ? ammoCost : weaponCost;
        if (candidate.Points < cost || ! PrerequisitesSatisfied()) {  return false;    }
        IsSatisfied = true;
        candidate.Points -= hasWeapon ? ammoCost : weaponCost;

        pWeapHandler.PickupWeapon (gun);

        OnInteract (candidate);
        return true;
    }

}




