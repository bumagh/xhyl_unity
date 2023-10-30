using BCBM_GameCommon;
using BCBM_UICommon;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BCBM_GameTipManager : MonoBehaviour
{
	public Button mEnsureBtnCol;

	public Button mEscapeBtnCol;

	protected Text mTipContent;

	protected EGameTipType mTipType = EGameTipType.NoneTip;

	private int language;

	protected string mAppAdress = string.Empty;

	public static BCBM_GameTipManager gTipMgr;

	protected static float gMsxNetDelay = 1f;

	protected bool mIsNetTiming;

	protected float mTotalNetDelay;

	public EGameTipType GetTipType()
	{
		return mTipType;
	}

	public static BCBM_GameTipManager GetSingleton()
	{
		return gTipMgr;
	}

	private void Awake()
	{
		if (gTipMgr != null)
		{
			gTipMgr = null;
		}
		gTipMgr = this;
	}

	private void Start()
	{
		mTipContent = base.transform.Find("Text").GetComponent<Text>();
		mTipContent.transform.gameObject.SetActive(value: false);
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		language = BCBM_GameInfo.getInstance().Language;
		HideTip();
	}

	private void Update()
	{
		if (mIsNetTiming)
		{
			mTotalNetDelay += Time.deltaTime;
		}
		if (!BCBM_MyTest.TEST)
		{
		}
	}

	public void ShowTip(EGameTipType eType, string msg = "")
	{
		mTipType = eType;
		base.transform.localScale = Vector3.one;
		mEnsureBtnCol.enabled = true;
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		mEscapeBtnCol.enabled = false;
		mTipContent.transform.gameObject.SetActive(value: true);
		mEnsureBtnCol.transform.gameObject.SetActive(value: true);
		switch (eType)
		{
		case EGameTipType.UserIDFrozen:
		case EGameTipType.SelectSeat_NotEmpty:
			break;
		case EGameTipType.Custom:
			mTipContent.text = msg;
			break;
		case EGameTipType.Net_ConnectionError:
			mTipContent.text = BCBM_TipContent.contents[language][0];
			break;
		case EGameTipType.QuitToDesk:
			mTipContent.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
			break;
		case EGameTipType.LoseTheServer:
			mTipContent.text = BCBM_TipContent.contents[language][1];
			break;
		case EGameTipType.CoinOverFlow:
			mTipContent.text = BCBM_TipContent.contents[language][2];
			break;
		case EGameTipType.ApplyForExpCoin_Success:
			mTipContent.text = BCBM_TipContent.contents[language][4];
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
			}
			break;
		case EGameTipType.SelectTable_CreditBelowRistrict:
			mTipContent.text = BCBM_TipContent.contents[language][7];
			break;
		case EGameTipType.SelectTable_SendExpCoin:
			mTipContent.text = BCBM_TipContent.contents[language][8];
			break;
		case EGameTipType.RoomEmpty:
			mTipContent.text = BCBM_TipContent.contents[language][3];
			break;
		case EGameTipType.IsExitGame:
			mTipContent.text = BCBM_TipContent.contents[language][17];
			mEscapeBtnCol.transform.gameObject.SetActive(value: true);
			mEscapeBtnCol.enabled = true;
			break;
		case EGameTipType.TableParameterModified:
			mTipContent.text = BCBM_TipContent.contents[language][6];
			break;
		case EGameTipType.TableDeleted_Game:
			mTipContent.text = BCBM_TipContent.contents[language][5];
			break;
		case EGameTipType.LongTimeNoOperate:
			mTipContent.text = BCBM_TipContent.contents[language][9];
			break;
		case EGameTipType.ServerUpdate:
			mTipContent.text = BCBM_TipContent.contents[language][10];
			break;
		case EGameTipType.Game_UserIdFrozen:
			mTipContent.text = BCBM_TipContent.contents[language][11];
			break;
		case EGameTipType.UserIdDeleted:
			mTipContent.text = BCBM_TipContent.contents[language][13];
			break;
		case EGameTipType.UserIdRepeative:
			mTipContent.text = BCBM_TipContent.contents[language][14];
			break;
		case EGameTipType.UserPwdChanged:
			mTipContent.text = BCBM_TipContent.contents[language][15];
			break;
		case EGameTipType.UserIdPwdNotMatch:
			mTipContent.text = BCBM_TipContent.contents[language][12];
			break;
		case EGameTipType.GivingCoin:
			mTipContent.text = "恭喜你获得客服赠送的游戏币";
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
			}
			break;
		case EGameTipType.IsExitApplication:
			mTipContent.text = BCBM_TipContent.contents[language][16];
			mEscapeBtnCol.transform.gameObject.SetActive(value: true);
			mEscapeBtnCol.enabled = true;
			break;
		case EGameTipType.PoorNetSignal:
			mTipContent.text = BCBM_TipContent.contents[language][18];
			break;
		}
	}

	public void HideTip()
	{
		mTipType = EGameTipType.NoneTip;
		base.transform.DOScale(Vector3.zero, 0.2f);
		mEnsureBtnCol.enabled = false;
		mEscapeBtnCol.enabled = false;
	}

	public void OnClickEnsure()
	{
		switch (mTipType)
		{
		case EGameTipType.Net_ConnectionError:
		case EGameTipType.LoseTheServer:
		case EGameTipType.CoinOverFlow:
		case EGameTipType.RoomEmpty:
		case EGameTipType.ServerUpdate:
		case EGameTipType.Game_UserIdFrozen:
		case EGameTipType.UserIdDeleted:
		case EGameTipType.UserIdRepeative:
		case EGameTipType.UserPwdChanged:
		case EGameTipType.UserIdPwdNotMatch:
		case EGameTipType.IsExitApplication:
			UnityEngine.Debug.LogError("=======1");
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			BCBM_MySqlConnection.isStartedFromGame = true;
			BCBM_GameInfo.ClearGameInfo();
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			UnityEngine.Debug.LogError("这里被我注销了");
			AssetBundleManager.GetInstance().UnloadAB("BCBM");
			SceneManager.LoadScene("MainScene");
			break;
		case EGameTipType.SelectTable_SendExpCoin:
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
			break;
		case EGameTipType.IsExitGame:
			UnityEngine.Debug.LogError("退出游戏 " + BCBM_GameInfo.getInstance().UserInfo.SeatId);
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(BCBM_GameInfo.getInstance().UserInfo.TableId, BCBM_GameInfo.getInstance().UserInfo.SeatId);
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(BCBM_GameInfo.getInstance().UserInfo.TableId);
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(BCBM_GameInfo.getInstance().UserInfo.TableId);
			BCBM_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
			BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
			BCBM_GameInfo.getInstance().UserInfo.SeatId = -1;
			BCBM_Transmit.GetSingleton().game_Scene.SetActive(value: false);
			BCBM_Transmit.GetSingleton().ui_Scene.SetActive(value: true);
			BCBM_Transmit.GetSingleton().gameManger.SetActive(value: true);
			break;
		case EGameTipType.QuitToDesk:
			UnityEngine.Debug.LogError("退出游戏 " + BCBM_GameInfo.getInstance().UserInfo.SeatId);
			BCBM_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
			BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
			BCBM_GameInfo.getInstance().UserInfo.SeatId = -1;
			BCBM_Transmit.GetSingleton().game_Scene.SetActive(value: false);
			BCBM_Transmit.GetSingleton().ui_Scene.SetActive(value: true);
			BCBM_Transmit.GetSingleton().gameManger.SetActive(value: true);
			break;
		case EGameTipType.TableParameterModified:
		case EGameTipType.TableDeleted_Game:
		case EGameTipType.LongTimeNoOperate:
			if (BCBM_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_Game)
			{
				UnityEngine.Debug.LogError("============清空=============");
				BCBM_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
				BCBM_GameInfo.getInstance().UserInfo.SeatId = -1;
			}
			else
			{
				BCBM_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
			}
			break;
		}
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		HideTip();
	}

	public void OnClickEscape()
	{
		HideTip();
	}

	public void StartNetTiming()
	{
		mTotalNetDelay = 0f;
		mIsNetTiming = true;
	}

	public void EndNetTiming()
	{
		if (mTotalNetDelay > gMsxNetDelay)
		{
			ShowTip(EGameTipType.PoorNetSignal, string.Empty);
		}
		mTotalNetDelay = 0f;
		mIsNetTiming = false;
	}

	protected void _quitToLogin()
	{
		UnityEngine.Debug.LogError("===========_quitToLogin===========");
		BCBM_GameInfo.getInstance().UserInfo.RoomId = -1;
		BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
		BCBM_GameInfo.getInstance().UserInfo.SeatId = -1;
	}

	protected void _quitToRoomList()
	{
		UnityEngine.Debug.LogError("===========_quitToRoomList===========");
		BCBM_GameInfo.getInstance().UserInfo.RoomId = -1;
		BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
		BCBM_GameInfo.getInstance().UserInfo.SeatId = -1;
	}
}
