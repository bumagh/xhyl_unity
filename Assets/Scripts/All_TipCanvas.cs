using System;
using UnityEngine;

public class All_TipCanvas : MonoBehaviour
{
	public AudioClip clip;

	private static All_TipCanvas publicTipCanvas;

	private Transform safeBoxPwdCheckPanel;

	private Transform payPanel;

	private Transform safeBoxPanel;

	private AudioSource source;

	public static All_TipCanvas GetInstance()
	{
		if (publicTipCanvas == null)
		{
			GameObject gameObject = new GameObject();
			gameObject = (Resources.Load("TipsCanvas") as GameObject);
			gameObject = Instantiate(gameObject);
			publicTipCanvas = gameObject.transform.GetComponent<All_TipCanvas>();
			publicTipCanvas.Awake2();
			Debug.LogError(publicTipCanvas.name);
		}
		return publicTipCanvas;
	}

	private void Awake2()
	{
		publicTipCanvas = this;
		DontDestroyOnLoad(gameObject);
		ZH2_GVars.OpenCheckSafeBoxPwdPanel = (Action<ZH2_GVars.GameType_DJ>)Delegate.Combine(ZH2_GVars.OpenCheckSafeBoxPwdPanel, new Action<ZH2_GVars.GameType_DJ>(OpenCheckSafeBoxPwdPanel));
		ZH2_GVars.OpenSafeBoxPwdPanel = (Action)Delegate.Combine(ZH2_GVars.OpenSafeBoxPwdPanel, new Action(OpenSafeBoxPwdPanel));
		ZH2_GVars.OpenPlyBoxPanel = (Action<ZH2_GVars.GameType_DJ>)Delegate.Combine(ZH2_GVars.OpenPlyBoxPanel, new Action<ZH2_GVars.GameType_DJ>(OpenPlyBoxPwdPanel));
		GetPanel();
	}

	private void OnDestroy()
	{
		ZH2_GVars.OpenCheckSafeBoxPwdPanel = (Action<ZH2_GVars.GameType_DJ>)Delegate.Remove(ZH2_GVars.OpenCheckSafeBoxPwdPanel, new Action<ZH2_GVars.GameType_DJ>(OpenCheckSafeBoxPwdPanel));
		ZH2_GVars.OpenSafeBoxPwdPanel = (Action)Delegate.Remove(ZH2_GVars.OpenSafeBoxPwdPanel, new Action(OpenSafeBoxPwdPanel));
		ZH2_GVars.OpenPlyBoxPanel = (Action<ZH2_GVars.GameType_DJ>)Delegate.Remove(ZH2_GVars.OpenPlyBoxPanel, new Action<ZH2_GVars.GameType_DJ>(OpenPlyBoxPwdPanel));
	}

	private void GetPanel()
	{
		if (safeBoxPwdCheckPanel == null)
		{
			safeBoxPwdCheckPanel = base.transform.Find("SafeBoxPwdCheckPanel");
		}
		if (payPanel == null)
		{
			payPanel = base.transform.Find("PayPanel");
		}
		if (safeBoxPanel == null)
		{
			safeBoxPanel = base.transform.Find("SafeBoxPanel");
		}
		if (source == null)
		{
			source = GetComponent<AudioSource>();
		}
	}

	public void SourcePlayClip()
	{
		source.clip = clip;
		source.loop = false;
		source.volume = 1f;
		source.Play();
	}

	private void OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ openType)
	{
		GetPanel();
		ZH2_GVars.allSetType = (int)openType;
		safeBoxPwdCheckPanel.gameObject.SetActive(value: true);
	}

	private void OpenPlyBoxPwdPanel(ZH2_GVars.GameType_DJ openType)
	{
		GetPanel();
		ZH2_GVars.allSetType = (int)openType;
		payPanel.gameObject.SetActive(value: true);
	}

	private void OpenSafeBoxPwdPanel()
	{
		GetPanel();
		safeBoxPanel.gameObject.SetActive(value: true);
	}

	public bool IsPayPanelActive()
	{
		GetPanel();
		return payPanel.gameObject.activeInHierarchy;
	}

	public bool IsCheckPwdActive()
	{
		GetPanel();
		return safeBoxPwdCheckPanel.gameObject.activeInHierarchy || safeBoxPanel.gameObject.activeInHierarchy;
	}
}
