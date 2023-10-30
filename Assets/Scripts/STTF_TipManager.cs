using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STTF_TipManager
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

	public STTF_TipType mTipType = STTF_TipType.NoneTip;

	public bool isSomethingError;

	private static STTF_TipManager instance;

	public static STTF_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new STTF_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = STTF_GameInfo.getInstance().Language;
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
		if (STTF_GameInfo.getInstance().currentState != STTF_GameState.On_Game)
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

	public void ShowTip(STTF_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
			{
				STTF_GameInfo.getInstance().GameScene.ClickBG();
			}
			else if (STTF_GameInfo.getInstance().currentState != 0)
			{
				STTF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case STTF_TipType.OpenToUp:
				txtTip.text = msg;
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STTF_TipType.Custom:
				txtTip.text = msg;
				break;
			case STTF_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case STTF_TipType.Net_ConnectionError:
				txtTip.text = STTF_TipContent.contents[language][0];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.LoseTheServer:
				txtTip.text = STTF_TipContent.contents[language][1];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.CoinOverFlow:
				txtTip.text = STTF_TipContent.contents[language][2];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.ApplyForExpCoin_Success:
				txtTip.text = STTF_TipContent.contents[language][4];
				break;
			case STTF_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = STTF_TipContent.contents[language][9];
				break;
			case STTF_TipType.SelectTable_SendExpCoin:
				txtTip.text = STTF_TipContent.contents[language][11];
				break;
			case STTF_TipType.SelectSeat_NotEmpty:
				txtTip.text = STTF_TipContent.contents[language][10];
				break;
			case STTF_TipType.RoomEmpty:
				txtTip.text = STTF_TipContent.contents[language][3];
				break;
			case STTF_TipType.IsExitGame:
				txtTip.text = STTF_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STTF_TipType.TableDeleted:
				txtTip.text = STTF_TipContent.contents[language][7];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.TableConfigChanged:
				txtTip.text = STTF_TipContent.contents[language][8];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.ServerUpdate:
				txtTip.text = STTF_TipContent.contents[language][13];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.Game_UserIdFrozen:
				txtTip.text = STTF_TipContent.contents[language][14];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.UserIdPwdNotMatch:
				txtTip.text = STTF_TipContent.contents[language][15];
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.UserIdDeleted:
				txtTip.text = STTF_TipContent.contents[language][16];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.LongTimeNoHandle:
				txtTip.text = STTF_TipContent.contents[language][12];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.UserIdRepeative:
				txtTip.text = STTF_TipContent.contents[language][17];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.UserPwdChanged:
				txtTip.text = STTF_TipContent.contents[language][18];
				isSomethingError = true;
				STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.error);
				break;
			case STTF_TipType.GameShutUp:
				txtTip.text = STTF_TipContent.contents[language][5];
				break;
			case STTF_TipType.UserShutUp:
				txtTip.text = STTF_TipContent.contents[language][6];
				break;
			case STTF_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case STTF_TipType.IsExitApplication:
				txtTip.text = STTF_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBack()
	{
		UnityEngine.Debug.LogError("退出房间: " + roomId);
		ZH2_GVars.isStartedFromGame = true;
		STTF_GameInfo.getInstance().ClearGameInfo();
		STTF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		BackToHall();
	}

	public void ClickBtnConfirm()
	{
		STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.common);
		switch (mTipType)
		{
		case STTF_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			STTF_GameInfo.getInstance().GameScene.ShowInOut2();
			break;
		case STTF_TipType.Net_ConnectionError:
		case STTF_TipType.LoseTheServer:
		case STTF_TipType.CoinOverFlow:
		case STTF_TipType.ServerUpdate:
		case STTF_TipType.Game_UserIdFrozen:
		case STTF_TipType.UserIdPwdNotMatch:
		case STTF_TipType.UserIdDeleted:
		case STTF_TipType.UserIdRepeative:
		case STTF_TipType.UserPwdChanged:
		case STTF_TipType.IsExitApplication:
			UnityEngine.Debug.LogError("退出房间: " + roomId);
			ZH2_GVars.isStartedFromGame = true;
			STTF_GameInfo.getInstance().ClearGameInfo();
			STTF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			BackToHall();
			break;
		case STTF_TipType.SelectTable_SendExpCoin:
			STTF_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case STTF_TipType.IsExitGame:
			STTF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			STTF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STTF_GameInfo.getInstance().Table.Id, STTF_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STTF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STTF_TipType.QuitToDesk:
			SceneManager.LoadScene("STTF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STTF_TipType.TableDeleted:
		case STTF_TipType.TableConfigChanged:
		case STTF_TipType.LongTimeNoHandle:
			if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_SelectTable)
			{
				STTF_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("STTF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STTF_TipType.GivingCoin:
			STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.getCoin);
			break;
		case STTF_TipType.SelectTable_CreditBelowRistrict:
		case STTF_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			STTF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			STTF_GameInfo.getInstance().UIScene.ShowTable(0f);
			break;
		}
		if (mTipType != STTF_TipType.NoneTip)
		{
			mTipType = STTF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.common);
		if (mTipType != STTF_TipType.NoneTip)
		{
			mTipType = STTF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("ToadFish");
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}
}
