using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TF_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public TF_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public TF_FISH_TYPE[] mFishTypes;
	}

	public static TF_FishPoolMngr G_FishPoolMngr;

	private List<GameObject> mFishList = new List<GameObject>();

	public TF_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	private FishCreatePara mPara = new FishCreatePara();

	[HideInInspector]
	public int totalScore;

	public static TF_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[TF_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(TF_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (!GetSwimObjByTag(gameObject).IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie1", bulletPara);
			}
		}
	}

	public void FixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<TF_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		TF_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<TF_DoMove>().Play();
		}
		TF_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing(Vector3 fishingPos, TF_Bullet bullet, GameObject collisionFish)
	{
		int num = bullet.mPower;
		if (bullet.mIsLizi)
		{
			num *= 2;
		}
		TF_BulletPara tF_BulletPara = new TF_BulletPara(bullet.mPlayerID, num);
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		bool flag = false;
		Vector3 zero = Vector3.zero;
		list.Add(collisionFish);
		TF_ISwimObj component = collisionFish.GetComponent<TF_ISwimObj>();
		if (component.mFishType == TF_FISH_TYPE.Fish_SuperBomb || (component.mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp && component.mFishType <= TF_FISH_TYPE.Fish_Same_Turtle))
		{
			flag = true;
			zero = collisionFish.transform.position;
			list2.Add(collisionFish);
		}
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag2 = false;
			TF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
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
				if ((component.mFishType == TF_FISH_TYPE.Fish_SuperBomb || (component.mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp && component.mFishType <= TF_FISH_TYPE.Fish_Same_Turtle)) && !flag)
				{
					flag = true;
					zero = gameObject.transform.position;
					list2.Add(gameObject);
				}
				list.Add(gameObject);
			}
		}
		if (flag)
		{
			for (int j = 0; j < mAllFishArr.Count; j++)
			{
				GameObject gameObject2 = mAllFishArr[j];
				TF_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject2);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error isKnifeIn tag1");
					return;
				}
				int mFishType = (int)swimObjByTag2.mFishType;
				if (!swimObjByTag2.IsLockFishOutsideWindow())
				{
					if (component.mFishType == TF_FISH_TYPE.Fish_SuperBomb)
					{
						list2.Add(gameObject2);
					}
					else if (component.mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp && component.mFishType <= TF_FISH_TYPE.Fish_Same_Turtle && swimObjByTag2.mFishType == component.mFishType - 25)
					{
						list2.Add(gameObject2);
					}
				}
			}
		}
		TF_HitFish[] array = null;
		if ((float)list2.Count > 0f)
		{
			int mPlayerSeatID = TF_GameMngr.GetSingleton().mPlayerSeatID;
			int count = list2.Count;
			array = new TF_HitFish[count];
			for (int k = 0; k < count; k++)
			{
				GameObject gameObject3 = list2[k];
				TF_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
				array[k] = new TF_HitFish();
				array[k].fishid = swimObjByTag3.GetFishSvrID();
				array[k].fishtype = (int)swimObjByTag3.mFishType;
				TF_HitFish obj = array[k];
				Vector3 position = gameObject3.transform.position;
				obj.fx = position.x;
				TF_HitFish obj2 = array[k];
				Vector3 position2 = gameObject3.transform.position;
				obj2.fy = position2.y;
			}
		}
		if (!((float)list.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID2 = TF_GameMngr.GetSingleton().mPlayerSeatID;
		int num2 = (list.Count < 10) ? list.Count : 10;
		TF_HitFish[] array2 = new TF_HitFish[num2];
		for (int l = 0; l < num2; l++)
		{
			GameObject gameObject4 = list[l];
			TF_ISwimObj swimObjByTag4 = GetSwimObjByTag(gameObject4);
			if (!(swimObjByTag4 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array2[l] = new TF_HitFish();
			array2[l].fishid = swimObjByTag4.GetFishSvrID();
			array2[l].fishtype = (int)swimObjByTag4.mFishType;
			TF_HitFish obj3 = array2[l];
			Vector3 position3 = gameObject4.transform.position;
			obj3.fx = position3.x;
			TF_HitFish obj4 = array2[l];
			Vector3 position4 = gameObject4.transform.position;
			obj4.fy = position4.y;
		}
		if (!TF_GameParameter.G_bTest)
		{
			TF_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, flag, array);
		}
	}

	public void OneFishing(Vector3 fishingPos, TF_Bullet bullet, GameObject collisionFish)
	{
		bool bHaveKnife = false;
		Vector3 zero = Vector3.zero;
		if (collisionFish.GetComponent<TF_ISwimObj>().mFishType == TF_FISH_TYPE.Fish_PartBomb)
		{
			bHaveKnife = true;
			zero = collisionFish.transform.position;
		}
		TF_HitFish[] hitfish = null;
		TF_HitFish[] array = new TF_HitFish[1]
		{
			new TF_HitFish()
		};
		TF_ISwimObj swimObjByTag = GetSwimObjByTag(collisionFish);
		if (swimObjByTag == null)
		{
			UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
			return;
		}
		array[0].fishid = swimObjByTag.GetFishSvrID();
		array[0].fishtype = (int)swimObjByTag.mFishType;
		TF_HitFish obj = array[0];
		Vector3 position = collisionFish.transform.position;
		obj.fx = position.x;
		TF_HitFish obj2 = array[0];
		Vector3 position2 = collisionFish.transform.position;
		obj2.fy = position2.y;
		TF_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array, bHaveKnife, hitfish);
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

	private void _updateFishIndex(TF_FISH_TYPE fishTyp)
	{
		if (fishTyp >= TF_FISH_TYPE.Fish_Shrimp && fishTyp <= TF_FISH_TYPE.Fish_Dragon)
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
		else if (fishTyp >= TF_FISH_TYPE.Fish_Same_Shrimp && fishTyp <= TF_FISH_TYPE.Fish_Same_Turtle)
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
		else if (fishTyp >= TF_FISH_TYPE.Fish_FixBomb && fishTyp <= TF_FISH_TYPE.Fish_CoralReefs)
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
		else if (fishTyp >= TF_FISH_TYPE.Fish_BigEars_Group && fishTyp <= TF_FISH_TYPE.Fish_Turtle_Group)
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

	public int GetFishIndexInLayer(TF_FISH_TYPE fishTyp)
	{
		return mFishIndexInLayer[(int)fishTyp];
	}

	public Transform CreateFishForBig(TF_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= TF_FISH_TYPE.Fish_TYPE_NONE || fishType < TF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		return CreateFish(fishType, nSvrID);
	}

	public Transform CreateFishForCoralReefs(TF_FISH_TYPE fishType)
	{
		if (fishType >= TF_FISH_TYPE.Fish_TYPE_NONE || fishType < TF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		if (num >= 22 && num <= 24)
		{
			num -= 2;
		}
		if (num <= 34 && num >= 25)
		{
			num -= 25;
		}
		else if (num > 34)
		{
			num -= 12;
		}
		Transform transform = TF_PoolManager.Pools["TFFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(TF_FISH_TYPE fishType, int nSvrID, TF_FISH_TYPE[] fishTypes = null)
	{
		if (fishType >= TF_FISH_TYPE.Fish_TYPE_NONE || fishType < TF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		int num = (int)fishType;
		if (num >= 22 && num <= 24)
		{
			num -= 2;
		}
		if (num <= 34 && num >= 25)
		{
			num -= 25;
		}
		else if (num > 34)
		{
			num -= 12;
		}
		Transform transform = TF_PoolManager.Pools["TFFishPool"].Spawn(fishPrefabs.objFish[num].transform);
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

	public Transform CreateCoralReefsFish(TF_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(TF_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<TF_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(TF_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber, TF_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType <= 88 && nPathType >= 0 && fishType < TF_FISH_TYPE.Fish_TYPE_NONE && fishType >= TF_FISH_TYPE.Fish_Shrimp)
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
		if ((para.mFishType == TF_FISH_TYPE.Fish_Grass || para.mFishType == TF_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			TF_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				TF_FISH_TYPE mFishType5 = para.mFishType;
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
				TF_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID, para.mFishTypes);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(TF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case TF_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case TF_FISH_TYPE.Fish_Grass:
			return 1f;
		case TF_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case TF_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case TF_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case TF_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case TF_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case TF_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case TF_FISH_TYPE.Fish_Lamp:
			return 2f;
		case TF_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case TF_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case TF_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(TF_FISH_TYPE fishType, int nPathType, int nSvrID, TF_FISH_TYPE[] fishTypes = null)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= TF_FISH_TYPE.Fish_TYPE_NONE || fishType < TF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, fishTypes);
		TF_PathManager fishPath = TF_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		TF_DoMove component = transform.GetComponent<TF_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<TF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<TF_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(TF_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= TF_FISH_TYPE.Fish_TYPE_NONE || fishType < TF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		TF_PathManager smallFishPath = TF_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		TF_DoMove component = transform.GetComponent<TF_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<TF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<TF_ISwimObj>().SetUpDir(isR: false);
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
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				TF_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		TF_PoolManager.Pools["TFFishPool"].Despawn(fish.transform);
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		TF_BulletPara tF_BulletPara = new TF_BulletPara(nPlayerID, nPower);
		TF_ISwimObj tF_ISwimObj = null;
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			tF_ISwimObj = GetSwimObjByTag(value);
			tF_ISwimObj.HideLockedFlag();
			for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(TF_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				TF_Lock currentGun = TF_GameInfo.getInstance().GameScene.GetCurrentGun(TF_GameInfo.getInstance().UserList[i].SeatIndex);
				if (TF_GameInfo.getInstance().UserList[i].SeatIndex == TF_GameInfo.getInstance().User.SeatIndex && TF_GameInfo.getInstance().UserList[i].Lock)
				{
					TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					TF_GameInfo.getInstance().UserList[i].LockFish = null;
					if (TF_GameInfo.getInstance().GameScene.bAuto)
					{
						TF_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						TF_GameInfo.getInstance().UserList[i].Lock = false;
						TF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (TF_GameInfo.getInstance().UserList[i].Lock)
				{
					TF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(TF_GameInfo.getInstance().UserList[i].SeatIndex);
					TF_GameInfo.getInstance().UserList[i].LockFish = null;
					TF_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			if (value.CompareTag("CoralReefs"))
			{
				value.GetComponent<TF_CoralReefsFish>().mRealFishType = (TF_FISH_TYPE)UnityEngine.Random.Range(9, 16);
			}
			value.SendMessage("GoDie", tF_BulletPara);
			if (nFishType >= 17 && nFishType <= 19)
			{
				int nScore = nFishOODs * tF_BulletPara.mPower;
				TF_EffectMngr.GetSingleton().ShowFishScore(tF_BulletPara.mPlyerIndex, pos, nScore);
				TF_EffectMngr.GetSingleton().PlayCoinFly(tF_BulletPara.mPlyerIndex, (TF_FISH_TYPE)nFishType, pos);
			}
		}
		if (nFishType == 23)
		{
			AllFishDie(tF_BulletPara);
		}
		if (nFishType == 22)
		{
			FixAllFish();
		}
		if (nFishType <= 34 && nFishType >= 25)
		{
			SameFishDie((TF_FISH_TYPE)nFishType, tF_BulletPara);
		}
		if (TF_EffectMngr.GetSingleton().IsBigFish((TF_FISH_TYPE)nFishType))
		{
			TF_EffectMngr.GetSingleton().ShowBigPrizePlate(tF_BulletPara.mPlyerIndex, totalScore);
		}
	}

	public void SameFishDie(TF_FISH_TYPE fishType, TF_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			TF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType - 25 && !swimObjByTag.IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie", bulletPara);
			}
		}
	}

	public TF_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		TF_ISwimObj result = null;
		if (fish.CompareTag("NormalFish"))
		{
			result = fish.GetComponent<TF_NormalFish>();
		}
		else if (fish.CompareTag("GroupFish"))
		{
			result = fish.GetComponent<TF_GroupFish>();
		}
		else if (fish.CompareTag("DoubleKill"))
		{
			result = fish.GetComponent<TF_GroupFish>();
		}
		else if (fish.CompareTag("CoralReefs"))
		{
			result = fish.GetComponent<TF_CoralReefsFish>();
		}
		else if (fish.CompareTag("ForCoralReefsDie"))
		{
			result = fish.GetComponent<TF_NormalFish>();
		}
		else if (fish.CompareTag("StopBomb"))
		{
			result = fish.GetComponent<TF_NormalFish>();
		}
		else if (fish.CompareTag("SuperBomb"))
		{
			result = fish.GetComponent<TF_NormalFish>();
		}
		return result;
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == TF_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		TF_Lock currentGun = TF_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				TF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (TF_GameInfo.getInstance().UserList[i].Lock && TF_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					TF_GameInfo.getInstance().UserList[i].LockFish = value;
					TF_ISwimObj swimObjByTag = GetSwimObjByTag(TF_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					TF_GameInfo.getInstance().UserList[i].LockFish = value;
					TF_ISwimObj swimObjByTag2 = GetSwimObjByTag(TF_GameInfo.getInstance().UserList[i].LockFish);
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
				TF_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == TF_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			TF_GameInfo.getInstance().UserList[i].Lock = false;
			TF_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)TF_GameInfo.getInstance().UserList[i].LockFish)
			{
				TF_Lock currentGun = TF_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				TF_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
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
				TF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if ((bool)swimObjByTag && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
				{
					list2.Add(gameObject);
				}
			}
		}
		int index = 0;
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < list2.Count; j++)
		{
			TF_ISwimObj swimObjByTag2 = GetSwimObjByTag(list2[j]);
			int mFishType = (int)swimObjByTag2.mFishType;
			int fishOODS = TF_EffectMngr.GetSingleton().GetFishOODS(swimObjByTag2.mFishType);
			if (fishOODS > num2)
			{
				num2 = fishOODS;
				num = mFishType;
				index = j;
			}
			else if (fishOODS == num2 && mFishType >= num)
			{
				num = mFishType;
				index = j;
			}
		}
		if (num2 < 30)
		{
			return -1;
		}
		for (int k = 0; k < TF_GameInfo.getInstance().UserList.Count; k++)
		{
			if (TF_GameInfo.getInstance().UserList[k].SeatIndex != seatid)
			{
				continue;
			}
			if (list2.Count != 0)
			{
				GameObject gameObject3 = list2[index];
				GameObject gameObject4 = gameObject3.transform.Find("Fish").gameObject;
				if ((bool)gameObject3 && gameObject3.activeSelf)
				{
					TF_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
					if ((bool)swimObjByTag3 && gameObject3 != TF_GameInfo.getInstance().UserList[k].LockFish && gameObject4.activeSelf && !swimObjByTag3.bFishDead && !swimObjByTag3.IsLockFishOutsideWindow())
					{
						TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
						int fishSvrID = swimObjByTag3.GetFishSvrID();
						TF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
						TF_GameInfo.getInstance().UserList[k].LockFish = gameObject3;
						swimObjByTag3.bLocked = true;
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
