using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class STQM_ISwimObj : STQM_FishForFormation
{
	private STQM_LockFlag sptLockFlag;

	public STQM_FISH_TYPE mSecondFishType = STQM_FISH_TYPE.Fish_TYPE_NONE;

	public STQM_FISH_TYPE mFirstFishType = STQM_FISH_TYPE.Fish_TYPE_NONE;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public STQM_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(STQM_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
		SetTag(typ);
	}

	public void SetFirstFishType(STQM_FISH_TYPE typ)
	{
		mFirstFishType = typ;
	}

	public void SetSecondFishType(STQM_FISH_TYPE typ)
	{
		mSecondFishType = typ;
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

	public virtual void GoDie(object obj)
	{
		STQM_BulletPara sTQM_BulletPara = (STQM_BulletPara)obj;
		if (sTQM_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<STQM_DoMove>().Stop();
			GetComponent<STQM_DoMove>().enabled = false;
			if (mFishType != STQM_FISH_TYPE.Fish_BuleWhale)
			{
				int num = STQM_EffectMngr.GetSingleton().GetFishOODS(mFishType) * sTQM_BulletPara.mPower;
				STQM_EffectMngr.GetSingleton().ShowFishScore(sTQM_BulletPara.mPlyerIndex, base.transform.position, num);
				STQM_FishPoolMngr.GetSingleton().totalScore += num;
			}
			STQM_EffectMngr.GetSingleton().PlayCoinFly(sTQM_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			if (mFishType <= STQM_FISH_TYPE.Fish_Same_Turtle && mFishType >= STQM_FISH_TYPE.Fish_Same_Shrimp)
			{
				STQM_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			else if (mFishType >= STQM_FISH_TYPE.Fish_SilverShark && mFishType <= STQM_FISH_TYPE.Fish_BowlFish)
			{
				STQM_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			else if (mFishType == STQM_FISH_TYPE.Fish_AllBomb)
			{
				STQM_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		STQM_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		STQM_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
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
		mBoxCollider.enabled = false;
		GetComponent<STQM_DoMove>().Reset();
		GetComponent<STQM_DoMove>().enabled = false;
		formationType = STQM_FORMATION.Formation_Normal;
		STQM_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		formationType = STQM_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<STQM_DoMove>().Reset();
		GetComponent<STQM_DoMove>().enabled = false;
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
		STQM_Bullet component = other.transform.parent.GetComponent<STQM_Bullet>();
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
				if (num < STQM_GameInfo.getInstance().UserList.Count)
				{
					if (STQM_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)STQM_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			STQM_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(STQM_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (STQM_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().User.SeatIndex == STQM_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = STQM_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < STQM_GameInfo.getInstance().UserList.Count; j++)
		{
			if (STQM_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)STQM_GameInfo.getInstance().UserList[j].LockFish && (STQM_GameInfo.getInstance().User.SeatIndex == seatid || STQM_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				if (sptLockFlag == null)
				{
					sptLockFlag = base.transform.Find("LockFlag").GetComponent<STQM_LockFlag>();
				}
				if (sptLockFlag != null)
				{
					sptLockFlag.seatId = seatid;
					sptLockFlag.InitLockFlag();
					sptLockFlag.gameObject.SetActive(value: true);
				}
			}
		}
	}

	public Vector3 GetLockFishPos()
	{
		return base.transform.position;
	}

	public Vector3 GetLockFishLocalPos()
	{
		return base.transform.localPosition;
	}

	public void HideLockedFlag()
	{
		if (sptLockFlag == null)
		{
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<STQM_LockFlag>();
		}
		sptLockFlag.gameObject.SetActive(value: false);
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().UserList[i].LockFish == base.gameObject && sptLockFlag != null)
			{
				sptLockFlag.seatId = i + 1;
				sptLockFlag.InitLockFlag();
				sptLockFlag.gameObject.SetActive(value: true);
			}
		}
	}

	public bool IsLockFishOutsideWindow()
	{
		if (sptLockFlag == null)
		{
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<STQM_LockFlag>();
		}
		Vector3 position = sptLockFlag.transform.position;
		float num = 6.3f;
		float num2 = 3.45f;
		return position.x < 0f - num || position.x > num || position.y < 0f - num2 || position.y > num2;
	}

	private void SetTag(STQM_FISH_TYPE typ)
	{
		switch (typ)
		{
		case STQM_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case STQM_FISH_TYPE.Fish_AllBomb:
			base.gameObject.tag = "SuperBomb";
			break;
		case STQM_FISH_TYPE.Fish_BowlFish:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
