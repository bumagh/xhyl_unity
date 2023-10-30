using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STMF_EffectMngr : MonoBehaviour
{
	public static STMF_EffectMngr G_EffectMngr;

	public STMF_PlayerEffectCtrl[] mPlayerCtrl = new STMF_PlayerEffectCtrl[4];

	[SerializeField]
	private GameObject _knifeEffect;

	[SerializeField]
	private GameObject objEffSimilarBomb;

	private List<GameObject> listEffSimilarBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffSimilarLightning;

	public List<GameObject> listEffSimilarLightning = new List<GameObject>();

	[SerializeField]
	private STMF_BigFishEffect[] listEffSuperBomb;

	private static int score = 100;

	private static int sPlayer = 1;

	[HideInInspector]
	public Vector3 tongLDiePos;

	private float _linkElapsedTime;

	private bool isPlayer;

	public static STMF_EffectMngr GetSingleton()
	{
		return G_EffectMngr;
	}

	private void Awake()
	{
		if (G_EffectMngr == null)
		{
			G_EffectMngr = this;
		}
	}

	private void Start()
	{
		Init();
	}

	private bool _checkPlayerID(int nPlayerID)
	{
		return nPlayerID <= 4 && nPlayerID >= 1;
	}

	public void PlayCoinFly(int nPlayerID, STMF_FISH_TYPE fishType, Vector3 pos)
	{
		if (_checkPlayerID(nPlayerID))
		{
			mPlayerCtrl[nPlayerID - 1].ShowCoin(fishType, pos);
		}
	}

	public void PlayLiziCardFly(int nPlayerID, Vector3 pos)
	{
		if (_checkPlayerID(nPlayerID))
		{
			mPlayerCtrl[nPlayerID - 1].ShowLiziCard(pos);
		}
	}

	public void ResetLiziTime(int nPlayerID)
	{
		if (_checkPlayerID(nPlayerID))
		{
			if (nPlayerID < 1 || nPlayerID > mPlayerCtrl.Length)
			{
				nPlayerID = 1;
			}
			try
			{
				mPlayerCtrl[nPlayerID - 1].SetLiziReset();
			}
			catch (Exception)
			{
			}
		}
	}

	public void ShowFishScore(int nPlayerID, Vector3 pos, int nScore)
	{
		if (_checkPlayerID(nPlayerID))
		{
			mPlayerCtrl[nPlayerID - 1].ShowFishScore(pos, nScore);
		}
	}

	public void PlayKnifeEffect(Vector3 pos)
	{
		CreateObj(_knifeEffect.transform).GetComponent<STMF_KnifeEffect>().PlayEffect(pos);
	}

	public void PlayBigFishEffect(Vector3 pos)
	{
		tongLDiePos = pos;
		STMF_BigFishEffect sTMF_BigFishEffect = null;
		for (int i = 0; i < listEffSuperBomb.Length; i++)
		{
			if (!listEffSuperBomb[i].bPlaying)
			{
				sTMF_BigFishEffect = listEffSuperBomb[i];
			}
		}
		if (sTMF_BigFishEffect != null)
		{
			sTMF_BigFishEffect.PlayEffect(pos);
		}
		STMF_SceneBgMngr.GetSingleton().ShakeBg();
		STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffSimilarBomb(Vector3 pos)
	{
		PlayBigFishEffect(pos);
		tongLDiePos = pos;
		if (listEffSimilarBomb.Count >= 2)
		{
			GameObject gameObject = listEffSimilarBomb[0];
			listEffSimilarBomb.Remove(gameObject);
			DestroyUIEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffSimilarBomb.transform);
		transform.position = pos;
		transform.GetComponent<Canvas>().sortingOrder = 944;
		listEffSimilarBomb.Add(transform.gameObject);
	}

	public void OverEffSimilarLightning()
	{
		isPlayer = false;
		for (int i = 0; i < listEffSimilarLightning.Count; i++)
		{
			DestroyUIEffectObj(listEffSimilarLightning[i]);
		}
		listEffSimilarLightning = new List<GameObject>();
	}

	public void ShowEffSimilarLightning(Vector3 fishPos)
	{
		UnityEngine.Debug.LogError("===同类闪电===");
	}

	private IEnumerator doUpdate(Transform _chainEffect, Vector3 moveTargetPos, Vector3 fishPos)
	{
		while (isPlayer)
		{
			Vector3 vec = moveTargetPos - fishPos;
			Vector3 normalized = vec.normalized;
			float distance = vec.magnitude;
			_chainEffect.position = moveTargetPos;
			Vector3 vector = fishPos - moveTargetPos;
			float num = Mathf.Atan2(vector.y, vector.x);
			_chainEffect.rotation = Quaternion.AngleAxis(57.29578f * num - 90f, Vector3.forward);
			float d2 = Mathf.Lerp(0f, distance, _linkElapsedTime / 0.4f);
			_linkElapsedTime += Time.deltaTime;
			float scale = d2 * 0.5f;
			_chainEffect.localScale = new Vector3((!(scale >= 1.8f)) ? scale : 1.8f, scale, scale);
			yield return null;
		}
	}

	public Transform CreateEffectObj(Transform trans)
	{
		return PoolManager.Pools["UIEffectPool"].Spawn(trans);
	}

	public bool IsBigFish(STMF_FISH_TYPE fishtype)
	{
		bool result = false;
		if (fishtype >= STMF_FISH_TYPE.Fish_GoldenShark && fishtype <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group && fishtype != STMF_FISH_TYPE.Fish_CoralReefs)
		{
			result = true;
		}
		return result;
	}

	public void ShowBigPrizePlate(int nPlayerID, int nScore)
	{
		if (nScore > 0)
		{
			mPlayerCtrl[nPlayerID - 1].ShowPrizePlate(nScore);
		}
	}

	public Transform CreateObj(Transform trans)
	{
		return PoolManager.Pools["EffectPool"].Spawn(trans);
	}

	public Transform CreateUIObj(Transform tf)
	{
		return PoolManager.Pools["UIEffectPool"].Spawn(tf);
	}

	public void DestroyEffectObj(GameObject obj)
	{
		PoolManager.Pools["EffectPool"].Despawn(obj.transform);
	}

	public void DestroyUIEffectObj(GameObject obj)
	{
		PoolManager.Pools["UIEffectPool"].Despawn(obj.transform);
	}

	private void Init()
	{
		for (int i = 0; i < 4; i++)
		{
			try
			{
				mPlayerCtrl[i] = base.transform.Find("Player" + (i + 1)).GetComponent<STMF_PlayerEffectCtrl>();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}
	}

	public int GetFlyCoinNum(STMF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STMF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STMF_FISH_TYPE.Fish_Grass:
			return 2;
		case STMF_FISH_TYPE.Fish_Zebra:
			return 3;
		case STMF_FISH_TYPE.Fish_BigEars:
			return 4;
		case STMF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STMF_FISH_TYPE.Fish_Ugly:
			return 6;
		case STMF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STMF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STMF_FISH_TYPE.Fish_Lamp:
			return 9;
		case STMF_FISH_TYPE.Fish_Turtle:
			return 2;
		case STMF_FISH_TYPE.Fish_Trailer:
			return 3;
		case STMF_FISH_TYPE.Fish_Butterfly:
			return 3;
		case STMF_FISH_TYPE.Fish_Beauty:
			return 4;
		case STMF_FISH_TYPE.Fish_Arrow:
			return 4;
		case STMF_FISH_TYPE.Fish_Bat:
			return 5;
		case STMF_FISH_TYPE.Fish_SilverShark:
			return 6;
		case STMF_FISH_TYPE.Fish_GoldenShark:
			return 8;
		case STMF_FISH_TYPE.Fish_GreenDragon:
			return 10;
		case STMF_FISH_TYPE.Fish_SilverDragon:
			return 10;
		case STMF_FISH_TYPE.Fish_GoldenDragon:
			return 10;
		case STMF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STMF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STMF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STMF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STMF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STMF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STMF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STMF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STMF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STMF_FISH_TYPE.Fish_Same_Turtle:
			return 2;
		case STMF_FISH_TYPE.Fish_LimitedBomb:
			return 2;
		case STMF_FISH_TYPE.Fish_AllBomb:
			return 0;
		case STMF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STMF_FISH_TYPE.Fish_DragonBeauty_Group:
			return 10;
		case STMF_FISH_TYPE.Fish_GoldenArrow_Group:
			return 10;
		case STMF_FISH_TYPE.Fish_Knife_Butterfly_Group:
			return 10;
		case STMF_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public int GetFishOODS(STMF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STMF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STMF_FISH_TYPE.Fish_Grass:
			return 2;
		case STMF_FISH_TYPE.Fish_Zebra:
			return 3;
		case STMF_FISH_TYPE.Fish_BigEars:
			return 4;
		case STMF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STMF_FISH_TYPE.Fish_Ugly:
			return 6;
		case STMF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STMF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STMF_FISH_TYPE.Fish_Lamp:
			return 9;
		case STMF_FISH_TYPE.Fish_Turtle:
			return 10;
		case STMF_FISH_TYPE.Fish_Trailer:
			return 12;
		case STMF_FISH_TYPE.Fish_Butterfly:
			return 15;
		case STMF_FISH_TYPE.Fish_Beauty:
			return 18;
		case STMF_FISH_TYPE.Fish_Arrow:
			return 20;
		case STMF_FISH_TYPE.Fish_Bat:
			return 25;
		case STMF_FISH_TYPE.Fish_SilverShark:
			return 30;
		case STMF_FISH_TYPE.Fish_GoldenShark:
			return 40;
		case STMF_FISH_TYPE.Fish_GreenDragon:
			return 100;
		case STMF_FISH_TYPE.Fish_SilverDragon:
			return 120;
		case STMF_FISH_TYPE.Fish_GoldenDragon:
			return 160;
		case STMF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STMF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STMF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STMF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STMF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STMF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STMF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STMF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STMF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STMF_FISH_TYPE.Fish_Same_Turtle:
			return 10;
		case STMF_FISH_TYPE.Fish_LimitedBomb:
			return 10;
		case STMF_FISH_TYPE.Fish_AllBomb:
			return 0;
		case STMF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STMF_FISH_TYPE.Fish_DragonBeauty_Group:
			return 174;
		case STMF_FISH_TYPE.Fish_GoldenArrow_Group:
			return 100;
		case STMF_FISH_TYPE.Fish_Knife_Butterfly_Group:
			return 60;
		case STMF_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public void ResetAllEffect()
	{
		PoolManager.Pools["EffectPool"].DespawnAll();
	}
}
