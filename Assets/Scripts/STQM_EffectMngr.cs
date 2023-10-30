using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STQM_EffectMngr : MonoBehaviour
{
	public static STQM_EffectMngr G_EffectMngr;

	private STQM_PlayerEffectCtrl[] mPlayerCtrl = new STQM_PlayerEffectCtrl[4];

	[SerializeField]
	private GameObject objEffSimilarBomb;

	private List<GameObject> listEffSimilarBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffSimilarBomb_New;

	private List<GameObject> listEffSimilarBomb_New = new List<GameObject>();

	[SerializeField]
	private GameObject objEffSimilarLightning;

	public List<GameObject> listEffSimilarLightning = new List<GameObject>();

	[SerializeField]
	private STQM_BigFishEffect[] listEffSuperBomb;

	[SerializeField]
	private GameObject objEffGroup;

	private List<GameObject> listEffGroup = new List<GameObject>();

	[HideInInspector]
	public Vector3 tongLDiePos;

	public ShakeCamera shakeCamera;

	private float _linkElapsedTime;

	private bool isPlayer;

	public static STQM_EffectMngr GetSingleton()
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

	public void PlayCoinFly(int nPlayerID, STQM_FISH_TYPE fishType, Vector3 pos)
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
			mPlayerCtrl[nPlayerID - 1].SetLiziReset();
		}
	}

	public void ShowFishScore(int nPlayerID, Vector3 pos, int nScore)
	{
		if (_checkPlayerID(nPlayerID))
		{
			mPlayerCtrl[nPlayerID - 1].ShowFishScore(pos, nScore);
		}
	}

	public void ShowEffSimilarBomb(Vector3 pos)
	{
		ShowEffSimilarBomb_New(pos);
		if (shakeCamera != null && !shakeCamera.enabled)
		{
			shakeCamera.enabled = true;
		}
		tongLDiePos = pos;
		if (listEffSimilarBomb.Count >= 2)
		{
			GameObject gameObject = listEffSimilarBomb[0];
			listEffSimilarBomb.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffSimilarBomb.transform);
		transform.position = pos;
		Canvas component = transform.GetComponent<Canvas>();
		if ((bool)component)
		{
			component.sortingOrder = 944;
		}
		listEffSimilarBomb.Add(transform.gameObject);
	}

	public void ShowEffSimilarBomb_New(Vector3 pos)
	{
		if (listEffSimilarBomb_New.Count >= 2)
		{
			GameObject gameObject = listEffSimilarBomb_New[0];
			listEffSimilarBomb_New.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffSimilarBomb_New.transform);
		transform.position = pos;
		Canvas component = transform.GetComponent<Canvas>();
		if ((bool)component)
		{
			component.sortingOrder = 944;
		}
		listEffSimilarBomb_New.Add(transform.gameObject);
	}

	public void OverEffSimilarLightning()
	{
		isPlayer = false;
		for (int i = 0; i < listEffSimilarLightning.Count; i++)
		{
			DestroyEffectObj(listEffSimilarLightning[i]);
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

	public void ShowEffSuperBomb(Vector3 pos)
	{
		STQM_BigFishEffect sTQM_BigFishEffect = null;
		for (int i = 0; i < listEffSuperBomb.Length; i++)
		{
			if (!listEffSuperBomb[i].bPlaying)
			{
				sTQM_BigFishEffect = listEffSuperBomb[i];
			}
		}
		if (sTQM_BigFishEffect != null)
		{
			sTQM_BigFishEffect.PlayEffect(pos);
		}
		STQM_MusicMngr.GetSingleton().PlayGameSound(STQM_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffGroup(Vector3 pos)
	{
		tongLDiePos = pos;
		ShowEffSimilarBomb_New(pos);
		if (listEffGroup.Count >= 2)
		{
			GameObject gameObject = listEffGroup[0];
			listEffGroup.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffGroup.transform);
		transform.position = pos;
		try
		{
			transform.GetComponent<Canvas>().sortingOrder = 944;
		}
		catch (Exception)
		{
		}
		listEffGroup.Add(transform.gameObject);
	}

	public bool IsBigFish(STQM_FISH_TYPE fishtype)
	{
		bool result = false;
		if (fishtype >= STQM_FISH_TYPE.Fish_GoldenShark && fishtype <= STQM_FISH_TYPE.Fish_BowlFish)
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

	public Transform CreateEffectObj(Transform trans)
	{
		return STQM_PoolManager.Pools["QMEffectPool"].Spawn(trans);
	}

	public void DestroyEffectObj(GameObject obj)
	{
		STQM_PoolManager.Pools["QMEffectPool"].Despawn(obj.transform);
	}

	public void Init()
	{
		for (int i = 0; i < 4; i++)
		{
			mPlayerCtrl[i] = base.transform.GetChild(i).GetComponent<STQM_PlayerEffectCtrl>();
		}
	}

	public int GetFlyCoinNum(STQM_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STQM_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STQM_FISH_TYPE.Fish_Grass:
			return 2;
		case STQM_FISH_TYPE.Fish_Zebra:
			return 3;
		case STQM_FISH_TYPE.Fish_BigEars:
			return 4;
		case STQM_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STQM_FISH_TYPE.Fish_Ugly:
			return 6;
		case STQM_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STQM_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STQM_FISH_TYPE.Fish_Lamp:
			return 9;
		case STQM_FISH_TYPE.Fish_Turtle:
			return 2;
		case STQM_FISH_TYPE.Fish_Trailer:
			return 3;
		case STQM_FISH_TYPE.Fish_Butterfly:
			return 3;
		case STQM_FISH_TYPE.Fish_Beauty:
			return 4;
		case STQM_FISH_TYPE.Fish_Arrow:
			return 4;
		case STQM_FISH_TYPE.Fish_Bat:
			return 5;
		case STQM_FISH_TYPE.Fish_SilverShark:
			return 6;
		case STQM_FISH_TYPE.Fish_GoldenShark:
			return 7;
		case STQM_FISH_TYPE.Fish_SuperPenguin:
			return 10;
		case STQM_FISH_TYPE.Fish_BuleWhale:
			return 8;
		case STQM_FISH_TYPE.Fish_BowlFish:
			return 10;
		case STQM_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STQM_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STQM_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STQM_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STQM_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STQM_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STQM_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STQM_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STQM_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STQM_FISH_TYPE.Fish_Same_Turtle:
			return 2;
		case STQM_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STQM_FISH_TYPE.Fish_AllBomb:
			return 10;
		case STQM_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public int GetFishOODS(STQM_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STQM_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STQM_FISH_TYPE.Fish_Grass:
			return 2;
		case STQM_FISH_TYPE.Fish_Zebra:
			return 3;
		case STQM_FISH_TYPE.Fish_BigEars:
			return 4;
		case STQM_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STQM_FISH_TYPE.Fish_Ugly:
			return 6;
		case STQM_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STQM_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STQM_FISH_TYPE.Fish_Lamp:
			return 9;
		case STQM_FISH_TYPE.Fish_Turtle:
			return 10;
		case STQM_FISH_TYPE.Fish_Trailer:
			return 12;
		case STQM_FISH_TYPE.Fish_Butterfly:
			return 15;
		case STQM_FISH_TYPE.Fish_Beauty:
			return 18;
		case STQM_FISH_TYPE.Fish_Arrow:
			return 20;
		case STQM_FISH_TYPE.Fish_Bat:
			return 25;
		case STQM_FISH_TYPE.Fish_SilverShark:
			return 30;
		case STQM_FISH_TYPE.Fish_GoldenShark:
			return 40;
		case STQM_FISH_TYPE.Fish_SuperPenguin:
			return 200;
		case STQM_FISH_TYPE.Fish_BuleWhale:
			return 40;
		case STQM_FISH_TYPE.Fish_BowlFish:
			return 150;
		case STQM_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STQM_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STQM_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STQM_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STQM_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STQM_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STQM_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STQM_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STQM_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STQM_FISH_TYPE.Fish_Same_Turtle:
			return 10;
		case STQM_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STQM_FISH_TYPE.Fish_AllBomb:
			return 50;
		case STQM_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public void Clear()
	{
		STQM_PoolManager.Pools["QMEffectPool"].DespawnAll();
		mPlayerCtrl = null;
		listEffSimilarBomb.Clear();
		listEffGroup.Clear();
		G_EffectMngr = null;
	}
}
