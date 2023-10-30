using System;
using UnityEngine;
using UnityEngine.UI;

public class SPA_HUDController : SPA_MB_Singleton<SPA_HUDController>
{
	private GameObject _goContainer;

	private Text txtTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private SPA_RulePicController ruleController;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		txtTime = base.transform.Find("TxtTime").GetComponent<Text>();
		ImgWifi = base.transform.Find("ImgWifi").GetComponent<Image>();
		if (SPA_MB_Singleton<SPA_HUDController>._instance == null)
		{
			SPA_MB_Singleton<SPA_HUDController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
		if (ruleController == null)
		{
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<SPA_RulePicController>();
		}
	}

	public void Init()
	{
		SPA_MB_Singleton<SPA_OptionsController>.GetInstance().onItemRules = delegate
		{
			ruleController.Show();
		};
		ruleController.Hide();
	}

	public void Update()
	{
		_updateCurrentTime();
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private void _updateCurrentTime()
	{
		DateTime now = DateTime.Now;
		txtTime.text = $"{now.Hour:D2}:{now.Minute:D2}";
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			if (ImgWifi != null)
			{
				ImgWifi.sprite = _sprites[0];
			}
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && ImgWifi != null)
		{
			ImgWifi.sprite = _sprites[1];
		}
	}

	public void OnBtnOptions_Click()
	{
		SPA_SoundManager.Instance.PlayClickAudio();
		if (!SPA_LockManager.IsLocked("btn_options"))
		{
			if (SPA_MB_Singleton<SPA_OptionsController>.GetInstance().isShow)
			{
				SPA_MB_Singleton<SPA_OptionsController>.GetInstance().Hide();
			}
			else
			{
				SPA_MB_Singleton<SPA_OptionsController>.GetInstance().Show();
			}
		}
	}

	public void ResetSprite()
	{
	}

	public void HideRules()
	{
		ruleController.Hide();
	}

	public void SetBtnOptionsEnable(bool isEnable)
	{
	}
}
