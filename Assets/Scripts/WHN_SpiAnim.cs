using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WHN_SpiAnim : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spi;

	private Image img;

	private int len;

	[HideInInspector]
	public int index;

	[SerializeField]
	private float intervel;

	private WaitForSeconds wait;

	private bool bEndActive;

	private Coroutine coroutine;

	private void Awake()
	{
		img = GetComponent<Image>();
		len = spi.Length;
		wait = new WaitForSeconds((intervel != 0f) ? intervel : 0.1f);
	}

	private IEnumerator SpiAnimLoop()
	{
		while (true)
		{
			yield return wait;
			index++;
			index %= len;
			img.sprite = spi[index];
			img.SetNativeSize();
		}
	}

	private IEnumerator SpiAnimOnce()
	{
		while (index < len - 1)
		{
			yield return wait;
			index++;
			img.sprite = spi[index];
			img.SetNativeSize();
			if (index == len - 1 && !bEndActive)
			{
				yield return wait;
				base.transform.gameObject.SetActive(value: false);
			}
		}
	}

	public void PlayOnce(bool bActive)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		index = 0;
		img.sprite = spi[0];
		Stop();
		bEndActive = bActive;
		coroutine = StartCoroutine(SpiAnimOnce());
	}

	public void PlayLoop()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		index = 0;
		img.sprite = spi[0];
		Stop();
		coroutine = StartCoroutine(SpiAnimLoop());
	}

	public void Stop()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		coroutine = null;
	}

	private void PlayRoll()
	{
		img.sprite = spi[0];
		Stop();
		coroutine = StartCoroutine(SpiAnimLoop());
	}
}
