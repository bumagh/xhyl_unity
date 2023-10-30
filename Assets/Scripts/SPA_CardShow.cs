using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SPA_CardShow
{
	private Transform Tran;

	private Image img;

	private Button button;

	private int index;

	private SPA_GamblePanel gaml;

	private Sequence seq_show;

	private int Result;

	private Sprite norsprite;

	public SPA_CardShow(Transform tran, int idx, SPA_GamblePanel Gaml)
	{
		Tran = tran;
		index = idx;
		gaml = Gaml;
		img = Tran.GetComponent<Image>();
		norsprite = img.sprite;
		button = Tran.GetComponent<Button>();
		if (index != 0)
		{
			button.Addaction(ClickCard);
		}
		else
		{
			button.interactable = false;
		}
		seq_show = DOTween.Sequence();
		seq_show.Pause();
		seq_show.SetAutoKill(autoKillOnCompletion: false);
		seq_show.Append(Tran.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.7f));
		seq_show.AppendCallback(Changeimg);
		seq_show.Append(Tran.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.7f));
	}

	public void InitCard()
	{
		img.sprite = norsprite;
		Result = -1;
	}

	private void Changeimg()
	{
		try
		{
			Sprite sprite = (Result != -1) ? Getsprite(Result) : norsprite;
			img.sprite = sprite;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			throw;
		}
	}

	private Sprite Getsprite(int value)
	{
		try
		{
			int num = Getnum(value);
			return SPA_GamblePanel.ins.GetSprite($"bjlpk{num + 1:0}");
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			throw;
		}
	}

	private int Getnum(int value)
	{
		int num = value;
		if (value >= 65)
		{
			num -= 65;
		}
		else if (value >= 49)
		{
			num -= 49;
			num += 13;
		}
		else if (value >= 33)
		{
			num -= 33;
			num += 26;
		}
		else if (value >= 17)
		{
			num -= 17;
			num += 39;
		}
		return num;
	}

	private void ClickCard()
	{
		gaml.ClickCard(index);
	}

	public void Showcard(int res)
	{
		Result = res;
		seq_show.Restart();
	}

	public void SetButton(bool canclick)
	{
		button.interactable = canclick;
	}
}
