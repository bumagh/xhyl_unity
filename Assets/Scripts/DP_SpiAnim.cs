using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DP_SpiAnim : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spi;

	private Image img;

	private int len;

	private int index;

	private float interval;

	private bool bSetSize;

	private WaitForSeconds wait;

	private void Awake()
	{
		img = GetComponent<Image>();
		len = spi.Length;
	}

	private IEnumerator SpiAnimLoop()
	{
		while (true)
		{
			yield return wait;
			index++;
			index %= len;
			img.sprite = spi[index];
			if (bSetSize)
			{
				img.SetNativeSize();
			}
		}
	}

	private IEnumerator SpiAnimOnce()
	{
		while (index < len - 1)
		{
			yield return wait;
			index++;
			img.sprite = spi[index];
			if (bSetSize)
			{
				img.SetNativeSize();
			}
			if (index == len - 1)
			{
				yield return wait;
				base.transform.gameObject.SetActive(value: false);
			}
		}
	}

	public void PlayAnim(float interval, bool bLoop, bool bSetSize = false)
	{
		index = 0;
		img.sprite = spi[0];
		wait = new WaitForSeconds(interval);
		this.bSetSize = bSetSize;
		if (!bLoop)
		{
			StartCoroutine(SpiAnimOnce());
		}
		else
		{
			StartCoroutine(SpiAnimLoop());
		}
	}
}
