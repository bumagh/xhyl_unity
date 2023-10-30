using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class DK_ISwimObj : DK_FishForFormation
{
	private DK_LockFlag sptLockFlag;

	public DK_FISH_TYPE mSecondFishType = DK_FISH_TYPE.Fish_TYPE_NONE;

	public DK_FISH_TYPE mFirstFishType = DK_FISH_TYPE.Fish_TYPE_NONE;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public DK_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(DK_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
		SetTag(typ);
	}

	public void SetFirstFishType(DK_FISH_TYPE typ)
	{
		mFirstFishType = typ;
	}

	public void SetSecondFishType(DK_FISH_TYPE typ)
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

	public virtual void GoDie2(object obj)
	{
		DK_BulletPara dK_BulletPara = (DK_BulletPara)obj;
		if (dK_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<DK_DoMove>().Over();
			GetComponent<DK_DoMove>().enabled = false;
			if ((mFishType <= DK_FISH_TYPE.Fish_Same_Turtle && mFishType >= DK_FISH_TYPE.Fish_Same_Shrimp) || mFishType == DK_FISH_TYPE.Fish_Double_Kill)
			{
				DK_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			else if ((mFishType >= DK_FISH_TYPE.Fish_SilverShark && mFishType <= DK_FISH_TYPE.Fish_NiuMoWang) || (mFishType >= DK_FISH_TYPE.Fish_BigEars_Group && mFishType <= DK_FISH_TYPE.Fish_Turtle_Group))
			{
				DK_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			else if (mFishType == DK_FISH_TYPE.Fish_FixBomb)
			{
				DK_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
			}
			else if (mFishType == DK_FISH_TYPE.Fish_SuperBomb)
			{
				DK_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		DK_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		DK_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
		yield return new WaitForSeconds(2f);
		ZH2_GVars.niuMoWangBeiLv = 0;
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
		GetComponent<DK_DoMove>().Reset();
		GetComponent<DK_DoMove>().enabled = false;
		FormationType = DK_FORMATION.Formation_Normal;
		if (mFishType == DK_FISH_TYPE.Fish_Double_Kill)
		{
			Transform child = base.transform.Find("Fish").GetChild(0);
			Transform child2 = base.transform.Find("Fish").GetChild(1);
			child.SetParent(base.transform.parent);
			child2.SetParent(base.transform.parent);
			DK_PoolManager.Pools["DKFishPool"].Despawn(child);
			DK_PoolManager.Pools["DKFishPool"].Despawn(child2);
		}
		DK_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		FormationType = DK_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<DK_DoMove>().Reset();
		GetComponent<DK_DoMove>().enabled = false;
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
		DK_Bullet component = other.transform.parent.GetComponent<DK_Bullet>();
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
				if (num < DK_GameInfo.getInstance().UserList.Count)
				{
					if (DK_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)DK_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			DK_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(DK_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (DK_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().User.SeatIndex == DK_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = DK_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < DK_GameInfo.getInstance().UserList.Count; j++)
		{
			if (DK_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)DK_GameInfo.getInstance().UserList[j].LockFish && (DK_GameInfo.getInstance().User.SeatIndex == seatid || DK_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				if (sptLockFlag == null)
				{
					sptLockFlag = base.transform.Find("LockFlag").GetComponent<DK_LockFlag>();
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<DK_LockFlag>();
		}
		sptLockFlag.gameObject.SetActive(value: false);
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].LockFish == base.gameObject && sptLockFlag != null)
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<DK_LockFlag>();
		}
		Vector3 position = sptLockFlag.transform.position;
		float num = 6.3f;
		float num2 = 3.45f;
		return position.x < 0f - num || position.x > num || position.y < 0f - num2 || position.y > num2;
	}

	private void SetTag(DK_FISH_TYPE typ)
	{
		switch (typ)
		{
		case DK_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case DK_FISH_TYPE.Fish_SuperBomb:
			base.gameObject.tag = "SuperBomb";
			break;
		case DK_FISH_TYPE.Fish_FixBomb:
			base.gameObject.tag = "StopBomb";
			break;
		case DK_FISH_TYPE.Fish_BigEars_Group:
		case DK_FISH_TYPE.Fish_YellowSpot_Group:
		case DK_FISH_TYPE.Fish_Hedgehog_Group:
		case DK_FISH_TYPE.Fish_Ugly_Group:
		case DK_FISH_TYPE.Fish_BlueAlgae_Group:
		case DK_FISH_TYPE.Fish_Turtle_Group:
		case DK_FISH_TYPE.Fish_Double_Kill:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
