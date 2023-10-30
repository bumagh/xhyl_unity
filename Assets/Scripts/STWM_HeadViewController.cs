using System;
using UnityEngine;
using UnityEngine.UI;

public class STWM_HeadViewController : STWM_MB_Singleton<STWM_HeadViewController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	[SerializeField]
	public Text _textHeadInfo;

	[SerializeField]
	public Text _textGoldCoin;

	[SerializeField]
	public Text _textTestGoldCoin;

	[SerializeField]
	private Image _imgUserIcon;

	[SerializeField]
	private STWM_PersonIcon personIcon;

	public Action btnReturnAction;

	public Button btnBlack;

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_HeadViewController>._instance == null)
		{
			STWM_MB_Singleton<STWM_HeadViewController>.SetInstance(this);
			PreInit();
		}
		btnBlack = base.transform.Find("BtnBack").GetComponent<Button>();
		if (btnBlack != null)
		{
			btnBlack.onClick.AddListener(OnBtnReturn_Click);
		}

		_textHeadInfo.text = ZH2_GVars.ShowTip("水浒传", "WaterMargin", "WaterMargin", "Thủy Hử");
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
	}

	public void Hide()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public void UpdateView()
	{
		_textGoldCoin.text = STWM_GVars.user.gameGold.ToString();
		_textTestGoldCoin.text = STWM_GVars.user.expeGold.ToString();
		SetLevelInfo(STWM_GVars.user.level);
		int num = STWM_GVars.user.photoId - 1;
		if (num >= personIcon.spiIcons.Length || num < 0)
		{
			num = 0;
		}
		_imgUserIcon.sprite = personIcon.spiIcons[num];
		if (ZH2_GVars.hallInfo != null && STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().hallInfo != ZH2_GVars.hallInfo)
		{
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().ShowHall();
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().hallInfo = ZH2_GVars.hallInfo;
		}
	}

	public void UpdateView(STWM_User user)
	{
		_textGoldCoin.text = user.gameGold.ToString();
		_textTestGoldCoin.text = user.expeGold.ToString();
		SetLevelInfo(user.level);
		_imgUserIcon.sprite = personIcon.spiIcons[user.photoId - 1];
	}

	public void SetLevelInfo(int level)
	{
		STWM_Utils.GetLevelName(level, STWM_GVars.language);
	}

	public void OnBtnReturn_Click()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		if (btnReturnAction != null)
		{
			btnReturnAction();
		}
	}

	public void SetInfo(string info)
	{
		_textHeadInfo.text = info;
	}

	public void UpdateExpeAndGold()
	{
		_textGoldCoin.text = STWM_GVars.user.gameGold.ToString();
		_textTestGoldCoin.text = STWM_GVars.user.expeGold.ToString();
	}

	public Sprite[] GetUserIconSprites()
	{
		return personIcon.spiIcons;
	}
}
