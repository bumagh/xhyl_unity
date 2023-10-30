using com.miracle9.game.bean;
using LL_GameCommon;
using LL_UICommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_TableList : MonoBehaviour
{
	private Transform dots;

	private UISprite menuArrow;

	private UIPanel panel;

	private UIGrid grid;

	public GameObject dotItem;

	public GameObject tableListItem;

	private Collider tableListCollider;

	private bool isExtendTableList;

	private bool isShrinkTableList;

	private List<UISprite> dotList = new List<UISprite>();

	public GameObject tableItems;

	public UILabel userName;

	public UILabel coinCount;

	public UILabel testCoinCount;

	public UILabel roomName;

	protected Collider mBackgroundCol;

	protected Collider mBackCol;

	protected Collider mToLeftCol;

	protected Collider mToRightCol;

	protected Collider mSelectTableCol;

	protected LL_PersonInfo mUserInfo;

	protected LL_SeatList mSeatList;

	protected LL_RoomList mRoomList;

	protected LL_HudManager mHudMngr;

	protected LL_PrizeResult mResultPanel;

	protected LL_BetPanel mBetPanel;

	protected bool mButtonActive;

	public ArrayList mTableInfoList = new ArrayList();

	protected int mCurrentRoomId = -1;

	protected int mCurrentTableId;

	[HideInInspector]
	public LL_TableInfo mSelectTable;

	protected GameObject mRistrictObj;

	protected UIPanel mTablePropertyPanel;

	protected UILabel mTableName0;

	protected UILabel mTableName1;

	protected UILabel mMinCoin;

	protected UILabel mMinBet;

	protected UILabel mMaxBet;

	protected UILabel mExchange;

	protected UILabel mPersonCount;

	protected float mAnimationTime = -1f;

	protected int mAnimationStep = -1;

	protected GameObject mMoveTableObj;

	protected GameObject[] mStaticTableObj = new GameObject[3];

	protected Transform mCurrentTableTran;

	protected bool mIsTableListViable;

	public ETableToSeatStep mListStep;

	protected float mStepTime;

	protected bool mIsDraging;

	private LL_GameInfo mGameInfo;

	public LL_TableInfo SelectedTable
	{
		get
		{
			return mSelectTable;
		}
		set
		{
			mSelectTable = value;
		}
	}

	private void Start()
	{
		tableListCollider = tableItems.transform.Find("tableList").GetComponent<Collider>();
		menuArrow = tableItems.transform.Find("tableList/listTitle/arrow").GetComponent<UISprite>();
		panel = tableItems.transform.Find("tableList/Scroll View").GetComponent<UIPanel>();
		grid = tableItems.transform.Find("tableList/Scroll View/Grid").GetComponent<UIGrid>();
		dots = tableItems.transform.Find("dots");
		userName = base.transform.Find("title/userName").GetComponent<UILabel>();
		coinCount = base.transform.Find("title/coin").GetComponent<UILabel>();
		testCoinCount = base.transform.Find("title/testCoin").GetComponent<UILabel>();
		roomName = base.transform.Find("title/name").GetComponent<UILabel>();
		mBackCol = base.transform.Find("title/backButton").GetComponent<Collider>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mToLeftCol = base.transform.Find("ToLeftBtn").GetComponent<Collider>();
		mToRightCol = base.transform.Find("ToRightBtn").GetComponent<Collider>();
		mSelectTableCol = base.transform.Find("CurrentTable").GetComponent<Collider>();
		mSeatList = GameObject.Find("SeatPanel").GetComponent<LL_SeatList>();
		mUserInfo = LL_GameInfo.getInstance().UserInfo;
		mRoomList = LL_AppUIMngr.GetSingleton().mRoomList;
		mHudMngr = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		mResultPanel = GameObject.Find("ResultPanel").GetComponent<LL_PrizeResult>();
		mBetPanel = GameObject.Find("BetPanel").GetComponent<LL_BetPanel>();
		mRistrictObj = base.transform.Find("CurrentTable").Find("TableProperty").gameObject;
		mTablePropertyPanel = mRistrictObj.transform.GetComponent<UIPanel>();
		mTableName0 = mRistrictObj.transform.Find("TableName").GetComponentInChildren<UILabel>();
		mTableName1 = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("Name")
			.GetComponent<UILabel>();
		mMinCoin = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("MinCoin")
			.GetComponent<UILabel>();
		mMinBet = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("MinBet")
			.GetComponent<UILabel>();
		mMaxBet = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("MaxBet")
			.GetComponent<UILabel>();
		mExchange = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("Exchange")
			.GetComponent<UILabel>();
		mPersonCount = mRistrictObj.transform.Find("TableRistrict").Find("Ristrict").Find("Count")
			.GetComponent<UILabel>();
		mCurrentTableTran = base.transform.Find("CurrentTable");
		mMoveTableObj = base.transform.Find("Table3").gameObject;
		mMoveTableObj.SetActiveRecursively(state: false);
		for (int i = 0; i < 3; i++)
		{
			mStaticTableObj[i] = base.transform.Find("Table" + i).gameObject;
		}
		base.transform.gameObject.AddComponent<TweenPosition>();
		mCurrentTableTran.gameObject.AddComponent<TweenPosition>();
		mStaticTableObj[0].gameObject.AddComponent<TweenPosition>();
		mStaticTableObj[2].gameObject.AddComponent<TweenPosition>();
		HideTableList(3);
		mGameInfo = LL_GameInfo.getInstance();
	}

	public void updateUserInfo()
	{
		userName.text = mGameInfo.UserInfo.strName;
		coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
		testCoinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.ExpCoinCount % 10000).ToString() : mGameInfo.UserInfo.ExpCoinCount.ToString());
		if (mGameInfo.UserInfo.RoomId == 1)
		{
			roomName.text = ((LL_GameInfo.getInstance().Language == 0) ? "竞技厅" : "Arena");
		}
		else
		{
			roomName.text = ((LL_GameInfo.getInstance().Language == 0) ? "练习厅" : "Training");
		}
	}

	private void setIndicator()
	{
		dotList.Clear();
		for (int i = 0; i < dots.childCount; i++)
		{
			UnityEngine.Object.Destroy(dots.GetChild(i).gameObject);
		}
		dots.DetachChildren();
		int num = (mTableInfoList.Count > 10) ? 10 : mTableInfoList.Count;
		for (int j = 0; j < num; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(dotItem);
			gameObject.name = "item" + j;
			gameObject.transform.parent = dots;
			gameObject.GetComponent<UIWidget>().depth = 5;
			gameObject.GetComponent<UISprite>().MakePixelPerfect();
			int num2 = (num % 2 == 0) ? ((j < num / 2) ? ((j - (num / 2 - 1)) * 30 - 15) : ((j - num / 2) * 30 + 15)) : ((j - num / 2) * 30);
			gameObject.transform.localPosition = new Vector3(num2, 0f, 0f);
			dotList.Add(gameObject.GetComponent<UISprite>());
		}
	}

	private void updateTableList()
	{
		for (int i = 0; i < grid.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(grid.transform.GetChild(i).gameObject);
		}
		grid.transform.DetachChildren();
		for (int j = 0; j < mTableInfoList.Count; j++)
		{
			GameObject gameObject = NGUITools.AddChild(grid.gameObject, tableListItem);
			gameObject.transform.Find("personCount").GetComponent<UILabel>().text = "(" + ((LL_TableInfo)mTableInfoList[j]).PersonCount + "/8)";
			gameObject.transform.Find("name").GetComponent<UILabel>().text = ((LL_TableInfo)mTableInfoList[j]).TableName;
			gameObject.name = "tableListItem" + j;
			gameObject.transform.localPosition = new Vector3(0f, -50 * j, 0f);
			UIButtonMessage uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
			uIButtonMessage.functionName = "clickListItem";
			uIButtonMessage.target = base.gameObject;
		}
	}

	private void updateTableListItem(int index)
	{
		Transform transform = grid.transform.Find("tableListItem" + index);
		transform.Find("personCount").GetComponent<UILabel>().text = "(" + ((LL_TableInfo)mTableInfoList[index]).PersonCount + "/8)";
		transform.Find("name").GetComponent<UILabel>().text = ((LL_TableInfo)mTableInfoList[index]).TableName;
	}

	private void clickListItem(GameObject go)
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		if (!mIsDraging && mButtonActive)
		{
			int num = Convert.ToInt32(go.name.Substring(13));
			if (num != mCurrentTableId)
			{
				mCurrentTableId = num;
				mAnimationTime = 0f;
				mAnimationStep = 0;
				mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
				mTablePropertyPanel.enabled = false;
			}
		}
	}

	private void Update()
	{
		if (mListStep == ETableToSeatStep.TableListMove)
		{
			mStepTime += Time.deltaTime;
			if (mStepTime >= 0.5f)
			{
				mListStep = ETableToSeatStep.NoneAnimation;
				mButtonActive = true;
			}
		}
		_doSelectingTable();
		if (mIsTableListViable)
		{
			_doSeatToTable();
		}
		else
		{
			_doTableToSeat();
		}
		if (isExtendTableList || isShrinkTableList)
		{
			tableListAction();
		}
	}

	private void tableListAction()
	{
		int num = mTableInfoList.Count;
		if (num > 4)
		{
			num = 4;
		}
		if (isExtendTableList)
		{
			panel.clipRange += new Vector4(0f, 0f, 0f, Time.deltaTime / 0.2f * 400f);
			UIPanel uIPanel = panel;
			Vector4 clipRange = panel.clipRange;
			float y = (0f - clipRange.w) / 2f;
			Vector4 clipRange2 = panel.clipRange;
			uIPanel.clipRange = new Vector4(0f, y, 0f, clipRange2.w);
			Vector4 clipRange3 = panel.clipRange;
			if (clipRange3.w >= (float)num * 50f)
			{
				panel.clipRange = new Vector4(0f, (0f - (48f + (float)num * 50f)) / 2f, 280f, num * 50);
				isExtendTableList = false;
				tableListCollider.enabled = true;
			}
			panel.transform.localPosition = new Vector3(0f, 0f, -5f);
		}
		else if (isShrinkTableList)
		{
			panel.clipRange -= new Vector4(0f, 0f, 0f, Time.deltaTime / 0.2f * 400f);
			UIPanel uIPanel2 = panel;
			Vector4 clipRange4 = panel.clipRange;
			float y2 = 200f - clipRange4.w / 2f;
			Vector4 clipRange5 = panel.clipRange;
			uIPanel2.clipRange = new Vector4(0f, y2, 0f, clipRange5.w);
			Vector4 clipRange6 = panel.clipRange;
			if (clipRange6.w <= 0f)
			{
				panel.clipRange = new Vector4(0f, 0f, 280f, 1f);
				isShrinkTableList = false;
				tableListCollider.enabled = false;
			}
			panel.transform.localPosition = new Vector3(0f, 0f, -5f);
		}
	}

	public void handleTableList()
	{
		panel.clipOffset = new Vector2(0f, 0f);
		panel.transform.localPosition = new Vector3(0f, 0f, -5f);
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		Vector4 clipRange = panel.clipRange;
		if (clipRange.w > 10f)
		{
			isShrinkTableList = true;
			menuArrow.spriteName = "xl_sjx2";
		}
		else
		{
			isExtendTableList = true;
			menuArrow.spriteName = "xl_sjx";
		}
	}

	private void Awake()
	{
		GameObject gameObject = base.transform.Find("Background").gameObject;
		UIEventListener.VectorDelegate onDrag = _dragTable;
		UIEventListener.Get(gameObject).onDrag = onDrag;
		UIEventListener.BoolDelegate onPress = _releaseFromBg;
		UIEventListener.Get(gameObject).onPress = onPress;
	}

	public void InitTableList(LL_Desk[] deskList, int iRoomId = 1)
	{
		mCurrentRoomId = iRoomId;
		mUserInfo.RoomId = iRoomId - 1;
		mTableInfoList.Clear();
		mCurrentTableId = 0;
		if (deskList == null)
		{
			return;
		}
		for (int i = 0; i < deskList.Length; i++)
		{
			LL_TableInfo lL_TableInfo = new LL_TableInfo();
			lL_TableInfo.RoomId = deskList[i].roomId;
			lL_TableInfo.TableServerID = deskList[i].id;
			if ((LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game || LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Table) && mSelectTable.TableServerID == lL_TableInfo.TableServerID)
			{
				mCurrentTableId = i;
			}
			lL_TableInfo.TableName = deskList[i].name;
			lL_TableInfo.MinBet = deskList[i].minBet;
			lL_TableInfo.MaxBet = deskList[i].maxBet;
			lL_TableInfo.Ristrict = deskList[i].minGold;
			lL_TableInfo.CreditPerCoin = deskList[i].exchange;
			lL_TableInfo.MinZHXBet = deskList[i].min_zxh;
			lL_TableInfo.MaxCD = deskList[i].betTime;
			lL_TableInfo.MaxZXBet = deskList[i].max_zx;
			lL_TableInfo.MaxHBet = deskList[i].max_h;
			lL_TableInfo.PersonCount = deskList[i].onlineNumber;
			lL_TableInfo.CoinInSetting = deskList[i].onceExchangeValue;
			lL_TableInfo.IsAutoKick = deskList[i].autoKick;
			mTableInfoList.Add(lL_TableInfo);
		}
		setIndicator();
		updateTableList();
		mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
		mSeatList.updateUserInfo();
		_updateRistrictText();
		if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game)
		{
			mBetPanel.ModifyChip(mSelectTable);
			mHudMngr.SetCurrentPosition(mCurrentRoomId, mSelectTable);
		}
	}

	public int SetTotalPerson(int iTableId, int nCount, int iRoomId = 1)
	{
		int result = 0;
		if (nCount > 8 || nCount < 0)
		{
			UnityEngine.Debug.Log("Error: 人数超过限定");
			return 2;
		}
		int index;
		LL_TableInfo tableFromGrid = GetTableFromGrid(iTableId, out index, mCurrentRoomId);
		if (tableFromGrid != null)
		{
			tableFromGrid.PersonCount = nCount;
			if (iTableId == mSelectTable.TableServerID)
			{
				mPersonCount.text = nCount + "/8";
			}
			updateTableListItem(index);
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public int SetTotalPerson(Seat[] seat, int iTableId, int nCount, int iRoomId = 1)
	{
		int result = 0;
		if (nCount > 8 || nCount < 0)
		{
			UnityEngine.Debug.Log("Error: 人数超过限定");
			return 2;
		}
		int index;
		LL_TableInfo tableFromGrid = GetTableFromGrid(iTableId, out index, mCurrentRoomId);
		if (tableFromGrid != null)
		{
			tableFromGrid.PersonCount = nCount;
			if (iTableId == mSelectTable.TableServerID)
			{
				mPersonCount.text = nCount + "/8";
			}
			updateTableListItem(index);
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public int SetTableInfo(int iTableId, int[] iUserKeyID, string[] strNickname, int[] iIconId, int iRoomId = 1, int nLength = 8)
	{
		int result = 0;
		if (nLength > 8)
		{
			UnityEngine.Debug.Log("Error: 人数超过限定");
			return 2;
		}
		if (mSelectTable != null)
		{
			mResultPanel.SetUserList(strNickname, iIconId);
			for (int i = 0; i < 8; i++)
			{
				if (i <= nLength - 1)
				{
					mSelectTable.SetUserKeyID(i, iUserKeyID[i]);
					mSelectTable.SetNick(i, strNickname[i]);
					mSelectTable.SetIcon(i, iIconId[i]);
					mSeatList.AddPerson(i + 1, strNickname[i], iIconId[i]);
					mHudMngr.AddUser(i, strNickname[i], iIconId[i]);
					continue;
				}
				mSelectTable.SetUserKeyID(i, 1);
				mSelectTable.SetNick(i, string.Empty);
				if (iTableId == mUserInfo.TableId)
				{
					mSeatList.AddPerson(i + 1, string.Empty);
					mHudMngr.AddUser(i, string.Empty);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Error: 桌子不存在");
			result = 1;
		}
		return result;
	}

	public void ShowTableList(int iMode = 0)
	{
		LL_GameTipManager.GetSingleton().HideAllPopupPanel();
		base.transform.localPosition = new Vector3(0f, 0f, -20f);
		mStaticTableObj[0].SetActiveRecursively(state: true);
		mStaticTableObj[1].SetActiveRecursively(state: true);
		mStaticTableObj[2].SetActiveRecursively(state: true);
		mMoveTableObj.SetActiveRecursively(state: false);
		float num;
		float num2;
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			num = (float)Screen.width / (float)Screen.height * 720f;
			num2 = 720f;
		}
		else
		{
			num = 1280f;
			num2 = 720f;
		}
		mStaticTableObj[0].transform.localPosition = new Vector3(-495f * (num / 1280f), 20f, 0f);
		mStaticTableObj[2].transform.localPosition = new Vector3(496.5f * (num / 1280f), 20f, 0f);
		mCurrentTableTran.localPosition = new Vector3(0f, -90f * (num2 / 720f), 0f);
		base.transform.GetComponent<TweenPosition>().enabled = false;
		mCurrentTableTran.GetComponent<TweenPosition>().enabled = false;
		mStaticTableObj[0].transform.GetComponent<TweenPosition>().enabled = false;
		mStaticTableObj[2].transform.GetComponent<TweenPosition>().enabled = false;
		switch (iMode)
		{
		case 0:
			mStepTime = 0f;
			mButtonActive = false;
			mListStep = ETableToSeatStep.TableListMove;
			base.transform.localPosition = new Vector3(1290f, 0f, -20f);
			TweenPosition.Begin(base.transform.gameObject, 0.5f, new Vector3(0f, 0f, -20f));
			break;
		case 1:
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel)
			{
				return;
			}
			mButtonActive = false;
			mStaticTableObj[0].transform.localPosition = new Vector3(-785f * (num / 1280f), 20f, 0f);
			mStaticTableObj[2].transform.localPosition = new Vector3(783.5f * (num / 1280f), 20f, 0f);
			mCurrentTableTran.localPosition = new Vector3(0f, -620f * (num2 / 720f), 0f);
			mListStep = ETableToSeatStep.FlyCausorDown;
			TweenPosition.Begin(mCurrentTableTran.gameObject, 0.5f, new Vector3(0f, -90f * (num2 / 720f), 0f));
			mStepTime = 0f;
			break;
		case 2:
			mButtonActive = true;
			mListStep = ETableToSeatStep.NoneAnimation;
			break;
		default:
			mButtonActive = true;
			mListStep = ETableToSeatStep.NoneAnimation;
			TweenPosition.Begin(mCurrentTableTran.gameObject, 0.5f, new Vector3(0f, -90f * (num2 / 720f), 0f));
			TweenPosition.Begin(mStaticTableObj[0], 0.3f, new Vector3(-495f * (num / 1280f), 20f, 0f));
			TweenPosition.Begin(mStaticTableObj[2], 0.3f, new Vector3(496.5f * (num / 1280f), 20f, 0f));
			mStaticTableObj[0].GetComponent<TweenPosition>().enabled = true;
			mStaticTableObj[2].GetComponent<TweenPosition>().enabled = true;
			mCurrentTableTran.GetComponent<TweenPosition>().enabled = true;
			break;
		}
		mIsTableListViable = true;
		base.transform.GetComponent<UIPanel>().enabled = true;
		mTablePropertyPanel.enabled = true;
		_setColliderAcitve(bIsActive: true);
		mRoomList.HideRoomList();
		updateUserInfo();
	}

	public void HideTableList(int iMode = 0)
	{
		base.transform.GetComponent<TweenPosition>().enabled = false;
		mCurrentTableTran.GetComponent<TweenPosition>().enabled = false;
		mStaticTableObj[0].transform.GetComponent<TweenPosition>().enabled = false;
		mStaticTableObj[2].transform.GetComponent<TweenPosition>().enabled = false;
		switch (iMode)
		{
		case 0:
			TweenPosition.Begin(base.transform.gameObject, 0.5f, new Vector3(1290f, 0f, -20f));
			break;
		case 1:
		{
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
			{
				return;
			}
			mListStep = ETableToSeatStep.TableMove;
			float num;
			if ((float)Screen.width / (float)Screen.height > 1.77777779f)
			{
				num = (float)Screen.width / (float)Screen.height * 720f;
				float num2 = 720f;
			}
			else
			{
				num = 1280f;
				float num2 = 720f;
			}
			TweenPosition.Begin(mStaticTableObj[0], 0.3f, new Vector3(-785f * (num / 1280f), 20f, 0f));
			TweenPosition.Begin(mStaticTableObj[2], 0.3f, new Vector3(783.5f * (num / 1280f), 20f, 0f));
			mStepTime = 0f;
			break;
		}
		case 2:
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
			{
				return;
			}
			mListStep = ETableToSeatStep.NoneAnimation;
			base.transform.GetComponent<UIPanel>().enabled = false;
			break;
		default:
			mListStep = ETableToSeatStep.NoneAnimation;
			base.transform.localPosition = new Vector3(1290f, 0f, -20f);
			base.transform.GetComponent<UIPanel>().enabled = false;
			mTablePropertyPanel.enabled = false;
			break;
		}
		mButtonActive = false;
		mIsTableListViable = false;
		_setColliderAcitve(bIsActive: false);
	}

	protected void _doSelectingTable()
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel || mAnimationStep < 0)
		{
			return;
		}
		if (mAnimationTime >= 0.2f)
		{
			if (mAnimationStep == 0)
			{
				mAnimationStep = 1;
				mStaticTableObj[0].SetActiveRecursively(state: false);
				mStaticTableObj[1].SetActiveRecursively(state: false);
				mStaticTableObj[2].SetActiveRecursively(state: false);
				mMoveTableObj.SetActiveRecursively(state: true);
			}
			else if (mAnimationStep == 1)
			{
				LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.ChangeTable);
				mAnimationStep = 2;
				mStaticTableObj[0].SetActiveRecursively(state: true);
				mStaticTableObj[1].SetActiveRecursively(state: true);
				mStaticTableObj[2].SetActiveRecursively(state: true);
				mMoveTableObj.SetActiveRecursively(state: false);
			}
			else if (mAnimationStep == 2)
			{
				mAnimationStep = -1;
				_updateRistrictText();
				mTablePropertyPanel.enabled = true;
			}
			mAnimationTime = 0f;
		}
		mAnimationTime += Time.deltaTime;
	}

	protected void _doTableToSeat()
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table || mListStep == ETableToSeatStep.NoneAnimation)
		{
			return;
		}
		mStepTime += Time.deltaTime;
		if (mListStep == ETableToSeatStep.TableMove)
		{
			if (mStepTime >= 0.3f)
			{
				mStepTime = 0f;
				mListStep = ETableToSeatStep.FlyCausorDown;
				float num2;
				if ((float)Screen.width / (float)Screen.height > 1.77777779f)
				{
					float num = (float)Screen.width / (float)Screen.height * 720f;
					num2 = 720f;
				}
				else
				{
					float num = 1280f;
					num2 = 720f;
				}
				TweenPosition.Begin(mCurrentTableTran.gameObject, 0.5f, new Vector3(0f, -620f * (num2 / 720f), 0f));
			}
		}
		else if (mListStep == ETableToSeatStep.FlyCausorDown && mStepTime >= 0.5f)
		{
			HideTableList(2);
			mSeatList.ShowSeatList(1);
		}
	}

	protected void _doSeatToTable()
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel || mListStep == ETableToSeatStep.NoneAnimation)
		{
			return;
		}
		mStepTime += Time.deltaTime;
		if (mListStep == ETableToSeatStep.TableMove)
		{
			if (mStepTime >= 0.5f)
			{
				mButtonActive = true;
				mListStep = ETableToSeatStep.NoneAnimation;
			}
		}
		else if (mListStep == ETableToSeatStep.FlyCausorDown && mStepTime >= 0.5f)
		{
			mListStep = ETableToSeatStep.TableMove;
			float num;
			if ((float)Screen.width / (float)Screen.height > 1.77777779f)
			{
				num = (float)Screen.width / (float)Screen.height * 720f;
				float num2 = 720f;
			}
			else
			{
				num = 1280f;
				float num2 = 720f;
			}
			TweenPosition.Begin(mStaticTableObj[0], 0.3f, new Vector3(-495f * (num / 1280f), 20f, 0f));
			TweenPosition.Begin(mStaticTableObj[2], 0.3f, new Vector3(496.5f * (num / 1280f), 20f, 0f));
			mStepTime = 0f;
		}
	}

	protected void _updateRistrictText()
	{
		mTableName0.text = mSelectTable.TableName;
		mTableName1.text = mSelectTable.TableName;
		if (LL_GameInfo.getInstance().Language == 1)
		{
			mMinCoin.text = ((mUserInfo.RoomId == 0) ? ("Limit: More than " + mSelectTable.Ristrict + " tickets") : ("Limit: More than " + mSelectTable.Ristrict + " coins"));
			mMinBet.text = "Min bet: " + mSelectTable.MinBet + " credits";
			mMaxBet.text = "Max bet: " + mSelectTable.MaxBet + " credits";
			mExchange.text = ((mUserInfo.RoomId == 0) ? ("TicketValue: " + mSelectTable.CreditPerCoin + " credits/ticket") : ("CoinValue: " + mSelectTable.CreditPerCoin + " credits/coin"));
		}
		else
		{
			mMinCoin.text = ((mUserInfo.RoomId == 0) ? ("体验币值：大于" + mSelectTable.Ristrict + "币") : ("游戏币值：大于" + mSelectTable.Ristrict + "币"));
			mMinBet.text = "最小押注：" + mSelectTable.MinBet + "分";
			mMaxBet.text = "最大押注：" + mSelectTable.MaxBet + "分";
			mExchange.text = "一币分值：" + mSelectTable.CreditPerCoin + "分/币";
		}
		mPersonCount.text = mSelectTable.PersonCount + "/8";
		int num = (mTableInfoList.Count > 10) ? Mathf.FloorToInt(10f / (float)mTableInfoList.Count * (float)mCurrentTableId) : mCurrentTableId;
		for (int i = 0; i < dotList.Count; i++)
		{
			if (i == num)
			{
				dotList[i].spriteName = "d2";
			}
			else
			{
				dotList[i].spriteName = "d";
			}
		}
		for (int j = 0; j < grid.transform.childCount; j++)
		{
			if (j == mCurrentTableId)
			{
				grid.transform.Find("tableListItem" + j).GetComponentInChildren<UISprite>().spriteName = "x12";
			}
			else
			{
				grid.transform.Find("tableListItem" + j).GetComponentInChildren<UISprite>().spriteName = "x11";
			}
		}
	}

	protected void _setColliderAcitve(bool bIsActive)
	{
		mBackgroundCol.enabled = bIsActive;
		mBackCol.enabled = bIsActive;
		mSelectTableCol.enabled = bIsActive;
		mToLeftCol.enabled = bIsActive;
		mToRightCol.enabled = bIsActive;
	}

	public void _clickTable()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		if (!mButtonActive || LL_GameInfo.getInstance().UserInfo.TableId != -1 || LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel)
		{
			return;
		}
		LL_GameInfo.getInstance().UserInfo.TableId = mSelectTable.TableServerID;
		if (!LL_MyTest.TEST)
		{
			isShrinkTableList = true;
			menuArrow.spriteName = "xl_sjx2";
			LL_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(LL_GameInfo.getInstance().UserInfo.RoomId, mSelectTable.TableServerID);
			return;
		}
		for (int i = 0; i < 8; i++)
		{
			mSeatList.AddPerson(i + 1, mSelectTable.GetNick(i), mSelectTable.GetIcon(i));
		}
		LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Table);
		mSeatList.ShowSeatList();
	}

	public LL_TableInfo GetTableFromGrid(int iTableId, out int index, int iRoomId = 1)
	{
		LL_TableInfo result = null;
		int i;
		for (i = 0; i < mTableInfoList.Count; i++)
		{
			if (((LL_TableInfo)mTableInfoList[i]).RoomId == iRoomId && ((LL_TableInfo)mTableInfoList[i]).TableServerID == iTableId)
			{
				result = (LL_TableInfo)mTableInfoList[i];
				break;
			}
		}
		index = i;
		return result;
	}

	public void OnClickBackToRoom()
	{
		panel.clipOffset = new Vector2(0f, 0f);
		panel.transform.localPosition = new Vector3(0f, 0f, -5f);
		Vector4 clipRange = panel.clipRange;
		if (clipRange.w > 10f)
		{
			isShrinkTableList = true;
			menuArrow.spriteName = "xl_sjx2";
		}
		if (mButtonActive && LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_TableList_Panel)
		{
			mRoomList.ShowRoomList(1);
			LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
			LL_GameInfo.getInstance().UserInfo.RoomId = -1;
			LL_GameInfo.getInstance().UserInfo.TableId = -1;
			HideTableList();
			mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
			mTablePropertyPanel.enabled = false;
			GetComponent<UIPanel>().enabled = false;
		}
	}

	private void _onPressTable()
	{
		Vector4 clipRange = panel.clipRange;
		if (clipRange.w > 10f)
		{
			isShrinkTableList = true;
			menuArrow.spriteName = "xl_sjx2";
		}
	}

	protected void _onReleaseTable()
	{
		Vector2 totalDelta = UICamera.currentTouch.totalDelta;
		UnityEngine.Debug.LogError(totalDelta.x + "," + totalDelta.y);
		if (Mathf.Abs(totalDelta.x) >= 160f)
		{
			_dragTable(mCurrentTableTran.gameObject, totalDelta);
			mIsDraging = false;
		}
		else
		{
			_clickTable();
		}
	}

	protected void _releaseFromBg(GameObject go, bool isPress)
	{
		if (!isPress)
		{
			mIsDraging = false;
			return;
		}
		Vector4 clipRange = panel.clipRange;
		if (clipRange.w > 10f)
		{
			isShrinkTableList = true;
			menuArrow.spriteName = "xl_sjx2";
		}
	}

	protected void _dragTable(GameObject go, Vector2 delt)
	{
		if (mIsDraging || !mButtonActive)
		{
			return;
		}
		if (delt.x < 0f)
		{
			mCurrentTableId++;
			if (mCurrentTableId >= mTableInfoList.Count)
			{
				mCurrentTableId = 0;
			}
		}
		else
		{
			mCurrentTableId--;
			if (mCurrentTableId < 0)
			{
				mCurrentTableId = mTableInfoList.Count - 1;
			}
		}
		mAnimationTime = 0f;
		mAnimationStep = 0;
		mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
		mTablePropertyPanel.enabled = false;
		mIsDraging = true;
	}

	public void _onClickRightButton()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		mCurrentTableId--;
		if (mCurrentTableId < 0)
		{
			mCurrentTableId = mTableInfoList.Count - 1;
		}
		mAnimationTime = 0f;
		mAnimationStep = 0;
		mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
		mTablePropertyPanel.enabled = false;
	}

	public void _onClickLeftButton()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		mCurrentTableId++;
		if (mCurrentTableId >= mTableInfoList.Count)
		{
			mCurrentTableId = 0;
		}
		mAnimationTime = 0f;
		mAnimationStep = 0;
		mSelectTable = (LL_TableInfo)mTableInfoList[mCurrentTableId];
		mTablePropertyPanel.enabled = false;
	}
}
