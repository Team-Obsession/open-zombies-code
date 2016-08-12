using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour 
{
	private Player player;
	private PlayerInput input;

	float realMoveSpeed;
	float realJumpForce;
	float realSprintScalar;

	private Rigidbody rb;
	private Vector3 globalMoveDirection = Vector3.zero;
	private bool grounded;



	void OnInputMove(Vector2 inputVector)
	{
		inputVector = inputVector.gradientNormalize2D();
		globalMoveDirection = transform.TransformDirection (	new Vector3( inputVector.x, 0f, inputVector.y)	);
		globalMoveDirection *= player.baseMoveSpeed * realSprintScalar;
		rb.velocity = new Vector3	(	Mathf.Lerp(	rb.velocity.x, globalMoveDirection.x, player.groundControlDamp * Time.deltaTime),
									 	rb.velocity.y,
									 	Mathf.Lerp( rb.velocity.z, globalMoveDirection.z, player.groundControlDamp * Time.deltaTime)
									);
	}

	void OnInputJump ( float timeHeld ) //TODO: Implement grounded
	{
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
		
	}

	void OnInputSprint (float timeHeld)
	{
		realSprintScalar = player.sprintScalar;
	}

	void OnInputWalk (float timeHeld)
	{
		realSprintScalar = 1f;
	}

	void OnInputSwitch (float timeHeld)
	{
		
	}

	void Start() //initialization
	{
		player = GetComponent<Player>();
		if(player == null)
		{
			Debug.LogError ("No Player on this GameObject");
		}

		if(input != null)
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
	}


	void OnEnable() //To register all of the callbacks
	{
		input.RegisterInputMove (OnInputMove);
		input.RegisterInputJump (OnInputJump);
		input.RegisterInputStance (OnInputStance);
		input.RegisterInputSprint (OnInputSprint);
		input.RegisterInputWalk (OnInputWalk);
		input.RegisterInputSwitch (OnInputSwitch);
	}

	void OnDisable() //To unregister all of the callbacks
	{
		input.UnregisterInputMove (OnInputMove);
		input.UnregisterInputJump (OnInputJump);
		input.UnregisterInputStance (OnInputStance);
		input.UnregisterInputSprint (OnInputSprint);
		input.UnregisterInputWalk (OnInputWalk);
		input.UnregisterInputSwitch (OnInputSwitch);
	}

}












