using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STQM_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public STQM_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public STQM_FISH_TYPE[] mFishTypes;
	}

	public static STQM_FishPoolMngr G_FishPoolMngr;

	public STQM_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	private FishCreatePara mPara = new FishCreatePara();

	[HideInInspector]
	public int totalScore;

	public static STQM_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[STQM_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(STQM_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (!GetSwimObjByTag(gameObject).IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie", bulletPara);
				STQM_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	public void FixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<STQM_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		STQM_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<STQM_DoMove>().Play();
		}
		STQM_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing(Vector3 fishingPos, STQM_Bullet bullet, GameObject collisionFish)
	{
		int num = bullet.mPower;
		if (bullet.mIsLizi)
		{
			num *= 2;
		}
		STQM_BulletPara sTQM_BulletPara = new STQM_BulletPara(bullet.mPlayerID, num);
		List<GameObject> list = new List<GameObject>();
		bool flag = false;
		Vector3 zero = Vector3.zero;
		list.Add(collisionFish);
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag2 = false;
			STQM_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (!(swimObjByTag != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error tag1");
				return;
			}
			if (gameObject.CompareTag("ForCoralReefsDie"))
			{
				flag2 = true;
			}
			if (!flag2 && !swimObjByTag.bFishDead && collisionFish != gameObject && Vector3.Distance(gameObject.transform.position, fishingPos) < 1.2f)
			{
				if (list.Count >= 10)
				{
					break;
				}
				list.Add(gameObject);
			}
		}
		if (flag)
		{
			for (int j = 0; j < mAllFishArr.Count; j++)
			{
				GameObject fish = mAllFishArr[j];
				STQM_ISwimObj swimObjByTag2 = GetSwimObjByTag(fish);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error isKnifeIn tag1");
					return;
				}
				int mFishType = (int)swimObjByTag2.mFishType;
			}
		}
		STQM_HitFish[] hitfish = null;
		if (!((float)list.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID = STQM_GameMngr.GetSingleton().mPlayerSeatID;
		int num2 = (list.Count < 10) ? list.Count : 10;
		STQM_HitFish[] array = new STQM_HitFish[num2];
		for (int k = 0; k < num2; k++)
		{
			GameObject gameObject2 = list[k];
			STQM_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject2);
			if (!(swimObjByTag3 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array[k] = new STQM_HitFish();
			array[k].fishid = swimObjByTag3.GetFishSvrID();
			array[k].fishtype = (int)swimObjByTag3.mFishType;
			STQM_HitFish obj = array[k];
			Vector3 position = gameObject2.transform.position;
			obj.fx = position.x;
			STQM_HitFish obj2 = array[k];
			Vector3 position2 = gameObject2.transform.position;
			obj2.fy = position2.y;
		}
		if (!STQM_GameParameter.G_bTest)
		{
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array, flag, hitfish);
		}
	}

	public void RemoveAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.SendMessage("BeRemoved");
		}
		StopCoroutine("IE_CreateFish");
	}

	private void _updateFishIndex(STQM_FISH_TYPE fishTyp)
	{
		if (fishTyp >= STQM_FISH_TYPE.Fish_Shrimp && fishTyp <= STQM_FISH_TYPE.Fish_BuleWhale)
		{
			if (mFishIndexInLayer[(int)fishTyp] < 8)
			{
				mFishIndexInLayer[(int)fishTyp]++;
			}
			else
			{
				mFishIndexInLayer[(int)fishTyp] = 0;
			}
		}
		else if (fishTyp >= STQM_FISH_TYPE.Fish_Same_Shrimp && fishTyp <= STQM_FISH_TYPE.Fish_Same_Turtle)
		{
			if (mFishIndexInLayer[(int)fishTyp] < 24)
			{
				mFishIndexInLayer[(int)fishTyp] += 3;
			}
			else
			{
				mFishIndexInLayer[(int)fishTyp] = 0;
			}
		}
		else if (fishTyp >= STQM_FISH_TYPE.Fish_AllBomb && fishTyp < STQM_FISH_TYPE.Fish_BowlFish)
		{
			if (mFishIndexInLayer[(int)fishTyp] < 8)
			{
				mFishIndexInLayer[(int)fishTyp]++;
			}
			else
			{
				mFishIndexInLayer[(int)fishTyp] = 0;
			}
		}
		else if (fishTyp == STQM_FISH_TYPE.Fish_BowlFish)
		{
			if (mFishIndexInLayer[(int)fishTyp] < 32)
			{
				mFishIndexInLayer[(int)fishTyp] += 4;
			}
			else
			{
				mFishIndexInLayer[(int)fishTyp] = 0;
			}
		}
	}

	public int GetFishIndexInLayer(STQM_FISH_TYPE fishTyp)
	{
		return mFishIndexInLayer[(int)fishTyp];
	}

	public Transform CreateFishForBig(STQM_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= STQM_FISH_TYPE.Fish_TYPE_NONE || fishType < STQM_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		return CreateFish(fishType, nSvrID);
	}

	public Transform CreateFishForCoralReefs(STQM_FISH_TYPE fishType)
	{
		if (fishType >= STQM_FISH_TYPE.Fish_TYPE_NONE || fishType < STQM_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		if (num <= 29 && num >= 20)
		{
			num -= 20;
		}
		else if (num > 29)
		{
			num -= 10;
		}
		Transform transform = STQM_PoolManager.Pools["QMFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(STQM_FISH_TYPE fishType, int nSvrID, STQM_FISH_TYPE[] fishTypes = null)
	{
		if (fishType >= STQM_FISH_TYPE.Fish_TYPE_NONE || fishType < STQM_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		if (num <= 29 && num >= 20)
		{
			num -= 20;
		}
		else if (num > 29)
		{
			num -= 10;
		}
		Transform transform = STQM_PoolManager.Pools["QMFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.Find("Fish").gameObject.SetActive(value: true);
		transform.SendMessage("SetFishType", fishType);
		transform.SendMessage("SetFishSvrID", nSvrID);
		mAllFishArr.Add(transform.gameObject);
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			UnityEngine.Debug.Log("@ttttttttttttttttttttttttttttttttttttttttttttttttttFishPoolMngr ServerID already exist with value :" + nSvrID + "  nfishtype=" + fishType);
		}
		else
		{
			mID_Fish_Dictionary.Add(nSvrID, transform.gameObject);
			mFish_ID_Dictionary.Add(transform.gameObject, nSvrID);
		}
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateCoralReefsFish(STQM_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(STQM_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<STQM_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(STQM_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber, STQM_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType <= 88 && nPathType >= 0 && fishType < STQM_FISH_TYPE.Fish_TYPE_NONE && fishType >= STQM_FISH_TYPE.Fish_Shrimp)
		{
			StartCoroutine("IE_CreateFish", new FishCreatePara
			{
				mFishNumber = nFishNumber,
				mFishPathType = nPathType,
				mSvrID = nSvrIDBegin,
				mFishType = fishType,
				mFishTypes = fishTypes
			});
		}
	}

	private IEnumerator IE_CreateFish(FishCreatePara para)
	{
		int svrIDBegin = para.mSvrID;
		if ((para.mFishType == STQM_FISH_TYPE.Fish_Grass || para.mFishType == STQM_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			STQM_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				STQM_FISH_TYPE mFishType5 = para.mFishType;
				int nPathType = para.mFishPathType * 4 + j - 1;
				svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
				CreateSmall_5_Fish(mFishType5, nPathType, nSvrID2);
			}
		}
		else
		{
			mPara = para;
			for (int i = 0; i < para.mFishNumber; i++)
			{
				STQM_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID, para.mFishTypes);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(STQM_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STQM_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case STQM_FISH_TYPE.Fish_Grass:
			return 1f;
		case STQM_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case STQM_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case STQM_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case STQM_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case STQM_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case STQM_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case STQM_FISH_TYPE.Fish_Lamp:
			return 2f;
		case STQM_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case STQM_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case STQM_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(STQM_FISH_TYPE fishType, int nPathType, int nSvrID, STQM_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STQM_FISH_TYPE.Fish_TYPE_NONE || fishType < STQM_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, fishTypes);
		STQM_PathManager fishPath = STQM_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		STQM_DoMove component = transform.GetComponent<STQM_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STQM_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STQM_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(STQM_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STQM_FISH_TYPE.Fish_TYPE_NONE || fishType < STQM_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		STQM_PathManager smallFishPath = STQM_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		STQM_DoMove component = transform.GetComponent<STQM_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STQM_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STQM_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public void DestroyFish(GameObject fish)
	{
		mAllFishArr.Remove(fish);
		int value = 0;
		if (mFish_ID_Dictionary.ContainsKey(fish))
		{
			mFish_ID_Dictionary.TryGetValue(fish, out value);
			mFish_ID_Dictionary.Remove(fish);
			mID_Fish_Dictionary.Remove(value);
		}
		GetSwimObjByTag(fish).bLocked = false;
		fish.transform.Find("LockFlag").gameObject.SetActive(value: false);
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				STQM_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		STQM_PoolManager.Pools["QMFishPool"].Despawn(fish.transform);
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		totalScore = 0;
		STQM_BulletPara sTQM_BulletPara = new STQM_BulletPara(nPlayerID, nPower);
		STQM_ISwimObj sTQM_ISwimObj = null;
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			sTQM_ISwimObj = GetSwimObjByTag(value);
			sTQM_ISwimObj.HideLockedFlag();
			for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(STQM_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				STQM_Lock currentGun = STQM_GameInfo.getInstance().GameScene.GetCurrentGun(STQM_GameInfo.getInstance().UserList[i].SeatIndex);
				if (STQM_GameInfo.getInstance().UserList[i].SeatIndex == STQM_GameInfo.getInstance().User.SeatIndex && STQM_GameInfo.getInstance().UserList[i].Lock)
				{
					STQM_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					STQM_GameInfo.getInstance().UserList[i].LockFish = null;
					if (STQM_GameInfo.getInstance().GameScene.bAuto)
					{
						STQM_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						STQM_GameInfo.getInstance().UserList[i].Lock = false;
						STQM_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (STQM_GameInfo.getInstance().UserList[i].Lock)
				{
					STQM_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(STQM_GameInfo.getInstance().UserList[i].SeatIndex);
					STQM_GameInfo.getInstance().UserList[i].LockFish = null;
					STQM_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			if (value.CompareTag("CoralReefs"))
			{
				value.GetComponent<STQM_CoralReefsFish>().mRealFishType = (STQM_FISH_TYPE)UnityEngine.Random.Range(9, 16);
			}
			value.SendMessage("GoDie", sTQM_BulletPara);
			if (sTQM_ISwimObj.mFishType == STQM_FISH_TYPE.Fish_BuleWhale)
			{
				int num = nFishOODs * nPower;
				STQM_EffectMngr.GetSingleton().ShowFishScore(sTQM_BulletPara.mPlyerIndex, base.transform.position, num);
				totalScore += num;
			}
		}
		if (nFishType == 31)
		{
			AllFishDie(sTQM_BulletPara);
		}
		if (nFishType <= 29 && nFishType >= 20)
		{
			SameFishDie((STQM_FISH_TYPE)nFishType, sTQM_BulletPara);
		}
		if (STQM_EffectMngr.GetSingleton().IsBigFish((STQM_FISH_TYPE)nFishType))
		{
			STQM_EffectMngr.GetSingleton().ShowBigPrizePlate(sTQM_BulletPara.mPlyerIndex, totalScore);
		}
	}

	public void SameFishDie(STQM_FISH_TYPE fishType, STQM_BulletPara bulletPara)
	{
		STQM_EffectMngr.GetSingleton().OverEffSimilarLightning();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			STQM_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType - 20)
			{
				gameObject.SendMessage("GoDie", bulletPara);
				STQM_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		STQM_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public STQM_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		STQM_ISwimObj result = null;
		if (fish.CompareTag("NormalFish"))
		{
			result = fish.GetComponent<STQM_NormalFish>();
		}
		else if (fish.CompareTag("GroupFish"))
		{
			result = fish.GetComponent<STQM_GroupFish>();
		}
		else if (fish.CompareTag("CoralReefs"))
		{
			result = fish.GetComponent<STQM_CoralReefsFish>();
		}
		else if (fish.CompareTag("ForCoralReefsDie"))
		{
			result = fish.GetComponent<STQM_NormalFish>();
		}
		else if (fish.CompareTag("SuperBomb"))
		{
			result = fish.GetComponent<STQM_NormalFish>();
		}
		return result;
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == STQM_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		STQM_Lock currentGun = STQM_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && STQM_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				STQM_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (STQM_GameInfo.getInstance().UserList[i].Lock && STQM_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					STQM_GameInfo.getInstance().UserList[i].LockFish = value;
					STQM_ISwimObj swimObjByTag = GetSwimObjByTag(STQM_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					STQM_GameInfo.getInstance().UserList[i].LockFish = value;
					STQM_ISwimObj swimObjByTag2 = GetSwimObjByTag(STQM_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag2.bLocked = true;
					if ((bool)currentGun)
					{
						currentGun.StartLocking(seatid);
					}
					else
					{
						UnityEngine.Debug.Log("gunobj is not exit at LockFish" + seatid);
					}
				}
				STQM_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == STQM_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			STQM_GameInfo.getInstance().UserList[i].Lock = false;
			STQM_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)STQM_GameInfo.getInstance().UserList[i].LockFish)
			{
				STQM_Lock currentGun = STQM_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				STQM_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!STQM_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			return -1;
		}
		List<GameObject> list = mAllFishArr;
		List<GameObject> list2 = new List<GameObject>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			GameObject gameObject2 = gameObject.transform.Find("Fish").gameObject;
			if ((bool)gameObject && gameObject.activeSelf)
			{
				STQM_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if ((bool)swimObjByTag && swimObjByTag.mFishType >= STQM_FISH_TYPE.Fish_Turtle && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
				{
					list2.Add(gameObject);
				}
			}
		}
		for (int j = 0; j < STQM_GameInfo.getInstance().UserList.Count; j++)
		{
			if (STQM_GameInfo.getInstance().UserList[j].SeatIndex != seatid)
			{
				continue;
			}
			if (list2.Count != 0)
			{
				int index = 0;
				if (list2.Count != 1)
				{
					index = UnityEngine.Random.Range(0, list2.Count - 1);
				}
				GameObject gameObject3 = list2[index];
				GameObject gameObject4 = gameObject3.transform.Find("Fish").gameObject;
				if ((bool)gameObject3 && gameObject3.activeSelf)
				{
					STQM_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject3);
					if ((bool)swimObjByTag2 && swimObjByTag2.mFishType >= STQM_FISH_TYPE.Fish_Turtle && gameObject3 != STQM_GameInfo.getInstance().UserList[j].LockFish && gameObject4.activeSelf && !swimObjByTag2.bFishDead && !swimObjByTag2.IsLockFishOutsideWindow())
					{
						STQM_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
						int fishSvrID = swimObjByTag2.GetFishSvrID();
						STQM_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
						STQM_GameInfo.getInstance().UserList[j].LockFish = gameObject3;
						swimObjByTag2.bLocked = true;
						return fishSvrID;
					}
				}
				return -1;
			}
			return -1;
		}
		return -1;
	}

	public void Clear()
	{
		RemoveAllFish();
		mAllFishArr.Clear();
		mID_Fish_Dictionary.Clear();
		mFish_ID_Dictionary.Clear();
		mPara = null;
		G_FishPoolMngr = null;
	}
}
