using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STQM_TipManager
{
	private GameObject objFrame;

	private Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private Button btnCancelX;

	private Text txtConfirm;

	private Text txtCancel;

	private int language;

	public STQM_TipType mTipType = STQM_TipType.NoneTip;

	public bool isSomethingError;

	private static STQM_TipManager instance;

	public static STQM_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new STQM_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = STQM_GameInfo.getInstance().Language;
		isSomethingError = false;
		objFrame = GameObject.Find("TipDialog_New");
		txtTip = objFrame.transform.Find("ImgBg/TxtTip").GetComponent<Text>();
		btnConfirm = objFrame.transform.Find("ImgBg/Btns/BtnConfirm").GetComponent<Button>();
		btnCancel = objFrame.transform.Find("ImgBg/Btns/BtnCancel").GetComponent<Button>();
		Transform transform = objFrame.transform.Find("ImgBg/BtnX");
		if (transform != null)
		{
			btnCancelX = transform.GetComponent<Button>();
		}
		if (STQM_GameInfo.getInstance().currentState != STQM_GameState.On_Game)
		{
			txtConfirm = btnConfirm.transform.Find("Text").GetComponent<Text>();
			if (txtConfirm != null)
			{
				txtConfirm.text = ((language != 0) ? "Continue" : "确定");
			}
			txtCancel = btnCancel.transform.Find("Text").GetComponent<Text>();
			if (txtCancel != null)
			{
				txtCancel.text = ((language != 0) ? "Cancel" : "取消");
			}
		}
		objFrame.SetActive(value: false);
		btnConfirm.onClick.AddListener(ClickBtnConfirm);
		btnCancel.onClick.AddListener(ClickBtnCancel);
		if (btnCancelX != null)
		{
			btnCancelX.onClick.AddListener(ClickBtnCancel);
		}
	}

	public void ShowTip(STQM_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
			{
				STQM_GameInfo.getInstance().GameScene.ClickBG();
			}
			else if (STQM_GameInfo.getInstance().currentState != 0)
			{
				STQM_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case STQM_TipType.OpenToUp:
				txtTip.text = msg;
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STQM_TipType.Custom:
				txtTip.text = msg;
				break;
			case STQM_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case STQM_TipType.Net_ConnectionError:
				txtTip.text = STQM_TipContent.contents[language][0];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.LoseTheServer:
				txtTip.text = STQM_TipContent.contents[language][1];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.CoinOverFlow:
				txtTip.text = STQM_TipContent.contents[language][2];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.ApplyForExpCoin_Success:
				txtTip.text = STQM_TipContent.contents[language][4];
				break;
			case STQM_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = STQM_TipContent.contents[language][9];
				break;
			case STQM_TipType.SelectTable_SendExpCoin:
				txtTip.text = STQM_TipContent.contents[language][11];
				break;
			case STQM_TipType.SelectSeat_NotEmpty:
				txtTip.text = STQM_TipContent.contents[language][10];
				break;
			case STQM_TipType.RoomEmpty:
				txtTip.text = STQM_TipContent.contents[language][3];
				break;
			case STQM_TipType.IsExitGame:
				txtTip.text = STQM_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STQM_TipType.TableDeleted:
				txtTip.text = STQM_TipContent.contents[language][7];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.TableConfigChanged:
				txtTip.text = STQM_TipContent.contents[language][8];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.ServerUpdate:
				txtTip.text = STQM_TipContent.contents[language][13];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.Game_UserIdFrozen:
				txtTip.text = STQM_TipContent.contents[language][14];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.UserIdPwdNotMatch:
				txtTip.text = STQM_TipContent.contents[language][15];
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.UserIdDeleted:
				txtTip.text = STQM_TipContent.contents[language][16];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.LongTimeNoHandle:
				txtTip.text = STQM_TipContent.contents[language][12];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.UserIdRepeative:
				txtTip.text = STQM_TipContent.contents[language][17];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.UserPwdChanged:
				txtTip.text = STQM_TipContent.contents[language][18];
				isSomethingError = true;
				STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.error);
				break;
			case STQM_TipType.GameShutUp:
				txtTip.text = STQM_TipContent.contents[language][5];
				break;
			case STQM_TipType.UserShutUp:
				txtTip.text = STQM_TipContent.contents[language][6];
				break;
			case STQM_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case STQM_TipType.IsExitApplication:
				txtTip.text = STQM_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBlack()
	{
		ZH2_GVars.isStartedFromGame = true;
		STQM_GameInfo.getInstance().ClearGameInfo();
		STQM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		BackToHall();
	}

	public void ClickBtnConfirm()
	{
		STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.common);
		switch (mTipType)
		{
		case STQM_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			STQM_GameInfo.getInstance().GameScene.ShowInOut2();
			break;
		case STQM_TipType.Net_ConnectionError:
		case STQM_TipType.LoseTheServer:
		case STQM_TipType.CoinOverFlow:
		case STQM_TipType.ServerUpdate:
		case STQM_TipType.Game_UserIdFrozen:
		case STQM_TipType.UserIdPwdNotMatch:
		case STQM_TipType.UserIdDeleted:
		case STQM_TipType.UserIdRepeative:
		case STQM_TipType.UserPwdChanged:
		case STQM_TipType.IsExitApplication:
			if (mTipType == STQM_TipType.IsExitApplication || mTipType == STQM_TipType.CoinOverFlow || mTipType == STQM_TipType.Net_ConnectionError || mTipType == STQM_TipType.LoseTheServer)
			{
				ZH2_GVars.isStartedFromGame = true;
			}
			else
			{
				ZH2_GVars.isStartedFromGame = false;
			}
			STQM_GameInfo.getInstance().ClearGameInfo();
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			BackToHall();
			break;
		case STQM_TipType.SelectTable_SendExpCoin:
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case STQM_TipType.IsExitGame:
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(STQM_GameInfo.getInstance().User.RoomId);
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STQM_GameInfo.getInstance().Table.Id, STQM_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STQM_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STQM_TipType.QuitToDesk:
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(STQM_GameInfo.getInstance().User.RoomId);
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STQM_GameInfo.getInstance().Table.Id, STQM_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STQM_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STQM_TipType.TableDeleted:
		case STQM_TipType.TableConfigChanged:
		case STQM_TipType.LongTimeNoHandle:
			if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_SelectTable)
			{
				STQM_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("STQM_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STQM_TipType.GivingCoin:
			STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.getCoin);
			break;
		case STQM_TipType.SelectTable_CreditBelowRistrict:
		case STQM_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			STQM_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			STQM_GameInfo.getInstance().UIScene.ShowTable();
			break;
		}
		if (mTipType != STQM_TipType.NoneTip)
		{
			mTipType = STQM_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.common);
		if (mTipType != STQM_TipType.NoneTip)
		{
			mTipType = STQM_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("QQMermaid");
		SceneManager.LoadScene("MainScene");
	}
}
