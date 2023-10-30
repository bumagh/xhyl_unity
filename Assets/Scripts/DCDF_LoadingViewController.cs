using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_LoadingViewController : DCDF_MB_Singleton<DCDF_LoadingViewController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	[SerializeField]
	private Image imgPregress;

	[SerializeField]
	private Text _textPercent;

	private AsyncOperation async;

	private bool isLoadLevelFinish;

	public event Action loadingFinishAction;

	public event Action loadLevelFinishAction;

	private void Awake()
	{
		if (DCDF_MB_Singleton<DCDF_LoadingViewController>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_LoadingViewController>.SetInstance(this);
			PreInit();
		}
		if (DCDF_MB_Singleton<DCDF_LoadingViewController>._instance != this)
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
	}

	public void Show()
	{
		DCDF_Utils.TrySetActive(_goUIContainer, active: true);
		DCDF_MySqlConnection.curView = "LoadingView";
	}

	public void Hide()
	{
		DCDF_Utils.TrySetActive(_goUIContainer, active: false);
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
		while (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isConnected)
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
		while (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isLogined)
		{
			yield return null;
		}
		this.loadingFinishAction();
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	private void _setProgress(float percent)
	{
		imgPregress.fillAmount = percent;
		_textPercent.text = $"玩命加载中{Mathf.Min(100, Mathf.CeilToInt(percent * 100f))}%";
	}
}
