using System.Collections;
using UnityEngine;

public class All_LightCor : MonoBehaviour
{
	private ParticleSystem particleSystem;

	public Color color;

	[Range(0f, 1f)]
	public float minValue;

	[Range(0f, 1f)]
	public float maxValue = 0.1f;

	[Range(0.02f, 1f)]
	public float waitTime = 0.02f;

	[Range(0.02f, 1f)]
	public float addSpeed = 0.02f;

	private Coroutine setShowLight;

	private float tempA;

	private int add = 1;

	private void OnEnable()
	{
		if (particleSystem == null)
		{
			particleSystem = GetComponent<ParticleSystem>();
		}
		tempA = 0f;
		if (setShowLight != null)
		{
			StopCoroutine(setShowLight);
		}
		setShowLight = StartCoroutine(SetShowLight());
	}

	private IEnumerator SetShowLight()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1.1f));
		while (true)
		{
			if (tempA > maxValue)
			{
				add = -1;
			}
			else if (tempA < minValue)
			{
				add = 1;
			}
			particleSystem.startColor = new Color(color.r, color.g, color.b, tempA);
			yield return new WaitForSeconds(waitTime);
			tempA += addSpeed * (float)add;
		}
	}
}
