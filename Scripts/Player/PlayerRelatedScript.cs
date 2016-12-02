using UnityEngine;
using System.Collections;

public class PlayerRelatedScript : MonoBehaviour
{
	protected GameController gc;

	private bool hasInitialized = false;

	public void Awake ()
	{
		gc = GameController.Instance ();
		gc.RegisterInitialize (this);	
	}

	protected virtual void OnInitialize () {}
	protected virtual void OnTerminate () {}

	public void Initialize ()
	{
		OnInitialize ();
		hasInitialized = true;
	}

	public void Terminate ()
	{
		OnTerminate ();
		hasInitialized = false;
	}

	void OnEnable ()
	{
		if (!hasInitialized)	{	return;		}
		Initialize ();
	}

	void OnDisable ()
	{
		if (!hasInitialized)	{	return;		}
		Terminate ();
	}
	
}
