using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class STTF_SpiAnim : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spi;

	private Image img;

	private int len;

	private int index;

	[SerializeField]
	private float intervel;

	[SerializeField]
	private bool bOnce;

	private WaitForSeconds wait;

	private void Awake()
	{
		img = GetComponent<Image>();
		len = spi.Length;
		wait = new WaitForSeconds((intervel != 0f) ? intervel : 0.1f);
	}

	private void OnEnable()
	{
		index = 0;
		img.sprite = spi[0];
		StopAllCoroutines();
		if (bOnce)
		{
			StartCoroutine(SpiAnimOnce());
		}
		else
		{
			StartCoroutine(SpiAnimLoop());
		}
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
			if (index == len - 1)
			{
				yield return wait;
				base.transform.gameObject.SetActive(value: false);
			}
		}
	}
}
