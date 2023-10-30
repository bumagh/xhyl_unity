using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STMF_TipManager
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

	public STMF_TipType mTipType = STMF_TipType.NoneTip;

	public bool isSomethingError;

	private static STMF_TipManager instance;

	public static STMF_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new STMF_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = STMF_GameInfo.getInstance().Language;
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
		if (STMF_GameInfo.getInstance().currentState != STMF_GameState.On_Game)
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

	public void ClickBtnBack()
	{
		UnityEngine.Debug.LogError("退出房间: " + roomId);
		ZH2_GVars.isStartedFromGame = true;
		STMF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		STMF_GameInfo.ClearGameInfo();
		BackToHall();
	}

	public void ShowTip(STMF_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType != eType)
		{
			if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
			{
				STMF_GameInfo.getInstance().GameScene.ClickBG();
			}
			else if (STMF_GameInfo.getInstance().currentState != 0)
			{
				STMF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case STMF_TipType.OpenToUp:
				txtTip.text = msg;
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STMF_TipType.Custom:
				txtTip.text = msg;
				break;
			case STMF_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case STMF_TipType.Net_ConnectionError:
				txtTip.text = STMF_TipContent.contents[language][0];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.LoseTheServer:
				txtTip.text = STMF_TipContent.contents[language][1];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.CoinOverFlow:
				txtTip.text = STMF_TipContent.contents[language][2];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.ApplyForExpCoin_Success:
				txtTip.text = STMF_TipContent.contents[language][4];
				break;
			case STMF_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = STMF_TipContent.contents[language][9];
				break;
			case STMF_TipType.SelectTable_SendExpCoin:
				txtTip.text = STMF_TipContent.contents[language][11];
				break;
			case STMF_TipType.SelectSeat_NotEmpty:
				txtTip.text = STMF_TipContent.contents[language][10];
				break;
			case STMF_TipType.RoomEmpty:
				txtTip.text = STMF_TipContent.contents[language][3];
				break;
			case STMF_TipType.IsExitGame:
				txtTip.text = STMF_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case STMF_TipType.TableDeleted:
				txtTip.text = STMF_TipContent.contents[language][7];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.TableConfigChanged:
				txtTip.text = STMF_TipContent.contents[language][8];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.ServerUpdate:
				txtTip.text = STMF_TipContent.contents[language][13];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.Game_UserIdFrozen:
				txtTip.text = STMF_TipContent.contents[language][14];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.UserIdPwdNotMatch:
				txtTip.text = STMF_TipContent.contents[language][15];
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.UserIdDeleted:
				txtTip.text = STMF_TipContent.contents[language][16];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.LongTimeNoHandle:
				txtTip.text = STMF_TipContent.contents[language][12];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.UserIdRepeative:
				txtTip.text = STMF_TipContent.contents[language][17];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.UserPwdChanged:
				txtTip.text = STMF_TipContent.contents[language][18];
				isSomethingError = true;
				STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.error);
				break;
			case STMF_TipType.GameShutUp:
				txtTip.text = STMF_TipContent.contents[language][5];
				break;
			case STMF_TipType.UserShutUp:
				txtTip.text = STMF_TipContent.contents[language][6];
				break;
			case STMF_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case STMF_TipType.IsExitApplication:
				txtTip.text = STMF_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClickBtnConfirm()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common);
		switch (mTipType)
		{
		case STMF_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			STMF_GameInfo.getInstance().GameScene.ShowInOut2();
			break;
		case STMF_TipType.Net_ConnectionError:
		case STMF_TipType.LoseTheServer:
		case STMF_TipType.CoinOverFlow:
		case STMF_TipType.ServerUpdate:
		case STMF_TipType.Game_UserIdFrozen:
		case STMF_TipType.UserIdPwdNotMatch:
		case STMF_TipType.UserIdDeleted:
		case STMF_TipType.UserIdRepeative:
		case STMF_TipType.UserPwdChanged:
		case STMF_TipType.IsExitApplication:
			UnityEngine.Debug.LogError("退出房间: " + roomId);
			ZH2_GVars.isStartedFromGame = true;
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			STMF_GameInfo.ClearGameInfo();
			BackToHall();
			break;
		case STMF_TipType.SelectTable_SendExpCoin:
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case STMF_TipType.IsExitGame:
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STMF_GameInfo.getInstance().Table.Id, STMF_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STMF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STMF_TipType.QuitToDesk:
			SceneManager.LoadScene("STMF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STMF_TipType.TableDeleted:
		case STMF_TipType.TableConfigChanged:
		case STMF_TipType.LongTimeNoHandle:
			if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_SelectSeat)
			{
				STMF_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("STMF_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case STMF_TipType.GivingCoin:
			STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.getCoin);
			break;
		case STMF_TipType.SelectTable_CreditBelowRistrict:
		case STMF_TipType.SelectSeat_NotEmpty:
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(STMF_GameInfo.getInstance().Table.Id, STMF_GameInfo.getInstance().User.SeatIndex);
			STMF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			UnityEngine.Debug.LogError("座位有人");
			STMF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			break;
		}
		if (mTipType != STMF_TipType.NoneTip)
		{
			mTipType = STMF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.common);
		if (mTipType != STMF_TipType.NoneTip)
		{
			mTipType = STMF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("MoneyFish");
		SceneManager.LoadScene("MainScene");
	}
}
