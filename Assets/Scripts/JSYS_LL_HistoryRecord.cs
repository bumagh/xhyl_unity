using com.miracle9.game.bean;
using UnityEngine;

public class JSYS_LL_HistoryRecord : MonoBehaviour
{
	protected struct SRecord
	{
		public GameObject RecordObj;

		public Transform DirectionObj;

		public GameObject LuckyMacObj;

		public GameObject LuckTypeObj;

		public UISprite RecordZHX;

		public UISprite RecordIcon;

		public UISprite LuckyType;

		public UILabel LuckyMac;
	}

	protected JSYS_LL_HudManager mHudMngr;

	protected Collider mBackgroundCol;

	protected UIPanel mRecordPanel;

	protected static int gMaxRecord = 21;

	protected SRecord[] mRecordList = new SRecord[gMaxRecord];

	protected DeskRecord[] mHistoryList = new DeskRecord[gMaxRecord];

	protected int mRecordCount;

	protected static string[] gLuckySpriteName = new string[3]
	{
		"luckyJP",
		"luckyBonus",
		"luckyTimes"
	};

	protected static string[] gZHXSpriteName = new string[3]
	{
		"zhuang",
		"he",
		"xian"
	};

	protected static string[] gColorSpriteName = new string[3]
	{
		"colorRed",
		"colorGreen",
		"colorYellow"
	};

	protected static string[] gAnimalSpriteName = new string[4]
	{
		"aniimalLion",
		"animalPanda",
		"animalMonkey",
		"animalRabbit"
	};

	private void Start()
	{
		mHudMngr = GameObject.Find("HudPanel").GetComponent<JSYS_LL_HudManager>();
		mRecordPanel = GetComponent<UIPanel>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		for (int i = 0; i < 21; i++)
		{
			mRecordList[i].RecordObj = base.transform.Find("Record" + i).gameObject;
			mRecordList[i].RecordIcon = mRecordList[i].RecordObj.transform.Find("Icon").GetComponent<UISprite>();
			mRecordList[i].RecordZHX = mRecordList[i].RecordObj.transform.Find("ZHXResult").GetComponent<UISprite>();
			mRecordList[i].LuckTypeObj = mRecordList[i].RecordObj.transform.Find("LuckyType").gameObject;
			mRecordList[i].LuckyType = mRecordList[i].LuckTypeObj.transform.GetComponent<UISprite>();
			mRecordList[i].LuckyMacObj = mRecordList[i].RecordObj.transform.Find("LuckyMac").gameObject;
			mRecordList[i].LuckyMac = mRecordList[i].LuckyMacObj.transform.GetComponent<UILabel>();
			if (i > 0)
			{
				mRecordList[i].DirectionObj = mRecordList[i].RecordObj.transform.Find("Direction");
			}
			mRecordList[i].RecordObj.SetActiveRecursively(state: false);
		}
		HideRecord();
	}

	private void Update()
	{
	}

	public void ClearRecord()
	{
		for (int i = 0; i < 21; i++)
		{
			mRecordList[i].RecordObj.SetActiveRecursively(state: false);
		}
	}

	public void ResetRecord(DeskRecord[] result)
	{
		mRecordCount = ((result.Length <= gMaxRecord) ? result.Length : gMaxRecord);
		if (mRecordCount <= gMaxRecord)
		{
			for (int i = 0; i < mRecordCount; i++)
			{
				mHistoryList[i] = result[i];
			}
		}
		_updateRecord();
	}

	protected void _updateRecord()
	{
		Quaternion identity = Quaternion.identity;
		Vector3 localPosition = new Vector3(0f, 0f, -5f);
		for (int i = 0; i < 21; i++)
		{
			if (i < mRecordCount)
			{
				if (mRecordCount <= 7)
				{
					localPosition.x = 390f - (float)((mRecordCount - 1 - i) % 7) * 130f;
				}
				else if (mRecordCount <= 14)
				{
					if (i < mRecordCount - 7)
					{
						localPosition.x = 390f - (float)((mRecordCount - 1 - i) % 7) * 130f;
					}
					else
					{
						localPosition.x = -390f + (float)((mRecordCount - 1 - i) % 7) * 130f;
					}
				}
				else if (i >= 7 && i < 14)
				{
					localPosition.x = -390f + (float)((mRecordCount - 1 - i) % 7) * 130f;
				}
				else
				{
					localPosition.x = 390f - (float)((mRecordCount - 1 - i) % 7) * 130f;
				}
				localPosition.y = -165f + (float)((mRecordCount - 1 - i) / 7) * 165f;
				mRecordList[i].RecordObj.SetActiveRecursively(state: true);
				mRecordList[i].RecordObj.transform.localPosition = localPosition;
				if (i != 0)
				{
					if ((localPosition.x == -390f && localPosition.y == -165f) || (localPosition.x == 390f && localPosition.y == 0f))
					{
						mRecordList[i].DirectionObj.localPosition = new Vector3(0f, 85f, 0f);
						identity = Quaternion.AngleAxis(-90f, Vector3.forward);
						mRecordList[i].DirectionObj.localRotation = identity;
					}
					else if (localPosition.y == 0f)
					{
						mRecordList[i].DirectionObj.localPosition = new Vector3(65f, 0f, 0f);
						identity = Quaternion.AngleAxis(180f, Vector3.forward);
						mRecordList[i].DirectionObj.localRotation = identity;
					}
					else
					{
						mRecordList[i].DirectionObj.localPosition = new Vector3(-65f, 0f, 0f);
						identity = Quaternion.AngleAxis(0f, Vector3.forward);
						mRecordList[i].DirectionObj.localRotation = identity;
					}
				}
				mRecordList[i].LuckyMacObj.SetActiveRecursively(state: false);
				mRecordList[i].LuckTypeObj.SetActiveRecursively(state: false);
				if (mHistoryList[i].zhuangXianHe < 0 || mHistoryList[i].zhuangXianHe > 2)
				{
					JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果庄和闲错误" + i + "_" + mHistoryList[i].zhuangXianHe);
					mHistoryList[i].zhuangXianHe = 0;
				}
				mRecordList[i].RecordZHX.spriteName = gZHXSpriteName[mHistoryList[i].zhuangXianHe];
				if (mHistoryList[i].awardType == 0)
				{
					if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 11)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果中奖动物错误" + i + "_" + mHistoryList[i].animalType);
						mHistoryList[i].animalType = 11;
					}
					mRecordList[i].RecordIcon.spriteName = "animalIcon" + mHistoryList[i].animalType;
					continue;
				}
				if (mHistoryList[i].awardType == 1)
				{
					if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 11)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果中奖动物错误" + i + "_" + mHistoryList[i].animalType);
						mHistoryList[i].animalType = 11;
					}
					mRecordList[i].RecordIcon.spriteName = "animalIcon" + mHistoryList[i].animalType;
					mRecordList[i].LuckTypeObj.SetActiveRecursively(state: true);
					mRecordList[i].LuckyType.spriteName = "luckyJP";
					continue;
				}
				if (mHistoryList[i].awardType == 2)
				{
					if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 11)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果中奖动物错误" + i + "_" + mHistoryList[i].animalType);
						mHistoryList[i].animalType = 11;
					}
					mRecordList[i].RecordIcon.spriteName = "bonusAnimal" + mHistoryList[i].animalType;
					continue;
				}
				if (mHistoryList[i].awardType == 3)
				{
					if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 3)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果大三元动物错误" + i + "_" + mHistoryList[i].animalType);
						mHistoryList[i].animalType = 3;
					}
					mRecordList[i].RecordIcon.spriteName = gAnimalSpriteName[mHistoryList[i].animalType];
					continue;
				}
				if (mHistoryList[i].awardType == 4)
				{
					if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 2)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果大四喜颜色错误" + i + "_" + mHistoryList[i].animalType);
						mHistoryList[i].animalType = 0;
					}
					mRecordList[i].RecordIcon.spriteName = gColorSpriteName[mHistoryList[i].animalType];
					continue;
				}
				if (mHistoryList[i].awardType == 5)
				{
					if (mHistoryList[i].songDengCount < 2 || mHistoryList[i].songDengCount > 11)
					{
						JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果送灯错误" + i + "_" + mHistoryList[i].songDengCount);
						mHistoryList[i].songDengCount = 2;
					}
					mRecordList[i].RecordIcon.spriteName = "songDeng" + (mHistoryList[i].songDengCount - 1);
					continue;
				}
				int num = mHistoryList[i].awardType - 6;
				if (mHistoryList[i].animalType < 0 || mHistoryList[i].animalType > 11)
				{
					JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果中奖动物错误" + i + "_" + mHistoryList[i].animalType);
					mHistoryList[i].animalType = 11;
				}
				mRecordList[i].RecordIcon.spriteName = "animalIcon" + mHistoryList[i].animalType;
				mRecordList[i].LuckyMacObj.SetActiveRecursively(state: true);
				if (num < 0 || num > 2)
				{
					JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果幸运奖错误" + i + "_" + num);
					num = 0;
				}
				mRecordList[i].LuckyType.spriteName = gLuckySpriteName[num];
				mRecordList[i].LuckTypeObj.SetActiveRecursively(state: true);
				if (mHistoryList[i].luckNumber < 1 || mHistoryList[i].luckNumber > 8)
				{
					JSYS_LL_ErrorManager.GetSingleton().AddError("Error:记录结果分机错误" + i + "_" + mHistoryList[i].luckNumber);
					mHistoryList[i].luckNumber = 1;
				}
				mRecordList[i].LuckyMac.text = mHistoryList[i].luckNumber.ToString();
			}
			else
			{
				mRecordList[i].RecordObj.SetActiveRecursively(state: false);
			}
		}
	}

	public void ShowRecord()
	{
		if (mHudMngr.IsRecordPressed)
		{
			mRecordPanel.enabled = true;
			mBackgroundCol.enabled = true;
		}
	}

	public void HideRecord()
	{
		mHudMngr.IsRecordPressed = false;
		mRecordPanel.enabled = false;
		mBackgroundCol.enabled = false;
	}
}
