using UnityEngine;
using System.Collections;

public class PlayerRelatedScript : MonoBehaviour
{
	public GameController gc;


	private bool hasInitialized = false;

	public void Awake ()
	{
		gc = GameController.Instance ();
		gc.RegisterInitialize (this);	
	}

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
