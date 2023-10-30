using System;
using UnityEngine;
using UnityEngine.UI;

public class PHG_HeadViewController : PHG_MB_Singleton<PHG_HeadViewController>
{
	private GameObject _goUIContainer;

	private Text _textHeadInfo;

	private Text _textNickName;

	private Text _textLevelInfo;

	private Text _textGoldCoin;

	private Text _textExpeCoin;

	private Image _imgUserIcon;

	private Button btnBack;

	[SerializeField]
	private CSF_PersonIcon personIcon;

	private string _headInfo;

	public Action btnReturnAction;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		_textHeadInfo = base.transform.Find("TxtTitelName").GetComponent<Text>();
		_textNickName = base.transform.Find("TxtUserName").GetComponent<Text>();
		_textLevelInfo = base.transform.Find("TxtUserLvel").GetComponent<Text>();
		_textGoldCoin = base.transform.Find("TxtCoin").GetComponent<Text>();
		_textExpeCoin = base.transform.Find("TxtTestCoin").GetComponent<Text>();
		_imgUserIcon = base.transform.Find("ImgPersonIcon").GetComponent<Image>();
		btnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		if (PHG_MB_Singleton<PHG_HeadViewController>._instance == null)
		{
			PHG_MB_Singleton<PHG_HeadViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnBack.onClick.AddListener(PHG_MB_Singleton<PHG_GameManager>.GetInstance().Handle_BtnReturn);
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
		PHG_Utils.TrySetActive(_goUIContainer, active: true);
	}

	public void Hide()
	{
		PHG_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public void UpdateView()
	{
		_textHeadInfo.text = _headInfo;
		_textNickName.text = PHG_GVars.user.nickname;
		_textGoldCoin.text = PHG_GVars.user.gameGold.ToString();
		_textExpeCoin.text = PHG_GVars.user.expeGold.ToString();
		SetLevelInfo(PHG_GVars.user.level);
		_imgUserIcon.sprite = personIcon.spiIcons[PHG_GVars.user.photoId - 1];
	}

	public void UpdateView(PHG_User user)
	{
		_textHeadInfo.text = _headInfo;
		_textNickName.text = user.nickname;
		_textGoldCoin.text = user.gameGold.ToString();
		_textExpeCoin.text = user.expeGold.ToString();
		SetLevelInfo(user.level);
		_imgUserIcon.sprite = personIcon.spiIcons[user.photoId - 1];
	}

	public void SetLevelInfo(int level)
	{
		_textLevelInfo.text = "LV. " + level + "   " + PHG_Utils.GetLevelName(level, PHG_GVars.language);
	}

	public void OnBtnReturn_Click()
	{
		PHG_SoundManager.Instance.PlayClickAudio();
		if (btnReturnAction != null)
		{
			btnReturnAction();
		}
	}

	public void SetInfo(string info)
	{
		_headInfo = info;
		_textHeadInfo.text = info;
	}

	public void UpdateExpeAndGold()
	{
		_textGoldCoin.text = PHG_GVars.user.gameGold.ToString();
		_textExpeCoin.text = PHG_GVars.user.expeGold.ToString();
	}

	public Sprite[] GetUserIconSprites()
	{
		return personIcon.spiIcons;
	}
}
