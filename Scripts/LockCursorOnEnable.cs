using UnityEngine;
using System.Collections;

public class LockCursorOnEnable : MonoBehaviour
{

	void OnEnable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void OnDisable()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}



