using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDRoundUpdate : MonoBehaviour
{
	Text text;

	void OnRoundChange (int round)
	{
		//TODO: Implement a neat effect / animation
		text.text = round;
	}

	void Start()
	{
		//GameController.Instance ().RegisterRoundChange(OnRoundChange);
	}
}



