using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class STWM_LoadingViewController : STWM_MB_Singleton<STWM_LoadingViewController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	private AsyncOperation async;

	private GameObject _goLoadingCanvas;

	private bool isLoadLevelFinish;

	public event Action loadingFinishAction;

	public event Action loadLevelFinishAction;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_LoadingViewController>._instance == null)
		{
			STWM_MB_Singleton<STWM_LoadingViewController>.SetInstance(this);
			PreInit();
		}
		if (STWM_MB_Singleton<STWM_LoadingViewController>._instance != this)
		{
			Hide();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	private void Start()
	{
	}

	public void PreInit()
	{
		if (_goUIContainer == null)
		{
			_goUIContainer = base.gameObject;
		}
	}

	public void Show()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: true);
		STWM_GVars.curView = "LoadingView";
	}

	public void Hide()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public IEnumerator LoadingAni()
	{
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isLogined)
		{
			yield return null;
		}
		this.loadingFinishAction();
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	[Obsolete]
	public IEnumerator LoadingAni2()
	{
		UnityEngine.Debug.Log("[LoadingAni2] start");
		_goLoadingCanvas = GameObject.Find("LoadingCanvas");
		yield return null;
		yield return new WaitForSeconds(0.2f);
		async = SceneManager.LoadSceneAsync("Main");
		async.allowSceneActivation = false;
		StartCoroutine(_waitLoadLevelFinish());
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(0.1f);
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isLogined)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(0.1f);
		async.allowSceneActivation = true;
		yield return null;
		UnityEngine.Debug.Log("_waitLoadLevelFinish 3");
		while (!isLoadLevelFinish)
		{
			yield return null;
		}
		UnityEngine.Debug.Log("LoadingAni2 2");
		this.loadingFinishAction();
		UnityEngine.Object.DestroyImmediate(_goLoadingCanvas);
	}

	private IEnumerator _waitLoadLevelFinish()
	{
		UnityEngine.Debug.Log("_waitLoadLevelFinish");
		while (async != null && !async.isDone)
		{
			yield return null;
		}
		UnityEngine.Debug.Log("_waitLoadLevelFinish " + async);
		if (!isLoadLevelFinish && this.loadLevelFinishAction != null)
		{
			isLoadLevelFinish = true;
			this.loadLevelFinishAction();
		}
	}
}
