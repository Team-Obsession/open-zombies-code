using UnityEngine;
using System;
using System.Collections;

public interface IShootable
{
	void OnInputShoot (float timeHeld);
}

public enum FireType
{
	Automatic, SemiAuto, BoltAction
}
