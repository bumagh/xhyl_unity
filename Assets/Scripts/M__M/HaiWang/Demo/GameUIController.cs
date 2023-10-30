using DG.Tweening;
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
	public class GameUIController : HW2_MB_Singleton<GameUIController>
	{
		private int roomShowVecX = 270;

		private int roomCloseVecX = 890;

		public int roomId = -1;

		public LobbyContext myLobbyContext = new LobbyContext();

		[HideInInspector]
		public List<GameObject> tableList = new List<GameObject>();

		public List<GameObject> selectBtnList = new List<GameObject>();

		public Sprite[] _icon;

		private Transform Content;

		private Transform ContentOldPos;

		private Transform ContentTagPos;

		private Transform scrollView;

		public GameObject tablePre;

		[SerializeField]
		private Transform tfTableInfo;

		private int JingJiGun = 1000;

		private int ChuJiGun = 500;

		private bool isInitOk;

		private int tempSelectId = -1;

		private int currRoomId = 1;

		[HideInInspector]
		public int otherRoomId = -1;

		private List<DeskInfo> allTableArr = new List<DeskInfo>();

		private List<DeskInfo> tempAllTableArr = new List<DeskInfo>();

		private void Awake()
		{
			if (HW2_MB_Singleton<GameUIController>._instance == null)
			{
				HW2_MB_Singleton<GameUIController>.SetInstance(this);
			}
			else
			{
				UnityEngine.Debug.LogError("GameUIController已经存在");
			}
			scrollView = base.transform.Find("Normal/Tables/Scroll View");
			Content = scrollView.Find("Viewport/Content");
			ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
			ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
			HideGameObjects();
		}

		private void Start()
		{
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("enterRoom", HandleNetMsg_EnterRoom);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("requestSeat", HandleNetMsg_RequestSeat);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("leaveRoom", HandleNetMsg_LeaveRoom);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("updateSeatPush", HandleNetMsg_UpdateSeat);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("updateRoomInfoPush", HandleNetMsg_UpdateRoomInfo);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("addExpeGoldAuto", HandleNetMsg_addExpeGoldAuto);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("overflowPush", HandleNetMsg_Overflow);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userAwardPush", HandleNetMsg_UserAward);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("scrollMessagePush", HandleNetMsg_ScrollMessage);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gameShutupPush", HandleNetMsg_GameShutup);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userShutupPush", HandleNetMsg_UserShutup);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("playerInfo", HandleNetMsg_PlayerInfo);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("playerAddLevelPush", HandleNetMsg_PlayerAddLevel);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
			Init();
			try
			{
				HW2_GVars.SetLobbyContext(myLobbyContext);
				HW2_GVars.SetUserInfo(HW2_GVars.lobby.user);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		private void Init()
		{
			UnityEngine.Debug.LogError("打开房间");
		}

		private void OnEnable()
		{
			try
			{
				if (HW2_GVars.m_curState != Demo_UI_State.StartupLoading)
				{
					UnityEngine.Debug.LogError("初始化开始");
					try
					{
						HW2_UserInfoShow.Get().Init();
						UnityEngine.Debug.LogError("初始化完成");
					}
					catch (Exception message)
					{
						UnityEngine.Debug.LogError(message);
					}
				}
				if (HW2_MB_Singleton<HW2_NetManager>.Get() != null)
				{
					HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("gameGoldPush", HandleNetMsg_GameGold);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		public void RefreshGameUI(Demo_UI_State state)
		{
			HW2_GVars.m_curState = state;
			UnityEngine.Debug.LogError("当前状态: " + state);
			isInitOk = true;
			switch (state)
			{
			case Demo_UI_State.RoomSelection:
				UpdateUI_UserInfo();
				break;
			case Demo_UI_State.DeskSelection:
				UpdateUI_Desk();
				break;
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				UnityEngine.Debug.Log("Up:m_curState：" + HW2_GVars.m_curState);
				OnBtnBack_Click();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
			{
				isInitOk = true;
			}
			if (tempSelectId != HW2_GVars.selectRoomId && isInitOk)
			{
				tempSelectId = HW2_GVars.selectRoomId;
				UnityEngine.Debug.LogError("执行选房间: " + tempSelectId);
				StartCoroutine(ClickBtnRoom(tempSelectId));
			}
		}

		private IEnumerator ClickBtnRoom(int index)
		{
			HideGameObjects();
			Transform content = Content;
			Vector3 localPosition = ContentTagPos.localPosition;
			content.DOLocalMoveY(localPosition.y, 0.2f);
			Content.GetComponent<DK_DislodgShader>().SetImaAndText(0.2f);
			switch (index)
			{
			case 0:
				index = 2;
				otherRoomId = 0;
				if (HW2_UserInfoShow.Get() != null && HW2_UserInfoShow.Get()._txtLogo != null)
				{
					HW2_UserInfoShow.Get()._txtLogo.text = "中级厅";
				}
				break;
			case 1:
				index = 2;
				otherRoomId = 1;
				if (HW2_UserInfoShow.Get() != null && HW2_UserInfoShow.Get()._txtLogo != null)
				{
					HW2_UserInfoShow.Get()._txtLogo.text = "初级厅";
				}
				break;
			case 4:
				index = 10;
				otherRoomId = 10;
				if (HW2_UserInfoShow.Get() != null && HW2_UserInfoShow.Get()._txtLogo != null)
				{
					HW2_UserInfoShow.Get()._txtLogo.text = "体验厅";
				}
				break;
			case 5:
				index = 10;
				otherRoomId = 10;
				UnityEngine.Debug.LogError("3");
				break;
			case 2:
				index = 1;
				otherRoomId = -1;
				if (HW2_UserInfoShow.Get() != null && HW2_UserInfoShow.Get()._txtLogo != null)
				{
					HW2_UserInfoShow.Get()._txtLogo.text = "练习厅";
				}
				break;
			case 3:
				index = 2;
				otherRoomId = -1;
				if (HW2_UserInfoShow.Get() != null && HW2_UserInfoShow.Get()._txtLogo != null)
				{
					HW2_UserInfoShow.Get()._txtLogo.text = "竞技厅";
				}
				break;
			default:
				index = 10;
				break;
			}
			if (index >= 3)
			{
				UnityEngine.Debug.LogError("被反回了");
				GoToRoom();
				yield return new WaitForSeconds(0.2f);
				for (int i = 0; i < tableList.Count; i++)
				{
					UnityEngine.Object.Destroy(tableList[i].gameObject);
				}
				Transform content2 = Content;
				Vector3 localPosition2 = ContentOldPos.localPosition;
				content2.DOLocalMoveY(localPosition2.y, 0.2f);
				Content.GetComponent<DK_DislodgShader>().SetOver();
				tableList = new List<GameObject>();
			}
			else
			{
				isInitOk = true;
				GoToRoom();
				yield return new WaitForSeconds(0.25f);
				UnityEngine.Debug.LogError("进入 " + index + " 厅");
				currRoomId = index;
				HW2_Singleton<SoundMgr>.Get().PlayClip("选座选厅自动发炮");
				HW2_Singleton<SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
				RoomMgr.GetInstance().SelectRoom(index);
			}
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
			if (HW2_GVars.m_curState == Demo_UI_State.DeskSelection)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/leaveRoom", new object[1]
				{
					HW2_GVars.lobby.curRoomId
				});
				TableController.Get().itemList.Clear();
			}
		}

		public void OnBtnBack_Click()
		{
			UnityEngine.Debug.LogError("点击返回  当前状态: " + HW2_GVars.m_curState);
			QuitToHallGame();
		}

		private void UpdateUI_ReadyInGame(int sceneId, int stageId, int stageType, int sceneDuration, int stageDuration, int gScore)
		{
			HW2_DeskMgr.Get().mask.gameObject.SetActive(value: false);
			HW2_MB_Singleton<EnterLobbyLogic>.Get().DisableScene();
			AppSceneMgr.RunAction("Game.EnterGame", new object[6]
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
			HW2_UserInfoShow.Get().Init();
			UIRoomManager.GetInstance().CloseUI("Login");
			UIRoomManager.GetInstance().CloseUI("NoDeskImage");
		}

		private void UpdateUI_Desk()
		{
			UnityEngine.Debug.LogError("打开桌子");
			HW2_UserInfoShow.Get().Init();
			HW2_MessageCenter.SendMessage("RoomMove", new KeyValueInfo("moveVecx", roomCloseVecX));
			HW2_MessageCenter.SendMessage("ShowDeskAni", new KeyValueInfo("showType", 1));
			if (myLobbyContext.desks.Count == 0)
			{
				UnityEngine.Debug.LogError("该房间没有桌子");
				HideGameObjects();
			}
			else
			{
				ShowGameObjects();
				UIRoomManager.GetInstance().CloseUI("NoDeskImage");
				HW2_MessageCenter.SendMessage("ReshDeskInfo", null);
			}
		}

		private void RefreshDeskShow(int index)
		{
			DeskController.Get().UpdateUI_Click(index);
		}

		private void HandleNetMsg_EnterRoom(object[] args)
		{
			UnityEngine.Debug.LogError("EnterRoom: " + JsonMapper.ToJson(args));
			NetMsgInfo_RecvEnterRoom netMsgInfo_RecvEnterRoom = new NetMsgInfo_RecvEnterRoom(args);
			try
			{
				netMsgInfo_RecvEnterRoom.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfo_RecvEnterRoom.valid && netMsgInfo_RecvEnterRoom.code == 0)
			{
				myLobbyContext.desks = netMsgInfo_RecvEnterRoom.desks;
				InItTable(myLobbyContext.desks);
				HW2_GVars.lobby.curRoomId = RoomMgr.GetInstance().RoomId;
				RefreshGameUI(Demo_UI_State.DeskSelection);
			}
		}

		public void InItTable(List<DeskInfo> tableArr)
		{
			UpdateRoomInfo();
			Transform content = Content;
			Vector3 localPosition = ContentOldPos.localPosition;
			content.DOLocalMoveY(localPosition.y, 0.2f);
			Content.GetComponent<DK_DislodgShader>().SetOver();
			allTableArr = new List<DeskInfo>();
			tempAllTableArr = new List<DeskInfo>();
			tempAllTableArr = tableArr;
			if (currRoomId == 2 && otherRoomId == -1)
			{
				UnityEngine.Debug.LogError("当前是竞技厅");
				for (int i = 0; i < tempAllTableArr.Count; i++)
				{
					if (tempAllTableArr[i].maxGunValue >= JingJiGun)
					{
						allTableArr.Add(tempAllTableArr[i]);
					}
				}
			}
			else if (currRoomId == 1 && otherRoomId == -1)
			{
				UnityEngine.Debug.LogError("当前是练习厅");
				for (int j = 0; j < tempAllTableArr.Count; j++)
				{
					allTableArr.Add(tempAllTableArr[j]);
				}
			}
			if (otherRoomId != -1)
			{
				UnityEngine.Debug.LogError((otherRoomId != 0) ? "当前是初级厅" : "当前是中级厅");
				for (int k = 0; k < tempAllTableArr.Count; k++)
				{
					switch (otherRoomId)
					{
					case 0:
						if (tempAllTableArr[k].maxGunValue >= ChuJiGun && tempAllTableArr[k].maxGunValue < JingJiGun)
						{
							allTableArr.Add(tempAllTableArr[k]);
						}
						break;
					case 1:
						if (tempAllTableArr[k].maxGunValue < ChuJiGun)
						{
							allTableArr.Add(tempAllTableArr[k]);
						}
						break;
					}
				}
			}
			HW2_GVars.lobby.desks = new List<DeskInfo>();
			for (int l = 0; l < allTableArr.Count; l++)
			{
				HW2_GVars.lobby.desks.Add(allTableArr[l]);
			}
			for (int m = 0; m < allTableArr.Count; m++)
			{
				GameObject item = UnityEngine.Object.Instantiate(tablePre, Content);
				tableList.Add(item);
			}
			UnityEngine.Debug.LogError("桌子个数: " + tableList.Count + "  " + allTableArr.Count);
			for (int n = 0; n < tableList.Count; n++)
			{
				int num = n;
				for (int num2 = 0; num2 < allTableArr[n].seats.Length; num2++)
				{
					tableList[n].transform.Find("Ico/Image" + num2).GetComponent<Image>().sprite = _icon[0];
				}
				for (int num3 = 0; num3 < allTableArr[n].seats.Length; num3++)
				{
					if (!allTableArr[n].seats[num3].isUsed)
					{
						continue;
					}
					int num4 = allTableArr[n].seats[num3].id + num3;
					int num5 = allTableArr[n].seats[num3].id - 1;
					UnityEngine.Debug.LogError("num1: " + num4 + " deakId: " + num5);
					if (num4 >= _icon.Length)
					{
						num4 %= _icon.Length;
						UnityEngine.Debug.LogError("num2: " + num4);
						if (num4 >= _icon.Length)
						{
							num4 = _icon.Length - 1;
							UnityEngine.Debug.LogError("num3: " + num4);
						}
					}
					if (num4 <= 0)
					{
						num4 = _icon.Length - 1;
					}
					tableList[n].transform.Find("Ico/Image" + num5).GetComponent<Image>().sprite = _icon[num4];
				}
				tableList[n].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[n].name;
				tableList[n].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[n].minGunValue.ToString();
				tableList[n].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[n].maxGunValue.ToString();
			}
			try
			{
				if (allTableArr.Count >= 1)
				{
					selectBtnList[currRoomId - 1].transform.Find("Ico/Text").GetComponent<Text>().text = (tableList.Count * 4).ToString();
					selectBtnList[currRoomId - 1].transform.Find("Min/Text").GetComponent<Text>().text = allTableArr[0].minGunValue.ToString();
					selectBtnList[currRoomId - 1].transform.Find("Select/Ico/Text").GetComponent<Text>().text = (tableList.Count * 4).ToString();
					selectBtnList[currRoomId - 1].transform.Find("Select/Min/Text").GetComponent<Text>().text = allTableArr[0].minGunValue.ToString();
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			for (int num6 = 0; num6 < tableList.Count; num6++)
			{
				int index = num6;
				for (int num7 = 0; num7 < 4; num7++)
				{
					int deskid = num7;
					tableList[index].transform.Find("Ico/Image" + num7 + "/Btn").GetComponent<Button>().onClick.AddListener(delegate
					{
						ClickBtnArrow(deskid, allTableArr[index].id);
					});
				}
			}
		}

		public void ClickBtnTable(SeatInfo[] netSeats, int deskNum)
		{
			UnityEngine.Debug.LogError("点击了桌子: " + deskNum);
			EnterSeat(netSeats, deskNum);
		}

		public void EnterSeat(SeatInfo[] netSeats, int deskId)
		{
			int num = 0;
			int num2 = 0;
			while (true)
			{
				if (num2 < netSeats.Length)
				{
					if (!netSeats[num2].isUsed && num <= 0)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			num = 1;
			int num3 = num2;
			ClickBtnArrow(num3, deskId);
			UnityEngine.Debug.LogError("EnterSeat选择了: " + num3 + " 号位");
		}

		private void ClickBtnArrow(int index, int DeskId)
		{
			HW2_DeskMgr.Get().SeatBeClickCall(index + 1, DeskId);
		}

		private void HandleNetMsg_RequestSeat(object[] args)
		{
			UnityEngine.Debug.LogError("RequestSeat: " + JsonMapper.ToJson(args));
			NetMsgInfo_RecvRequestSeat netMsgInfo_RecvRequestSeat = new NetMsgInfo_RecvRequestSeat(args);
			netMsgInfo_RecvRequestSeat.Parse();
			int gScore = 0;
			if (netMsgInfo_RecvRequestSeat.valid && netMsgInfo_RecvRequestSeat.code == 0)
			{
				myLobbyContext.curSeatId = netMsgInfo_RecvRequestSeat.seatId;
				myLobbyContext.inGameSeats = netMsgInfo_RecvRequestSeat.seats;
				foreach (SeatInfo2 seat in netMsgInfo_RecvRequestSeat.seats)
				{
					if (seat.id == netMsgInfo_RecvRequestSeat.seatId)
					{
						gScore = seat.user.gameScore;
					}
				}
				HW2_GVars.m_curState = Demo_UI_State.GameLoading;
				UnityEngine.Debug.Log(HW2_LogHelper.Yellow("m_curState: " + Demo_UI_State.GameLoading));
				UpdateUI_ReadyInGame(netMsgInfo_RecvRequestSeat.sceneId, netMsgInfo_RecvRequestSeat.stageId, netMsgInfo_RecvRequestSeat.stageType, netMsgInfo_RecvRequestSeat.sceneDuration, netMsgInfo_RecvRequestSeat.stageDuration, gScore);
			}
			else
			{
				if (netMsgInfo_RecvRequestSeat.code == 29)
				{
					HW2_AlertDialog.Get().ShowDialog("游戏币不足");
				}
				HW2_DeskMgr.Get().mask.gameObject.SetActive(value: false);
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
			SeatInfo seatInfo;
			if (flag)
			{
				seatInfo = new SeatInfo(num2, !flag, string.Empty);
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
				seatInfo = new SeatInfo(num2, !flag, name, icon, level, sex);
			}
			int num3 = -1;
			for (int i = 0; i < myLobbyContext.desks.Count; i++)
			{
				if (myLobbyContext.desks[i].id == num)
				{
					num3 = i;
				}
			}
			myLobbyContext.desks[num3].seats[num2 - 1] = seatInfo;
			DeskController.Get().UpdateUI_SeatInfoPush(num3);
			TableController.Get().UpdateListUI(num3);
		}

		private void HideGameObjects()
		{
			tfTableInfo.gameObject.SetActive(value: false);
			if (HW2_DeskMgr.Get() != null)
			{
				HW2_DeskMgr.Get().mask.gameObject.SetActive(value: false);
			}
		}

		private void ShowGameObjects()
		{
			tfTableInfo.gameObject.SetActive(value: true);
		}

		private void HandleNetMsg_UpdateRoomInfo(object[] args)
		{
			UnityEngine.Debug.LogError("UpdateRoomInfo: " + JsonMapper.ToJson(args));
			NetMsgInfo_RecvEnterRoom netMsgInfo_RecvEnterRoom = new NetMsgInfo_RecvEnterRoom(args);
			try
			{
				netMsgInfo_RecvEnterRoom.Parse();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (netMsgInfo_RecvEnterRoom.valid && netMsgInfo_RecvEnterRoom.code == 0)
			{
				myLobbyContext.desks.Clear();
				myLobbyContext.desks = netMsgInfo_RecvEnterRoom.desks;
				InItTable(myLobbyContext.desks);
				if (netMsgInfo_RecvEnterRoom.desks.Count == 0)
				{
					UnityEngine.Debug.LogError("该房间内没有桌子！");
					UIRoomManager.GetInstance().CloseUI("Desk");
				}
				else
				{
					UnityEngine.Debug.Log("UpdateUI_RoomInfoPush SeatBeClickCall");
					DeskController.Get().UpdateUI_RoomInfoPush(0, RoomMgr.GetInstance().RoomId);
					HW2_DeskMgr.Get().Fewer.GetComponent<TableController>().InitTableList(RefreshDeskShow);
				}
			}
		}

		private void HandleNetMsg_UserAward(object[] args)
		{
			UnityEngine.Debug.LogError("UserAward: " + JsonMapper.ToJson(args));
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int num = (int)dictionary["gameGold"];
			HW2_AlertDialog.Get().ShowDialog($"恭喜获得后台赠送的{num}游戏币");
			HW2_UserInfoShow.Get().Init();
		}

		private void HandleNetMsg_ScrollMessage(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			string msg = (string)dictionary["content"];
			NoticeMgr.Get().AddMessage(msg);
		}

		private void HandleNetMsg_GameShutup(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			bool isShutup = (bool)dictionary["isShutup"];
			HW2_GVars.isShutup = isShutup;
		}

		private void HandleNetMsg_UserShutup(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			bool isUserShutup = (bool)dictionary["isShutup"];
			HW2_GVars.isUserShutup = isUserShutup;
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
			if (HW2_GVars.m_curState == Demo_UI_State.InGame)
			{
				UIIngameManager.GetInstance().OpenUI("UserInfoRoot", show: false);
				SimpleSingletonBehaviour<HW2_UserInfoInGame>.Get().ShowUserInfo(num, level, dailyHonor, weekHonor, totalHonor);
			}
			else
			{
				DeskController.Get().ShowSeatUserInfo(gameScore, HW2_GVars.lobby.GetDeskById(deskId).GetSeat(num), level, dailyHonor, weekHonor, totalHonor);
			}
		}

		private void HandleNetMsg_PlayerAddLevel(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int level = (int)dictionary["level"];
			HW2_GVars.user.level = level;
			HW2_UserInfoShow.Get().Init();
		}

		private void QuitToRoom()
		{
			HW2_MessageCenter.SendMessage("RoomMove", new KeyValueInfo("moveVecx", roomCloseVecX));
			HW2_MessageCenter.SendMessage("ShowDeskAni", new KeyValueInfo("showType", 1));
			RefreshGameUI(Demo_UI_State.RoomSelection);
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
				HW2_AlertDialog.Get().ShowDialog("服务器正在维护", showOkCancel: false, QuitToLogin);
				break;
			case 2:
				HW2_AlertDialog.Get().ShowDialog("您的账号已被冻结，详情请联系客服", showOkCancel: false, QuitToLogin);
				break;
			case 3:
				HW2_AlertDialog.Get().ShowDialog("您的账号已被删除，详情请联系客服", showOkCancel: false, QuitToLogin);
				break;
			case 4:
				HW2_AlertDialog.Get().ShowDialog("该账号在别处登录，您已被迫下线", showOkCancel: false, QuitToLogin);
				break;
			case 5:
				HW2_AlertDialog.Get().ShowDialog("密码已重置，请重新登陆", showOkCancel: false, QuitToLogin);
				break;
			case 6:
				HW2_AlertDialog.Get().ShowDialog("与服务器断开连接", showOkCancel: false, QuitToLogin);
				break;
			default:
				HW2_AlertDialog.Get().ShowDialog("异常quitType类型:" + num);
				break;
			}
			HW2_MB_Singleton<HW2_AppManager>.Get().PrepareQuitGame();
		}

		private void HandleNetMsg_Overflow(object[] args)
		{
			HW2_AlertDialog.Get().ShowDialog("您的账号已爆机，请先兑奖", showOkCancel: false, QuitToHallGame);
			HW2_MB_Singleton<HW2_AppManager>.Get().PrepareQuitGame();
		}

		public void QuitToLogin()
		{
			UnityEngine.Debug.LogError("QuitToLogin");
			HeadAreaMgr.Get().QuitToHallGame(isToHall: false);
		}

		private void QuitToHallGame()
		{
			UnityEngine.Debug.LogError("QuitToHallGame");
			HeadAreaMgr.Get().QuitToHallGame();
		}

		private void HandleNetMsg_LeaveRoom(object[] args)
		{
			HW2_MessageCenter.SendMessage("RoomMove", new KeyValueInfo("moveVecx", roomShowVecX));
			HW2_MessageCenter.SendMessage("ShowDeskAni", new KeyValueInfo("showType", 0));
			RefreshGameUI(Demo_UI_State.RoomSelection);
		}

		private void HandleNetMsg_GameGold(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int gameGold = (int)dictionary["gameGold"];
			HW2_GVars.user.gameGold = gameGold;
			HW2_UserInfoShow.Get().Init();
		}

		private void HandleNetMsg_addExpeGoldAuto(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int expeGold = (int)dictionary["expeGold"];
			HW2_GVars.user.expeGold = expeGold;
			HW2_UserInfoShow.Get().Init();
			HW2_AlertDialog.Get().ShowDialog((HW2_GVars.language == "zh") ? "体验币不足，系统已自动补足至10000" : string.Empty);
		}
	}
}
