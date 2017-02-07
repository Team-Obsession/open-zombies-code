using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : PlayerRelatedScript 
{

	float realMoveSpeed;
	float realJumpForce;
	float realAirControlScalar = 1f;
	float realSprintScalar;
	float realControlDamp;

	private Player player;
	private PlayerInput input;
	private Collider myCollider;
	private PlayerWeaponHandler weaponHandler;
	private WeaponInstance weapon;
	private PauseHandler pauseHandler;

	private int notPlayerMask = ~ (1 << 8);
	private Rigidbody rb;
	private Vector3 globalMoveDirection = Vector3.zero;
	private bool grounded;
	private float weaponScalar = 1f;

	void OnInputMove(Vector2 inputVector)
	{
		realControlDamp = grounded ? player.groundControlDamp : player.airControlDamp;
		realAirControlScalar = grounded ? 1f : player.airControlScalar;
		inputVector = inputVector.gradientNormalize2D();
		globalMoveDirection = transform.TransformDirection (	new Vector3( inputVector.x, 0f, inputVector.y)	);
		globalMoveDirection *= player.baseMoveSpeed * realSprintScalar * realAirControlScalar * weaponScalar;
		rb.velocity = new Vector3	(	Mathf.Lerp(	rb.velocity.x, globalMoveDirection.x, realControlDamp * Time.deltaTime),
									 	rb.velocity.y,
									 	Mathf.Lerp( rb.velocity.z, globalMoveDirection.z, realControlDamp * Time.deltaTime)
									);
	}

	void OnInputNotMove (float timeHeld)
	{
		if (grounded && input.TimeHeldNotJump > player.extraJumpTime)
		{
			rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.deltaTime * 50f);
		}
	}

	void OnInputJump ( float timeHeld )
	{

		if(!grounded || input.TimeHeldNotJump <= 0.05f) {	return;		}
		if (timeHeld == 0f)
		{
			rb.AddForce (0f, player.baseJumpForce, 0f, ForceMode.VelocityChange);
		}
		else if (timeHeld <= player.extraJumpTime)
		{
			rb.AddForce (0f, player.baseJumpForce, 0f, ForceMode.Force);
		}

	}

	void OnInputStance (float timeHeld)
	{
		//TODO: Implement crouching and maybe proning
	}

	void OnInputSprint (float timeHeld)
	{
		realSprintScalar = Mathf.Lerp (1, player.sprintScalar, Mathf.Clamp (timeHeld * 5f, 0, 1));
	}

	void OnInputWalk (float timeHeld)
	{
		realSprintScalar = 1f;
	}

	void OnInputSwitch (float timeHeld)
	{
		//This is just for potentially limiting the player's movement speed when they're switching weapons
	}

	void OnPauseStateChange (bool newState)
	{
		if (newState)
		{
			UnregisterCallbacks ();
		}
		else
		{
			RegisterCallbacks ();
		}
	}

	void OnWeaponChange (WeaponInstance currentWeapon)
	{
		weapon = currentWeapon;
		weaponScalar = weapon.weapon.movementScalar;
	}

	bool GroundedRayCast()
	{
		return Physics.Raycast (transform.position, -Vector3.up, 0.01f);
	}

	bool GroundedCapsuleCast()
	{
		return Physics.CheckCapsule(	myCollider.bounds.center,
										new Vector3	(myCollider.bounds.center.x, myCollider.bounds.min.y - 0.01f, myCollider.bounds.center.z),
										((CapsuleCollider) myCollider).radius,
										notPlayerMask
									);
	}

	void FixedUpdate()
	{
		grounded = GroundedCapsuleCast ();
	}

	protected override void OnInitialize() //initialization
	{
		if(player == null && ((player = GetComponent<LocalPlayer>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a player.");
		}

		if(weaponHandler == null && ((weaponHandler = GetComponent<PlayerWeaponHandler>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a PlayerWeaponHandler");
		}

		if(input == null && ((input = player.GetComponent<PlayerInput>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a PlayerInput from its player");
		}

		if(rb == null && ((rb = GetComponent<Rigidbody>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a Rigidbody");
		}

		if(myCollider == null && ((myCollider = GetComponent<Collider>()) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a Collider");
		}

		if(pauseHandler == null && ((pauseHandler = PauseHandler.Instance) == null))
		{
			Debug.LogError (gameObject.name + " couldn't find a PauseHandler");
		}

		RegisterCallbacks ();
		pauseHandler.RegisterPauseStateChange (OnPauseStateChange);
	}


	protected override void OnTerminate ()
	{
		UnregisterCallbacks ();
		pauseHandler.UnregisterPauseStateChange (OnPauseStateChange);
	}

	void RegisterCallbacks()
	{
		input.RegisterInputMove (OnInputMove);
		input.RegisterInputNotMove (OnInputNotMove);
		input.RegisterInputJump (OnInputJump);
		input.RegisterInputStance (OnInputStance);
		input.RegisterInputSprint (OnInputSprint);
		input.RegisterInputWalk (OnInputWalk);
		input.RegisterInputSwitch (OnInputSwitch);
		weaponHandler.RegisterWeaponChange (OnWeaponChange);
	}

	void UnregisterCallbacks()
	{
		input.UnregisterInputMove (OnInputMove);
		input.UnregisterInputNotMove (OnInputNotMove);
		input.UnregisterInputJump (OnInputJump);
		input.UnregisterInputStance (OnInputStance);
		input.UnregisterInputSprint (OnInputSprint);
		input.UnregisterInputWalk (OnInputWalk);
		input.UnregisterInputSwitch (OnInputSwitch);
		weaponHandler.UnregisterWeaponChange (OnWeaponChange);
	}


}











