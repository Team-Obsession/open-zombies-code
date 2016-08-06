using UnityEngine;
using System;
using System.Collections;


public class PlayerInput : MonoBehaviour {

	private Player player;
	public Player Player
	{
		get { return player;	}
		protected set
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
				OnPlayerIndexChange ();
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
	float timeHeldJump = 0f;
	float timeHeldStance = 0f;
	float timeHeldSprint = 0f;
	float timeHeldWalk = 0f;
	float timeHeldInteract = 0f;
	float timeHeldSwitch = 0f;
	float timeHeldAim = 0f;
	float timeHeldAimAlt = 0f;
	float timeHeldShoot = 0f;
	float timeHeldShootAlt = 0f;

	void Start()
	{
		Player = GetComponent<Player>();
		if(Player == null)
		{
			Debug.LogError ("No Player component on Player " + PlayerIndex + "'s GameObject");
		}
	}

	void Update()  //The meat and potatoes
	{

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
			OnInputJump (timeHeldJump);
			timeHeldJump += Time.deltaTime;
		}
		else
		{
			timeHeldJump = 0f;
		}


		if(GetButton ("Stance"))
		{
			OnInputStance (timeHeldStance);
			timeHeldStance += Time.deltaTime; 
		}
		else
		{
			timeHeldStance = 0f;
		}

		if(GetButton ("Sprint"))
		{
			OnInputSprint (timeHeldSprint);
			timeHeldWalk = 0f;
			timeHeldSprint += Time.deltaTime;
		}
		else
		{
			OnInputWalk (timeHeldWalk);
			timeHeldSprint = 0f;
			timeHeldWalk += Time.deltaTime;
		}

		if(	GetAxis ("Horizontal") != 0f	||  GetAxis ("Vertical") != 0f )
		{
			moveVector = new Vector2 (	GetAxis ("Horizontal"), GetAxis ("Vertical")	);
			OnInputMove (moveVector);
		}
		else
		{
			moveVector = Vector2.zero;
		}

		if(GetButton ("Interact"))
		{
			OnInputInteract (timeHeldInteract);
			timeHeldInteract += Time.deltaTime; 
		}
		else
		{
			timeHeldInteract = 0f;
		}

		if(GetButton ("Switch"))
		{
			OnInputSwitch (timeHeldSwitch);
			timeHeldSwitch += Time.deltaTime; 
		}
		else
		{
			timeHeldSwitch = 0f;
		}

		if(GetButton ("Aim"))
		{
			OnInputAim (timeHeldAim);
			timeHeldAim += Time.deltaTime; 
		}
		else
		{
			timeHeldAim = 0f;
		}

		if(GetButton ("AimAlt"))
		{
			OnInputAimAlt (timeHeldAimAlt);
			timeHeldAimAlt += Time.deltaTime; 
		}
		else
		{
			timeHeldAimAlt = 0f;
		}

		if(GetButton ("Shoot"))
		{
			OnInputShoot (timeHeldShoot);
			timeHeldShoot += Time.deltaTime; 
		}
		else
		{
			timeHeldShoot = 0f;
		}

		if(GetButton ("ShootAlt"))
		{
			OnInputShootAlt (timeHeldShootAlt);
			timeHeldShootAlt += Time.deltaTime; 
		}
		else
		{
			timeHeldShootAlt = 0f;
		}
	}

	//Callbacks
	Action<int> cbPlayerIndexChange;
	Action<Vector2> cbInputMove;
	Action<Vector2> cbInputLook;
	Action<float> cbInputJump;
	Action<float> cbInputStance;
	Action<float> cbInputSprint;
	Action<float> cbInputWalk;
	Action<float> cbInputInteract;
	Action<float> cbInputSwitch;
	Action<float> cbInputAim;
	Action<float> cbInputAimAlt;
	Action<float> cbInputShoot;
	Action<float> cbInputShootAlt;

	void OnPlayerIndexChange()
	{
		if( cbPlayerIndexChange != null)
		{
			cbPlayerIndexChange (playerIndex);
		}
	}

	void OnInputMove( Vector2 inputVector)
	{
		if( cbInputMove != null)
		{
			cbInputMove ( inputVector);
		}
	}

	void OnInputLook( Vector2 lookVector)
	{
		if(cbInputLook != null)
		{
			cbInputLook	( lookVector);
		}
	}

	void OnInputJump( float timeHeld)
	{
		if(cbInputJump != null)
		{
			cbInputJump( timeHeld);
		}
	}

	void OnInputStance( float timeHeld)
	{
		if(cbInputStance != null)
		{
			cbInputStance( timeHeld);
		}
	}

	void OnInputSprint( float timeHeld)
	{
		if(cbInputSprint != null)
		{
			cbInputSprint( timeHeld);
		}
	}

	void OnInputWalk( float timeHeld)
	{
		if(cbInputWalk != null)
		{
			cbInputWalk( timeHeld);
		}
	}

	void OnInputInteract ( float timeHeld)
	{
		if(cbInputInteract  != null)
		{
			cbInputInteract (timeHeld);
		}
	}

	void OnInputSwitch ( float timeHeld)
	{
		if(cbInputSwitch  != null)
		{
			cbInputSwitch (timeHeld);
		}
	}

	void OnInputAim ( float timeHeld)
	{
		if(cbInputAim != null)
		{
			cbInputAim ( timeHeld);
		}
	}

	void OnInputAimAlt ( float timeHeld)
	{
		if(cbInputAimAlt != null)
		{
			cbInputAimAlt ( timeHeld);
		}
	}

	void OnInputShoot ( float timeHeld)
	{
		if(cbInputShoot  != null)
		{
			cbInputShoot (timeHeld);
		}
	}

	void OnInputShootAlt ( float timeHeld)
	{
		if(cbInputShootAlt  != null)
		{
			cbInputShootAlt (timeHeld);
		}
	}


	public void RegisterPlayerIndexChange(Action<int> callbackFunction)
	{
		cbPlayerIndexChange += callbackFunction;
	}
	public void UnregisterPlayerIndexChange (Action<int> callbackFunction)
	{
		cbPlayerIndexChange -= callbackFunction;
	}

	public void RegisterInputMove (Action<Vector2> callbackFunction)
	{
		cbInputMove += callbackFunction;
	}
	public void UnregisterInputMove (Action<Vector2> callbackFunction)
	{
		cbInputMove -= callbackFunction;
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










