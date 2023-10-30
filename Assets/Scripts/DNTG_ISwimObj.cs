using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DNTG_ISwimObj : DNTG_FishForFormation
{
	private GameObject objLockFlag;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	[HideInInspector]
	public int mServerID = -9999;

	public DNTG_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D[] mBoxCollider;

	public SpecialFishType specialFishType;

	[HideInInspector]
	public int fishKingPos = -1;

	[HideInInspector]
	public int monkeyKingBet = -1;

	private Text betText;

	private void Awake()
	{
		GetObjLockFlag();
		GetAllBoxCollider();
	}

	private void OnEnable()
	{
		GetAllBoxCollider();
		ZH2_GVars.setMonkeyKingBet = (Action<object[]>)Delegate.Combine(ZH2_GVars.setMonkeyKingBet, new Action<object[]>(SetMonkeyKingBet));
	}

	private void OnDisable()
	{
		ZH2_GVars.setMonkeyKingBet = (Action<object[]>)Delegate.Remove(ZH2_GVars.setMonkeyKingBet, new Action<object[]>(SetMonkeyKingBet));
	}

	public void SetFishType(DNTG_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
		SetTag(typ);
	}

	public void SetSpecialFishType(SpecialFishType specialFishType)
	{
		this.specialFishType = specialFishType;
	}

	public void SetMonkeyKingBet(int monkeyKingBet)
	{
		this.monkeyKingBet = monkeyKingBet;
		SetBet();
	}

	private void SetMonkeyKingBet(object[] args)
	{
		if (specialFishType == SpecialFishType.MonkeyKing)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			monkeyKingBet = (int)dictionary["bet"];
			SetBet();
		}
	}

	private void SetBet()
	{
		if (betText == null)
		{
			Transform transform = base.transform.Find("Text");
			if (transform != null)
			{
				betText = transform.GetComponent<Text>();
			}
		}
		if (betText != null)
		{
			betText.text = monkeyKingBet.ToString();
		}
	}

	public void SetFishKingPos(int fishKingPos)
	{
		if (fishKingPos >= 0)
		{
			this.fishKingPos = fishKingPos;
		}
	}

	public void SetFishSvrID(int nSvrID)
	{
		mServerID = nSvrID;
	}

	public int GetFishSvrID()
	{
		return mServerID;
	}

	public void DoSwimAnim()
	{
		_doSwimAnim();
	}

	public void DoDieAnim()
	{
		_doDieAnim();
	}

	public virtual void GoDieLightning(object obj)
	{
		UnityEngine.Debug.LogError(base.gameObject.name + "是被电死的");
		DNTG_BulletPara dNTG_BulletPara = (DNTG_BulletPara)obj;
		if (dNTG_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<DNTG_DoMove>().Over();
			GetComponent<DNTG_DoMove>().enabled = false;
			DNTG_EffectMngr.GetSingleton().PlayCoinFly(dNTG_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	public virtual void GoDie(object obj)
	{
		DNTG_BulletPara dNTG_BulletPara = (DNTG_BulletPara)obj;
		if (dNTG_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<DNTG_DoMove>().Over();
			GetComponent<DNTG_DoMove>().enabled = false;
			DNTG_EffectMngr.GetSingleton().PlayCoinFly(dNTG_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			switch (mFishType)
			{
			case DNTG_FISH_TYPE.Fish_Same_Shrimp:
			case DNTG_FISH_TYPE.Fish_Same_Grass:
			case DNTG_FISH_TYPE.Fish_Same_Zebra:
			case DNTG_FISH_TYPE.Fish_Same_BigEars:
			case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
			case DNTG_FISH_TYPE.Fish_KillDouble_One:
			case DNTG_FISH_TYPE.Fish_KillDouble_Two:
			case DNTG_FISH_TYPE.Fish_KillDouble_Three:
			case DNTG_FISH_TYPE.Fish_KillDouble_Four:
			case DNTG_FISH_TYPE.Fish_KillDouble_Five:
			case DNTG_FISH_TYPE.Fish_KillThree_One:
			case DNTG_FISH_TYPE.Fish_KillThree_Two:
			case DNTG_FISH_TYPE.Fish_KillThree_Three:
			case DNTG_FISH_TYPE.Fish_KillThree_Four:
				DNTG_EffectMngr.GetSingleton().ShowEffBingChuanBomb(base.transform.position);
				break;
			case DNTG_FISH_TYPE.Fish_SilverShark:
			case DNTG_FISH_TYPE.Fish_GoldenShark:
			case DNTG_FISH_TYPE.Fish_BlueDolphin:
			case DNTG_FISH_TYPE.Fish_Boss:
			case DNTG_FISH_TYPE.Fish_Monkey:
			case DNTG_FISH_TYPE.Fish_GoldFull:
			case DNTG_FISH_TYPE.Fish_Penguin:
			case DNTG_FISH_TYPE.Fish_GoldenDragon:
			case DNTG_FISH_TYPE.Fish_Sailboat:
				DNTG_EffectMngr.GetSingleton().ShowEffGoldBomb(base.transform.position);
				break;
			case DNTG_FISH_TYPE.Fish_FixBomb:
				DNTG_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
				break;
			case DNTG_FISH_TYPE.Fish_LocalBomb:
			case DNTG_FISH_TYPE.Fish_Wheels:
				DNTG_EffectMngr.GetSingleton().ShowEffPartBomb(base.transform.position);
				break;
			case DNTG_FISH_TYPE.Fish_SuperBomb:
				UnityEngine.Debug.Log("打中超级炸弹");
				DNTG_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
				break;
			}
			if (specialFishType == SpecialFishType.LightningFish)
			{
				UnityEngine.Debug.LogError("====被打死的闪电鱼: " + base.gameObject.name);
				DNTG_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		DNTG_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		DNTG_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
		if (specialFishType == SpecialFishType.HeavenFish)
		{
			DNTG_FISH_TYPE dNTG_FISH_TYPE = mFishType + 20;
			UnityEngine.Debug.LogError($"{mFishType} 死亡,出鱼王 {dNTG_FISH_TYPE} 鱼阵");
			DNTG_Formation.GetSingleton().ShowFormation(DNTG_FORMATION.Formation_FishArray, base.transform.position, mFishType, dNTG_FISH_TYPE, fishKingPos);
		}
		yield return new WaitForSeconds(2f);
		ObjDestroy();
	}

	public void BeRemoved()
	{
		if (!bFishDead)
		{
			bFishDead = true;
			StopCoroutine("IE_Remove");
			StartCoroutine("IE_Remove");
		}
	}

	private IEnumerator IE_Remove()
	{
		yield return new WaitForEndOfFrame();
		ObjDestroy();
	}

	public void ObjDestroy()
	{
		SetBoxColl(isEnabled: false);
		GetComponent<DNTG_DoMove>().Reset();
		GetComponent<DNTG_DoMove>().enabled = false;
		FormationType = DNTG_FORMATION.Formation_Normal;
		DNTG_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
	}

	protected abstract void _doSwimAnim();

	protected abstract void _doDieAnim();

	protected abstract void _onDestroy();

	protected abstract void _setFishLayer();

	public virtual void SetUpDir(bool isR)
	{
	}

	protected abstract void _setCollider();

	public void OnSpawned()
	{
		bFishDead = false;
		DoSwimAnim();
	}

	public void OnDespawned()
	{
		_onDestroy();
		FormationType = DNTG_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<DNTG_DoMove>().Reset();
		GetComponent<DNTG_DoMove>().enabled = false;
	}

	public void OnPathEnd()
	{
		ObjDestroy();
	}

	public void OnOtherCollision(Collider2D other)
	{
		if (!(other.name == "Bullet"))
		{
			return;
		}
		DNTG_Bullet component = other.transform.parent.GetComponent<DNTG_Bullet>();
		if (bFishDead || component.mIsDead || !base.gameObject.activeSelf)
		{
			return;
		}
		if (component.IsLocking)
		{
			int value = -1;
			int num = 0;
			while (true)
			{
				if (num < DNTG_GameInfo.getInstance().UserList.Count)
				{
					if (DNTG_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)DNTG_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			DNTG_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(DNTG_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (DNTG_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
			{
				component.ObjDestroy(base.gameObject);
			}
		}
		else
		{
			component.ObjDestroy(base.gameObject);
		}
	}

	public void InitLock(int seatid)
	{
		GameObject y = null;
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().User.SeatIndex == DNTG_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = DNTG_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < DNTG_GameInfo.getInstance().UserList.Count; j++)
		{
			if (DNTG_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)DNTG_GameInfo.getInstance().UserList[j].LockFish && (DNTG_GameInfo.getInstance().User.SeatIndex == seatid || DNTG_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				GetObjLockFlag();
				DNTG_LockFlag component = objLockFlag.GetComponent<DNTG_LockFlag>();
				if (component != null)
				{
					component.seatId = seatid;
					component.InitLockFlag();
					objLockFlag.gameObject.SetActive(value: true);
				}
			}
		}
	}

	public Vector3 GetLockFishPos()
	{
		GetObjLockFlag();
		return objLockFlag.transform.position;
	}

	public void HideLockedFlag()
	{
		GetObjLockFlag();
		objLockFlag.SetActive(value: false);
	}

	private void GetObjLockFlag()
	{
		if (objLockFlag == null)
		{
			Transform transform = base.transform.Find("LockFlag");
			if (transform != null)
			{
				objLockFlag = transform.gameObject;
			}
			else if (DNTG_FishPoolMngr.G_FishPoolMngr != null)
			{
				objLockFlag = UnityEngine.Object.Instantiate(DNTG_FishPoolMngr.G_FishPoolMngr.objLockFlag, base.transform);
				objLockFlag.name = "LockFlag";
			}
		}
	}

	public void SetBoxColl(bool isEnabled)
	{
		GetAllBoxCollider();
		int num = 0;
		while (true)
		{
			if (num < mBoxCollider.Length)
			{
				if (!(mBoxCollider[num] != null))
				{
					break;
				}
				mBoxCollider[num].enabled = isEnabled;
				num++;
				continue;
			}
			return;
		}
		UnityEngine.Debug.LogError("=====" + base.gameObject.name + "mBoxCollider为空");
	}

	private void GetAllBoxCollider()
	{
		if (mBoxCollider.Length > 0 && mBoxCollider[0] != null)
		{
			return;
		}
		mBoxCollider = GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
		if (mBoxCollider.Length > 0)
		{
			return;
		}
		mBoxCollider = new BoxCollider2D[1];
		Transform transform = base.transform.Find("Fish");
		if (transform != null)
		{
			mBoxCollider[0] = transform.GetComponent<BoxCollider2D>();
			return;
		}
		UnityEngine.Debug.LogError(base.gameObject.name + "===获取fish失败");
		Transform transform2 = base.transform.Find("DriftBottle");
		if (transform2 != null)
		{
			mBoxCollider[0] = transform2.GetComponent<BoxCollider2D>();
		}
		else
		{
			UnityEngine.Debug.LogError(base.gameObject.name + "===获取driftBottle失败");
		}
	}

	public bool IsLockFishOutsideWindow()
	{
		Transform x = base.transform.Find("Fish");
		Vector3 vector = Vector3.zero;
		if (x != null)
		{
			vector = base.transform.Find("Fish").position;
		}
		else
		{
			if (mFishType == DNTG_FISH_TYPE.Fish_GoldFull)
			{
				x = base.transform.Find("MiddelFish/Fish");
			}
			if (x == null)
			{
				UnityEngine.Debug.LogError(base.gameObject.name + " 未找到Fish");
				return true;
			}
		}
		float num = 6.3f;
		float num2 = 3.45f;
		return vector.x < 0f - num || vector.x > num || vector.y < 0f - num2 || vector.y > num2;
	}

	private void SetTag(DNTG_FISH_TYPE typ)
	{
		base.gameObject.tag = "NormalFish";
	}
}
