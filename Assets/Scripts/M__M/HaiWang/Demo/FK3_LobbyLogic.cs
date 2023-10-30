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
	public class FK3_LobbyLogic : MonoBehaviour
	{
		public string sceneName;

		public List<GameObject> roots;

		private void Awake()
		{
			FK3_AppSceneMgr.scene_lobby_name = sceneName;
			FK3_AppSceneMgr.RegisterScene(sceneName);
			try
			{
				FK3_AppSceneMgr.RegisterAction("Lobby.BackLobby", BackLobby);
				FK3_AppSceneMgr.RegisterAction("Lobby.EnterLobby", EnterLobby);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			if (!FK3_AppSceneMgr.isFirstScene(sceneName))
			{
				DisableScene();
			}
		}

		private void Start()
		{
			MonoBehaviour.print(sceneName + ".Start");
			FK3_GVars.SetLobbyContext(FK3_LobbyUI.Get().GetContext());
			FK3_GVars.SetUserInfo(FK3_GVars.lobby.user);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("enterRoom", HandleNetMsg_EnterRoom);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("requestSeat", HandleNetMsg_RequestSeat);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("leaveRoom", HandleNetMsg_LeaveRoom);
			FK3_LobbyUI.Get().EventHandler_UI_Login_OnBtnLoginClick += Handler_UI_Login_OnBtnLoginClick;
			FK3_LobbyUI.Get().EventHandler_UI_RoomSelection_OnRoomClick += Handler_UI_RoomSelection_OnRoomClick;
			FK3_LobbyUI.Get().EventHandler_UI_DeskSelection_OnSeatClick += Handler_UI_DeskSelection_OnSeatClick;
			FK3_LobbyUI.Get().EventHandler_UI_DeskSelection_OnBtnReturnClick += Handler_UI_DeskSelection_OnBtnReturnClick;
			FK3_LobbyUI.Get().Change_UI_State(FK3_Demo_UI_State.Login);
			UnityEngine.Object.DontDestroyOnLoad(FK3_MB_Singleton<FK3_NetManager>.Get().gameObject);
			FK3_GVars.dontDestroyOnLoadList.Add(FK3_MB_Singleton<FK3_NetManager>.Get().gameObject);
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

		private void Handler_UI_Login_OnBtnLoginClick(FK3_LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_Login_OnBtnLoginClick");
			string strAccount = context.strAccount;
			string strPassword = context.strPassword;
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/login", new object[2]
			{
				strAccount,
				strPassword
			}, check: false);
		}

		private void Handler_UI_RoomSelection_OnRoomClick(FK3_LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_RoomSelection_OnRoomClick");
			int curRoomId = context.curRoomId;
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/enterRoom", new object[1]
			{
				curRoomId
			});
		}

		private void Handler_UI_DeskSelection_OnSeatClick(FK3_LobbyContext context)
		{
			UnityEngine.Debug.Log("Handler_UI_DeskSelection_OnSeatClick");
			int curDeskId = context.curDeskId;
			int curSeatId = context.curSeatId;
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/requestSeat", new object[2]
			{
				curDeskId,
				curSeatId
			});
		}

		private void Handler_UI_DeskSelection_OnBtnReturnClick(FK3_LobbyContext context)
		{
			int curRoomId = context.curRoomId;
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/leaveRoom", new object[1]
			{
				curRoomId
			});
		}

		private void HandleNetMsg_Login(object[] args)
		{
			FK3_NetMsgInfo_RecvLogin fK3_NetMsgInfo_RecvLogin = new FK3_NetMsgInfo_RecvLogin(args);
			fK3_NetMsgInfo_RecvLogin.Parse();
			fK3_NetMsgInfo_RecvLogin.user.CopyTo(FK3_LobbyUI.Get().GetContext().user);
			if (fK3_NetMsgInfo_RecvLogin.valid && fK3_NetMsgInfo_RecvLogin.code == 0)
			{
				FK3_LobbyUI.Get().Change_UI_State(FK3_Demo_UI_State.RoomSelection);
			}
		}

		private void HandleNetMsg_EnterRoom(object[] args)
		{
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
				FK3_LobbyUI.Get().GetContext().desks = fK3_NetMsgInfo_RecvEnterRoom.desks;
				UnityEngine.Debug.Log($"desk count {fK3_NetMsgInfo_RecvEnterRoom.desks.Count}");
				FK3_LobbyUI.Get().Change_UI_State(FK3_Demo_UI_State.DeskSelection);
			}
		}

		private void HandleNetMsg_RequestSeat(object[] args)
		{
			FK3_NetMsgInfo_RecvRequestSeat fK3_NetMsgInfo_RecvRequestSeat = new FK3_NetMsgInfo_RecvRequestSeat(args);
			fK3_NetMsgInfo_RecvRequestSeat.Parse();
			if (fK3_NetMsgInfo_RecvRequestSeat.valid && fK3_NetMsgInfo_RecvRequestSeat.code == 0)
			{
				FK3_LobbyUI.Get().GetContext().curSeatId = fK3_NetMsgInfo_RecvRequestSeat.seatId;
				FK3_LobbyUI.Get().GetContext().inGameSeats = fK3_NetMsgInfo_RecvRequestSeat.seats;
				DisableScene();
				FK3_AppSceneMgr.RunAction("Game.EnterGame", new object[3]
				{
					fK3_NetMsgInfo_RecvRequestSeat.sceneId,
					fK3_NetMsgInfo_RecvRequestSeat.stageId,
					fK3_NetMsgInfo_RecvRequestSeat.stageType
				}, delegate
				{
				});
			}
		}

		private void HandleNetMsg_LeaveRoom(object[] args)
		{
			FK3_LobbyUI.Get().Change_UI_State(FK3_Demo_UI_State.RoomSelection);
		}

		private IEnumerator IE_Flow()
		{
			string empty = string.Empty;
			string text = "xlcs.swccd88.xyz";
			int num = 10100;
			UnityEngine.Debug.LogError($"==========IE_Flow: {text}:{num}");
			FK3_MB_Singleton<FK3_NetManager>.Get().Connect(text, num);
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
					FK3_GVars.dontDestroyOnLoadList.Add(item.gameObject);
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
