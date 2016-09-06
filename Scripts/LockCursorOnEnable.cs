using UnityEngine;
using System.Collections;

public class LockCursorOnEnable : MonoBehaviour
{
	PlayerInput playerInput;

	void LateOnEnable()
	{
		playerInput = GetComponentInParent<HUD>().GetPlayer ().input;
		playerInput.LockState = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void LateOnDisable()
	{
		playerInput.LockState = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}



