using STDT_GameConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XLDT_TipManager : MonoBehaviour
{
	[HideInInspector]
	public XLDT_TipType tipType = XLDT_TipType.NoneTip;

	private GameObject objTip;

	[HideInInspector]
	public Text txtTip;

	private Button btnConfirm;

	private Button btnCancel;

	private int language;

	private static XLDT_TipManager tipManager;

	public static XLDT_TipManager getInstance()
	{
		return tipManager;
	}

	private void Awake()
	{
		tipManager = this;
		UnityEngine.Debug.LogError("====tipManager: " + base.gameObject.name);
		objTip = base.transform.Find("TipDialog_New").gameObject;
		objTip.transform.localScale = Vector3.one;
		txtTip = objTip.transform.Find("ImgBg/TxtTip").GetComponent<Text>();
		btnConfirm = objTip.transform.Find("ImgBg/Btns/BtnConfirm").GetComponent<Button>();
		btnCancel = objTip.transform.Find("ImgBg/Btns/BtnCancel").GetComponent<Button>();
		btnConfirm.transform.Find("Text").GetComponent<Text>().text = ((language != 0) ? "OK" : "确定");
		btnCancel.transform.Find("Text").GetComponent<Text>().text = ((language != 0) ? "Cancel" : "取消");
		objTip.SetActive(value: false);
		btnConfirm.onClick.AddListener(ClickBtnConfirm);
		btnCancel.onClick.AddListener(ClickBtnCancel);
	}

	public void ShowTip(XLDT_TipType eType, int type = 0)
	{
		language = XLDT_GameInfo.getInstance().Language;
		if (tipType != eType && (tipType < XLDT_TipType.Net_ConnectionError || tipType > XLDT_TipType.UserPwdChanged))
		{
			tipType = eType;
			objTip.SetActive(value: true);
			ShowCancelButton(type == 1);
			switch (eType)
			{
			case XLDT_TipType.GameShutUp:
			case XLDT_TipType.UserShutUp:
			case XLDT_TipType.WaitForGameOver:
				break;
			case XLDT_TipType.Net_ConnectionError:
				txtTip.text = XLDT_TipContent.contents[language][0];
				break;
			case XLDT_TipType.LoseTheServer:
				txtTip.text = XLDT_TipContent.contents[language][1];
				break;
			case XLDT_TipType.CoinOverFlow:
				txtTip.text = XLDT_TipContent.contents[language][2];
				break;
			case XLDT_TipType.ServerUpdate:
				txtTip.text = XLDT_TipContent.contents[language][11];
				break;
			case XLDT_TipType.Game_UserIdFrozen:
				txtTip.text = XLDT_TipContent.contents[language][12];
				break;
			case XLDT_TipType.UserIdPwdNotMatch:
				txtTip.text = XLDT_TipContent.contents[language][13];
				break;
			case XLDT_TipType.UserIdDeleted:
				txtTip.text = XLDT_TipContent.contents[language][14];
				break;
			case XLDT_TipType.UserIdRepeative:
				txtTip.text = XLDT_TipContent.contents[language][15];
				break;
			case XLDT_TipType.UserPwdChanged:
				txtTip.text = XLDT_TipContent.contents[language][16];
				break;
			case XLDT_TipType.ApplyForExpCoin_Success:
				txtTip.text = XLDT_TipContent.contents[language][4];
				break;
			case XLDT_TipType.SelectTable_CreditBelowRistrict:
				txtTip.text = XLDT_TipContent.contents[language][7];
				break;
			case XLDT_TipType.SelectTable_SendExpCoin:
				txtTip.text = XLDT_TipContent.contents[language][9];
				break;
			case XLDT_TipType.SelectSeat_NotEmpty:
				txtTip.text = XLDT_TipContent.contents[language][8];
				break;
			case XLDT_TipType.RoomEmpty:
				txtTip.text = XLDT_TipContent.contents[language][3];
				break;
			case XLDT_TipType.IsExitGame:
				txtTip.text = XLDT_TipContent.contents[language][18];
				break;
			case XLDT_TipType.TableDeleted:
				txtTip.text = XLDT_TipContent.contents[language][5];
				break;
			case XLDT_TipType.TableConfigChanged:
				txtTip.text = XLDT_TipContent.contents[language][6];
				break;
			case XLDT_TipType.LongTimeNoHandle:
				txtTip.text = XLDT_TipContent.contents[language][10];
				break;
			case XLDT_TipType.GivingCoin:
				txtTip.text = ((language == 0) ? "恭喜你获得客服赠送的  游戏币" : "Congratulations.You'v got  complimentary coins");
				break;
			case XLDT_TipType.IsExitApplication:
				txtTip.text = XLDT_TipContent.contents[language][17];
				break;
			}
		}
	}

	protected void ShowCancelButton(bool show)
	{
		btnCancel.gameObject.SetActive(show);
	}

	public void HideTip()
	{
		tipType = XLDT_TipType.NoneTip;
		objTip.gameObject.SetActive(value: false);
	}

	public void ClickBtnBack()
	{
		UnityEngine.Debug.LogError("退出游戏");
		ZH2_GVars.isStartedFromGame = true;
		XLDT_GameInfo.getInstance().CoinList.Clear();
		XLDT_GameInfo.getInstance().ClearGameInfo();
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendQuitGame();
		BackToHall();
	}

	public void ClickBtnConfirm()
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
			XLDT_GameInfo.getInstance().ClearGameInfo();
			XLDT_NetMain.GetSingleton().MyCreateSocket.SendQuitGame();
			BackToHall();
			break;
		case XLDT_TipType.IsExitGame:
			UnityEngine.Debug.LogError("退出游戏");
			break;
		case XLDT_TipType.TableDeleted:
		case XLDT_TipType.TableConfigChanged:
		case XLDT_TipType.LongTimeNoHandle:
			if (XLDT_GameInfo.getInstance().currentState != XLDT_GameState.On_SelectSeat)
			{
			}
			break;
		case XLDT_TipType.SelectTable_CreditBelowRistrict:
		case XLDT_TipType.SelectSeat_NotEmpty:
			UnityEngine.Debug.LogError("座位有人");
			break;
		}
		if (tipType == XLDT_TipType.GivingCoin)
		{
			XLDT_GameInfo.getInstance().CoinList.RemoveAt(XLDT_GameInfo.getInstance().CoinList.Count - 1);
			if (XLDT_GameInfo.getInstance().CoinList.Count != 0)
			{
				ShowTip(XLDT_TipType.GivingCoin);
				if (language == 0)
				{
					txtTip.text = "恭喜您，获得客服赠送的" + (int)XLDT_GameInfo.getInstance().CoinList[XLDT_GameInfo.getInstance().CoinList.Count - 1] + "游戏币";
				}
				else
				{
					txtTip.text = "Congratulations.You have got " + (int)XLDT_GameInfo.getInstance().CoinList[XLDT_GameInfo.getInstance().CoinList.Count - 1] + " complimentary coins";
				}
			}
			else
			{
				HideTip();
			}
		}
		else
		{
			HideTip();
		}
	}

	public void ClickBtnCancel()
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		HideTip();
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
