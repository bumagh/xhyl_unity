using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class EnterLobbyLogic : HW2_MB_Singleton<EnterLobbyLogic>
	{
		public string sceneName;

		public List<GameObject> roots;

		private Coroutine deltaPlayBg;

		private void Awake()
		{
			if (HW2_MB_Singleton<EnterLobbyLogic>._instance == null)
			{
				HW2_MB_Singleton<EnterLobbyLogic>.SetInstance(this);
			}
			AppSceneMgr.scene_lobby_name = sceneName;
			AppSceneMgr.RegisterScene(sceneName);
			AppSceneMgr.RegisterAction("Lobby.EnterLobby", EnterLobby);
			if (!AppSceneMgr.isFirstScene(sceneName))
			{
				DisableScene();
			}
		}

		private void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(HW2_MB_Singleton<HW2_NetManager>.Get().gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(HW2_MB_Singleton<HW2_NetManager>.Get().gameObject);
		}

		private void EnterLobby(object arg, Action<object> next)
		{
			EnableScene();
			if (HW2_GVars.m_curState == Demo_UI_State.InGame)
			{
				HW2_GVars.m_curState = Demo_UI_State.DeskSelection;
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
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("深海八爪鱼剧情背景音乐"));
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("霸王蟹剧情背景音乐"));
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("暗夜炬兽背景音乐"));
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
			UIRoomManager.GetInstance().OpenUI("AlertDialog");
			UIRoomManager.GetInstance().CloseUI("AlertDialog");
			UIRoomManager.GetInstance().OpenUI("Notice");
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
				HW2_Singleton<SoundMgr>.Get().PlayClip("深海八爪鱼剧情背景音乐", loop: true);
				HW2_Singleton<SoundMgr>.Get().SetVolume("深海八爪鱼剧情背景音乐", 1f);
				break;
			case 1:
				HW2_Singleton<SoundMgr>.Get().PlayClip("霸王蟹剧情背景音乐", loop: true);
				HW2_Singleton<SoundMgr>.Get().SetVolume("霸王蟹剧情背景音乐", 1f);
				break;
			case 2:
				HW2_Singleton<SoundMgr>.Get().PlayClip("暗夜炬兽背景音乐", loop: true);
				HW2_Singleton<SoundMgr>.Get().SetVolume("暗夜炬兽背景音乐", 1f);
				break;
			}
		}
	}
}
