using GameConfig;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TF_TipManager
{
	private GameObject objFrame;

	private Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private Text txtConfirm;

	private Text txtCancel;

	private int language;

	public TF_TipType mTipType = TF_TipType.NoneTip;

	public bool isSomethingError;

	private static TF_TipManager instance;

	public static TF_TipManager getInstance()
	{
		if (instance == null)
		{
			instance = new TF_TipManager();
		}
		return instance;
	}

	public void InitTip()
	{
		language = TF_GameInfo.getInstance().Language;
		isSomethingError = false;
		objFrame = GameObject.Find("TipDialog");
		txtTip = objFrame.transform.Find("TxtTip").GetComponent<Text>();
		btnConfirm = objFrame.transform.Find("BtnConfirm").GetComponent<Button>();
		btnCancel = objFrame.transform.Find("BtnCancel").GetComponent<Button>();
		if (TF_GameInfo.getInstance().currentState != TF_GameState.On_Game)
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
		try
		{
			GameObject gameObject = objFrame.transform.Find("BtnOk").gameObject;
			gameObject.SetActive(value: false);
		}
		catch (Exception)
		{
		}
	}

	public void ShowTip(TF_TipType eType, int coins = 0)
	{
		if (mTipType != eType)
		{
			if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
			{
				TF_GameInfo.getInstance().GameScene.ClickBG();
			}
			else if (TF_GameInfo.getInstance().currentState != 0)
			{
				TF_GameInfo.getInstance().UIScene.ClickBtnBG(bChangeState: false);
			}
			mTipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			btnConfirm.transform.localPosition = Vector3.down * 100f;
			switch (eType)
			{
			case TF_TipType.Net_ConnectionError:
				txtTip.text = TF_TipContent.contents[language][0];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.LoseTheServer:
				txtTip.text = TF_TipContent.contents[language][1];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.CoinOverFlow:
				txtTip.text = TF_TipContent.contents[language][2];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.ApplyForExpCoin_Success:
				txtTip.text = TF_TipContent.contents[language][4];
				break;
			case TF_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = TF_TipContent.contents[language][9];
				break;
			case TF_TipType.SelectTable_SendExpCoin:
				txtTip.text = TF_TipContent.contents[language][11];
				break;
			case TF_TipType.SelectSeat_NotEmpty:
				txtTip.text = TF_TipContent.contents[language][10];
				break;
			case TF_TipType.RoomEmpty:
				txtTip.text = TF_TipContent.contents[language][3];
				break;
			case TF_TipType.IsExitGame:
				txtTip.text = TF_TipContent.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				btnConfirm.transform.localPosition = Vector3.down * 100f + Vector3.left * 100f;
				break;
			case TF_TipType.TableDeleted:
				txtTip.text = TF_TipContent.contents[language][7];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.TableConfigChanged:
				txtTip.text = TF_TipContent.contents[language][8];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.ServerUpdate:
				txtTip.text = TF_TipContent.contents[language][13];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.Game_UserIdFrozen:
				txtTip.text = TF_TipContent.contents[language][14];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.UserIdPwdNotMatch:
				txtTip.text = TF_TipContent.contents[language][15];
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.UserIdDeleted:
				txtTip.text = TF_TipContent.contents[language][16];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.LongTimeNoHandle:
				txtTip.text = TF_TipContent.contents[language][12];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.UserIdRepeative:
				txtTip.text = TF_TipContent.contents[language][17];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.UserPwdChanged:
				txtTip.text = TF_TipContent.contents[language][18];
				isSomethingError = true;
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
				break;
			case TF_TipType.GameShutUp:
				txtTip.text = TF_TipContent.contents[language][5];
				break;
			case TF_TipType.UserShutUp:
				txtTip.text = TF_TipContent.contents[language][6];
				break;
			case TF_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case TF_TipType.IsExitApplication:
				txtTip.text = TF_TipContent.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				btnConfirm.transform.localPosition = Vector3.down * 100f + Vector3.left * 100f;
				break;
			case TF_TipType.GameMaintained:
				txtTip.text = TF_TipContent.contents[language][21];
				break;
			}
		}
	}

	public void ClickBtnConfirm()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		switch (mTipType)
		{
		case TF_TipType.Net_ConnectionError:
		case TF_TipType.LoseTheServer:
		case TF_TipType.CoinOverFlow:
		case TF_TipType.ServerUpdate:
		case TF_TipType.Game_UserIdFrozen:
		case TF_TipType.UserIdPwdNotMatch:
		case TF_TipType.UserIdDeleted:
		case TF_TipType.UserIdRepeative:
		case TF_TipType.UserPwdChanged:
		case TF_TipType.IsExitApplication:
			if (mTipType == TF_TipType.IsExitApplication || mTipType == TF_TipType.CoinOverFlow || mTipType == TF_TipType.Net_ConnectionError || mTipType == TF_TipType.LoseTheServer)
			{
				if (TF_NetMngr.G_NetMngr != null)
				{
					TF_NetMngr.G_NetMngr.NetDestroy();
				}
				if (TF_SoundManage.getInstance() != null)
				{
					TF_SoundManage.getInstance().SoundManageDestroy();
				}
				TF_GameInfo.getInstance().LoadStep = TF_LoadType.On_ConnectNet;
				ZH2_GVars.isStartedFromGame = true;
			}
			UnityEngine.Debug.Log("当前：" + mTipType);
			SceneManager.LoadSceneAsync(0);
			break;
		case TF_TipType.IsExitGame:
			TF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(TF_GameInfo.getInstance().Table.Id, TF_GameInfo.getInstance().User.SeatIndex);
			SceneManager.LoadScene("STTF_UIScene");
			break;
		case TF_TipType.TableDeleted:
		case TF_TipType.TableConfigChanged:
		case TF_TipType.LongTimeNoHandle:
			if (TF_GameInfo.getInstance().currentState == TF_GameState.On_SelectTable)
			{
				TF_GameInfo.getInstance().UIScene.ClickBtnBack();
			}
			else
			{
				SceneManager.LoadScene("STTF_UIScene");
			}
			break;
		case TF_TipType.GivingCoin:
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.getCoin);
			break;
		}
		if (mTipType != TF_TipType.NoneTip)
		{
			mTipType = TF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void ClickBtnCancel()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (mTipType != TF_TipType.NoneTip)
		{
			mTipType = TF_TipType.NoneTip;
			objFrame.SetActive(value: false);
		}
	}

	public void BackToHall()
	{
		TF_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		TF_GameInfo.getInstance().ClearGameInfo();
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("ToadFish");
		SceneManager.LoadScene("MainScene");
	}
}
