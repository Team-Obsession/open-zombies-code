using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Prerequisite : MonoBehaviour
{

	private int satisfiedPrerequisites = 0;
	public int SatisfiedPrequisites
	{
		get	{	return satisfiedPrerequisites;	}
		private set
		{
			if (satisfiedPrerequisites == value)	{	return;		}
			if (value > prerequisites.Count)
			{
				Debug.LogError ("Prerequisite counter set above maximum\tvalue:" + value + "\tmax:" + prerequisites.Count);
			}
			else if (value < 0)
			{
				Debug.LogError ("Prerequisite counter set below zero\tvalue:" + value); 
			}
			else
			{
				satisfiedPrerequisites = value;
				SatisfiedChanged ();
			}
		}
	}

	public bool IsSatisfied
	{
		get		{	return satisfiedPrerequisites == prerequisites.Count && SelfSatisfied;	}
	}


	[SerializeField]
	private List<Prerequisite> prerequisites = new List<Prerequisite>();
	[SerializeField]
	private bool selfSatisfied = true;
	public bool  SelfSatisfied
	{
		get 	{	return selfSatisfied;	}
		protected set
		{
			if (selfSatisfied == value) {	return;		}
			selfSatisfied = value;
			SatisfiedChanged ();
		}
	}

	void OnEnable ()
	{
		SatisfiedPrequisites = NumPrerequisitesSatisfied ();
		RegisterPrequisiteCallbacks ();
		if (IsSatisfied)
		{
			SatisfiedChanged ();
		}
	}

	private void OnDisable()
	{
		UnregisterPrerequisiteCallbacks ();
	}

	private int NumPrerequisitesSatisfied ()
	{
		if (prerequisites.Count == 0)	{	return 0;	}
		int result = 0;
		foreach (Prerequisite p in prerequisites)
		{
			result += p.IsSatisfied ? 1 : 0;
		}
		return result;
	}

	private void SatisfiedChanged ()
	{
		if (cbSatisfiedChange == null)	{	return;		}
		cbSatisfiedChange (IsSatisfied);
	}

	private void OnSatisfiedChange (bool satisfied)
	{
		satisfiedPrerequisites += satisfied ? 1 : -1;
	}

	protected void RegisterPrequisiteCallbacks ()
	{
		foreach (Prerequisite p in prerequisites)
		{
			p.RegisterSatisfiedChange (OnSatisfiedChange);
		}
	}

	protected void UnregisterPrerequisiteCallbacks ()
	{
		foreach (Prerequisite p in prerequisites)
		{
			p.UnregisterSatisfiedChange (OnSatisfiedChange);
		}
	}

	private Action<bool> cbSatisfiedChange;

	public void RegisterSatisfiedChange (Action<bool> callbackFunc)
	{
		cbSatisfiedChange += callbackFunc;
	}
	public void UnregisterSatisfiedChange (Action<bool> callbackFunc)
	{
		cbSatisfiedChange -= callbackFunc;
	}

	public void RegisterPrerequisite (Prerequisite requisite)
	{
		prerequisites.Add (requisite);
		requisite.RegisterSatisfiedChange (OnSatisfiedChange);
		SatisfiedPrequisites += requisite.IsSatisfied ? 1 : 0;
	}
	public void UnregisterPrerequisite (Prerequisite requisite)
	{
		prerequisites.Remove (requisite);
		requisite.UnregisterSatisfiedChange (OnSatisfiedChange);
		SatisfiedPrequisites--;
	}




}







