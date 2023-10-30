using com.miracle9.game.bean;
using DG.Tweening;
using LitJson;
using LL_GameCommon;
using LL_UICommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LL_AppUIMngr : LL_Observer
{
	public GameObject u_Canvas;

	public GameObject u_Camera;

	public LL_GameTipManager_Canvas lL_GameTipManager_Canvas;

	public List<Sprite> icoSprite = new List<Sprite>();

	private Transform Content;

	private Transform ContentOldPos;

	private Transform ContentTagPos;

	private Transform scrollView;

	private Button btnLeft;

	private Button btnRight;

	public GameObject tablePre;

	public LL_HudManager mHudPanel;

	public Image imaIco;

	[HideInInspector]
	public List<GameObject> tableList = new List<GameObject>();

	public CircleScrollRect circleScrollRect;

	public List<RotateBtnInfo> selectBtnList = new List<RotateBtnInfo>();

	public GameObject RotateButton_new;

	public static LL_AppUIMngr G_AppUIMngr;

	public LL_RoomList mRoomList;

	public LL_OtherUserInfoPanel mOtherUserInfoPanel;

	public LL_TableList mTableList;

	public LL_SeatList mSeatList;

	public LL_BetPanel mBetPanel;

	public LL_HudManager mHudManager;

	public LL_PrizeResult mPrizeResult;

	public LL_HistoryRecord mHistoryRecord;

	public LL_ChatPanel mChatPanel;

	private AppState m_appState = AppState.App_On_RoomList_Panel;

	private Camera _gameCam;

	private bool isFirstTimeEnterRoom = true;

	private bool isAttach;

	private float fLastTouchTime;

	public JsonData hallInfo = new JsonData();

	private bool isOnEnter = true;

	public int tempSelectId = -1;

	private float contentanchoredPositionX;

	private RectTransform contentRectTransform;

	private BYSD_TwoContentSizeCtrl bYSD_TwoContentSize;

	private int currRoomId;

	private LL_Desk[] allTableArr;

	public static LL_AppUIMngr GetSingleton()
	{
		return G_AppUIMngr;
	}

	private void Awake()
	{
		scrollView = u_Canvas.transform.Find("Tables_LL/Scroll View");
		Content = scrollView.Find("Viewport/Content");
		ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
		ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
		btnLeft = scrollView.Find("Viewport/Buttons/LeftBtn").GetComponent<Button>();
		btnRight = scrollView.Find("Viewport/Buttons/RightBtn").GetComponent<Button>();
		bYSD_TwoContentSize = Content.GetComponent<BYSD_TwoContentSizeCtrl>();
		contentRectTransform = Content.GetComponent<RectTransform>();
		btnLeft.onClick.AddListener(delegate
		{
			LeftAndRightBtnClick(isLeft: true);
		});
		btnRight.onClick.AddListener(delegate
		{
			LeftAndRightBtnClick(isLeft: false);
		});
	}

	public void OnEnable1()
	{
		tempSelectId = -1;
		isOnEnter = true;
		Transform transform = u_Canvas.transform.Find("RotateButton_new");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(RotateButton_new, u_Canvas.transform);
		gameObject.name = "RotateButton_new";
		circleScrollRect = gameObject.transform.Find("Button").GetComponent<CircleScrollRect>();
		hallInfo = new JsonData();
		selectBtnList = new List<RotateBtnInfo>();
		for (int i = 0; i < circleScrollRect.listItems.Length; i++)
		{
			selectBtnList.Add(circleScrollRect.listItems[i].GetComponent<RotateBtnInfo>());
		}
		if (ZH2_GVars.hallInfo2 != null && hallInfo != ZH2_GVars.hallInfo2)
		{
			ShowHall();
			hallInfo = ZH2_GVars.hallInfo2;
		}
	}

	private void Start()
	{
		init();
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void CloseSafeBox()
	{
		LL_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn();
	}

	private void SaveScore()
	{
		LL_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut();
	}

	private void OnDisable()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void LeftAndRightBtnClick(bool isLeft)
	{
		if (isLeft)
		{
			RectTransform target = contentRectTransform;
			Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
			target.DOLocalMoveX(anchoredPosition.x + 994f, 0.35f);
		}
		else
		{
			RectTransform target2 = contentRectTransform;
			Vector2 anchoredPosition2 = contentRectTransform.anchoredPosition;
			target2.DOLocalMoveX(anchoredPosition2.x - 994f, 0.35f);
		}
	}

	public void SetBtnLeft(bool isInteractable)
	{
		btnLeft.interactable = isInteractable;
	}

	public void SetBtnRight(bool isInteractable)
	{
		btnRight.interactable = isInteractable;
	}

	private void Update()
	{
		if (tempSelectId != ZH2_GVars.selectRoomId)
		{
			tempSelectId = ZH2_GVars.selectRoomId;
			StartCoroutine(ClickBtnRoom(tempSelectId));
		}
		if (Content != null && contentRectTransform != null)
		{
			Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
			contentanchoredPositionX = anchoredPosition.x;
			if (UnityEngine.Input.GetKeyDown(KeyCode.H))
			{
				UnityEngine.Debug.LogError("content: " + contentanchoredPositionX);
			}
			if (Mathf.Abs(contentanchoredPositionX) <= 1f)
			{
				SetBtnLeft(isInteractable: false);
			}
			else if (allTableArr != null && allTableArr.Length > 4)
			{
				SetBtnLeft(isInteractable: true);
			}
			float num = Mathf.Abs(contentanchoredPositionX);
			Vector2 sizeDelta = bYSD_TwoContentSize.content.sizeDelta;
			if (num >= Mathf.Abs(sizeDelta.x) - 1f)
			{
				SetBtnRight(isInteractable: false);
			}
			else if (allTableArr != null && allTableArr.Length > 4)
			{
				SetBtnRight(isInteractable: true);
			}
		}
		if (!isAttach)
		{
			isAttach = true;
			LL_GameMsgMngr.GetSingleton().BackBtnMsgAttach(this);
			LL_GameMsgMngr.GetSingleton().QuitMsgAttach(this);
		}
		BackInputCtrl();
	}

	private IEnumerator ClickBtnRoom(int index)
	{
		SetNoHall(isHavHall: true);
		Transform content = Content;
		Vector3 localPosition = ContentTagPos.localPosition;
		content.DOLocalMoveY(localPosition.y, 0.2f);
		Content.GetComponent<DK_DislodgShader>().SetImaAndText(0.2f);
		switch (index)
		{
		case 0:
			index = 1;
			break;
		case 1:
			index = 2;
			break;
		case 2:
			index = 4;
			break;
		case 3:
			index = 0;
			break;
		case 4:
			index = 3;
			break;
		default:
			index = 0;
			break;
		}
		if ((bool)mRoomList && ZH2_GVars.isShowTingName)
		{
			mRoomList.gameName.text = selectBtnList[index].name;
		}
		GoToRoom();
		float Time = (!isOnEnter) ? 0.25f : 0.05f;
		isOnEnter = false;
		yield return new WaitForSeconds(Time);
		currRoomId = index;
		int id = selectBtnList[index].hallId;
		int roomId = selectBtnList[index].hallType;
		if (roomId <= 0)
		{
			mRoomList.coinCount.gameObject.SetActive(value: false);
			mRoomList.coinTestCount.gameObject.SetActive(value: true);
		}
		else
		{
			mRoomList.coinCount.gameObject.SetActive(value: true);
			mRoomList.coinTestCount.gameObject.SetActive(value: false);
		}
		LL_GameInfo.getInstance().UserInfo.RoomId = roomId;
		UnityEngine.Debug.LogError("========当前厅========: " + roomId + " hallId: " + id);
		LL_NetMngr.GetSingleton().MyCreateSocket.SendEnterHall(id);
	}

	public void ShowHall()
	{
		JsonData jsonData = new JsonData();
		jsonData = ZH2_GVars.hallInfo2;
		UnityEngine.Debug.LogError("房间导航栏信息: " + jsonData.ToJson());
		if (selectBtnList.Count <= 0)
		{
			UnityEngine.Debug.LogError("未获取完毕");
			return;
		}
		for (int i = 0; i < jsonData.Count; i++)
		{
			selectBtnList[i].hallId = (int)jsonData[i.ToString()]["hallId"];
			selectBtnList[i].hallType = (int)jsonData[i.ToString()]["roomId"] - 1;
			selectBtnList[i].name = jsonData[i.ToString()]["hallName"].ToString();
			selectBtnList[i].minGlod = jsonData[i.ToString()]["minGold"].ToString();
			selectBtnList[i].onlinePeople = "0";
			selectBtnList[i].UpdateText();
		}
	}

	public void SetNoHall(bool isHavHall)
	{
	}

	public void GoToRoom()
	{
	}

	public void InItTable(LL_Desk[] tableArr, int roomId)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			UnityEngine.Object.Destroy(tableList[i].gameObject);
		}
		Transform content = Content;
		Vector3 localPosition = ContentOldPos.localPosition;
		content.DOLocalMoveY(localPosition.y, 0.2f);
		Content.GetComponent<DK_DislodgShader>().SetOver();
		tableList = new List<GameObject>();
		if (tableArr == null)
		{
			SetBtnLeft(isInteractable: false);
			SetBtnRight(isInteractable: false);
			return;
		}
		allTableArr = new LL_Desk[tableArr.Length];
		allTableArr = tableArr;
		for (int j = 0; j < allTableArr.Length; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(tablePre, Content);
			LongPressBtn3 component = gameObject.GetComponent<LongPressBtn3>();
			if (component == null)
			{
				component = gameObject.AddComponent<LongPressBtn3>();
			}
			tableList.Add(gameObject);
		}
		for (int k = 0; k < tableList.Count; k++)
		{
			for (int l = 0; l < tableList[k].transform.Find("Ico").childCount; l++)
			{
				tableList[k].transform.Find("Ico/Image" + l).GetComponent<Image>().sprite = icoSprite[0];
				tableList[k].transform.Find("Ico/Image" + l + "/Btn").GetComponent<Button>().enabled = true;
			}
			int num = k;
			for (int m = 0; m < allTableArr[k].seats.Length; m++)
			{
				if (!allTableArr[k].seats[m].isFree)
				{
					int num2 = allTableArr[k].seats[m].photoId;
					int num3 = allTableArr[k].seats[m].id - 1;
					if (num2 <= 0 || num2 >= icoSprite.Count)
					{
						num2 = 1;
					}
					try
					{
						tableList[k].transform.Find("Ico/Image" + num3).GetComponent<Image>().sprite = icoSprite[num2];
						tableList[k].transform.Find("Ico/Image" + num3 + "/Btn").GetComponent<Button>().enabled = false;
					}
					catch (Exception)
					{
						throw;
					}
				}
			}
			tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
			Destroy(tableList[k].transform.Find("Inifo/min").GetComponent<Translation_Game>());
			Destroy(tableList[k].transform.Find("Inifo/Max").GetComponent<Translation_Game>());
			tableList[k].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip("最小押注: ", "MiniBet: ", string.Empty, "Tối thiểu:");
			tableList[k].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[k].minBet.ToString();
			tableList[k].transform.Find("Inifo/Max").GetComponent<Text>().text = ZH2_GVars.ShowTip("最大押注: ", "MaxBet: ", string.Empty, "Tối đa:");
			tableList[k].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[k].maxBet.ToString();
		}
		for (int n = 0; n < tableList.Count; n++)
		{
			int index = n;
			for (int num4 = 0; num4 < 8; num4++)
			{
				int deskid = num4;
				Transform transform = tableList[index].transform.Find("Ico/Image" + num4 + "/Btn");
				transform.GetComponent<Button>()?.onClick.AddListener(delegate
				{
					ClickBtnArrow(allTableArr[index].id, deskid + 1, index);
				});
				LongPressBtn3 longPressBtn = transform.GetComponent<LongPressBtn3>();
				if (longPressBtn == null)
				{
					longPressBtn = transform.gameObject.AddComponent<LongPressBtn3>();
				}
				longPressBtn.onClickUp.AddListener(delegate
				{
				});
			}
		}
	}

	public void UpdateSeat(Seat[] netSeats, int deskId)
	{
		if (!base.gameObject || allTableArr == null)
		{
			return;
		}
		for (int i = 0; i < allTableArr.Length; i++)
		{
			if (allTableArr[i].id != deskId)
			{
				continue;
			}
			UnityEngine.Debug.LogError("更新座位信息: " + JsonMapper.ToJson(netSeats));
			allTableArr[i].seats = netSeats;
			for (int j = 0; j < netSeats.Length; j++)
			{
				tableList[i].transform.Find("Ico/Image" + j).GetComponent<Image>().sprite = icoSprite[0];
				tableList[i].transform.Find("Ico/Image" + j + "/Btn").GetComponent<Button>().enabled = true;
			}
			for (int k = 0; k < netSeats.Length; k++)
			{
				if (!allTableArr[i].seats[k].isFree)
				{
					int num = allTableArr[i].seats[k].photoId;
					int num2 = allTableArr[i].seats[k].id - 1;
					if (num <= 0 || num >= icoSprite.Count)
					{
						num = 1;
					}
					tableList[i].transform.Find("Ico/Image" + num2).GetComponent<Image>().sprite = icoSprite[num];
					tableList[i].transform.Find("Ico/Image" + num2 + "/Btn").GetComponent<Button>().enabled = false;
				}
			}
		}
	}

	public void ClickBtnTable(int num, int index)
	{
		mRoomList.updateUserInfo(num);
		UnityEngine.Debug.LogError("点击了: " + num + "  " + index);
		LL_GameInfo.getInstance().UserInfo.TableId = num;
		try
		{
			mTableList.mSelectTable = (LL_TableInfo)mTableList.mTableInfoList[index];
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(LL_GameInfo.getInstance().UserInfo.RoomId, num);
	}

	public void InSeat(Seat[] netSeats)
	{
		UnityEngine.Debug.LogError("===阻止自动选桌子===");
	}

	private void ClickBtnArrow(int index)
	{
		mHudPanel.SetCurrentPosition(LL_GameInfo.getInstance().UserInfo.RoomId, mTableList.SelectedTable);
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(LL_GameInfo.getInstance().UserInfo.TableId, index);
	}

	private void ClickBtnArrow(int deskId, int index, int deskIndex)
	{
		string minGlod = selectBtnList[currRoomId].minGlod;
		UnityEngine.Debug.LogError("=====当前房最低携带: " + minGlod);
		int num = 0;
		int num2 = 0;
		try
		{
			num = int.Parse(minGlod);
			num2 = ((LL_GameInfo.getInstance().UserInfo.RoomId != 1) ? int.Parse(mRoomList.coinTestCount.text) : int.Parse(mRoomList.coinCount.text));
		}
		catch
		{
		}
		if (num2 < num)
		{
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_CreditBelowRistrict, string.Empty);
			return;
		}
		LL_GameInfo.getInstance().UserInfo.TableId = deskId;
		try
		{
			mTableList.mSelectTable = (LL_TableInfo)mTableList.mTableInfoList[deskIndex];
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		mHudPanel.SetCurrentPosition(LL_GameInfo.getInstance().UserInfo.RoomId, mTableList.SelectedTable);
		LL_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(LL_GameInfo.getInstance().UserInfo.RoomId, deskId);
		UnityEngine.Debug.LogError("TableId: " + deskId + "  座位ID: " + index);
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(LL_GameInfo.getInstance().UserInfo.TableId, index);
	}

	public void EnterGame()
	{
		ZH2_GVars.isCanSenEnterGame = true;
	}

	private IEnumerator WaitStart()
	{
		while (ZH2_GVars.isCanSenEnterGame)
		{
			yield return new WaitForSeconds(2.5f);
			try
			{
				WebSocket2.GetInstance().SenEnterGame(isEnterGame: true, ZH2_GVars.GameType.desk, LL_GameInfo.getInstance().UserInfo.TableId.ToString(), LL_GameInfo.getInstance().UserInfo.GameScore.ToString());
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}
	}

	public void SetAppState(AppState state)
	{
		switch (state)
		{
		case AppState.App_On_Loading:
			_gameCam.enabled = false;
			break;
		case AppState.App_On_RoomList_Panel:
			_gameCam.enabled = false;
			break;
		case AppState.App_On_TableList_Panel:
			_gameCam.enabled = false;
			break;
		case AppState.App_On_Table:
			_gameCam.enabled = false;
			break;
		case AppState.App_On_Game:
			_gameCam.enabled = true;
			mHudManager.ShowHud();
			fLastTouchTime = 0f;
			LL_GameMngr.GetSingleton().ResetLuckySpinOffset();
			break;
		}
		m_appState = state;
	}

	public AppState GetAppState(AppState state)
	{
		return m_appState;
	}

	private void init()
	{
		if (G_AppUIMngr == null)
		{
			G_AppUIMngr = this;
		}
		mOtherUserInfoPanel = GameObject.Find("OtherUserInfoPanel").GetComponent<LL_OtherUserInfoPanel>();
		mTableList = GameObject.Find("TableListPanel").GetComponent<LL_TableList>();
		mSeatList = GameObject.Find("SeatPanel").GetComponent<LL_SeatList>();
		mBetPanel = GameObject.Find("BetPanel").GetComponent<LL_BetPanel>();
		mHudManager = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		mPrizeResult = GameObject.Find("ResultPanel").GetComponent<LL_PrizeResult>();
		mHistoryRecord = GameObject.Find("RecordPanel").GetComponent<LL_HistoryRecord>();
		mChatPanel = GameObject.Find("ChatPanel").GetComponent<LL_ChatPanel>();
		_gameCam = GameObject.Find("MainCamera").GetComponent<Camera>();
	}

	public AppState GetAppState()
	{
		return m_appState;
	}

	public void OnUpdate(float time)
	{
	}

	public override void OnRcvMsg(int type, object obj)
	{
		switch (type)
		{
		case 0:
			switch (m_appState)
			{
			case AppState.App_On_RoomList_Panel:
				SetAppState(AppState.App_On_Quit);
				break;
			case AppState.App_On_TableList_Panel:
				SetAppState(AppState.App_On_RoomList_Panel);
				break;
			case AppState.App_On_Table:
				SetAppState(AppState.App_On_TableList_Panel);
				break;
			case AppState.App_On_Game:
				LL_GameMngr.GetSingleton().Reset();
				_gameCam.enabled = false;
				SetAppState(AppState.App_On_TableList_Panel);
				break;
			case AppState.APP_NET_ERROR:
				LL_GameMngr.GetSingleton().Reset();
				SetAppState(AppState.App_On_Quit);
				break;
			}
			break;
		case 1:
		{
			int num = (int)obj;
			if (num == 1 && m_appState != AppState.App_On_RoomList_Panel)
			{
				LL_GameMngr.GetSingleton().Reset();
				SetAppState(AppState.App_On_RoomList_Panel);
			}
			break;
		}
		}
	}

	public void clickBack()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.IsExitApplication, string.Empty);
	}

	public void BackInputCtrl()
	{
	}
}
