using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public DK_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public DK_FISH_TYPE[] mFishTypes;
	}

	public static DK_FishPoolMngr G_FishPoolMngr;

	private List<GameObject> mFishList = new List<GameObject>();

	public DK_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	private FishCreatePara mPara = new FishCreatePara();

	public static DK_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[DK_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(DK_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (!GetSwimObjByTag(gameObject).IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie2", bulletPara);
				DK_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	public List<GameObject> GetAllFishArr()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			if (GetSwimObjByTag(mAllFishArr[i]).mFishType != DK_FISH_TYPE.Fish_NiuMoWang && GetSwimObjByTag(mAllFishArr[i]).mFishType != DK_FISH_TYPE.Fish_SuperBomb)
			{
				list.Add(mAllFishArr[i]);
			}
		}
		return list;
	}

	public void FixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<DK_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		DK_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<DK_DoMove>().Play();
		}
		DK_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing2(Vector3 fishingPos, DK_Bullet bullet, GameObject collisionFish)
	{
		int mPower = bullet.mPower;
		if (bullet.mIsLizi)
		{
			mPower *= 2;
		}
		List<GameObject> list = new List<GameObject>();
		bool bHaveKnife = false;
		list.Add(collisionFish);
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag = false;
			DK_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (!(swimObjByTag != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error tag1");
				return;
			}
			if (gameObject.CompareTag("ForCoralReefsDie"))
			{
				flag = true;
			}
			if (!flag && !swimObjByTag.bFishDead && collisionFish != gameObject && Vector3.Distance(gameObject.transform.position, fishingPos) < 1.2f)
			{
				if (list.Count >= 10)
				{
					break;
				}
				list.Add(gameObject);
			}
		}
		DK_HitFish[] array = null;
		if (collisionFish.GetComponent<DK_ISwimObj>().mFishType == DK_FISH_TYPE.Fish_SuperBomb)
		{
			bHaveKnife = true;
			int count = GetAllFishArr().Count;
			array = new DK_HitFish[count];
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject2 = GetAllFishArr()[j];
				DK_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject2);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				array[j] = new DK_HitFish();
				array[j].fishid = swimObjByTag2.GetFishSvrID();
				array[j].fishtype = (int)swimObjByTag2.mFishType;
				DK_HitFish obj = array[j];
				Vector3 position = gameObject2.transform.position;
				obj.fx = position.x;
				DK_HitFish obj2 = array[j];
				Vector3 position2 = gameObject2.transform.position;
				obj2.fy = position2.y;
			}
		}
		if (!((float)list.Count > 0f))
		{
			return;
		}
		int num = (list.Count < 10) ? list.Count : 10;
		DK_HitFish[] array2 = new DK_HitFish[num];
		for (int k = 0; k < num; k++)
		{
			GameObject gameObject3 = list[k];
			DK_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
			if (!(swimObjByTag3 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array2[k] = new DK_HitFish();
			array2[k].fishid = swimObjByTag3.GetFishSvrID();
			array2[k].fishtype = (int)swimObjByTag3.mFishType;
			DK_HitFish obj3 = array2[k];
			Vector3 position3 = gameObject3.transform.position;
			obj3.fx = position3.x;
			DK_HitFish obj4 = array2[k];
			Vector3 position4 = gameObject3.transform.position;
			obj4.fy = position4.y;
		}
		if (!DK_GameParameter.G_bTest)
		{
			DK_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, bHaveKnife, array);
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

	private void _updateFishIndex(DK_FISH_TYPE fishTyp)
	{
		if (fishTyp >= DK_FISH_TYPE.Fish_Shrimp && fishTyp <= DK_FISH_TYPE.Fish_NiuMoWang)
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
		else if (fishTyp >= DK_FISH_TYPE.Fish_Same_Shrimp && fishTyp <= DK_FISH_TYPE.Fish_Same_Turtle)
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
		else if (fishTyp >= DK_FISH_TYPE.Fish_FixBomb && fishTyp <= DK_FISH_TYPE.Fish_CoralReefs)
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
		else if (fishTyp >= DK_FISH_TYPE.Fish_BigEars_Group && fishTyp <= DK_FISH_TYPE.Fish_Double_Kill)
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

	public int GetFishIndexInLayer(DK_FISH_TYPE fishTyp)
	{
		return mFishIndexInLayer[(int)fishTyp];
	}

	public Transform CreateFishForBig(DK_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= DK_FISH_TYPE.Fish_TYPE_NONE || fishType < DK_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		return CreateFish(fishType, nSvrID);
	}

	public Transform CreateFishForCoralReefs(DK_FISH_TYPE fishType)
	{
		if (fishType >= DK_FISH_TYPE.Fish_TYPE_NONE || fishType < DK_FISH_TYPE.Fish_Shrimp)
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
		Transform transform = DK_PoolManager.Pools["DKFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(DK_FISH_TYPE fishType, int nSvrID, DK_FISH_TYPE[] fishTypes = null)
	{
		if (fishType >= DK_FISH_TYPE.Fish_TYPE_NONE || fishType < DK_FISH_TYPE.Fish_Shrimp)
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
		Transform transform = DK_PoolManager.Pools["DKFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.Find("Fish").gameObject.SetActive(value: true);
		if (fishType == DK_FISH_TYPE.Fish_Double_Kill)
		{
			Transform transform2 = DK_PoolManager.Pools["DKFishPool"].Spawn(fishPrefabs.objFish[(int)(fishTypes[0] + 30)].transform);
			Transform transform3 = DK_PoolManager.Pools["DKFishPool"].Spawn(fishPrefabs.objFish[(int)(fishTypes[1] + 30)].transform);
			transform2.parent = transform.Find("Fish");
			transform3.parent = transform.Find("Fish");
			Transform transform4 = transform.Find("FishRing");
			transform2.localPosition = transform4.GetChild(0).localPosition;
			transform3.localPosition = transform4.GetChild(1).localPosition;
			transform.SendMessage("SetSecondFishType", fishTypes[1]);
			transform.SendMessage("SetFirstFishType", fishTypes[0]);
			transform.GetComponent<DK_GroupFish>().InitDoubleFish();
		}
		transform.SendMessage("SetFishType", fishType);
		transform.SendMessage("SetFishSvrID", nSvrID);
		mAllFishArr.Add(transform.gameObject);
		if (!mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.Add(nSvrID, transform.gameObject);
			mFish_ID_Dictionary.Add(transform.gameObject, nSvrID);
		}
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateCoralReefsFish(DK_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(DK_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<DK_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(DK_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber, DK_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType <= 88 && nPathType >= 0 && fishType < DK_FISH_TYPE.Fish_TYPE_NONE && fishType >= DK_FISH_TYPE.Fish_Shrimp)
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
		if ((para.mFishType == DK_FISH_TYPE.Fish_Grass || para.mFishType == DK_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			DK_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				DK_FISH_TYPE mFishType5 = para.mFishType;
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
				DK_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID, para.mFishTypes);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(DK_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DK_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case DK_FISH_TYPE.Fish_Grass:
			return 1f;
		case DK_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case DK_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case DK_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case DK_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case DK_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case DK_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case DK_FISH_TYPE.Fish_Lamp:
			return 2f;
		case DK_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case DK_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case DK_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(DK_FISH_TYPE fishType, int nPathType, int nSvrID, DK_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= DK_FISH_TYPE.Fish_TYPE_NONE || fishType < DK_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, fishTypes);
		DK_PathManager fishPath = DK_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		DK_DoMove component = transform.GetComponent<DK_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<DK_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<DK_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(DK_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= DK_FISH_TYPE.Fish_TYPE_NONE || fishType < DK_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		DK_PathManager smallFishPath = DK_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		DK_DoMove component = transform.GetComponent<DK_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<DK_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<DK_ISwimObj>().SetUpDir(isR: false);
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
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				DK_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		try
		{
			DK_PoolManager.Pools["DKFishPool"].Despawn(fish.transform);
		}
		catch (Exception)
		{
		}
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		DK_BulletPara dK_BulletPara = new DK_BulletPara(nPlayerID, nPower);
		DK_ISwimObj dK_ISwimObj = null;
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			dK_ISwimObj = GetSwimObjByTag(value);
			dK_ISwimObj.HideLockedFlag();
			for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(DK_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				DK_Lock currentGun = DK_GameInfo.getInstance().GameScene.GetCurrentGun(DK_GameInfo.getInstance().UserList[i].SeatIndex);
				if (DK_GameInfo.getInstance().UserList[i].SeatIndex == DK_GameInfo.getInstance().User.SeatIndex && DK_GameInfo.getInstance().UserList[i].Lock)
				{
					DK_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					DK_GameInfo.getInstance().UserList[i].LockFish = null;
					if (DK_GameInfo.getInstance().GameScene.bAuto)
					{
						DK_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						DK_GameInfo.getInstance().UserList[i].Lock = false;
						DK_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (DK_GameInfo.getInstance().UserList[i].Lock)
				{
					DK_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(DK_GameInfo.getInstance().UserList[i].SeatIndex);
					DK_GameInfo.getInstance().UserList[i].LockFish = null;
					DK_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			if (value.CompareTag("CoralReefs"))
			{
				value.GetComponent<DK_CoralReefsFish>().mRealFishType = (DK_FISH_TYPE)UnityEngine.Random.Range(9, 16);
			}
			value.SendMessage("GoDie2", dK_BulletPara);
			if (dK_ISwimObj != null)
			{
				int num = nFishOODs * nPower;
				if ((float)num > 0f)
				{
					DK_EffectMngr.GetSingleton().ShowFishScore(nPlayerID, pos, num);
					DK_EffectMngr.GetSingleton().PlayCoinFly(nPlayerID, dK_ISwimObj.mFishType, base.gameObject.transform.localPosition, nFishOODs);
				}
			}
		}
		if (nFishType == 30)
		{
			AllFishDie(dK_BulletPara);
		}
		if (nFishType == 31)
		{
			FixAllFish();
		}
		if (nFishType <= 29 && nFishType >= 20)
		{
			SameFishDie((DK_FISH_TYPE)nFishType, dK_BulletPara);
		}
		if (nFishType == 39 && dK_ISwimObj != null)
		{
			SameFishDie(dK_ISwimObj.mFirstFishType + 20, dK_BulletPara);
			SameFishDie(dK_ISwimObj.mSecondFishType + 20, dK_BulletPara);
		}
		if (DK_EffectMngr.GetSingleton().IsBigFish((DK_FISH_TYPE)nFishType))
		{
			int num2 = nFishOODs * nPower;
			UnityEngine.Debug.LogError("鱼: " + nFishType.ToString() + " 倍率: " + nFishOODs + " 炮值: " + nPower + " 得分: " + num2);
			DK_EffectMngr.GetSingleton().ShowBigPrizePlate(dK_BulletPara.mPlyerIndex, num2);
			ZH2_GVars.niuMoWangBeiLv = 0;
		}
	}

	public void SameFishDie(DK_FISH_TYPE fishType, DK_BulletPara bulletPara)
	{
		DK_EffectMngr.GetSingleton().OverEffSimilarLightning();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			DK_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType - 20)
			{
				gameObject.SendMessage("GoDie2", bulletPara);
				DK_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		DK_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public DK_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		DK_ISwimObj result = null;
		if (fish.CompareTag("NormalFish"))
		{
			result = fish.GetComponent<DK_NormalFish>();
		}
		else if (fish.CompareTag("GroupFish"))
		{
			result = fish.GetComponent<DK_GroupFish>();
		}
		else if (fish.CompareTag("CoralReefs"))
		{
			result = fish.GetComponent<DK_CoralReefsFish>();
		}
		else if (fish.CompareTag("ForCoralReefsDie"))
		{
			result = fish.GetComponent<DK_NormalFish>();
		}
		else if (fish.CompareTag("StopBomb"))
		{
			result = fish.GetComponent<DK_NormalFish>();
		}
		else if (fish.CompareTag("SuperBomb"))
		{
			result = fish.GetComponent<DK_NormalFish>();
		}
		return result;
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == DK_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		DK_Lock currentGun = DK_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				DK_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (DK_GameInfo.getInstance().UserList[i].Lock && DK_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					DK_GameInfo.getInstance().UserList[i].LockFish = value;
					DK_ISwimObj swimObjByTag = GetSwimObjByTag(DK_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					DK_GameInfo.getInstance().UserList[i].LockFish = value;
					DK_ISwimObj swimObjByTag2 = GetSwimObjByTag(DK_GameInfo.getInstance().UserList[i].LockFish);
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
				DK_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == DK_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			DK_GameInfo.getInstance().UserList[i].Lock = false;
			DK_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)DK_GameInfo.getInstance().UserList[i].LockFish)
			{
				DK_Lock currentGun = DK_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				DK_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
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
				DK_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if ((bool)swimObjByTag && swimObjByTag.mFishType >= DK_FISH_TYPE.Fish_Turtle && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
				{
					list2.Add(gameObject);
				}
			}
		}
		for (int j = 0; j < DK_GameInfo.getInstance().UserList.Count; j++)
		{
			if (DK_GameInfo.getInstance().UserList[j].SeatIndex != seatid)
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
					DK_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject3);
					if ((bool)swimObjByTag2 && swimObjByTag2.mFishType >= DK_FISH_TYPE.Fish_Turtle && gameObject3 != DK_GameInfo.getInstance().UserList[j].LockFish && gameObject4.activeSelf && !swimObjByTag2.bFishDead && !swimObjByTag2.IsLockFishOutsideWindow())
					{
						DK_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
						int fishSvrID = swimObjByTag2.GetFishSvrID();
						DK_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
						DK_GameInfo.getInstance().UserList[j].LockFish = gameObject3;
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
}
