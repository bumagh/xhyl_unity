using BCBM_GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCBM_GameMngr : MonoBehaviour
{
	public static BCBM_GameMngr G_GameMngr;

	private int m_nAnimalNo;

	private AnimalType m_ZeroAnimalTyp;

	private int m_IniAnimalNo = 5;

	private GameObject m_CurPrizeAnimal;

	private GameStates m_GameState;

	private BCBM_PrizeParameter mPrizePara = new BCBM_PrizeParameter();

	private Queue<PrizeAnimalState> _AnimalPrizeQue = new Queue<PrizeAnimalState>();

	private PrizeAnimalState mCurPrizeAnimalState = default(PrizeAnimalState);

	private bool _isLastAnimalRoundEnd = true;

	private bool _isLastBetRoundEnd = true;

	private float _fPlayWinMaxTime = 8f;

	private float _fPlayWinTime;

	private float _fCurTime;

	private float _fStateMaxTime;

	private long lCurTickTime;

	private float fDelayTime;

	private bool _IsPause;

	private bool _IsReset;

	private static int s_testAnimal;

	public static BCBM_GameMngr GetSingleton()
	{
		return G_GameMngr;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!BCBM_Parameter.G_Test || pauseStatus)
		{
		}
		_IsPause = pauseStatus;
		if (_IsPause)
		{
			lCurTickTime = DateTime.Now.Ticks;
		}
		else
		{
			_IsReset = true;
		}
	}

	private void Awake()
	{
		Application.runInBackground = true;
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		init();
	}

	private void Update()
	{
		BackGroundCheck();
	}

	public IEnumerator TestShowText()
	{
		StopCoroutine("TestShowText");
		yield return new WaitForSeconds(3f);
	}

	public void BackGroundCheck()
	{
		if (_IsReset)
		{
			_IsReset = false;
			long num = DateTime.Now.Ticks - lCurTickTime;
			if (num > 120000000)
			{
				BCBM_NetMngr.GetSingleton().MyPostMessageThread.ClearAllState();
			}
		}
	}

	private void init()
	{
		if (G_GameMngr == null)
		{
			G_GameMngr = this;
		}
		m_ZeroAnimalTyp = AnimalType.Monkey;
		m_GameState = GameStates.GAME_PLAY_Wait;
		OptimizeAllMesh();
		StartCoroutine(NetGetNotice());
	}

	private IEnumerator NetGetNotice()
	{
		yield return new WaitForSeconds(3f);
	}

	private void OptimizeAllMesh()
	{
	}

	public void ClearPrizeQue()
	{
		_isLastAnimalRoundEnd = true;
		_AnimalPrizeQue.Clear();
	}

	public void SetGameState(GameStates state)
	{
		m_GameState = state;
		_fPlayWinTime = 0f;
		switch (state + 1)
		{
		case GameStates.GAME_Play_Animal_GoCenter_Animation:
			_fCurTime = 0f;
			_fStateMaxTime = 2f;
			break;
		case (GameStates)4:
			mCurPrizeAnimalState._prizeTyp = PrizeType.PRIZE_None;
			_fCurTime = 0f;
			_fStateMaxTime = 6f;
			break;
		}
	}

	public void Reset()
	{
		StopCoroutine("_OneAnimalProcess");
		StopCoroutine("_AllCaiJinProcess");
		StopCoroutine("_AllSongDengProcess");
		StopCoroutine("_AllShanDianProcess");
		StopCoroutine("_AllDaSanYuanProcess");
		StopCoroutine("_AllDaSiXiProcess");
		StopCoroutine("_LuckySongDengProcess");
		StopCoroutine("_LuckyBonusProcess");
		StopCoroutine("_LuckyLuckyLightningProcess");
		StopAllCoroutines();
		ClearPrizeQue();
		SetGameState(GameStates.GAME_PLAY_Wait);
		if (BCBM_EffectMngr.GetSingleton() != null)
		{
			BCBM_EffectMngr.GetSingleton().Reset();
		}
		if (BCBM_MusicMngr.GetSingleton() != null)
		{
			BCBM_MusicMngr.GetSingleton().ResetWithoutUI();
		}
		if (BCBM_UITimesPrize.GetSingleton() != null)
		{
			BCBM_UITimesPrize.GetSingleton().Reset();
		}
	}
}
