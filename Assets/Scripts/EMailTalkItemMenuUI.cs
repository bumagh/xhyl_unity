using UnityEngine;
using UnityEngine.UI;

public class EMailTalkItemMenuUI : MonoBehaviour
{
	private EMailTalkItemUI _item;

	public RectTransform buttonRect;

	public Text buttonText;

	private TalkPlayer _talkPlayer;

	public void ButtonOnClick()
	{
		if (_item == null)
		{
			Hide();
		}
		else
		{
			Hide();
		}
	}

	public void DeleteOnClick()
	{
		TalkData.DeleteMsg(_talkPlayer, _item.msg);
		Hide();
	}

	public void Show(TalkPlayer mplayer, EMailTalkItemUI itemUI)
	{
		base.gameObject.SetActive(value: true);
		_item = itemUI;
		_talkPlayer = mplayer;
		if (itemUI.isImage)
		{
			buttonText.text = "保存";
		}
		else
		{
			buttonText.text = "复制";
		}
		buttonRect.position = itemUI.transform.position;
		buttonRect.localPosition -= Vector3.up * itemUI.GetComponent<RectTransform>().rect.height * 0.5f;
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
