using GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DK_TipManager
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

	public DK_TipType mTipType = DK_TipType.NoneTip;

	public bool isSomethingError;

	private static DK_TipManager instance;

	public static DK_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new DK_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = DK_GameInfo.getInstance().Language;
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
		if (DK_GameInfo.getInstance().currentState != DK_GameState.On_Game)
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

	public void ShowTip(DK_TipType eType, int coins = 0, string msg = "")
	{
		if (mTipType == eType)
		{
			return;
		}
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.ClickBG();
		}
		else if (DK_GameInfo.getInstance().currentState != 0)
		{
			DK_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
		}
		mTipType = eType;
		objFrame.SetActive(value: true);
		btnCancel.gameObject.SetActive(value: false);
		btnConfirm.gameObject.SetActive(value: true);
		if (msg != string.Empty)
		{
			txtTip.text = msg;
			btnCancel.gameObject.SetActive(value: true);
			return;
		}
		switch (eType)
		{
		case DK_TipType.OpenToUp:
			txtTip.text = msg;
			btnCancel.gameObject.SetActive(value: true);
			break;
		case DK_TipType.Custom:
			txtTip.text = msg;
			break;
		case DK_TipType.QuitToDesk:
			txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
			break;
		case DK_TipType.Net_ConnectionError:
			txtTip.text = DK_TipContent.contents[language][0];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.LoseTheServer:
			txtTip.text = DK_TipContent.contents[language][1];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.CoinOverFlow:
			txtTip.text = DK_TipContent.contents[language][2];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.ApplyForExpCoin_Success:
			txtTip.text = DK_TipContent.contents[language][4];
			break;
		case DK_TipType.SelectTable_CreditBelowRistrict:
			txtTip.text = DK_TipContent.contents[language][9];
			break;
		case DK_TipType.SelectTable_SendExpCoin:
			txtTip.text = DK_TipContent.contents[language][11];
			break;
		case DK_TipType.SelectSeat_NotEmpty:
			txtTip.text = DK_TipContent.contents[language][10];
			break;
		case DK_TipType.RoomEmpty:
			txtTip.text = DK_TipContent.contents[language][3];
			break;
		case DK_TipType.IsExitGame:
			txtTip.text = DK_TipContent.contents[language][20];
			btnCancel.gameObject.SetActive(value: true);
			btnConfirm.gameObject.SetActive(value: true);
			break;
		case DK_TipType.TableDeleted:
			txtTip.text = DK_TipContent.contents[language][7];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.TableConfigChanged:
			txtTip.text = DK_TipContent.contents[language][8];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.ServerUpdate:
			txtTip.text = DK_TipContent.contents[language][13];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.Game_UserIdFrozen:
			txtTip.text = DK_TipContent.contents[language][14];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.UserIdPwdNotMatch:
			txtTip.text = DK_TipContent.contents[language][15];
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.UserIdDeleted:
			txtTip.text = DK_TipContent.contents[language][16];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.LongTimeNoHandle:
			txtTip.text = DK_TipContent.contents[language][12];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.UserIdRepeative:
			txtTip.text = DK_TipContent.contents[language][17];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.UserPwdChanged:
			txtTip.text = DK_TipContent.contents[language][18];
			isSomethingError = true;
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.error);
			break;
		case DK_TipType.GameShutUp:
			txtTip.text = DK_TipContent.contents[language][5];
			break;
		case DK_TipType.UserShutUp:
			txtTip.text = DK_TipContent.contents[language][6];
			break;
		case DK_TipType.GivingCoin:
			txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
			break;
		case DK_TipType.IsExitApplication:
			txtTip.text = DK_TipContent.contents[language][19];
			btnCancel.gameObject.SetActive(value: true);
			btnConfirm.gameObject.SetActive(value: true);
			break;
		}
	}

	public void ClickBlack()
	{
		UnityEngine.Debug.LogError("退出房间: " + roomId);
		DK_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
		ZH2_GVars.isStartedFromGame = true;
		DK_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		BackToHall();
	}

	public void ClickBtnConfirm()
	{
		DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.common);
		switch (mTipType)
		{
		case DK_TipType.OpenToUp:
			UnityEngine.Debug.LogError("=====打开充值面板====");
			DK_GameInfo.getInstance().GameScene.ShowInOut2();
			break;
		case DK_TipType.Net_ConnectionError:
		case DK_TipType.LoseTheServer:
		case DK_TipType.CoinOverFlow:
		case DK_TipType.ServerUpdate:
		case DK_TipType.Game_UserIdFrozen:
		case DK_TipType.UserIdPwdNotMatch:
		case DK_TipType.UserIdDeleted:
		case DK_TipType.UserIdRepeative:
		case DK_TipType.UserPwdChanged:
		case DK_TipType.IsExitApplication:
			UnityEngine.Debug.LogError("退出房间: " + roomId);
			DK_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
			ZH2_GVars.isStartedFromGame = true;
			DK_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			BackToHall();
			break;
		case DK_TipType.SelectTable_SendExpCoin:
			DK_NetMngr.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
			break;
		case DK_TipType.IsExitGame:
			DK_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(DK_GameInfo.getInstance().Table.Id, DK_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STDK_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DK_TipType.QuitToDesk:
			SceneManager.LoadScene("STDK_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DK_TipType.TableDeleted:
		case DK_TipType.TableConfigChanged:
		case DK_TipType.LongTimeNoHandle:
			if (DK_GameInfo.getInstance().currentState == DK_GameState.On_SelectTable)
			{
				DK_GameInfo.getInstance().UIScene.ClickBtnBack();
				break;
			}
			SceneManager.LoadScene("STDK_UIScene");
			ZH2_GVars.isGameToDaTing = true;
			ZH2_GVars.isFirstToDaTing = false;
			break;
		case DK_TipType.GivingCoin:
			DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.getCoin);
			break;
		case DK_TipType.SelectTable_CreditBelowRistrict:
		case DK_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			DK_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: true);
			DK_GameInfo.getInstance().UIScene.ShowTable();
			break;
		}
		if (mTipType != DK_TipType.NoneTip)
		{
			mTipType = DK_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.common);
		if (mTipType != DK_TipType.NoneTip)
		{
			mTipType = DK_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("DemonKing");
		SceneManager.LoadScene("MainScene");
	}
}
