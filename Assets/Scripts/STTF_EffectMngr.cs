using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STTF_EffectMngr : MonoBehaviour
{
	public static STTF_EffectMngr G_EffectMngr;

	private STTF_PlayerEffectCtrl[] mPlayerCtrl = new STTF_PlayerEffectCtrl[4];

	[SerializeField]
	private GameObject objEffSimilarBomb;

	private List<GameObject> listEffSimilarBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffGold;

	[SerializeField]
	private GameObject objEffSimilarLightning;

	private List<GameObject> listEffGold = new List<GameObject>();

	public List<GameObject> listEffSimilarLightning = new List<GameObject>();

	[SerializeField]
	private STTF_BigFishEffect[] listEffSuperBomb;

	[SerializeField]
	private GameObject objEffTimeStop;

	private List<GameObject> listEffTimeStop = new List<GameObject>();

	[SerializeField]
	private GameObject objEffGroup;

	private List<GameObject> listEffGroup = new List<GameObject>();

	[HideInInspector]
	public Vector3 tongLDiePos;

	public ShakeCamera shakeCamera;

	private float _linkElapsedTime;

	private bool isPlayer;

	public static STTF_EffectMngr GetSingleton()
	{
		return G_EffectMngr;
	}

	private void Awake()
	{
		if (G_EffectMngr == null)
		{
			G_EffectMngr = this;
		}
		Init();
	}

	private bool _checkPlayerID(int nPlayerID)
	{
		return nPlayerID <= 4 && nPlayerID >= 1;
	}

	public void PlayCoinFly(int nPlayerID, STTF_FISH_TYPE fishType, Vector3 pos)
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
		ShowGoldEff(pos);
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
		transform.GetComponent<Canvas>().sortingOrder = 944;
		listEffSimilarBomb.Add(transform.gameObject);
	}

	public void ShowGoldEff(Vector3 pos)
	{
		if (listEffGold.Count >= 2)
		{
			GameObject gameObject = listEffGold[0];
			listEffGold.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffGold.transform);
		transform.position = pos;
		listEffGold.Add(transform.gameObject);
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
		STTF_BigFishEffect sTTF_BigFishEffect = null;
		for (int i = 0; i < listEffSuperBomb.Length; i++)
		{
			if (!listEffSuperBomb[i].bPlaying)
			{
				sTTF_BigFishEffect = listEffSuperBomb[i];
			}
		}
		if (sTTF_BigFishEffect != null)
		{
			sTTF_BigFishEffect.PlayEffect(pos);
		}
		STTF_MusicMngr.GetSingleton().PlayGameSound(STTF_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffTimeStop(Vector3 pos)
	{
		if (listEffTimeStop.Count >= 2)
		{
			GameObject gameObject = listEffTimeStop[0];
			listEffTimeStop.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffTimeStop.transform);
		listEffTimeStop.Add(transform.gameObject);
		transform.position = pos;
		transform.GetComponent<Canvas>().sortingOrder = 944;
		STTF_MusicMngr.GetSingleton().PlayGameSound(STTF_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffGroup(Vector3 pos)
	{
		if (listEffGroup.Count >= 2)
		{
			GameObject gameObject = listEffGroup[0];
			listEffGroup.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffGroup.transform);
		transform.position = pos;
		transform.GetComponent<Canvas>().sortingOrder = 944;
		listEffGroup.Add(transform.gameObject);
	}

	public bool IsBigFish(STTF_FISH_TYPE fishtype)
	{
		bool flag = false;
		switch (fishtype)
		{
		case STTF_FISH_TYPE.Fish_GoldenShark:
		case STTF_FISH_TYPE.Fish_BigShark:
		case STTF_FISH_TYPE.Fish_Dragon:
		case STTF_FISH_TYPE.Fish_Toad:
		case STTF_FISH_TYPE.Fish_Same_Shrimp:
		case STTF_FISH_TYPE.Fish_Same_Grass:
		case STTF_FISH_TYPE.Fish_Same_Zebra:
		case STTF_FISH_TYPE.Fish_Same_BigEars:
		case STTF_FISH_TYPE.Fish_Same_YellowSpot:
		case STTF_FISH_TYPE.Fish_Same_Ugly:
		case STTF_FISH_TYPE.Fish_Same_Hedgehog:
		case STTF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STTF_FISH_TYPE.Fish_Same_Lamp:
		case STTF_FISH_TYPE.Fish_Same_Turtle:
		case STTF_FISH_TYPE.Fish_SuperBomb:
		case STTF_FISH_TYPE.Fish_FixBomb:
		case STTF_FISH_TYPE.Fish_BigEars_Group:
		case STTF_FISH_TYPE.Fish_YellowSpot_Group:
		case STTF_FISH_TYPE.Fish_Hedgehog_Group:
		case STTF_FISH_TYPE.Fish_Ugly_Group:
		case STTF_FISH_TYPE.Fish_BlueAlgae_Group:
		case STTF_FISH_TYPE.Fish_Turtle_Group:
		case STTF_FISH_TYPE.Fish_PartBomb:
		case STTF_FISH_TYPE.Fish_Boss:
			return true;
		default:
			return false;
		}
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
		return TF_PoolManager.Pools["TFEffectPool"].Spawn(trans);
	}

	public void DestroyEffectObj(GameObject obj)
	{
		TF_PoolManager.Pools["TFEffectPool"].Despawn(obj.transform);
	}

	private void Init()
	{
		for (int i = 0; i < 4; i++)
		{
			mPlayerCtrl[i] = base.transform.GetChild(i).GetComponent<STTF_PlayerEffectCtrl>();
		}
	}

	public int GetFlyCoinNum(STTF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STTF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STTF_FISH_TYPE.Fish_Grass:
			return 2;
		case STTF_FISH_TYPE.Fish_Zebra:
			return 3;
		case STTF_FISH_TYPE.Fish_BigEars:
			return 4;
		case STTF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STTF_FISH_TYPE.Fish_Ugly:
			return 6;
		case STTF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STTF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STTF_FISH_TYPE.Fish_Lamp:
			return 9;
		case STTF_FISH_TYPE.Fish_Turtle:
			return 2;
		case STTF_FISH_TYPE.Fish_Trailer:
			return 3;
		case STTF_FISH_TYPE.Fish_Butterfly:
			return 3;
		case STTF_FISH_TYPE.Fish_Beauty:
			return 4;
		case STTF_FISH_TYPE.Fish_Arrow:
			return 4;
		case STTF_FISH_TYPE.Fish_Bat:
			return 5;
		case STTF_FISH_TYPE.Fish_SilverShark:
			return 6;
		case STTF_FISH_TYPE.Fish_GoldenShark:
			return 7;
		case STTF_FISH_TYPE.Fish_BigShark:
			return 10;
		case STTF_FISH_TYPE.Fish_Toad:
			return 10;
		case STTF_FISH_TYPE.Fish_Dragon:
			return 10;
		case STTF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STTF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STTF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STTF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STTF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STTF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STTF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STTF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STTF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STTF_FISH_TYPE.Fish_Same_Turtle:
			return 2;
		case STTF_FISH_TYPE.Fish_FixBomb:
			return 4;
		case STTF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STTF_FISH_TYPE.Fish_SuperBomb:
			return 10;
		case STTF_FISH_TYPE.Fish_BigEars_Group:
			return 2;
		case STTF_FISH_TYPE.Fish_YellowSpot_Group:
			return 3;
		case STTF_FISH_TYPE.Fish_Hedgehog_Group:
			return 4;
		case STTF_FISH_TYPE.Fish_Ugly_Group:
			return 4;
		case STTF_FISH_TYPE.Fish_BlueAlgae_Group:
			return 6;
		case STTF_FISH_TYPE.Fish_Turtle_Group:
			return 8;
		case STTF_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public int GetFishOODS(STTF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STTF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case STTF_FISH_TYPE.Fish_Grass:
			return 2;
		case STTF_FISH_TYPE.Fish_Zebra:
			return 3;
		case STTF_FISH_TYPE.Fish_BigEars:
			return 4;
		case STTF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case STTF_FISH_TYPE.Fish_Ugly:
			return 6;
		case STTF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case STTF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case STTF_FISH_TYPE.Fish_Lamp:
			return 9;
		case STTF_FISH_TYPE.Fish_Turtle:
			return 10;
		case STTF_FISH_TYPE.Fish_Trailer:
			return 12;
		case STTF_FISH_TYPE.Fish_Butterfly:
			return 15;
		case STTF_FISH_TYPE.Fish_Beauty:
			return 18;
		case STTF_FISH_TYPE.Fish_Arrow:
			return 20;
		case STTF_FISH_TYPE.Fish_Bat:
			return 25;
		case STTF_FISH_TYPE.Fish_SilverShark:
			return 30;
		case STTF_FISH_TYPE.Fish_GoldenShark:
			return 35;
		case STTF_FISH_TYPE.Fish_BigShark:
			return 40;
		case STTF_FISH_TYPE.Fish_Toad:
			return 100;
		case STTF_FISH_TYPE.Fish_Dragon:
			return 120;
		case STTF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case STTF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case STTF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case STTF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case STTF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case STTF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case STTF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case STTF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case STTF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case STTF_FISH_TYPE.Fish_Same_Turtle:
			return 10;
		case STTF_FISH_TYPE.Fish_FixBomb:
			return 20;
		case STTF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case STTF_FISH_TYPE.Fish_SuperBomb:
			return 50;
		case STTF_FISH_TYPE.Fish_BigEars_Group:
			return 12;
		case STTF_FISH_TYPE.Fish_YellowSpot_Group:
			return 15;
		case STTF_FISH_TYPE.Fish_Hedgehog_Group:
			return 21;
		case STTF_FISH_TYPE.Fish_Ugly_Group:
			return 24;
		case STTF_FISH_TYPE.Fish_BlueAlgae_Group:
			return 32;
		case STTF_FISH_TYPE.Fish_Turtle_Group:
			return 40;
		case STTF_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public void ResetAllEffect()
	{
		TF_PoolManager.Pools["TFEffectPool"].DespawnAll();
	}
}
