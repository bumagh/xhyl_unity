using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class TF_EffectMngr : MonoBehaviour
{
	public static TF_EffectMngr G_EffectMngr;

	private TF_PlayerEffectCtrl[] mPlayerCtrl = new TF_PlayerEffectCtrl[4];

	[SerializeField]
	private GameObject objEffSimilarBomb;

	private List<GameObject> listEffSimilarBomb = new List<GameObject>();

	[SerializeField]
	private TF_BigFishEffect[] listEffSuperBomb;

	[SerializeField]
	private GameObject objEffTimeStop;

	private List<GameObject> listEffTimeStop = new List<GameObject>();

	[SerializeField]
	private GameObject objEffGroup;

	private List<GameObject> listEffGroup = new List<GameObject>();

	public static TF_EffectMngr GetSingleton()
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

	public void PlayCoinFly(int nPlayerID, TF_FISH_TYPE fishType, Vector3 pos)
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

	public void ShowEffSuperBomb(Vector3 pos)
	{
		TF_BigFishEffect tF_BigFishEffect = null;
		for (int i = 0; i < listEffSuperBomb.Length; i++)
		{
			if (!listEffSuperBomb[i].bPlaying)
			{
				tF_BigFishEffect = listEffSuperBomb[i];
			}
		}
		if (tF_BigFishEffect != null)
		{
			tF_BigFishEffect.PlayEffect(pos);
		}
		TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
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
		TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
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

	public bool IsBigFish(TF_FISH_TYPE fishtype)
	{
		bool result = false;
		if (fishtype >= TF_FISH_TYPE.Fish_GoldenShark && fishtype <= TF_FISH_TYPE.Fish_Turtle_Group && fishtype != TF_FISH_TYPE.Fish_CoralReefs)
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
			mPlayerCtrl[i] = base.transform.GetChild(i).GetComponent<TF_PlayerEffectCtrl>();
		}
	}

	public int GetFlyCoinNum(TF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case TF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case TF_FISH_TYPE.Fish_Grass:
			return 2;
		case TF_FISH_TYPE.Fish_Zebra:
			return 3;
		case TF_FISH_TYPE.Fish_BigEars:
			return 4;
		case TF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case TF_FISH_TYPE.Fish_Ugly:
			return 6;
		case TF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case TF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case TF_FISH_TYPE.Fish_Lamp:
			return 9;
		case TF_FISH_TYPE.Fish_Turtle:
			return 2;
		case TF_FISH_TYPE.Fish_Trailer:
			return 3;
		case TF_FISH_TYPE.Fish_Butterfly:
			return 3;
		case TF_FISH_TYPE.Fish_Beauty:
			return 4;
		case TF_FISH_TYPE.Fish_Arrow:
			return 4;
		case TF_FISH_TYPE.Fish_Bat:
			return 5;
		case TF_FISH_TYPE.Fish_SilverShark:
			return 6;
		case TF_FISH_TYPE.Fish_GoldenShark:
			return 7;
		case TF_FISH_TYPE.Fish_BigShark:
			return 10;
		case TF_FISH_TYPE.Fish_Toad:
			return 10;
		case TF_FISH_TYPE.Fish_Dragon:
			return 10;
		case TF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case TF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case TF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case TF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case TF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case TF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case TF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case TF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case TF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case TF_FISH_TYPE.Fish_Same_Turtle:
			return 2;
		case TF_FISH_TYPE.Fish_FixBomb:
			return 4;
		case TF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case TF_FISH_TYPE.Fish_SuperBomb:
			return 10;
		case TF_FISH_TYPE.Fish_BigEars_Group:
			return 2;
		case TF_FISH_TYPE.Fish_YellowSpot_Group:
			return 3;
		case TF_FISH_TYPE.Fish_Hedgehog_Group:
			return 4;
		case TF_FISH_TYPE.Fish_Ugly_Group:
			return 4;
		case TF_FISH_TYPE.Fish_BlueAlgae_Group:
			return 6;
		case TF_FISH_TYPE.Fish_Turtle_Group:
			return 8;
		case TF_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public int GetFishOODS(TF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case TF_FISH_TYPE.Fish_Shrimp:
			return 2;
		case TF_FISH_TYPE.Fish_Grass:
			return 2;
		case TF_FISH_TYPE.Fish_Zebra:
			return 3;
		case TF_FISH_TYPE.Fish_BigEars:
			return 4;
		case TF_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case TF_FISH_TYPE.Fish_Ugly:
			return 6;
		case TF_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case TF_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case TF_FISH_TYPE.Fish_Lamp:
			return 9;
		case TF_FISH_TYPE.Fish_Turtle:
			return 10;
		case TF_FISH_TYPE.Fish_Trailer:
			return 12;
		case TF_FISH_TYPE.Fish_Butterfly:
			return 15;
		case TF_FISH_TYPE.Fish_Beauty:
			return 18;
		case TF_FISH_TYPE.Fish_Arrow:
			return 20;
		case TF_FISH_TYPE.Fish_Bat:
			return 25;
		case TF_FISH_TYPE.Fish_SilverShark:
			return 30;
		case TF_FISH_TYPE.Fish_GoldenShark:
			return 35;
		case TF_FISH_TYPE.Fish_BigShark:
			return 40;
		case TF_FISH_TYPE.Fish_Toad:
			return 100;
		case TF_FISH_TYPE.Fish_Dragon:
			return 120;
		case TF_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case TF_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case TF_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case TF_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case TF_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case TF_FISH_TYPE.Fish_Same_Ugly:
			return 6;
		case TF_FISH_TYPE.Fish_Same_Hedgehog:
			return 7;
		case TF_FISH_TYPE.Fish_Same_BlueAlgae:
			return 8;
		case TF_FISH_TYPE.Fish_Same_Lamp:
			return 9;
		case TF_FISH_TYPE.Fish_Same_Turtle:
			return 10;
		case TF_FISH_TYPE.Fish_FixBomb:
			return 20;
		case TF_FISH_TYPE.Fish_CoralReefs:
			return 0;
		case TF_FISH_TYPE.Fish_SuperBomb:
			return 50;
		case TF_FISH_TYPE.Fish_BigEars_Group:
			return 12;
		case TF_FISH_TYPE.Fish_YellowSpot_Group:
			return 15;
		case TF_FISH_TYPE.Fish_Hedgehog_Group:
			return 21;
		case TF_FISH_TYPE.Fish_Ugly_Group:
			return 24;
		case TF_FISH_TYPE.Fish_BlueAlgae_Group:
			return 32;
		case TF_FISH_TYPE.Fish_Turtle_Group:
			return 40;
		case TF_FISH_TYPE.Fish_TYPE_NONE:
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
