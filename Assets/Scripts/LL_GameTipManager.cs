using LL_GameCommon;

using LL_UICommon;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LL_GameTipManager : MonoBehaviour
{
    protected Collider mCloseBtnCol;

    protected Collider mEnsureBtnCol;

    protected Collider mEscapeBtnCol;

    protected Collider mBackgroundCol;

    protected UILabel mTipContent;

    protected UIPanel mTipPanel;

    protected GameObject mTipFrame;

    protected LL_RoomList mRoomList;

    protected LL_TableList mTableList;

    protected LL_SeatList mSeatList;

    protected LL_HudManager mHudMgr;

    protected LL_SettingPanel mSettingPanel;

    protected LL_OtherUserInfoPanel mOtherInfoPanel;

    protected LL_ChatPanel mChatPanel;

    protected LL_BetPanel mBetPanel;

    protected LL_PrizeResult mResultPanel;

    protected EGameTipType mTipType = EGameTipType.NoneTip;

    private int language;

    protected string mAppAdress = string.Empty;

    protected UILabel mAwardNumText;

    protected GameObject mAwardObj;

    protected int mAwardNum = -1;

    public static LL_GameTipManager gTipMgr;

    protected static float gMsxNetDelay = 1f;

    protected bool mIsNetTiming;

    protected float mTotalNetDelay;

    public string AppAdress
    {
        set
        {
            mAppAdress = value;
        }
    }

    public int AwardNum
    {
        set
        {
            mAwardNum = value;
            mAwardNumText.text = "+" + value;
        }
    }

    public EGameTipType GetTipType()
    {
        return mTipType;
    }

    public static LL_GameTipManager GetSingleton()
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

    private void Start()
    {
        if (!(base.transform.parent.name != "UI Root"))
        {
            mCloseBtnCol = base.transform.Find("CloseBtn").GetComponent<Collider>();
            mEnsureBtnCol = base.transform.Find("EnsureBtn").GetComponent<Collider>();
            mEscapeBtnCol = base.transform.Find("EscapeBtn").GetComponent<Collider>();
            mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
            mTipContent = base.transform.Find("TipContent").GetComponent<UILabel>();
            mTipPanel = base.transform.GetComponent<UIPanel>();
            mTipFrame = base.transform.Find("TipFrame").gameObject;
            mTipContent.transform.gameObject.SetActive(value: false);
            mCloseBtnCol.transform.gameObject.SetActive(value: false);
            mEscapeBtnCol.transform.gameObject.SetActive(value: false);
            mAwardObj = base.transform.Find("AwardNum").gameObject;
            mAwardNumText = base.transform.Find("AwardNum").GetComponent<UILabel>();
            mRoomList = LL_AppUIMngr.GetSingleton().mRoomList;
            mTableList = GameObject.Find("TableListPanel").GetComponent<LL_TableList>();
            mHudMgr = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
            mOtherInfoPanel = GameObject.Find("OtherUserInfoPanel").GetComponent<LL_OtherUserInfoPanel>();
            mChatPanel = GameObject.Find("ChatPanel").GetComponent<LL_ChatPanel>();
            mSeatList = GameObject.Find("SeatPanel").GetComponent<LL_SeatList>();
            mSettingPanel = GameObject.Find("SettingPanel").GetComponent<LL_SettingPanel>();
            mBetPanel = GameObject.Find("BetPanel").GetComponent<LL_BetPanel>();
            mResultPanel = GameObject.Find("ResultPanel").GetComponent<LL_PrizeResult>();
            language = LL_GameInfo.getInstance().Language;
            HideTip();
        }
    }

    private void Update()
    {
        if (mIsNetTiming)
        {
            mTotalNetDelay += Time.deltaTime;
        }
        if (!LL_MyTest.TEST)
        {
        }
    }

    public void ShowTip(EGameTipType eType, string msg = "")
    {
        if (LL_AppUIMngr.GetSingleton() != null && LL_AppUIMngr.GetSingleton().u_Canvas != null && LL_AppUIMngr.GetSingleton().u_Canvas.activeInHierarchy)
        {
            LL_AppUIMngr.GetSingleton().lL_GameTipManager_Canvas.ShowTip(eType, msg);
            UnityEngine.Debug.LogError("=========交给ugui处理=======");
            HideTip();
            return;
        }
        mTipType = eType;
        mTipPanel.enabled = true;
        mEnsureBtnCol.enabled = true;
        mBackgroundCol.enabled = true;
        mCloseBtnCol.transform.gameObject.SetActive(value: false);
        mCloseBtnCol.enabled = false;
        mEscapeBtnCol.transform.gameObject.SetActive(value: false);
        mEscapeBtnCol.enabled = false;
        mTipFrame.SetActive(value: true);
        mTipContent.transform.gameObject.SetActive(value: true);
        mEnsureBtnCol.transform.gameObject.SetActive(value: true);
        mAwardObj.SetActive(value: false);
        mEnsureBtnCol.transform.localPosition = new Vector3(0f, -80f, 0f);
        mTipContent.pivot = UIWidget.Pivot.Center;
        mTipContent.transform.localPosition = new Vector3(0f, 80f, -5f);
        switch (eType)
        {
            case EGameTipType.UserIDFrozen:
            case EGameTipType.SelectSeat_NotEmpty:
                break;
            case EGameTipType.OutGame:
            case EGameTipType.OutGame2:
            case EGameTipType.Custom:
                mTipContent.text = msg;
                break;
            case EGameTipType.Net_ConnectionError:
                mTipContent.text = LL_TipContent.contents[language][0];
                break;
            case EGameTipType.QuitToDesk:
                mTipContent.text = ZH2_GVars.ShowTip("桌子参数改变,请重新进入", "Table parameters have changed, please re-enter", string.Empty);
                break;
            case EGameTipType.LoseTheServer:
                mTipContent.text = LL_TipContent.contents[language][1];
                break;
            case EGameTipType.CoinOverFlow:
                mTipContent.text = LL_TipContent.contents[language][2];
                break;
            case EGameTipType.ApplyForExpCoin_Success:
                mTipContent.text = LL_TipContent.contents[language][4];
                LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
                break;
            case EGameTipType.SelectTable_CreditBelowRistrict:
                mTipContent.text = LL_TipContent.contents[language][7];
                break;
            case EGameTipType.SelectTable_SendExpCoin:
                mTipContent.text = LL_TipContent.contents[language][8];
                break;
            case EGameTipType.RoomEmpty:
                mTipContent.text = LL_TipContent.contents[language][3];
                break;
            case EGameTipType.IsExitGame:
                mTipContent.text = LL_TipContent.contents[language][17];
                mEscapeBtnCol.transform.gameObject.SetActive(value: true);
                mEscapeBtnCol.enabled = true;
                mEnsureBtnCol.transform.localPosition = new Vector3(-150f, -80f, 0f);
                break;
            case EGameTipType.TableParameterModified:
                mTipContent.text = LL_TipContent.contents[language][6];
                break;
            case EGameTipType.TableDeleted_Game:
                mTipContent.text = LL_TipContent.contents[language][5];
                break;
            case EGameTipType.LongTimeNoOperate:
                mTipContent.text = LL_TipContent.contents[language][9];
                break;
            case EGameTipType.ServerUpdate:
                mTipContent.text = LL_TipContent.contents[language][10];
                break;
            case EGameTipType.Game_UserIdFrozen:
                mTipContent.text = LL_TipContent.contents[language][11];
                break;
            case EGameTipType.UserIdDeleted:
                mTipContent.text = LL_TipContent.contents[language][13];
                break;
            case EGameTipType.UserIdRepeative:
                mTipContent.text = LL_TipContent.contents[language][14];
                break;
            case EGameTipType.UserPwdChanged:
                mTipContent.text = LL_TipContent.contents[language][15];
                break;
            case EGameTipType.UserIdPwdNotMatch:
                mTipContent.text = LL_TipContent.contents[language][12];
                break;
            case EGameTipType.GivingCoin:
                mTipContent.text = ((language == 0) ? ("恭喜你获得客服赠送的 " + mAwardNum + " 游戏币") : ("Congratulations.You'v got " + mAwardNum + " complimentary coins"));
                LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.GivingCoins);
                mAwardObj.SetActive(value: true);
                mAwardObj.transform.localPosition = new Vector3(0f, 0f, -10f);
                TweenPosition.Begin(mAwardObj, 1f, new Vector3(0f, -80f, -10f));
                if ((bool)mAwardObj.GetComponent<TweenAlpha>())
                {
                    mAwardObj.GetComponent<TweenAlpha>().alpha = 1f;
                }
                TweenAlpha.Begin(mAwardObj, 1f, 0f);
                break;
            case EGameTipType.IsExitApplication:
                mTipContent.text = LL_TipContent.contents[language][16];
                mEscapeBtnCol.transform.gameObject.SetActive(value: true);
                mEscapeBtnCol.enabled = true;
                mEnsureBtnCol.transform.localPosition = new Vector3(-150f, -80f, 0f);
                break;
            case EGameTipType.PoorNetSignal:
                mTipContent.text = LL_TipContent.contents[language][18];
                break;
        }
    }

    public void HideTip()
    {
        mTipType = EGameTipType.NoneTip;
        mTipPanel.enabled = false;
        mEnsureBtnCol.enabled = false;
        mCloseBtnCol.enabled = false;
        mEscapeBtnCol.enabled = false;
        mBackgroundCol.enabled = false;
    }

    public void OnClickEnsure()
    {
        switch (mTipType)
        {
            case EGameTipType.OutGame:
                GameOver(isOver: true);
                break;
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
                /*
                LL_GameInfo.getInstance().ClearGameInfo();
                LL_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
                if (mTipType == EGameTipType.IsExitApplication || mTipType == EGameTipType.CoinOverFlow || mTipType == EGameTipType.Net_ConnectionError || mTipType == EGameTipType.LoseTheServer)
                {
                    ZH2_GVars.isStartedFromGame = true;
                }
                else
                {
                    ZH2_GVars.isStartedFromGame = false;
                }
                BackToHall();
                */

                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LL_GameInfo.getInstance().UserInfo.TableId, LL_GameInfo.getInstance().UserInfo.SeatId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(LL_GameInfo.getInstance().UserInfo.TableId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(LL_GameInfo.getInstance().UserInfo.RoomId);
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                UnityEngine.Debug.Log("异常退出？？");
                LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().tempSelectId = -1;
                break;
            case EGameTipType.SelectTable_SendExpCoin:
                LL_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
                break;
            case EGameTipType.IsExitGame:
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LL_GameInfo.getInstance().UserInfo.TableId, LL_GameInfo.getInstance().UserInfo.SeatId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(LL_GameInfo.getInstance().UserInfo.TableId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(LL_GameInfo.getInstance().UserInfo.RoomId);
                mHudMgr.HideHud();
                mHudMgr.ResetGame();
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                mTableList.ShowTableList(3);
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                UnityEngine.Debug.Log("退出了？？");
                LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().tempSelectId = -1;
                break;
            case EGameTipType.QuitToDesk:
                mHudMgr.HideHud();
                mHudMgr.ResetGame();
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                mTableList.ShowTableList(3);
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().tempSelectId = -1;
                break;
            case EGameTipType.TableParameterModified:
            case EGameTipType.TableDeleted_Game:
            case EGameTipType.LongTimeNoOperate:
                /*
			if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game)
			{
				mHudMgr.HideHud();
				mHudMgr.ResetGame();
				LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				mTableList.ShowTableList(2);
				LL_GameInfo.getInstance().UserInfo.TableId = -1;
				LL_GameInfo.getInstance().UserInfo.SeatId = -1;
			}
			else
			{
				LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				LL_GameInfo.getInstance().UserInfo.TableId = -1;
				mSeatList.HideSeatList(1);
			}
			*/
                Debug.Log("退出这里");
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LL_GameInfo.getInstance().UserInfo.TableId, LL_GameInfo.getInstance().UserInfo.SeatId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(LL_GameInfo.getInstance().UserInfo.TableId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(LL_GameInfo.getInstance().UserInfo.RoomId);
                mHudMgr.HideHud();
                mHudMgr.ResetGame();
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                mTableList.ShowTableList(3);
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                UnityEngine.Debug.Log("退出了？？");
                LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().tempSelectId = -1;

                break;
        }
        LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
        HideTip();
    }

    private void GameOver(bool isOver)
    {
        Application.Quit();
    }

    public void OnClickEscape()
    {
        LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
        HideTip();
    }

    public void HideAllPopupPanel()
    {
        mOtherInfoPanel.HideOtherInfo();
        mChatPanel.HidePrivateChatWindow();
        mChatPanel.HidePublicChatWindow();
        mSettingPanel.HideSetting();
        mBetPanel.HideBetPanel(bIsMove: false);
        mResultPanel.HidePrizeResult(bIsFadeOut: false);
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
        mTableList.HideTableList(3);
        mSeatList.HideSeatList(3);
        mRoomList.HideRoomList(2);
        mHudMgr.HideHud();
        HideAllPopupPanel();
        LL_GameInfo.getInstance().UserInfo.RoomId = -1;
        LL_GameInfo.getInstance().UserInfo.TableId = -1;
        LL_GameInfo.getInstance().UserInfo.SeatId = -1;
    }

    protected void _quitToRoomList()
    {
        mHudMgr.HideHud();
        mTableList.HideTableList(3);
        mSeatList.HideSeatList(3);
        mRoomList.ShowRoomList(2);
        LL_GameInfo.getInstance().UserInfo.RoomId = -1;
        LL_GameInfo.getInstance().UserInfo.TableId = -1;
        LL_GameInfo.getInstance().UserInfo.SeatId = -1;
    }

    public void BackToHall()
    {
        UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
        AssetBundleManager.GetInstance().UnloadAB("LuckyLion");
        SceneManager.LoadScene("MainScene");
    }
}
