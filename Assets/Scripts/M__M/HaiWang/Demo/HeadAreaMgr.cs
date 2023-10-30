using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Message;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class HeadAreaMgr : BaseUIForm
	{
		private static HeadAreaMgr _instance;

		[SerializeField]
		private Image _headIconImage;

		[SerializeField]
		private Text _userGameGlodText;

		private Vector3 upOldPos;

		private Vector3 downOldPos;

		public static HeadAreaMgr Get()
		{
			return _instance;
		}

		private void Awake()
		{
			_instance = this;
			uiType.uiFormType = UIFormTypes.Normal;
			Init();
		}

		private void OnEnable()
		{
		}

		private void Update()
		{
		}

		public void Init()
		{
			try
			{
				if (HW2_GVars.user.photoId >= HW2_MB_Singleton<GameUIController>.Get()._icon.Length)
				{
					HW2_GVars.user.photoId = 1;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
			try
			{
				_headIconImage.sprite = HW2_MB_Singleton<GameUIController>.Get()._icon[(HW2_GVars.user.photoId < 1) ? 1 : HW2_GVars.user.photoId];
			}
			catch (Exception message2)
			{
				UnityEngine.Debug.Log(message2);
			}
			try
			{
				_userGameGlodText.text = HW2_GVars.user.gameGold.ToString();
			}
			catch (Exception message3)
			{
				UnityEngine.Debug.Log(message3);
			}
		}

		public void OnBtnBack_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("返回界面弹出音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume("返回界面弹出音效", 1f);
			if (HW2_GVars.m_curState == Demo_UI_State.RoomSelection)
			{
				UnityEngine.Debug.Log("m_curState：" + HW2_GVars.m_curState);
				HW2_AlertDialog.Get().ShowDialog("是否退出程序？", showOkCancel: true, delegate
				{
					QuitToHallGame();
				});
			}
			else if (HW2_GVars.m_curState == Demo_UI_State.DeskSelection)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/leaveRoom", new object[1]
				{
					HW2_GVars.lobby.curRoomId
				});
				TableController.Get().itemList.Clear();
			}
			else
			{
				UnityEngine.Debug.Log("返回按钮异常点击");
			}
		}

		public void QuitToHallGame(bool isToHall = true)
		{
			UnityEngine.Debug.LogError("返回大厅");
			Send_QuitGame();
			ZH2_GVars.isStartedFromGame = isToHall;
			SceneManager.LoadScene("MainScene");
		}

		public void Send_QuitGame()
		{
			object[] args = new object[0];
			HW2_MB_Singleton<HW2_NetManager>.GetInstance().Send("userService/quitGame", args);
			UnityEngine.Debug.LogError("卸载DontDestroyOnLoad");
			for (int i = 0; i < HW2_GVars.dontDestroyOnLoadList.Count; i++)
			{
				try
				{
					UnityEngine.Object.Destroy(HW2_GVars.dontDestroyOnLoadList[i]);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
			HW2_Singleton<SoundMgr>.Get().UnloadAllSounds();
			HW2_Singleton<SoundMgr>.OnApplicationQuit();
			HW2_UserInfoShow.Get().OnQuit();
			RoomMgr.GetInstance().OnQuit();
			UIRoomManager.OnQuit();
			HW2_GVars.m_curState = Demo_UI_State.StartupLoading;
			HW2_MB_Singleton<GameUIController>.Quit();
			HW2_MB_Singleton<HW2_NetManager>.Quit();
			HW2_MB_Singleton<GetDaTingRoot>.Quit();
			HW2_MessageCenter.messageList = new Dictionary<string, HW2_MessageCenter.HandleMessage>();
			HW2_GVars.dontDestroyOnLoadList = new List<GameObject>();
		}
	}
}
