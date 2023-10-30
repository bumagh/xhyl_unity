using STDT_GameConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_GameUIMngr : MonoBehaviour
{
	private static XLDT_GameUIMngr gameUIMngr;

	private XLDT_GameInfo mGameInfo;

	public XLDT_POP_DLG_TYPE mCurDlgType = XLDT_POP_DLG_TYPE.DLG_NONE;

	[SerializeField]
	private XLDT_DlgBase sptDlgBase;

	public XLDT_DlgSetting mSettingDlg;

	public XLDT_DlgPersonInfo mPersonInfoDlg;

	public XLDT_BetCtrl mBetCtrl;

	public XLDT_UserListCtrl mUserList;

	public GameObject objRoom;

	public GameObject objGame;

	public GameObject objBtnList;

	public GameObject objDark;

	protected int _userCoin;

	protected int _userScore;

	public int UserCoin
	{
		get
		{
			return _userCoin;
		}
		set
		{
			_userCoin = value;
		}
	}

	public int UserScore
	{
		get
		{
			return _userScore;
		}
		set
		{
			_userScore = value;
			mBetCtrl.TotalScore = value;
		}
	}

	public static XLDT_GameUIMngr GetSingleton()
	{
		return gameUIMngr;
	}

	public void init()
	{
		mCurDlgType = XLDT_POP_DLG_TYPE.DLG_NONE;
	}

	private void Awake()
	{
		if (gameUIMngr == null)
		{
			gameUIMngr = this;
		}
		init();
	}

	private void Start()
	{
		mGameInfo = XLDT_GameInfo.getInstance();
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void CloseSafeBox()
	{
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserCoinIn();
	}

	private void SaveScore()
	{
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserCoinOut();
	}

	private void OnDisable()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	public IEnumerator StartGame()
	{
		Application.runInBackground = true;
		yield return new WaitForEndOfFrame();
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_Game;
		GetSingleton().UpdateUserList(XLDT_GameInfo.getInstance().UserList);
		if (XLDT_GameInfo.getInstance().CurAward.awardType != XLDT_EAwardType.None)
		{
			GetSingleton().Restart((int)XLDT_GameInfo.getInstance().CurAward.awardType, (int)XLDT_GameInfo.getInstance().CurAward.color, XLDT_GameInfo.getInstance().CurAward.num);
		}
	}

	public void ShowSetDlg(bool isShow)
	{
		if (isShow)
		{
			mSettingDlg.gameObject.SetActive(value: true);
		}
		ShowDlg(isShow, XLDT_POP_DLG_TYPE.DLG_SETTING);
	}

	public void ShowMoneyInOutDlg(bool isShow)
	{
		if (isShow)
		{
		}
		ShowDlg(isShow, XLDT_POP_DLG_TYPE.DLG_MONEY_INOUT);
	}

	public void ShowMoneyInOutDlg()
	{
		if (ZH2_GVars.OpenPlyBoxPanel != null)
		{
			ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.card_desk);
		}
	}

	public void ShowPersonInfoDlg(bool isShow, int userId = 0, string nickName = "", int nPhotoIndex = 0, int nLevel = 0, int nGameCoin = 0, string honour = "")
	{
		if (isShow)
		{
			mPersonInfoDlg.gameObject.SetActive(value: true);
			mPersonInfoDlg.ShowPersonInfo(userId, nickName, nPhotoIndex, nLevel, nGameCoin, honour);
		}
		ShowDlg(isShow, XLDT_POP_DLG_TYPE.DLG_PERSON_INFO);
	}

	public void ShowPersonInfoDlg(XLDT_User user, int[] nHonorList)
	{
		string[] array = new string[3]
		{
			XLDT_Localization.Get("DlgPersonInfoTopList_All"),
			XLDT_Localization.Get("DlgPersonInfoTopList_Week"),
			XLDT_Localization.Get("DlgPersonInfoTopList_Day")
		};
		string text = string.Empty;
		for (int i = 0; i < nHonorList.Length; i++)
		{
			if (nHonorList[i] != -1)
			{
				string text2 = text;
				text = text2 + array[i] + nHonorList[i] + " ";
			}
		}
		if (text == string.Empty)
		{
			text = XLDT_Localization.Get("DlgPersonInfoNoHonour");
		}
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			if (XLDT_GameInfo.getInstance().User.RoomId == 0)
			{
				GetSingleton().ShowPersonInfoDlg(isShow: true, user.id, user.nickname, user.photoId, user.level, user.expeScore, text);
			}
			else
			{
				GetSingleton().ShowPersonInfoDlg(isShow: true, user.id, user.nickname, user.photoId, user.level, user.gameScore, text);
			}
		}
	}

	public void ShowChatDlg(bool isShow, bool isPrivateChat = false, string nickName = "", int userId = 0)
	{
		if (isShow)
		{
		}
		ShowDlg(isShow, XLDT_POP_DLG_TYPE.DLG_CHAT);
	}

	public void ShowChatDlg()
	{
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.card_desk);
		}
	}

	public void ShowTip(bool isShow)
	{
		if (isShow)
		{
			mCurDlgType = XLDT_POP_DLG_TYPE.DLG_TIP;
		}
		else
		{
			mCurDlgType = XLDT_POP_DLG_TYPE.DLG_NONE;
		}
	}

	public void UpdateUserList(List<XLDT_UserInfo> list)
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			XLDT_UserInfo xLDT_UserInfo = list[i];
			if (xLDT_UserInfo.SeatIndex >= 9 || xLDT_UserInfo.SeatIndex <= 0)
			{
				break;
			}
			if (xLDT_UserInfo.IsExist)
			{
				if (xLDT_UserInfo.SeatIndex == XLDT_GameInfo.getInstance().User.SeatIndex)
				{
					mUserList.ShowUser(xLDT_UserInfo.SeatIndex - 1, xLDT_UserInfo.Icon, xLDT_UserInfo.Name, isPlayer: true);
				}
				else
				{
					mUserList.ShowUser(xLDT_UserInfo.SeatIndex - 1, xLDT_UserInfo.Icon, xLDT_UserInfo.Name);
				}
			}
			else
			{
				mUserList.HideUser(xLDT_UserInfo.SeatIndex - 1);
			}
		}
	}

	public void Restart(int nClearCard, int nClearCardSuit, int nClearCardPoint)
	{
		XLDT_BET_PRIZE_TYPE prizeTyp = XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL;
		switch (nClearCard)
		{
		case 0:
			prizeTyp = XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL;
			break;
		case 1:
			switch (nClearCardSuit)
			{
			case 1:
			case 3:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_RED;
				break;
			case 0:
			case 2:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_BLACK;
				break;
			}
			break;
		case 2:
			switch (nClearCardSuit)
			{
			case 0:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_SPADE;
				break;
			case 1:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_HEART;
				break;
			case 2:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_CLUB;
				break;
			case 3:
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_DIAMOND;
				break;
			}
			if (nClearCardPoint >= 14)
			{
				prizeTyp = XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_PORKER_JOKER;
			}
			break;
		}
		mBetCtrl.Restart(prizeTyp);
		mUserList.Restart();
	}

	public void ShowDlg(bool isShow, XLDT_POP_DLG_TYPE dlgType)
	{
		if (isShow)
		{
			if (mCurDlgType == XLDT_POP_DLG_TYPE.DLG_NONE)
			{
				mCurDlgType = dlgType;
				ShowMoneyInOutDlg();
			}
		}
		else
		{
			sptDlgBase.PopBack();
		}
	}

	public void OnDlgBlackClick(object dlgType = null)
	{
		switch (mCurDlgType)
		{
		case XLDT_POP_DLG_TYPE.DLG_SETTING:
			ShowSetDlg(isShow: false);
			break;
		case XLDT_POP_DLG_TYPE.DLG_MONEY_INOUT:
			ShowMoneyInOutDlg(isShow: false);
			break;
		case XLDT_POP_DLG_TYPE.DLG_PERSON_INFO:
			ShowPersonInfoDlg(isShow: false, 0, string.Empty, 0, 0, 0, string.Empty);
			break;
		case XLDT_POP_DLG_TYPE.DLG_CHAT:
			ShowChatDlg(isShow: false, isPrivateChat: false, string.Empty);
			break;
		}
	}

	public bool IsUserScoreEnough(int scoreValue)
	{
		return mGameInfo.User.ScoreCount >= scoreValue;
	}

	public void updateSignal(string sLevel)
	{
		XLDT_ShowScene.getInstance().updateSignal(sLevel);
	}

	public void SetCancelYaFenBtnEnable(int type)
	{
		if (XLDT_GameInfo.getInstance().Language == 0)
		{
			XLDT_GameManager.getInstance().setLanguage(isCN: true);
		}
		else
		{
			XLDT_GameManager.getInstance().setLanguage(isCN: false);
		}
		mBetCtrl.SetCancelYaFenBtnEnable(type);
	}

	public void EnterGame()
	{
		objRoom.SetActive(value: false);
		objGame.SetActive(value: true);
		objBtnList.SetActive(value: true);
		mUserList.gameObject.SetActive(value: true);
		objDark.SetActive(value: true);
		StartCoroutine("StartGame");
	}

	public void LeaveGame()
	{
		objRoom.SetActive(value: true);
		objGame.SetActive(value: false);
		objBtnList.SetActive(value: false);
		mUserList.gameObject.SetActive(value: false);
		objDark.SetActive(value: false);
		StopCoroutine("StartGame");
	}
}
