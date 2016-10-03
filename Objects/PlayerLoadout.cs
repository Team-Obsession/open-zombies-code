using UnityEngine;
using System.Collections;

public class PlayerLoadout
{

	public Weapon weapon1;
	public Weapon weapon2;

	private int playerNum = 1;
	public int PlayerNumber
	{
		get	{	return playerNum;	}
		set
		{
			if (value < 1) return;
			//callback?
			playerNum = value;
		}
	}

	public PlayerLoadout (int playerNumber, Weapon weapon1, Weapon weapon2)
	{
		this.PlayerNumber = playerNumber;
		this.weapon1 = weapon1;
		this.weapon2 = weapon2;
	}

}




