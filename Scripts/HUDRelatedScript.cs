using UnityEngine;
using System.Collections;

public class HUDRelatedScript : MonoBehaviour
{
	public HUDHandler handler;

	private bool hasInitialized = false;

	public void Awake()
	{
		handler = HUDHandler.Instance;
		handler.RegisterInitialize (this);
	}

	public virtual void OnInitialize () {}
	public virtual void OnTerminate () {}

	public void Initialize ()
	{
		OnInitialize ();
	}

	public void Terminate ()
	{
		OnTerminate();
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





