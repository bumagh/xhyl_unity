using UnityEngine;
using UnityEngine.UI;

public class CSF_ImageAnim : MonoBehaviour
{
	public float duration;

	[SerializeField]
	private Sprite[] spis;

	private Image img;

	private int len;

	private float interval;

	public bool bPlaying;

	private float playTime;

	public bool bLoop;

	private int index;

	private void Awake()
	{
		img = GetComponent<Image>();
		len = spis.Length;
		if (len > 0)
		{
			interval = duration / (float)len;
		}
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
				img.SetNativeSize();
			}
			else
			{
				bPlaying = false;
				playTime = 0f;
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
