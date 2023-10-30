using STDT_GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XLDT_GameUITipMngr : MonoBehaviour
{
	[SerializeField]
	private GameObject objFrame;

	private Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private int localY;

	private int language;

	private XLDT_TipType tipType = XLDT_TipType.NoneTip;

	private static XLDT_GameUITipMngr instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		initTip();
	}

	public static XLDT_GameUITipMngr getInstance()
	{
		return instance;
	}

	public void initTip()
	{
		language = XLDT_GameInfo.getInstance().Language;
		localY = -90;
		txtTip = objFrame.transform.Find("ImgBg/TxtTip").GetComponent<Text>();
		btnConfirm = objFrame.transform.Find("ImgBg/Btns/BtnConfirm").GetComponent<Button>();
		btnCancel = objFrame.transform.Find("ImgBg/Btns/BtnCancel").GetComponent<Button>();
		objFrame.SetActive(value: false);
		btnConfirm.onClick.AddListener(delegate
		{
			clickButton(btnConfirm);
		});
		btnCancel.onClick.AddListener(delegate
		{
			clickButton(btnCancel);
		});
	}

	public void ShowTip(XLDT_TipType eType, int coins = 0, string msg = "")
	{
		if (tipType != eType)
		{
			XLDT_GameUIMngr.GetSingleton().OnDlgBlackClick();
			XLDT_GameUIMngr.GetSingleton().ShowTip(isShow: true);
			tipType = eType;
			objFrame.SetActive(value: true);
			btnCancel.gameObject.SetActive(value: false);
			switch (eType)
			{
			case XLDT_TipType.Custom:
				txtTip.text = msg;
				break;
			case XLDT_TipType.Net_ConnectionError:
				txtTip.text = XLDT_TipContentGame.contents[language][0];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.QuitToDesk:
				txtTip.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
				break;
			case XLDT_TipType.LoseTheServer:
				txtTip.text = XLDT_TipContentGame.contents[language][1];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.CoinOverFlow:
				txtTip.text = XLDT_TipContentGame.contents[language][2];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.ServerUpdate:
				txtTip.text = XLDT_TipContentGame.contents[language][13];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.Game_UserIdFrozen:
				txtTip.text = XLDT_TipContentGame.contents[language][14];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.UserIdPwdNotMatch:
				txtTip.text = XLDT_TipContentGame.contents[language][15];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.UserIdDeleted:
				txtTip.text = XLDT_TipContentGame.contents[language][16];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.UserIdRepeative:
				txtTip.text = XLDT_TipContentGame.contents[language][17];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.UserPwdChanged:
				txtTip.text = XLDT_TipContentGame.contents[language][18];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.ApplyForExpCoin_Success:
				txtTip.text = XLDT_TipContentGame.contents[language][4];
				break;
			case XLDT_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = XLDT_TipContentGame.contents[language][9];
				break;
			case XLDT_TipType.SelectTable_SendExpCoin:
				txtTip.text = XLDT_TipContentGame.contents[language][11];
				break;
			case XLDT_TipType.SelectSeat_NotEmpty:
				txtTip.text = XLDT_TipContentGame.contents[language][10];
				break;
			case XLDT_TipType.RoomEmpty:
				txtTip.text = XLDT_TipContentGame.contents[language][3];
				break;
			case XLDT_TipType.IsExitGame:
				txtTip.text = XLDT_TipContentGame.contents[language][20];
				btnCancel.gameObject.SetActive(value: true);
				break;
			case XLDT_TipType.TableDeleted:
				txtTip.text = XLDT_TipContentGame.contents[language][7];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.TableConfigChanged:
				txtTip.text = XLDT_TipContentGame.contents[language][8];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.LongTimeNoHandle:
				txtTip.text = XLDT_TipContentGame.contents[language][12];
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
				break;
			case XLDT_TipType.GameShutUp:
				txtTip.text = XLDT_TipContentGame.contents[language][5];
				break;
			case XLDT_TipType.UserShutUp:
				txtTip.text = XLDT_TipContentGame.contents[language][6];
				break;
			case XLDT_TipType.WaitForGameOver:
				txtTip.text = XLDT_TipContentGame.contents[language][21];
				break;
			case XLDT_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + coins + " 游戏币") : ("Congratulations.You'v got " + coins + " complimentary coins"));
				break;
			case XLDT_TipType.IsExitApplication:
				txtTip.text = XLDT_TipContentGame.contents[language][19];
				btnCancel.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void clickButton(Button btn)
	{
		if (btn == btnConfirm)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
			switch (tipType)
			{
			case XLDT_TipType.Net_ConnectionError:
			case XLDT_TipType.LoseTheServer:
			case XLDT_TipType.CoinOverFlow:
			case XLDT_TipType.ServerUpdate:
			case XLDT_TipType.Game_UserIdFrozen:
			case XLDT_TipType.UserIdPwdNotMatch:
			case XLDT_TipType.UserIdDeleted:
			case XLDT_TipType.UserIdRepeative:
			case XLDT_TipType.UserPwdChanged:
			case XLDT_TipType.IsExitApplication:
				if (tipType == XLDT_TipType.IsExitApplication || tipType == XLDT_TipType.CoinOverFlow || tipType == XLDT_TipType.Net_ConnectionError || tipType == XLDT_TipType.LoseTheServer)
				{
					ZH2_GVars.isStartedFromGame = true;
				}
				else
				{
					ZH2_GVars.isStartedFromGame = false;
				}
				XLDT_GameInfo.getInstance().CoinList.Clear();
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendQuitGame();
				BackToHall();
				break;
			case XLDT_TipType.SelectTable_SendExpCoin:
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendAddExpeGoldAuto();
				break;
			case XLDT_TipType.IsExitGame:
				if (!XLDT_DanTiaoCommon.G_TEST)
				{
					UnityEngine.Debug.LogError("退出游戏111");
					XLDT_NetMain.GetSingleton().MyCreateSocket.SendLeaveSeat(XLDT_GameInfo.getInstance().CurTable.Id, XLDT_GameInfo.getInstance().User.SeatIndex);
				}
				break;
			case XLDT_TipType.QuitToDesk:
				BackToSelectSeat();
				break;
			case XLDT_TipType.TableDeleted:
			case XLDT_TipType.TableConfigChanged:
			case XLDT_TipType.LongTimeNoHandle:
				BackToSelectSeat();
				break;
			case XLDT_TipType.GivingCoin:
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.getCoin);
				break;
			}
		}
		hideTip();
	}

	public void hideTip()
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		if (tipType != XLDT_TipType.NoneTip)
		{
			tipType = XLDT_TipType.NoneTip;
			objFrame.SetActive(value: false);
			XLDT_GameUIMngr.GetSingleton().ShowTip(isShow: false);
		}
	}

	public void BackToSelectSeat()
	{
		XLDT_GameUIMngr.GetSingleton().LeaveGame();
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectSeat;
		XLDT_ShowUI.getInstance().SetTableInfoVisible();
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		UnityEngine.Object.Destroy(GameObject.Find("GameManager"));
		UnityEngine.Object.Destroy(GameObject.Find("NetManager"));
		AssetBundleManager.GetInstance().UnloadAB("DanTiao");
		SceneManager.LoadScene("MainScene");
	}
}
