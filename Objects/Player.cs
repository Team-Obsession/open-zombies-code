using UnityEngine;
using System.Collections;

public class Player : Actor 
{
	public float baseMoveSpeed = 5f; //in m/s
	public float sprintScalar = 1.2f;
	public float baseJumpForce = 4f;
	public float extraJumpTime = 1f; //The amount of time you can affect your jump by holding the button
	public float turnSpeed = 1f; //In rotations per second, where the input's absolute value is 1
	public float airControlScalar = 0.5f;
	public float groundControlDamp = 40f; //Higher values yield more responsive movement
	public float airControlDamp = 10f;

}












