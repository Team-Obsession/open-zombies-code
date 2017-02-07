using UnityEngine;
using System.Collections;

public class WeaponInstance : MonoBehaviour
{
	[HideInInspector]
	public LocalPlayer player;
	[HideInInspector]
	public PlayerWeaponHandler weapHandler;
	[HideInInspector]
	public PlayerInput input;
	[HideInInspector]
	public bool hasInitialized = false;

	public Weapon weapon;

	public virtual void OnInitialize () {}
	public virtual void OnTerminate () {}

	public void Initialize () 
	{
		OnInitialize ();
		hasInitialized = true;
	}

	public void Terminate () 
	{
		OnTerminate ();
	}

	void OnEnable ()
	{
		if (hasInitialized)
		{
			Initialize ();
		}
	}

	void OnDisable ()
	{
		if (hasInitialized)
		{
			Terminate ();
		}
	}

}
