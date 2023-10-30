using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNTG_FishPoolMngr : MonoBehaviour
{
	public class FishCreatePara
	{
		public int mFishNumber;

		public DNTG_FISH_TYPE mFishType;

		public int mFishPathType;

		public int mSvrID = -99999;

		public DNTG_FISH_TYPE[] mSecondFishType;

		public SpecialFishType mSpecialFishType;

		public int mFishKingPos = -1;

		public int mMonkeyKingBet = -1;
	}

	public static DNTG_FishPoolMngr G_FishPoolMngr;

	private List<GameObject> mFishList = new List<GameObject>();

	public DNTG_FishPrefabs fishPrefabs;

	public GameObject objLockFlag;

	public int[] mFishIndexInLayer;

	public List<GameObject> mAllFishArr = new List<GameObject>();

	public Dictionary<int, GameObject> mID_Fish_Dictionary = new Dictionary<int, GameObject>();

	public Dictionary<GameObject, int> mFish_ID_Dictionary = new Dictionary<GameObject, int>();

	private int s_count;

	private static int s_SvrID;

	[HideInInspector]
	public int totalScore;

	public DNTG_FreeEffect freeEffect;

	private List<DNTG_ISwimObj> iSwimObjPart = new List<DNTG_ISwimObj>();

	public static DNTG_FishPoolMngr GetSingleton()
	{
		return G_FishPoolMngr;
	}

	private void Awake()
	{
		if (G_FishPoolMngr == null)
		{
			G_FishPoolMngr = this;
		}
		mFishIndexInLayer = new int[DNTG_GameParameter.G_nAllFishNum];
	}

	public void AllFishDie(DNTG_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null && !GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_SuperBomb && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_Wheels && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_LocalBomb)
			{
				gameObject.SendMessage("GoDie", bulletPara);
			}
		}
	}

	public void PartFishDie(DNTG_BulletPara bulletPara)
	{
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			for (int j = 0; j < iSwimObjPart.Count; j++)
			{
				DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if (swimObjByTag != null && swimObjByTag == iSwimObjPart[j] && !swimObjByTag.IsLockFishOutsideWindow() && swimObjByTag.mFishType != DNTG_FISH_TYPE.Fish_SuperBomb && swimObjByTag.mFishType != DNTG_FISH_TYPE.Fish_Wheels && swimObjByTag.mFishType != DNTG_FISH_TYPE.Fish_LocalBomb)
				{
					gameObject.SendMessage("GoDie", bulletPara);
					break;
				}
			}
		}
	}

	public List<GameObject> GetAllFishArr()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null && !GetSwimObjByTag(gameObject).IsLockFishOutsideWindow() && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_SuperBomb && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_Wheels && GetSwimObjByTag(gameObject).mFishType != DNTG_FISH_TYPE.Fish_LocalBomb)
			{
				list.Add(gameObject);
			}
		}
		return list;
	}

	public void FixAllFish()
	{
		if (freeEffect != null)
		{
			freeEffect.ShowEff();
		}
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			gameObject.GetComponent<DNTG_DoMove>().Stop();
		}
		StopCoroutine("IE_CreateFish");
		DNTG_GameInfo.getInstance().CountTime = true;
	}

	public void UnFixAllFish()
	{
		if (freeEffect != null)
		{
			freeEffect.EndEff();
		}
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null)
			{
				gameObject.GetComponent<DNTG_DoMove>().Play();
			}
		}
		DNTG_GameInfo.getInstance().CountTime = false;
	}

	public void Fishing2(Vector3 fishingPos, DNTG_Bullet bullet, GameObject collisionFish)
	{
		iSwimObjPart = new List<DNTG_ISwimObj>();
		int mPower = bullet.mPower;
		List<GameObject> list = new List<GameObject>();
		bool bHaveKnife = false;
		Vector3 zero = Vector3.zero;
		list.Add(collisionFish);
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			bool flag = false;
			DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
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
		DNTG_FISH_TYPE mFishType = collisionFish.GetComponent<DNTG_ISwimObj>().mFishType;
		if (mFishType == DNTG_FISH_TYPE.Fish_SuperBomb || mFishType == DNTG_FISH_TYPE.Fish_Wheels || mFishType == DNTG_FISH_TYPE.Fish_LocalBomb || IsFishKing(mFishType))
		{
			bHaveKnife = true;
		}
		DNTG_HitFish[] array = null;
		int count = GetAllFishArr().Count;
		array = new DNTG_HitFish[count];
		switch (mFishType)
		{
		case DNTG_FISH_TYPE.Fish_SuperBomb:
			for (int num3 = 0; num3 < count; num3++)
			{
				GameObject gameObject4 = GetAllFishArr()[num3];
				DNTG_ISwimObj swimObjByTag5 = GetSwimObjByTag(gameObject4);
				if (!(swimObjByTag5 != null))
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				array[num3] = new DNTG_HitFish();
				array[num3].fishid = swimObjByTag5.GetFishSvrID();
				array[num3].fishtype = (int)swimObjByTag5.mFishType;
				DNTG_HitFish obj5 = array[num3];
				Vector3 position7 = gameObject4.transform.position;
				obj5.fx = position7.x;
				DNTG_HitFish obj6 = array[num3];
				Vector3 position8 = gameObject4.transform.position;
				obj6.fy = position8.y;
			}
			break;
		case DNTG_FISH_TYPE.Fish_Wheels:
		{
			zero = collisionFish.transform.position;
			for (int l = 0; l < count; l++)
			{
				GameObject gameObject2 = GetAllFishArr()[l];
				DNTG_ISwimObj swimObjByTag3 = GetSwimObjByTag(gameObject2);
				if (swimObjByTag3 == null)
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				if (Vector3.Distance(gameObject2.transform.position, zero) < 3.5f)
				{
					iSwimObjPart.Add(swimObjByTag3);
				}
			}
			array = new DNTG_HitFish[iSwimObjPart.Count];
			string text = string.Empty;
			for (int m = 0; m < iSwimObjPart.Count; m++)
			{
				array[m] = new DNTG_HitFish();
				array[m].fishid = iSwimObjPart[m].GetFishSvrID();
				array[m].fishtype = (int)iSwimObjPart[m].mFishType;
				DNTG_HitFish obj = array[m];
				Vector3 position3 = iSwimObjPart[m].transform.position;
				obj.fx = position3.x;
				DNTG_HitFish obj2 = array[m];
				Vector3 position4 = iSwimObjPart[m].transform.position;
				obj2.fy = position4.y;
				text = text + iSwimObjPart[m].mFishType + " , ";
			}
			if (array.Length > 0)
			{
				UnityEngine.Debug.LogWarning("大范围局部炸弹: " + array.Length + " " + text);
			}
			break;
		}
		case DNTG_FISH_TYPE.Fish_LocalBomb:
		{
			zero = collisionFish.transform.position;
			for (int n = 0; n < count; n++)
			{
				GameObject gameObject3 = GetAllFishArr()[n];
				DNTG_ISwimObj swimObjByTag4 = GetSwimObjByTag(gameObject3);
				if (swimObjByTag4 == null)
				{
					UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
					return;
				}
				if (Vector3.Distance(gameObject3.transform.position, zero) < 2f)
				{
					iSwimObjPart.Add(swimObjByTag4);
				}
			}
			array = new DNTG_HitFish[iSwimObjPart.Count];
			string arg = string.Empty;
			for (int num2 = 0; num2 < iSwimObjPart.Count; num2++)
			{
				array[num2] = new DNTG_HitFish();
				array[num2].fishid = iSwimObjPart[num2].GetFishSvrID();
				array[num2].fishtype = (int)iSwimObjPart[num2].mFishType;
				DNTG_HitFish obj3 = array[num2];
				Vector3 position5 = iSwimObjPart[num2].transform.position;
				obj3.fx = position5.x;
				DNTG_HitFish obj4 = array[num2];
				Vector3 position6 = iSwimObjPart[num2].transform.position;
				obj4.fy = position6.y;
				arg = arg + iSwimObjPart[num2].mFishType + " , ";
			}
			break;
		}
		default:
		{
			if (!IsFishKing(mFishType))
			{
				break;
			}
			DNTG_FISH_TYPE dNTG_FISH_TYPE = mFishType - 20;
			for (int j = 0; j < count; j++)
			{
				GameObject fish = GetAllFishArr()[j];
				DNTG_ISwimObj swimObjByTag2 = GetSwimObjByTag(fish);
				if (swimObjByTag2 != null && swimObjByTag2.mFishType == dNTG_FISH_TYPE)
				{
					iSwimObjPart.Add(swimObjByTag2);
				}
			}
			array = new DNTG_HitFish[iSwimObjPart.Count];
			for (int k = 0; k < iSwimObjPart.Count; k++)
			{
				DNTG_HitFish[] array2 = array;
				int num = k;
				DNTG_HitFish dNTG_HitFish = new DNTG_HitFish
				{
					fishid = iSwimObjPart[k].GetFishSvrID(),
					fishtype = (int)iSwimObjPart[k].mFishType
				};
				DNTG_HitFish dNTG_HitFish2 = dNTG_HitFish;
				Vector3 position = iSwimObjPart[k].transform.position;
				dNTG_HitFish2.fx = position.x;
				DNTG_HitFish dNTG_HitFish3 = dNTG_HitFish;
				Vector3 position2 = iSwimObjPart[k].transform.position;
				dNTG_HitFish3.fy = position2.y;
				array2[num] = dNTG_HitFish;
			}
			break;
		}
		}
		if (!((float)list.Count > 0f))
		{
			return;
		}
		int mPlayerSeatID = DNTG_GameMngr.GetSingleton().mPlayerSeatID;
		int num4 = (list.Count < 10) ? list.Count : 10;
		DNTG_HitFish[] array3 = new DNTG_HitFish[num4];
		for (int num5 = 0; num5 < num4; num5++)
		{
			GameObject gameObject5 = list[num5];
			DNTG_ISwimObj swimObjByTag6 = GetSwimObjByTag(gameObject5);
			if (!(swimObjByTag6 != null))
			{
				UnityEngine.Debug.Log("@FishPoolMngr Fishing Error,with fishArr.Count error tag");
				return;
			}
			array3[num5] = new DNTG_HitFish();
			array3[num5].fishid = swimObjByTag6.GetFishSvrID();
			array3[num5].fishtype = (int)swimObjByTag6.mFishType;
			DNTG_HitFish obj7 = array3[num5];
			Vector3 position9 = gameObject5.transform.position;
			obj7.fx = position9.x;
			DNTG_HitFish obj8 = array3[num5];
			Vector3 position10 = gameObject5.transform.position;
			obj8.fy = position10.y;
		}
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendGunHitfish(bullet.mBulletID, array3, bHaveKnife, array);
	}

	private bool IsFishKing(DNTG_FISH_TYPE nFishType)
	{
		switch (nFishType)
		{
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
		case DNTG_FISH_TYPE.Fish_Same_Grass:
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
			return true;
		default:
			return false;
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

	private void _updateFishIndex(DNTG_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num < 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		switch (fishTyp)
		{
		case DNTG_FISH_TYPE.Fish_Monkey:
		case DNTG_FISH_TYPE.Fish_KillThree_One:
		case DNTG_FISH_TYPE.Fish_KillThree_Two:
		case DNTG_FISH_TYPE.Fish_KillThree_Three:
		case DNTG_FISH_TYPE.Fish_KillThree_Four:
		case DNTG_FISH_TYPE.Fish_GoldFull:
		case DNTG_FISH_TYPE.Fish_LightningChain:
		case DNTG_FISH_TYPE.Fish_Heaven:
		case DNTG_FISH_TYPE.Fish_SilverDragon:
		case DNTG_FISH_TYPE.Fish_GoldDolphin:
		case DNTG_FISH_TYPE.Fish_Octopus:
		case DNTG_FISH_TYPE.Fish_Mermaid:
		case DNTG_FISH_TYPE.Fish_Sailboat:
		case DNTG_FISH_TYPE.Fish_TYPE_NONE:
			break;
		case DNTG_FISH_TYPE.Fish_Shrimp:
		case DNTG_FISH_TYPE.Fish_Grass:
		case DNTG_FISH_TYPE.Fish_Zebra:
		case DNTG_FISH_TYPE.Fish_BigEars:
		case DNTG_FISH_TYPE.Fish_YellowSpot:
		case DNTG_FISH_TYPE.Fish_Ugly:
		case DNTG_FISH_TYPE.Fish_Hedgehog:
		case DNTG_FISH_TYPE.Fish_BlueAlgae:
		case DNTG_FISH_TYPE.Fish_Lamp:
		case DNTG_FISH_TYPE.Fish_Turtle:
		case DNTG_FISH_TYPE.Fish_Trailer:
		case DNTG_FISH_TYPE.Fish_Butterfly:
		case DNTG_FISH_TYPE.Fish_Beauty:
		case DNTG_FISH_TYPE.Fish_Arrow:
		case DNTG_FISH_TYPE.Fish_Bat:
		case DNTG_FISH_TYPE.Fish_SilverShark:
		case DNTG_FISH_TYPE.Fish_GoldenShark:
		case DNTG_FISH_TYPE.Fish_BlueDolphin:
		case DNTG_FISH_TYPE.Fish_Boss:
		case DNTG_FISH_TYPE.Fish_Penguin:
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
		case DNTG_FISH_TYPE.Fish_Same_Grass:
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
			if (mFishIndexInLayer[num] < 24)
			{
				mFishIndexInLayer[num] += 3;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		case DNTG_FISH_TYPE.Fish_SuperBomb:
		case DNTG_FISH_TYPE.Fish_FixBomb:
		case DNTG_FISH_TYPE.Fish_LocalBomb:
		case DNTG_FISH_TYPE.Fish_Wheels:
			if (mFishIndexInLayer[num] < 8)
			{
				mFishIndexInLayer[num]++;
			}
			else
			{
				mFishIndexInLayer[num] = 0;
			}
			break;
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
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

	public int GetFishIndexInLayer(DNTG_FISH_TYPE fishTyp)
	{
		int num = (int)fishTyp;
		if (num < 0 || num >= mFishIndexInLayer.Length)
		{
			num = 0;
		}
		return mFishIndexInLayer[num];
	}

	public Transform CreateFishForBig(DNTG_FISH_TYPE fishType, int nSvrID, SpecialFishType specialFishType)
	{
		if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
		{
			UnityEngine.Debug.LogError("创建鱼异常: " + fishType);
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, specialFishType, -1, -1);
		if (transform == null)
		{
			UnityEngine.Debug.LogError("创建鱼异常: " + fishType + "  " + nSvrID);
			return null;
		}
		return transform;
	}

	public Transform CreateFishForCoralReefs(DNTG_FISH_TYPE fishType)
	{
		if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
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
		Transform transform = DNTG_PoolManager.Pools["DNTGFishPool"].Spawn(fishPrefabs.objFish[num].transform);
		transform.Rotate(Vector3.up, 90f, Space.World);
		transform.SendMessage("SetFishType", fishType);
		mAllFishArr.Add(transform.gameObject);
		_updateFishIndex(fishType);
		return transform;
	}

	public Transform CreateFish(DNTG_FISH_TYPE fishType, int nSvrID, SpecialFishType specialFishType, int fishKingPos, int monkeyKingBet)
	{
		if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
		{
			return null;
		}
		Transform transform = null;
		try
		{
			transform = ((fishType != DNTG_FISH_TYPE.Fish_Monkey) ? DNTG_PoolManager.Pools["DNTGFishPool"].Spawn(fishPrefabs.objFish[(int)fishType].transform) : DNTG_PoolManager.Pools["DNTGFishPoolUGUI"].Spawn(fishPrefabs.objFish[(int)fishType].transform));
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError((int)fishType + "  " + arg);
		}
		if (transform == null)
		{
			UnityEngine.Debug.LogError((int)fishType + " 号鱼没找到");
			return null;
		}
		if (fishType != DNTG_FISH_TYPE.Fish_Monkey)
		{
			transform.Rotate(Vector3.up, 90f, Space.World);
		}
		else
		{
			transform.Rotate(Vector3.zero);
		}
		Transform transform2 = transform.Find("Fish");
		if (transform2 == null)
		{
			if (fishType != DNTG_FISH_TYPE.Fish_GoldFull)
			{
				UnityEngine.Debug.LogError((int)fishType + " 号Fish没找到");
				return null;
			}
			Animator[] componentsInChildren = transform.GetComponentsInChildren<Animator>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].gameObject.SetActive(value: true);
				}
				else
				{
					UnityEngine.Debug.LogError(i + "======空======");
				}
			}
		}
		else
		{
			transform2.gameObject.SetActive(value: true);
		}
		transform.SendMessage("SetSpecialFishType", specialFishType);
		transform.SendMessage("SetFishKingPos", fishKingPos);
		if (monkeyKingBet > 0)
		{
			transform.SendMessage("SetMonkeyKingBet", monkeyKingBet);
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

	public void CreateLightningFishFish(DNTG_FISH_TYPE fishType, int nPathType, int nSvrID, SpecialFishType specialFishType)
	{
		StartCoroutine("IE_CreateFish", new FishCreatePara
		{
			mFishNumber = 1,
			mFishPathType = nPathType,
			mSvrID = nSvrID,
			mFishType = fishType,
			mSpecialFishType = specialFishType
		});
	}

	public void CreateFish(DNTG_FISH_TYPE fishType, int nPathType, int nSvrIDBegin, int nFishNumber, SpecialFishType specialFishType, int fishKingPos, int MonkeyKingBet)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			UnityEngine.Debug.LogError("路径错误: " + nPathType);
		}
		else if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
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
				mFishType = fishType,
				mSpecialFishType = specialFishType,
				mFishKingPos = fishKingPos,
				mMonkeyKingBet = MonkeyKingBet
			});
		}
	}

	private IEnumerator IE_CreateFish(FishCreatePara para)
	{
		int svrIDBegin = para.mSvrID;
		int fishKingPos = para.mFishKingPos;
		if ((para.mFishType == DNTG_FISH_TYPE.Fish_Grass || para.mFishType == DNTG_FISH_TYPE.Fish_Shrimp) && para.mFishNumber == 5)
		{
			DNTG_FISH_TYPE mFishType4 = para.mFishType;
			int mFishPathType3 = para.mFishPathType;
			SpecialFishType mSpecialFishType = para.mSpecialFishType;
			int nSvrID2;
			svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
			CreateFish(mFishType4, mFishPathType3, nSvrID2, mSpecialFishType, fishKingPos, -1);
			for (int j = 1; j < para.mFishNumber; j++)
			{
				DNTG_FISH_TYPE mFishType5 = para.mFishType;
				int nPathType = para.mFishPathType * 4 + j - 1;
				svrIDBegin = (nSvrID2 = svrIDBegin) + 1;
				CreateSmall_5_Fish(mFishType5, nPathType, nSvrID2, mSpecialFishType, fishKingPos);
			}
		}
		else
		{
			int monkeyKingBet = para.mMonkeyKingBet;
			for (int i = 0; i < para.mFishNumber; i++)
			{
				DNTG_FISH_TYPE mFishType3 = para.mFishType;
				int mFishPathType2 = para.mFishPathType;
				SpecialFishType specialFishType = para.mSpecialFishType;
				int num;
				int nSvrID = num = svrIDBegin;
				svrIDBegin = num + 1;
				CreateFish(mFishType3, mFishPathType2, nSvrID, specialFishType, fishKingPos, monkeyKingBet);
				yield return new WaitForSeconds(_getFishWaitTime(para.mFishType));
			}
		}
	}

	private float _getFishWaitTime(DNTG_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DNTG_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case DNTG_FISH_TYPE.Fish_Grass:
			return 1f;
		case DNTG_FISH_TYPE.Fish_Zebra:
			return 1.5f;
		case DNTG_FISH_TYPE.Fish_BigEars:
			return 1.2f;
		case DNTG_FISH_TYPE.Fish_YellowSpot:
			return 1.7f;
		case DNTG_FISH_TYPE.Fish_Ugly:
			return 1.7f;
		case DNTG_FISH_TYPE.Fish_Hedgehog:
			return 1.6f;
		case DNTG_FISH_TYPE.Fish_BlueAlgae:
			return 2f;
		case DNTG_FISH_TYPE.Fish_Lamp:
			return 2f;
		case DNTG_FISH_TYPE.Fish_Turtle:
			return 2.1f;
		case DNTG_FISH_TYPE.Fish_Trailer:
			return 2.1f;
		case DNTG_FISH_TYPE.Fish_Butterfly:
			return 2.3f;
		default:
			return 1f;
		}
	}

	public Transform CreateFish(DNTG_FISH_TYPE fishType, int nPathType, int nSvrID, SpecialFishType specialFishType, int fishKingPos, int monkeyKingBet)
	{
		if (nPathType > 88 || nPathType < 0)
		{
			UnityEngine.Debug.LogError("====路径异常: " + nPathType);
			return null;
		}
		if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
		{
			UnityEngine.Debug.LogError("====异常鱼：" + fishType);
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, specialFishType, fishKingPos, monkeyKingBet);
		if (transform == null)
		{
			UnityEngine.Debug.LogError("===创建的鱼为空: " + fishType + "  " + nSvrID);
			return null;
		}
		DNTG_PathManager fishPath = DNTG_FishPathMngr.GetSingleton().GetFishPath(nPathType);
		DNTG_DoMove component = transform.GetComponent<DNTG_DoMove>();
		component.enabled = true;
		component.points = fishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<DNTG_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<DNTG_ISwimObj>().SetUpDir(isR: false);
		}
		return transform;
	}

	public Transform CreateSmall_5_Fish(DNTG_FISH_TYPE fishType, int nPathType, int nSvrID, SpecialFishType specialFishType, int fishKingPos)
	{
		if (nPathType >= 352 || nPathType < 0)
		{
			UnityEngine.Debug.LogError("====路径异常: " + nPathType);
			return null;
		}
		if ((fishType >= DNTG_FISH_TYPE.Fish_TYPE_NONE || fishType < DNTG_FISH_TYPE.Fish_Shrimp) && fishType != DNTG_FISH_TYPE.Fish_Penguin)
		{
			UnityEngine.Debug.LogError("====异常鱼：" + fishType);
			return null;
		}
		Transform transform = CreateFish(fishType, nSvrID, specialFishType, fishKingPos, -1);
		if (transform == null)
		{
			UnityEngine.Debug.LogError("===创建的鱼为空: " + fishType + "  " + nSvrID);
			return null;
		}
		DNTG_PathManager smallFishPath = DNTG_FishPathMngr.GetSingleton().GetSmallFishPath(nPathType);
		DNTG_DoMove component = transform.GetComponent<DNTG_DoMove>();
		component.enabled = true;
		component.points = smallFishPath.vecs;
		component.DoPlay();
		Vector3 position = transform.transform.position;
		if (position.x > 0f)
		{
			transform.GetComponent<DNTG_ISwimObj>().SetUpDir(isR: true);
		}
		else
		{
			transform.GetComponent<DNTG_ISwimObj>().SetUpDir(isR: false);
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
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].LockFish == fish)
			{
				DNTG_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		if (GetSwimObjByTag(fish).mFishType == DNTG_FISH_TYPE.Fish_Monkey)
		{
			DNTG_PoolManager.Pools["DNTGFishPoolUGUI"].Despawn(fish.transform);
		}
		else
		{
			DNTG_PoolManager.Pools["DNTGFishPool"].Despawn(fish.transform);
		}
	}

	public void SetFishDie(int nSvrID, int nPower, int nFishType, int nFishOODs, int nPlayerID, Vector3 pos)
	{
		totalScore = 0;
		DNTG_BulletPara dNTG_BulletPara = new DNTG_BulletPara(nPlayerID, nPower);
		DNTG_ISwimObj dNTG_ISwimObj = null;
		if (mID_Fish_Dictionary.ContainsKey(nSvrID))
		{
			mID_Fish_Dictionary.TryGetValue(nSvrID, out GameObject value);
			dNTG_ISwimObj = GetSwimObjByTag(value);
			dNTG_ISwimObj.HideLockedFlag();
			for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
			{
				if (!(DNTG_GameInfo.getInstance().UserList[i].LockFish == value))
				{
					continue;
				}
				DNTG_Lock currentGun = DNTG_GameInfo.getInstance().GameScene.GetCurrentGun(DNTG_GameInfo.getInstance().UserList[i].SeatIndex);
				if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex == DNTG_GameInfo.getInstance().User.SeatIndex && DNTG_GameInfo.getInstance().UserList[i].Lock)
				{
					DNTG_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
					DNTG_GameInfo.getInstance().UserList[i].LockFish = null;
					if (DNTG_GameInfo.getInstance().GameScene.bAuto)
					{
						DNTG_GameInfo.getInstance().UserList[i].Lock = false;
						currentGun.StartLockForLockFish();
					}
					else
					{
						DNTG_GameInfo.getInstance().UserList[i].Lock = false;
						DNTG_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
						currentGun.EndLock();
					}
				}
				else if (DNTG_GameInfo.getInstance().UserList[i].Lock)
				{
					DNTG_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(DNTG_GameInfo.getInstance().UserList[i].SeatIndex);
					DNTG_GameInfo.getInstance().UserList[i].LockFish = null;
					DNTG_GameInfo.getInstance().UserList[i].Lock = false;
					currentGun.EndLock();
				}
			}
			value.SendMessage("GoDie", dNTG_BulletPara);
			int num = nFishOODs * nPower;
			DNTG_EffectMngr.GetSingleton().ShowFishScore(dNTG_BulletPara.mPlyerIndex, dNTG_ISwimObj.transform.position, num);
			totalScore += num;
		}
		if (nFishType == 25)
		{
			dNTG_BulletPara.mPower = 0;
			AllFishDie(dNTG_BulletPara);
		}
		if (nFishType == 28 || nFishType == 27)
		{
			dNTG_BulletPara.mPower = 0;
			PartFishDie(dNTG_BulletPara);
		}
		if (nFishType == 26)
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
			SameFishDie((DNTG_FISH_TYPE)nFishType, dNTG_BulletPara);
			break;
		}
		if (dNTG_ISwimObj != null && dNTG_ISwimObj.specialFishType == SpecialFishType.LightningFish)
		{
			LightningFishDie(dNTG_BulletPara, dNTG_ISwimObj);
		}
		if (DNTG_EffectMngr.GetSingleton().IsBigFish((DNTG_FISH_TYPE)nFishType))
		{
			DNTG_EffectMngr.GetSingleton().ShowBigPrizePlate(dNTG_BulletPara.mPlyerIndex, totalScore);
		}
	}

	public void SameFishDie(DNTG_FISH_TYPE fishType, DNTG_BulletPara bulletPara)
	{
		DNTG_FISH_TYPE dNTG_FISH_TYPE = fishType - 20;
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			if (gameObject != null)
			{
				DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
				if (swimObjByTag.mFishType == dNTG_FISH_TYPE && !swimObjByTag.IsLockFishOutsideWindow() && !swimObjByTag.IsLockFishOutsideWindow())
				{
					gameObject.SendMessage("GoDie", bulletPara);
				}
			}
		}
	}

	public void LightningFishDie(DNTG_BulletPara bulletPara, DNTG_ISwimObj LightningFish)
	{
		DNTG_EffectMngr.GetSingleton().OverEffSimilarLightning();
		int num = 0;
		for (int i = 0; i < mAllFishArr.Count; i++)
		{
			GameObject gameObject = mAllFishArr[i];
			DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
			if (swimObjByTag != null && swimObjByTag != LightningFish && swimObjByTag.specialFishType == SpecialFishType.LightningFish && !swimObjByTag.IsLockFishOutsideWindow())
			{
				gameObject.SendMessage("GoDieLightning", bulletPara);
				num++;
				DNTG_EffectMngr.GetSingleton().ShowEffSimilarLightning(gameObject.transform.position);
			}
		}
		if (num > 0)
		{
			UnityEngine.Debug.LogError("被锁的闪电鱼: " + num);
			StartCoroutine(WiatSetOver());
		}
	}

	private IEnumerator WiatSetOver()
	{
		yield return new WaitForSeconds(2f);
		DNTG_EffectMngr.GetSingleton().OverEffSimilarLightning();
	}

	public DNTG_ISwimObj GetSwimObjByTag(GameObject fish)
	{
		if (fish == null)
		{
			UnityEngine.Debug.LogError("=====没鱼====");
			return null;
		}
		DNTG_ISwimObj dNTG_ISwimObj = null;
		return fish.GetComponent<DNTG_NormalFish>();
	}

	public void LockFish(int fishid, int seatid, bool locking)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == DNTG_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		mID_Fish_Dictionary.TryGetValue(fishid, out GameObject value);
		DNTG_Lock currentGun = DNTG_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex != seatid || !locking)
			{
				continue;
			}
			if ((bool)value && DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr && value.activeSelf && !GetSwimObjByTag(value).IsLockFishOutsideWindow())
			{
				DNTG_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
				if (DNTG_GameInfo.getInstance().UserList[i].Lock && DNTG_GameInfo.getInstance().UserList[i].LockFish != null)
				{
					DNTG_GameInfo.getInstance().UserList[i].LockFish = value;
					DNTG_GameInfo.getInstance().UserList[i].LockFishID = fishid;
					DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(DNTG_GameInfo.getInstance().UserList[i].LockFish);
					swimObjByTag.bLocked = true;
					currentGun.ChangeLockFish(swimObjByTag.GetLockFishPos());
				}
				else
				{
					DNTG_GameInfo.getInstance().UserList[i].LockFish = value;
					DNTG_GameInfo.getInstance().UserList[i].LockFishID = fishid;
					DNTG_ISwimObj swimObjByTag2 = GetSwimObjByTag(DNTG_GameInfo.getInstance().UserList[i].LockFish);
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
				DNTG_GameInfo.getInstance().UserList[i].Lock = locking;
			}
			else if ((bool)currentGun)
			{
				currentGun.EndLock();
			}
		}
	}

	public void UnLockFish(int fishid, int seatid)
	{
		if (!mID_Fish_Dictionary.ContainsKey(fishid) || seatid == DNTG_GameInfo.getInstance().User.SeatIndex)
		{
			return;
		}
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex != seatid)
			{
				continue;
			}
			DNTG_GameInfo.getInstance().UserList[i].Lock = false;
			DNTG_BulletPoolMngr.GetSingleton().OtherLockingBulletToNormal(seatid);
			if ((bool)DNTG_GameInfo.getInstance().UserList[i].LockFish)
			{
				DNTG_Lock currentGun = DNTG_GameInfo.getInstance().GameScene.GetCurrentGun(seatid);
				if ((bool)currentGun)
				{
					currentGun.EndLock();
				}
				else
				{
					UnityEngine.Debug.Log("gunobj is not exit at UnLockFish");
				}
				DNTG_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
	}

	public int LookForACanBeLockFish(int seatid)
	{
		if (!DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			return -1;
		}
		List<GameObject> list = mAllFishArr;
		List<GameObject> list2 = new List<GameObject>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError("======go1为空======");
				break;
			}
			Transform transform = gameObject.transform.Find("Fish");
			GameObject gameObject2 = null;
			if (transform != null)
			{
				gameObject2 = transform.gameObject;
				if ((bool)gameObject && gameObject.activeSelf)
				{
					DNTG_ISwimObj swimObjByTag = GetSwimObjByTag(gameObject);
					if ((bool)swimObjByTag && swimObjByTag.mFishType >= DNTG_FISH_TYPE.Fish_Turtle && gameObject2.activeSelf && !swimObjByTag.bFishDead && !swimObjByTag.IsLockFishOutsideWindow())
					{
						list2.Add(gameObject);
					}
				}
				continue;
			}
			UnityEngine.Debug.LogError("======Fish为空======");
			break;
		}
		for (int j = 0; j < DNTG_GameInfo.getInstance().UserList.Count; j++)
		{
			if (DNTG_GameInfo.getInstance().UserList[j].SeatIndex != seatid)
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
				if (gameObject3 == null)
				{
					UnityEngine.Debug.LogError("=====go3为空=====");
					break;
				}
				Transform transform2 = gameObject3.transform.Find("Fish");
				GameObject gameObject4 = null;
				if (transform2 != null)
				{
					gameObject4 = transform2.gameObject;
					if ((bool)gameObject3 && gameObject3.activeSelf)
					{
						DNTG_ISwimObj swimObjByTag2 = GetSwimObjByTag(gameObject3);
						if ((bool)swimObjByTag2 && swimObjByTag2.mFishType >= DNTG_FISH_TYPE.Fish_Turtle && gameObject3 != DNTG_GameInfo.getInstance().UserList[j].LockFish && gameObject4.activeSelf && !swimObjByTag2.bFishDead && !swimObjByTag2.IsLockFishOutsideWindow())
						{
							DNTG_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
							int fishSvrID = swimObjByTag2.GetFishSvrID();
							DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(fishSvrID);
							DNTG_GameInfo.getInstance().UserList[j].LockFish = gameObject3;
							swimObjByTag2.bLocked = true;
							return fishSvrID;
						}
					}
					return -1;
				}
				UnityEngine.Debug.LogError("=====fish为空=====");
				break;
			}
			return -1;
		}
		return -1;
	}
}
