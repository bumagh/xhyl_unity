using UnityEngine;
using UnityEngine.UI;

public class TF_DlgInOut : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiError;

	private Text txtCoinLabel;

	private Text txtCreditLabel;

	[HideInInspector]
	public Text txtCoinValue;

	[HideInInspector]
	public Text txtCreditValue;

	private Text txtTip;

	private Button btnOut;

	private Button btnIn;

	private Image imgError;

	private TF_ErrorTipAnim sptErrorTipAnim;

	private TF_GameInfo gameInfo;

	private int language;

	private bool bOutDown;

	private bool bInDown;

	private bool bStartInOut;

	private float downTime;

	private void Awake()
	{
		gameInfo = TF_GameInfo.getInstance();
		language = gameInfo.Language;
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		txtCoinLabel = base.transform.Find("TxtCoin").GetComponent<Text>();
		txtCoinLabel.text = ((language != 0) ? "Coins" : "币值");
		txtCoinValue = base.transform.Find("TxtCoinValue").GetComponent<Text>();
		txtCreditLabel = base.transform.Find("TxtCredits").GetComponent<Text>();
		txtCreditLabel.text = ((language != 0) ? "Credits" : "分值");
		txtCreditValue = base.transform.Find("TxtCreditsValue").GetComponent<Text>();
		txtTip = base.transform.Find("TxtTip").GetComponent<Text>();
		txtTip.text = ((language != 0) ? "Tip:Credits can't be more than 9000000 when key in" : "注意：取分不可超过90万");
		btnOut = base.transform.Find("BtnOut").GetComponent<Button>();
		btnIn = base.transform.Find("BtnIn").GetComponent<Button>();
		imgError = base.transform.Find("ImgError").GetComponent<Image>();
		sptErrorTipAnim = imgError.GetComponent<TF_ErrorTipAnim>();
		btnOut.onClick.AddListener(ClickBtnOut);
		btnIn.onClick.AddListener(ClickBtnIn);
	}

	public void ShowInOut()
	{
		base.gameObject.SetActive(value: true);
		UpdateCoinScore();
	}

	public void CheckUpDownCoin()
	{
		if (bOutDown)
		{
			downTime += Time.deltaTime;
			if (bStartInOut || downTime > 0.5f)
			{
				bStartInOut = true;
				if (downTime > 0.1f)
				{
					ClickBtnOut();
					downTime = 0f;
				}
			}
		}
		else
		{
			if (!bInDown)
			{
				return;
			}
			downTime += Time.deltaTime;
			if (bStartInOut || downTime > 0.5f)
			{
				bStartInOut = true;
				if (downTime > 0.1f)
				{
					ClickBtnIn();
					downTime = 0f;
				}
			}
		}
	}

	private void ClickBtnOut()
	{
		if (gameInfo.User.ScoreCount >= gameInfo.Table.DeltaCoin * gameInfo.Table.ScorePerCoin)
		{
			TF_NetMngr.GetSingleton().MyCreateSocket.SendCoinOut(gameInfo.Table.DeltaCoin * gameInfo.Table.ScorePerCoin);
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.coinUp);
			return;
		}
		int scoreCount = gameInfo.User.ScoreCount;
		if (scoreCount > 0)
		{
			TF_NetMngr.GetSingleton().MyCreateSocket.SendCoinOut(scoreCount);
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.coinUp);
		}
		else
		{
			imgError.sprite = spiError[language * 3];
			imgError.SetNativeSize();
			sptErrorTipAnim.PlayTipAnim();
		}
	}

	private void ClickBtnIn()
	{
		int num = 0;
		if (gameInfo.User.RoomId == 1)
		{
			if (gameInfo.User.TestCoinCount >= gameInfo.Table.DeltaCoin)
			{
				num = gameInfo.Table.DeltaCoin;
			}
			else if (gameInfo.User.TestCoinCount > 0)
			{
				num = gameInfo.User.TestCoinCount;
			}
		}
		else if (gameInfo.User.CoinCount >= gameInfo.Table.DeltaCoin)
		{
			num = gameInfo.Table.DeltaCoin;
		}
		else if (gameInfo.User.CoinCount > 0)
		{
			num = gameInfo.User.CoinCount;
		}
		if (num * gameInfo.Table.ScorePerCoin + gameInfo.User.ScoreCount <= 900000)
		{
			if (num > 0)
			{
				TF_NetMngr.GetSingleton().MyCreateSocket.SendCoinIn(num);
				TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.coinUp);
			}
			else
			{
				imgError.sprite = spiError[language * 3 + 2];
				imgError.SetNativeSize();
				sptErrorTipAnim.PlayTipAnim();
			}
		}
		else
		{
			imgError.sprite = spiError[language * 3 + 1];
			imgError.SetNativeSize();
			sptErrorTipAnim.PlayTipAnim();
		}
	}

	public void UpdateCoinScore()
	{
		if (base.gameObject.activeSelf)
		{
			txtCoinValue.text = ((gameInfo.User.RoomId != 1) ? (gameInfo.IsSpecial ? (gameInfo.User.CoinCount % 10000).ToString() : gameInfo.User.CoinCount.ToString()) : (gameInfo.IsSpecial ? (gameInfo.User.TestCoinCount % 10000).ToString() : gameInfo.User.TestCoinCount.ToString()));
			txtCreditValue.text = gameInfo.User.ScoreCount.ToString();
		}
	}

	public void DownOut()
	{
		bOutDown = true;
	}

	public void DownIn()
	{
		bInDown = true;
	}

	public void UpOut()
	{
		downTime = 0f;
		bOutDown = false;
		bStartInOut = false;
	}

	public void UpIn()
	{
		downTime = 0f;
		bInDown = false;
		bStartInOut = false;
	}
}
