using System;
using UnityEngine;
using UnityEngine.UI;

public class STWM_HUDController : STWM_MB_Singleton<STWM_HUDController>
{
	[SerializeField]
	private GameObject _goContainer;

	[SerializeField]
	private Button btnOptions;

	[SerializeField]
	private Text txtDeskName;

	[SerializeField]
	private Text txtTime;

	[SerializeField]
	private Image imgArrow;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private STWM_RulePicController ruleController;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_HUDController>._instance == null)
		{
			STWM_MB_Singleton<STWM_HUDController>.SetInstance(this);
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
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<STWM_RulePicController>();
		}
	}

	public void Init()
	{
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemRules = delegate
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
		txtDeskName.text = STWM_GVars.desk.name;
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private void _updateCurrentTime()
	{
		DateTime now = DateTime.Now;
		txtTime.text = $"{now.Hour:D2}:{now.Minute:D2}";
	}

	public void OnBtnOptions_Click()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		if (!STWM_LockManager.IsLocked("btn_options"))
		{
			if (STWM_MB_Singleton<STWM_OptionsController>.GetInstance().isShow)
			{
				imgArrow.sprite = _sprites[0];
				STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
			}
			else
			{
				imgArrow.sprite = _sprites[1];
				STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Show();
			}
		}
	}

	public void ResetSprite()
	{
		imgArrow.sprite = _sprites[0];
	}

	public void HideRules()
	{
		ruleController.Hide();
	}

	public void SetBtnOptionsEnable(bool isEnable)
	{
		btnOptions.interactable = isEnable;
	}
}
