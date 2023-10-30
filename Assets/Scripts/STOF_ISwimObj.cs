using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class STOF_ISwimObj : STOF_FishForFormation
{
	public GameObject objLockFlag;

	[HideInInspector]
	public bool bLocked;

	[HideInInspector]
	public int layer;

	[HideInInspector]
	public int mClickOneSID;

	public int mServerID = -9999;

	public STOF_FISH_TYPE mFishType;

	[HideInInspector]
	public bool bFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(STOF_FISH_TYPE typ)
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
		STOF_BulletPara sTOF_BulletPara = (STOF_BulletPara)obj;
		if (sTOF_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else if (!bFishDead)
		{
			StartMove(move: false);
			bFishDead = true;
			GetComponent<STOF_DoMove>().Over();
			GetComponent<STOF_DoMove>().enabled = false;
			if (mFishType != STOF_FISH_TYPE.Fish_GoldenDragon && mFishType != STOF_FISH_TYPE.Fish_GoldenSharkB && mFishType != STOF_FISH_TYPE.Fish_Boss && mFishType != STOF_FISH_TYPE.Fish_SuperBomb && mFishType != STOF_FISH_TYPE.Fish_PartBomb)
			{
				int num = STOF_EffectMngr.GetSingleton().GetFishOODS(mFishType) * sTOF_BulletPara.mPower;
				STOF_EffectMngr.GetSingleton().ShowFishScore(sTOF_BulletPara.mPlyerIndex, base.transform.position, num);
				STOF_FishPoolMngr.GetSingleton().totalScore += num;
			}
			STOF_EffectMngr.GetSingleton().PlayCoinFly(sTOF_BulletPara.mPlyerIndex, mFishType, base.transform.position);
			switch (mFishType)
			{
			case STOF_FISH_TYPE.Fish_Same_Shrimp:
			case STOF_FISH_TYPE.Fish_Same_Grass:
			case STOF_FISH_TYPE.Fish_Same_Zebra:
			case STOF_FISH_TYPE.Fish_Same_BigEars:
			case STOF_FISH_TYPE.Fish_Same_YellowSpot:
			case STOF_FISH_TYPE.Fish_Same_Ugly:
			case STOF_FISH_TYPE.Fish_Same_Hedgehog:
			case STOF_FISH_TYPE.Fish_Same_BlueAlgae:
			case STOF_FISH_TYPE.Fish_Same_Lamp:
			case STOF_FISH_TYPE.Fish_Same_Turtle:
				STOF_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
				break;
			case STOF_FISH_TYPE.Fish_SilverShark:
			case STOF_FISH_TYPE.Fish_GoldenShark:
			case STOF_FISH_TYPE.Fish_GoldenSharkB:
			case STOF_FISH_TYPE.Fish_GoldenDragon:
			case STOF_FISH_TYPE.Fish_Boss:
			case STOF_FISH_TYPE.Fish_Penguin:
				STOF_EffectMngr.GetSingleton().ShowEffTP(base.transform.position);
				break;
			case STOF_FISH_TYPE.Fish_FixBomb:
				STOF_EffectMngr.GetSingleton().ShowEffTimeStop(base.transform.position);
				break;
			case STOF_FISH_TYPE.Fish_PartBomb:
				STOF_EffectMngr.GetSingleton().ShowEffPartBomb(base.transform.position);
				break;
			case STOF_FISH_TYPE.Fish_SuperBomb:
				UnityEngine.Debug.Log("打中超级炸弹");
				STOF_EffectMngr.GetSingleton().ShowEffSuperBomb(base.transform.position);
				break;
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		STOF_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		STOF_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
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
		GetComponent<STOF_DoMove>().Reset();
		GetComponent<STOF_DoMove>().enabled = false;
		FormationType = STOF_FORMATION.Formation_Normal;
		STOF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		FormationType = STOF_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<STOF_DoMove>().Reset();
		GetComponent<STOF_DoMove>().enabled = false;
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
		STOF_Bullet component = other.transform.parent.GetComponent<STOF_Bullet>();
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
				if (num < STOF_GameInfo.getInstance().UserList.Count)
				{
					if (STOF_GameInfo.getInstance().UserList[num].SeatIndex == component.mPlayerID && (bool)STOF_GameInfo.getInstance().UserList[num].LockFish)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			STOF_FishPoolMngr.GetSingleton().mFish_ID_Dictionary.TryGetValue(STOF_GameInfo.getInstance().UserList[num].LockFish, out value);
			if (STOF_GameInfo.getInstance().UserList[num].LockFish == base.gameObject || mServerID == value)
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
		for (int i = 0; i < STOF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STOF_GameInfo.getInstance().User.SeatIndex == STOF_GameInfo.getInstance().UserList[i].SeatIndex)
			{
				y = STOF_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		for (int j = 0; j < STOF_GameInfo.getInstance().UserList.Count; j++)
		{
			if (STOF_GameInfo.getInstance().UserList[j].SeatIndex == seatid && (bool)STOF_GameInfo.getInstance().UserList[j].LockFish && (STOF_GameInfo.getInstance().User.SeatIndex == seatid || STOF_GameInfo.getInstance().UserList[j].LockFish != y))
			{
				STOF_LockFlag component = objLockFlag.GetComponent<STOF_LockFlag>();
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

	private void SetTag(STOF_FISH_TYPE typ)
	{
		switch (typ)
		{
		case STOF_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case STOF_FISH_TYPE.Fish_SuperBomb:
			base.gameObject.tag = "SuperBomb";
			break;
		case STOF_FISH_TYPE.Fish_FixBomb:
		case STOF_FISH_TYPE.Fish_PartBomb:
			base.gameObject.tag = "StopBomb";
			break;
		case STOF_FISH_TYPE.Fish_BigEars_Group:
		case STOF_FISH_TYPE.Fish_YellowSpot_Group:
		case STOF_FISH_TYPE.Fish_Hedgehog_Group:
		case STOF_FISH_TYPE.Fish_Ugly_Group:
		case STOF_FISH_TYPE.Fish_BlueAlgae_Group:
		case STOF_FISH_TYPE.Fish_Turtle_Group:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
