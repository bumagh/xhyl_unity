using System;
using UnityEngine;
using UnityEngine.UI;

public class LLD_HUDController : LLD_MB_Singleton<LLD_HUDController>
{
	private GameObject _goContainer;

	private Text txtTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private LLD_RulePicController ruleController;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		txtTime = base.transform.Find("TxtTime").GetComponent<Text>();
		ImgWifi = base.transform.Find("ImgWifi").GetComponent<Image>();
		if (LLD_MB_Singleton<LLD_HUDController>._instance == null)
		{
			LLD_MB_Singleton<LLD_HUDController>.SetInstance(this);
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
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<LLD_RulePicController>();
		}
	}

	public void Init()
	{
		LLD_MB_Singleton<LLD_OptionsController>.GetInstance().onItemRules = delegate
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
		LLD_SoundManager.Instance.PlayClickAudio();
		if (!LLD_LockManager.IsLocked("btn_options"))
		{
			if (LLD_MB_Singleton<LLD_OptionsController>.GetInstance().isShow)
			{
				LLD_MB_Singleton<LLD_OptionsController>.GetInstance().Hide();
			}
			else
			{
				LLD_MB_Singleton<LLD_OptionsController>.GetInstance().Show();
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
