using System;
using UnityEngine;
using UnityEngine.UI;

public class CRL_HeadViewController : CRL_MB_Singleton<CRL_HeadViewController>
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
		if (CRL_MB_Singleton<CRL_HeadViewController>._instance == null)
		{
			CRL_MB_Singleton<CRL_HeadViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnBack.onClick.AddListener(CRL_MB_Singleton<CRL_GameManager>.GetInstance().Handle_BtnReturn);
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
		CRL_Utils.TrySetActive(_goUIContainer, active: true);
	}

	public void Hide()
	{
		CRL_Utils.TrySetActive(_goUIContainer, active: false);
	}

	public void UpdateView()
	{
		_textHeadInfo.text = _headInfo;
		_textNickName.text = CRL_MySqlConnection.user.nickname;
		_textGoldCoin.text = CRL_MySqlConnection.user.gameGold.ToString();
		_textExpeCoin.text = CRL_MySqlConnection.user.expeGold.ToString();
		SetLevelInfo(CRL_MySqlConnection.user.level);
		_imgUserIcon.sprite = personIcon.spiIcons[CRL_MySqlConnection.user.photoId - 1];
	}

	public void UpdateView(CRL_User user)
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
		_textLevelInfo.text = "LV. " + level + "   " + CRL_Utils.GetLevelName(level, CRL_MySqlConnection.language);
	}

	public void OnBtnReturn_Click()
	{
		CRL_SoundManager.Instance.PlayClickAudio();
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
		_textGoldCoin.text = CRL_MySqlConnection.user.gameGold.ToString();
		_textExpeCoin.text = CRL_MySqlConnection.user.expeGold.ToString();
	}

	public Sprite[] GetUserIconSprites()
	{
		return personIcon.spiIcons;
	}
}
