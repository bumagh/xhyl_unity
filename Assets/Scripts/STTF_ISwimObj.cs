using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class STTF_ISwimObj : STTF_FishForFormation
{
	private STTF_LockFlag sptLockFlag;

	public STTF_FISH_TYPE mSecondFishType = STTF_FISH_TYPE.Fish_TYPE_NONE;

	public STTF_FISH_TYPE mFirstFishType = STTF_FISH_TYPE.Fish_TYPE_NONE;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public STTF_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(STTF_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
		SetTag(typ);
	}

	public void SetFirstFishType(STTF_FISH_TYPE typ)
	{
		mFirstFishType = typ;
	}

	public void SetSecondFishType(STTF_FISH_TYPE typ)
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
		STTF_BulletPara sTTF_BulletPara = (STTF_BulletPara)obj;
		if (sTTF_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<STTF_DoMove>().Stop();
			GetComponent<STTF_DoMove>().enabled = false;
			if (mFishType != STTF_FISH_TYPE.Fish_BigShark && mFishType != STTF_FISH_TYPE.Fish_Toad && mFishType != STTF_FISH_TYPE.Fish_Dragon && mFishType != STTF_FISH_TYPE.Fish_SuperBomb)
			{
				int num = STTF_EffectMngr.GetSingleton().GetFishOODS(mFishType) * sTTF_BulletPara.mPower;
				STTF_EffectMngr.GetSingleton().ShowFishScore(sTTF_BulletPara.mPlyerIndex, base.transform.position, num);
				STTF_FishPoolMngr.GetSingleton().totalScore += num;
			}
			STTF_EffectMngr.GetSingleton().PlayCoinFly(sTTF_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			switch (mFishType)
			{
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
				STTF_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
				break;
			case STTF_FISH_TYPE.Fish_SilverShark:
			case STTF_FISH_TYPE.Fish_GoldenShark:
			case STTF_FISH_TYPE.Fish_BigShark:
			case STTF_FISH_TYPE.Fish_Dragon:
			case STTF_FISH_TYPE.Fish_Toad:
			case STTF_FISH_TYPE.Fish_BigEars_Group:
			case STTF_FISH_TYPE.Fish_YellowSpot_Group:
			case STTF_FISH_TYPE.Fish_Hedgehog_Group:
			case STTF_FISH_TYPE.Fish_Ugly_Group:
			case STTF_FISH_TYPE.Fish_BlueAlgae_Group:
			case STTF_FISH_TYPE.Fish_Turtle_Group:
				STTF_EffectMngr.GetSingleton().ShowEffGroup(base.transform.position);
				break;
			case STTF_FISH_TYPE.Fish_FixBomb:
				STTF_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
				break;
			case STTF_FISH_TYPE.Fish_SuperBomb:
				STTF_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
				break;
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		STTF_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		STTF_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
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
		GetComponent<STTF_DoMove>().Reset();
		GetComponent<STTF_DoMove>().enabled = false;
		FormationType = STTF_FORMATION.Formation_Normal;
		STTF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		FormationType = STTF_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<STTF_DoMove>().Reset();
		GetComponent<STTF_DoMove>().enabled = false;
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
		STTF_Bullet component = other.transform.parent.GetComponent<STTF_Bullet>();
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
				if (num < STTF_GameInfo.getInstance().UserList.Count)
				{
					if (STTF_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)STTF_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			STTF_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(STTF_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (STTF_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().User.SeatIndex == STTF_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = STTF_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < STTF_GameInfo.getInstance().UserList.Count; j++)
		{
			if (STTF_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)STTF_GameInfo.getInstance().UserList[j].LockFish && (STTF_GameInfo.getInstance().User.SeatIndex == seatid || STTF_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				if (sptLockFlag == null)
				{
					sptLockFlag = base.transform.Find("LockFlag").GetComponent<STTF_LockFlag>();
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<STTF_LockFlag>();
		}
		sptLockFlag.gameObject.SetActive(value: false);
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].LockFish == base.gameObject && sptLockFlag != null)
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
			sptLockFlag = base.transform.Find("LockFlag").GetComponent<STTF_LockFlag>();
		}
		Vector3 position = sptLockFlag.transform.position;
		float num = 6.3f;
		float num2 = 3.45f;
		return position.x < 0f - num || position.x > num || position.y < 0f - num2 || position.y > num2;
	}

	private void SetTag(STTF_FISH_TYPE typ)
	{
		switch (typ)
		{
		case STTF_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case STTF_FISH_TYPE.Fish_SuperBomb:
			base.gameObject.tag = "SuperBomb";
			break;
		case STTF_FISH_TYPE.Fish_FixBomb:
			base.gameObject.tag = "StopBomb";
			break;
		case STTF_FISH_TYPE.Fish_BigEars_Group:
		case STTF_FISH_TYPE.Fish_YellowSpot_Group:
		case STTF_FISH_TYPE.Fish_Hedgehog_Group:
		case STTF_FISH_TYPE.Fish_Ugly_Group:
		case STTF_FISH_TYPE.Fish_BlueAlgae_Group:
		case STTF_FISH_TYPE.Fish_Turtle_Group:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
