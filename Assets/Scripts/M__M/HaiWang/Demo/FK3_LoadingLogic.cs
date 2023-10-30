using M__M.GameHall.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace M__M.HaiWang.Demo
{
	public class FK3_LoadingLogic : MonoBehaviour
	{
		private static FK3_LoadingLogic s_instance;

		public AudioClip[] array;

		[SerializeField]
		public List<GameObject> games;

		public string sceneName;

		[SerializeField]
		private bool m_autoLoadingOnStart;

		public string lobbyScene = "Lobby";

		public string gameScene = "MainGame3";

		public static FK3_LoadingLogic Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			FK3_GVars.dontDestroyOnLoadList.Add(base.gameObject);
			FK3_AppSceneMgr.scene_loading_name = sceneName;
			FK3_AppSceneMgr.RegisterScene(sceneName);
			Screen.sleepTimeout = -1;
		}

		private void Start()
		{
			if (m_autoLoadingOnStart)
			{
				_StartLoading();
			}
			FK3_Singleton<FK3_SoundMgr>.Get().LoadSounds(array);
			FK3_Singleton<FK3_SoundMgr>.Get().ChangeValue(1f);
		}

		public GameObject FindGame(string name)
		{
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
			return true;
		}

		private void _StartLoading()
		{
			FK3_MB_Singleton<FK3_AppManager>.Get().StartApp();
			StartCoroutine(IE_Loading());
		}

		private IEnumerator IE_Loading()
		{
			yield return null;
			AsyncOperation op2 = SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
			op2.allowSceneActivation = false;
			yield return null;
			op2.allowSceneActivation = true;
			yield return null;
			FK3_AppSceneMgr.RunAction("Lobby.EnterLobby", null, delegate
			{
			});
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
