using System;
using UnityEngine;
using UnityEngine.UI;

public class WHN_DeskWidget : MonoBehaviour
{
	private Text _textDeskName;

	private Text _textMinCoin;

	private Text _textCoinRate;

	private Image _imgUserIcon;

	private Image _imgUserIconBG;

	private GameObject _goSelectable;

	private Text _UserName;

	private Button btnSeat;

	[SerializeField]
	private CSF_PersonIcon personIcon;

	public Action onDeskClickAction;

	private void Awake()
	{
		_textDeskName = base.transform.Find("Info/TxtDeskName").GetComponent<Text>();
		_textMinCoin = base.transform.Find("Info/TxtMin").GetComponent<Text>();
		_textCoinRate = base.transform.Find("Info/TxtValue").GetComponent<Text>();
		_imgUserIcon = base.transform.Find("ImgPerson").GetComponent<Image>();
		_imgUserIconBG = base.transform.Find("ImgPersonBg").GetComponent<Image>();
		_goSelectable = base.transform.Find("Arrow").gameObject;
		_UserName = base.transform.Find("ImgPersonBg/TxtUserName").GetComponent<Text>();
		btnSeat = base.transform.Find("BtnSeat").GetComponent<Button>();
		btnSeat.onClick.AddListener(OnBtnClick);
	}

	public void InitDesk(WHN_Desk desk)
	{
		if (desk.userPhotoId == -1)
		{
			_imgUserIcon.gameObject.SetActive(value: false);
			_imgUserIconBG.gameObject.SetActive(value: false);
			_goSelectable.SetActive(value: true);
		}
		else
		{
			_UserName.text = desk.nickname;
			_imgUserIcon.gameObject.SetActive(value: true);
			_imgUserIcon.sprite = personIcon.spiIcons[desk.userPhotoId - 1];
			_imgUserIconBG.gameObject.SetActive(value: true);
			_goSelectable.SetActive(value: false);
		}
		_textDeskName.text = desk.name;
		_textMinCoin.text = ((!(WHN_GVars.language == "zh")) ? "Min Coin :" : "最小携带：") + desk.minGold.ToString();
		_textCoinRate.text = ((!(WHN_GVars.language == "zh")) ? "Min Coin :" : "一币分值：") + desk.exchange.ToString();
	}

	public void InitPlayerInDesk(string userName = "", string deskName = "", int photoId = -1)
	{
		_UserName.text = string.Empty;
		_textDeskName.text = string.Empty;
		_textMinCoin.text = string.Empty;
		_textCoinRate.text = string.Empty;
		if (photoId < 0)
		{
			_imgUserIcon.sprite = null;
		}
		else
		{
			_imgUserIcon.sprite = personIcon.spiIcons[photoId];
		}
	}

	public void OnBtnClick()
	{
		if (onDeskClickAction != null)
		{
			onDeskClickAction();
		}
	}
}
