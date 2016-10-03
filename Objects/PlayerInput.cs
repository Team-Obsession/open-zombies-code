using UnityEngine;
using System;
using System.Collections;


public class PlayerInput : MonoBehaviour
{
	public PauseHandler pauseHandler;
	
	private LocalPlayer player;
	public LocalPlayer Player
	{
		get { return player;	}
		set
		{
			player = value;
		}
	}

	private int playerIndex = 1; //1 is player one, 2 is player 2, etc.
	public int PlayerIndex
	{
		get {	return playerIndex;		}
		protected set
		{
			if(value != playerIndex)
			{
				playerIndex = value;
			}
		}
	}

	public float GetAxis (string axisName)
	{
		return Input.GetAxis (axisName + "_" + playerIndex);
	}

	public float GetAxisRaw (string axisName)
	{
		return Input.GetAxisRaw(axisName + "_" + playerIndex);
	}

	public bool GetButton (string axisName)
	{
		return Input.GetButton (axisName + "_" + playerIndex);
	}

	public bool GetButtonDown (string axisName)
	{
		return Input.GetButtonDown (axisName + "_" + playerIndex);
	}

	public bool GetButtonUp (string axisName)
	{
		return Input.GetButtonUp (axisName + "_" + playerIndex);
	}

	public Vector3 MousePosition
	{
		get {	return Input.mousePosition;	 }
	}

	Vector2 moveVector = Vector2.zero;
	Vector2 lookVector = Vector2.zero;

	float timeHeldNotMove = 0f;
	public float TimeHeldNotMove { get {return timeHeldNotMove; }}

	float timeHeldJump = 0f;
	public float TimeHeldJump { get { return timeHeldJump; }}

	float timeHeldNotJump = 0f;
	public float TimeHeldNotJump { get { return timeHeldNotJump; }}

	float timeHeldStance = 0f;
	public float TimeHeldStance { get { return timeHeldStance; }}

	float timeHeldSprint = 0f;
	public float TimeHeldSprint { get { return timeHeldSprint; }}

	float timeHeldWalk = 0f;
	public float TimeHeldWalk { get { return timeHeldWalk; }}

	float timeHeldInteract = 0f;
	public float TimeHeldInteract { get { return timeHeldInteract; }}

	float timeHeldReload = 0f;
	public float TimeHeldReload { get { return timeHeldReload; }}

	float timeHeldSwitch = 0f;
	public float TimeHeldSwitch { get { return timeHeldSwitch; }}

	float timeHeldAim = 0f;
	public float TimeHeldAim { get {return timeHeldAim; }}

	float timeHeldNotAim = 0f;
	public float TimeHeldNotAim { get {return timeHeldNotAim; }}

	float timeHeldAimAlt = 0f;
	public float TimeHeldAimAlt { get { return timeHeldAimAlt; }}

	float timeHeldShoot = 0f;
	public float TimeHeldShoot { get { return timeHeldShoot; }}

	float timeHeldShootAlt = 0f;
	public float TimeHeldShootAlt { get { return timeHeldShootAlt; }}


	void Start()
	{

		if(Player == null && ((Player = GetComponent<LocalPlayer>()) == null))
		{
			Debug.LogError ("No Player component on Player " + PlayerIndex + "'s GameObject");
		}
		PlayerIndex = player.playerIndex;

		pauseHandler = PauseHandler.Instance;
	}

	void Update()  //The meat and potatoes
	{
		if (GetButtonDown ("Pause"))
		{
			pauseHandler.TogglePause ();
		}

		if(	GetAxis ("Horizontal") != 0f	||  GetAxis ("Vertical") != 0f )
		{
			moveVector = new Vector2 (	GetAxisRaw ("Horizontal"), GetAxisRaw ("Vertical")	);
			OnInputMove (moveVector);
			timeHeldNotMove = 0f;
		}
		else
		{
			moveVector = Vector2.zero;
			timeHeldNotMove += Time.deltaTime;
			CallbackTimeHeld (cbInputNotMove, timeHeldNotMove);
		}

		if (GetAxis ("Mouse X") != 0f || GetAxis ("Mouse Y") != 0f)
		{
			lookVector = new Vector2 (	GetAxis ("Mouse X"), GetAxis ("Mouse Y")	);
			OnInputLook (lookVector);
		}
		else
		{
			lookVector = Vector2.zero;
		}

		if(	GetButton ("Jump"))
		{
			CallbackTimeHeld (cbInputJump, timeHeldJump);
			timeHeldJump += Time.deltaTime;
			timeHeldNotJump = 0f;
		}
		else
		{
			CallbackTimeHeld (cbInputNotJump, timeHeldNotJump);
			timeHeldJump = 0f;
			timeHeldNotJump += Time.deltaTime;
		}


		if(GetButton ("Stance"))
		{
			CallbackTimeHeld (cbInputStance, timeHeldStance);
			timeHeldStance += Time.deltaTime; 
		}
		else
		{
			timeHeldStance = 0f;
		}

		if(GetButton ("Sprint"))
		{
			CallbackTimeHeld (cbInputSprint, timeHeldSprint);
			timeHeldWalk = 0f;
			timeHeldSprint += Time.deltaTime;
		}
		else
		{
			CallbackTimeHeld (cbInputWalk, timeHeldWalk);
			timeHeldSprint = 0f;
			timeHeldWalk += Time.deltaTime;
		}


		if(GetButton ("Interact"))
		{
			CallbackTimeHeld (cbInputInteract, timeHeldInteract);
			timeHeldInteract += Time.deltaTime; 
		}
		else
		{
			timeHeldInteract = 0f;
		}

		if (GetButton ("Reload"))
		{
			CallbackTimeHeld (cbInputReload, timeHeldReload);
			timeHeldReload += Time.deltaTime; 
		}
		else
		{
			timeHeldReload = 0f;
		}

		if(GetButton ("Switch"))
		{
			CallbackTimeHeld (cbInputSwitch, timeHeldSwitch);
			timeHeldSwitch += Time.deltaTime; 
		}
		else
		{
			timeHeldSwitch = 0f;
		}

		if(GetButton ("Aim"))
		{
			CallbackTimeHeld (cbInputAim, timeHeldAim);
			timeHeldNotAim = 0f;
			timeHeldAim += Time.deltaTime;
		}
		else
		{
			CallbackTimeHeld (cbInputNotAim, timeHeldNotAim);
			timeHeldAim = 0f;
			timeHeldNotAim += Time.deltaTime;
		}

		if(GetButton ("AimAlt"))
		{
			CallbackTimeHeld (cbInputAimAlt, timeHeldAimAlt);
			timeHeldAimAlt += Time.deltaTime; 
		}
		else
		{
			timeHeldAimAlt = 0f;
		}

		if(GetButton ("Shoot"))
		{
			CallbackTimeHeld (cbInputShoot, timeHeldShoot);
			timeHeldShoot += Time.deltaTime; 
		}
		else
		{
			timeHeldShoot = 0f;
		}

		if(GetButton ("ShootAlt"))
		{
			CallbackTimeHeld (cbInputShootAlt, timeHeldShootAlt);
			timeHeldShootAlt += Time.deltaTime; 
		}
		else
		{
			timeHeldShootAlt = 0f;
		}
	}

	//Callbacks
	Action<bool> cbInputLocked;
	Action<Vector2> cbInputMove;
	Action<float> cbInputNotMove;
	Action<Vector2> cbInputLook;
	Action<float> cbInputJump;
	Action<float> cbInputNotJump;
	Action<float> cbInputStance;
	Action<float> cbInputSprint;
	Action<float> cbInputWalk;
	Action<float> cbInputInteract;
	Action<float> cbInputReload;
	Action<float> cbInputSwitch;
	Action<float> cbInputAim;
	Action<float> cbInputNotAim;
	Action<float> cbInputAimAlt;
	Action<float> cbInputShoot;
	Action<float> cbInputShootAlt;


	void OnInputMove (Vector2 inputVector)
	{
		if( cbInputMove != null)
		{
			cbInputMove ( inputVector);
		}
	}

	void OnInputLook (Vector2 lookVector)
	{
		if(cbInputLook != null)
		{
			cbInputLook	( lookVector);
		}
	}

	void CallbackTimeHeld (Action<float> callback, float timeHeld)
	{
		if(callback != null)
		{
			callback(timeHeld);
		}
	}


	public void RegisterInputLocked (Action<bool> callbackFunc)
	{
		cbInputLocked += callbackFunc;
	}
	public void UnregisterInputLocked (Action<bool> callbackFunc)
	{
		cbInputLocked -= callbackFunc;
	}

	public void RegisterInputMove (Action<Vector2> callbackFunction)
	{
		cbInputMove += callbackFunction;
	}
	public void UnregisterInputMove (Action<Vector2> callbackFunction)
	{
		cbInputMove -= callbackFunction;
	}

	public void RegisterInputNotMove (Action<float> callbackFunc)
	{
		cbInputNotMove += callbackFunc;
	}
	public void UnregisterInputNotMove (Action<float> callbackFunc)
	{
		cbInputNotMove -= callbackFunc;
	}

	public void RegisterInputLook (Action<Vector2> callbackFunction)
	{
		cbInputLook += callbackFunction;
	}
	public void UnregisterInputLook (Action<Vector2> callbackFunction)
	{
		cbInputLook -= callbackFunction;
	}

	public void RegisterInputJump (Action<float> callbackFuntion)
	{
		cbInputJump += callbackFuntion;
	}
	public void UnregisterInputJump (Action<float> callbackFunction)
	{
		cbInputJump -= callbackFunction;
	}

	public void RegisterInputNotJump (Action<float> callbackFunction)
	{
		cbInputNotJump += callbackFunction;
	}
	public void UnregisterInputNotJump (Action<float> callbackFunction)
	{
		cbInputNotJump -= callbackFunction;
	}

	public void RegisterInputStance (Action<float> callbackFuntion)
	{
		cbInputStance += callbackFuntion;
	}
	public void UnregisterInputStance (Action<float> callbackFunction)
	{
		cbInputStance -= callbackFunction;
	}

	public void RegisterInputSprint (Action<float> callbackFunction)
	{
		cbInputSprint += callbackFunction;
	}
	public void UnregisterInputSprint (Action<float> callbackFunction)
	{
		cbInputSprint -= callbackFunction;
	}

	public void RegisterInputWalk (Action<float> callbackFunction)
	{
		cbInputWalk += callbackFunction;
	}
	public void UnregisterInputWalk (Action<float> callbackFunction)
	{
		cbInputWalk -= callbackFunction;
	}

	public void RegisterInputInteract (Action<float> callbackFuntion)
	{
		cbInputInteract += callbackFuntion;
	}
	public void UnregisterInputInteract (Action<float> callbackFunction)
	{
		cbInputInteract -= callbackFunction;
	}

	public void RegisterInputReload (Action<float> callbackFunc)
	{
		cbInputReload += callbackFunc;
	}
	public void UnregisterInputReload (Action<float> callbackFunc)
	{
		cbInputReload -= callbackFunc;
	}

	public void RegisterInputSwitch (Action<float> callbackFuntion)
	{
		cbInputSwitch += callbackFuntion;
	}
	public void UnregisterInputSwitch (Action<float> callbackFunction)
	{
		cbInputSwitch -= callbackFunction;
	}

	public void RegisterInputAim (Action<float> callbackFuntion)
	{
		cbInputAim += callbackFuntion;
	}
	public void UnregisterInputAim (Action<float> callbackFunction)
	{
		cbInputAim -= callbackFunction;
	}

	public void RegisterInputNotAim (Action<float> callbackFunc)
	{
		cbInputNotAim += callbackFunc;
	}
	public void UnregisterInputNotAim (Action<float> callbackFunc)
	{
		cbInputNotAim -= callbackFunc;
	}

	public void RegisterInputAimAlt (Action<float> callbackFuntion)
	{
		cbInputAimAlt += callbackFuntion;
	}
	public void UnregisterInputAimAlt (Action<float> callbackFunction)
	{
		cbInputAimAlt -= callbackFunction;
	}

	public void RegisterInputShoot (Action<float> callbackFuntion)
	{
		cbInputShoot += callbackFuntion;
	}
	public void UnregisterInputShoot (Action<float> callbackFunction)
	{
		cbInputShoot -= callbackFunction;
	}

	public void RegisterInputShootAlt (Action<float> callbackFuntion)
	{
		cbInputShootAlt += callbackFuntion;
	}
	public void UnregisterInputShootAlt (Action<float> callbackFunction)
	{
		cbInputShootAlt -= callbackFunction;
	}
}










