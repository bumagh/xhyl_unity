using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BZJX_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public BZJX_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public BZJX_FISH_TYPE[] mSecondFishType;
	}

	public static BZJX_FishPoolMngr G_FishPoolMngr;

	private List<GameObject> mFishList = new List<GameObject>();

	public BZJX_FishPrefabs fishPrefabs;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	[HideInInspector]
	public int totalScore;

	private List<BZJX_ISwimObj> iSwimObjPart = new List<BZJX_ISwimObj>();

	public static BZJX_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[BZJX_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(BZJX_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null && !GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != BZJX_FISH_TYPE.Fish_SuperBomb && GetSwimObjByTag(gameObject).mFishType != BZJX_FISH_TYPE.Fish_PartBomb)
			{
				gameObject.SendMessage("GoDie", bulletPara);
				BZJX_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	public void PartFishDie(BZJX_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			for (int j = 0; j < iSwimObjPart.Count; j++)
			{
				BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if (swimObjByTag != null && swimObjByTag == iSwimObjPart[j] && !swimObjByTag.IsLockFishOutsideWindow() && swimObjByTag.mFishType != BZJX_FISH_TYPE.Fish_SuperBomb && swimObjByTag.mFishType != BZJX_FISH_TYPE.Fish_PartBomb)
				{
					gameObject.SendMessage("GoDie", bulletPara);
					BZJX_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
					break;
				}
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
			if (gameObject != null && !GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != BZJX_FISH_TYPE.Fish_SuperBomb && GetSwimObjByTag(gameObject).mFishType != BZJX_FISH_TYPE.Fish_PartBomb)
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
			gameObject.GetComponent<BZJX_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		BZJX_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<BZJX_DoMove>().Play();
		}
		BZJX_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing2(Vector3 fishingPos, BZJX_Bullet bullet, GameObject collisionFish)
	{
		iSwimObjPart = new List<BZJX_ISwimObj>();
		int mPower = bullet.mPower;
		if (bullet.mIsLizi)
		{
			mPower *= 2;
		}
		List<GameObject> list = new List<GameObject>();
		bool bHaveKnife = false;
		Vector3 zero = Vector3.zero;
		list.Add(collisionFish);
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag = false;
			BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
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
		if (collisionFish.GetComponent<BZJX_ISwimObj>().mFishType == BZJX_FISH_TYPE.Fish_SuperBomb || collisionFish.GetComponent<BZJX_ISwimObj>().mFishType == BZJX_FISH_TYPE.Fish_PartBomb)
		{
			bHaveKnife = true;
		}
		BZJX_HitFish[] array = null;
		int count = GetAllFishArr().Count;
		array = new BZJX_HitFish[count];
		if (collisionFish.GetComponent<BZJX_ISwimObj>().mFishType == BZJX_FISH_TYPE.Fish_SuperBomb)
		{
			UnityEngine.Debug.LogError("==========超级炸弹========");
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject2 = GetAllFishArr()[j];
				BZJX_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject2);
				if (!(swimObjByTag2 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				array[j] = new BZJX_HitFish();
				array[j].fishid = swimObjByTag2.GetFishSvrID();
				array[j].fishtype = (int)swimObjByTag2.mFishType;
				BZJX_HitFish obj = array[j];
				Vector3 position = gameObject2.transform.position;
				obj.fx = position.x;
				BZJX_HitFish obj2 = array[j];
				Vector3 position2 = gameObject2.transform.position;
				obj2.fy = position2.y;
			}
			UnityEngine.Debug.LogError("==========超级炸弹========" + array.Length);
		}
		else if (collisionFish.GetComponent<BZJX_ISwimObj>().mFishType == BZJX_FISH_TYPE.Fish_PartBomb)
		{
			zero = collisionFish.transform.position;
			for (int k = 0; k < count; k++)
			{
				GameObject gameObject3 = GetAllFishArr()[k];
				BZJX_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject3);
				if (swimObjByTag3 == null)
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				if (Vector3.Distance(gameObject3.transform.position, zero) < 3.5f)
				{
					iSwimObjPart.Add(swimObjByTag3);
				}
			}
			array = new BZJX_HitFish[iSwimObjPart.Count];
			string text = string.Empty;
			for (int l = 0; l < iSwimObjPart.Count; l++)
			{
				array[l] = new BZJX_HitFish();
				array[l].fishid = iSwimObjPart[l].GetFishSvrID();
				array[l].fishtype = (int)iSwimObjPart[l].mFishType;
				BZJX_HitFish obj3 = array[l];
				Vector3 position3 = iSwimObjPart[l].transform.position;
				obj3.fx = position3.x;
				BZJX_HitFish obj4 = array[l];
				Vector3 position4 = iSwimObjPart[l].transform.position;
				obj4.fy = position4.y;
				text = text + iSwimObjPart[l].mFishType + " , ";
			}
			if (array.Length > 0)
			{
				UnityEngine.Debug.LogError("捕鱼: " + array.Length + " " + text);
			}
		}
		if (!((float)list.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID = BZJX_GameMngr.GetSingleton().mPlayerSeatID;
		int num = (list.Count < 10) ? list.Count : 10;
		BZJX_HitFish[] array2 = new BZJX_HitFish[num];
		for (int m = 0; m < num; m++)
		{
			GameObject gameObject4 = list[m];
			BZJX_ISwimObj swimObjByTag4 = GetSwimObjByTag(gameObject4);
			if (!(swimObjByTag4 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array2[m] = new BZJX_HitFish();
			array2[m].fishid = swimObjByTag4.GetFishSvrID();
			array2[m].fishtype = (int)swimObjByTag4.mFishType;
			BZJX_HitFish obj5 = array2[m];
			Vector3 position5 = gameObject4.transform.position;
			obj5.fx = position5.x;
			BZJX_HitFish obj6 = array2[m];
			Vector3 position6 = gameObject4.transform.position;
			obj6.fy = position6.y;
		}
		BZJX_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array2, bHaveKnife, array);
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

	private void _updateFishIndex(BZJX_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num < 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		switch (fishTyp)
		{
		case BZJX_FISH_TYPE.Fish_TYPE_NONE:
			break;
		case BZJX_FISH_TYPE.Fish_Shrimp:
		case BZJX_FISH_TYPE.Fish_Grass:
		case BZJX_FISH_TYPE.Fish_Zebra:
		case BZJX_FISH_TYPE.Fish_BigEars:
		case BZJX_FISH_TYPE.Fish_YellowSpot:
		case BZJX_FISH_TYPE.Fish_Ugly:
		case BZJX_FISH_TYPE.Fish_Hedgehog:
		case BZJX_FISH_TYPE.Fish_BlueAlgae:
		case BZJX_FISH_TYPE.Fish_Lamp:
		case BZJX_FISH_TYPE.Fish_Turtle:
		case BZJX_FISH_TYPE.Fish_Trailer:
		case BZJX_FISH_TYPE.Fish_Butterfly:
		case BZJX_FISH_TYPE.Fish_Beauty:
		case BZJX_FISH_TYPE.Fish_Arrow:
		case BZJX_FISH_TYPE.Fish_Bat:
		case BZJX_FISH_TYPE.Fish_SilverShark:
		case BZJX_FISH_TYPE.Fish_GoldenShark:
		case BZJX_FISH_TYPE.Fish_GoldenSharkB:
		case BZJX_FISH_TYPE.Fish_GoldenDragon:
		case BZJX_FISH_TYPE.Fish_Boss:
		case BZJX_FISH_TYPE.Fish_Penguin:
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
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
			if (mFishIndexInLayer[num] < 24)
			{
				mFishIndexInLayer[num] += 3;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		case BZJX_FISH_TYPE.Fish_SuperBomb:
		case BZJX_FISH_TYPE.Fish_FixBomb:
		case BZJX_FISH_TYPE.Fish_CoralReefs:
		case BZJX_FISH_TYPE.Fish_PartBomb:
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		case BZJX_FISH_TYPE.Fish_BigEars_Group:
		case BZJX_FISH_TYPE.Fish_YellowSpot_Group:
		case BZJX_FISH_TYPE.Fish_Hedgehog_Group:
		case BZJX_FISH_TYPE.Fish_Ugly_Group:
		case BZJX_FISH_TYPE.Fish_BlueAlgae_Group:
		case BZJX_FISH_TYPE.Fish_Turtle_Group:
			if (mFishIndexInLayer[num] < 32)
			{
				mFishIndexInLayer[num] += 4;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		}
	}

	public int GetFishIndexInLayer(BZJX_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num < 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		return mFishIndexInLayer[num];
	}

	public Transform CreateFishForBig(BZJX_FISH_TYPE fishType, int nSvrID)
	{
		if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		if (transform == null)
		{
			return null;
		}
		return transform;
	}

	public Transform CreateFishForCoralReefs(BZJX_FISH_TYPE fishType)
	{
		if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
		{
			return null;
		}
		int num = (int)fishType;
		if (num <= 29 && num >= 20)
		{
			num -= 20;
		}
		else if (num >= 30)
		{
			num -= 10;
		}
		Transform transform = BZJX_PoolManager.Pools["BZJXFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(BZJX_FISH_TYPE fishType, int nSvrID)
	{
		if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
		{
			return null;
		}
		int num = (int)fishType;
		if (num <= 29 && num >= 20)
		{
			num -= 20;
		}
		else if (num >= 30)
		{
			num -= 10;
		}
		Transform transform = BZJX_PoolManager.Pools["BZJXFishPool"].Spawn(fishPrefabs.objFish[num].transform);
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

	public Transform CreateCoralReefsFish(BZJX_FISH_TYPE RealFishType, int nPathType, int nSvrID)
	{
		Transform transform = CreateFish(BZJX_FISH_TYPE.Fish_CoralReefs, nPathType, nSvrID);
		transform.GetComponent<BZJX_CoralReefsFish>().mRealFishType = RealFishType;
		return transform;
	}

	public void CreateFish(BZJX_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber)
	{
		if (nPathType <= 88 && nPathType >= 0)
		{
			if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
			{
				UnityEngine.Debug.LogError("======异常鱼: " + fishType);
			}
			else
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
	}

	private IEnumerator IE_CreateFish(FishCreatePara para)
	{
		int svrIDBegin = para.mSvrID;
		if ((para.mFishType == BZJX_FISH_TYPE.Fish_Grass || para.mFishType == BZJX_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			BZJX_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				BZJX_FISH_TYPE mFishType5 = para.mFishType;
				int nPathType = para.mFishPathType * 4 + j - 1;
				svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
				CreateSmall_5_Fish(mFishType5, nPathType, nSvrID2);
			}
		}
		else
		{
			for (int i = 0; i < para.mFishNumber; i++)
			{
				BZJX_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(BZJX_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case BZJX_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case BZJX_FISH_TYPE.Fish_Grass:
			return 1f;
		case BZJX_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case BZJX_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case BZJX_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case BZJX_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case BZJX_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case BZJX_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case BZJX_FISH_TYPE.Fish_Lamp:
			return 2f;
		case BZJX_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case BZJX_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case BZJX_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(BZJX_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			return null;
		}
		if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
		{
			UnityEngine.Debug.LogError("====异常鱼：");
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		if (transform == null)
		{
			return null;
		}
		BZJX_PathManager fishPath = BZJX_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		BZJX_DoMove component = transform.GetComponent<BZJX_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<BZJX_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<BZJX_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(BZJX_FISH_TYPE fishType, int nPathType, int nSvrID)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			return null;
		}
		if ((fishType >= BZJX_FISH_TYPE.Fish_TYPE_NONE || fishType < BZJX_FISH_TYPE.Fish_Shrimp) && fishType != BZJX_FISH_TYPE.Fish_Penguin)
		{
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID);
		if (transform == null)
		{
			return null;
		}
		BZJX_PathManager smallFishPath = BZJX_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		BZJX_DoMove component = transform.GetComponent<BZJX_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<BZJX_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<BZJX_ISwimObj>().SetUpDir(isR: false);
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
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				BZJX_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		BZJX_PoolManager.Pools["BZJXFishPool"].Despawn(fish.transform);
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		totalScore = 0;
		BZJX_BulletPara bZJX_BulletPara = new BZJX_BulletPara(nPlayerID, nPower);
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(value);
			swimObjByTag.HideLockedFlag();
			for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(BZJX_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				BZJX_Lock currentGun = BZJX_GameInfo.getInstance().GameScene.GetCurrentGun(BZJX_GameInfo.getInstance().UserList[i].SeatIndex);
				if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex == BZJX_GameInfo.getInstance().User.SeatIndex && BZJX_GameInfo.getInstance().UserList[i].Lock)
				{
					BZJX_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					BZJX_GameInfo.getInstance().UserList[i].LockFish = null;
					if (BZJX_GameInfo.getInstance().GameScene.bAuto)
					{
						BZJX_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						BZJX_GameInfo.getInstance().UserList[i].Lock = false;
						BZJX_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (BZJX_GameInfo.getInstance().UserList[i].Lock)
				{
					BZJX_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(BZJX_GameInfo.getInstance().UserList[i].SeatIndex);
					BZJX_GameInfo.getInstance().UserList[i].LockFish = null;
					BZJX_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			if (value.CompareTag("CoralReefs"))
			{
				value.GetComponent<BZJX_CoralReefsFish>().mRealFishType = (BZJX_FISH_TYPE)UnityEngine.Random.Range(9, 16);
			}
			value.SendMessage("GoDie", bZJX_BulletPara);
			if (swimObjByTag.mFishType == BZJX_FISH_TYPE.Fish_GoldenDragon || swimObjByTag.mFishType == BZJX_FISH_TYPE.Fish_GoldenSharkB || swimObjByTag.mFishType == BZJX_FISH_TYPE.Fish_Boss || swimObjByTag.mFishType == BZJX_FISH_TYPE.Fish_SuperBomb || swimObjByTag.mFishType == BZJX_FISH_TYPE.Fish_PartBomb)
			{
				int num = nFishOODs * nPower;
				UnityEngine.Debug.LogError("鱼: " + swimObjByTag.mFishType + "  变倍率: " + nFishOODs);
				BZJX_EffectMngr.GetSingleton().ShowFishScore(bZJX_BulletPara.mPlyerIndex, base.transform.position, num);
				totalScore += num;
			}
		}
		if (nFishType == 30)
		{
			bZJX_BulletPara.mPower = 0;
			AllFishDie(bZJX_BulletPara);
		}
		if (nFishType == 39)
		{
			bZJX_BulletPara.mPower = 0;
			PartFishDie(bZJX_BulletPara);
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
			SameFishDie((BZJX_FISH_TYPE)nFishType, bZJX_BulletPara);
			break;
		}
		if (BZJX_EffectMngr.GetSingleton().IsBigFish((BZJX_FISH_TYPE)nFishType))
		{
			BZJX_EffectMngr.GetSingleton().ShowBigPrizePlate(bZJX_BulletPara.mPlyerIndex, totalScore);
		}
	}

	public void SameFishDie(BZJX_FISH_TYPE fishType, BZJX_BulletPara bulletPara)
	{
		BZJX_EffectMngr.GetSingleton().OverEffSimilarLightning();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag.mFishType == fishType - 20 && !swimObjByTag.IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDie", bulletPara);
				BZJX_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		StartCoroutine(WiatSetOver());
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		BZJX_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public BZJX_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		BZJX_ISwimObj result = null;
		if (fish.CompareTag("NormalFish"))
		{
			result = fish.GetComponent<BZJX_NormalFish>();
		}
		else if (fish.CompareTag("GroupFish"))
		{
			result = fish.GetComponent<BZJX_GroupFish>();
		}
		else if (fish.CompareTag("CoralReefs"))
		{
			result = fish.GetComponent<BZJX_CoralReefsFish>();
		}
		else if (fish.CompareTag("ForCoralReefsDie"))
		{
			result = fish.GetComponent<BZJX_NormalFish>();
		}
		else if (fish.CompareTag("PartBomb"))
		{
			result = fish.GetComponent<BZJX_NormalFish>();
		}
		else if (fish.CompareTag("StopBomb"))
		{
			result = fish.GetComponent<BZJX_NormalFish>();
		}
		else if (fish.CompareTag("SuperBomb"))
		{
			result = fish.GetComponent<BZJX_NormalFish>();
		}
		return result;
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == BZJX_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		BZJX_Lock currentGun = BZJX_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				BZJX_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (BZJX_GameInfo.getInstance().UserList[i].Lock && BZJX_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					BZJX_GameInfo.getInstance().UserList[i].LockFish = value;
					BZJX_GameInfo.getInstance().UserList[i].LockFishID = fishid;
					BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(BZJX_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					BZJX_GameInfo.getInstance().UserList[i].LockFish = value;
					BZJX_GameInfo.getInstance().UserList[i].LockFishID = fishid;
					BZJX_ISwimObj swimObjByTag2 = GetSwimObjByTag(BZJX_GameInfo.getInstance().UserList[i].LockFish);
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
				BZJX_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == BZJX_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			BZJX_GameInfo.getInstance().UserList[i].Lock = false;
			BZJX_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)BZJX_GameInfo.getInstance().UserList[i].LockFish)
			{
				BZJX_Lock currentGun = BZJX_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				BZJX_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
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
				BZJX_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if ((bool)swimObjByTag && swimObjByTag.mFishType >= BZJX_FISH_TYPE.Fish_Turtle && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
				{
					list2.Add(gameObject);
				}
			}
		}
		for (int j = 0; j < BZJX_GameInfo.getInstance().UserList.Count; j++)
		{
			if (BZJX_GameInfo.getInstance().UserList[j].SeatIndex != seatid)
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
					BZJX_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject3);
					if ((bool)swimObjByTag2 && swimObjByTag2.mFishType >= BZJX_FISH_TYPE.Fish_Turtle && gameObject3 != BZJX_GameInfo.getInstance().UserList[j].LockFish && gameObject4.activeSelf && !swimObjByTag2.bFishDead && !swimObjByTag2.IsLockFishOutsideWindow())
					{
						BZJX_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
						int fishSvrID = swimObjByTag2.GetFishSvrID();
						BZJX_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
						BZJX_GameInfo.getInstance().UserList[j].LockFish = gameObject3;
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
