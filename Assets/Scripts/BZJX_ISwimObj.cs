using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class BZJX_ISwimObj : BZJX_FishForFormation
{
	public GameObject objLockFlag;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public BZJX_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(BZJX_FISH_TYPE typ)
	{
		mFishType = typ;
		_setFishLayer();
		_setCollider();
		SetTag(typ);
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
		BZJX_BulletPara bZJX_BulletPara = (BZJX_BulletPara)obj;
		if (bZJX_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<BZJX_DoMove>().Over();
			GetComponent<BZJX_DoMove>().enabled = false;
			if (mFishType != BZJX_FISH_TYPE.Fish_GoldenDragon && mFishType != BZJX_FISH_TYPE.Fish_GoldenSharkB && mFishType != BZJX_FISH_TYPE.Fish_Boss && mFishType != BZJX_FISH_TYPE.Fish_SuperBomb && mFishType != BZJX_FISH_TYPE.Fish_PartBomb)
			{
				int num = BZJX_EffectMngr.GetSingleton().GetFishOODS(mFishType) * bZJX_BulletPara.mPower;
				BZJX_EffectMngr.GetSingleton().ShowFishScore(bZJX_BulletPara.mPlyerIndex, base.transform.position, num);
				BZJX_FishPoolMngr.GetSingleton().totalScore += num;
			}
			BZJX_EffectMngr.GetSingleton().PlayCoinFly(bZJX_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			switch (mFishType)
			{
			case BZJX_FISH_TYPE.Fish_Same_Shrimp:
			case BZJX_FISH_TYPE.Fish_Same_Grass:
			case BZJX_FISH_TYPE.Fish_Same_Zebra:
			case BZJX_FISH_TYPE.Fish_Same_BigEars:
			case BZJX_FISH_TYPE.Fish_Same_YellowSpot:
			case BZJX_FISH_TYPE.Fish_Same_Ugly:
			case BZJX_FISH_TYPE.Fish_Same_Hedgehog:
			case BZJX_FISH_TYPE.Fish_Same_BlueAlgae:
			case BZJX_FISH_TYPE.Fish_Same_Lamp:
			case BZJX_FISH_TYPE.Fish_Same_Turtle:
				BZJX_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
				break;
			case BZJX_FISH_TYPE.Fish_SilverShark:
			case BZJX_FISH_TYPE.Fish_GoldenShark:
			case BZJX_FISH_TYPE.Fish_GoldenSharkB:
			case BZJX_FISH_TYPE.Fish_GoldenDragon:
			case BZJX_FISH_TYPE.Fish_Boss:
			case BZJX_FISH_TYPE.Fish_Penguin:
				BZJX_EffectMngr.GetSingleton().ShowEffTP(base.transform.position);
				break;
			case BZJX_FISH_TYPE.Fish_FixBomb:
				BZJX_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
				break;
			case BZJX_FISH_TYPE.Fish_PartBomb:
				BZJX_EffectMngr.GetSingleton().ShowEffPartBomb(base.transform.position);
				break;
			case BZJX_FISH_TYPE.Fish_SuperBomb:
				UnityEngine.Debug.Log("打中超级炸弹");
				BZJX_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
				break;
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		BZJX_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		BZJX_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
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
		GetComponent<BZJX_DoMove>().Reset();
		GetComponent<BZJX_DoMove>().enabled = false;
		FormationType = BZJX_FORMATION.Formation_Normal;
		BZJX_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		FormationType = BZJX_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<BZJX_DoMove>().Reset();
		GetComponent<BZJX_DoMove>().enabled = false;
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
		BZJX_Bullet component = other.transform.parent.GetComponent<BZJX_Bullet>();
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
				if (num < BZJX_GameInfo.getInstance().UserList.Count)
				{
					if (BZJX_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)BZJX_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			BZJX_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(BZJX_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (BZJX_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().User.SeatIndex == BZJX_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = BZJX_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < BZJX_GameInfo.getInstance().UserList.Count; j++)
		{
			if (BZJX_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)BZJX_GameInfo.getInstance().UserList[j].LockFish && (BZJX_GameInfo.getInstance().User.SeatIndex == seatid || BZJX_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				BZJX_LockFlag component = objLockFlag.GetComponent<BZJX_LockFlag>();
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
		return objLockFlag.transform.position;
	}

	public Vector3 GetLockFishLocalPos()
	{
		return objLockFlag.transform.localPosition;
	}

	public void HideLockedFlag()
	{
		objLockFlag.SetActive(value: false);
	}

	public bool IsLockFishOutsideWindow()
	{
		Vector3 position = base.transform.Find("Fish").position;
		float num = 6.3f;
		float num2 = 3.45f;
		return position.x < 0f - num || position.x > num || position.y < 0f - num2 || position.y > num2;
	}

	private void SetTag(BZJX_FISH_TYPE typ)
	{
		switch (typ)
		{
		case BZJX_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case BZJX_FISH_TYPE.Fish_SuperBomb:
			base.gameObject.tag = "SuperBomb";
			break;
		case BZJX_FISH_TYPE.Fish_FixBomb:
		case BZJX_FISH_TYPE.Fish_PartBomb:
			base.gameObject.tag = "StopBomb";
			break;
		case BZJX_FISH_TYPE.Fish_BigEars_Group:
		case BZJX_FISH_TYPE.Fish_YellowSpot_Group:
		case BZJX_FISH_TYPE.Fish_Hedgehog_Group:
		case BZJX_FISH_TYPE.Fish_Ugly_Group:
		case BZJX_FISH_TYPE.Fish_BlueAlgae_Group:
		case BZJX_FISH_TYPE.Fish_Turtle_Group:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
