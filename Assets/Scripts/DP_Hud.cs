using DG.Tweening;
using DP_UICommon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DP_Hud : MonoBehaviour
{
	[HideInInspector]
	public Text txtBonus;

	[HideInInspector]
	public Transform tfCountDown;

	[HideInInspector]
	public Text txtCountDown;

	[HideInInspector]
	public Button btnBack;

	[HideInInspector]
	public Button btnBet;

	[HideInInspector]
	public Text txtBtnBet;

	[HideInInspector]
	public Button btnResultRecord;

	[HideInInspector]
	public GameObject objWaitTip;

	private Text txtWait;

	[HideInInspector]
	public GameObject objDouble;

	private Text txtDouble;

	[HideInInspector]
	public GameObject objUserList;

	[HideInInspector]
	public DP_UserItem[] userItems = new DP_UserItem[8];

	[SerializeField]
	private DP_PersonIcon personIcon;

	[HideInInspector]
	public GameObject objNotice;

	[HideInInspector]
	public Image imgNoticeBg;

	[HideInInspector]
	public Text txtNotice;

	private List<string> listNotice = new List<string>();

	private bool bAllNoticeHasEnd = true;

	[HideInInspector]
	public Image imgCoinTitle;

	[HideInInspector]
	public Button btnSavePoint;

	[HideInInspector]
	public Button btnTakePoint;

	[HideInInspector]
	public Text txtCoin;

	[HideInInspector]
	public Text txtScore;

	[HideInInspector]
	public Image imgCoinTip;

	[HideInInspector]
	public GameObject objScoreTip;

	private DP_TableInfo mCurrentTable;

	[HideInInspector]
	public bool mIsJoinGame;

	private float mCDTime;

	private float mCDApartTime = 1f;

	private bool mIsCDTimeVisable;

	private System.Random mRandSeed;

	private float mApartTime;

	private bool mIsBonusStay = true;

	private bool mIsBonusSpin;

	[HideInInspector]
	public DP_AnimalRecord animalRecord;

	private GameObject objStopBet;

	[HideInInspector]
	public bool bInit;

	public void Init()
	{
		bInit = true;
		mCurrentTable = DP_GameInfo.getInstance().TableInfo;
		mRandSeed = new System.Random();
		txtBonus = base.transform.Find("BonusBg/Number").GetComponent<Text>();
		tfCountDown = base.transform.Find("TimeBg");
		txtCountDown = tfCountDown.Find("Number").GetComponent<Text>();
		btnBack = base.transform.Find("BackBtn").GetComponent<Button>();
		objDouble = base.transform.Find("ObjDouble").gameObject;
		txtDouble = objDouble.transform.Find("TxtDouble").GetComponent<Text>();
		btnBet = base.transform.Find("DR/BtnBet").GetComponent<Button>();
		txtBtnBet = btnBet.transform.Find("Image/Text").GetComponent<Text>();
		txtBtnBet.text = "下注";
		btnResultRecord = base.transform.Find("DR/BtnRecord").GetComponent<Button>();
		objWaitTip = base.transform.parent.Find("WaitTip").gameObject;
		txtWait = objWaitTip.transform.Find("Text").GetComponent<Text>();
		objUserList = base.transform.parent.Find("UserList").gameObject;
		for (int i = 0; i < 8; i++)
		{
			userItems[i] = objUserList.transform.GetChild(i).GetComponent<DP_UserItem>();
		}
		objNotice = base.transform.parent.Find("NoticeMessage").gameObject;
		imgNoticeBg = objNotice.transform.GetChild(0).GetComponent<Image>();
		txtNotice = imgNoticeBg.transform.GetChild(0).GetComponent<Text>();
		Transform transform = base.transform.Find("GameScore");
		btnSavePoint = transform.Find("savepoints").GetComponent<Button>();
		btnTakePoint = transform.Find("takepoints").GetComponent<Button>();
		txtCoin = transform.Find("CoinText").GetComponent<Text>();
		txtScore = transform.Find("ScoreText").GetComponent<Text>();
		imgCoinTip = transform.Find("CoinZeroTip").GetComponent<Image>();
		objScoreTip = transform.Find("ScoreZeroTip").gameObject;
		imgCoinTitle = transform.Find("CoinTitle").GetComponent<Image>();
		btnBack.onClick.AddListener(ClickBtnBack);
		btnSavePoint.onClick.AddListener(ClickBtnSavePoint);
		btnTakePoint.onClick.AddListener(ClickBtnTakePoint);
		animalRecord = base.transform.Find("Record").GetComponent<DP_AnimalRecord>();
		animalRecord.Init();
		objStopBet = base.transform.parent.Find("StopBet").gameObject;
		objStopBet.SetActive(value: false);
	}

	private void Update()
	{
		AnimBonus();
		AnimCountDownTime();
		AnimWaitTime();
	}

	public void ShowHud()
	{
		int roomId = DP_GameInfo.getInstance().UserInfo.RoomId;
		DP_TipManager.GetSingleton().EndNetTiming();
		base.gameObject.SetActive(value: true);
		txtCoin.text = ((DP_GameInfo.getInstance().UserInfo.RoomId == 0) ? DP_GameInfo.getInstance().ExpCoinCount.ToString() : DP_GameInfo.getInstance().CoinCount.ToString());
		txtScore.text = ((DP_GameInfo.getInstance().GameScore != 0) ? DP_GameInfo.getInstance().GameScore.ToString() : ((DP_GameInfo.getInstance().Language == 0) ? "0" : "please key in"));
		tfCountDown.gameObject.SetActive(value: false);
		objDouble.SetActive(value: false);
		DP_SoundManager.GetSingleton().PlayGameMusic();
	}

	public void HideHud()
	{
		DP_SoundManager.GetSingleton().PlayGameHallMusic();
		base.gameObject.SetActive(value: false);
	}

	public void ResetGame()
	{
		mIsCDTimeVisable = false;
		DP_GameData.timeCD = 0;
		objWaitTip.SetActive(value: true);
		objNotice.SetActive(value: false);
		listNotice.Clear();
		mIsJoinGame = false;
	}

	public void SetGameCD(int nTime = 30, bool bIsJoinGame = false)
	{
		if (!mIsJoinGame)
		{
			objWaitTip.SetActive(value: false);
			mIsJoinGame = true;
		}
		if (!mIsCDTimeVisable)
		{
			mIsCDTimeVisable = true;
			tfCountDown.gameObject.SetActive(value: true);
		}
		if (bIsJoinGame)
		{
			DP_GameData.timeCD = nTime;
			DP_GameData.waitTime = nTime;
		}
		else
		{
			DP_GameData.timeCD = mCurrentTable.MaxCD;
		}
		mCDTime = 0f;
		txtCountDown.text = DP_GameData.timeCD.ToString();
		DP_NetMngr.GetSingleton().MyCreateSocket.SendResultList(DP_GameInfo.getInstance().UserInfo.TableId);
	}

	public void SetBonusNumber(int nNum, bool bIsBonusAward = false)
	{
		DP_GameData.bBonusAward = bIsBonusAward;
		DP_GameData.bonusNumber = nNum;
	}

	public void StopBonusSpin()
	{
		mIsBonusSpin = false;
	}

	public void PlayDoubleAnim(bool bPlay)
	{
		if (bPlay)
		{
			objDouble.transform.DOKill();
			objDouble.transform.localEulerAngles = Vector3.back * -10f;
			objDouble.transform.DOLocalRotate(Vector3.forward * 10f, 0.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		}
		else
		{
			objDouble.transform.DOKill();
			objDouble.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.Linear);
		}
	}

	public void ShowObjDouble(bool bShow)
	{
		objDouble.transform.DOKill();
		objDouble.SetActive(bShow);
		if (bShow)
		{
			txtDouble.text = DP_GameData.times.ToString();
		}
	}

	public void AddUser(int iSeatID, string strNickname, int iIconIndex = 1)
	{
		if (iSeatID > 7 && iSeatID < 0)
		{
			UnityEngine.Debug.Log("Error:座位号错误");
			return;
		}
		DP_UserItem dP_UserItem = userItems[iSeatID];
		if (strNickname.CompareTo(string.Empty) == 0)
		{
			dP_UserItem.tfUserItem.gameObject.SetActive(value: false);
			dP_UserItem.bFree = true;
			return;
		}
		dP_UserItem.txtNickname.text = strNickname;
		dP_UserItem.tfUserItem.gameObject.SetActive(value: true);
		dP_UserItem.imgPersonIcon.sprite = personIcon.spis[iIconIndex - 1];
		dP_UserItem.bFree = false;
	}

	public void AddNotice(string noticeMessage)
	{
		if (bAllNoticeHasEnd)
		{
			UpdateNotice(noticeMessage);
			objNotice.SetActive(value: true);
			imgNoticeBg.color = Color.white;
			bAllNoticeHasEnd = false;
		}
		else
		{
			listNotice.Add(noticeMessage);
		}
	}

	private void UpdateNotice(string noticeMessage)
	{
		txtNotice.text = noticeMessage;
		txtNotice.transform.DOScale(1f, 0.02f).OnComplete(delegate
		{
			Vector2 sizeDelta = txtNotice.rectTransform.sizeDelta;
			float num = sizeDelta.x / 2f;
			txtNotice.transform.localPosition = Vector3.right * (num + 300f);
			float duration = (num + 600f) / 200f;
			txtNotice.transform.DOLocalMoveX(0f - num - 300f, duration).OnComplete(NoticeEnd);
		});
	}

	public void NoticeEnd()
	{
		if (listNotice.Count == 0)
		{
			bAllNoticeHasEnd = true;
			imgNoticeBg.DOFade(0f, 0.5f).OnComplete(delegate
			{
				objNotice.SetActive(value: false);
			});
		}
		else
		{
			UpdateNotice(listNotice[0]);
			listNotice.RemoveAt(0);
		}
	}

	public void SetCurrentPosition(int room, DP_TableInfo table)
	{
		mCurrentTable = table;
		mCDApartTime = (float)(mCurrentTable.MaxCD - 1) / (float)mCurrentTable.MaxCD;
	}

	private void AnimBonus()
	{
		if (mIsBonusStay)
		{
			return;
		}
		if (!mIsBonusSpin)
		{
			mIsBonusStay = true;
			txtBonus.text = DP_GameData.bonusNumber.ToString().PadLeft(5, '0');
			return;
		}
		mApartTime += Time.deltaTime;
		if (mApartTime >= 0.1f)
		{
			mApartTime = 0f;
			txtBonus.text = mRandSeed.Next(1000, 30000).ToString().PadLeft(5, '0');
		}
	}

	private void AnimCountDownTime()
	{
		if (DP_GameData.timeCD <= 0)
		{
			return;
		}
		mCDTime += Time.deltaTime;
		if (!(mCDTime >= mCDApartTime))
		{
			return;
		}
		DP_GameData.timeCD -= (int)(mCDTime / mCDApartTime);
		mCDTime = 0f;
		txtCountDown.text = DP_GameData.timeCD.ToString();
		if (DP_GameData.timeCD <= 0)
		{
			mIsCDTimeVisable = false;
			DP_MusicMngr.GetSingleton().PlayUISound(DP_MusicMngr.MUSIC_UI.UI_COUNTDONW1);
			tfCountDown.gameObject.SetActive(value: false);
			objStopBet.SetActive(value: false);
		}
		if (DP_GameData.timeCD <= 3 && DP_GameData.timeCD > 0)
		{
			DP_MusicMngr.GetSingleton().PlayUISound(DP_MusicMngr.MUSIC_UI.UI_COUNTDOWN0);
			if (DP_GameData.timeCD <= 1)
			{
				objStopBet.SetActive(value: true);
			}
		}
	}

	private void AnimWaitTime()
	{
		if (DP_GameData.waitTime > 0f)
		{
			txtWait.text = $"游戏同步中请等待...{(int)DP_GameData.waitTime + 1}";
			DP_GameData.waitTime -= Time.deltaTime;
		}
	}

	public void ShowScoreTip()
	{
		objScoreTip.SetActive(value: true);
		objScoreTip.transform.DOKill();
		objScoreTip.transform.DOScale(1f, 2f).OnComplete(delegate
		{
			objScoreTip.SetActive(value: false);
		});
	}

	public void ShowCoinTip()
	{
		imgCoinTip.gameObject.SetActive(value: true);
		imgCoinTip.transform.DOKill();
		imgCoinTip.transform.DOScale(1f, 2f).OnComplete(delegate
		{
			imgCoinTip.gameObject.SetActive(value: false);
		});
	}

	public void ClickBtnTakePoint()
	{
		int num = 0;
		int num2 = (DP_GameInfo.getInstance().UserInfo.RoomId == 0) ? DP_GameInfo.getInstance().ExpCoinCount : DP_GameInfo.getInstance().CoinCount;
		MonoBehaviour.print(DP_GameInfo.getInstance().UserInfo.RoomId);
		if (num2 <= 0)
		{
			if (DP_GameInfo.getInstance().UserInfo.RoomId == 0 && DP_GameInfo.getInstance().GameScore <= 0)
			{
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_SendExpCoin);
			}
			ShowCoinTip();
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.BetFail);
		}
		else
		{
			num = ((num2 > mCurrentTable.CoinInSetting) ? mCurrentTable.CoinInSetting : num2);
			UnityEngine.Debug.Log(mCurrentTable.CoinInSetting + "," + mCurrentTable.CreditPerCoin);
			DP_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn(num);
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.CoinIn);
		}
	}

	public void ClickBtnSavePoint()
	{
		int num = 0;
		int gameScore = DP_GameInfo.getInstance().GameScore;
		if (gameScore < mCurrentTable.CreditPerCoin)
		{
			ShowScoreTip();
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.BetFail);
		}
		else
		{
			num = ((gameScore < mCurrentTable.CoinInSetting * mCurrentTable.CreditPerCoin) ? (gameScore / mCurrentTable.CreditPerCoin * mCurrentTable.CreditPerCoin) : (mCurrentTable.CoinInSetting * mCurrentTable.CreditPerCoin));
			DP_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut(num);
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.CoinIn);
		}
	}

	public void ClickBtnBack()
	{
		DP_TipManager.GetSingleton().ShowTip(EGameTipType.IsExitGame);
		DP_SoundManager.GetSingleton().playButtonSound();
	}
}
