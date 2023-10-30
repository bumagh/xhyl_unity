using System;
using UnityEngine;
using UnityEngine.UI;

public class BZJX_DlgInOut : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiError;

	[HideInInspector]
	public Text txtCoinValue;

	[HideInInspector]
	public Text txtCreditValue;

	private Text txtTip;

	private Button btnOut;

	private Button btnIn;

	private Image imgError;

	private BZJX_ErrorTipAnim sptErrorTipAnim;

	private BZJX_GameInfo gameInfo;

	private int language;

	private bool bOutDown;

	private bool bInDown;

	private bool bStartInOut;

	private float downTime;

	private void Awake()
	{
		gameInfo = BZJX_GameInfo.getInstance();
		language = gameInfo.Language;
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		txtCoinValue = base.transform.Find("TxtCoinValue").GetComponent<Text>();
		txtCreditValue = base.transform.Find("TxtCreditsValue").GetComponent<Text>();
		txtTip = base.transform.Find("TxtTip").GetComponent<Text>();
		txtTip.text = ((language != 0) ? "Tip:Credits can't be more than 9000000 when key in" : "注意：取分不可超过90万");
		btnOut = base.transform.Find("BtnOut").GetComponent<Button>();
		btnIn = base.transform.Find("BtnIn").GetComponent<Button>();
		imgError = base.transform.Find("ImgError").GetComponent<Image>();
		sptErrorTipAnim = imgError.GetComponent<BZJX_ErrorTipAnim>();
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
			BZJX_NetMngr.GetSingleton().MyCreateSocket.SendCoinOut(gameInfo.Table.DeltaCoin * gameInfo.Table.ScorePerCoin);
			BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.coinUp);
			return;
		}
		int scoreCount = gameInfo.User.ScoreCount;
		if (scoreCount > 0)
		{
			BZJX_NetMngr.GetSingleton().MyCreateSocket.SendCoinOut(scoreCount);
			BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.coinUp);
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
		UnityEngine.Debug.LogError("点击取分 房间: " + gameInfo.User.RoomId);
		int num = 0;
		if (gameInfo.User.RoomId == 0)
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
				BZJX_NetMngr.GetSingleton().MyCreateSocket.SendCoinIn(num);
				BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.coinUp);
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
			try
			{
				txtCoinValue.text = ((gameInfo.User.RoomId == 1) ? (gameInfo.IsSpecial ? (gameInfo.User.CoinCount % 10000).ToString() : gameInfo.User.CoinCount.ToString()) : (gameInfo.IsSpecial ? (gameInfo.User.TestCoinCount % 10000).ToString() : gameInfo.User.TestCoinCount.ToString()));
				txtCreditValue.text = gameInfo.User.ScoreCount.ToString();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
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
