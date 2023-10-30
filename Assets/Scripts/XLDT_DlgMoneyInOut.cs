using STDT_GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_DlgMoneyInOut : MonoBehaviour
{
	public Sprite[] spiBtnIn;

	public Sprite[] spiBtnOut;

	private Button btnIn;

	private Button btnOut;

	private XLDT_BtnLongPress blpIn;

	private XLDT_BtnLongPress blpOut;

	private Text txtCoin;

	private Text txtScore;

	private XLDT_TipAnim sptTA;

	private XLDT_GameInfo mGameInfo;

	private void Awake()
	{
		InitDlg();
	}

	private void Start()
	{
		mGameInfo = XLDT_GameInfo.getInstance();
		int language = XLDT_Localization.language;
		btnIn.GetComponent<Image>().sprite = spiBtnIn[language];
		btnOut.GetComponent<Image>().sprite = spiBtnOut[language];
		base.transform.Find("TxtNotice").GetComponent<Text>().text = XLDT_Localization.Get("DlgMoneyInOutTip");
		base.transform.Find("TxtLabelCoin").GetComponent<Text>().text = XLDT_Localization.Get("DlgMoneyInOutCoinValue");
		base.transform.Find("TxtLabelScore").GetComponent<Text>().text = XLDT_Localization.Get("DlgMoneyInOutScoreValue");
	}

	private void InitDlg()
	{
		btnIn = base.transform.Find("BtnIn").GetComponent<Button>();
		btnOut = base.transform.Find("BtnOut").GetComponent<Button>();
		blpIn = btnIn.transform.GetComponent<XLDT_BtnLongPress>();
		blpOut = btnOut.transform.GetComponent<XLDT_BtnLongPress>();
		txtCoin = base.transform.Find("TxtCoin").GetComponent<Text>();
		txtScore = base.transform.Find("TxtScore").GetComponent<Text>();
		sptTA = base.transform.Find("Tip").GetComponent<XLDT_TipAnim>();
		btnIn.onClick.AddListener(coinDown);
		btnOut.onClick.AddListener(coinUp);
		blpIn.action = coinDown;
		blpOut.action = coinUp;
	}

	public void OnBlackClick()
	{
		SendMessageUpwards("OnDlgBlackClick", XLDT_POP_DLG_TYPE.DLG_MONEY_INOUT);
	}

	public void coinUp()
	{
		int num = 0;
		if (mGameInfo.User.RoomId == 0)
		{
			if (mGameInfo.User.TestCoinCount >= mGameInfo.CurTable.DeltaCoin)
			{
				num = mGameInfo.CurTable.DeltaCoin;
			}
			else if (mGameInfo.User.TestCoinCount > 0)
			{
				num = mGameInfo.User.TestCoinCount;
			}
		}
		else if (mGameInfo.User.RoomId == 1)
		{
			if (mGameInfo.User.CoinCount >= mGameInfo.CurTable.DeltaCoin)
			{
				num = mGameInfo.CurTable.DeltaCoin;
			}
			else if (mGameInfo.User.CoinCount > 0)
			{
				num = mGameInfo.User.CoinCount;
			}
		}
		if (num * mGameInfo.CurTable.ScorePerCoin + mGameInfo.User.ScoreCount <= 900000)
		{
			if (num > 0)
			{
				if (!XLDT_DanTiaoCommon.G_TEST)
				{
					XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserCoinIn(num);
				}
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.coinUp);
			}
			else
			{
				SetMoneyTip(XLDT_Localization.Get("DlgMoneyInOutCoinShortage"));
			}
		}
		else
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
			SetMoneyTip(XLDT_Localization.Get("DlgMoneyInOutGameScoreExceeds90"));
		}
	}

	public void coinDown()
	{
		if (mGameInfo.User.ScoreCount >= mGameInfo.CurTable.DeltaCoin * mGameInfo.CurTable.ScorePerCoin)
		{
			if (!XLDT_DanTiaoCommon.G_TEST)
			{
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserCoinOut(mGameInfo.CurTable.DeltaCoin * mGameInfo.CurTable.ScorePerCoin);
			}
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.coinUp);
			return;
		}
		int num = mGameInfo.User.ScoreCount / mGameInfo.CurTable.ScorePerCoin * mGameInfo.CurTable.ScorePerCoin;
		if (num > 0)
		{
			if (!XLDT_DanTiaoCommon.G_TEST)
			{
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserCoinOut(num);
			}
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.coinUp);
		}
		else
		{
			SetMoneyTip(XLDT_Localization.Get("DlgMoneyInOutScoreShortage"));
		}
	}

	public void ShowCoinScore(long nCoin, long nScore)
	{
		if (!txtCoin)
		{
			txtCoin = base.transform.Find("TxtCoin").GetComponent<Text>();
		}
		txtCoin.text = XLDT_DanTiaoCommon.ChangeNumber(nCoin);
		if (!txtScore)
		{
			txtScore = base.transform.Find("TxtScore").GetComponent<Text>();
		}
		txtScore.text = XLDT_DanTiaoCommon.ChangeNumber(nScore);
	}

	public void ShowCoinScore()
	{
		if (XLDT_GameInfo.getInstance().User.RoomId == 0)
		{
			ShowCoinScore(XLDT_GameInfo.getInstance().User.TestCoinCount, XLDT_GameInfo.getInstance().User.ScoreCount);
		}
		else if (XLDT_GameInfo.getInstance().User.RoomId == 1)
		{
			ShowCoinScore(XLDT_GameInfo.getInstance().User.CoinCount, XLDT_GameInfo.getInstance().User.ScoreCount);
		}
	}

	public void SetMoneyTip(string words)
	{
		sptTA.Play(words);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
	}
}
