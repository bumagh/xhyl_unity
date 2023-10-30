using DP_GameCommon;
using System.Collections;
using UnityEngine;

public class DP_GameCtrl : MonoBehaviour
{
	private static DP_GameCtrl gameCtrl;

	[HideInInspector]
	public DP_SpinCtrl pointSpinCtrl;

	[HideInInspector]
	public DP_SpinCtrl animalSpinCtrl;

	[HideInInspector]
	public DP_AnimalColorCtrl animalColorCtrl;

	[HideInInspector]
	public DP_AnimalCtrl animalCtrl;

	private DP_CameraCtrl cameraCtrl;

	private DP_PrizeParameter prizePara = new DP_PrizeParameter();

	public static DP_GameCtrl GetSingleton()
	{
		return gameCtrl;
	}

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (gameCtrl == null)
		{
			gameCtrl = this;
		}
		cameraCtrl = GameObject.Find("MainCamera").GetComponent<DP_CameraCtrl>();
		Transform transform = GameObject.Find("3dModel").transform;
		pointSpinCtrl = transform.Find("lunpan_01").GetComponent<DP_SpinCtrl>();
		animalSpinCtrl = transform.Find("Animal").GetComponent<DP_SpinCtrl>();
		animalCtrl = animalSpinCtrl.transform.GetComponent<DP_AnimalCtrl>();
		animalColorCtrl = transform.Find("deng_00").GetComponent<DP_AnimalColorCtrl>();
	}

	public void Reset()
	{
		StopCoroutine("OneAnimalProcess");
		StopCoroutine("LuckyBonusProcess");
		StopAllCoroutines();
		animalSpinCtrl.Reset();
		pointSpinCtrl.Reset();
		animalCtrl.ResetAllAnimals();
		DP_GameInfo.getInstance().SceneGame.sptResult.HideResult();
		DP_GameInfo.getInstance().SceneGame.sptHud.ShowObjDouble(bShow: false);
		DP_MusicMngr.GetSingleton().ResetWithoutUI();
	}

	public void GoOneAnimal(AnimalType aType, int point, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		prizePara.animalType = aType;
		prizePara.pointIndex = point;
		prizePara.fAnimalSpinTime = fAnimalSpinTime;
		prizePara.fLightPointerSpinTime = fLightPointerSpinTime;
		StartCoroutine("OneAnimalProcess", prizePara);
	}

	private IEnumerator OneAnimalProcess(DP_PrizeParameter prize)
	{
		AnimalType aType = prize.animalType;
		float fAnimalSpinTime = prize.fAnimalSpinTime;
		float fLightPointerSpinTime = prize.fLightPointerSpinTime;
		int pointIndex = prize.pointIndex;
		int animalIndex = animalCtrl.GetAnimalIndex((int)aType);
		int index = (24 + pointIndex - animalIndex) % 24;
		pointSpinCtrl.SpinTo(pointIndex, fLightPointerSpinTime);
		StartCoroutine(animalColorCtrl.PlayAnimalColorAnimIsStop(fLightPointerSpinTime));
		DP_MusicMngr.GetSingleton().PlaySceneSound(DP_MusicMngr.MUSIC_SCENE_MUSIC.SCENE_SPIN_ANIMAL);
		DP_MusicMngr.GetSingleton().PlayGameMusic(DP_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN);
		animalSpinCtrl.SpinTo(index, fAnimalSpinTime, isClockwise: false);
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		DP_GameInfo.getInstance().SceneGame.sptHud.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		if (DP_GameData.times > 0)
		{
			DP_GameInfo.getInstance().SceneGame.sptHud.PlayDoubleAnim(bPlay: false);
		}
		DP_MusicMngr.GetSingleton().PlayGameMusic(DP_MusicMngr.MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_END);
		StartCoroutine(animalCtrl.animalAnimCtrls[animalIndex].AnimGoCenter());
		cameraCtrl.PlayAnim();
		yield return new WaitForSeconds(9f);
		int aColor = animalColorCtrl.GetPointIndexColor(pointIndex);
		DP_MusicMngr.GetSingleton().PlayNormalAnimalResult(aColor, MusicConvertAnimalTyp(aType));
		DP_GameInfo.getInstance().SceneGame.sptResult.ShowResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	public void GoBonus(AnimalType aType, int nBonus, int nLuckyTaihao, int point, float fAnimalSpinTime = 15f, float fLightPointerSpinTime = 10f)
	{
		Reset();
		prizePara.animalType = aType;
		prizePara.pointIndex = point;
		prizePara.luckyNum = nLuckyTaihao;
		prizePara.bonusNum = nBonus;
		prizePara.fAnimalSpinTime = fAnimalSpinTime;
		prizePara.fLightPointerSpinTime = fLightPointerSpinTime;
		StartCoroutine("BonusProcess", prizePara);
	}

	private IEnumerator BonusProcess(DP_PrizeParameter prize)
	{
		AnimalType aType = prize.animalType;
		float fAnimalSpinTime = prize.fAnimalSpinTime;
		float fLightPointerSpinTime = prize.fLightPointerSpinTime;
		int luckyNum = prize.luckyNum;
		int pointIndex = prize.pointIndex;
		int animalIndex = animalCtrl.GetAnimalIndex((int)aType);
		int index = (24 + pointIndex - animalIndex) % 24;
		pointSpinCtrl.SpinTo(pointIndex, fLightPointerSpinTime);
		StartCoroutine(animalColorCtrl.PlayAnimalColorAnimIsStop(fLightPointerSpinTime));
		DP_MusicMngr.GetSingleton().PlaySceneSound(DP_MusicMngr.MUSIC_SCENE_MUSIC.SCENE_SPIN_ANIMAL);
		DP_MusicMngr.GetSingleton().PlayGameMusic(DP_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_BEGIN);
		animalSpinCtrl.SpinTo(index, fAnimalSpinTime, isClockwise: false);
		float waitTime = (!(fAnimalSpinTime <= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		float lessTime = (!(fAnimalSpinTime >= fLightPointerSpinTime)) ? fAnimalSpinTime : fLightPointerSpinTime;
		yield return new WaitForSeconds(lessTime);
		DP_GameInfo.getInstance().SceneGame.sptHud.StopBonusSpin();
		yield return new WaitForSeconds(waitTime - lessTime);
		DP_MusicMngr.GetSingleton().PlayGameMusic(DP_MusicMngr.MUSIC_GAME_MUSIC.GAME_ALL_PRIZE_END);
		if (DP_GameData.times > 0)
		{
			DP_GameInfo.getInstance().SceneGame.sptHud.PlayDoubleAnim(bPlay: false);
		}
		StartCoroutine(animalCtrl.animalAnimCtrls[animalIndex].AnimGoCenter());
		cameraCtrl.PlayAnim();
		yield return new WaitForSeconds(9f);
		int aColor = animalColorCtrl.GetPointIndexColor(pointIndex);
		DP_MusicMngr.GetSingleton().PlayNormalAnimalResult(aColor, MusicConvertAnimalTyp(aType));
		DP_GameInfo.getInstance().SceneGame.sptResult.ShowResult();
		yield return new WaitForSeconds(6f);
		Reset();
	}

	private int MusicConvertAnimalTyp(AnimalType typ)
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
}
