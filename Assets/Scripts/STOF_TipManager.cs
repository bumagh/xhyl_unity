using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STOF_TipManager
{
	private GameObject objFrame;

	private Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private Button btnCancelX;

	private Text txtConfirm;

	private Text txtCancel;

	private int language;

	public int roomId = 1;

	public STOF_TipType mTipType = STOF_TipType.NoneTip;

	public bool isSomethingError;

	private static STOF_TipManager instance;

	public static STOF_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new STOF_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = STOF_GameInfo.getInstance().Language;
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
		if (STOF_GameInfo.getInstance().currentState != STOF_GameState.On_Game)
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

	public void ShowTip(STOF_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (STOF_GameInfo.getInstance().currentState == STOF_GameState.On_Game)
			{
				STOF_GameInfo.getInstance().GameScene.ClickBG(bShow: false);
			}
			else if (STOF_GameInfo.getInstance().currentState != 0)
			{
				STOF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case STOF_TipType.OpenToUp:
				txtTip.text = msg;
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STOF_TipType.Custom:
				txtTip.text = msg;
				break;
			case STOF_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case STOF_TipType.Net_ConnectionError:
				txtTip.text = STOF_TipContent.contents[language][0];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.LoseTheServer:
				txtTip.text = STOF_TipContent.contents[language][1];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.CoinOverFlow:
				txtTip.text = STOF_TipContent.contents[language][2];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.ApplyForExpCoin_Success:
				txtTip.text = STOF_TipContent.contents[language][4];
				break;
			case STOF_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = STOF_TipContent.contents[language][9];
				break;
			case STOF_TipType.SelectTable_SendExpCoin:
				txtTip.text = STOF_TipContent.contents[language][11];
				break;
			case STOF_TipType.SelectSeat_NotEmpty:
				txtTip.text = STOF_TipContent.contents[language][10];
				break;
			case STOF_TipType.RoomEmpty:
				txtTip.text = STOF_TipContent.contents[language][3];
				break;
			case STOF_TipType.IsExitGame:
				txtTip.text = STOF_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STOF_TipType.TableDeleted:
				txtTip.text = STOF_TipContent.contents[language][7];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.TableConfigChanged:
				txtTip.text = STOF_TipContent.contents[language][8];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.ServerUpdate:
				txtTip.text = STOF_TipContent.contents[language][13];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.Game_UserIdFrozen:
				txtTip.text = STOF_TipContent.contents[language][14];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.UserIdPwdNotMatch:
				txtTip.text = STOF_TipContent.contents[language][15];
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.UserIdDeleted:
				txtTip.text = STOF_TipContent.contents[language][16];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.LongTimeNoHandle:
				txtTip.text = STOF_TipContent.contents[language][12];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.UserIdRepeative:
				txtTip.text = STOF_TipContent.contents[language][17];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.UserPwdChanged:
				txtTip.text = STOF_TipContent.contents[language][18];
				isSomethingError = true;
				STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
				break;
			case STOF_TipType.GameShutUp:
				txtTip.text = STOF_TipContent.contents[language][5];
				break;
			case STOF_TipType.UserShutUp:
				txtTip.text = STOF_TipContent.contents[language][6];
				break;
			case STOF_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case STOF_TipType.IsExitApplication:
				txtTip.text = STOF_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBlack()
	{
		if (STOF_NetMngr.G_NetMngr != null)
		{
			STOF_NetMngr.G_NetMngr.NetDestroy();
		}
		if (STOF_SoundManage.getInstance() != null)
		{
			STOF_SoundManage.getInstance().SoundManageDestroy();
		}
		STOF_GameInfo.getInstance().LoadStep = STOF_LoadType.On_ConnectNet;
		ZH2_GVars.isStartedFromGame = true;
		BackToHall();
	}

	public void ClickBtnConfirm()
	{
		STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.common);
		switch (mTipType)
		{
		case STOF_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			STOF_GameInfo.getInstance().GameScene.ShowInOut2();
			break;
		case STOF_TipType.Net_ConnectionError:
		case STOF_TipType.LoseTheServer:
		case STOF_TipType.CoinOverFlow:
		case STOF_TipType.ServerUpdate:
		case STOF_TipType.Game_UserIdFrozen:
		case STOF_TipType.UserIdPwdNotMatch:
		case STOF_TipType.UserIdDeleted:
		case STOF_TipType.UserIdRepeative:
		case STOF_TipType.UserPwdChanged:
		case STOF_TipType.IsExitApplication:
			if (mTipType == STOF_TipType.IsExitApplication || mTipType == STOF_TipType.CoinOverFlow || mTipType == STOF_TipType.Net_ConnectionError || mTipType == STOF_TipType.LoseTheServer)
			{
				if (STOF_NetMngr.G_NetMngr != null)
				{
					STOF_NetMngr.G_NetMngr.NetDestroy();
				}
				if (STOF_SoundManage.getInstance() != null)
				{
					STOF_SoundManage.getInstance().SoundManageDestroy();
				}
				STOF_GameInfo.getInstance().LoadStep = STOF_LoadType.On_ConnectNet;
				ZH2_GVars.isStartedFromGame = true;
			}
			BackToHall();
			break;
		case STOF_TipType.SelectTable_SendExpCoin:
			STOF_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case STOF_TipType.IsExitGame:
			UnityEngine.Debug.LogError("============退出游戏=============");
			STOF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			STOF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STOF_GameInfo.getInstance().Table.Id, STOF_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STOF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STOF_TipType.QuitToDesk:
			UnityEngine.Debug.LogError("============退出游戏=============");
			SceneManager.LoadScene("STOF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STOF_TipType.TableDeleted:
		case STOF_TipType.TableConfigChanged:
		case STOF_TipType.LongTimeNoHandle:
			if (STOF_GameInfo.getInstance().currentState == STOF_GameState.On_SelectTable)
			{
				STOF_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("STOF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STOF_TipType.GivingCoin:
			STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.getCoin);
			break;
		case STOF_TipType.SelectTable_CreditBelowRistrict:
		case STOF_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			STOF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			STOF_GameInfo.getInstance().UIScene.ShowTable(0f);
			break;
		}
		if (mTipType != STOF_TipType.NoneTip)
		{
			mTipType = STOF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.common);
		if (mTipType != STOF_TipType.NoneTip)
		{
			mTipType = STOF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("OverFish");
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}
}
