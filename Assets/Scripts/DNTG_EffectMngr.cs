using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_EffectMngr : MonoBehaviour
{
	public static DNTG_EffectMngr G_EffectMngr;

	private DNTG_PlayerEffectCtrl[] mPlayerCtrl = new DNTG_PlayerEffectCtrl[4];

	[SerializeField]
	private GameObject objEffSimilarBomb;

	private List<GameObject> listEffSimilarBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffBingChuanBomb;

	private List<GameObject> listEffBingChuanBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffGoldBomb;

	private List<GameObject> listEffGoldBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffGold;

	[SerializeField]
	private GameObject objEffSimilarLightning;

	private List<GameObject> listEffGold = new List<GameObject>();

	public List<GameObject> listEffSimilarLightning = new List<GameObject>();

	[SerializeField]
	private GameObject objEffPartBomb;

	private List<GameObject> listEffPartBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffSuperBomb;

	private List<GameObject> listEffSuperBomb = new List<GameObject>();

	[SerializeField]
	private GameObject objEffTimeStop;

	private List<GameObject> listEffTimeStop = new List<GameObject>();

	[SerializeField]
	private GameObject objEffPT;

	private List<GameObject> listEffPT = new List<GameObject>();

	[HideInInspector]
	public Vector3 tongLDiePos;

	[SerializeField]
	private GameObject monkeyKingBet;

	public ShakeCamera shakeCamera;

	private float _linkElapsedTime;

	private bool isPlayer;

	public static DNTG_EffectMngr GetSingleton()
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

	private void OnEnable()
	{
		OverEffSimilarLightning();
		ZH2_GVars.DestroyEffSimilarLightning = (Action<GameObject>)Delegate.Combine(ZH2_GVars.DestroyEffSimilarLightning, new Action<GameObject>(DestroyEffectObj));
	}

	private void OnDisable()
	{
		ZH2_GVars.DestroyEffSimilarLightning = (Action<GameObject>)Delegate.Remove(ZH2_GVars.DestroyEffSimilarLightning, new Action<GameObject>(DestroyEffectObj));
	}

	private bool _checkPlayerID(int nPlayerID)
	{
		return nPlayerID <= 4 && nPlayerID >= 1;
	}

	public void PlayCoinFly(int nPlayerID, DNTG_FISH_TYPE fishType, Vector3 pos)
	{
		if (_checkPlayerID(nPlayerID))
		{
			mPlayerCtrl[nPlayerID - 1].ShowCoin(fishType, pos);
		}
		else
		{
			UnityEngine.Debug.LogError("不存在玩家id：" + nPlayerID);
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
		ShakeCamera();
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

	public void ShowEffBingChuanBomb(Vector3 pos)
	{
		ShakeCamera();
		if (listEffBingChuanBomb.Count > 2)
		{
			GameObject gameObject = listEffBingChuanBomb[0];
			listEffBingChuanBomb.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffBingChuanBomb.transform);
		transform.position = pos;
	}

	public void ShowEffGoldBomb(Vector3 pos)
	{
		ShakeCamera();
		if (listEffGoldBomb.Count > 2)
		{
			GameObject gameObject = listEffGoldBomb[0];
			listEffGoldBomb.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffGoldBomb.transform);
		transform.position = pos;
	}

	private void ShakeCamera()
	{
		if (shakeCamera != null && !shakeCamera.enabled)
		{
			shakeCamera.enabled = true;
		}
	}

	public void ShowFishKingBomb(Vector3 pos)
	{
		ShakeCamera();
		ShowGoldEff(pos);
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

	public Transform ShowmonkeyKingBet(Vector3 pos, int bet)
	{
		Transform transform = CreateEffectObj(monkeyKingBet.transform);
		transform.position = pos;
		transform.GetComponent<Text>().text = bet.ToString();
		return transform;
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
		Transform transform = CreateEffectObj(objEffSimilarLightning.transform);
		DNTG_EffSimilarLightning component = transform.GetComponent<DNTG_EffSimilarLightning>();
		if (component == null)
		{
			component = transform.gameObject.AddComponent<DNTG_EffSimilarLightning>();
		}
		Canvas component2 = transform.GetComponent<Canvas>();
		if (component2 != null)
		{
			component2.sortingOrder = 945;
		}
		listEffSimilarLightning.Add(transform.gameObject);
		isPlayer = true;
		StartCoroutine(DoUpdate(transform, tongLDiePos, fishPos));
	}

	private IEnumerator DoUpdate(Transform _chainEffect, Vector3 moveTargetPos, Vector3 fishPos)
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
			_chainEffect.localScale = new Vector3((!(scale >= 1.3f)) ? scale : 1.3f, scale, scale);
			yield return null;
		}
		yield return null;
		OverEffSimilarLightning();
	}

	public void ShowEffPartBomb(Vector3 pos)
	{
		ShakeCamera();
		if (listEffPartBomb.Count >= 2)
		{
			GameObject gameObject = listEffPartBomb[0];
			listEffPartBomb.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffPartBomb.transform);
		transform.position = pos;
		Canvas component = transform.GetComponent<Canvas>();
		if (component != null)
		{
			component.sortingOrder = 944;
		}
		listEffPartBomb.Add(transform.gameObject);
		DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffSuperBomb(Vector3 pos)
	{
		ShakeCamera();
		if (listEffSuperBomb.Count >= 2)
		{
			GameObject gameObject = listEffSuperBomb[0];
			listEffSuperBomb.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffSuperBomb.transform);
		transform.position = pos;
		transform.GetComponent<Canvas>().sortingOrder = 944;
		listEffSuperBomb.Add(transform.gameObject);
		DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
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
		DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_SUPERBOMB);
	}

	public void ShowEffTP(Vector3 pos)
	{
		ShowGoldEff(pos);
		if (listEffPT.Count >= 2)
		{
			GameObject gameObject = listEffPT[0];
			listEffPT.Remove(gameObject);
			DestroyEffectObj(gameObject);
		}
		Transform transform = CreateEffectObj(objEffPT.transform);
		transform.position = pos;
		transform.GetComponent<Canvas>().sortingOrder = 944;
		listEffPT.Add(transform.gameObject);
	}

	public bool IsBigFish(DNTG_FISH_TYPE fishtype)
	{
		bool flag = false;
		switch (fishtype)
		{
		case DNTG_FISH_TYPE.Fish_GoldenShark:
		case DNTG_FISH_TYPE.Fish_BlueDolphin:
		case DNTG_FISH_TYPE.Fish_Boss:
		case DNTG_FISH_TYPE.Fish_Monkey:
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
		case DNTG_FISH_TYPE.Fish_Same_Grass:
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
		case DNTG_FISH_TYPE.Fish_SuperBomb:
		case DNTG_FISH_TYPE.Fish_FixBomb:
		case DNTG_FISH_TYPE.Fish_LocalBomb:
		case DNTG_FISH_TYPE.Fish_Wheels:
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
		case DNTG_FISH_TYPE.Fish_KillThree_One:
		case DNTG_FISH_TYPE.Fish_KillThree_Two:
		case DNTG_FISH_TYPE.Fish_KillThree_Three:
		case DNTG_FISH_TYPE.Fish_KillThree_Four:
		case DNTG_FISH_TYPE.Fish_GoldFull:
		case DNTG_FISH_TYPE.Fish_LightningChain:
		case DNTG_FISH_TYPE.Fish_Heaven:
		case DNTG_FISH_TYPE.Fish_Penguin:
		case DNTG_FISH_TYPE.Fish_SilverDragon:
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
		case DNTG_FISH_TYPE.Fish_GoldDolphin:
		case DNTG_FISH_TYPE.Fish_Octopus:
		case DNTG_FISH_TYPE.Fish_Mermaid:
		case DNTG_FISH_TYPE.Fish_Sailboat:
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
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
		return DNTG_PoolManager.Pools["DNTGEffectPool"].Spawn(trans);
	}

	public void DestroyEffectObj(GameObject obj)
	{
		DNTG_PoolManager.Pools["DNTGEffectPool"].Despawn(obj.transform);
	}

	private void Init()
	{
		for (int i = 0; i < 4; i++)
		{
			mPlayerCtrl[i] = base.transform.GetChild(i).GetComponent<DNTG_PlayerEffectCtrl>();
		}
	}

	public int GetFlyCoinNum(DNTG_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DNTG_FISH_TYPE.Fish_Shrimp:
			return 2;
		case DNTG_FISH_TYPE.Fish_Grass:
			return 2;
		case DNTG_FISH_TYPE.Fish_Zebra:
			return 3;
		case DNTG_FISH_TYPE.Fish_BigEars:
			return 4;
		case DNTG_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case DNTG_FISH_TYPE.Fish_Ugly:
			return 6;
		case DNTG_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case DNTG_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case DNTG_FISH_TYPE.Fish_Lamp:
			return 9;
		case DNTG_FISH_TYPE.Fish_Turtle:
			return 2;
		case DNTG_FISH_TYPE.Fish_Trailer:
			return 3;
		case DNTG_FISH_TYPE.Fish_Butterfly:
			return 3;
		case DNTG_FISH_TYPE.Fish_Beauty:
			return 4;
		case DNTG_FISH_TYPE.Fish_Arrow:
			return 4;
		case DNTG_FISH_TYPE.Fish_Bat:
			return 5;
		case DNTG_FISH_TYPE.Fish_SilverShark:
			return 6;
		case DNTG_FISH_TYPE.Fish_GoldenShark:
			return 7;
		case DNTG_FISH_TYPE.Fish_Monkey:
			return 8;
		case DNTG_FISH_TYPE.Fish_BlueDolphin:
			return 8;
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
			return 10;
		case DNTG_FISH_TYPE.Fish_Penguin:
			return 10;
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case DNTG_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
			return 6;
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
			return 7;
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
			return 8;
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
			return 9;
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
			return 2;
		case DNTG_FISH_TYPE.Fish_Wheels:
			return 3;
		case DNTG_FISH_TYPE.Fish_FixBomb:
			return 4;
		case DNTG_FISH_TYPE.Fish_LocalBomb:
			return 3;
		case DNTG_FISH_TYPE.Fish_Boss:
			return 10;
		case DNTG_FISH_TYPE.Fish_SuperBomb:
			return 10;
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
			return 3;
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
			return 3;
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
			return 4;
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
			return 4;
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
			return 6;
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
			return 8;
		case DNTG_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public int GetFishOODS(DNTG_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DNTG_FISH_TYPE.Fish_Shrimp:
			return 2;
		case DNTG_FISH_TYPE.Fish_Grass:
			return 2;
		case DNTG_FISH_TYPE.Fish_Zebra:
			return 3;
		case DNTG_FISH_TYPE.Fish_BigEars:
			return 4;
		case DNTG_FISH_TYPE.Fish_YellowSpot:
			return 5;
		case DNTG_FISH_TYPE.Fish_Ugly:
			return 6;
		case DNTG_FISH_TYPE.Fish_Hedgehog:
			return 7;
		case DNTG_FISH_TYPE.Fish_BlueAlgae:
			return 8;
		case DNTG_FISH_TYPE.Fish_Lamp:
			return 9;
		case DNTG_FISH_TYPE.Fish_Turtle:
			return 10;
		case DNTG_FISH_TYPE.Fish_Trailer:
			return 12;
		case DNTG_FISH_TYPE.Fish_Butterfly:
			return 15;
		case DNTG_FISH_TYPE.Fish_Beauty:
			return 18;
		case DNTG_FISH_TYPE.Fish_Arrow:
			return 20;
		case DNTG_FISH_TYPE.Fish_Bat:
			return 25;
		case DNTG_FISH_TYPE.Fish_SilverShark:
			return 30;
		case DNTG_FISH_TYPE.Fish_GoldenShark:
			return 50;
		case DNTG_FISH_TYPE.Fish_BlueDolphin:
			return 100;
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
			return 100;
		case DNTG_FISH_TYPE.Fish_Penguin:
			return 400;
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
			return 2;
		case DNTG_FISH_TYPE.Fish_Same_Grass:
			return 2;
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
			return 3;
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
			return 4;
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
			return 5;
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
			return 8;
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
			return 9;
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
			return 8;
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
			return 13;
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
			return 15;
		case DNTG_FISH_TYPE.Fish_Wheels:
			return 20;
		case DNTG_FISH_TYPE.Fish_FixBomb:
			return 20;
		case DNTG_FISH_TYPE.Fish_LocalBomb:
			return 20;
		case DNTG_FISH_TYPE.Fish_Boss:
			return 20;
		case DNTG_FISH_TYPE.Fish_SuperBomb:
			return 30;
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
			return 12;
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
			return 15;
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
			return 21;
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
			return 24;
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
			return 32;
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
			return 40;
		case DNTG_FISH_TYPE.Fish_TYPE_NONE:
			return 0;
		default:
			return 0;
		}
	}

	public void ResetAllEffect()
	{
		DNTG_PoolManager.Pools["DNTGEffectPool"].DespawnAll();
	}
}
