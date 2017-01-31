using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Prerequisite : MonoBehaviour
{
	private bool isSatisfied;
	public bool IsSatisfied
	{
		get		{	return isSatisfied;	}
		protected set
		{
			if (value != isSatisfied)
			{
				isSatisfied = value;
				//TODO: callback
			}
		}
	}

	[SerializeField]
	private List<Prerequisite> prerequisites = new List<Prerequisite>();
	public List<Prerequisite> Prerequisites
	{
		get
		{
			return new List<Prerequisite> (prerequisites);
		}
	}

	void Start()
	{
		if (PrerequisitesSatisfied ())
		{
			OnSatisfied ();
		}
	}

	public bool PrerequisitesSatisfied ()
	{
		if (prerequisites.Count == 0)	{	return true;	}
		bool result = false;
		foreach (Prerequisite p in prerequisites)
		{
			result = p.isSatisfied ? true : result;
		}
		return result;
	}

	void OnSatisfied ()
	{
		if (cbSatisfied == null)	{	return;		}
		cbSatisfied();
	}

	private Action cbSatisfied;

	public void RegisterSatisfied (Action callbackFunc)
	{
		cbSatisfied += callbackFunc;
	}
	public void UnregisterSatisfied (Action callbackFunc)
	{
		cbSatisfied -= callbackFunc;
	}




}







