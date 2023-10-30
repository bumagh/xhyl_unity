using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STWM_ScoreBank : STWM_MB_Singleton<STWM_ScoreBank>
{
	[SerializeField]
	private GameObject _goContainer;

	[SerializeField]
	private Text txtCoinLabel;

	[SerializeField]
	private Text txtCoinNum;

	[SerializeField]
	private Text txtScoreLabel;

	[SerializeField]
	private Text txtScoreNum;

	[SerializeField]
	private GameObject objErrorTip;

	[SerializeField]
	private Text txtErrorTip;

	private bool _isExpeGold;

	[SerializeField]
	private GameObject objWarning;

	[SerializeField]
	private Text txtWarning;

	[SerializeField]
	private Text txtBtnKeyIn;

	[SerializeField]
	private Text txtBtnKeyOut;

	[SerializeField]
	private STWM_BtnLongPress sptBLPKeyIn;

	[SerializeField]
	private STWM_BtnLongPress sptBLPKeyOut;

	public Action onSetScoreAndGold;

	private int _keepScore;

	private int _rate;

	private int language;

	private void Awake()
	{
		base.transform.localScale = Vector3.zero;
		if (STWM_MB_Singleton<STWM_ScoreBank>._instance == null)
		{
			STWM_MB_Singleton<STWM_ScoreBank>.SetInstance(this);
			PreInit();
		}
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
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", HandleNetMsg_UpdateGoldAndScore);
		language = (int)ZH2_GVars.language_enum;
		txtCoinLabel.text = ((language != 0) ? "Coins：" : "币值：");
		txtScoreLabel.text = ((language != 0) ? "Cerdits：" : "分值：");
		txtBtnKeyIn.text = ((language != 0) ? "KeyIn" : "存分");
		txtBtnKeyOut.text = ((language != 0) ? "KeyOut" : "取分");
		txtWarning.text = ((language != 0) ? "Cannot more than 900 thousands" : "取分不能超过90万分");
		objWarning.transform.localPosition = ((language != 0) ? new Vector3(-170f, -137f, 0f) : new Vector3(-110f, -137f, 0f));
		sptBLPKeyIn.action = OnBtnDeposit_Click;
		sptBLPKeyOut.action = OnBtnWithdraw_Click;
	}

	public void InitBank()
	{
		int roomId = STWM_GVars.desk.roomId;
		_isExpeGold = (roomId == 1);
		int num = _isExpeGold ? STWM_GVars.user.expeGold : STWM_GVars.user.gameGold;
		txtCoinNum.text = num.ToString();
		txtScoreNum.text = STWM_GVars.credit.ToString();
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
		base.transform.localScale = Vector3.zero;
	}

	public void Hide()
	{
		Show();
	}

	public void ShowWithAni()
	{
		Show();
	}

	public void HideWithAni()
	{
	}

	public void OnBtnDeposit_Click()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		UnityEngine.Debug.Log("OnBtnDeposit_Click");
		if (!STWM_LockManager.IsLocked("Deposit_Or_Withdraw"))
		{
			int a = STWM_GVars.desk.onceExchangeValue * STWM_GVars.desk.exchange;
			if (STWM_GVars.realCredit < STWM_GVars.desk.exchange + _keepScore)
			{
				_showBubble(isLeft: true);
			}
			else
			{
				Send_UserCoinOut(Mathf.Min(a, STWM_GVars.credit / STWM_GVars.desk.exchange * STWM_GVars.desk.exchange));
			}
		}
	}

	public void OnBtnWithdraw_Click()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		int rightGold = GetRightGold();
		if (rightGold == 0)
		{
			_showBubble(isLeft: false);
		}
		else if (int.Parse(txtScoreNum.text) < 900000)
		{
			int onceExchangeValue = STWM_GVars.desk.onceExchangeValue;
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
		HideWithAni();
	}

	public void Send_UserCoinIn(int coin)
	{
		STWM_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			coin
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/userCoinIn", args);
	}

	public void Send_UserCoinOut(int score)
	{
		STWM_LockManager.Lock("Deposit_Or_Withdraw");
		object[] args = new object[1]
		{
			score
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/userCoinOut", args);
	}

	public void HandleNetMsg_UpdateGoldAndScore(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg 更新金币和分数"));
		STWM_LockManager.UnLock("Deposit_Or_Withdraw");
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
		UnityEngine.Debug.LogError("newGold: " + newGold + " newScore: " + newScore);
		int keepScore = _keepScore;
		STWM_GVars.realCredit = newScore;
		STWM_GVars.credit = newScore - keepScore;
		txtCoinNum.text = string.Empty + newGold;
		txtScoreNum.text = string.Empty + STWM_GVars.credit;
		if (_isExpeGold)
		{
			STWM_GVars.user.expeGold = newGold;
		}
		else
		{
			STWM_GVars.user.gameGold = newGold;
		}
		if (onSetScoreAndGold != null)
		{
			onSetScoreAndGold();
		}
	}

	public static int GetRightGold()
	{
		return (STWM_GVars.desk.roomId == 1) ? STWM_GVars.user.expeGold : STWM_GVars.user.gameGold;
	}

	private IEnumerator CloseBubble()
	{
		yield return new WaitForSeconds(1f);
		objErrorTip.gameObject.SetActive(value: false);
	}
}
