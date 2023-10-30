using BCBM_GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class BCBM_EffectMngr : MonoBehaviour
{
	public static BCBM_EffectMngr G_EffectMngr;

	private GameObject _backFireWorks;

	private GameObject _AllCaiJinSatellite;

	private GameObject _AllSongDengTV;

	private GameObject _AllShanDian;

	private GameObject _AllShanDianPtc;

	private GameObject _AllDaSiXi;

	private GameObject _AllDaSanYuan;

	private GameObject _LuckyBonus;

	private GameObject _LuckyShanDian;

	private GameObject _LuckyPrizeEffect;

	private BCBM_BackWallAnim _BackWallAnim;

	private GameObject backwall;

	private BCBM_Script_AnimalPtc2 _AnimalPtcCtrl2;

	private BCBM_Script_AnimalPtc3 _AnimalPtcCtrl3;

	private GameObject _AnimalPtcObj1;

	private GameObject _AnimalPtcObj2;

	private GameObject _AnimalPtcObj3;

	private GameObject _LuckyBonusCoin;

	private GameObject _luckyLightningSwirl;

	public static BCBM_EffectMngr GetSingleton()
	{
		return G_EffectMngr;
	}

	private void Awake()
	{
		_backFireWorks = GameObject.Find("MultiFireWorks");
		_AllCaiJinSatellite = GameObject.Find("All_CaiJin");
		_AllSongDengTV = GameObject.Find("All_songdeng");
		_AllShanDian = GameObject.Find("All_shandian");
		_AllShanDianPtc = GameObject.Find("Lightningx2x3");
		_AllDaSiXi = GameObject.Find("All_dasixi");
		_AllDaSanYuan = GameObject.Find("All_dasanyuan");
		_BackWallAnim = GameObject.Find("Wall").GetComponent<BCBM_BackWallAnim>();
		backwall = GameObject.Find("Wall");
		_LuckyBonus = GameObject.Find("LuckyBonus");
		_LuckyShanDian = GameObject.Find("LuckyLightning");
		_AnimalPtcObj1 = GameObject.Find("AnimalPrizePtc1");
		_AnimalPtcObj2 = GameObject.Find("AnimalPrizePtc2");
		_AnimalPtcCtrl2 = _AnimalPtcObj2.GetComponent<BCBM_Script_AnimalPtc2>();
		_AnimalPtcObj3 = GameObject.Find("AnimalPrizePtc3_6Agl");
		_AnimalPtcCtrl3 = _AnimalPtcObj3.GetComponent<BCBM_Script_AnimalPtc3>();
		_LuckyPrizeEffect = GameObject.Find("LuckyNumber");
		_LuckyBonusCoin = GameObject.Find("LuckyBonusCoin");
		_luckyLightningSwirl = GameObject.Find("Lucky_LightningPtc");
		ShowAnimalParticle(isShow: false);
	}

	private void Start()
	{
		if (G_EffectMngr == null)
		{
			G_EffectMngr = this;
		}
		ShowAllCaiJinSatellite(isShow: false);
		SetSongDengTVShow(isShow: false, -1);
		SetAllShanDianShow(isShow: false, isDouble: false);
		SetAllDaSiXiShow(isShow: false);
		SetAllDaSanYuanShow(isShow: false);
		SetAllShanDianParticleShow(isShow: false);
		SetLuckyPrizeHide();
		SetLuckyBonusShow(isShow: false);
		SetLuckyLightingShow(isShow: false);
	}

	private void Update()
	{
	}

	public void ShowAnimalParticle(bool isShow)
	{
		if (!isShow)
		{
			_AnimalPtcObj1.SetActive(isShow);
			_AnimalPtcObj2.SetActive(isShow);
			_AnimalPtcObj3.SetActive(isShow);
			return;
		}
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 1:
			_AnimalPtcObj1.SetActive(value: true);
			break;
		case 2:
			_AnimalPtcObj2.SetActive(value: true);
			_AnimalPtcCtrl2.PlayPtc(6f);
			break;
		default:
			_AnimalPtcObj3.SetActive(value: true);
			_AnimalPtcCtrl3.PlayPtc(3f);
			break;
		}
	}

	public void ShowFireWorksRandom(bool isShow)
	{
		_backFireWorks.SetActive(value: true);
		if (isShow)
		{
			BCBM_MusicMngr.GetSingleton().PlayEffectSound(BCBM_MusicMngr.MUSIC_EFFECT_MUSIC.EFFECT_FIREWORKS);
			_backFireWorks.SetActive(value: true);
			IEnumerator enumerator = _backFireWorks.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					Transform transform = (Transform)current;
					transform.gameObject.SendMessage("SetEffectActive", isShow);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		else
		{
			IEnumerator enumerator2 = _backFireWorks.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object current2 = enumerator2.Current;
					Transform transform2 = (Transform)current2;
					transform2.gameObject.SendMessage("SetEffectActive", isShow);
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			_backFireWorks.SetActive(value: false);
		}
	}

	public void ShowAllCaiJinSatellite(bool isShow)
	{
		_AllCaiJinSatellite.SetActive(isShow);
	}

	public void ShowWallAnim(bool isShow)
	{
		_BackWallAnim.ShowWallAnim(isShow);
	}

	public void SetWallColorState(BCBM_BackWallAnim.WALL_COLOR_TYPE typ)
	{
		_BackWallAnim.ShowColorAnim(typ);
	}

	public void SetSongDengTVShow(bool isShow, int number, bool withAnim = false)
	{
		_AllSongDengTV.SetActive(isShow);
		if (isShow)
		{
			_AllSongDengTV.GetComponent<BCBM_All_SongDeng>().ShowSongDeng(isShow, number, withAnim);
		}
	}

	public void SetAllShanDianShow(bool isShow, bool isDouble)
	{
		_AllShanDian.SetActive(value: false);
		_AllShanDian.SetActive(isShow);
		if (isShow && isDouble)
		{
			_AllShanDian.transform.Find("shandian").Find("X2").GetComponent<Renderer>()
				.enabled = true;
				_AllShanDian.transform.Find("shandian").Find("X3").GetComponent<Renderer>()
					.enabled = false;
				}
				else if (isShow && !isDouble)
				{
					_AllShanDian.transform.Find("shandian").Find("X2").GetComponent<Renderer>()
						.enabled = false;
						_AllShanDian.transform.Find("shandian").Find("X3").GetComponent<Renderer>()
							.enabled = true;
						}
					}

					public void SetAllShanDianParticleShow(bool isShow)
					{
						StopCoroutine("_shandianParticle");
						if (isShow)
						{
							StartCoroutine("_shandianParticle");
						}
						else
						{
							_AllShanDianPtc.SetActive(value: false);
						}
					}

					private IEnumerator _shandianParticle()
					{
						_AllShanDianPtc.SetActive(value: true);
						yield return new WaitForSeconds(1f);
						_AllShanDianPtc.SetActive(value: false);
					}

					public void SetAllDaSiXiShow(bool isShow)
					{
						_AllDaSiXi.SetActive(isShow);
						if (isShow)
						{
							_AllDaSiXi.SendMessage("Show");
						}
					}

					public void SetAllDaSiXiShowOneColor(AnimalColor color)
					{
						_AllDaSiXi.GetComponent<BCBM_All_DaSiXi>().SetColor(color);
					}

					public void SetAllDaSanYuanShow(bool isShow)
					{
						_AllDaSanYuan.SetActive(isShow);
						if (isShow)
						{
							_AllDaSanYuan.SendMessage("Show");
						}
					}

					public void SetAllDaSanYuanShowOneAnimal(int nAnimalIndex)
					{
						_AllDaSanYuan.SetActive(value: true);
						_AllDaSanYuan.GetComponent<BCBM_All_DaSanYuan>().ShowOneAnimal(nAnimalIndex);
					}

					public void SetLuckyPrizeShow(int nLuckyNumber, int nLuckyTyp)
					{
						_LuckyPrizeEffect.SetActive(value: true);
						_LuckyPrizeEffect.GetComponent<BCBM_LuckyNumber>().ShowAnimation(nLuckyNumber, nLuckyTyp);
					}

					public void SetLuckyPrizeHide()
					{
						_LuckyPrizeEffect.GetComponent<BCBM_LuckyNumber>().Reset();
						_LuckyPrizeEffect.SetActive(value: false);
					}

					public void SetLuckyBonusShow(bool isShow, int bonusNum = 1000)
					{
						if (isShow)
						{
							_LuckyBonusCoin.SetActive(value: true);
							_LuckyBonus.SetActive(value: true);
							_LuckyBonus.GetComponent<BCBM_LuckyBonus>().SetBonusNum(bonusNum);
							BCBM_MusicMngr.GetSingleton().PlayEffectSound(BCBM_MusicMngr.MUSIC_EFFECT_MUSIC.Effect_BONUS_COINS);
						}
						else
						{
							_LuckyBonus.SetActive(value: false);
							_LuckyBonusCoin.SetActive(value: false);
						}
					}

					public void SetLuckyLightingShow(bool isShow, int lightningPower = 0)
					{
						if (isShow)
						{
							if (lightningPower > 5 || lightningPower < 2)
							{
								return;
							}
							for (int i = 2; i <= 5; i++)
							{
								Transform transform = _LuckyShanDian.transform.Find("child").Find("sz_" + i.ToString());
								if (i == lightningPower)
								{
									transform.GetComponent<Renderer>().enabled = true;
								}
								else
								{
									transform.GetComponent<Renderer>().enabled = false;
								}
							}
							_LuckyShanDian.SetActive(value: true);
							_luckyLightningSwirl.SetActive(value: true);
						}
						else
						{
							_LuckyShanDian.SetActive(value: false);
							_luckyLightningSwirl.SetActive(value: false);
						}
					}

					public void Reset()
					{
						ShowFireWorksRandom(isShow: false);
						ShowAllCaiJinSatellite(isShow: false);
						SetAllShanDianShow(isShow: false, isDouble: true);
						SetAllShanDianParticleShow(isShow: false);
						SetAllDaSiXiShow(isShow: false);
						SetAllDaSanYuanShow(isShow: false);
						SetLuckyPrizeHide();
						SetLuckyBonusShow(isShow: false);
						SetLuckyLightingShow(isShow: false);
						_BackWallAnim.Reset();
						SetSongDengTVShow(isShow: false, 0);
						ShowAnimalParticle(isShow: false);
					}
				}
