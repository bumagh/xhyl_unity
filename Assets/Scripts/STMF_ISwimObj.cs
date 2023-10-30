using GameCommon;
using System.Collections;
using UnityEngine;

public abstract class STMF_ISwimObj : STMF_FishForFormation
{
	protected int mServerID = -9999;

	public STMF_FISH_TYPE mFishType;

	public bool mIsFishDead;

	public BoxCollider2D mBoxCollider;

	public void SetFishType(STMF_FISH_TYPE typ)
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

	public STMF_FISH_TYPE GetFishType()
	{
		return mFishType;
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
		STMF_BulletPara sTMF_BulletPara = (STMF_BulletPara)obj;
		if (sTMF_BulletPara == null)
		{
			UnityEngine.Debug.Log("@ISwimObj GoDie Error : obj null!");
		}
		else
		{
			if (mIsFishDead)
			{
				return;
			}
			StartMove(move: false);
			mBoxCollider.enabled = false;
			mIsFishDead = true;
			GetComponent<STMF_HoMove>().Stop();
			if (obj != null && mFishType != STMF_FISH_TYPE.Fish_GoldenDragon)
			{
				STMF_EffectMngr.GetSingleton().PlayCoinFly(sTMF_BulletPara.mPlyerIndex, mFishType, base.transform.localPosition);
				int nScore = STMF_EffectMngr.GetSingleton().GetFishOODS(mFishType) * sTMF_BulletPara.mPower;
				STMF_EffectMngr.GetSingleton().ShowFishScore(sTMF_BulletPara.mPlyerIndex, base.transform.localPosition, nScore);
			}
			if (mFishType >= STMF_FISH_TYPE.Fish_DragonBeauty_Group && mFishType <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
			{
				STMF_EffectMngr.GetSingleton().PlayBigFishEffect(base.transform.position);
			}
			if (mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp && mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle)
			{
				STMF_EffectMngr.GetSingleton().ShowEffSimilarBomb(base.transform.position);
			}
			if (mFishType == STMF_FISH_TYPE.Fish_LimitedBomb)
			{
				STMF_EffectMngr.GetSingleton().PlayKnifeEffect(base.transform.position);
			}
			if (STMF_GameParameter.G_bTest)
			{
				if (mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle && mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp)
				{
					STMF_FishPoolMngr.GetSingleton().SameFishDie(mFishType, sTMF_BulletPara);
				}
				if (mFishType == STMF_FISH_TYPE.Fish_AllBomb)
				{
					STMF_FishPoolMngr.GetSingleton().AllFishDie(sTMF_BulletPara);
				}
			}
			StopCoroutine("IE_GoDie");
			StartCoroutine("IE_GoDie");
		}
	}

	private IEnumerator IE_GoDie()
	{
		DoDieAnim();
		STMF_MusicMngr.GetSingleton().PlayFishCaught(mFishType);
		STMF_MusicMngr.GetSingleton().PlayRandomWordsByFish(mFishType);
		yield return new WaitForSeconds(2f);
		ObjDestroy();
	}

	public void BeRemoved()
	{
		if (!mIsFishDead)
		{
			mIsFishDead = true;
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
		GetComponent<STMF_HoMove>().Stop();
		GetComponent<STMF_HoMove>().Reset();
		FormationType = STMF_FORMATION.Formation_Normal;
		STMF_FishPoolMngr.GetSingleton().DestroyFish(base.gameObject);
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
		mIsFishDead = false;
		DoSwimAnim();
	}

	public void OnDespawned()
	{
		_onDestroy();
		FormationType = STMF_FORMATION.Formation_Normal;
		StartMove(move: false);
		GetComponent<STMF_HoMove>().Stop();
		GetComponent<STMF_HoMove>().Reset();
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
		STMF_Bullet component = other.transform.parent.GetComponent<STMF_Bullet>();
		if (!mIsFishDead && !component.mIsDead)
		{
			component.ObjDestroy(base.gameObject);
			if (STMF_GameParameter.G_bTest && !mIsFishDead)
			{
				GoDie(new STMF_BulletPara(component.mPlayerID, component.mPower));
			}
		}
	}

	private void SetTag(STMF_FISH_TYPE typ)
	{
		switch (typ)
		{
		case STMF_FISH_TYPE.Fish_CoralReefs:
			base.gameObject.tag = "CoralReefs";
			break;
		case STMF_FISH_TYPE.Fish_AllBomb:
			base.gameObject.tag = "AllBomb";
			break;
		case STMF_FISH_TYPE.Fish_LimitedBomb:
			base.gameObject.tag = "LimitedBomb";
			break;
		case STMF_FISH_TYPE.Fish_DragonBeauty_Group:
		case STMF_FISH_TYPE.Fish_GoldenArrow_Group:
		case STMF_FISH_TYPE.Fish_Knife_Butterfly_Group:
			base.gameObject.tag = "GroupFish";
			break;
		default:
			base.gameObject.tag = "NormalFish";
			break;
		}
	}
}
