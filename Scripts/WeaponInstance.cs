using UnityEngine;
using System.Collections;

public class WeaponInstance : MonoBehaviour
{
	public LocalPlayer player;
	public PlayerWeaponHandler weapHandler;
	public PlayerInput input;
	public Weapon weapon;
	public bool hasInitialized = false;

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
