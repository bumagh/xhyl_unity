using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LRS_LoadingViewController : LRS_MB_Singleton<LRS_LoadingViewController>
{
	private GameObject _goUIContainer;

	private Slider _progress;

	private Text _textPercent;

	private AsyncOperation async;

	private GameObject _goLoadingCanvas;

	private bool isLoadLevelFinish;

	public event Action loadingFinishAction;

	public event Action loadLevelFinishAction;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		_progress = base.transform.Find("Slider").GetComponent<Slider>();
		_textPercent = base.transform.Find("TxtProgress").GetComponent<Text>();
		if (LRS_MB_Singleton<LRS_LoadingViewController>._instance == null)
		{
			LRS_MB_Singleton<LRS_LoadingViewController>.SetInstance(this);
			PreInit();
		}
		if (LRS_MB_Singleton<LRS_LoadingViewController>._instance != this)
		{
			Hide();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	private void Start()
	{
		_setProgress(0f);
	}

	public void PreInit()
	{
		if (_goUIContainer == null)
		{
			_goUIContainer = base.gameObject;
		}
		base.transform.Find("TxtVersion").GetComponent<Text>().text = "10.0.1";
		base.transform.Find("TxtVersion1").GetComponent<Text>().text = "10代";
	}

	public void Show()
	{
		LRS_Utils.TrySetActive(_goUIContainer, active: true);
		LRS_GVars.curView = "LoadingView";
	}

	public void Hide()
	{
		LRS_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public IEnumerator LoadingAni()
	{
		float beginPercent = 0.01f;
		float percent = beginPercent;
		_setProgress(percent);
		yield return null;
		float connectDuration = 0.5f;
		float connectPercent = 0.5f;
		float targetPercent2 = connectPercent;
		float timer2 = 0f;
		while (timer2 < connectDuration)
		{
			percent += Time.deltaTime * targetPercent2 / connectDuration;
			timer2 += Time.deltaTime;
			_setProgress(percent);
			yield return null;
		}
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		float loginDuration = 0.5f;
		float loginPercent = 0.9f;
		timer2 = 0f;
		targetPercent2 = loginPercent - percent;
		while (timer2 < loginDuration)
		{
			percent += Time.deltaTime * targetPercent2 / loginDuration;
			timer2 += Time.deltaTime;
			_setProgress(percent);
			yield return null;
		}
		while (percent < 1.1f)
		{
			percent += Time.deltaTime;
			_setProgress(percent);
			yield return null;
		}
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isLogined)
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
		float percent = 0f;
		_setProgress(percent);
		yield return new WaitForSeconds(0.2f);
		async = SceneManager.LoadSceneAsync("Main");
		async.allowSceneActivation = false;
		StartCoroutine(_waitLoadLevelFinish());
		while (async.progress < 0.9f)
		{
			percent = Mathf.MoveTowards(percent, async.progress * 0.5f, 1f * Time.deltaTime);
			_setProgress(percent);
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		float connectDuration = 0.5f;
		float connectPercent = 0.7f;
		float targetPercent2 = connectPercent - percent;
		float timer2 = 0f;
		while (timer2 < connectDuration)
		{
			percent += Time.deltaTime * targetPercent2 / connectDuration;
			timer2 += Time.deltaTime;
			_setProgress(percent);
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		float loginDuration = 0.5f;
		float loginPercent = 0.9f;
		timer2 = 0f;
		targetPercent2 = loginPercent - percent;
		while (timer2 < loginDuration)
		{
			percent += Time.deltaTime * targetPercent2 / loginDuration;
			timer2 += Time.deltaTime;
			_setProgress(percent);
			yield return null;
		}
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isLogined)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.1f);
		float num = percent;
		float t = 0f;
		while (percent < 1f)
		{
			t += Time.deltaTime;
			percent = Mathf.Lerp(num, 1f, t / 0.1f);
			_setProgress(percent);
			yield return null;
		}
		_setProgress(1f);
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

	private void _setProgress(float percent)
	{
		percent += LRS_GVars.RandomTime;
		if (percent >= 1f)
		{
			percent = 1f;
		}
		_progress.value = percent;
		_textPercent.text = $"{Mathf.Min(100, Mathf.CeilToInt(percent * 100f))}%";
	}
}
