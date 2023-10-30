using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPA_GamblePanel : SPA_MB_Singleton<SPA_GamblePanel>
{
	private Button Redbu;

	private Button Blackbu;

	private Text balance;

	private Text scoretext;

	private Button getscore;

	private List<SPA_CardShow> showlist = new List<SPA_CardShow>();

	private List<GameObject> Wintypelist = new List<GameObject>();

	private bool showcard;

	private int result;

	private long balancescore;

	private Text bet;

	private Text pay0;

	private Text pay1;

	private long Betvalue;

	private int[] CardValuelist = new int[5];

	public PTM_Pokers spiPokers;

	public Dictionary<string, Sprite> Spritelist = new Dictionary<string, Sprite>();

	public static SPA_GamblePanel ins;

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
		Redbu = base.transform.Find<Button>("Red");
		Redbu.Addaction(Clickred);
		Blackbu = base.transform.Find<Button>("Black");
		Blackbu.Addaction(ClickBlack);
		scoretext = base.transform.Find<Text>("winscore");
		balance = base.transform.Find<Text>("balamce");
		Transform transform = base.transform.Find("ShowCardGroup");
		int num = 0;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform tran = (Transform)enumerator.Current;
				showlist.Add(new SPA_CardShow(tran, num++, this));
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		Transform transform2 = base.transform.Find("Tipimggroup");
		IEnumerator enumerator2 = transform2.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform3 = (Transform)enumerator2.Current;
				Wintypelist.Add(transform3.gameObject);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		getscore = base.transform.Find<Button>("GetSocre");
		getscore.Addaction(GetScore);
		bet = base.transform.Find<Text>("bet");
		pay0 = base.transform.Find<Text>("pay0");
		pay1 = base.transform.Find<Text>("pay1");
	}

	private void OnEnable()
	{
		Betvalue = 0L;
		isGamble = false;
		Showgamelegame(isshow: true, -1L);
	}

	private void SetallcardButton(bool canclick)
	{
		for (int i = 1; i < showlist.Count; i++)
		{
			showlist[i].SetButton(canclick);
		}
	}

	private void SetButton(bool canclick)
	{
		Redbu.interactable = canclick;
		Blackbu.interactable = canclick;
		getscore.interactable = canclick;
	}

	private void Clickred()
	{
		SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().Send_MultipleInfo(SPA_BetDiceType.Half);
		Betvalue = balancescore / 2;
		SetBetNscore();
		SetButton(canclick: false);
	}

	private void ClickBlack()
	{
		SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().Send_MultipleInfo(SPA_BetDiceType.Risk);
		SetButton(canclick: false);
		Betvalue = balancescore;
		SetBetNscore();
	}

	private void SetBetNscore()
	{
		UnityEngine.Debug.LogError("Betvalue:  " + Betvalue + "   balancescore:  " + balancescore + "   " + (balancescore - Betvalue).ToString());
		bet.text = Betvalue.ToString();
		balance.text = (balancescore - Betvalue).ToString();
	}

	private void GetScore()
	{
		if (balancescore == 0)
		{
			balancescore = Gamlewin;
		}
		GambleEnd();
	}

	private void GetCardlist()
	{
		long gamelewinid = SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().Gamelewinid;
		UnityEngine.Debug.Log(gamelewinid);
		for (int i = 0; i < 5; i++)
		{
			CardValuelist[i] = (int)((gamelewinid & (255L << i * 8)) >> i * 8);
		}
		UnityEngine.Debug.Log(JsonMapper.ToJson(CardValuelist));
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

	private void ShowTip(int index)
	{
		foreach (GameObject item in Wintypelist)
		{
			item.SetActive(value: false);
		}
		Wintypelist[index].SetActive(value: true);
	}

	public void InitGamble()
	{
		balancescore = 0L;
		scoretext.text = string.Empty;
		for (int i = 0; i < showlist.Count; i++)
		{
			showlist[i].InitCard();
		}
		SetallcardButton(canclick: false);
		Startgamble(isStart: true);
		showcard = false;
	}

	public void Showgamelegame(bool isshow, long score = -1L)
	{
		base.transform.localScale = new Vector3(isshow ? 1 : 0, 1f, 1f);
		if (score >= 0)
		{
			ShowCard();
		}
		else
		{
			InitGamble();
		}
	}

	public void ClickCard(int num)
	{
		SetallcardButton(canclick: false);
		showlist[num].Showcard(CardValuelist[1]);
		int idx = 2;
		Sequence s = DOTween.Sequence();
		s.AppendInterval(1f);
		s.AppendCallback(delegate
		{
			for (int i = 1; i < 5; i++)
			{
				if (i != num)
				{
					showlist[i].Showcard(CardValuelist[++idx]);
				}
			}
		});
		s.AppendInterval(1f);
		s.AppendCallback(delegate
		{
			ShowScore(Gamlewin);
		});
		s.AppendInterval(1f);
		s.AppendCallback(CheckEnd);
	}

	private void Startgamble(bool isStart)
	{
		UnityEngine.Debug.LogError("分数:" + ((!isStart) ? Gamlewin : SPA_GVars.winScorce) + " isStart: " + isStart);
		scoretext.text = string.Empty;
		Betvalue = 0L;
		Setbalance((!isStart) ? Gamlewin : SPA_GVars.winScorce);
		SetBetNscore();
		ShowTip(2);
		SetButton(canclick: true);
	}

	private void InitCrad()
	{
		for (int i = 0; i < showlist.Count; i++)
		{
			showlist[i].Showcard(-1);
		}
	}

	private void ShowCard()
	{
		GetCardlist();
		showlist[0].Showcard(CardValuelist[0]);
		ShowTip(1);
		Sequence s = DOTween.Sequence();
		s.AppendInterval(1.4f);
		s.AppendCallback(delegate
		{
			SetallcardButton(canclick: true);
		});
	}

	private void ShowResult()
	{
	}

	private void ShowScore(long score)
	{
		scoretext.text = ((score != 0) ? score.ToString() : string.Empty);
		if (score != 0)
		{
			SPA_SoundManager.Instance.PlayDiceWinAudio();
			ShowTip(3);
		}
		else
		{
			SPA_SoundManager.Instance.PlayDiceLooseAudio();
			ShowTip(0);
		}
	}

	private void Setbalance(long score)
	{
		if (score > 0)
		{
			balance.text = (balancescore + score).ToString("0");
			balancescore += score;
			pay0.text = ((float)balancescore * 1.5f).ToString("0");
			pay1.text = ((float)balancescore * 2f).ToString("0");
		}
		else
		{
			pay0.text = "0";
			pay1.text = "0";
		}
	}

	private void CheckEnd()
	{
		if (Gamlewin > 0)
		{
			Startgamble(isStart: false);
			InitCrad();
		}
		else
		{
			balancescore -= Betvalue;
			GambleEnd();
		}
	}

	private void GambleEnd()
	{
		SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().OnBtnReady_Click();
		SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().Hide();
		int credit = (!isGamble) ? (SPA_GVars.winScorce + SPA_GVars.credit) : AllScroce;
		SPA_MB_Singleton<SPA_MajorGameController>.GetInstance().SetCredit(credit);
	}
}
