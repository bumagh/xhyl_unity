using System;
using UnityEngine;
using UnityEngine.UI;

public class EMailTalkItemUI : MonoBehaviour
{
	public Sprite sprite;

	public Image avatar;

	public Text msgText;

	public Text dateText;

	public Text nameText;

	private RectTransform _mRectTran;

	private RectTransform _avatarBGRectTran;

	private RectTransform _avatarRectTran;

	private RectTransform _msgTextRectTran;

	private RectTransform _imageRectTran;

	private RectTransform _dateTextRectTran;

	private bool isMItem;

	private bool isClick;

	private float clickTime;

	private Action<EMailTalkItemUI> _onClickEvent;

	public Color oneselfColor;

	public Color serviceColor;

	public TalkMsg msg
	{
		get;
		private set;
	}

	public bool isImage
	{
		get;
		private set;
	}

	private RectTransform mRectTran
	{
		get
		{
			if (_mRectTran == null)
			{
				_mRectTran = GetComponent<RectTransform>();
			}
			return _mRectTran;
		}
	}

	private void Update()
	{
		if (isMItem)
		{
			if (MB_Singleton<AppManager>.Get().m_mainPanel != null && MB_Singleton<AppManager>.Get().m_mainPanel.ImageUserIcon != null && (bool)avatar)
			{
				avatar.sprite = MB_Singleton<AppManager>.Get().m_mainPanel.ImageUserIcon.sprite;
			}
		}
		else if ((bool)avatar && sprite != null)
		{
			avatar.sprite = sprite;
		}
		if (isClick && Time.time - clickTime >= 1f)
		{
			OnClick();
			isClick = false;
		}
	}

	public bool SetData(TalkMsg msg)
	{
		this.msg = msg;
		if (msg.sendUsername == MB_Singleton<AppManager>.Get().m_mainPanel.ID.text)
		{
			base.transform.localScale = Vector3.one;
			dateText.transform.localScale = Vector3.one;
			msgText.transform.localScale = Vector3.one;
			nameText.transform.localScale = Vector3.one;
			avatar.transform.localScale = Vector3.one;
			base.transform.GetComponent<Image>().color = oneselfColor;
			dateText.alignment = TextAnchor.MiddleRight;
			msgText.alignment = TextAnchor.MiddleLeft;
			nameText.alignment = TextAnchor.MiddleRight;
			avatar.sprite = MB_Singleton<AppManager>.Get().m_mainPanel.ImageUserIcon.sprite;
			isMItem = true;
		}
		else
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			dateText.transform.localScale = new Vector3(-1f, 1f, 1f);
			msgText.transform.localScale = new Vector3(-1f, 1f, 1f);
			nameText.transform.localScale = new Vector3(-1f, 1f, 1f);
			avatar.transform.localScale = new Vector3(-1f, 1f, 1f);
			base.transform.GetComponent<Image>().color = serviceColor;
			dateText.alignment = TextAnchor.MiddleLeft;
			nameText.alignment = TextAnchor.MiddleLeft;
			msgText.alignment = TextAnchor.MiddleRight;
			avatar.sprite = sprite;
			isMItem = false;
		}
		dateText.text = msg.msgTime.Year + "-" + msg.msgTime.Month + "-" + msg.msgTime.Day + " " + msg.msgTime.Hour + ":" + msg.msgTime.Minute + ":" + msg.msgTime.Second;
		return true;
	}

	public void SetOnClickEvent(Action<EMailTalkItemUI> setEvent)
	{
		_onClickEvent = setEvent;
	}

	public void OnClick()
	{
		if (_onClickEvent != null)
		{
			_onClickEvent(this);
		}
	}
}
