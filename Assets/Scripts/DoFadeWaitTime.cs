using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoFadeWaitTime : MonoBehaviour
{
	public Image image;

	public float addSpeed;

	public float waitTime;

	public float intervalMaxTime = 1f;

	public float intervalMinTime = 1f;

	[Range(0f, 1f)]
	public float minValue;

	[Range(0f, 1f)]
	public float maxValue = 1f;

	private Coroutine wiatSet;

	private float tempA;

	private int add = 1;

	private void OnEnable()
	{
		if (wiatSet != null)
		{
			StopCoroutine(wiatSet);
		}
		if (image != null)
		{
			wiatSet = StartCoroutine(WiatSet());
		}
	}

	private IEnumerator WiatSet()
	{
		while (image != null)
		{
			if (tempA > maxValue)
			{
				yield return new WaitForSeconds(intervalMaxTime);
			}
			if (tempA < minValue)
			{
				yield return new WaitForSeconds(intervalMinTime);
			}
			if (tempA > maxValue)
			{
				add = -1;
			}
			else if (tempA < minValue)
			{
				add = 1;
			}
			Image obj = image;
			Color color = image.color;
			float r = color.r;
			Color color2 = image.color;
			float g = color2.g;
			Color color3 = image.color;
			obj.color = new Color(r, g, color3.b, tempA);
			yield return new WaitForSeconds(waitTime);
			tempA += addSpeed * (float)add;
		}
	}
}
