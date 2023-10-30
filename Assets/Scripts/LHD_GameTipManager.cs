using LHD_UICommon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LHD_GameTipManager : MonoBehaviour
{
	public Text mTipContent;

	private int language;

	protected LHD_EGameTipType mTipType = LHD_EGameTipType.NoneTip;

	public Button mEnsureBtnCol;

	public Button mEscapeBtnCol;

	public static LHD_GameTipManager gTipMgr;

	private void Awake()
	{
		gTipMgr = this;
		base.gameObject.SetActive(value: false);
		base.transform.localScale = Vector3.one;
		language = LHD_GameInfo.getInstance().Language;
		mEnsureBtnCol.onClick.AddListener(OnClickEnsure);
		mEscapeBtnCol.onClick.AddListener(HideTip);
	}

	public static LHD_GameTipManager GetSingleton()
	{
		return gTipMgr;
	}

	public void ShowTip(LHD_EGameTipType eType, string msg = "")
	{
		mTipType = eType;
		UnityEngine.Debug.LogError("EGameTipType: " + eType);
		base.gameObject.SetActive(value: true);
		mEscapeBtnCol.transform.gameObject.SetActive(value: false);
		mTipContent.transform.gameObject.SetActive(value: true);
		mEnsureBtnCol.transform.gameObject.SetActive(value: true);
		switch (eType)
		{
		case LHD_EGameTipType.UserIDFrozen:
		case LHD_EGameTipType.SelectSeat_NotEmpty:
			break;
		case LHD_EGameTipType.OutGame:
		case LHD_EGameTipType.OutGame2:
			mTipContent.text = msg;
			break;
		case LHD_EGameTipType.Net_ConnectionError:
			mTipContent.text = LHD_TipContent.contents[language][0];
			break;
		case LHD_EGameTipType.LoseTheServer:
			mTipContent.text = LHD_TipContent.contents[language][1];
			break;
		case LHD_EGameTipType.CoinOverFlow:
			mTipContent.text = LHD_TipContent.contents[language][2];
			break;
		case LHD_EGameTipType.ApplyForExpCoin_Success:
			mTipContent.text = LHD_TipContent.contents[language][4];
			break;
		case LHD_EGameTipType.SelectTable_CreditBelowRistrict:
			mTipContent.text = LHD_TipContent.contents[language][7];
			break;
		case LHD_EGameTipType.SelectTable_SendExpCoin:
			mTipContent.text = LHD_TipContent.contents[language][8];
			break;
		case LHD_EGameTipType.RoomEmpty:
			mTipContent.text = LHD_TipContent.contents[language][3];
			break;
		case LHD_EGameTipType.IsExitGame:
			mTipContent.text = LHD_TipContent.contents[language][17];
			mEscapeBtnCol.gameObject.SetActive(value: true);
			break;
		case LHD_EGameTipType.TableParameterModified:
			mTipContent.text = LHD_TipContent.contents[language][6];
			break;
		case LHD_EGameTipType.TableDeleted_Game:
			mTipContent.text = LHD_TipContent.contents[language][5];
			break;
		case LHD_EGameTipType.LongTimeNoOperate:
			mTipContent.text = LHD_TipContent.contents[language][9];
			break;
		case LHD_EGameTipType.ServerUpdate:
			mTipContent.text = LHD_TipContent.contents[language][10];
			break;
		case LHD_EGameTipType.Game_UserIdFrozen:
			mTipContent.text = LHD_TipContent.contents[language][11];
			break;
		case LHD_EGameTipType.UserIdDeleted:
			mTipContent.text = LHD_TipContent.contents[language][13];
			break;
		case LHD_EGameTipType.UserIdRepeative:
			mTipContent.text = LHD_TipContent.contents[language][14];
			break;
		case LHD_EGameTipType.UserPwdChanged:
			mTipContent.text = LHD_TipContent.contents[language][15];
			break;
		case LHD_EGameTipType.UserIdPwdNotMatch:
			mTipContent.text = LHD_TipContent.contents[language][12];
			break;
		case LHD_EGameTipType.GivingCoin:
			mTipContent.text = ((language == 0) ? "恭喜你获得客服赠送的游戏币" : "Congratulations.You'v got complimentary coins");
			break;
		case LHD_EGameTipType.IsExitApplication:
			mTipContent.text = LHD_TipContent.contents[language][16];
			mEscapeBtnCol.gameObject.SetActive(value: true);
			break;
		case LHD_EGameTipType.PoorNetSignal:
			mTipContent.text = LHD_TipContent.contents[language][18];
			break;
		}
	}

	public void HideTip()
	{
		mTipType = LHD_EGameTipType.NoneTip;
		base.gameObject.SetActive(value: false);
	}

	public void OnClickEnsure()
	{
		UnityEngine.Debug.LogError("===EGameTipType：" + mTipType);
		switch (mTipType)
		{
		case LHD_EGameTipType.OutGame:
			GameOver(isOver: true);
			break;
		case LHD_EGameTipType.Net_ConnectionError:
		case LHD_EGameTipType.LoseTheServer:
		case LHD_EGameTipType.CoinOverFlow:
		case LHD_EGameTipType.ServerUpdate:
		case LHD_EGameTipType.Game_UserIdFrozen:
		case LHD_EGameTipType.UserIdDeleted:
		case LHD_EGameTipType.UserIdRepeative:
		case LHD_EGameTipType.UserPwdChanged:
		case LHD_EGameTipType.UserIdPwdNotMatch:
		case LHD_EGameTipType.IsExitApplication:
			LHD_GameInfo.getInstance().ClearGameInfo();
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
			if (mTipType == LHD_EGameTipType.IsExitApplication || mTipType == LHD_EGameTipType.CoinOverFlow || mTipType == LHD_EGameTipType.Net_ConnectionError || mTipType == LHD_EGameTipType.LoseTheServer)
			{
				ZH2_GVars.isStartedFromGame = true;
			}
			else
			{
				ZH2_GVars.isStartedFromGame = false;
			}
			BackToHall();
			break;
		case LHD_EGameTipType.SelectTable_SendExpCoin:
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
			break;
		case LHD_EGameTipType.IsExitGame:
			UnityEngine.Debug.LogError("========退出 未完成======");
			break;
		case LHD_EGameTipType.TableParameterModified:
		case LHD_EGameTipType.TableDeleted_Game:
		case LHD_EGameTipType.LongTimeNoOperate:
			UnityEngine.Debug.LogError("======超时 未完成=====");
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
		if (AssetBundleManager.GetInstance() != null)
		{
			AssetBundleManager.GetInstance().UnloadAB("LHD");
		}
		SceneManager.LoadScene("MainScene");
	}
}
