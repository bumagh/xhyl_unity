using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STMF_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public STMF_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;
	}

	public static STMF_FishPoolMngr G_FishPoolMngr;

	public STMF_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public ArrayList mAllFishArr = new ArrayList();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	public static STMF_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[STMF_GameParameter.G_nAllFishNum];
	}

	private void Start()
	{
		if (STMF_GameParameter.G_bTest)
		{
			InvokeRepeating("testCreateFish", 0f, 0.4285f);
		}
	}

	public void testCreateFish()
	{
		int nFishNumber = 1;
		int num = (int)STMF_FishGo.GetSingleton().FishOut(out nFishNumber);
		if (num != 36)
		{
			int randomPath = STMF_FishPathMngr.GetSingleton().GetRandomPath();
			if (num == 32)
			{
				CreateCoralReefsFish(STMF_FISH_TYPE.Fish_Ugly, randomPath, s_SvrID++);
			}
			else
			{
				UnityEngine.Debug.Log("-----FishType--------" + num);
				CreateFish((STMF_FISH_TYPE)num, randomPath, s_SvrID, nFishNumber);
				s_SvrID += nFishNumber;
			}
			CreateCoralReefsFish(STMF_FISH_TYPE.Fish_SilverShark, randomPath, s_SvrID++);
		}
	}

	public void AllFishDie(STMF_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i] as GameObject;
			gameObject.SendMessage("GoDie", bulletPara);
			STMF_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
		}
		StartCoroutine(WiatSetOver());
	}

	public void Fishing(Vector3 fishingPos, STMF_Bullet bullet, GameObject collisionFish)
	{
		int num = bullet.mPower;
		if (bullet.mIsLizi)
		{
			num *= 2;
		}
		STMF_BulletPara value = new STMF_BulletPara(bullet.mPlayerID, num);
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		bool flag = false;
		Vector3 b = Vector3.zero;
		arrayList.Add(collisionFish);
		if (collisionFish.GetComponent<STMF_ISwimObj>().mFishType == STMF_FISH_TYPE.Fish_LimitedBomb)
		{
			UnityEngine.Debug.Log("击中屠龙刀!!");
			flag = true;
			b = collisionFish.transform.position;
			arrayList2.Add(collisionFish);
		}
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i] as GameObject;
			bool flag2 = false;
			STMF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (!(swimObjByTag != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error tag1");
				return;
			}
			if (gameObject.CompareTag("ForCoralReefsDie"))
			{
				flag2 = true;
			}
			if (!flag2 && !swimObjByTag.mIsFishDead && collisionFish != gameObject && Vector3.Distance(gameObject.transform.position, fishingPos) < 1.2f)
			{
				if (arrayList.Count >= 10)
				{
					break;
				}
				if (swimObjByTag.mFishType == STMF_FISH_TYPE.Fish_LimitedBomb && !flag)
				{
					UnityEngine.Debug.Log("捕鱼中有屠龙刀 !!");
					flag = true;
					b = gameObject.transform.position;
					arrayList2.Add(gameObject);
				}
				arrayList.Add(gameObject);
				if (!STMF_GameParameter.G_bTest)
				{
				}
			}
		}
		if (flag)
		{
			for (int j = 0; j < mAllFishArr.Count; j++)
			{
				GameObject gameObject2 = mAllFishArr[j] as GameObject;
				STMF_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject2);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with error isKnifeIn tag1");
					return;
				}
				int mFishType = (int)swimObjByTag2.mFishType;
				if (!gameObject2.CompareTag("ForCoralReefsDie") && !gameObject2.CompareTag("LimitedBomb") && !gameObject2.CompareTag("AllBomb") && !swimObjByTag2.mIsFishDead && Vector3.Distance(gameObject2.transform.position, b) < 3f)
				{
					if (arrayList2.Count >= 100)
					{
						break;
					}
					arrayList2.Add(gameObject2);
					if (STMF_GameParameter.G_bTest)
					{
						gameObject2.SendMessage("GoDie", value);
					}
				}
			}
		}
		STMF_HitFish[] array = null;
		if ((float)arrayList2.Count > 0f)
		{
			int mPlayerSeatID = STMF_GameMngr.GetSingleton().mPlayerSeatID;
			int num2 = (arrayList2.Count < 100) ? arrayList2.Count : 100;
			array = new STMF_HitFish[num2];
			for (int k = 0; k < num2; k++)
			{
				GameObject gameObject3 = arrayList2[k] as GameObject;
				STMF_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
				array[k] = new STMF_HitFish();
				array[k].fishid = swimObjByTag3.GetFishSvrID();
				array[k].fishtype = (int)swimObjByTag3.mFishType;
				STMF_HitFish obj = array[k];
				Vector3 position = gameObject3.transform.position;
				obj.fx = position.x;
				STMF_HitFish obj2 = array[k];
				Vector3 position2 = gameObject3.transform.position;
				obj2.fy = position2.y;
			}
		}
		if (!((float)arrayList.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID2 = STMF_GameMngr.GetSingleton().mPlayerSeatID;
		int num3 = (arrayList.Count < 10) ? arrayList.Count : 10;
		STMF_HitFish[] array2 = new STMF_HitFish[num3];
		for (int l = 0; l < num3; l++)
		{
			GameObject gameObject4 = arrayList[l] as GameObject;
			STMF_ISwimObj swimObjByTag4 = GetSwimObjByTag(gameObject4);
			if (!(swimObjByTag4 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array2[l] = new STMF_HitFish();
			array2[l].fishid = swimObjByTag4.GetFishSvrID();
			array2[l].fishtype = (int)swimObjByTag4.mFishType;
			STMF_HitFish obj3 = array2[l];
			Vector3 position3 = gameObject4.transform.position;
			obj3.fx = position3.x;
			STMF_HitFish obj4 = array2[l];
			Vector3 position4 = gameObject4.transform.position;
			obj4.fy = position4.y;
		}
		if (array != null)
		{
		}
		if (!STMF_GameParameter.G_bTest)
		{
			if (arrayList2.Count > 0)
			{
				STMF_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, flag, array);
			}
			else
			{
				STMF_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, flag, array);
			}
		}
	}

	public void RemoveAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i] as GameObject;
			gameObject.SendMessage("BeRemoved");
		}
		StopCoroutine("IE_CreateFish");
	}

	private void _updateFishIndex(STMF_FISH_TYPE fishTyp)
	{
		if (fishTyp >= STMF_FISH_TYPE.Fish_Shrimp && fishTyp <= STMF_FISH_TYPE.Fish_GoldenDragon)
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
		else if (fishTyp >= STMF_FISH_TYPE.Fish_Same_Shrimp && fishTyp <= STMF_FISH_TYPE.Fish_Same_Turtle)
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
		else if (fishTyp >= STMF_FISH_TYPE.Fish_LimitedBomb && fishTyp <= STMF_FISH_TYPE.Fish_CoralReefs)
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
		else if (fishTyp >= STMF_FISH_TYPE.Fish_DragonBeauty_Group && fishTyp <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
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

	public int GetFishIndexInLayer(STMF_FISH_TYPE fishTyp)
	{
		return mFishIndexInLayer[(int)fishTyp];
	}

	public Transform CreateFishForBig(STMF_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= STMF_FISH_TYPE.Fish_TYPE_NONE || fishType < STMF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		return CreateFish(fishType, nSvrID);
	}

	public Transform CreateFishForCoralReefs(STMF_FISH_TYPE fishType)
	{
		if (fishType >= STMF_FISH_TYPE.Fish_TYPE_NONE || fishType < STMF_FISH_TYPE.Fish_Shrimp)
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
		Transform transform = PoolManager.Pools["FishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		transform.Rotate(Vector3.up, 90f, Space.World);
		return transform;
	}

	public Transform CreateFish(STMF_FISH_TYPE fishType, int nSvrID)
	{
		if (fishType >= STMF_FISH_TYPE.Fish_TYPE_NONE || fishType < STMF_FISH_TYPE.Fish_Shrimp)
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
		Transform transform = PoolManager.Pools["FishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		transform.SendMessage("SetFishSvrID", nSvrID);
		mAllFishArr.Add(transform.gameObject);
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			UnityEngine.Debug.Log("@FishPoolMngr ServerID already exist with value :" + nSvrID);
		}
		else
		{
			mID_Fish_Dictionary.Add(nSvrID, transform.gameObject);
			mFish_ID_Dictionary.Add(transform.gameObject, nSvrID);
		}
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateCoralReefsFish(STMF_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(STMF_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<STMF_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(STMF_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber)
	{
		if (nPathType <= 88 && nPathType >= 0 && fishType < STMF_FISH_TYPE.Fish_TYPE_NONE && fishType >= STMF_FISH_TYPE.Fish_Shrimp)
		{
			StartCoroutine("IE_CreateFish", new FishCreatePara
			{
				mFishNumber = nFishNumber,
				mFishPathType = nPathType,
				mSvrID = nSvrIDBegin,
				mFishType = fishType
			});
		}
	}

	private IEnumerator IE_CreateFish(FishCreatePara para)
	{
		int svrIDBegin = para.mSvrID;
		if ((para.mFishType == STMF_FISH_TYPE.Fish_Grass || para.mFishType == STMF_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			STMF_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				STMF_FISH_TYPE mFishType5 = para.mFishType;
				int nPathType = para.mFishPathType * 4 + j - 1;
				svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
				CreateSmall_5_Fish(mFishType5, nPathType, nSvrID2);
			}
		}
		else
		{
			for (int i = 0; i < para.mFishNumber; i++)
			{
				STMF_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(STMF_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STMF_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case STMF_FISH_TYPE.Fish_Grass:
			return 1f;
		case STMF_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case STMF_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case STMF_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case STMF_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case STMF_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case STMF_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case STMF_FISH_TYPE.Fish_Lamp:
			return 2f;
		case STMF_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case STMF_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case STMF_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(STMF_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STMF_FISH_TYPE.Fish_TYPE_NONE || fishType < STMF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		STMF_PathManager fishPath = STMF_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		STMF_HoMove component = transform.GetComponent<STMF_HoMove>();
		component.enabled = true;
		component.waypoints = fishPath.vecs;
		component.Start();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STMF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STMF_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(STMF_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if (fishType >= STMF_FISH_TYPE.Fish_TYPE_NONE || fishType < STMF_FISH_TYPE.Fish_Shrimp)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		STMF_PathManager smallFishPath = STMF_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		STMF_HoMove component = transform.GetComponent<STMF_HoMove>();
		component.enabled = true;
		component.waypoints = smallFishPath.vecs;
		component.Start();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<STMF_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<STMF_ISwimObj>().SetUpDir(isR: false);
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
		PoolManager.Pools["FishPool"].Despawn(fish.transform);
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		STMF_BulletPara sTMF_BulletPara = new STMF_BulletPara(nPlayerID, nPower);
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			STMF_ISwimObj swimObjByTag = GetSwimObjByTag(value);
			if (value.CompareTag("CoralReefs"))
			{
				STMF_CoralReefsFish component = value.GetComponent<STMF_CoralReefsFish>();
				component.mRealFishType = (STMF_FISH_TYPE)nFishType;
			}
			value.SendMessage("GoDie", sTMF_BulletPara);
			pos = value.transform.position;
			if (swimObjByTag.mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle && swimObjByTag.mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp)
			{
				STMF_EffectMngr.GetSingleton().PlayBigFishEffect(value.transform.position);
			}
			else if (swimObjByTag.mFishType == STMF_FISH_TYPE.Fish_AllBomb)
			{
				STMF_EffectMngr.GetSingleton().PlayBigFishEffect(value.transform.position);
			}
			if (STMF_EffectMngr.GetSingleton().IsBigFish(swimObjByTag.mFishType))
			{
				STMF_EffectMngr.GetSingleton().ShowBigPrizePlate(nPlayerID, nFishOODs * nPower);
			}
		}
		else
		{
			int num = STMF_EffectMngr.GetSingleton().GetFishOODS((STMF_FISH_TYPE)nFishType) * nPower;
			if (nFishType == 19)
			{
				num = nFishType;
			}
			if ((float)num > 0f)
			{
				STMF_EffectMngr.GetSingleton().ShowFishScore(nPlayerID, pos, num);
			}
			STMF_EffectMngr.GetSingleton().PlayCoinFly(nPlayerID, (STMF_FISH_TYPE)nFishType, pos);
			if (nFishType <= 29 && nFishType >= 20)
			{
				STMF_EffectMngr.GetSingleton().PlayBigFishEffect(pos);
			}
			if (nFishType == 31)
			{
				STMF_EffectMngr.GetSingleton().PlayBigFishEffect(pos);
			}
			if (STMF_EffectMngr.GetSingleton().IsBigFish((STMF_FISH_TYPE)nFishType))
			{
				STMF_EffectMngr.GetSingleton().ShowBigPrizePlate(nPlayerID, nFishOODs * nPower);
			}
		}
		if (nFishType <= 29 && nFishType >= 20)
		{
			SameFishDie((STMF_FISH_TYPE)nFishType, sTMF_BulletPara);
		}
		if (nFishType == 31)
		{
			AllFishDie(sTMF_BulletPara);
		}
	}

	public void SameFishDie(STMF_FISH_TYPE fishType, STMF_BulletPara bulletPara)
	{
		STMF_EffectMngr.GetSingleton().OverEffSimilarLightning();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i] as GameObject;
			STMF_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType || swimObjByTag.mFishType == fishType - 20)
			{
				gameObject.SendMessage("GoDie", bulletPara);
				STMF_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		STMF_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public STMF_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		if (fish.CompareTag("NormalFish"))
		{
			return fish.GetComponent<STMF_NormalFish>();
		}
		if (fish.CompareTag("GroupFish"))
		{
			return fish.GetComponent<STMF_GroupFish>();
		}
		if (fish.CompareTag("CoralReefs"))
		{
			return fish.GetComponent<STMF_CoralReefsFish>();
		}
		if (fish.CompareTag("ForCoralReefsDie"))
		{
			return fish.GetComponent<STMF_NormalFish>();
		}
		if (fish.CompareTag("LimitedBomb"))
		{
			return fish.GetComponent<STMF_NormalFish>();
		}
		if (!fish.CompareTag("AllBomb"))
		{
			UnityEngine.Debug.Log("@GetSwimObjByTag fish tag Error,with error tag1");
			return null;
		}
		return fish.GetComponent<STMF_NormalFish>();
	}
}
