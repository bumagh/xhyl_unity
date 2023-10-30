using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class FK3_EnterLobbyLogic : FK3_MB_Singleton<FK3_EnterLobbyLogic>
	{
		public string sceneName;

		public List<GameObject> roots;

		private Coroutine deltaPlayBg;

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_EnterLobbyLogic>._instance == null)
			{
				FK3_MB_Singleton<FK3_EnterLobbyLogic>.SetInstance(this);
			}
			FK3_AppSceneMgr.scene_lobby_name = sceneName;
			FK3_AppSceneMgr.RegisterScene(sceneName);
			FK3_AppSceneMgr.RegisterAction("Lobby.EnterLobby", EnterLobby);
			if (!FK3_AppSceneMgr.isFirstScene(sceneName))
			{
				DisableScene();
			}
		}

		private void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(FK3_MB_Singleton<FK3_NetManager>.Get().gameObject);
			FK3_GVars.dontDestroyOnLoadList.Add(FK3_MB_Singleton<FK3_NetManager>.Get().gameObject);
		}

		private void EnterLobby(object arg, Action<object> next)
		{
			EnableScene();
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame)
			{
				FK3_GVars.m_curState = FK3_Demo_UI_State.DeskSelection;
			}
		}

		public void DisableScene()
		{
			List<GameObject> list = _GetRoots();
			foreach (GameObject item in list)
			{
				if (item != null)
				{
					item.SetActive(value: false);
				}
			}
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("深海八爪鱼剧情背景音乐"));
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("霸王蟹剧情背景音乐"));
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("暗夜炬兽背景音乐"));
			FK3_Singleton<FK3_SoundMgr>.Get().StopClip(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("史前巨鳄背景音乐"));
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
			FK3_UIRoomManager.GetInstance().OpenUI("AlertDialog");
			FK3_UIRoomManager.GetInstance().CloseUI("AlertDialog");
			FK3_UIRoomManager.GetInstance().OpenUI("Notice");
			if (deltaPlayBg != null)
			{
				StopCoroutine(deltaPlayBg);
			}
			deltaPlayBg = StartCoroutine(DeltaPlayBg());
		}

		private IEnumerator DeltaPlayBg()
		{
			yield return new WaitForSeconds(0.1f);
			SetBG();
		}

		private List<GameObject> _GetRoots()
		{
			return roots;
		}

		private void SetBG()
		{
			switch (UnityEngine.Random.Range(0, 3))
			{
			case 0:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("深海八爪鱼剧情背景音乐", loop: true);
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("深海八爪鱼剧情背景音乐", 1f);
				break;
			case 1:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("霸王蟹剧情背景音乐", loop: true);
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("霸王蟹剧情背景音乐", 1f);
				break;
			case 2:
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("暗夜炬兽背景音乐", loop: true);
				FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("暗夜炬兽背景音乐", 1f);
				break;
			}
		}
	}
}
