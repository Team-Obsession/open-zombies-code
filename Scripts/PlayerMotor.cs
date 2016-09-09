using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour 
{

	float realMoveSpeed;
	float realJumpForce;
	float realAirControlScalar = 1f;
	float realSprintScalar;
	float realControlDamp;

	private Player player;
	private PlayerInput input;
	private Collider myCollider;

	private int notPlayerMask = ~ (1 << 8);
	private Rigidbody rb;
	private Vector3 globalMoveDirection = Vector3.zero;
	private bool grounded;
	private bool paused = false;


	bool CanJump
	{
		get
		{
			return grounded;
		}
	}

	void OnInputMove(Vector2 inputVector)
	{
		realControlDamp = CanJump ? player.groundControlDamp : player.airControlDamp;
		realAirControlScalar = CanJump ? 1f : player.airControlScalar;
		inputVector = inputVector.gradientNormalize2D();
		globalMoveDirection = transform.TransformDirection (	new Vector3( inputVector.x, 0f, inputVector.y)	);
		globalMoveDirection *= player.baseMoveSpeed * realSprintScalar * realAirControlScalar;
		rb.velocity = new Vector3	(	Mathf.Lerp(	rb.velocity.x, globalMoveDirection.x, realControlDamp * Time.deltaTime),
									 	rb.velocity.y,
									 	Mathf.Lerp( rb.velocity.z, globalMoveDirection.z, realControlDamp * Time.deltaTime)
									);
	}

	void OnInputJump ( float timeHeld )
	{

		if(!CanJump || input.TimeHeldNotJump <= 0.05f) {	return;		}
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
		realSprintScalar = Mathf.Lerp (1, player.sprintScalar, Mathf.Clamp(timeHeld * 3f, 0, 1));
	}

	void OnInputWalk (float timeHeld)
	{
		realSprintScalar = 1f;
	}

	void OnInputSwitch (float timeHeld)
	{
		//This is just for potentially limiting the player's movement speed when they're switching weapons
	}

	void OnInputPause ()
	{
		paused = !paused;
		if (paused)
		{
			UnregisterInputs ();
		}
		else
		{
			RegisterInputs ();
		}
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

	void Start() //initialization
	{
		player = GetComponent<Player>();
		if(player == null)
		{
			Debug.LogError ("No Player on this GameObject");
		}

		if(input == null)
		{
			input = GetComponent<PlayerInput>();
			if (input == null)
			{
				Debug.LogError ("No PlayerInput on this GameObject");
			}
		}

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError ("No Rigidbody on this GameObject");
		}

		myCollider = GetComponent<Collider>();
	}

	void OnEnable() //To register all of the callbacks
	{
		if(input == null)
		{
			input = GetComponent<PlayerInput>();
			if (input == null)
			{
				Debug.LogError ("No PlayerInput on this GameObject");
			}
		}
		input.RegisterInputPause (OnInputPause);
		RegisterInputs ();
	}

	void OnDisable() //To unregister all of the callbacks
	{
		input.UnregisterInputPause (OnInputPause);
		UnregisterInputs ();
	}

	void RegisterInputs()
	{
		input.RegisterInputMove (OnInputMove);
		input.RegisterInputJump (OnInputJump);
		input.RegisterInputStance (OnInputStance);
		input.RegisterInputSprint (OnInputSprint);
		input.RegisterInputWalk (OnInputWalk);
		input.RegisterInputSwitch (OnInputSwitch);
	}

	void UnregisterInputs()
	{
		input.UnregisterInputMove (OnInputMove);
		input.UnregisterInputJump (OnInputJump);
		input.UnregisterInputStance (OnInputStance);
		input.UnregisterInputSprint (OnInputSprint);
		input.UnregisterInputWalk (OnInputWalk);
		input.UnregisterInputSwitch (OnInputSwitch);
	}


}











