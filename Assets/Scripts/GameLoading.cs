using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoading : MonoBehaviour
{
	public static string loadingSceneName = string.Empty;

	private static GameLoading g_instance;

	public PrefabsSO prefabs;

	private bool keepPrevious = true;

	public Action awakeTask;

	public Action startTask;

	private string m_hallSceneName = "TestSceneTransfer";

	private void Awake()
	{
		UnityEngine.Debug.LogFormat("GameLoading.Awake @{0} @id {1}", Time.frameCount, GetInstanceID());
		UnityEngine.Object.DontDestroyOnLoad(this);
		base.gameObject.name = "GameLoading";
		if (g_instance == null)
		{
			g_instance = this;
		}
		else if (keepPrevious)
		{
			if (this != g_instance)
			{
				UnityEngine.Debug.Log("Destroy new one");
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (this == g_instance)
		{
			UnityEngine.Debug.Log("Destroy old one");
			UnityEngine.Object.Destroy(g_instance.gameObject);
			g_instance = this;
		}
		UnityEngine.Debug.LogFormat("awakeTask is null: {0}, {1}", g_instance.awakeTask == null, GetInstanceID());
		if (g_instance.awakeTask != null)
		{
			g_instance.awakeTask();
			g_instance.awakeTask = null;
		}
	}

	private void Start()
	{
		UnityEngine.Debug.LogFormat("GameLoading.Start @{0}", Time.frameCount);
		Screen.orientation = ScreenOrientation.Portrait;
		if (startTask != null)
		{
			startTask();
			startTask = null;
		}
	}

	public static void LoadLoadingScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public static void LoadLoadingSceneAsync(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName);
	}

	public GameLoading Load()
	{
		SceneManager.LoadScene("_Scenes/GameLoading");
		return this;
	}

	public GameObject LoadLoadingPrefab(int index)
	{
		EnsurePrefabs();
		return UnityEngine.Object.Instantiate(prefabs[index]);
	}

	public void EnsurePrefabs()
	{
		if (prefabs == null)
		{
			prefabs = Resources.Load<PrefabsSO>("LoadingPrefabs");
		}
	}

	public static GameLoading Get()
	{
		return g_instance;
	}

	public static GameLoading Create()
	{
		GameObject gameObject = new GameObject();
		return gameObject.AddComponent<GameLoading>();
	}

	public GameLoading SetAwakeTask(Action task)
	{
		UnityEngine.Debug.LogFormat("GameLoading.SetAwakeTask @{0}", Time.frameCount);
		awakeTask = task;
		return this;
	}

	public GameLoading SetStartTask(Action task)
	{
		UnityEngine.Debug.LogFormat("GameLoading.SetStartTask @{0}", Time.frameCount);
		startTask = task;
		return this;
	}

	private void OnDestory()
	{
		UnityEngine.Debug.LogFormat("GameLoading.OnDestory @{0}", Time.frameCount);
		g_instance = null;
	}

	public void SetHallSceneName(string hallSceneName)
	{
		m_hallSceneName = hallSceneName;
	}

	public string GetHallSceneName()
	{
		return m_hallSceneName;
	}
}
