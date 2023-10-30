using DG.Tweening;
using JsonFx.Json;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Message;
using M__M.HaiWang.NetMsgDefine;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_GameUIController : FK3_MB_Singleton<FK3_GameUIController>
	{
		private int roomShowVecX = 270;

		private int roomCloseVecX = 890;

		public int roomId = -1;

		public FK3_LobbyContext myLobbyContext = new FK3_LobbyContext();

		[HideInInspector]
		public List<GameObject> tableList = new List<GameObject>();

		public CircleScrollRect circleScrollRect;

		public List<RotateBtnInfo> selectBtnList = new List<RotateBtnInfo>();

		public GameObject noHallTip;

		public Sprite[] _icon;

		private Transform Content;

		private Transform ContentOldPos;

		private Transform ContentTagPos;

		private Transform scrollView;

		private Button btnLeft;

		private Button btnRight;

		public GameObject tablePre;

		public GameObject RotateButton;

		public GameObject NoTips;

		[SerializeField]
		private Transform tfTableInfo;

		private JsonData hallInfo = new JsonData();

		private float contentanchoredPositionX;

		private RectTransform contentRectTransform;

		private BYSD_TwoContentSizeCtrl bYSD_TwoContentSize;

		private bool isInitOk;

		private int tempSelectId = -1;

		private bool isOnEnter = true;

		private List<FK3_DeskInfo> allTableArr = new List<FK3_DeskInfo>();

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_GameUIController>._instance == null)
			{
				FK3_MB_Singleton<FK3_GameUIController>.SetInstance(this);
			}
			else
			{
				UnityEngine.Debug.LogError("GameUIController已经存在");
			}
			scrollView = base.transform.Find("Normal/Tables/Scroll View");
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
			HideGameObjects();
			NoTips.SetActive(value: false);
			NoTips.transform.localScale = Vector3.zero;
		}

		private void Start()
		{
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("enterRoom", HandleNetMsg_EnterRoom);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("selectHall", HandleNetMsg_SelectHall);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("updateHallInfo", HandleNetMsg_SelectHall);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("requestSeat", HandleNetMsg_RequestSeat);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("leaveRoom", HandleNetMsg_LeaveRoom);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("updateSeatPush", HandleNetMsg_UpdateSeat);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("updateRoomInfoPush", HandleNetMsg_UpdateRoomInfo);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("addExpeGoldAuto", HandleNetMsg_addExpeGoldAuto);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("overflowPush", HandleNetMsg_Overflow);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userAwardPush", HandleNetMsg_UserAward);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("scrollMessagePush", HandleNetMsg_ScrollMessage);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gameShutupPush", HandleNetMsg_GameShutup);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userShutupPush", HandleNetMsg_UserShutup);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("playerInfo", HandleNetMsg_PlayerInfo);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("playerAddLevelPush", HandleNetMsg_PlayerAddLevel);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
			try
			{
				FK3_GVars.SetLobbyContext(myLobbyContext);
				FK3_GVars.SetUserInfo(FK3_GVars.lobby.user);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		private void OnEnable()
		{
			try
			{
				if (FK3_GVars.m_curState != FK3_Demo_UI_State.StartupLoading)
				{
					UnityEngine.Debug.LogError("初始化开始");
					try
					{
						FK3_UserInfoShow.Get().Init();
						UnityEngine.Debug.LogError("初始化完成");
					}
					catch (Exception message)
					{
						UnityEngine.Debug.LogError(message);
					}
				}
				if (FK3_MB_Singleton<FK3_NetManager>.Get() != null)
				{
					FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			tempSelectId = -1;
			Transform transform = base.transform.Find("Normal");
			Transform transform2 = transform.Find("RotateButton");
			if (transform2 != null)
			{
				UnityEngine.Object.Destroy(transform2.gameObject);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(RotateButton, transform);
			gameObject.name = "RotateButton";
			circleScrollRect = gameObject.transform.Find("Button").GetComponent<CircleScrollRect>();
			hallInfo = new JsonData();
			selectBtnList = new List<RotateBtnInfo>();
			for (int i = 0; i < circleScrollRect.listItems.Length; i++)
			{
				selectBtnList.Add(circleScrollRect.listItems[i].GetComponent<RotateBtnInfo>());
			}
			try
			{
				if (ZH2_GVars.hallInfo2 != null && ZH2_GVars.hallInfo2.Count > 0)
				{
					ShowHall();
				}
			}
			catch (Exception)
			{
			}
		}

		public void RefreshGameUI(FK3_Demo_UI_State state)
		{
			FK3_GVars.m_curState = state;
			isInitOk = true;
			switch (state)
			{
			case FK3_Demo_UI_State.RoomSelection:
				UpdateUI_UserInfo();
				break;
			case FK3_Demo_UI_State.DeskSelection:
				UpdateUI_Desk();
				break;
			}
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
				else if (allTableArr != null && allTableArr.Count > 4)
				{
					SetBtnLeft(isInteractable: true);
				}
				float num = Mathf.Abs(contentanchoredPositionX);
				Vector2 sizeDelta = bYSD_TwoContentSize.content.sizeDelta;
				if (num >= Mathf.Abs(sizeDelta.x) - 1f)
				{
					SetBtnRight(isInteractable: false);
				}
				else if (allTableArr != null && allTableArr.Count > 4)
				{
					SetBtnRight(isInteractable: true);
				}
			}
			if (tempSelectId != ZH2_GVars.selectRoomId && isInitOk)
			{
				tempSelectId = ZH2_GVars.selectRoomId;
				StartCoroutine(ClickBtnRoom(tempSelectId));
			}
		}

		private IEnumerator ClickBtnRoom(int index)
		{
			SetNoHall(isHavHall: true);
			HideGameObjects();
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
			if (FK3_UserInfoShow.Get() != null && FK3_UserInfoShow.Get()._txtLogo != null)
			{
				FK3_UserInfoShow.Get()._txtLogo.text = selectBtnList[index].name;
			}
			GoToRoom();
			float Time = (!isOnEnter) ? 0.25f : 0.05f;
			isOnEnter = false;
			yield return new WaitForSeconds(Time);
			int id = selectBtnList[index].hallId;
			int roomId = selectBtnList[index].hallType;
			if (FK3_UserInfoShow.Get() != null && FK3_UserInfoShow.Get()._userGameGlodText != null && FK3_UserInfoShow.Get()._userExpText != null)
			{
				if (roomId <= 1)
				{
					FK3_UserInfoShow.Get()._userGameGlodText.gameObject.SetActive(value: false);
					FK3_UserInfoShow.Get()._userExpText.gameObject.SetActive(value: true);
				}
				else
				{
					FK3_UserInfoShow.Get()._userGameGlodText.gameObject.SetActive(value: true);
					FK3_UserInfoShow.Get()._userExpText.gameObject.SetActive(value: false);
				}
			}
			FK3_GVars.lobby.curRoomId = roomId;
			FK3_GVars.roomId = roomId;
			FK3_GVars.hallId = id;
			if (FK3_RoomMgr.GetInstance() != null)
			{
				FK3_RoomMgr.GetInstance().RoomId = roomId;
			}
			UnityEngine.Debug.LogError("========当前厅========: " + FK3_GVars.roomId + " hallId: " + FK3_GVars.hallId);
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
			FK3_RoomMgr.GetInstance().SendEnterHall(FK3_GVars.hallId);
		}

		public void ShowHall()
		{
			if (selectBtnList.Count <= 0)
			{
				UnityEngine.Debug.LogError("未获取完毕");
			}
			else if (ZH2_GVars.hallInfo2 != null && hallInfo != ZH2_GVars.hallInfo2)
			{
				hallInfo = ZH2_GVars.hallInfo2;
				for (int i = 0; i < hallInfo.Count; i++)
				{
					selectBtnList[i].hallId = (int)hallInfo[i.ToString()]["hallId"];
					selectBtnList[i].hallType = (int)hallInfo[i.ToString()]["roomId"];
					selectBtnList[i].name = hallInfo[i.ToString()]["hallName"].ToString();
					selectBtnList[i].minGlod = hallInfo[i.ToString()]["minGold"].ToString();
					selectBtnList[i].onlinePeople = "0";
					selectBtnList[i].UpdateText();
				}
			}
		}

		public void SetNoHall(bool isHavHall)
		{
			noHallTip.SetActive(!isHavHall);
		}

		private void UpdateRoomInfo()
		{
			for (int i = 0; i < tableList.Count; i++)
			{
				UnityEngine.Object.Destroy(tableList[i].gameObject);
			}
			tableList = new List<GameObject>();
		}

		private void GoToRoom()
		{
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.DeskSelection)
			{
				FK3_TableController.Get().itemList.Clear();
			}
		}

		public void OnBtnBack_Click()
		{
			UnityEngine.Debug.LogError("点击返回  当前状态: " + FK3_GVars.m_curState);
			QuitToHallGame();
		}

		private void UpdateUI_ReadyInGame(int sceneId, int stageId, int stageType, int sceneDuration, int stageDuration, int gScore)
		{
			FK3_DeskMgr.Get().mask.gameObject.SetActive(value: false);
			FK3_MB_Singleton<FK3_EnterLobbyLogic>.Get().DisableScene();
			FK3_AppSceneMgr.RunAction("Game.EnterGame", new object[6]
			{
				sceneId,
				stageId,
				stageType,
				sceneDuration,
				stageDuration,
				gScore
			}, delegate
			{
			});
		}

		private void UpdateUI_UserInfo()
		{
			FK3_UserInfoShow.Get().Init();
			FK3_UIRoomManager.GetInstance().CloseUI("Login");
			FK3_UIRoomManager.GetInstance().CloseUI("NoDeskImage");
			ShowHall();
		}

		private void UpdateUI_Desk()
		{
			FK3_UserInfoShow.Get().Init();
			FK3_MessageCenter.SendMessage("RoomMove", new FK3_KeyValueInfo("moveVecx", roomCloseVecX));
			FK3_MessageCenter.SendMessage("ShowDeskAni", new FK3_KeyValueInfo("showType", 1));
			if (myLobbyContext.desks.Count == 0)
			{
				UnityEngine.Debug.LogError("该房间没有桌子");
				HideGameObjects();
			}
			else
			{
				ShowGameObjects();
				FK3_UIRoomManager.GetInstance().CloseUI("NoDeskImage");
				FK3_MessageCenter.SendMessage("ReshDeskInfo", null);
			}
		}

		private void RefreshDeskShow(int index)
		{
			FK3_DeskController.Get().UpdateUI_Click(index);
		}

		private void HandleNetMsg_EnterRoom(object[] args)
		{
			UnityEngine.Debug.LogError("=====EnterRoom====");
		}

		private void HandleNetMsg_SelectHall(object[] args)
		{
			UnityEngine.Debug.LogError("selectHall: " + JsonMapper.ToJson(args));
			FK3_NetMsgInfo_RecvEnterRoom fK3_NetMsgInfo_RecvEnterRoom = new FK3_NetMsgInfo_RecvEnterRoom(args);
			try
			{
				fK3_NetMsgInfo_RecvEnterRoom.Parse2();
				JsonData dictionary = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
				UpdateOnline(dictionary);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfo_RecvEnterRoom.valid && fK3_NetMsgInfo_RecvEnterRoom.code == 0)
			{
				myLobbyContext.desks = fK3_NetMsgInfo_RecvEnterRoom.desks;
				InItTable(myLobbyContext.desks);
				FK3_RoomMgr.GetInstance().RoomId = FK3_GVars.roomId;
				FK3_GVars.lobby.curRoomId = FK3_RoomMgr.GetInstance().RoomId;
				RefreshGameUI(FK3_Demo_UI_State.DeskSelection);
			}
		}

		private void UpdateOnline(JsonData dictionary)
		{
			if (dictionary == null)
			{
				UnityEngine.Debug.LogError("dictionary为空");
				return;
			}
			JsonData jsonData = dictionary["onlineNumber"];
			for (int i = 0; i < jsonData.Count; i++)
			{
				for (int j = 0; j < selectBtnList.Count; j++)
				{
					if (i == selectBtnList[j].hallId)
					{
						selectBtnList[j].onlinePeople = jsonData[i.ToString()].ToString();
						selectBtnList[j].UpdateText();
						break;
					}
				}
			}
		}

		public void InItTable(List<FK3_DeskInfo> tableArr)
		{
			UpdateRoomInfo();
			Transform content = Content;
			Vector3 localPosition = ContentOldPos.localPosition;
			content.DOLocalMoveY(localPosition.y, 0.2f);
			Content.GetComponent<DK_DislodgShader>().SetOver();
			allTableArr = new List<FK3_DeskInfo>();
			allTableArr = tableArr;
			FK3_GVars.lobby.desks = new List<FK3_DeskInfo>();
			for (int i = 0; i < allTableArr.Count; i++)
			{
				FK3_GVars.lobby.desks.Add(allTableArr[i]);
			}
			for (int j = 0; j < allTableArr.Count; j++)
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
				int num = k;
				for (int l = 0; l < allTableArr[k].seats.Length; l++)
				{
					tableList[k].transform.Find("Ico/Image" + l).GetComponent<Image>().sprite = _icon[0];
				}
				for (int m = 0; m < allTableArr[k].seats.Length; m++)
				{
					if (!allTableArr[k].seats[m].isUsed)
					{
						continue;
					}
					int num2 = allTableArr[k].seats[m].id + m;
					int num3 = allTableArr[k].seats[m].id - 1;
					UnityEngine.Debug.LogError("num1: " + num2 + " deakId: " + num3);
					if (num2 >= _icon.Length)
					{
						num2 %= _icon.Length;
						UnityEngine.Debug.LogError("num2: " + num2);
						if (num2 >= _icon.Length)
						{
							num2 = _icon.Length - 1;
							UnityEngine.Debug.LogError("num3: " + num2);
						}
					}
					if (num2 <= 0)
					{
						num2 = _icon.Length - 1;
					}
					tableList[k].transform.Find("Ico/Image" + num3).GetComponent<Image>().sprite = _icon[num2];
				}
				tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
				tableList[k].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[k].minGunValue.ToString();
				tableList[k].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[k].maxGunValue.ToString();
			}
			for (int n = 0; n < tableList.Count; n++)
			{
				int index = n;
				tableList[n].gameObject.GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnTable(allTableArr[index].seats, allTableArr[index].id);
				});
			}
			for (int num4 = 0; num4 < tableList.Count; num4++)
			{
				int index2 = num4;
				for (int num5 = 0; num5 < 4; num5++)
				{
					int deskid = num5;
					tableList[index2].transform.Find("Ico/Image" + num5 + "/Btn").GetComponent<Button>().onClick.AddListener(delegate
					{
						ClickBtnArrow(deskid, allTableArr[index2].id);
					});
				}
			}
		}

		public void ClickBtnTable(FK3_SeatInfo[] netSeats, int deskNum)
		{
			UnityEngine.Debug.LogError("点击了桌子: " + deskNum);
			EnterSeat(netSeats, deskNum);
		}

		public void EnterSeat(FK3_SeatInfo[] netSeats, int deskId)
		{
			int num = 0;
			for (int i = 0; i < netSeats.Length; i++)
			{
				if (!netSeats[i].isUsed && num <= 0)
				{
					num = 1;
					int num2 = i;
					ClickBtnArrow(num2, deskId);
					UnityEngine.Debug.LogError("EnterSeat选择了: " + num2 + " 号位");
					break;
				}
			}
			if (num <= 0)
			{
				UnityEngine.Debug.LogError("=====没有空余座位======");
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("该桌没有空余座位", "There is no vacant seat at the table", string.Empty));
			}
		}

		private void ClickBtnArrow(int index, int DeskId)
		{
			UnityEngine.Debug.LogError("DeskId: " + DeskId + "  index: " + index);
			FK3_DeskMgr.Get().SeatBeClickCall(FK3_GVars.roomId, index + 1, DeskId);
		}

		private void HandleNetMsg_RequestSeat(object[] args)
		{
			UnityEngine.Debug.LogError("RequestSeat: " + JsonMapper.ToJson(args));
			try
			{
				JsonData jsonData = JsonMapper.ToObject(JsonMapper.ToJson(args[0]));
				bool flag = jsonData["bCanSeat"].ToString() == "true";
				string a = jsonData["messageStatus"].ToString();
				if (!flag && a == "2")
				{
					FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("该桌没有空余座位", "There is no vacant seat at the table", string.Empty));
				}
				else
				{
					UnityEngine.Debug.LogError("正常进入游戏");
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			FK3_NetMsgInfo_RecvRequestSeat fK3_NetMsgInfo_RecvRequestSeat = new FK3_NetMsgInfo_RecvRequestSeat(args);
			fK3_NetMsgInfo_RecvRequestSeat.Parse();
			int gScore = 0;
			if (fK3_NetMsgInfo_RecvRequestSeat.valid && fK3_NetMsgInfo_RecvRequestSeat.code == 0)
			{
				myLobbyContext.curSeatId = fK3_NetMsgInfo_RecvRequestSeat.seatId;
				myLobbyContext.inGameSeats = fK3_NetMsgInfo_RecvRequestSeat.seats;
				foreach (FK3_SeatInfo2 seat in fK3_NetMsgInfo_RecvRequestSeat.seats)
				{
					if (seat.id == fK3_NetMsgInfo_RecvRequestSeat.seatId)
					{
						gScore = seat.user.gameScore;
					}
				}
				FK3_GVars.m_curState = FK3_Demo_UI_State.GameLoading;
				UpdateUI_ReadyInGame(fK3_NetMsgInfo_RecvRequestSeat.sceneId, fK3_NetMsgInfo_RecvRequestSeat.stageId, fK3_NetMsgInfo_RecvRequestSeat.stageType, fK3_NetMsgInfo_RecvRequestSeat.sceneDuration, fK3_NetMsgInfo_RecvRequestSeat.stageDuration, gScore);
			}
			else
			{
				if (fK3_NetMsgInfo_RecvRequestSeat.code == 29)
				{
					FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("游戏币不足", "Lack of game currency", string.Empty));
				}
				FK3_DeskMgr.Get().mask.gameObject.SetActive(value: false);
			}
		}

		private void HandleNetMsg_UpdateSeat(object[] args)
		{
			UnityEngine.Debug.LogError("UpdateSeat: " + JsonMapper.ToJson(args));
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			if ((int)dictionary["code"] != 0)
			{
				return;
			}
			int num = (int)dictionary["deskId"];
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["seat"];
			int num2 = (int)dictionary2["id"];
			bool flag = (bool)dictionary2["isFree"];
			FK3_SeatInfo fK3_SeatInfo;
			if (flag)
			{
				fK3_SeatInfo = new FK3_SeatInfo(num2, !flag, string.Empty);
			}
			else
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["user"];
				string name = (string)dictionary3["nickname"];
				int icon = (int)dictionary3["photoID"];
				int level = (int)dictionary3["level"];
				int sex = -1;
				if (dictionary3.ContainsKey("sex"))
				{
					string a = (string)dictionary3["sex"];
					sex = ((!(a == "女")) ? 1 : 0);
				}
				fK3_SeatInfo = new FK3_SeatInfo(num2, !flag, name, icon, level, sex);
			}
			int num3 = -1;
			for (int i = 0; i < myLobbyContext.desks.Count; i++)
			{
				if (myLobbyContext.desks[i].id == num)
				{
					num3 = i;
				}
			}
			myLobbyContext.desks[num3].seats[num2 - 1] = fK3_SeatInfo;
			FK3_DeskController.Get().UpdateUI_SeatInfoPush(num3);
			FK3_TableController.Get().UpdateListUI(num3);
		}

		private void HideGameObjects()
		{
			NoTips.SetActive(value: true);
			NoTips.transform.DOScale(Vector3.one, 0.35f);
			tfTableInfo.gameObject.SetActive(value: false);
			if (FK3_DeskMgr.Get() != null)
			{
				FK3_DeskMgr.Get().mask.gameObject.SetActive(value: false);
			}
		}

		private void ShowGameObjects()
		{
			NoTips.transform.DOScale(Vector3.zero, 0.1f);
			NoTips.SetActive(value: false);
			tfTableInfo.gameObject.SetActive(value: true);
		}

		private void HandleNetMsg_UpdateRoomInfo(object[] args)
		{
			UnityEngine.Debug.LogError("UpdateRoomInfo: " + JsonMapper.ToJson(args));
			FK3_NetMsgInfo_RecvEnterRoom fK3_NetMsgInfo_RecvEnterRoom = new FK3_NetMsgInfo_RecvEnterRoom(args);
			try
			{
				fK3_NetMsgInfo_RecvEnterRoom.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (fK3_NetMsgInfo_RecvEnterRoom.valid && fK3_NetMsgInfo_RecvEnterRoom.code == 0)
			{
				myLobbyContext.desks.Clear();
				myLobbyContext.desks = fK3_NetMsgInfo_RecvEnterRoom.desks;
				InItTable(myLobbyContext.desks);
				if (fK3_NetMsgInfo_RecvEnterRoom.desks.Count == 0)
				{
					UnityEngine.Debug.LogError("该房间内没有桌子！");
					FK3_UIRoomManager.GetInstance().CloseUI("Desk");
				}
				else
				{
					UnityEngine.Debug.Log("UpdateUI_RoomInfoPush SeatBeClickCall");
					FK3_DeskController.Get().UpdateUI_RoomInfoPush(0, FK3_RoomMgr.GetInstance().RoomId);
					FK3_DeskMgr.Get().Fewer.GetComponent<FK3_TableController>().InitTableList(RefreshDeskShow);
				}
			}
		}

		private void HandleNetMsg_UserAward(object[] args)
		{
			UnityEngine.Debug.LogError("UserAward: " + JsonMapper.ToJson(args));
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int num = (int)dictionary["gameGold"];
			FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip($"恭喜获得后台赠送的{num}游戏币", $"Congratulations on the backstage gift of {num} game coins", string.Empty));
			FK3_UserInfoShow.Get().Init();
		}

		private void HandleNetMsg_ScrollMessage(object[] args)
		{
			JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
			UnityEngine.Debug.LogError("通知: " + jsonData.ToJson());
			if (All_NoticePanel.GetInstance() != null)
			{
				All_NoticePanel.GetInstance().AddTip(jsonData);
			}
		}

		private void HandleNetMsg_GameShutup(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			bool isShutup = (bool)dictionary["isShutup"];
			FK3_GVars.isShutup = isShutup;
		}

		private void HandleNetMsg_UserShutup(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			bool isUserShutup = (bool)dictionary["isShutup"];
			FK3_GVars.isUserShutup = isUserShutup;
		}

		public void HandleNetMsg_PlayerInfo(object[] args)
		{
			UnityEngine.Debug.LogError("PlayerInfo: " + JsonMapper.ToJson(args));
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int deskId = (int)dictionary["deskId"];
			int num = (int)dictionary["seatId"];
			int dailyHonor = (int)dictionary["dailyHonor"];
			int weekHonor = (int)dictionary["weekHonor"];
			int totalHonor = (int)dictionary["totalHonor"];
			int level = (int)dictionary["level"];
			int gameScore = (int)dictionary["gameScore"];
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame)
			{
				FK3_UIIngameManager.GetInstance().OpenUI("UserInfoRoot", show: false);
				FK3_SimpleSingletonBehaviour<FK3_UserInfoInGame>.Get().ShowUserInfo(num, level, dailyHonor, weekHonor, totalHonor);
			}
			else
			{
				FK3_DeskController.Get().ShowSeatUserInfo(gameScore, FK3_GVars.lobby.GetDeskById(deskId).GetSeat(num), level, dailyHonor, weekHonor, totalHonor);
			}
		}

		private void HandleNetMsg_PlayerAddLevel(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int level = (int)dictionary["level"];
			FK3_GVars.user.level = level;
			FK3_UserInfoShow.Get().Init();
		}

		private void QuitToRoom()
		{
			FK3_MessageCenter.SendMessage("RoomMove", new FK3_KeyValueInfo("moveVecx", roomCloseVecX));
			FK3_MessageCenter.SendMessage("ShowDeskAni", new FK3_KeyValueInfo("showType", 1));
			RefreshGameUI(FK3_Demo_UI_State.RoomSelection);
			myLobbyContext.desks.Clear();
		}

		private void HandleNetMsg_QuitToLogin(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			try
			{
				UnityEngine.Debug.LogError("QuitToLogin: " + JsonMapper.ToJson(args));
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			int num = (int)dictionary["quitToLogin"];
			switch (num)
			{
			case 1:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("服务器正在维护", "The server is under maintenance", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			case 2:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("您的账号已被冻结，详情请联系客服", "Your account has been frozen. Please contact customer service for more details", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			case 3:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("您的账号已被删除，详情请联系客服", "Your account has been deleted. Please contact customer service for more details", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			case 4:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("该账号在别处登录，您已被迫下线", "The account was logged in elsewhere and you have been forced to log out", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			case 5:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("密码已重置，请重新登陆", "Password has been reset. Please log in again", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			case 6:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("与服务器断开连接", "Disconnect from the server", string.Empty), showOkCancel: false, QuitToLogin);
				break;
			default:
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("异常quitType类型:", "Abnormal quitType type:", string.Empty) + num);
				break;
			}
			FK3_MB_Singleton<FK3_AppManager>.Get().PrepareQuitGame();
		}

		private void HandleNetMsg_Overflow(object[] args)
		{
			FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("您的账号已爆机，请先兑奖", "Your account has been cancelled, please redeem the prize first", string.Empty), showOkCancel: false, QuitToHallGame);
			FK3_MB_Singleton<FK3_AppManager>.Get().PrepareQuitGame();
		}

		public void QuitToLogin()
		{
			UnityEngine.Debug.LogError("QuitToLogin");
			FK3_HeadAreaMgr.Get().QuitToHallGame(isToHall: false);
		}

		private void QuitToHallGame()
		{
			UnityEngine.Debug.LogError("QuitToHallGame");
			FK3_HeadAreaMgr.Get().QuitToHallGame();
		}

		private void HandleNetMsg_LeaveRoom(object[] args)
		{
			FK3_MessageCenter.SendMessage("RoomMove", new FK3_KeyValueInfo("moveVecx", roomShowVecX));
			FK3_MessageCenter.SendMessage("ShowDeskAni", new FK3_KeyValueInfo("showType", 0));
			RefreshGameUI(FK3_Demo_UI_State.RoomSelection);
		}

		private void HandleNetMsg_GameGold(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int gameGold = (int)dictionary["gameGold"];
			FK3_GVars.user.gameGold = gameGold;
			FK3_UserInfoShow.Get().Init();
		}

		private void HandleNetMsg_addExpeGoldAuto(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int expeGold = (int)dictionary["expeGold"];
			FK3_GVars.user.expeGold = expeGold;
			FK3_UserInfoShow.Get().Init();
			FK3_AlertDialog.Get().ShowDialog((FK3_GVars.language == "zh") ? "体验币不足，系统已自动补足至10000" : "The system has automatically made up the shortage of experience coins to 10000");
		}
	}
}
