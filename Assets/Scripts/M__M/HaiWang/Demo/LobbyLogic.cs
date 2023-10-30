using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.NetMsgDefine;
using M__M.HaiWang.UI;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class LobbyLogic : MonoBehaviour
	{
		public string sceneName;

		public List<GameObject> roots;

		private void Awake()
		{
			AppSceneMgr.scene_lobby_name = sceneName;
			AppSceneMgr.RegisterScene(sceneName);
			try
			{
				AppSceneMgr.RegisterAction("Lobby.BackLobby", BackLobby);
				AppSceneMgr.RegisterAction("Lobby.EnterLobby", EnterLobby);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			if (!AppSceneMgr.isFirstScene(sceneName))
			{
				DisableScene();
			}
		}

		private void Start()
		{
			MonoBehaviour.print(sceneName + ".Start");
			HW2_GVars.SetLobbyContext(LobbyUI.Get().GetContext());
			HW2_GVars.SetUserInfo(HW2_GVars.lobby.user);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("enterRoom", HandleNetMsg_EnterRoom);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("requestSeat", HandleNetMsg_RequestSeat);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("leaveRoom", HandleNetMsg_LeaveRoom);
			LobbyUI.Get().EventHandler_UI_Login_OnBtnLoginClick += Handler_UI_Login_OnBtnLoginClick;
			LobbyUI.Get().EventHandler_UI_RoomSelection_OnRoomClick += Handler_UI_RoomSelection_OnRoomClick;
			LobbyUI.Get().EventHandler_UI_DeskSelection_OnSeatClick += Handler_UI_DeskSelection_OnSeatClick;
			LobbyUI.Get().EventHandler_UI_DeskSelection_OnBtnReturnClick += Handler_UI_DeskSelection_OnBtnReturnClick;
			LobbyUI.Get().Change_UI_State(Demo_UI_State.Login);
			UnityEngine.Object.DontDestroyOnLoad(HW2_MB_Singleton<HW2_NetManager>.Get().gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(HW2_MB_Singleton<HW2_NetManager>.Get().gameObject);
			StartCoroutine(IE_Flow());
		}

		private bool _CheckDependence()
		{
			bool flag = true;
			try
			{
				return flag;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
				flag = false;
				return flag;
			}
			finally
			{
				if (!flag)
				{
					UnityEngine.Debug.LogError("Login_Demo 依赖检查失败");
				}
			}
		}

		private void Handler_UI_Login_OnBtnLoginClick(LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_Login_OnBtnLoginClick");
			string strAccount = context.strAccount;
			string strPassword = context.strPassword;
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/login", new object[2]
			{
				strAccount,
				strPassword
			}, check: false);
		}

		private void Handler_UI_RoomSelection_OnRoomClick(LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_RoomSelection_OnRoomClick");
			int curRoomId = context.curRoomId;
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/enterRoom", new object[1]
			{
				curRoomId
			});
		}

		private void Handler_UI_DeskSelection_OnSeatClick(LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_DeskSelection_OnSeatClick");
			int curDeskId = context.curDeskId;
			int curSeatId = context.curSeatId;
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/requestSeat", new object[2]
			{
				curDeskId,
				curSeatId
			});
		}

		private void Handler_UI_DeskSelection_OnBtnReturnClick(LobbyContext context)
		{
			int curRoomId = context.curRoomId;
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/leaveRoom", new object[1]
			{
				curRoomId
			});
		}

		private void HandleNetMsg_Login(object[] args)
		{
			NetMsgInfo_RecvLogin netMsgInfo_RecvLogin = new NetMsgInfo_RecvLogin(args);
			netMsgInfo_RecvLogin.Parse();
			netMsgInfo_RecvLogin.user.CopyTo(LobbyUI.Get().GetContext().user);
			if (netMsgInfo_RecvLogin.valid && netMsgInfo_RecvLogin.code == 0)
			{
				LobbyUI.Get().Change_UI_State(Demo_UI_State.RoomSelection);
			}
		}

		private void HandleNetMsg_EnterRoom(object[] args)
		{
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
				LobbyUI.Get().GetContext().desks = netMsgInfo_RecvEnterRoom.desks;
				UnityEngine.Debug.Log($"desk count {netMsgInfo_RecvEnterRoom.desks.Count}");
				LobbyUI.Get().Change_UI_State(Demo_UI_State.DeskSelection);
			}
		}

		private void HandleNetMsg_RequestSeat(object[] args)
		{
			NetMsgInfo_RecvRequestSeat netMsgInfo_RecvRequestSeat = new NetMsgInfo_RecvRequestSeat(args);
			netMsgInfo_RecvRequestSeat.Parse();
			if (netMsgInfo_RecvRequestSeat.valid && netMsgInfo_RecvRequestSeat.code == 0)
			{
				LobbyUI.Get().GetContext().curSeatId = netMsgInfo_RecvRequestSeat.seatId;
				LobbyUI.Get().GetContext().inGameSeats = netMsgInfo_RecvRequestSeat.seats;
				DisableScene();
				AppSceneMgr.RunAction("Game.EnterGame", new object[3]
				{
					netMsgInfo_RecvRequestSeat.sceneId,
					netMsgInfo_RecvRequestSeat.stageId,
					netMsgInfo_RecvRequestSeat.stageType
				}, delegate
				{
				});
			}
		}

		private void HandleNetMsg_LeaveRoom(object[] args)
		{
			LobbyUI.Get().Change_UI_State(Demo_UI_State.RoomSelection);
		}

		private IEnumerator IE_Flow()
		{
			string empty = string.Empty;
			string ip = "xlcs.swccd88.xyz";
			int port = 10100;
			HW2_MB_Singleton<HW2_NetManager>.Get().Connect(ip, port);
			yield break;
		}

		private void KeepOnLoad()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item in list)
			{
				if (item != null)
				{
					UnityEngine.Object.DontDestroyOnLoad(item.gameObject);
					HW2_GVars.dontDestroyOnLoadList.Add(item.gameObject);
				}
			}
		}

		private void DisableScene()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item in list)
			{
				if (item != null)
				{
					item.SetActive(value: false);
				}
			}
		}

		private void EnableScene()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item in list)
			{
				if (item != null)
				{
					item.SetActive(value: true);
				}
			}
		}

		private List<GameObject> _GetRoots()
		{
			return roots;
		}

		private void BackLobby(object arg, Action<object> next)
		{
			EnableScene();
		}

		private void EnterLobby(object arg, Action<object> next)
		{
			EnableScene();
		}
	}
}
