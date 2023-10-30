using System;
using UnityEngine;
using UnityEngine.UI;

public class STWM_DeskWidget : MonoBehaviour
{
	[SerializeField]
	private Text _textDeskName;

	[SerializeField]
	private Text _textMinCoin;

	[SerializeField]
	private Text _textCoinRate;

	[SerializeField]
	private Image _imgUserIcon;

	[SerializeField]
	private Image _imgUserIconBG;

	[SerializeField]
	private GameObject _goSelectable;

	[SerializeField]
	private Text _UserName;

	[SerializeField]
	private STWM_PersonIcon personIcon;

	public Action onDeskClickAction;

	public void InitDesk(STWM_Desk desk)
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
		_textMinCoin.text = ((!(STWM_GVars.language == "zh")) ? "Min Coin :" : "最小携带：") + desk.minGold.ToString();
		_textCoinRate.text = ((!(STWM_GVars.language == "zh")) ? "Min Coin :" : "一币分值：") + desk.exchange.ToString();
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
