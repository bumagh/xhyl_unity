using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRL_CellAnim : MonoBehaviour
{
	private float duration;

	[HideInInspector]
	public List<Sprite> spis = new List<Sprite>();

	private Image img;

	private int len;

	private float interval;

	private bool bPlaying;

	private float playTime;

	private bool bLoop;

	private int index;

	private void Awake()
	{
		img = GetComponent<Image>();
	}

	public void Play(Sprite[] arrSpi, float dur, bool loop)
	{
		if (spis.Count > 0)
		{
			spis.Clear();
		}
		len = arrSpi.Length;
		duration = dur;
		interval = duration / (float)len;
		for (int i = 0; i < len; i++)
		{
			spis.Add(arrSpi[i]);
		}
		bLoop = loop;
		bPlaying = true;
		playTime = 0f;
	}

	private void Update()
	{
		if (bPlaying)
		{
			playTime += Time.deltaTime;
			index = (int)(playTime / interval);
			if (bLoop)
			{
				index %= len;
			}
			if (index < len)
			{
				img.sprite = spis[index];
			}
			else
			{
				bPlaying = false;
			}
		}
	}

	public void Play()
	{
		bPlaying = true;
	}

	public void Stop()
	{
		bPlaying = false;
	}
}
