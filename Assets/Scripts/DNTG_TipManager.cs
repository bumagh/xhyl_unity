using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DNTG_TipManager
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

	public DNTG_TipType mTipType = DNTG_TipType.NoneTip;

	public bool isSomethingError;

	private static DNTG_TipManager instance;

	public static DNTG_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new DNTG_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = DNTG_GameInfo.getInstance().Language;
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
		if (DNTG_GameInfo.getInstance().currentState != DNTG_GameState.On_Game)
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

	public void ShowTip(DNTG_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
			{
				DNTG_GameInfo.getInstance().GameScene.ClickBG(bShow: false);
			}
			else if (DNTG_GameInfo.getInstance().currentState != 0)
			{
				DNTG_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case DNTG_TipType.Custom:
				txtTip.text = msg;
				break;
			case DNTG_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case DNTG_TipType.Net_ConnectionError:
				txtTip.text = DNTG_TipContent.contents[language][0];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.LoseTheServer:
				txtTip.text = DNTG_TipContent.contents[language][1];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.CoinOverFlow:
				txtTip.text = DNTG_TipContent.contents[language][2];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.ApplyForExpCoin_Success:
				txtTip.text = DNTG_TipContent.contents[language][4];
				break;
			case DNTG_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = DNTG_TipContent.contents[language][9];
				break;
			case DNTG_TipType.SelectTable_SendExpCoin:
				txtTip.text = DNTG_TipContent.contents[language][11];
				break;
			case DNTG_TipType.SelectSeat_NotEmpty:
				txtTip.text = DNTG_TipContent.contents[language][10];
				break;
			case DNTG_TipType.RoomEmpty:
				txtTip.text = DNTG_TipContent.contents[language][3];
				break;
			case DNTG_TipType.IsExitGame:
				txtTip.text = DNTG_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case DNTG_TipType.TableDeleted:
				txtTip.text = DNTG_TipContent.contents[language][7];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.TableConfigChanged:
				txtTip.text = DNTG_TipContent.contents[language][8];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.ServerUpdate:
				txtTip.text = DNTG_TipContent.contents[language][13];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.Game_UserIdFrozen:
				txtTip.text = DNTG_TipContent.contents[language][14];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.UserIdPwdNotMatch:
				txtTip.text = DNTG_TipContent.contents[language][15];
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.UserIdDeleted:
				txtTip.text = DNTG_TipContent.contents[language][16];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.LongTimeNoHandle:
				txtTip.text = DNTG_TipContent.contents[language][12];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.UserIdRepeative:
				UnityEngine.Debug.LogError("====该账号已在别处登录，您被迫下线=====");
				txtTip.text = DNTG_TipContent.contents[language][17];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.UserPwdChanged:
				txtTip.text = DNTG_TipContent.contents[language][18];
				isSomethingError = true;
				DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.error);
				break;
			case DNTG_TipType.GameShutUp:
				txtTip.text = DNTG_TipContent.contents[language][5];
				break;
			case DNTG_TipType.UserShutUp:
				txtTip.text = DNTG_TipContent.contents[language][6];
				break;
			case DNTG_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case DNTG_TipType.IsExitApplication:
				txtTip.text = DNTG_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBlack()
	{
		if (DNTG_NetMngr.G_NetMngr != null)
		{
			DNTG_NetMngr.G_NetMngr.NetDestroy();
		}
		if (DNTG_SoundManage.getInstance() != null)
		{
			DNTG_SoundManage.getInstance().SoundManageDestroy();
		}
		DNTG_GameInfo.getInstance().LoadStep = DNTG_LoadType.On_ConnectNet;
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadSceneAsync(0);
	}

	public void ClickBtnConfirm()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		switch (mTipType)
		{
		case DNTG_TipType.Net_ConnectionError:
		case DNTG_TipType.LoseTheServer:
		case DNTG_TipType.CoinOverFlow:
		case DNTG_TipType.ServerUpdate:
		case DNTG_TipType.Game_UserIdFrozen:
		case DNTG_TipType.UserIdPwdNotMatch:
		case DNTG_TipType.UserIdDeleted:
		case DNTG_TipType.UserIdRepeative:
		case DNTG_TipType.UserPwdChanged:
		case DNTG_TipType.IsExitApplication:
			if (mTipType == DNTG_TipType.IsExitApplication || mTipType == DNTG_TipType.CoinOverFlow || mTipType == DNTG_TipType.Net_ConnectionError || mTipType == DNTG_TipType.LoseTheServer)
			{
				if (DNTG_NetMngr.G_NetMngr != null)
				{
					DNTG_NetMngr.G_NetMngr.NetDestroy();
				}
				if (DNTG_SoundManage.getInstance() != null)
				{
					DNTG_SoundManage.getInstance().SoundManageDestroy();
				}
				DNTG_GameInfo.getInstance().LoadStep = DNTG_LoadType.On_ConnectNet;
				ZH2_GVars.isStartedFromGame = true;
			}
			BackToHall();
			break;
		case DNTG_TipType.SelectTable_SendExpCoin:
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case DNTG_TipType.IsExitGame:
			UnityEngine.Debug.LogError("============退出游戏=============");
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(DNTG_GameInfo.getInstance().Table.Id, DNTG_GameInfo.getInstance().User.SeatIndex);
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			SceneManager.LoadScene("DNTG_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DNTG_TipType.QuitToDesk:
			UnityEngine.Debug.LogError("============退出游戏=============");
			SceneManager.LoadScene("DNTG_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DNTG_TipType.TableDeleted:
		case DNTG_TipType.TableConfigChanged:
		case DNTG_TipType.LongTimeNoHandle:
			if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_SelectTable)
			{
				DNTG_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("DNTG_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DNTG_TipType.GivingCoin:
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.getCoin);
			break;
		case DNTG_TipType.SelectTable_CreditBelowRistrict:
		case DNTG_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			DNTG_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			DNTG_GameInfo.getInstance().UIScene.ShowTable(0f);
			break;
		}
		if (mTipType != DNTG_TipType.NoneTip)
		{
			mTipType = DNTG_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		if (mTipType != DNTG_TipType.NoneTip)
		{
			mTipType = DNTG_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("DNTianG");
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}
}
