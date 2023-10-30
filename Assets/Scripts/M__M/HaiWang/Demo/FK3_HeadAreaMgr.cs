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
	public class FK3_HeadAreaMgr : FK3_BaseUIForm
	{
		private static FK3_HeadAreaMgr _instance;

		[SerializeField]
		private Image _headIconImage;

		[SerializeField]
		private Text _userGameGlodText;

		[SerializeField]
		private Text _userExpText;

		private Vector3 upOldPos;

		private Vector3 downOldPos;

		public static FK3_HeadAreaMgr Get()
		{
			return _instance;
		}

		private void Awake()
		{
			_instance = this;
			uiType.uiFormType = FK3_UIFormTypes.Normal;
			Init();
		}

		public void Init()
		{
			try
			{
				if (FK3_MB_Singleton<FK3_GameUIController>.Get() != null && FK3_GVars.user.photoId >= FK3_MB_Singleton<FK3_GameUIController>.Get()._icon.Length)
				{
					FK3_GVars.user.photoId = 1;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
			try
			{
				if (FK3_MB_Singleton<FK3_GameUIController>.Get() != null)
				{
					_headIconImage.sprite = FK3_MB_Singleton<FK3_GameUIController>.Get()._icon[(FK3_GVars.user.photoId < 1) ? 1 : FK3_GVars.user.photoId];
				}
			}
			catch (Exception message2)
			{
				UnityEngine.Debug.Log(message2);
			}
			try
			{
				_userGameGlodText.text = FK3_GVars.user.gameGold.ToString();
			}
			catch (Exception message3)
			{
				UnityEngine.Debug.Log(message3);
			}
		}

		public void OnBtnBack_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("返回界面弹出音效");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("返回界面弹出音效", 1f);
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.RoomSelection)
			{
				UnityEngine.Debug.Log("m_curState：" + FK3_GVars.m_curState);
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("是否退出程序？", "Whether to exit the program?", string.Empty), showOkCancel: true, delegate
				{
					QuitToHallGame();
				});
			}
			else if (FK3_GVars.m_curState == FK3_Demo_UI_State.DeskSelection)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/leaveRoom", new object[1]
				{
					FK3_GVars.lobby.curRoomId
				});
				FK3_TableController.Get().itemList.Clear();
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
			AssetBundleManager.GetInstance().UnloadAB("HaiW3");
			SceneManager.LoadScene("MainScene");
		}

		public void Send_QuitGame()
		{
			object[] args = new object[0];
			FK3_MB_Singleton<FK3_NetManager>.GetInstance().Send("userService/quitGame", args);
			UnityEngine.Debug.LogError("卸载DontDestroyOnLoad");
			for (int i = 0; i < FK3_GVars.dontDestroyOnLoadList.Count; i++)
			{
				try
				{
					UnityEngine.Object.Destroy(FK3_GVars.dontDestroyOnLoadList[i]);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
			FK3_Singleton<FK3_SoundMgr>.Get().UnloadAllSounds();
			FK3_Singleton<FK3_SoundMgr>.OnApplicationQuit();
			FK3_UserInfoShow.Get().OnQuit();
			FK3_RoomMgr.GetInstance().OnQuit();
			FK3_UIRoomManager.OnQuit();
			FK3_GVars.m_curState = FK3_Demo_UI_State.StartupLoading;
			FK3_MB_Singleton<FK3_GameUIController>.Quit();
			FK3_MB_Singleton<FK3_NetManager>.Quit();
			FK3_MB_Singleton<FK3_GetDaTingRoot>.Quit();
			FK3_MessageCenter.messageList = new Dictionary<string, FK3_MessageCenter.HandleMessage>();
			FK3_GVars.dontDestroyOnLoadList = new List<GameObject>();
		}
	}
}
