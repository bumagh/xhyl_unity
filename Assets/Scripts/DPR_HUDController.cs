using System;
using UnityEngine;
using UnityEngine.UI;

public class DPR_HUDController : DPR_MB_Singleton<DPR_HUDController>
{
	private GameObject _goContainer;

	private Text txtTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private DPR_RulePicController ruleController;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		txtTime = base.transform.Find("TxtTime").GetComponent<Text>();
		ImgWifi = base.transform.Find("ImgWifi").GetComponent<Image>();
		if (DPR_MB_Singleton<DPR_HUDController>._instance == null)
		{
			DPR_MB_Singleton<DPR_HUDController>.SetInstance(this);
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
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<DPR_RulePicController>();
		}
	}

	public void Init()
	{
		DPR_MB_Singleton<DPR_OptionsController>.GetInstance().onItemRules = delegate
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
		DPR_SoundManager.Instance.PlayClickAudio();
		if (!DPR_LockManager.IsLocked("btn_options"))
		{
			if (DPR_MB_Singleton<DPR_OptionsController>.GetInstance().isShow)
			{
				DPR_MB_Singleton<DPR_OptionsController>.GetInstance().Hide();
			}
			else
			{
				DPR_MB_Singleton<DPR_OptionsController>.GetInstance().Show();
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
