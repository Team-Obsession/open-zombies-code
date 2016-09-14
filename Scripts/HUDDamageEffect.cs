using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDDamageEffect : MonoBehaviour
{

	public Color flashColor;
	public float fadeTime = 0.25f;

	LocalPlayer player;
	Image flash;

	public void OnPlayerHealthChange ()
	{
		StopCoroutine (FadeToClear (fadeTime));
		flash.color = flashColor;
		StartCoroutine (FadeToClear (fadeTime));
	}

	IEnumerator FadeToClear (float time)
	{
		float startTime = Time.time;
		while (Time.time - startTime < time)
		{
			flash.color = new Color(flash.color.r, flash.color.g, flash.color.b, Mathf.Lerp (flashColor.a, 0, (Time.time - startTime) / time));
			yield return new WaitForEndOfFrame ();
		}
		flash.color = Color.clear;
	}

	void Start()
	{
		player = GetComponentInParent<HUD>().GetPlayer ();
		flash = GetComponent<Image>();
		player.RegisterHealthChange (OnPlayerHealthChange);
	}

	void OnDisable()
	{
		player.UnregisterHealthChange (OnPlayerHealthChange);
	}
}



