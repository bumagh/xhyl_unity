using System;
using UnityEngine;
using UnityEngine.UI;

public class PTM_HUDController : PTM_MB_Singleton<PTM_HUDController>
{
	private GameObject _goContainer;

	private Text txtTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private PTM_RulePicController ruleController;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		txtTime = base.transform.Find("TxtTime").GetComponent<Text>();
		ImgWifi = base.transform.Find("ImgWifi").GetComponent<Image>();
		if (PTM_MB_Singleton<PTM_HUDController>._instance == null)
		{
			PTM_MB_Singleton<PTM_HUDController>.SetInstance(this);
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
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<PTM_RulePicController>();
		}
	}

	public void Init()
	{
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().onItemRules = delegate
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
		PTM_SoundManager.Instance.PlayClickAudio();
		if (!PTM_LockManager.IsLocked("btn_options"))
		{
			if (PTM_MB_Singleton<PTM_OptionsController>.GetInstance().isShow)
			{
				PTM_MB_Singleton<PTM_OptionsController>.GetInstance().Hide();
			}
			else
			{
				PTM_MB_Singleton<PTM_OptionsController>.GetInstance().Show();
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
