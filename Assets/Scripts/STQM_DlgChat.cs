using UnityEngine;
using UnityEngine.UI;

public class STQM_DlgChat : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiBtnSend;

	[SerializeField]
	private Sprite[] spiError;

	private Image imgError;

	private STQM_ErrorTipAnim sptErrorTipAnim;

	private InputField ifChart;

	private Button btnSend;

	private Button btnCommon;

	private Button btnRecord;

	private ScrollRect srChat;

	private RectTransform rtfContent;

	private Transform tfCommon;

	private Transform tfRecord;

	private Text txtRecord;

	private Button[] btnCommonUse;

	private Text[] txtCommonUse;

	private int count = 10;

	private int language;

	private int curBtnIndex;

	private int chatType = 1;

	private int chatUserId;

	private string chatUserName = string.Empty;

	private string chatRecord = string.Empty;

	private string strPrivateMark;

	private string[] words = new string[20]
	{
		"兄台高人啊，佩服佩服。",
		"有缘千里来相会，交个朋友吧！",
		"放开那龙龟，看我手到擒来。",
		"哈哈，这些都是我的囊中之物。",
		"嘿嘿，总算被我逮到条大的。",
		"这么小的鱼，回家炖汤都不够啊！",
		"为何大鱼总是与我擦肩而过？",
		"宝刀屠龙在我手，小样看你往哪游！",
		"青山不改绿水长流，大家后会有期。",
		"不要走，决战到天亮！",
		"It's so enjoyable to play with you.",
		"Don't go away, let's fight until tomorrow.",
		"Fuck! Don't let me over limit, I don't like cash out.",
		"The Dragon turtle can not escape from my hand.",
		"The Luck Fairy has forgotten me.",
		"Keep on going, never give up.",
		"I'm very pleased to play together with you.",
		"Wait for me, I'll be back.",
		"I kown you want to kill two birds with one stone.",
		"Winning is unimportant, enjoy it!"
	};

	private STQM_GameInfo gameInfo;

	[HideInInspector]
	public STQM_ChatCtrl sptChatCtrl;

	private void Awake()
	{
		gameInfo = STQM_GameInfo.getInstance();
		language = gameInfo.Language;
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		imgError = base.transform.Find("ImgError").GetComponent<Image>();
		imgError.sprite = spiError[language];
		imgError.SetNativeSize();
		sptErrorTipAnim = imgError.GetComponent<STQM_ErrorTipAnim>();
		ifChart = base.transform.Find("InputField").GetComponent<InputField>();
		btnSend = base.transform.Find("BtnSend").GetComponent<Button>();
		SpriteState spriteState = default(SpriteState);
		spriteState.disabledSprite = spiBtnSend[language];
		btnSend.spriteState = spriteState;
		btnSend.onClick.AddListener(ClickBtnSend);
		btnCommon = base.transform.Find("BtnCommon").GetComponent<Button>();
		btnCommon.transform.Find("Text").GetComponent<Text>().text = ((language != 0) ? string.Empty : "常用语");
		btnCommon.image.enabled = true;
		btnCommon.onClick.AddListener(ClickBtnCommon);
		btnRecord = base.transform.Find("BtnRecord").GetComponent<Button>();
		btnRecord.transform.Find("Text").GetComponent<Text>().text = ((language != 0) ? string.Empty : "聊天记录");
		btnRecord.image.enabled = false;
		btnRecord.onClick.AddListener(ClickBtnRecord);
		srChat = base.transform.Find("Content/Scroll View").GetComponent<ScrollRect>();
		srChat.movementType = ScrollRect.MovementType.Elastic;
		rtfContent = srChat.transform.Find("Viewport/Content").GetComponent<RectTransform>();
		rtfContent.sizeDelta = Vector2.up * 505f;
		tfCommon = rtfContent.transform.Find("CommonUse");
		tfRecord = rtfContent.transform.Find("ChatRecord");
		txtRecord = tfRecord.Find("Text").GetComponent<Text>();
		tfCommon.gameObject.SetActive(value: true);
		tfRecord.gameObject.SetActive(value: false);
		InitCommonUse();
	}

	private void InitCommonUse()
	{
		int num = language * count;
		btnCommonUse = new Button[count];
		txtCommonUse = new Text[count];
		for (int i = 0; i < count; i++)
		{
			btnCommonUse[i] = tfCommon.GetChild(i).GetComponent<Button>();
			txtCommonUse[i] = btnCommonUse[i].transform.Find("Text").GetComponent<Text>();
			txtCommonUse[i].text = words[num + i];
			int temp = i;
			btnCommonUse[i].onClick.AddListener(delegate
			{
				ClickBtnCommonUse(temp);
			});
		}
	}

	private void SetBtnCommonUse(bool b)
	{
		for (int i = 0; i < count; i++)
		{
			btnCommonUse[i].interactable = b;
		}
	}

	private void ClickBtnCommonUse(int index)
	{
		string str = words[language * count + index];
		ifChart.text = strPrivateMark + str;
		STQM_NetMngr.GetSingleton().MyCreateSocket.SendChat(chatType, chatUserId, ifChart.text);
		base.gameObject.SetActive(value: false);
	}

	private void ClickBtnSend()
	{
		if (ifChart.text != string.Empty)
		{
			STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.common);
			string chatMessage = strPrivateMark + ifChart.text;
			STQM_NetMngr.GetSingleton().MyCreateSocket.SendChat(chatType, chatUserId, chatMessage);
			base.gameObject.SetActive(value: false);
		}
		else
		{
			sptErrorTipAnim.PlayTipAnim();
		}
	}

	public void ShowChat()
	{
		base.gameObject.SetActive(value: true);
		if (gameInfo.IsGameShuUp || gameInfo.IsUserShutUp)
		{
			ifChart.text = ((language != 0) ? "Unable to chat" : "禁言状态，无法聊天");
			ifChart.interactable = false;
			btnSend.interactable = false;
			btnSend.GetComponent<STOF_ButtonImgAni>().enabled = false;
			btnSend.image.sprite = spiBtnSend[language * 2 + 1];
			SetBtnCommonUse(b: false);
		}
		else
		{
			btnSend.GetComponent<STOF_ButtonImgAni>().enabled = true;
			btnSend.image.sprite = spiBtnSend[language * 2];
			btnSend.interactable = true;
			ifChart.interactable = true;
			SetBtnCommonUse(b: true);
		}
	}

	public void SetPrivateChat(bool isPrivate, string userName = "", int userId = 0)
	{
		if (isPrivate)
		{
			chatType = 0;
			chatUserName = userName;
			chatUserId = userId;
			strPrivateMark = $"@{chatUserName}:";
		}
		else
		{
			chatType = 1;
			chatUserName = string.Empty;
			strPrivateMark = string.Empty;
		}
		ifChart.text = strPrivateMark;
	}

	public void UpdateChatInfo(int chatType, int seatIndex, string chatMessage)
	{
		if (seatIndex == gameInfo.User.SeatIndex || ((chatType != 1 || !gameInfo.Setted.bIsForbidPublicChat) && (chatType != 0 || !gameInfo.Setted.bIsForbidPrivateChat)))
		{
			sptChatCtrl.ShowChat(chatMessage);
			string empty = string.Empty;
			chatRecord = string.Concat(arg1: (seatIndex != gameInfo.User.SeatIndex) ? ((gameInfo.Language == 0) ? ("<color=green>" + gameInfo.UserList[seatIndex - 1].Name + "说: </color>" + chatMessage) : ("<color=green>" + gameInfo.UserList[seatIndex - 1].Name + " Say: </color>" + chatMessage)) : ((gameInfo.Language == 0) ? ("<color=green>我说: </color>" + chatMessage) : ("<color=green>I Say: </color>" + chatMessage)), arg0: chatRecord, arg2: '\n');
			chatRecord.Substring(0, chatRecord.Length - 1);
			txtRecord.text = chatRecord;
		}
	}

	private void ClickBtnCommon()
	{
		if (curBtnIndex != 0)
		{
			curBtnIndex = 0;
			btnCommon.image.enabled = true;
			btnRecord.image.enabled = false;
			srChat.movementType = ScrollRect.MovementType.Elastic;
			tfCommon.gameObject.SetActive(value: true);
			tfRecord.gameObject.SetActive(value: false);
			rtfContent.sizeDelta = Vector2.up * 505f;
		}
	}

	private void ClickBtnRecord()
	{
		if (curBtnIndex != 1)
		{
			curBtnIndex = 1;
			btnCommon.image.enabled = false;
			btnRecord.image.enabled = true;
			srChat.movementType = ScrollRect.MovementType.Clamped;
			tfCommon.gameObject.SetActive(value: false);
			tfRecord.gameObject.SetActive(value: true);
			RectTransform rectTransform = rtfContent;
			Vector2 up = Vector2.up;
			Vector2 sizeDelta = txtRecord.rectTransform.sizeDelta;
			rectTransform.sizeDelta = up * (sizeDelta.y + 10f);
		}
	}
}
