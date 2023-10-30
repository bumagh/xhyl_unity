using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSE_GamblePanel : MonoBehaviour
{
	private Image cardopenimg;

	private Button Redbu;

	private Button Blackbu;

	private GameObject norbg;

	private GameObject winbg;

	private Text balance;

	private Text douletext;

	private Text scoretext;

	private Button getscore;

	private Transform ShowCard;

	private Image cardshowimg;

	private List<GameObject> showlist = new List<GameObject>();

	public PTM_Pokers spiPokers;

	public Dictionary<string, Sprite> Spritelist = new Dictionary<string, Sprite>();

	public static MSE_GamblePanel ins;

	private Sequence opencard;

	private Sprite norsprite;

	private bool showcard;

	private int result;

	private long balancescore;

	private long doublescore;

	private long Winvalue;

	public int Gamlewin;

	public int AllScroce;

	public bool isGamble;

	private void Awake()
	{
		ins = this;
		Spritelist = new Dictionary<string, Sprite>();
		Sprite[] spis = spiPokers.spis;
		foreach (Sprite sprite in spis)
		{
			if (Spritelist.ContainsKey(sprite.name))
			{
				sprite.name += "_";
			}
			Spritelist.Add(sprite.name, sprite);
		}
		UnityEngine.Debug.LogError("Count: " + Spritelist.Count);
		cardopenimg = base.transform.Find<Image>("Card");
		norsprite = cardopenimg.sprite;
		Redbu = base.transform.Find<Button>("Red");
		Redbu.Addaction(Clickred);
		Blackbu = base.transform.Find<Button>("Black");
		Blackbu.Addaction(ClickBlack);
		douletext = base.transform.Find<Text>("double");
		scoretext = base.transform.Find<Text>("score");
		balance = base.transform.Find<Text>("balamce");
		winbg = base.transform.Find("Winbg").gameObject;
		norbg = base.transform.Find("Nomalbg").gameObject;
		getscore = base.transform.Find<Button>("GetSocre");
		getscore.Addaction(GetScore);
		ShowCard = base.transform.Find("ShowCardGroup");
		cardshowimg = ShowCard.Find<Image>("card");
		opencard = DOTween.Sequence();
		opencard.Pause();
		opencard.SetAutoKill(autoKillOnCompletion: false);
		opencard.Append(cardopenimg.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 1f));
		opencard.AppendCallback(Chanimg);
		opencard.Append(cardopenimg.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.6f));
	}

	private void OnEnable()
	{
		isGamble = false;
		Showgamelegame(isshow: true, -1L);
	}

	private void SetButton(bool canclick)
	{
		Redbu.interactable = canclick;
		Blackbu.interactable = canclick;
		getscore.interactable = canclick;
	}

	private void Clickred()
	{
		MSE_MB_Singleton<MSE_DiceGameController2>.GetInstance().Send_MultipleInfo(MSE_BetDiceType.Half);
		SetButton(canclick: false);
	}

	private void ClickBlack()
	{
		MSE_MB_Singleton<MSE_DiceGameController2>.GetInstance().Send_MultipleInfo(MSE_BetDiceType.Risk);
		SetButton(canclick: false);
	}

	private void GetScore()
	{
		if (balancescore == 0)
		{
			balancescore = Gamlewin;
		}
		GambleEnd();
	}

	public Sprite GetSprite(string spritename)
	{
		try
		{
			Sprite sprite = null;
			if (Spritelist.ContainsKey(spritename))
			{
				sprite = Spritelist[spritename];
			}
			return sprite;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			throw;
		}
	}

	private Sprite Getsprite()
	{
		try
		{
			uint num = Getnum(MSE_MB_Singleton<MSE_DiceGameController2>.GetInstance().Gamelewinid);
			return ins.GetSprite($"bjlpk{num + 1:0}");
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			throw;
		}
	}

	private uint Getnum(uint value)
	{
		uint num = value;
		switch (value)
		{
		default:
			num -= 65;
			break;
		case 49u:
		case 50u:
		case 51u:
		case 52u:
		case 53u:
		case 54u:
		case 55u:
		case 56u:
		case 57u:
		case 58u:
		case 59u:
		case 60u:
		case 61u:
		case 62u:
		case 63u:
		case 64u:
			num -= 49;
			num += 13;
			break;
		case 33u:
		case 34u:
		case 35u:
		case 36u:
		case 37u:
		case 38u:
		case 39u:
		case 40u:
		case 41u:
		case 42u:
		case 43u:
		case 44u:
		case 45u:
		case 46u:
		case 47u:
		case 48u:
			num -= 33;
			num += 26;
			break;
		case 17u:
		case 18u:
		case 19u:
		case 20u:
		case 21u:
		case 22u:
		case 23u:
		case 24u:
		case 25u:
		case 26u:
		case 27u:
		case 28u:
		case 29u:
		case 30u:
		case 31u:
		case 32u:
			num -= 17;
			num += 39;
			break;
		case 0u:
		case 1u:
		case 2u:
		case 3u:
		case 4u:
		case 5u:
		case 6u:
		case 7u:
		case 8u:
		case 9u:
		case 10u:
		case 11u:
		case 12u:
		case 13u:
		case 14u:
		case 15u:
		case 16u:
			break;
		}
		return num;
	}

	private void Chanimg()
	{
		cardopenimg.sprite = ((!showcard) ? Getsprite() : norsprite);
		showcard = !showcard;
	}

	public void Showgamelegame(bool isshow, long score = -1L)
	{
		base.transform.localScale = new Vector3(isshow ? 1 : 0, 1f, 1f);
		if (score >= 0)
		{
			ShowResult(score);
		}
		else
		{
			Initgamble();
		}
	}

	private void Initgamble()
	{
		balancescore = MSE_GVars.winScorce;
		doublescore = MSE_GVars.winScorce * 2;
		UnityEngine.Debug.LogError(balancescore + "  " + doublescore);
		balance.text = balancescore.ToString();
		douletext.text = doublescore.ToString();
		ShowScore(0L);
		Winvalue = 0L;
		showcard = false;
	}

	private void ShowResult(long score)
	{
		opencard.Restart();
		Sequence s = DOTween.Sequence();
		s.AppendInterval(1.5f);
		s.AppendCallback(delegate
		{
			ShowScore(score);
			ShowCardimg();
		});
		s.AppendInterval(1f);
		s.AppendCallback(CheckEnd);
	}

	private void ShowScore(long score)
	{
		winbg.SetActive(score != 0);
		norbg.SetActive(score == 0);
		scoretext.text = ((score != 0) ? score.ToString() : string.Empty);
		if (score != 0)
		{
			balance.text = (balancescore + score).ToString();
			balancescore += score;
			douletext.text = (balancescore * 2).ToString();
			doublescore = balancescore * 2;
		}
	}

	private void CheckEnd()
	{
		ShowScore(0L);
		if (Gamlewin > 0)
		{
			opencard.Restart();
			Sequence s = DOTween.Sequence();
			s.AppendInterval(1.8f);
			s.AppendCallback(delegate
			{
				SetButton(canclick: true);
			});
		}
		else
		{
			balancescore = 0L;
			GambleEnd();
		}
	}

	private void ShowCardimg()
	{
		Image image = UnityEngine.Object.Instantiate(cardshowimg);
		image.sprite = Getsprite();
		image.transform.SetParent(ShowCard);
		image.transform.localScale = Vector3.one;
		image.gameObject.SetActive(value: true);
		showlist.Add(image.gameObject);
	}

	private void GambleEnd()
	{
		MSE_MB_Singleton<MSE_DiceGameController2>.GetInstance().OnBtnReady_Click();
		MSE_MB_Singleton<MSE_DiceGameController2>.GetInstance().Hide();
		int credit = (!isGamble) ? (MSE_GVars.winScorce + MSE_GVars.credit) : AllScroce;
		MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().SetCredit(credit);
		MSE_MB_Singleton<MSE_HUDController>.GetInstance().Show();
		balance.text = "0";
		douletext.text = "0";
		cardopenimg.sprite = norsprite;
		for (int i = 0; i < showlist.Count; i++)
		{
			UnityEngine.Object.Destroy(showlist[i]);
		}
		showlist.Clear();
		SetButton(canclick: true);
	}
}
