using LL_GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_GameMngr : MonoBehaviour
{
	public static LL_GameMngr G_GameMngr;

	public GameObject m_TurnPlatform_Prefab;

	public LL_ColorBoard m_LightPointer;

	public LL_LuckyPrizeCtrl m_LuckyPrizeCtrl;

	public LL_SpinScript m_AnimalSpinCtrl;

	public List<GameObject> m_All_Obj_List;

	private int m_nAnimalNo;

	private AnimalType m_ZeroAnimalTyp;

	private int m_IniAnimalNo = 5;

	private GameObject m_CurPrizeAnimal;

	private LL_Script_Camera m_CameraCtrl;

	public GameObject mCamera;

	private GameStates m_GameState;

	public LL_AnimalMngr mAnimalMngr;

	private LL_PrizeParameter mPrizePara = new LL_PrizeParameter();

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

	public Texture btnTexture;

	private static int s_testAnimal;

	public static LL_GameMngr GetSingleton()
	{
		return G_GameMngr;
	}

	public void PlayAnimation(bool isPlay)
	{
		IEnumerator enumerator = m_TurnPlatform_Prefab.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				IEnumerator enumerator2 = transform.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object current2 = enumerator2.Current;
						Transform transform2 = (Transform)current2;
						if (transform2.CompareTag("Animal"))
						{
							if (isPlay)
							{
								transform2.GetComponent<Animation>().Play();
								transform2.GetComponent<Animation>().wrapMode = WrapMode.Loop;
							}
							else
							{
								transform2.GetComponent<Animation>().Stop();
								transform2.GetComponent<Animation>().wrapMode = WrapMode.Once;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (LL_Parameter.G_Test)
		{
			if (pauseStatus)
			{
				UnityEngine.Debug.Log("OnApplicationPause");
			}
			else
			{
				UnityEngine.Debug.Log("OnApplicationBack");
			}
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
		PlayAnimation(isPlay: true);
	}

	private void Update()
	{
		BackGroundCheck();
	}

	private void OnGUI()
	{
		if (!LL_Parameter.G_Test)
		{
			return;
		}
		if (!btnTexture)
		{
			UnityEngine.Debug.LogError("Please assign a texture on the inspector");
			return;
		}
		if (GUI.Button(new Rect(10f, 10f, 50f, 50f), btnTexture))
		{
			LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Game);
			LL_AppUIMngr.GetSingleton().mHudManager.ShowHud();
			m_nAnimalNo = (m_nAnimalNo + 1) % 4;
			Reset();
			TEST_OneAnimal();
		}
		if (GUI.Button(new Rect(10f, 100f, 50f, 50f), "Next"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			UnityEngine.Debug.Log("Clicked the button with text");
		}
		if (GUI.Button(new Rect(10f, 150f, 50f, 50f), btnTexture))
		{
			StartCoroutine(TestHideUI());
		}
	}

	public void BackGroundCheck()
	{
		if (_IsReset)
		{
			_IsReset = false;
			long num = DateTime.Now.Ticks - lCurTickTime;
			if (num > 120000000)
			{
				LL_NetMngr.GetSingleton().MyPostMessageThread.ClearAllState();
			}
		}
	}

	private IEnumerator TestHideUI()
	{
		LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_RoomList_Panel);
		yield return new WaitForSeconds(0.5f);
		LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_TableList_Panel);
		yield return new WaitForSeconds(0.5f);
		LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Table);
		yield return new WaitForSeconds(0.5f);
		LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Game);
		LL_AppUIMngr.GetSingleton().mHudManager.SetGameCD();
	}

	private void init()
	{
		if (G_GameMngr == null)
		{
			G_GameMngr = this;
		}
		mCamera = GameObject.Find("MainCamera");
		m_CameraCtrl = mCamera.GetComponent<LL_Script_Camera>();
		mAnimalMngr = base.gameObject.GetComponent<LL_AnimalMngr>();
		m_ZeroAnimalTyp = AnimalType.Monkey;
		m_GameState = GameStates.GAME_PLAY_Wait;
		m_AnimalSpinCtrl = m_TurnPlatform_Prefab.GetComponent<LL_SpinScript>();
		m_LightPointer = GameObject.Find("ColorBoard").GetComponent<LL_ColorBoard>();
		m_LuckyPrizeCtrl = GameObject.Find("LuckySpin").GetComponent<LL_LuckyPrizeCtrl>();
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

	public void GoAnimalPrize(AnimalType typ, int lightIndex)
	{
		int num = (typ - m_ZeroAnimalTyp + 4) % 4;
		int nNo = (lightIndex + num) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(lightIndex, 10f);
		m_AnimalSpinCtrl.SpinTo(nNo, 15f);
		m_LuckyPrizeCtrl.SpinLuckyPrize(0, LL_LuckySpin.LuckyType.LUCKY_NONE, 10f, 100f);
		int index = (m_IniAnimalNo + num) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(index);
		AnimalColor lightColor = m_LightPointer.GetLightColor2(lightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(index, lightColor);
	}

	public void GoMoreAnimalPrize(AnimalType typ, int index, AnimalColor color)
	{
		int num = (typ - m_ZeroAnimalTyp + 4) % 4;
		int num2 = UnityEngine.Random.Range(0, LL_Parameter.G_nAnimalNumber);
		int num3 = (num2 + num) % LL_Parameter.G_nAnimalNumber;
		num3 = (num3 + index * 4) % LL_Parameter.G_nAnimalNumber;
		m_AnimalSpinCtrl.StartSpinTo(num2);
		m_LightPointer.GoPointer_ToLightNo(num3, 15f);
		int index2 = (m_IniAnimalNo + num + index * 4) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(index2);
	}

	public void AddToPrizeQue(PrizeAnimalState prizeAnimal)
	{
		_AnimalPrizeQue.Enqueue(prizeAnimal);
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
		case GameStates.GAME_PLAY_Animal_Bounce:
			mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
			break;
		case GameStates.GAME_Play_Animal_GoCenter_Animation:
			_fCurTime = 0f;
			_fStateMaxTime = 2f;
			break;
		case (GameStates)4:
			mCurPrizeAnimalState._prizeTyp = PrizeType.PRIZE_None;
			_fCurTime = 0f;
			_fStateMaxTime = 6f;
			LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
			break;
		}
	}

	public void ResetLuckySpinOffset()
	{
		m_LuckyPrizeCtrl.ResetPrize();
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
		if (LL_AppUIMngr.GetSingleton() != null)
		{
			LL_AppUIMngr.GetSingleton().mPrizeResult.HidePrizeResult();
		}
		m_AnimalSpinCtrl.Reset();
		m_LightPointer.Reset();
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Wait);
		m_CameraCtrl.PlayAnimation(isPlay: false);
		m_LightPointer.Reset();
		m_LuckyPrizeCtrl.Reset();
		SetGameState(GameStates.GAME_PLAY_Wait);
		LL_EffectMngr.GetSingleton().Reset();
		LL_MusicMngr.GetSingleton().ResetWithoutUI();
		LL_UITimesPrize.GetSingleton().Reset();
	}

	public void TEST_OneAnimal()
	{
		s_testAnimal = (s_testAnimal + 1) % 4;
		GoOneAnimal((AnimalType)s_testAnimal, 12, 24f, 15f);
		int[] array = new int[4];
		int[] array2 = new int[4];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 3;
			array2[i] = i;
		}
		GoAllSongDeng(array, array2);
	}

	public void GoOneAnimal(AnimalType animal, int nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		if (_checkLightIndex(nLightIndex))
		{
			mPrizePara.mAnimal = animal;
			mPrizePara.mnLightIndex = nLightIndex;
			mPrizePara.mfAnimalSpinTime = fAnimalSpinTime;
			mPrizePara.mfLightPointerSpinTime = fLightPointerSpinTime;
			StartCoroutine("_OneAnimalProcess", mPrizePara);
		}
	}

	private IEnumerator _OneAnimalProcess(LL_PrizeParameter prize)
	{
		AnimalType animal = prize.mAnimal;
		int nLightIndex = prize.mnLightIndex;
		float fAnimalSpinTime = prize.mfAnimalSpinTime;
		float fLightPointerSpinTime = prize.mfLightPointerSpinTime;
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		LL_MusicMngr.GetSingleton().PlaySceneSound(LL_MusicMngr.MUSIC_SCENE_MUSIC.SCENE_SPIN_ANIMAL);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		int nRandomLuckyTyp = UnityEngine.Random.Range(1, 10) % 3 + 1;
		m_LuckyPrizeCtrl.SpinLuckyPrize(0, (LL_LuckySpin.LuckyType)nRandomLuckyTyp, 10f, 30f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		int tmp = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
		AnimalColor currentColor = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor);
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_END);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce_And_GoCenter);
		m_CameraCtrl.PlayAnimation(isPlay: true);
		LL_EffectMngr.GetSingleton().SetWallColorState((LL_BackWallAnim.WALL_COLOR_TYPE)currentColor);
		yield return new WaitForSeconds(2.4f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(6.6f);
		LL_MusicMngr.GetSingleton().PlayNormalAnimalResult((int)currentColor, _musicConvertAnimalTyp(animal));
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoAllCaiJin(AnimalType animal, int nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		if (_checkLightIndex(nLightIndex))
		{
			mPrizePara.mAnimal = animal;
			mPrizePara.mnLightIndex = nLightIndex;
			mPrizePara.mfAnimalSpinTime = fAnimalSpinTime;
			mPrizePara.mfLightPointerSpinTime = fLightPointerSpinTime;
			StartCoroutine("_AllCaiJinProcess", mPrizePara);
		}
	}

	private IEnumerator _AllCaiJinProcess(LL_PrizeParameter prize)
	{
		AnimalType animal = prize.mAnimal;
		int nLightIndex = prize.mnLightIndex;
		float fAnimalSpinTime = prize.mfAnimalSpinTime;
		float fLightPointerSpinTime = prize.mfLightPointerSpinTime;
		mAnimalMngr.ShineAllAnimal(isShine: true);
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_PRIZE_PRE);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(4.8f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(1.2f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		int tmp = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
		AnimalColor currentColor = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor);
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		mAnimalMngr.ShineAllAnimal(isShine: false);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce_And_GoCenter);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().ShowGold(isShow: true);
		LL_EffectMngr.GetSingleton().SetWallColorState((LL_BackWallAnim.WALL_COLOR_TYPE)currentColor);
		m_CameraCtrl.PlayAnimation(isPlay: true);
		yield return new WaitForSeconds(1f);
		LL_EffectMngr.GetSingleton().ShowFireWorksRandom(isShow: true);
		yield return new WaitForSeconds(2f);
		LL_EffectMngr.GetSingleton().ShowWallAnim(isShow: true);
		yield return new WaitForSeconds(2f);
		LL_EffectMngr.GetSingleton().ShowAllCaiJinSatellite(isShow: true);
		yield return new WaitForSeconds(4f);
		LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_CAIJIN);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoAllSongDeng(int[] animal, int[] nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		for (int i = 0; i < nLightIndex.Length; i++)
		{
			if (!_checkLightIndex(nLightIndex[i]))
			{
				return;
			}
		}
		LL_PrizeParameter[] array = new LL_PrizeParameter[animal.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new LL_PrizeParameter();
			array[j].mAnimal = AnimalType.Lion;
			array[j].mAnimal = (AnimalType)animal[j];
			array[j].mnLightIndex = nLightIndex[j];
			array[j].mfAnimalSpinTime = fAnimalSpinTime;
			array[j].mfLightPointerSpinTime = fLightPointerSpinTime;
		}
		StartCoroutine("_AllSongDengProcess", array);
	}

	private IEnumerator _AllSongDengProcess(LL_PrizeParameter[] prize)
	{
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_PRIZE_PRE);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(4.8f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(1.2f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		LL_EffectMngr.GetSingleton().SetSongDengTVShow(isShow: true, -1, withAnim: true);
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_SONGDENG);
		int[] nEveryAnimalIndex = new int[4];
		for (int j = 0; j < nEveryAnimalIndex.Length; j++)
		{
			nEveryAnimalIndex[j] = UnityEngine.Random.Range(0, 5);
		}
		for (int i = 0; i < prize.Length; i++)
		{
			AnimalType animal = prize[i].mAnimal;
			int nLightIndex = prize[i].mnLightIndex;
			float fAnimalSpinTime = prize[i].mfAnimalSpinTime;
			float fLightPointerSpinTime = prize[i].mfLightPointerSpinTime;
			int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
			int nAnimalIndex = (nLightIndex + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			int tmp = (m_IniAnimalNo + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
			AnimalColor currentColor = m_LightPointer.GetLightColor2(nLightIndex);
			mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor);
			m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
			mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
			m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
			nEveryAnimalIndex[(int)animal]++;
			float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
			float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
			yield return new WaitForSeconds(lessTime);
			LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
			yield return new WaitForSeconds(waitTime - lessTime);
			m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
			LL_EffectMngr.GetSingleton().SetSongDengTVShow(isShow: true, prize.Length - i - 1);
			LL_MusicMngr.GetSingleton().PlayNormalAnimalResult((int)currentColor, _musicConvertAnimalTyp(animal));
			yield return new WaitForSeconds(1f);
			int nUITyp = (int)(_musicConvertAnimalTyp(animal) * 3 + currentColor);
			LL_UITimesPrize.GetSingleton().AddAnimal(nUITyp);
		}
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		yield return new WaitForSeconds(2f);
		LL_UITimesPrize.GetSingleton().Reset();
		LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_SONGDENG);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoAllShanDian(int xNum, AnimalType animal, int nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		if (_checkLightIndex(nLightIndex))
		{
			mPrizePara.mAnimal = animal;
			mPrizePara.mnLightIndex = nLightIndex;
			mPrizePara.mfAnimalSpinTime = fAnimalSpinTime;
			mPrizePara.mfLightPointerSpinTime = fLightPointerSpinTime;
			mPrizePara.mMoreInfoValue = xNum;
			StartCoroutine("_AllShanDianProcess", mPrizePara);
		}
	}

	private IEnumerator _AllShanDianProcess(LL_PrizeParameter prize)
	{
		AnimalType animal = prize.mAnimal;
		int nLightIndex = prize.mnLightIndex;
		float fAnimalSpinTime = prize.mfAnimalSpinTime;
		float fLightPointerSpinTime = prize.mfLightPointerSpinTime;
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_PRIZE_PRE);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(4.8f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(0.6f);
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_All_LIGHTNING);
		yield return new WaitForSeconds(0.6f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		LL_EffectMngr.GetSingleton().SetAllShanDianParticleShow(isShow: true);
		if (prize.mMoreInfoValue == 2)
		{
			LL_EffectMngr.GetSingleton().SetAllShanDianShow(isShow: true, isDouble: true);
		}
		else
		{
			LL_EffectMngr.GetSingleton().SetAllShanDianShow(isShow: true, isDouble: false);
		}
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		int tmp = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
		AnimalColor currentColor = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor);
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce_And_GoCenter);
		LL_EffectMngr.GetSingleton().SetWallColorState((LL_BackWallAnim.WALL_COLOR_TYPE)currentColor);
		m_CameraCtrl.PlayAnimation(isPlay: true);
		yield return new WaitForSeconds(9f);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_LIGHTING);
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoAllDaSanYuan(AnimalType animal, int nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		if (_checkLightIndex(nLightIndex))
		{
			mPrizePara.mAnimal = animal;
			mPrizePara.mnLightIndex = nLightIndex;
			mPrizePara.mfAnimalSpinTime = fAnimalSpinTime;
			mPrizePara.mfLightPointerSpinTime = fLightPointerSpinTime;
			StartCoroutine("_AllDaSanYuanProcess", mPrizePara);
		}
	}

	private IEnumerator _AllDaSanYuanProcess(LL_PrizeParameter prize)
	{
		AnimalType animal = prize.mAnimal;
		int nLightIndex = prize.mnLightIndex;
		float fAnimalSpinTime = prize.mfAnimalSpinTime;
		float fLightPointerSpinTime = prize.mfLightPointerSpinTime;
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_PRIZE_PRE);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(4.8f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(1.2f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		LL_EffectMngr.GetSingleton().SetAllDaSanYuanShow(isShow: true);
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		int tmp = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
		AnimalColor currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor2);
		m_LightPointer.SetSpecial();
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		LL_EffectMngr.GetSingleton().SetAllDaSanYuanShowOneAnimal((int)animal);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		for (int j = 0; j < 6; j++)
		{
			int lightShineQuickly = (nLightIndex - j * 4 + LL_Parameter.G_nAnimalNumber) % LL_Parameter.G_nAnimalNumber;
			m_LightPointer.SetLightShineQuickly(lightShineQuickly);
		}
		for (int i = 0; i < 6; i++)
		{
			int nextAnimal = (m_IniAnimalNo + deltaNo + i * 4) % LL_Parameter.G_nAnimalNumber;
			m_CurPrizeAnimal = mAnimalMngr.GetAnimal(nextAnimal);
			int nNewLightIndex = (nLightIndex - i * 4 + LL_Parameter.G_nAnimalNumber) % LL_Parameter.G_nAnimalNumber;
			currentColor2 = m_LightPointer.GetLightColor2(nNewLightIndex);
			mAnimalMngr.SetColorOfAnimalRingByIndex(nextAnimal, currentColor2);
			m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
			yield return new WaitForSeconds(1f);
		}
		yield return new WaitForSeconds(3f);
		switch (animal)
		{
		case AnimalType.Lion:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASANYUAN_LION);
			break;
		case AnimalType.Panda:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASANYUAN_PANDA);
			break;
		case AnimalType.Monkey:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASANYUAN_MONKEY);
			break;
		default:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASANYUAN_RABBIT);
			break;
		}
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoAllDaSiXi(int nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		if (_checkLightIndex(nLightIndex))
		{
			mPrizePara.mnLightIndex = nLightIndex;
			mPrizePara.mfAnimalSpinTime = fAnimalSpinTime;
			mPrizePara.mfLightPointerSpinTime = fLightPointerSpinTime;
			StartCoroutine("_AllDaSiXiProcess", mPrizePara);
		}
	}

	private IEnumerator _AllDaSiXiProcess(LL_PrizeParameter prize)
	{
		int nLightIndex = prize.mnLightIndex;
		float fAnimalSpinTime = prize.mfAnimalSpinTime;
		float fLightPointerSpinTime = prize.mfLightPointerSpinTime;
		LL_MusicMngr.GetSingleton().PlayEffectSound(LL_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_ALL_PRIZE_PRE);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(4.8f);
		m_LuckyPrizeCtrl.FlipClosePlatform(0f, 1.2f);
		yield return new WaitForSeconds(1.2f);
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		LL_EffectMngr.GetSingleton().SetAllDaSiXiShow(isShow: true);
		int nAnimal = UnityEngine.Random.Range(0, 3);
		int deltaNo = (int)(nAnimal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		int tmp = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp);
		AnimalColor currentColor = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp, currentColor);
		m_LightPointer.SetSpecial();
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		LL_EffectMngr.GetSingleton().SetAllDaSiXiShowOneColor(currentColor);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		ArrayList animalFrontofLight = new ArrayList();
		for (int j = 0; j < 24; j++)
		{
			int num = (nLightIndex + j) % 24;
			int index = (tmp - j + 24) % 24;
			if (m_LightPointer.GetLightColor2(num) == currentColor)
			{
				m_LightPointer.SetLightShineQuickly(num);
				GameObject animal = mAnimalMngr.GetAnimal(index);
				mAnimalMngr.SetColorOfAnimalRingByIndex(index, currentColor);
				animalFrontofLight.Add(animal);
			}
		}
		for (int i = 0; i < animalFrontofLight.Count; i++)
		{
			GameObject prizeAnimal = (GameObject)animalFrontofLight[i];
			prizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
			yield return new WaitForSeconds(1f);
		}
		animalFrontofLight.Clear();
		yield return new WaitForSeconds(3f);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		switch (currentColor)
		{
		case AnimalColor.Animal_Red:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASIXI_RED);
			break;
		case AnimalColor.Animal_Green:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASIXI_GREEN);
			break;
		default:
			LL_MusicMngr.GetSingleton().PlayResultSound(LL_MusicMngr.MUSIC_BROADCAST_MUSIC.ALL_DASIXI_YELLOW);
			break;
		}
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoLuckySongDeng(int[] animal, int nLuckyTaihao, int[] nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		for (int i = 0; i < nLightIndex.Length; i++)
		{
			if (!_checkLightIndex(nLightIndex[i]))
			{
				return;
			}
		}
		LL_PrizeParameter[] array = new LL_PrizeParameter[animal.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new LL_PrizeParameter();
			array[j].mAnimal = AnimalType.Lion;
			array[j].mAnimal = (AnimalType)animal[j];
			array[j].mnLightIndex = nLightIndex[j];
			array[j].mnLuckyNum = nLuckyTaihao;
			if (j == 0)
			{
				array[0].mfAnimalSpinTime = 21f;
				array[0].mfLightPointerSpinTime = 15f;
			}
			else
			{
				array[j].mfAnimalSpinTime = fAnimalSpinTime;
				array[j].mfLightPointerSpinTime = fLightPointerSpinTime;
			}
		}
		StartCoroutine("_LuckySongDengProcess", array);
	}

	private IEnumerator _LuckySongDengProcess(LL_PrizeParameter[] prize)
	{
		AnimalType animal = prize[0].mAnimal;
		int nLightIndex = prize[0].mnLightIndex;
		float fAnimalSpinTime = prize[0].mfAnimalSpinTime;
		float fLightPointerSpinTime = prize[0].mfLightPointerSpinTime;
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
		m_LuckyPrizeCtrl.SpinLuckyPrize(prize[0].mnLuckyNum, LL_LuckySpin.LuckyType.LUCKY_SONGDENG, 15f, 27f);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN);
		int tmp2 = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
		AnimalColor currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
		float waitTime2 = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime2 = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime2);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime2 - lessTime2);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(27f - waitTime2);
		LL_EffectMngr.GetSingleton().SetLuckyPrizeShow(prize[0].mnLuckyNum, 2);
		LL_MusicMngr.GetSingleton().PlayLuckyPrizeSound(prize[0].mnLuckyNum, 0);
		int[] nEveryAnimalIndex = new int[4];
		for (int j = 0; j < nEveryAnimalIndex.Length; j++)
		{
			nEveryAnimalIndex[j] = UnityEngine.Random.Range(0, 5);
		}
		nEveryAnimalIndex[(int)animal] = 0;
		nEveryAnimalIndex[(int)animal]++;
		for (int i = 1; i < prize.Length; i++)
		{
			animal = prize[i].mAnimal;
			nLightIndex = prize[i].mnLightIndex;
			fAnimalSpinTime = prize[i].mfAnimalSpinTime;
			fLightPointerSpinTime = prize[i].mfLightPointerSpinTime;
			deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
			nAnimalIndex = (nLightIndex + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			tmp2 = (m_IniAnimalNo + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
			currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
			mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
			m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
			mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
			m_AnimalSpinCtrl.SpinTo(nAnimalIndex, fAnimalSpinTime);
			nEveryAnimalIndex[(int)animal]++;
			waitTime2 = ((!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			lessTime2 = ((!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			yield return new WaitForSeconds(lessTime2);
			LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
			yield return new WaitForSeconds(waitTime2 - lessTime2);
			LL_MusicMngr.GetSingleton().PlayLuckyAnimalResult((int)currentColor2, _musicConvertAnimalTyp(animal));
			m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
			yield return new WaitForSeconds(1f);
			int nUITyp = (int)(_musicConvertAnimalTyp(animal) * 3 + currentColor2);
			LL_UITimesPrize.GetSingleton().AddAnimal(nUITyp);
		}
		yield return new WaitForSeconds(4f);
		LL_UITimesPrize.GetSingleton().Reset();
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoLuckyBonus(int[] animal, int nBonus, int nLuckyTaihao, int[] nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		for (int i = 0; i < nLightIndex.Length; i++)
		{
			if (!_checkLightIndex(nLightIndex[i]))
			{
				return;
			}
		}
		LL_PrizeParameter[] array = new LL_PrizeParameter[animal.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new LL_PrizeParameter();
			array[j].mAnimal = AnimalType.Lion;
			array[j].mAnimal = (AnimalType)animal[j];
			array[j].mnLightIndex = nLightIndex[j];
			array[j].mnLuckyNum = nLuckyTaihao;
			array[j].mnBonus = nBonus;
			if (j == 0)
			{
				array[0].mfAnimalSpinTime = 25f;
				array[0].mfLightPointerSpinTime = 20f;
			}
			else
			{
				array[j].mfAnimalSpinTime = fAnimalSpinTime;
				array[j].mfLightPointerSpinTime = fLightPointerSpinTime;
			}
		}
		StartCoroutine("_LuckyBonusProcess", array);
	}

	private IEnumerator _LuckyBonusProcess(LL_PrizeParameter[] prize)
	{
		AnimalType animal = prize[0].mAnimal;
		int nLightIndex = prize[0].mnLightIndex;
		float fAnimalSpinTime = prize[0].mfAnimalSpinTime;
		float fLightPointerSpinTime = prize[0].mfLightPointerSpinTime;
		int bonusNum = prize[0].mnBonus;
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex2 = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex2, fAnimalSpinTime);
		m_LuckyPrizeCtrl.SpinLuckyPrize(prize[0].mnLuckyNum, LL_LuckySpin.LuckyType.LUCKY_BONUS, 15f, 27f);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN);
		int tmp2 = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
		AnimalColor currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
		float waitTime2 = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime2 = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime2);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime2 - lessTime2);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(27f - waitTime2);
		LL_EffectMngr.GetSingleton().SetLuckyPrizeShow(prize[0].mnLuckyNum, 0);
		LL_MusicMngr.GetSingleton().PlayLuckyPrizeSound(prize[0].mnLuckyNum, 2);
		yield return new WaitForSeconds(3f);
		LL_EffectMngr.GetSingleton().SetLuckyBonusShow(isShow: true, bonusNum);
		int[] nEveryAnimalIndex = new int[4];
		for (int j = 0; j < nEveryAnimalIndex.Length; j++)
		{
			nEveryAnimalIndex[j] = UnityEngine.Random.Range(0, 5);
		}
		nEveryAnimalIndex[(int)animal] = 0;
		nEveryAnimalIndex[(int)animal]++;
		for (int i = 1; i < prize.Length; i++)
		{
			animal = prize[i].mAnimal;
			nLightIndex = prize[i].mnLightIndex;
			fAnimalSpinTime = prize[i].mfAnimalSpinTime;
			fLightPointerSpinTime = prize[i].mfLightPointerSpinTime;
			deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
			nAnimalIndex2 = (nLightIndex + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			tmp2 = (m_IniAnimalNo + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
			currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
			mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
			m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
			mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
			m_AnimalSpinCtrl.SpinTo(nAnimalIndex2, fAnimalSpinTime);
			nEveryAnimalIndex[(int)animal]++;
			waitTime2 = ((!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			lessTime2 = ((!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			yield return new WaitForSeconds(lessTime2);
			LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
			yield return new WaitForSeconds(waitTime2 - lessTime2);
			LL_MusicMngr.GetSingleton().PlayLuckyAnimalResult((int)currentColor2, _musicConvertAnimalTyp(animal));
			m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
		}
		yield return new WaitForSeconds(4f);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoLuckyLightning(int[] animal, int lightningPower, int nLuckyTaihao, int[] nLightIndex, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		for (int i = 0; i < nLightIndex.Length; i++)
		{
			if (!_checkLightIndex(nLightIndex[i]))
			{
				return;
			}
		}
		LL_PrizeParameter[] array = new LL_PrizeParameter[animal.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = new LL_PrizeParameter();
			array[j].mAnimal = AnimalType.Lion;
			array[j].mAnimal = (AnimalType)animal[j];
			array[j].mnLightIndex = nLightIndex[j];
			array[j].mnLuckyNum = nLuckyTaihao;
			array[j].mMoreInfoValue = lightningPower;
			if (j == 0)
			{
				array[0].mfAnimalSpinTime = 25f;
				array[0].mfLightPointerSpinTime = 20f;
			}
			else
			{
				array[j].mfAnimalSpinTime = fAnimalSpinTime;
				array[j].mfLightPointerSpinTime = fLightPointerSpinTime;
			}
		}
		StartCoroutine("_LuckyLuckyLightningProcess", array);
	}

	private IEnumerator _LuckyLuckyLightningProcess(LL_PrizeParameter[] prize)
	{
		AnimalType animal = prize[0].mAnimal;
		int nLightIndex = prize[0].mnLightIndex;
		float fAnimalSpinTime = prize[0].mfAnimalSpinTime;
		float fLightPointerSpinTime = prize[0].mfLightPointerSpinTime;
		int nLightningPower = prize[0].mMoreInfoValue;
		LL_AppUIMngr.GetSingleton().mHudManager.StartRollDice();
		int deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
		int nAnimalIndex2 = (nLightIndex + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
		mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
		m_AnimalSpinCtrl.SpinTo(nAnimalIndex2, fAnimalSpinTime);
		m_LuckyPrizeCtrl.SpinLuckyPrize(prize[0].mnLuckyNum, LL_LuckySpin.LuckyType.LUCKY_LIGHTING, 15f, 27f);
		LL_MusicMngr.GetSingleton().PlayGameMusic(LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN);
		int tmp2 = (m_IniAnimalNo + deltaNo) % LL_Parameter.G_nAnimalNumber;
		m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
		AnimalColor currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
		mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
		float waitTime2 = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime2 = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime2);
		LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
		yield return new WaitForSeconds(waitTime2 - lessTime2);
		m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
		LL_EffectMngr.GetSingleton().SetWallColorState(LL_BackWallAnim.WALL_COLOR_TYPE.WALL_ALL_SHINE);
		yield return new WaitForSeconds(27f - waitTime2);
		LL_EffectMngr.GetSingleton().SetLuckyPrizeShow(prize[0].mnLuckyNum, 1);
		LL_MusicMngr.GetSingleton().PlayLuckyPrizeSound(prize[0].mnLuckyNum, 1);
		yield return new WaitForSeconds(3f);
		LL_EffectMngr.GetSingleton().SetLuckyLightingShow(isShow: true, nLightningPower);
		LL_EffectMngr.GetSingleton().ShowWallAnim(isShow: true);
		LL_EffectMngr.GetSingleton().ShowFireWorksRandom(isShow: true);
		int[] nEveryAnimalIndex = new int[4];
		for (int j = 0; j < nEveryAnimalIndex.Length; j++)
		{
			nEveryAnimalIndex[j] = UnityEngine.Random.Range(0, 5);
		}
		nEveryAnimalIndex[(int)animal] = 0;
		nEveryAnimalIndex[(int)animal]++;
		for (int i = 1; i < prize.Length; i++)
		{
			animal = prize[i].mAnimal;
			nLightIndex = prize[i].mnLightIndex;
			fAnimalSpinTime = prize[i].mfAnimalSpinTime;
			fLightPointerSpinTime = prize[i].mfLightPointerSpinTime;
			deltaNo = (animal - m_ZeroAnimalTyp + 4) % 4;
			nAnimalIndex2 = (nLightIndex + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			tmp2 = (m_IniAnimalNo + deltaNo + nEveryAnimalIndex[(int)animal] * 4) % LL_Parameter.G_nAnimalNumber;
			m_CurPrizeAnimal = mAnimalMngr.GetAnimal(tmp2);
			currentColor2 = m_LightPointer.GetLightColor2(nLightIndex);
			mAnimalMngr.SetColorOfAnimalRingByIndex(tmp2, currentColor2);
			m_LightPointer.GoPointer_ToLightNo(nLightIndex, fLightPointerSpinTime);
			mAnimalMngr.SetAllAnimalState(Animal_Action_State.Animal_Spin);
			m_AnimalSpinCtrl.SpinTo(nAnimalIndex2, fAnimalSpinTime);
			nEveryAnimalIndex[(int)animal]++;
			waitTime2 = ((!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			lessTime2 = ((!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime);
			yield return new WaitForSeconds(lessTime2);
			LL_AppUIMngr.GetSingleton().mHudManager.StopBonusSpin();
			yield return new WaitForSeconds(waitTime2 - lessTime2);
			LL_MusicMngr.GetSingleton().PlayLuckyAnimalResult((int)currentColor2, _musicConvertAnimalTyp(animal));
			m_CurPrizeAnimal.GetComponent<LL_AnimalScript>().SetAnimalActionState(Animal_Action_State.Animal_Bounce);
		}
		yield return new WaitForSeconds(4f);
		LL_AppUIMngr.GetSingleton().mPrizeResult.ShowPrizeResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	private int _musicConvertAnimalTyp(AnimalType typ)
	{
		switch (typ)
		{
		case AnimalType.Rabbit:
			return 3;
		case AnimalType.Monkey:
			return 2;
		case AnimalType.Panda:
			return 1;
		case AnimalType.Lion:
			return 0;
		default:
			return 0;
		}
	}

	private bool _checkLightIndex(int nLightIndex)
	{
		if (nLightIndex <= 23 && nLightIndex >= 0)
		{
			return true;
		}
		UnityEngine.Debug.Log("---Client3D Error---GameMngr _checkLightIndex Error!");
		LL_ErrorManager.GetSingleton().AddError("---Client3D Error---GameMngr _checkLightIndex Error!");
		return false;
	}
}
