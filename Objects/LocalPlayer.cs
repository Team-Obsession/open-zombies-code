using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayer : Player {

	public PlayerInput input;
	public PlayerLook look;
	public PlayerMotor motor;
	public PlayerWeaponHandler weaponHandler;
	public PlayerHUDHandler hudHandler;
	public Camera cam;
	public int playerIndex;

	void OnEnable()
	{
		input = GetComponent<PlayerInput>();
		input.Player = this;
		look = GetComponentInChildren<PlayerLook>();
		look.player = this;
		motor = GetComponent<PlayerMotor>();
		weaponHandler = GetComponent<PlayerWeaponHandler>();
		hudHandler = GetComponent<PlayerHUDHandler>();
		cam = GetComponentInChildren<Camera>();
	}
}
