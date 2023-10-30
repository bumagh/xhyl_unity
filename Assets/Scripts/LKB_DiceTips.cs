using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LKB_DiceTips : MonoBehaviour
{
	[SerializeField]
	private Sprite spiGongxi;

	[SerializeField]
	private Sprite spiBibei;

	[SerializeField]
	private Sprite spiDefen;

	[SerializeField]
	private Sprite spiBibeibaoji;

	[SerializeField]
	private Sprite spiGongxiEn;

	[SerializeField]
	private Sprite spiBibeiEn;

	[SerializeField]
	private Sprite spiDefenEn;

	[SerializeField]
	private Sprite spiBibeibaojiEn;

	private Image img;

	private void Awake()
	{
		img = GetComponent<Image>();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void PlayNormal()
	{
		StartCoroutine(_playNormal());
	}

	public void PlayOverflow()
	{
		StartCoroutine(_playOverflow());
	}

	private IEnumerator _playNormal()
	{
		float interval = 0.5f;
		if (LKB_GVars.language == "zh")
		{
			img.sprite = spiGongxi;
		}
		else
		{
			img.sprite = spiGongxiEn;
		}
		img.SetNativeSize();
		yield return new WaitForSeconds(interval);
		img.enabled = false;
		yield return new WaitForSeconds(interval);
		List<Sprite> sprites = new List<Sprite>
		{
			spiBibei,
			spiDefen
		};
		List<Sprite> sprites_En = new List<Sprite>
		{
			spiBibeiEn,
			spiDefenEn
		};
		int index = 0;
		bool enable = true;
		while (true)
		{
			img.enabled = enable;
			if (enable)
			{
				if (LKB_GVars.language == "zh")
				{
					img.sprite = sprites[index];
				}
				else
				{
					img.sprite = sprites_En[index];
				}
				img.SetNativeSize();
				index = (index + 1) % sprites.Count;
				yield return new WaitForSeconds(interval);
			}
			else
			{
				yield return new WaitForSeconds(0.3f);
			}
			enable = !enable;
		}
	}

	private IEnumerator _playOverflow()
	{
		float interval = 0.5f;
		if (LKB_GVars.language == "zh")
		{
			img.sprite = spiBibeibaoji;
		}
		else
		{
			img.sprite = spiBibeibaojiEn;
		}
		img.SetNativeSize();
		UnityEngine.Debug.Log("OverFlowShow()" + Time.time);
		yield return new WaitForSeconds(interval);
		bool enable = true;
		while (true)
		{
			img.enabled = enable;
			if (base.enabled)
			{
				yield return new WaitForSeconds(interval);
			}
			else
			{
				yield return new WaitForSeconds(0.3f);
			}
			enable = !enable;
		}
	}
}
