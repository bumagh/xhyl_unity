using DG.Tweening;
using STDT_GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_DlgChat : MonoBehaviour
{
	public Sprite[] spiBtnSend;

	private XLDT_GameInfo mGameInfo;

	private string chatRecord = string.Empty;

	private InputField chatInput;

	private int chatType = 1;

	private Button btnSend;

	private Image imgBtnSend;

	private int chatUserId;

	private string chatUserNick = string.Empty;

	private Text chatTip;

	private Button btnCommonUse;

	private Text txtBtnCommonUse;

	private Image imgBtnCommonUse;

	private Button btnRecord;

	private Text txtBtnRecord;

	private Image imgBtnRecord;

	private RectTransform tfContent;

	private Transform tfCommonUse;

	private Transform tfChatRecord;

	private Text txtChadRecord;

	private XLDT_DlgChatCommonUse sptDCCU;

	private string strPrivateMark;

	private ScrollRect srChat;

	private int curIndex;

	public XLDT_DlgBase sptDlgBase;

	private void Awake()
	{
		InitDlg();
	}

	private void Start()
	{
		txtBtnCommonUse.text = XLDT_Localization.Get("DlgChatTabBtn_CommonPhrase");
		txtBtnRecord.text = XLDT_Localization.Get("DlgChatTabBtn_ChatLog");
		imgBtnSend.sprite = spiBtnSend[XLDT_Localization.language];
		chatTip.text = XLDT_Localization.Get("DlgChatTip_MsgCannotBeBlank");
		for (int i = 0; i < 11; i++)
		{
			int temp = i;
			sptDCCU.btnCommonUse[i].onClick.AddListener(delegate
			{
				ClickBtnCommonUse(temp);
			});
		}
	}

	private void InitDlg()
	{
		mGameInfo = XLDT_GameInfo.getInstance();
		chatInput = base.transform.Find("InputField").GetComponent<InputField>();
		chatTip = base.transform.Find("TxtChatTip").GetComponent<Text>();
		btnSend = base.transform.Find("BtnSend").GetComponent<Button>();
		imgBtnSend = btnSend.GetComponent<Image>();
		btnCommonUse = base.transform.Find("BtnCommon").GetComponent<Button>();
		txtBtnCommonUse = btnCommonUse.transform.Find("Text").GetComponent<Text>();
		imgBtnCommonUse = btnCommonUse.GetComponent<Image>();
		btnRecord = base.transform.Find("BtnRecord").GetComponent<Button>();
		txtBtnRecord = btnRecord.transform.Find("Text").GetComponent<Text>();
		imgBtnRecord = btnRecord.GetComponent<Image>();
		srChat = base.transform.Find("Content/Scroll View").GetComponent<ScrollRect>();
		tfContent = base.transform.Find("Content/Scroll View/Viewport/Content").GetComponent<RectTransform>();
		tfCommonUse = tfContent.Find("CommonUse");
		tfChatRecord = tfContent.Find("ChatRecord");
		txtChadRecord = tfChatRecord.Find("Text").GetComponent<Text>();
		sptDCCU = tfCommonUse.GetComponent<XLDT_DlgChatCommonUse>();
		curIndex = 0;
		imgBtnCommonUse.enabled = true;
		imgBtnRecord.enabled = false;
		srChat.movementType = ScrollRect.MovementType.Elastic;
		tfCommonUse.gameObject.SetActive(value: true);
		tfChatRecord.gameObject.SetActive(value: false);
		tfContent.sizeDelta = Vector2.up * 555f;
		btnSend.onClick.AddListener(ClickSend);
		btnCommonUse.onClick.AddListener(ClickBtnCommon);
		btnRecord.onClick.AddListener(ClickBtnRecord);
	}

	public void OnBlackClick()
	{
		SetPrivateChat(isPrivate: false, string.Empty);
		chatInput.text = string.Empty;
		XLDT_GameUIMngr.GetSingleton().ShowDlg(isShow: false, XLDT_POP_DLG_TYPE.DLG_NONE);
	}

	public void Show()
	{
		if (mGameInfo.IsGameShuUp || mGameInfo.IsUserShutUp)
		{
			chatInput.text = XLDT_Localization.Get("DlgChatTip_ChatForbidden");
			btnSend.interactable = false;
			chatInput.interactable = false;
			sptDCCU.SetBtnsInteractable(b: false);
		}
		else
		{
			btnSend.interactable = true;
			chatInput.interactable = true;
			sptDCCU.SetBtnsInteractable(b: true);
		}
	}

	public void ClickBtnCommonUse(int index)
	{
		chatInput.text = sptDCCU.words[index];
		string strChatMessage = strPrivateMark + chatInput.text;
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendSendChat(chatType, chatUserId, strChatMessage);
		OnBlackClick();
	}

	public void SetPrivateChat(bool isPrivate, string userNick = "", int userId = 0)
	{
		if (isPrivate)
		{
			chatType = 0;
			chatUserNick = userNick;
			chatUserId = userId;
			strPrivateMark = $"@{chatUserNick}:";
		}
		else
		{
			chatType = 1;
			chatUserNick = string.Empty;
			strPrivateMark = string.Empty;
		}
		chatInput.text = strPrivateMark;
	}

	public void ClickSend()
	{
		if (XLDT_DanTiaoCommon.G_TEST && chatInput.text != string.Empty)
		{
			string chatMessage = strPrivateMark + chatInput.text;
			updateChatInfo(chatType, 0, chatMessage);
		}
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		if (chatInput.text != string.Empty)
		{
			string strChatMessage = strPrivateMark + chatInput.text;
			XLDT_NetMain.GetSingleton().MyCreateSocket.SendSendChat(chatType, chatUserId, strChatMessage);
			OnBlackClick();
			return;
		}
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
		chatTip.gameObject.SetActive(value: true);
		Color initCol = chatTip.color;
		initCol.a = 1f;
		Vector3 initPos = chatTip.transform.localPosition;
		chatTip.transform.DOBlendableLocalMoveBy(Vector3.up * 70f, 0.5f).SetDelay(0.5f).OnComplete(delegate
		{
			chatTip.gameObject.SetActive(value: false);
			chatTip.transform.localPosition = initPos;
			chatTip.color = initCol;
		});
		chatTip.DOFade(0.5f, 0.5f).SetDelay(0.5f);
	}

	public void updateChatInfo(int chatType, int seatIndex, string chatMessage)
	{
		if (XLDT_DanTiaoCommon.G_TEST || seatIndex == mGameInfo.User.SeatIndex || ((chatType != 1 || !mGameInfo.Setted.bIsForbidPublicChat) && (chatType != 0 || !mGameInfo.Setted.bIsForbidPrivateChat)))
		{
			if (!XLDT_DanTiaoCommon.G_TEST)
			{
				XLDT_GameUIMngr.GetSingleton().mUserList.ShowChat(seatIndex - 1, chatMessage);
				string empty = string.Empty;
				chatRecord = string.Concat(arg1: (seatIndex != mGameInfo.User.SeatIndex) ? ((mGameInfo.Language == 0) ? ("<color=green>" + mGameInfo.UserList[seatIndex - 1].Name + "说: </color>" + chatMessage) : ("<color=green>" + mGameInfo.UserList[seatIndex - 1].Name + " Say: </color>" + chatMessage)) : ((mGameInfo.Language == 0) ? ("<color=green>我说: </color>" + chatMessage) : ("<color=green>I Say: </color>" + chatMessage)), arg0: chatRecord, arg2: '\n');
				chatRecord.Substring(0, chatRecord.Length - 1);
				txtChadRecord.text = chatRecord;
			}
			if (XLDT_DanTiaoCommon.G_TEST)
			{
				string empty2 = string.Empty;
				chatRecord = string.Concat(arg1: (seatIndex != 0) ? ((mGameInfo.Language == 0) ? ("<color=green>顶顶顶顶顶说: </color>" + chatMessage) : ("<color=green>定的的的 的的 Say: </color>" + chatMessage)) : ((mGameInfo.Language == 0) ? ("<color=green>我说: </color>" + chatMessage) : ("<color=green>I Say: </color>" + chatMessage)), arg0: chatRecord, arg2: '\n');
				chatRecord.Substring(0, chatRecord.Length - 1);
				txtChadRecord.text = chatRecord;
			}
		}
	}

	public void ClickBtnCommon()
	{
		if (curIndex != 0)
		{
			curIndex = 0;
			imgBtnCommonUse.enabled = true;
			imgBtnRecord.enabled = false;
			srChat.movementType = ScrollRect.MovementType.Elastic;
			tfCommonUse.gameObject.SetActive(value: true);
			tfChatRecord.gameObject.SetActive(value: false);
			tfContent.sizeDelta = Vector2.up * 555f;
		}
	}

	public void ClickBtnRecord()
	{
		if (curIndex != 1)
		{
			curIndex = 1;
			imgBtnCommonUse.enabled = false;
			imgBtnRecord.enabled = true;
			srChat.movementType = ScrollRect.MovementType.Clamped;
			tfCommonUse.gameObject.SetActive(value: false);
			tfChatRecord.gameObject.SetActive(value: true);
			RectTransform rectTransform = tfContent;
			Vector2 up = Vector2.up;
			Vector2 sizeDelta = txtChadRecord.rectTransform.sizeDelta;
			rectTransform.sizeDelta = up * (sizeDelta.y + 10f);
		}
	}
}
