using M__M.GameHall.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace M__M.HaiWang.Demo
{
	public class LoadingLogic : MonoBehaviour
	{
		private static LoadingLogic s_instance;

		public AudioClip[] array;

		[SerializeField]
		public List<GameObject> games;

		public string sceneName;

		[SerializeField]
		private bool m_autoLoadingOnStart;

		public string lobbyScene = "FK3_DaTing";

		public string gameScene = "FK3_Game2";

		public static LoadingLogic Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
			AppSceneMgr.scene_loading_name = sceneName;
			AppSceneMgr.RegisterScene(sceneName);
			Screen.sleepTimeout = -1;
		}

		private void Start()
		{
			if (_CheckDependence())
			{
				if (m_autoLoadingOnStart)
				{
					_StartLoading();
				}
				HW2_Singleton<SoundMgr>.Get().LoadSounds(array);
				HW2_Singleton<SoundMgr>.Get().ChangeValue(1f);
			}
		}

		public GameObject FindGame(string name)
		{
			UnityEngine.Debug.LogError("games.Count: " + Get().games.Count);
			for (int i = 0; i < games.Count; i++)
			{
				if (games[i].name == name)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(games[i]);
					gameObject.name = name;
					return gameObject;
				}
			}
			UnityEngine.Debug.LogError("未找到: " + name);
			return null;
		}

		private bool _CheckDependence()
		{
			bool result = true;
			try
			{
				return result;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
				return false;
			}
		}

		private void _StartLoading()
		{
			StartCoroutine(IE_Loading());
		}

		private IEnumerator IE_Loading()
		{
			yield return null;
			float disPlayProgress = 0f;
			AsyncOperation op2 = SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
			op2.allowSceneActivation = false;
			float progress = 0f;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			float toProgress;
			while (progress < 0.9f)
			{
				progress = op2.progress;
				toProgress = progress * 100f;
				while (disPlayProgress < toProgress)
				{
					disPlayProgress += 1f;
					yield return new WaitForEndOfFrame();
				}
				yield return null;
			}
			toProgress = 100f;
			while (disPlayProgress < toProgress)
			{
				disPlayProgress += 1f;
				yield return new WaitForEndOfFrame();
			}
			yield return null;
			op2.allowSceneActivation = true;
			yield return null;
			AppSceneMgr.RunAction("Lobby.EnterLobby", null, delegate
			{
			});
			HW2_MB_Singleton<HW2_AppManager>.Get().StartApp();
			stopwatch.Stop();
			UnityEngine.Debug.Log(HW2_LogHelper.Orange($"加载耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		private IEnumerator IE_TestProgressBar()
		{
			float duration = 3f;
			float timer = 0f;
			while (timer < duration)
			{
				timer += Time.deltaTime;
				float num = timer / duration;
				yield return null;
			}
		}
	}
}
