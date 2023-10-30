using JSYS_LL_GameCommon;
using JSYS_LL_UICommon;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_GameTipManager : MonoBehaviour
{
	public Button mEnsureBtnCol;

	public Button mEscapeBtnCol;

	protected Text mTipContent;

	protected Transform mTipPanel;

	protected EGameTipType mTipType = EGameTipType.NoneTip;

	private int language;

	protected string mAppAdress = string.Empty;

	public static JSYS_LL_GameTipManager gTipMgr;

	protected static float gMsxNetDelay = 1f;

	protected bool mIsNetTiming;

	protected float mTotalNetDelay;

	public EGameTipType GetTipType()
	{
		return mTipType;
	}

	public static JSYS_LL_GameTipManager GetSingleton()
	{
		return gTipMgr;
	}

	private void Awake()
	{
		if (!gTipMgr)
		{
			gTipMgr = this;
		}
	}

	private void OnEnable()
	{
		mTipContent = base.transform.Find("Text").GetComponent<Text>();
		mTipPanel = base.transform;
		mTipContent.transform.gameObject.SetActive(value: false);
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		language = JSYS_LL_GameInfo.getInstance().Language;
		HideTip();
	}

	private void Start()
	{
		mTipContent = base.transform.Find("Text").GetComponent<Text>();
		mTipPanel = base.transform;
		mTipContent.transform.gameObject.SetActive(value: false);
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		language = JSYS_LL_GameInfo.getInstance().Language;
		HideTip();
	}

	private void Update()
	{
		if (mIsNetTiming)
		{
			mTotalNetDelay += Time.deltaTime;
		}
	}

	public void ShowTip(EGameTipType eType, string msg = "")
	{
		mTipType = eType;
		mTipPanel.localScale = Vector3.one;
		mEnsureBtnCol.enabled = true;
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		mEscapeBtnCol.enabled = false;
		mTipContent.transform.gameObject.SetActive(value: true);
		mEnsureBtnCol.transform.gameObject.SetActive(value: true);
		switch (eType)
		{
		case EGameTipType.UserIDFrozen:
			break;
		case EGameTipType.Custom:
			mTipContent.text = msg;
			break;
		case EGameTipType.Net_ConnectionError:
			mTipContent.text = JSYS_LL_TipContent.contents[language][0];
			break;
		case EGameTipType.LoseTheServer:
			mTipContent.text = JSYS_LL_TipContent.contents[language][1];
			break;
		case EGameTipType.CoinOverFlow:
			mTipContent.text = JSYS_LL_TipContent.contents[language][2];
			break;
		case EGameTipType.ApplyForExpCoin_Success:
			mTipContent.text = JSYS_LL_TipContent.contents[language][4];
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(JSYS_LL_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
			}
			break;
		case EGameTipType.SelectTable_CreditBelowRistrict:
			mTipContent.text = JSYS_LL_TipContent.contents[language][7];
			break;
		case EGameTipType.SelectTable_SendExpCoin:
			mTipContent.text = JSYS_LL_TipContent.contents[language][8];
			break;
		case EGameTipType.RoomEmpty:
			mTipContent.text = JSYS_LL_TipContent.contents[language][3];
			break;
		case EGameTipType.IsExitGame:
			mTipContent.text = JSYS_LL_TipContent.contents[language][17];
			mEscapeBtnCol.transform.gameObject.SetActive(value: true);
			mEscapeBtnCol.enabled = true;
			break;
		case EGameTipType.TableParameterModified:
			mTipContent.text = JSYS_LL_TipContent.contents[language][6];
			break;
		case EGameTipType.TableDeleted_Game:
			mTipContent.text = JSYS_LL_TipContent.contents[language][5];
			break;
		case EGameTipType.LongTimeNoOperate:
			mTipContent.text = JSYS_LL_TipContent.contents[language][9];
			break;
		case EGameTipType.ServerUpdate:
			mTipContent.text = JSYS_LL_TipContent.contents[language][10];
			break;
		case EGameTipType.Game_UserIdFrozen:
			mTipContent.text = JSYS_LL_TipContent.contents[language][11];
			break;
		case EGameTipType.UserIdDeleted:
			mTipContent.text = JSYS_LL_TipContent.contents[language][13];
			break;
		case EGameTipType.UserIdRepeative:
			mTipContent.text = JSYS_LL_TipContent.contents[language][14];
			break;
		case EGameTipType.UserPwdChanged:
			mTipContent.text = JSYS_LL_TipContent.contents[language][15];
			break;
		case EGameTipType.UserIdPwdNotMatch:
			mTipContent.text = JSYS_LL_TipContent.contents[language][12];
			break;
		case EGameTipType.GivingCoin:
			mTipContent.text = "恭喜你获得客服赠送的游戏币";
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(JSYS_LL_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
			}
			break;
		case EGameTipType.IsExitApplication:
			mTipContent.text = JSYS_LL_TipContent.contents[language][16];
			mEscapeBtnCol.transform.gameObject.SetActive(value: true);
			mEscapeBtnCol.enabled = true;
			break;
		case EGameTipType.PoorNetSignal:
			mTipContent.text = JSYS_LL_TipContent.contents[language][18];
			break;
		case EGameTipType.SelectSeat_NotEmpty:
			mTipContent.text = JSYS_LL_TipContent.contents[language][19];
			break;
		}
	}

	public void HideTip()
	{
		mTipType = EGameTipType.NoneTip;
		mTipPanel.localScale = Vector3.zero;
		mEnsureBtnCol.enabled = false;
		mEscapeBtnCol.enabled = false;
	}

	public void ClickBlack()
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		ZH2_GVars.isStartedFromGame = true;
		JSYS_LL_GameInfo.ClearGameInfo();
		UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
		UnityEngine.Debug.LogError("这里被我注销了");
		AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
		asyncOperation.allowSceneActivation = true;
	}

	public void OnClickEnsure()
	{
		switch (mTipType)
		{
		case EGameTipType.Net_ConnectionError:
		case EGameTipType.LoseTheServer:
		case EGameTipType.CoinOverFlow:
		case EGameTipType.ServerUpdate:
		case EGameTipType.Game_UserIdFrozen:
		case EGameTipType.UserIdDeleted:
		case EGameTipType.UserIdRepeative:
		case EGameTipType.UserPwdChanged:
		case EGameTipType.UserIdPwdNotMatch:
		case EGameTipType.IsExitApplication:
		{
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			if (mTipType == EGameTipType.IsExitApplication || mTipType == EGameTipType.CoinOverFlow || mTipType == EGameTipType.Net_ConnectionError || mTipType == EGameTipType.LoseTheServer)
			{
				JSYS_GVars.isStartedFromGame = true;
			}
			else
			{
				JSYS_GVars.isStartedFromGame = false;
			}
			JSYS_LL_GameInfo.ClearGameInfo();
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			UnityEngine.Debug.LogError("这里被我注销了");
			AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
			asyncOperation.allowSceneActivation = true;
			break;
		}
		case EGameTipType.SelectTable_SendExpCoin:
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
			break;
		case EGameTipType.IsExitGame:
			UnityEngine.Debug.LogError("退出游戏");
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(JSYS_LL_GameInfo.getInstance().UserInfo.TableId, JSYS_LL_GameInfo.getInstance().UserInfo.SeatId);
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
			JSYS_LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
			JSYS_LL_GameInfo.getInstance().UserInfo.TableId = -1;
			JSYS_LL_GameInfo.getInstance().UserInfo.SeatId = -1;
			break;
		case EGameTipType.TableParameterModified:
		case EGameTipType.TableDeleted_Game:
		case EGameTipType.LongTimeNoOperate:
			if (JSYS_LL_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_Game)
			{
				JSYS_LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				JSYS_LL_GameInfo.getInstance().UserInfo.TableId = -1;
				JSYS_LL_GameInfo.getInstance().UserInfo.SeatId = -1;
			}
			else
			{
				JSYS_LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				JSYS_LL_GameInfo.getInstance().UserInfo.TableId = -1;
			}
			break;
		}
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		HideTip();
	}

	public void OnClickEscape()
	{
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
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
}
