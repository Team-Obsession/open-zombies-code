using UnityEngine;
using System.Collections;

public class LockCursorOnEnable : MonoBehaviour
{
	PlayerInput playerInput;

	void OnEnable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		if (playerInput == null)
		{
			playerInput = GetComponentInParent<HUD>().GetPlayer().input;
		}
		playerInput.LockState = true;
	}

	void OnDisable()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (playerInput == null)
		{
			playerInput = GetComponentInParent<HUD>().GetPlayer().input;
		}
		playerInput.LockState = false;

	}
}



