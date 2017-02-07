using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour
{
	public Player player;
	public PlayerInput input;
	public bool clampXRotation = true;

	Vector3 eulerRot;

	private float xRotation;
	private Quaternion newRot;
	private PauseHandler pauseHandler;

	void Start()
	{
		player = GetComponentInParent<Player>();
		input = GetComponentInParent<PlayerInput>();
		input.RegisterInputLook (OnInputLook);
		pauseHandler = PauseHandler.Instance;
		pauseHandler.RegisterPauseStateChange (OnPauseStateChange);
	}

	void OnInputLook(Vector2 lookVector)
	{
		lookVector *= 360f * player.turnSpeed * Time.deltaTime;
		transform.parent.Rotate (0f, lookVector.x, 0f);
		xRotation -= lookVector.y;
		xRotation = Mathf.Clamp(xRotation, -85f, 85f);
		newRot = Quaternion.Euler (xRotation, 0f, 0f);
		transform.localRotation = newRot;
	}

	void OnPauseStateChange (bool newState)
	{
		if(newState)
		{
			input.UnregisterInputLook (OnInputLook);
		}
		else
		{
			input.RegisterInputLook (OnInputLook);
		}
	}

	void OnEnable()
	{
		
	}

	void OnDisable()
	{
		input.UnregisterInputLook (OnInputLook);
		pauseHandler.UnregisterPauseStateChange (OnPauseStateChange);
	}
}
