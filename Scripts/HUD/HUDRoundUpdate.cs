using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDRoundUpdate : MonoBehaviour
{
	Text thisText;

	void OnRoundChange (int round)
	{
		//TODO: Implement a neat effect / animation
		thisText.text = round.ToString ();
	}

	void OnEnable()
	{
		thisText = GetComponent<Text>();
		GameController.Instance ().RegisterRoundChange(OnRoundChange);
		OnRoundChange (GameController.Instance ().Round);
	}
}



