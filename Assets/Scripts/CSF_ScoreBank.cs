using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSF_ScoreBank : CSF_MB_Singleton<CSF_ScoreBank>
{
	private GameObject _goContainer;

	private Text txtCoinNum;

	private Text txtScoreNum;

	private GameObject objErrorTip;

	private Text txtErrorTip;

	private bool _isExpeGold;

	private GameObject objWarning;

	private Text txtWarning;

	private CSF_BtnLongPress sptBLPKeyIn;

	private CSF_BtnLongPress sptBLPKeyOut;

	public Action onSetScoreAndGold;

	private int _keepScore;

	private int _rate;

	private int language;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (CSF_MB_Singleton<CSF_ScoreBank>._instance == null)
		{
			CSF_MB_Singleton<CSF_ScoreBank>.SetInstance(this);
			PreInit();
		}
	}

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		txtCoinNum = base.transform.Find("TxtCoin").GetComponent<Text>();
		txtScoreNum = base.transform.Find("TxtTicket").GetComponent<Text>();
		objErrorTip = base.transform.Find("ErrorTip").gameObject;
		txtErrorTip = objErrorTip.transform.Find("Text").GetComponent<Text>();
		objWarning = base.transform.Find("ImgWarn").gameObject;
		txtWarning = base.transform.Find("TxtWarn").GetComponent<Text>();
		sptBLPKeyIn = base.transform.Find("BtnKeyIn").GetComponent<CSF_BtnLongPress>();
		sptBLPKeyOut = base.transform.Find("BtnKeyOut").GetComponent<CSF_BtnLongPress>();
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
	}

	public void Init()
	{
		objErrorTip.SetActive(value: false);
		_rate = 1;
		_keepScore = 0;
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", HandleNetMsg_UpdateGoldAndScore);
		language = ((CSF_MySqlConnection.language == "en") ? 1 : 0);
		txtWarning.text = ((language != 0) ? "Cannot more than 900 thousands" : "取分不能超过90万分");
		sptBLPKeyIn.action = OnBtnDeposit_Click;
		sptBLPKeyOut.action = OnBtnWithdraw_Click;
	}

	public void InitBank()
	{
		int roomId = CSF_MySqlConnection.desk.roomId;
		_isExpeGold = (roomId == 1);
		int num = _isExpeGold ? CSF_MySqlConnection.user.expeGold : CSF_MySqlConnection.user.gameGold;
		txtCoinNum.text = num.ToString();
		txtScoreNum.text = CSF_MySqlConnection.credit.ToString();
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void ShowWithAni()
	{
		Show();
	}

	public void HideWithAni()
	{
		_goContainer.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(delegate
		{
			_goContainer.transform.localScale = Vector3.one;
			Hide();
		});
	}

	public void OnBtnDeposit_Click()
	{
		CSF_SoundManager.Instance.PlayClickAudio();
		UnityEngine.Debug.Log("OnBtnDeposit_Click");
		if (!CSF_LockManager.IsLocked("Deposit_Or_Withdraw"))
		{
			int a = CSF_MySqlConnection.desk.onceExchangeValue * CSF_MySqlConnection.desk.exchange;
			if (CSF_MySqlConnection.realCredit < CSF_MySqlConnection.desk.exchange + _keepScore)
			{
				_showBubble(isLeft: true);
			}
			else
			{
				Send_UserCoinOut(Mathf.Min(a, CSF_MySqlConnection.credit / CSF_MySqlConnection.desk.exchange * CSF_MySqlConnection.desk.exchange));
			}
		}
	}

	public void OnBtnWithdraw_Click()
	{
		CSF_SoundManager.Instance.PlayClickAudio();
		int rightGold = GetRightGold();
		if (rightGold == 0)
		{
			_showBubble(isLeft: false);
		}
		else if (int.Parse(txtScoreNum.text) < 900000)
		{
			int onceExchangeValue = CSF_MySqlConnection.desk.onceExchangeValue;
			Send_UserCoinIn(Mathf.Min(rightGold, onceExchangeValue));
		}
	}

	private void _showBubble(bool isLeft)
	{
		string text = (!isLeft) ? ((language == 0) ? "币值不足" : "Coins shortage") : ((language == 0) ? "分值不足" : "Credits shortage");
		objErrorTip.SetActive(value: true);
		objErrorTip.transform.localPosition = (isLeft ? new Vector2(-120f, -15f) : new Vector2(120f, -12f));
		txtErrorTip.text = text;
		StartCoroutine(CloseBubble());
	}

	public void SetKeepScoreAndRate(int keepScore, int rate)
	{
		_keepScore = keepScore;
		_rate = rate;
	}

	public void SetKeepScore(int keepScore)
	{
		_keepScore = keepScore;
	}

	public void SetRate(int rate)
	{
		_rate = rate;
	}

	public void OnBtnBg_Click()
	{
		CSF_MB_Singleton<CSF_MajorGameController>.GetInstance().SetBtnStartState(CSF_BtnState.start);
		HideWithAni();
	}

	public void Send_UserCoinIn(int coin)
	{
		CSF_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			coin
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/userCoinIn", args);
	}

	public void Send_UserCoinOut(int score)
	{
		CSF_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			score
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/userCoinOut", args);
	}

	public void HandleNetMsg_UpdateGoldAndScore(object[] args)
	{
		CSF_LockManager.UnLock("Deposit_Or_Withdraw");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (dictionary.ContainsKey("gold") && dictionary.ContainsKey("gameScore"))
		{
			int newGold = (int)dictionary["gold"];
			int newScore = (int)dictionary["gameScore"];
			SetGoldAndScore(newGold, newScore);
		}
	}

	public void SetGoldAndScore(int newGold, int newScore)
	{
		UnityEngine.Debug.Log(CSF_LogHelper.Magenta(" 更新金币和分数 credit: {0}, realCredit: {1}, newScore: {2}, newGold: {3}, _keepScore: {4}", CSF_MySqlConnection.credit, CSF_MySqlConnection.realCredit, newGold, newScore, _keepScore));
		int keepScore = _keepScore;
		CSF_MySqlConnection.realCredit = newScore;
		CSF_MySqlConnection.credit = newScore - keepScore;
		txtCoinNum.text = string.Empty + newGold;
		txtScoreNum.text = string.Empty + CSF_MySqlConnection.credit;
		if (_isExpeGold)
		{
			CSF_MySqlConnection.user.expeGold = newGold;
		}
		else
		{
			CSF_MySqlConnection.user.gameGold = newGold;
		}
		if (onSetScoreAndGold != null)
		{
			onSetScoreAndGold();
		}
	}

	public static int GetRightGold()
	{
		return (CSF_MySqlConnection.desk.roomId == 1) ? CSF_MySqlConnection.user.expeGold : CSF_MySqlConnection.user.gameGold;
	}

	private IEnumerator CloseBubble()
	{
		yield return new WaitForSeconds(1f);
		objErrorTip.gameObject.SetActive(value: false);
	}
}
