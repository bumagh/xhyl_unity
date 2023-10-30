using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class TF_ISwimObj : TF_FishForFormation
{
	private TF_LockFlag sptLockFlag;

	public TF_FISH_TYPE mSecondFishType = TF_FISH_TYPE.Fish_TYPE_NONE;

	public TF_FISH_TYPE mFirstFishType = TF_FISH_TYPE.Fish_TYPE_NONE;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public TF_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(TF_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
	}

	public void SetFirstFishType(TF_FISH_TYPE typ)
	{
		mFirstFishType = typ;
	}

	public void SetSecondFishType(TF_FISH_TYPE typ)
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
		TF_BulletPara tF_BulletPara = (TF_BulletPara)obj;
		if (tF_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<TF_DoMove>().Over();
			GetComponent<TF_DoMove>().enabled = false;
			if (mFishType < TF_FISH_TYPE.Fish_BigShark || mFishType > TF_FISH_TYPE.Fish_Dragon)
			{
				int nScore = TF_EffectMngr.GetSingleton().GetFishOODS(mFishType) * tF_BulletPara.mPower;
				TF_EffectMngr.GetSingleton().ShowFishScore(tF_BulletPara.mPlyerIndex, base.transform.position, nScore);
				TF_EffectMngr.GetSingleton().PlayCoinFly(tF_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			}
			if (mFishType <= TF_FISH_TYPE.Fish_Same_Turtle && mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp)
			{
				TF_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			else if ((mFishType >= TF_FISH_TYPE.Fish_SilverShark && mFishType <= TF_FISH_TYPE.Fish_Dragon) || (mFishType >= TF_FISH_TYPE.Fish_BigEars_Group && mFishType <= TF_FISH_TYPE.Fish_Turtle_Group))
			{
				TF_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			else if (mFishType == TF_FISH_TYPE.Fish_FixBomb)
			{
				TF_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
			}
			else if (mFishType == TF_FISH_TYPE.Fish_SuperBomb)
			{
				TF_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	public virtual void GoDie1(object obj)
	{
		TF_BulletPara tF_BulletPara = (TF_BulletPara)obj;
		if (tF_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<TF_DoMove>().Over();
			GetComponent<TF_DoMove>().enabled = false;
			int nScore = TF_EffectMngr.GetSingleton().GetFishOODS(mFishType) * tF_BulletPara.mPower;
			TF_EffectMngr.GetSingleton().ShowFishScore(tF_BulletPara.mPlyerIndex, base.transform.position, nScore);
			TF_EffectMngr.GetSingleton().PlayCoinFly(tF_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			if (mFishType <= TF_FISH_TYPE.Fish_Same_Turtle && mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp)
			{
				TF_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			else if ((mFishType >= TF_FISH_TYPE.Fish_SilverShark && mFishType <= TF_FISH_TYPE.Fish_Dragon) || (mFishType >= TF_FISH_TYPE.Fish_BigEars_Group && mFishType <= TF_FISH_TYPE.Fish_Turtle_Group))
			{
				TF_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			else if (mFishType == TF_FISH_TYPE.Fish_FixBomb)
			{
				TF_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
			}
			else if (mFishType == TF_FISH_TYPE.Fish_SuperBomb)
			{
				TF_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		TF_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		TF_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
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
		GetComponent<TF_DoMove>().Reset();
		GetComponent<TF_DoMove>().enabled = false;
		FormationType = TF_FORMATION.Formation_Normal;
		TF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		FormationType = TF_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<TF_DoMove>().Reset();
		GetComponent<TF_DoMove>().enabled = false;
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
		TF_Bullet component = other.transform.parent.GetComponent<TF_Bullet>();
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
				if (num < TF_GameInfo.getInstance().UserList.Count)
				{
					if (TF_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)TF_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			TF_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(TF_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (TF_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().User.SeatIndex == TF_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = TF_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < TF_GameInfo.getInstance().UserList.Count; j++)
		{
			if (TF_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)TF_GameInfo.getInstance().UserList[j].LockFish && (TF_GameInfo.getInstance().User.SeatIndex == seatid || TF_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				if (sptLockFlag == null)
				{
					sptLockFlag = base.transform.Find("LockFlag").GetComponent<TF_LockFlag>();
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<TF_LockFlag>();
		}
		sptLockFlag.gameObject.SetActive(value: false);
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].LockFish == base.gameObject && sptLockFlag != null)
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<TF_LockFlag>();
		}
		Vector3 position = sptLockFlag.transform.position;
		float num = 6.3f;
		float num2 = 3.45f;
		return position.x < 0f - num || position.x > num || position.y < 0f - num2 || position.y > num2;
	}
}
