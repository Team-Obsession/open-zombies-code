using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour
{
	public Player player;
	public PlayerInput input;


	void Start()
	{
		player = GetComponentInParent<Player>();
		input = GetComponentInParent<PlayerInput>();
	}

	void OnInputLook(Vector2 lookVector)
	{
		lookVector *= 360f * Time.deltaTime;
		lookVector = lookVector * player.turnSpeed;
		transform.parent.Rotate (0f, lookVector.x, 0f);
		transform.Rotate (-lookVector.y, 0f, 0f);
	}

	void OnEnable()
	{
		input.RegisterInputLook (OnInputLook);
	}

	void OnDisable()
	{
		input.UnregisterInputLook (OnInputLook);
	}

}
