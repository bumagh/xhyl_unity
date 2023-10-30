using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STTF_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public STTF_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public STTF_FISH_TYPE[] mFishTypes;
	}

	public static STTF_FishPoolMngr G_FishPoolMngr;

	private List<GameObject> mFishList = new List<GameObject>();

	public STTF_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	private FishCreatePara mPara = new FishCreatePara();

	[HideInInspector]
	public int totalScore;

	public static STTF_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[STTF_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(STTF_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (!GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != STTF_FISH_TYPE.Fish_SuperBomb)
			{
				gameObject.SendMessage("GoDie", bulletPara);
				STTF_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	public List<GameObject> GetAllFishArr()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null && !GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != STTF_FISH_TYPE.Fish_SuperBomb)
			{
				list.Add(gameObject);
			}
		}
		return list;
	}

	public void FixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<STTF_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		STTF_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<STTF_DoMove>().Play();
		}
		STTF_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing2(Vector3 fishingPos, STTF_Bullet bullet, GameObject collisionFish)
	{
		List<STTF_ISwimObj> list = new List<STTF_ISwimObj>();
		int mPower = bullet.mPower;
		if (bullet.mIsLizi)
		{
			mPower *= 2;
		}
		List<GameObject> list2 = new List<GameObject>();
		bool bHaveKnife = false;
		Vector3 zero = Vector3.zero;
		list2.Add(collisionFish);
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag = false;
			STTF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
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
				if (list2.Count >= 10)
				{
					break;
				}
				list2.Add(gameObject);
			}
		}
		if (collisionFish.GetComponent<STTF_ISwimObj>().mFishType == STTF_FISH_TYPE.Fish_SuperBomb)
		{
			bHaveKnife = true;
		}
		STTF_HitFish[] array = new STTF_HitFish[0];
		if (collisionFish.GetComponent<STTF_ISwimObj>().mFishType == STTF_FISH_TYPE.Fish_SuperBomb)
		{
			int count = GetAllFishArr().Count;
			array = new STTF_HitFish[count];
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject2 = GetAllFishArr()[j];
				STTF_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject2);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				array[j] = new STTF_HitFish();
				array[j].fishid = swimObjByTag2.GetFishSvrID();
				array[j].fishtype = (int)swimObjByTag2.mFishType;
				STTF_HitFish obj = array[j];
				Vector3 position = gameObject2.transform.position;
				obj.fx = position.x;
				STTF_HitFish obj2 = array[j];
				Vector3 position2 = gameObject2.transform.position;
				obj2.fy = position2.y;
			}
			UnityEngine.Debug.LogError("==========超级炸弹========" + array.Length);
		}
		if (!((float)list2.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID = STTF_GameMngr.GetSingleton().mPlayerSeatID;
		int num = (list2.Count < 10) ? list2.Count : 10;
		STTF_HitFish[] array2 = new STTF_HitFish[num];
		for (int k = 0; k < num; k++)
		{
			GameObject gameObject3 = list2[k];
			STTF_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
			if (!(swimObjByTag3 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array2[k] = new STTF_HitFish();
			array2[k].fishid = swimObjByTag3.GetFishSvrID();
			array2[k].fishtype = (int)swimObjByTag3.mFishType;
			STTF_HitFish obj3 = array2[k];
			Vector3 position3 = gameObject3.transform.position;
			obj3.fx = position3.x;
			STTF_HitFish obj4 = array2[k];
			Vector3 position4 = gameObject3.transform.position;
			obj4.fy = position4.y;
		}
		STTF_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, bHaveKnife, array);
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

	private void _updateFishIndex(STTF_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num <= 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		if (fishTyp >= STTF_FISH_TYPE.Fish_Shrimp && fishTyp <= STTF_FISH_TYPE.Fish_Dragon)
		{
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
		}
		else if (fishTyp >= STTF_FISH_TYPE.Fish_Same_Shrimp && fishTyp <= STTF_FISH_TYPE.Fish_Same_Turtle)
		{
			if (mFishIndexInLayer[num] < 24)
			{
				mFishIndexInLayer[num] += 3;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
		}
		else if (fishTyp >= STTF_FISH_TYPE.Fish_FixBomb && fishTyp <= STTF_FISH_TYPE.Fish_CoralReefs)
		{
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
		}
		else if (fishTyp >= STTF_FISH_TYPE.Fish_BigEars_Group && fishTyp <= STTF_FISH_TYPE.Fish_Turtle_Group)
		{
			if (mFishIndexInLayer[num] < 32)
			{
				mFishIndexInLayer[num] += 4;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
		}
	}

	public int GetFishIndexInLayer(STTF_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num <= 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		return mFishIndexInLayer[num];
	}

	public Transform CreateFishForBig(STTF_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= STTF_FISH_TYPE.Fish_TYPE_NONE || fishType < STTF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		return CreateFish(fishType, nSvrID);
	}

	public Transform CreateFishForCoralReefs(STTF_FISH_TYPE fishType)
	{
		if (fishType >= STTF_FISH_TYPE.Fish_TYPE_NONE || fishType < STTF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		switch (num)
		{
		case 31:
			num = 20;
			break;
		case 30:
			num = 21;
			break;
		case 32:
			num = 22;
			break;
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			num -= 20;
			break;
		default:
			if (num > 32)
			{
				num -= 10;
			}
			break;
		}
		Transform transform = TF_PoolManager.Pools["TFFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(STTF_FISH_TYPE fishType, int nSvrID, STTF_FISH_TYPE[] fishTypes = null)
	{
		if (fishType >= STTF_FISH_TYPE.Fish_TYPE_NONE || fishType < STTF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		switch (num)
		{
		case 31:
			num = 20;
			break;
		case 30:
			num = 21;
			break;
		case 32:
			num = 22;
			break;
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			num -= 20;
			break;
		default:
			if (num > 32)
			{
				num -= 10;
			}
			break;
		}
		Transform result = null;
		if (num >= 0 && num < fishPrefabs.objFish.Length)
		{
			result = TF_PoolManager.Pools["TFFishPool"].Spawn(fishPrefabs.objFish[num].transform);
			result.Rotate(Vector3.up, 90f, Space.World);
			result.Find("Fish").gameObject.SetActive(value: true);
			result.SendMessage("SetFishType", fishType);
			result.SendMessage("SetFishSvrID", nSvrID);
			mAllFishArr.Add(result.gameObject);
			if (!mID_Fish_Dictionary.ContainsKey(nSvrID))
			{
				mID_Fish_Dictionary.Add(nSvrID, result.gameObject);
				mFish_ID_Dictionary.Add(result.gameObject, nSvrID);
			}
			_updateFishIndex(fishType);
			return result;
		}
		UnityEngine.Debug.LogError(fishType + "------" + (int)fishType + "======" + num);
		return result;
	}

	public Transform CreateCoralReefsFish(STTF_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(STTF_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<STTF_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(STTF_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber, STTF_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType <= 88 && nPathType >= 0 && fishType < STTF_FISH_TYPE.Fish_TYPE_NONE && fishType >= STTF_FISH_TYPE.Fish_Shrimp)
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
		if ((para.mFishType == STTF_FISH_TYPE.Fish_Grass || para.mFishType == STTF_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			STTF_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				STTF_FISH_TYPE mFishType5 = para.mFishType;
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
				STTF_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID, para.mFishTypes);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(STTF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STTF_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case STTF_FISH_TYPE.Fish_Grass:
			return 1f;
		case STTF_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case STTF_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case STTF_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case STTF_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case STTF_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case STTF_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case STTF_FISH_TYPE.Fish_Lamp:
			return 2f;
		case STTF_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case STTF_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case STTF_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(STTF_FISH_TYPE fishType, int nPathType, int nSvrID, STTF_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STTF_FISH_TYPE.Fish_TYPE_NONE || fishType < STTF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, fishTypes);
		if (transform == null)
		{
			UnityEngine.Debug.LogError("===没有这只鱼===");
			return null;
		}
		STTF_PathManager fishPath = STTF_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		STTF_DoMove component = transform.GetComponent<STTF_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STTF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STTF_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(STTF_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STTF_FISH_TYPE.Fish_TYPE_NONE || fishType < STTF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		STTF_PathManager smallFishPath = STTF_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		STTF_DoMove component = transform.GetComponent<STTF_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STTF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STTF_ISwimObj>().SetUpDir(isR: false);
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
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				STTF_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		TF_PoolManager.Pools["TFFishPool"].Despawn(fish.transform);
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		totalScore = 0;
		STTF_BulletPara sTTF_BulletPara = new STTF_BulletPara(nPlayerID, nPower);
		STTF_ISwimObj sTTF_ISwimObj = null;
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			sTTF_ISwimObj = GetSwimObjByTag(value);
			sTTF_ISwimObj.HideLockedFlag();
			for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(STTF_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				STTF_Lock currentGun = STTF_GameInfo.getInstance().GameScene.GetCurrentGun(STTF_GameInfo.getInstance().UserList[i].SeatIndex);
				if (STTF_GameInfo.getInstance().UserList[i].SeatIndex == STTF_GameInfo.getInstance().User.SeatIndex && STTF_GameInfo.getInstance().UserList[i].Lock)
				{
					STTF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					STTF_GameInfo.getInstance().UserList[i].LockFish = null;
					if (STTF_GameInfo.getInstance().GameScene.bAuto)
					{
						STTF_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						STTF_GameInfo.getInstance().UserList[i].Lock = false;
						STTF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (STTF_GameInfo.getInstance().UserList[i].Lock)
				{
					STTF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(STTF_GameInfo.getInstance().UserList[i].SeatIndex);
					STTF_GameInfo.getInstance().UserList[i].LockFish = null;
					STTF_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			if (value.CompareTag("CoralReefs"))
			{
				value.GetComponent<STTF_CoralReefsFish>().mRealFishType = (STTF_FISH_TYPE)UnityEngine.Random.Range(9, 16);
			}
			value.SendMessage("GoDie", sTTF_BulletPara);
			if (sTTF_ISwimObj.mFishType == STTF_FISH_TYPE.Fish_BigShark || sTTF_ISwimObj.mFishType == STTF_FISH_TYPE.Fish_Toad || sTTF_ISwimObj.mFishType == STTF_FISH_TYPE.Fish_Dragon || sTTF_ISwimObj.mFishType == STTF_FISH_TYPE.Fish_SuperBomb)
			{
				int num = nFishOODs * nPower;
				UnityEngine.Debug.LogError("鱼: " + sTTF_ISwimObj.mFishType + "  变倍率: " + nFishOODs);
				STTF_EffectMngr.GetSingleton().ShowFishScore(sTTF_BulletPara.mPlyerIndex, base.transform.position, num);
				totalScore += num;
			}
		}
		if (nFishType == 30)
		{
			sTTF_BulletPara.mPower = 0;
			AllFishDie(sTTF_BulletPara);
		}
		if (nFishType == 31)
		{
			FixAllFish();
		}
		switch (nFishType)
		{
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			SameFishDie((STTF_FISH_TYPE)nFishType, sTTF_BulletPara);
			break;
		}
		if (STTF_EffectMngr.GetSingleton().IsBigFish((STTF_FISH_TYPE)nFishType))
		{
			STTF_EffectMngr.GetSingleton().ShowBigPrizePlate(sTTF_BulletPara.mPlyerIndex, totalScore);
		}
	}

	public void SameFishDie(STTF_FISH_TYPE fishType, STTF_BulletPara bulletPara)
	{
		STTF_EffectMngr.GetSingleton().OverEffSimilarLightning();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			STTF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType - 20 && !swimObjByTag.IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie", bulletPara);
				STTF_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		STTF_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public STTF_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		STTF_ISwimObj result = null;
		if (fish.CompareTag("NormalFish"))
		{
			result = fish.GetComponent<STTF_NormalFish>();
		}
		else if (fish.CompareTag("GroupFish"))
		{
			result = fish.GetComponent<STTF_GroupFish>();
		}
		else if (fish.CompareTag("CoralReefs"))
		{
			result = fish.GetComponent<STTF_CoralReefsFish>();
		}
		else if (fish.CompareTag("ForCoralReefsDie"))
		{
			result = fish.GetComponent<STTF_NormalFish>();
		}
		else if (fish.CompareTag("StopBomb"))
		{
			result = fish.GetComponent<STTF_NormalFish>();
		}
		else if (fish.CompareTag("SuperBomb"))
		{
			result = fish.GetComponent<STTF_NormalFish>();
		}
		return result;
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == STTF_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		STTF_Lock currentGun = STTF_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				STTF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (STTF_GameInfo.getInstance().UserList[i].Lock && STTF_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					STTF_GameInfo.getInstance().UserList[i].LockFish = value;
					STTF_ISwimObj swimObjByTag = GetSwimObjByTag(STTF_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					STTF_GameInfo.getInstance().UserList[i].LockFish = value;
					STTF_ISwimObj swimObjByTag2 = GetSwimObjByTag(STTF_GameInfo.getInstance().UserList[i].LockFish);
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
				STTF_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == STTF_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			STTF_GameInfo.getInstance().UserList[i].Lock = false;
			STTF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)STTF_GameInfo.getInstance().UserList[i].LockFish)
			{
				STTF_Lock currentGun = STTF_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				STTF_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
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
				STTF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if ((bool)swimObjByTag && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
				{
					list2.Add(gameObject);
				}
			}
		}
		for (int j = 0; j < STTF_GameInfo.getInstance().UserList.Count; j++)
		{
			if (STTF_GameInfo.getInstance().UserList[j].SeatIndex != seatid)
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
					STTF_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject3);
					if ((bool)swimObjByTag2 && gameObject3 != STTF_GameInfo.getInstance().UserList[j].LockFish && gameObject4.activeSelf && !swimObjByTag2.bFishDead && !swimObjByTag2.IsLockFishOutsideWindow())
					{
						STTF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
						int fishSvrID = swimObjByTag2.GetFishSvrID();
						STTF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
						STTF_GameInfo.getInstance().UserList[j].LockFish = gameObject3;
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
