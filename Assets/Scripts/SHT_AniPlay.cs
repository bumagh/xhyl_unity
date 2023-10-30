using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SHT_AniPlay
{
	public static Sequence SHT_PlayAni(this Image img, string imgname, int startnum, float frames, bool issetactive = false, bool isloop = true)
	{
		List<Sprite> sprilist = new List<Sprite>();
		Sequence sequence = DOTween.Sequence();
		sequence.Pause();
		sequence.SetAutoKill(autoKillOnCompletion: false);
		while (true)
		{
			string text = string.Format(imgname, (++startnum).ToString("00"));
			Sprite sprite = SHT_MB_Singleton<SHT_GameManager>.GetInstance().Getanisprite(text);
			UnityEngine.Debug.LogError("动画帧: " + text);
			if (sprite != null)
			{
				sprilist.Add(sprite);
				continue;
			}
			break;
		}
		startnum = 0;
		int len = sprilist.Count;
		for (int i = 0; i < len; i++)
		{
			sequence.AppendCallback(delegate
			{
				img.sprite = sprilist[startnum];
				startnum = (startnum + 1) % len;
			});
			sequence.AppendInterval(1f / frames);
		}
		if (isloop)
		{
			sequence.SetLoops(-1);
		}
		return sequence;
	}

	public static Sequence SHT_PlayAni(this Image img, string imgname, string imgname2, int startnum, float frames, bool issetactive = false, bool isloop = true)
	{
		int num = startnum;
		List<Sprite> sprilist = new List<Sprite>();
		Sequence sequence = DOTween.Sequence();
		sequence.Pause();
		sequence.SetAutoKill(autoKillOnCompletion: false);
		while (true)
		{
			string spritename = string.Format(imgname, (++startnum).ToString("00"));
			Sprite sprite = SHT_MB_Singleton<SHT_GameManager>.GetInstance().Getanisprite(spritename);
			if (sprite != null)
			{
				sprilist.Add(sprite);
				continue;
			}
			break;
		}
		while (true)
		{
			Sprite sprite2 = SHT_MB_Singleton<SHT_GameManager>.GetInstance().Getanisprite(string.Format(imgname2, num++));
			if (sprite2 != null)
			{
				sprilist.Add(sprite2);
				continue;
			}
			break;
		}
		startnum = 0;
		num = 0;
		for (int i = 0; i < sprilist.Count; i++)
		{
			sequence.AppendCallback(delegate
			{
				img.sprite = sprilist[startnum];
				startnum = (startnum + 1) % sprilist.Count;
			});
			sequence.AppendInterval(1f / frames);
		}
		if (isloop)
		{
			sequence.SetLoops(-1);
		}
		return sequence;
	}
}
