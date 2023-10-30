using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRL_ScoreBank : CRL_MB_Singleton<CRL_ScoreBank>
{
	private GameObject _goContainer;

	private Text txtCoinNum;

	private Text txtScoreNum;

	private GameObject objErrorTip;

	private Text txtErrorTip;

	private bool _isExpeGold;

	private GameObject objWarning;

	private Text txtWarning;

	private CRL_BtnLongPress sptBLPKeyIn;

	private CRL_BtnLongPress sptBLPKeyOut;

	public Action onSetScoreAndGold;

	private Button BtnBg;

	private Button BtnKeyIn;

	private Button BtnKeyOut;

	private int _keepScore;

	private int _rate;

	private int language;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (CRL_MB_Singleton<CRL_ScoreBank>._instance == null)
		{
			CRL_MB_Singleton<CRL_ScoreBank>.SetInstance(this);
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
		sptBLPKeyIn = base.transform.Find("BtnKeyIn").GetComponent<CRL_BtnLongPress>();
		sptBLPKeyOut = base.transform.Find("BtnKeyOut").GetComponent<CRL_BtnLongPress>();
		BtnBg = base.transform.Find("BtnBg").GetComponent<Button>();
		BtnBg.onClick.AddListener(OnBtnBg_Click);
		BtnKeyIn = base.transform.Find("BtnKeyIn").GetComponent<Button>();
		BtnKeyIn.onClick.AddListener(OnBtnDeposit_Click);
		BtnKeyOut = base.transform.Find("BtnKeyOut").GetComponent<Button>();
		BtnKeyOut.onClick.AddListener(OnBtnWithdraw_Click);
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
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", HandleNetMsg_UpdateGoldAndScore);
		language = ((CRL_MySqlConnection.language == "en") ? 1 : 0);
		txtWarning.text = ((language != 0) ? "Cannot more than 900 thousands" : "取分不能超过90万分");
		sptBLPKeyIn.action = OnBtnDeposit_Click;
		sptBLPKeyOut.action = OnBtnWithdraw_Click;
	}

	public void InitBank()
	{
		int roomId = CRL_MySqlConnection.desk.roomId;
		_isExpeGold = (roomId == 1);
		int num = _isExpeGold ? CRL_MySqlConnection.user.expeGold : CRL_MySqlConnection.user.gameGold;
		txtCoinNum.text = num.ToString();
		txtScoreNum.text = CRL_MySqlConnection.credit.ToString();
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
		CRL_SoundManager.Instance.PlayClickAudio();
		UnityEngine.Debug.Log("OnBtnDeposit_Click");
		if (!CRL_LockManager.IsLocked("Deposit_Or_Withdraw"))
		{
			int a = CRL_MySqlConnection.desk.onceExchangeValue * CRL_MySqlConnection.desk.exchange;
			if (CRL_MySqlConnection.realCredit < CRL_MySqlConnection.desk.exchange + _keepScore)
			{
				_showBubble(isLeft: true);
			}
			else
			{
				Send_UserCoinOut(Mathf.Min(a, CRL_MySqlConnection.credit / CRL_MySqlConnection.desk.exchange * CRL_MySqlConnection.desk.exchange));
			}
		}
	}

	public void OnBtnWithdraw_Click()
	{
		CRL_SoundManager.Instance.PlayClickAudio();
		int rightGold = GetRightGold();
		if (rightGold == 0)
		{
			_showBubble(isLeft: false);
		}
		else if (int.Parse(txtScoreNum.text) < 900000)
		{
			int onceExchangeValue = CRL_MySqlConnection.desk.onceExchangeValue;
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
		CRL_MB_Singleton<CRL_MajorGameController>.GetInstance().SetBtnStartState(CRL_BtnState.start);
		HideWithAni();
	}

	public void Send_UserCoinIn(int coin)
	{
		CRL_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			coin
		};
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().Send("userService/userCoinIn", args);
	}

	public void Send_UserCoinOut(int score)
	{
		CRL_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			score
		};
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().Send("userService/userCoinOut", args);
	}

	public void HandleNetMsg_UpdateGoldAndScore(object[] args)
	{
		CRL_LockManager.UnLock("Deposit_Or_Withdraw");
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
		int keepScore = _keepScore;
		CRL_MySqlConnection.realCredit = newScore;
		CRL_MySqlConnection.credit = newScore - keepScore;
		txtCoinNum.text = string.Empty + newGold;
		txtScoreNum.text = string.Empty + CRL_MySqlConnection.credit;
		if (_isExpeGold)
		{
			CRL_MySqlConnection.user.expeGold = newGold;
		}
		else
		{
			CRL_MySqlConnection.user.gameGold = newGold;
		}
		if (onSetScoreAndGold != null)
		{
			onSetScoreAndGold();
		}
	}

	public static int GetRightGold()
	{
		return (CRL_MySqlConnection.desk.roomId == 1) ? CRL_MySqlConnection.user.expeGold : CRL_MySqlConnection.user.gameGold;
	}

	private IEnumerator CloseBubble()
	{
		yield return new WaitForSeconds(1f);
		objErrorTip.gameObject.SetActive(value: false);
	}
}
