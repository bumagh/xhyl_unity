using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_SpiAnim : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spi;

	private Image img;

	private SpriteRenderer render;

	private int len;

	private int index;

	[SerializeField]
	private float intervel;

	[SerializeField]
	private bool bOnce;

	private WaitForSeconds wait;

	public bool isSetNativeSize;

	public float waitTime;

	private Sprite blankAll;

	private void Awake()
	{
		img = GetComponent<Image>();
		render = GetComponent<SpriteRenderer>();
		len = spi.Length;
		wait = new WaitForSeconds((intervel != 0f) ? intervel : 0.1f);
		blankAll = Resources.Load<Sprite>("blankAll");
	}

	private void OnEnable()
	{
		index = 0;
		if (img != null)
		{
			img.sprite = spi[0];
		}
		if (render != null)
		{
			render.sprite = spi[0];
		}
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
			if (index >= spi.Length && waitTime > 0f)
			{
				SetBlankAll();
				yield return new WaitForSeconds(waitTime);
			}
			index %= len;
			if (img != null)
			{
				img.sprite = spi[index];
			}
			if (render != null)
			{
				render.sprite = spi[index];
			}
			if (isSetNativeSize)
			{
				img.SetNativeSize();
			}
		}
	}

	private void SetBlankAll()
	{
		if (img != null)
		{
			img.sprite = blankAll;
		}
		if (render != null)
		{
			render.sprite = blankAll;
		}
	}

	private IEnumerator SpiAnimOnce()
	{
		while (index < len - 1)
		{
			yield return wait;
			index++;
			img.sprite = spi[index];
			if (isSetNativeSize && img != null)
			{
				img.SetNativeSize();
			}
			if (render != null)
			{
				render.sprite = spi[index];
			}
			if (index == len - 1)
			{
				yield return wait;
				if (isSetNativeSize)
				{
					base.transform.gameObject.SetActive(value: false);
				}
			}
		}
	}
}
