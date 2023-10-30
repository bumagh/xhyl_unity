using LL_GameCommon;

using LL_UICommon;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LL_GameTipManager_Canvas : MonoBehaviour
{
    public Text mTipContent;

    private int language;

    protected EGameTipType mTipType = EGameTipType.NoneTip;

    public Button mEnsureBtnCol;

    public Button mEscapeBtnCol;

    private void Awake()
    {
        base.gameObject.SetActive(value: false);
        language = LL_GameInfo.getInstance().Language;
        mEnsureBtnCol.onClick.AddListener(OnClickEnsure);
        mEscapeBtnCol.onClick.AddListener(HideTip);
    }

    public void ShowTip(EGameTipType eType, string msg = "")
    {
        mTipType = eType;
        base.gameObject.SetActive(value: true);
        mEscapeBtnCol.transform.gameObject.SetActive(value: false);
        mTipContent.transform.gameObject.SetActive(value: true);
        mEnsureBtnCol.transform.gameObject.SetActive(value: true);
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
            case EGameTipType.LoseTheServer:
                mTipContent.text = LL_TipContent.contents[language][1];
                break;
            case EGameTipType.CoinOverFlow:
                mTipContent.text = LL_TipContent.contents[language][2];
                break;
            case EGameTipType.ApplyForExpCoin_Success:
                mTipContent.text = LL_TipContent.contents[language][4];
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
                mEscapeBtnCol.gameObject.SetActive(value: true);
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
                mTipContent.text = ((language == 0) ? "恭喜你获得客服赠送的游戏币" : "Congratulations.You'v got complimentary coins");
                break;
            case EGameTipType.IsExitApplication:
                mTipContent.text = LL_TipContent.contents[language][16];
                mEscapeBtnCol.gameObject.SetActive(value: true);
                break;
            case EGameTipType.PoorNetSignal:
                mTipContent.text = LL_TipContent.contents[language][18];
                break;
        }
    }

    public void HideTip()
    {
        mTipType = EGameTipType.NoneTip;
        base.gameObject.SetActive(value: false);
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
                break;
            case EGameTipType.SelectTable_SendExpCoin:
                LL_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
                break;
            case EGameTipType.IsExitGame:
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LL_GameInfo.getInstance().UserInfo.TableId, LL_GameInfo.getInstance().UserInfo.SeatId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(LL_GameInfo.getInstance().UserInfo.TableId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(LL_GameInfo.getInstance().UserInfo.RoomId);
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                UnityEngine.Debug.Log("退出了？？");
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
				LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				LL_GameInfo.getInstance().UserInfo.TableId = -1;
				LL_GameInfo.getInstance().UserInfo.SeatId = -1;
			}
			else
			{
				LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
				LL_GameInfo.getInstance().UserInfo.TableId = -1;
			}
			*/

                Debug.Log("退出这里222");
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LL_GameInfo.getInstance().UserInfo.TableId, LL_GameInfo.getInstance().UserInfo.SeatId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(LL_GameInfo.getInstance().UserInfo.TableId);
                LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(LL_GameInfo.getInstance().UserInfo.RoomId);
                LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
                LL_GameInfo.getInstance().UserInfo.TableId = -1;
                LL_GameInfo.getInstance().UserInfo.SeatId = -1;
                UnityEngine.Debug.Log("退出了？？");
                LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: true);
                LL_AppUIMngr.GetSingleton().tempSelectId = -1;
                break;
        }
        HideTip();
    }

    private void GameOver(bool isOver)
    {
        Application.Quit();
    }

    public void BackToHall()
    {
        UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
        AssetBundleManager.GetInstance().UnloadAB("LuckyLion");
        SceneManager.LoadScene("MainScene");
    }
}
