using DP_UICommon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DP_TipManager : MonoBehaviour
{
	private Transform tfGameTip;

	private Button btnConfirm;

	private Button btnCancel;

	private Button btnClose;

	private Text txtTip;

	private EGameTipType mTipType = EGameTipType.NoneTip;

	private static DP_TipManager gTipMgr;

	private static float gMsxNetDelay = 1f;

	private bool mIsNetTiming;

	private float mTotalNetDelay;

	private int language;

	private Color colTp;

	public EGameTipType GetTipType()
	{
		return mTipType;
	}

	public static DP_TipManager GetSingleton()
	{
		return gTipMgr;
	}

	private void Awake()
	{
		if (gTipMgr == null)
		{
			gTipMgr = this;
		}
	}

	public void Init()
	{
		tfGameTip = GameObject.Find("Tip").transform;
		btnConfirm = tfGameTip.Find("BtnConfirm").GetComponent<Button>();
		btnCancel = tfGameTip.Find("BtnCancel").GetComponent<Button>();
		btnClose = tfGameTip.Find("BtnClose").GetComponent<Button>();
		txtTip = tfGameTip.Find("TxtTip").GetComponent<Text>();
		language = DP_GameInfo.getInstance().Language;
		HideTip();
		btnConfirm.onClick.AddListener(ClickBtnConfirm);
		btnCancel.onClick.AddListener(ClickBtnCancel);
		btnClose.onClick.AddListener(ClickBtnCancel);
	}

	private void Update()
	{
		if (DP_NetMngr.shouldBeBlocked)
		{
			DP_NetMngr.shouldBeBlocked = false;
		}
		if (mIsNetTiming)
		{
			mTotalNetDelay += Time.deltaTime;
		}
	}

	public void ClickBtnConfirm()
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
			ZH2_GVars.isStartedFromGame = false;
			BackToMainGame();
			break;
		case EGameTipType.SelectTable_SendExpCoin:
			DP_NetMngr.GetSingleton().MyCreateSocket.SendAutoAddExpeGold();
			break;
		case EGameTipType.IsExitGame:
		case EGameTipType.TableParameterModified:
		case EGameTipType.TableDeleted_Game:
		case EGameTipType.LongTimeNoOperate:
		case EGameTipType.IsExitApplication:
			ZH2_GVars.isStartedFromGame = true;
			BackToMainGame();
			break;
		}
		DP_SoundManager.GetSingleton().playButtonSound();
		HideTip();
	}

	public void ClickBtnCancel()
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		HideTip();
	}

	private void HideTip()
	{
		mTipType = EGameTipType.NoneTip;
		tfGameTip.gameObject.SetActive(value: false);
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
			ShowTip(EGameTipType.PoorNetSignal);
		}
		mTotalNetDelay = 0f;
		mIsNetTiming = false;
	}

	public void ShowTip(EGameTipType eType)
	{
		UnityEngine.Debug.LogError("提示: " + eType);
		mTipType = eType;
		tfGameTip.gameObject.SetActive(value: true);
		btnConfirm.gameObject.SetActive(value: true);
		btnCancel.gameObject.SetActive(value: false);
		txtTip.gameObject.SetActive(value: true);
		btnConfirm.transform.localPosition = Vector3.down * 165f;
		txtTip.transform.localPosition = Vector3.up * 20f;
		switch (eType)
		{
		case EGameTipType.UserIDFrozen:
		case EGameTipType.SelectSeat_NotEmpty:
			break;
		case EGameTipType.Net_ConnectionError:
			txtTip.text = DP_TipContent.contents[language][0];
			break;
		case EGameTipType.LoseTheServer:
			txtTip.text = DP_TipContent.contents[language][1];
			break;
		case EGameTipType.CoinOverFlow:
			txtTip.text = DP_TipContent.contents[language][2];
			break;
		case EGameTipType.ApplyForExpCoin_Success:
			txtTip.text = DP_TipContent.contents[language][4];
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.GivingCoins);
			break;
		case EGameTipType.SelectTable_CreditBelowRistrict:
			txtTip.text = DP_TipContent.contents[language][7];
			break;
		case EGameTipType.SelectTable_SendExpCoin:
			txtTip.text = DP_TipContent.contents[language][8];
			break;
		case EGameTipType.RoomEmpty:
			txtTip.text = DP_TipContent.contents[language][3];
			break;
		case EGameTipType.IsExitGame:
			txtTip.text = DP_TipContent.contents[language][17];
			btnCancel.gameObject.SetActive(value: true);
			btnConfirm.transform.localPosition = Vector3.right * 150f + Vector3.up * -165f;
			break;
		case EGameTipType.TableParameterModified:
			txtTip.text = DP_TipContent.contents[language][6];
			break;
		case EGameTipType.TableDeleted_Game:
			txtTip.text = DP_TipContent.contents[language][5];
			break;
		case EGameTipType.LongTimeNoOperate:
			txtTip.text = DP_TipContent.contents[language][9];
			break;
		case EGameTipType.ServerUpdate:
			txtTip.text = DP_TipContent.contents[language][10];
			break;
		case EGameTipType.Game_UserIdFrozen:
			txtTip.text = DP_TipContent.contents[language][11];
			break;
		case EGameTipType.UserIdDeleted:
			txtTip.text = DP_TipContent.contents[language][13];
			break;
		case EGameTipType.UserIdRepeative:
			txtTip.text = DP_TipContent.contents[language][14];
			break;
		case EGameTipType.UserPwdChanged:
			txtTip.text = DP_TipContent.contents[language][15];
			break;
		case EGameTipType.UserIdPwdNotMatch:
			txtTip.text = DP_TipContent.contents[language][12];
			break;
		case EGameTipType.GivingCoin:
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.GivingCoins);
			break;
		case EGameTipType.IsExitApplication:
			txtTip.text = DP_TipContent.contents[language][16];
			btnCancel.gameObject.SetActive(value: true);
			btnConfirm.transform.localPosition = Vector3.right * 150f + Vector3.up * -165f;
			break;
		case EGameTipType.PoorNetSignal:
			txtTip.text = DP_TipContent.contents[language][18];
			break;
		}
	}

	public void BackToMainGame()
	{
		DP_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		SceneManager.LoadScene("MainScene");
		UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
	}
}
