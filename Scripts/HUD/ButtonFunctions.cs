using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonFunctions : MonoBehaviour
{

	public bool quit;

	Button button;

	void Start()
	{
		button = GetComponent<Button>();
		if (quit)
		{
			button.onClick.AddListener (Quit);
		}
	}

	public void Quit()
	{
		Application.Quit ();
	}
}



