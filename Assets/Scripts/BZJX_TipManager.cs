using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BZJX_TipManager
{
	private GameObject objFrame;

	private Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private Text txtConfirm;

	private Text txtCancel;

	private int language;

	public int roomId = 1;

	public BZJX_TipType mTipType = BZJX_TipType.NoneTip;

	public bool isSomethingError;

	private static BZJX_TipManager instance;

	public static BZJX_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new BZJX_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = BZJX_GameInfo.getInstance().Language;
		isSomethingError = false;
		objFrame = GameObject.Find("TipDialog_New");
		txtTip = objFrame.transform.Find("ImgBg/TxtTip").GetComponent<Text>();
		btnConfirm = objFrame.transform.Find("ImgBg/Btns/BtnConfirm").GetComponent<Button>();
		btnCancel = objFrame.transform.Find("ImgBg/Btns/BtnCancel").GetComponent<Button>();
		if (BZJX_GameInfo.getInstance().currentState != BZJX_GameState.On_Game)
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
	}

	public void ShowTip(BZJX_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (BZJX_GameInfo.getInstance().currentState == BZJX_GameState.On_Game)
			{
				BZJX_GameInfo.getInstance().GameScene.ClickBG(bShow: false);
			}
			else if (BZJX_GameInfo.getInstance().currentState != 0)
			{
				BZJX_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case BZJX_TipType.OpenToUp:
				txtTip.text = msg;
				btnCancel.gameObject.SetActive(value: true);
				break;
			case BZJX_TipType.Custom:
				txtTip.text = msg;
				break;
			case BZJX_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case BZJX_TipType.Net_ConnectionError:
				txtTip.text = BZJX_TipContent.contents[language][0];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.LoseTheServer:
				txtTip.text = BZJX_TipContent.contents[language][1];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.CoinOverFlow:
				txtTip.text = BZJX_TipContent.contents[language][2];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.ApplyForExpCoin_Success:
				txtTip.text = BZJX_TipContent.contents[language][4];
				break;
			case BZJX_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = BZJX_TipContent.contents[language][9];
				break;
			case BZJX_TipType.SelectTable_SendExpCoin:
				txtTip.text = BZJX_TipContent.contents[language][11];
				break;
			case BZJX_TipType.SelectSeat_NotEmpty:
				txtTip.text = BZJX_TipContent.contents[language][10];
				break;
			case BZJX_TipType.RoomEmpty:
				txtTip.text = BZJX_TipContent.contents[language][3];
				break;
			case BZJX_TipType.IsExitGame:
				txtTip.text = BZJX_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case BZJX_TipType.TableDeleted:
				txtTip.text = BZJX_TipContent.contents[language][7];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.TableConfigChanged:
				txtTip.text = BZJX_TipContent.contents[language][8];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.ServerUpdate:
				txtTip.text = BZJX_TipContent.contents[language][13];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.Game_UserIdFrozen:
				txtTip.text = BZJX_TipContent.contents[language][14];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.UserIdPwdNotMatch:
				txtTip.text = BZJX_TipContent.contents[language][15];
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.UserIdDeleted:
				txtTip.text = BZJX_TipContent.contents[language][16];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.LongTimeNoHandle:
				txtTip.text = BZJX_TipContent.contents[language][12];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.UserIdRepeative:
				UnityEngine.Debug.LogError("====该账号已在别处登录，您被迫下线=====");
				txtTip.text = BZJX_TipContent.contents[language][17];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.UserPwdChanged:
				txtTip.text = BZJX_TipContent.contents[language][18];
				isSomethingError = true;
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.error);
				break;
			case BZJX_TipType.GameShutUp:
				txtTip.text = BZJX_TipContent.contents[language][5];
				break;
			case BZJX_TipType.UserShutUp:
				txtTip.text = BZJX_TipContent.contents[language][6];
				break;
			case BZJX_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case BZJX_TipType.IsExitApplication:
				txtTip.text = BZJX_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBlack()
	{
		if (BZJX_NetMngr.G_NetMngr != null)
		{
			BZJX_NetMngr.G_NetMngr.NetDestroy();
		}
		if (BZJX_SoundManage.getInstance() != null)
		{
			BZJX_SoundManage.getInstance().SoundManageDestroy();
		}
		BZJX_GameInfo.getInstance().LoadStep = BZJX_LoadType.On_ConnectNet;
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadSceneAsync(0);
	}

	public void ClickBtnConfirm()
	{
		BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.common);
		switch (mTipType)
		{
		case BZJX_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			BZJX_GameInfo.getInstance().GameScene.ShowTopUp2();
			break;
		case BZJX_TipType.Net_ConnectionError:
		case BZJX_TipType.LoseTheServer:
		case BZJX_TipType.CoinOverFlow:
		case BZJX_TipType.ServerUpdate:
		case BZJX_TipType.Game_UserIdFrozen:
		case BZJX_TipType.UserIdPwdNotMatch:
		case BZJX_TipType.UserIdDeleted:
		case BZJX_TipType.UserIdRepeative:
		case BZJX_TipType.UserPwdChanged:
		case BZJX_TipType.IsExitApplication:
			if (mTipType == BZJX_TipType.IsExitApplication || mTipType == BZJX_TipType.CoinOverFlow || mTipType == BZJX_TipType.Net_ConnectionError || mTipType == BZJX_TipType.LoseTheServer)
			{
				if (BZJX_NetMngr.G_NetMngr != null)
				{
					BZJX_NetMngr.G_NetMngr.NetDestroy();
				}
				if (BZJX_SoundManage.getInstance() != null)
				{
					BZJX_SoundManage.getInstance().SoundManageDestroy();
				}
				BZJX_GameInfo.getInstance().LoadStep = BZJX_LoadType.On_ConnectNet;
				ZH2_GVars.isStartedFromGame = true;
			}
			BackToHall();
			break;
		case BZJX_TipType.SelectTable_SendExpCoin:
			BZJX_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case BZJX_TipType.IsExitGame:
			UnityEngine.Debug.LogError("============退出游戏=============");
			BZJX_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(BZJX_GameInfo.getInstance().Table.Id, BZJX_GameInfo.getInstance().User.SeatIndex);
			BZJX_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			SceneManager.LoadScene("BZJX_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case BZJX_TipType.QuitToDesk:
			UnityEngine.Debug.LogError("============退出游戏=============");
			SceneManager.LoadScene("BZJX_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case BZJX_TipType.TableDeleted:
		case BZJX_TipType.TableConfigChanged:
		case BZJX_TipType.LongTimeNoHandle:
			if (BZJX_GameInfo.getInstance().currentState == BZJX_GameState.On_SelectTable)
			{
				BZJX_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("BZJX_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case BZJX_TipType.GivingCoin:
			BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.getCoin);
			break;
		case BZJX_TipType.SelectTable_CreditBelowRistrict:
		case BZJX_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			BZJX_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			BZJX_GameInfo.getInstance().UIScene.ShowTable(0f);
			break;
		}
		if (mTipType != BZJX_TipType.NoneTip)
		{
			mTipType = BZJX_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.common);
		if (mTipType != BZJX_TipType.NoneTip)
		{
			mTipType = BZJX_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("BZJXFish");
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}
}
