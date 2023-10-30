using UnityEngine;

public class JSYS_LL_ChatPanel : MonoBehaviour
{
	protected JSYS_LL_TableList mTableList;

	protected JSYS_LL_PersonInfo mUserSelf;

	protected UIPanel mPublicChatWinPanel;

	protected UIPanel mChatRecordPanel;

	protected Collider mPublicWinBgCol;

	protected Collider mPublicPanelCol;

	protected Collider mPublicSendBtnCol;

	protected Collider mPublicInputCol;

	protected Collider mPublicRecordCol;

	protected GameObject mPublicChatTipObj;

	protected UITextList mChatRecordText;

	protected UIInput mPublicInput;

	protected UIPanel mPrivateChatWinPanel;

	protected GameObject mPrivateChatTipObj;

	protected Collider mPrivateWinBgCol;

	protected Collider mPrivatePanelCol;

	protected Collider mPrivateSendBtnCol;

	protected Collider mPrivateInputCol;

	protected Collider mPrivateCancelCol;

	protected UIInput mPrivateInput;

	protected UI2DSprite mPublicSendBg;

	protected UI2DSprite mPrivateSendBg;

	protected bool mIsShutUp;

	protected bool mIsPrivateChatActive;

	protected bool mIsPublicChatActive;

	protected UILabel mPrivateWinTitle;

	protected int mChatReceiverKeyId = 1;

	protected int mChatSeatId = 1;

	protected GameObject[] mChatBubbleObj = new GameObject[8];

	protected UIPanel[] mChatBubblePanel = new UIPanel[8];

	protected Transform[] mBubbleTextTran = new Transform[8];

	protected UILabel[] mChatBubbleText = new UILabel[8];

	protected bool[] mIsChatVisable = new bool[8];

	protected float[] mTotalTime = new float[8];

	protected float[] mCurBubbleTranY = new float[8]
	{
		30f,
		30f,
		30f,
		30f,
		30f,
		30f,
		30f,
		30f
	};

	protected float[] mFinalBubbleTranY = new float[8]
	{
		30f,
		30f,
		30f,
		30f,
		30f,
		30f,
		30f,
		30f
	};

	private Collider ChipBg2;

	public int ChatSeatID
	{
		set
		{
			if (value < 1 || value > 8)
			{
				JSYS_LL_ErrorManager.GetSingleton().AddError("私聊玩家座位序号错误：" + value);
				value = 1;
			}
			mChatSeatId = value;
		}
	}

	private void Start()
	{
		mTableList = GameObject.Find("TableListPanel").GetComponent<JSYS_LL_TableList>();
		mUserSelf = JSYS_LL_GameInfo.getInstance().UserInfo;
		mPublicChatWinPanel = base.transform.Find("PublicChatWindow").GetComponent<UIPanel>();
		mChatRecordPanel = base.transform.Find("PublicChatWindow").Find("ChatRecord").GetComponent<UIPanel>();
		mPublicChatTipObj = base.transform.Find("PublicChatWindow").Find("ChatTip").gameObject;
		mPublicChatTipObj.SetActive(value: false);
		mPublicPanelCol = base.transform.Find("PublicChatWindow").Find("PublicChatWinCollider").GetComponent<Collider>();
		mPublicWinBgCol = base.transform.Find("PublicChatWindow").Find("Background").GetComponent<Collider>();
		mPublicSendBtnCol = base.transform.Find("PublicChatWindow").Find("SendBtn").GetComponent<Collider>();
		mPublicInputCol = base.transform.Find("PublicChatWindow").Find("Input").GetComponent<Collider>();
		mPublicRecordCol = base.transform.Find("PublicChatWindow").Find("ChatRecord").GetComponent<Collider>();
		mChatRecordText = base.transform.Find("PublicChatWindow").Find("ChatRecord").GetComponent<UITextList>();
		mPublicInput = base.transform.Find("PublicChatWindow").Find("Input").GetComponent<UIInput>();
		mPrivateChatWinPanel = base.transform.Find("PrivateChatWindow").GetComponent<UIPanel>();
		mPrivateChatTipObj = base.transform.Find("PrivateChatWindow").Find("ChatTip").gameObject;
		mPrivateChatTipObj.SetActive(value: false);
		mPrivateWinTitle = base.transform.Find("PrivateChatWindow").Find("WindowTitle").Find("Label")
			.GetComponent<UILabel>();
		mPrivatePanelCol = base.transform.Find("PrivateChatWindow").Find("PrivateChatWinCollider").GetComponent<Collider>();
		mPrivateWinBgCol = base.transform.Find("PrivateChatWindow").Find("Background").GetComponent<Collider>();
		mPrivateSendBtnCol = base.transform.Find("PrivateChatWindow").Find("SendBtn").GetComponent<Collider>();
		mPrivateInputCol = base.transform.Find("PrivateChatWindow").Find("Input").GetComponent<Collider>();
		mPrivateCancelCol = base.transform.Find("PrivateChatWindow").Find("CancelBtn").GetComponent<Collider>();
		mPrivateInput = base.transform.Find("PrivateChatWindow").Find("Input").GetComponent<UIInput>();
		mPublicSendBg = base.transform.Find("PublicChatWindow").Find("SendBtn").Find("Background")
			.GetComponent<UI2DSprite>();
		ChipBg2 = base.transform.Find("PublicChatWindow").Find("ChipBg2").GetComponent<Collider>();
		mPrivateSendBg = base.transform.Find("PrivateChatWindow").Find("SendBtn").Find("Background")
			.GetComponent<UI2DSprite>();
		Transform transform = GameObject.Find("HudPanel").transform.Find("UserList").Find("BubbleList");
		for (int i = 0; i < 8; i++)
		{
			mChatBubbleObj[i] = transform.Find("ChatBubble" + i).gameObject;
			mChatBubblePanel[i] = mChatBubbleObj[i].transform.Find("Panel").GetComponent<UIPanel>();
			mBubbleTextTran[i] = mChatBubbleObj[i].transform.Find("Panel").Find("Text");
			mChatBubbleText[i] = mBubbleTextTran[i].GetComponent<UILabel>();
			mChatBubbleObj[i].SetActive(value: false);
			mBubbleTextTran[i].gameObject.AddComponent<TweenPosition>();
			mChatBubblePanel[i].enabled = false;
		}
		HidePublicChatWindow();
		HidePrivateChatWindow();
		updateChatConfig();
	}

	private void updateChatConfig()
	{
		if (JSYS_LL_GameInfo.getInstance().UserInfo.IsGlobalFibbidChat || JSYS_LL_GameInfo.getInstance().UserInfo.IsSelfFibbidChat)
		{
			IsForbidChat(bIsForbidChat: true);
		}
	}

	private void Update()
	{
		for (int i = 0; i < 8; i++)
		{
			if (!mIsChatVisable[i])
			{
				continue;
			}
			if (mTotalTime[i] >= 2f)
			{
				if (mCurBubbleTranY[i] >= mFinalBubbleTranY[i])
				{
					mIsChatVisable[i] = false;
					mChatBubbleObj[i].SetActive(value: false);
					mChatBubblePanel[i].enabled = false;
				}
				else
				{
					mTotalTime[i] = 0f;
					Vector3 localPosition = mBubbleTextTran[i].localPosition;
					mBubbleTextTran[i].GetComponent<TweenPosition>().enabled = false;
					float y = localPosition.y;
					Vector3 localScale = mBubbleTextTran[i].localScale;
					localPosition.y = y + localScale.y;
					mCurBubbleTranY[i] = localPosition.y;
					TweenPosition.Begin(mBubbleTextTran[i].gameObject, 0.5f, localPosition);
				}
			}
			mTotalTime[i] += Time.deltaTime;
		}
	}

	public void ShowPublicChatWindow()
	{
		mPublicChatWinPanel.enabled = true;
		mChatRecordPanel.enabled = true;
		_setPublicChatColliderActive(bIsActive: true);
		UICamera.Notify(mChatRecordText.gameObject, "OnSelect", true);
	}

	public void HidePublicChatWindow()
	{
		mPublicChatWinPanel.enabled = false;
		mChatRecordPanel.enabled = false;
		mPublicChatTipObj.SetActive(value: false);
		_setPublicChatColliderActive(bIsActive: false);
	}

	protected float _caculateBubbleFinalY(UILabel textLabel, float fStartY = 30f)
	{
		float num = JSYS_LL_MyUICommon.CalculateCharCount(textLabel.text);
		Vector3 localScale = textLabel.transform.localScale;
		float num2 = num * localScale.x / 2f;
		int num3 = (int)(num2 / (float)textLabel.lineWidth + 1f);
		float num4 = num3;
		Vector3 localScale2 = textLabel.transform.localScale;
		return fStartY + num4 * localScale2.y;
	}

	protected void _setPublicChatColliderActive(bool bIsActive)
	{
		mIsPublicChatActive = bIsActive;
		if ((bool)mPublicPanelCol)
		{
			mPublicPanelCol.enabled = bIsActive;
		}
		if ((bool)mPublicWinBgCol)
		{
			mPublicWinBgCol.enabled = bIsActive;
		}
		if ((bool)mPublicInputCol)
		{
			mPublicInputCol.enabled = bIsActive;
		}
		if ((bool)mPublicRecordCol)
		{
			mPublicRecordCol.enabled = bIsActive;
		}
		if ((bool)mPublicSendBtnCol)
		{
			mPublicSendBtnCol.enabled = (bIsActive && !mIsShutUp);
		}
		if ((bool)mPublicSendBg)
		{
			if (bIsActive && !mIsShutUp)
			{
				mPublicSendBg.color = new Color(1f, 1f, 1f);
			}
			else
			{
				mPublicSendBg.color = new Color(0.5f, 0.5f, 0.5f);
			}
		}
		if ((bool)ChipBg2)
		{
			ChipBg2.enabled = bIsActive;
		}
	}

	public void ShowPrivateChatWindow()
	{
		mPrivateChatWinPanel.enabled = true;
		_setPrivateChatColliderActive(bIsActive: true);
	}

	public void HidePrivateChatWindow()
	{
		mPrivateChatWinPanel.enabled = false;
		_setPrivateChatColliderActive(bIsActive: false);
		mPrivateChatTipObj.SetActive(value: false);
	}

	protected void _setPrivateChatColliderActive(bool bIsActive)
	{
		mIsPrivateChatActive = bIsActive;
		if ((bool)mPrivatePanelCol)
		{
			mPrivatePanelCol.enabled = bIsActive;
		}
		if ((bool)mPrivateInputCol)
		{
			mPrivateInputCol.enabled = bIsActive;
		}
		if ((bool)mPrivateWinBgCol)
		{
			mPrivateWinBgCol.enabled = bIsActive;
		}
		if ((bool)mPrivateCancelCol)
		{
			mPrivateCancelCol.enabled = bIsActive;
		}
		if ((bool)mPrivateSendBtnCol)
		{
			mPrivateSendBtnCol.enabled = (bIsActive && !mIsShutUp);
		}
		if ((bool)mPrivateSendBg)
		{
			if (bIsActive && !mIsShutUp)
			{
				mPrivateSendBg.color = new Color(1f, 1f, 1f);
			}
			else
			{
				mPrivateSendBg.color = new Color(0.5f, 0.5f, 0.5f);
			}
		}
	}

	public void AddChatMessage(int iChatType, int iSenderSeatID, string strChatMessage)
	{
		if ((!JSYS_LL_GameInfo.getInstance().Setted.bIsForbidPublicChat && iChatType == 1) || (!JSYS_LL_GameInfo.getInstance().Setted.bIsForbidPrivateChat && iChatType == 0))
		{
			Vector3 localPosition = mBubbleTextTran[iSenderSeatID - 1].localPosition;
			if (JSYS_LL_GameInfo.getInstance().Language == 0)
			{
				mChatBubbleText[iSenderSeatID - 1].text = ((iChatType == 1) ? strChatMessage : ("悄悄话:" + strChatMessage));
			}
			else
			{
				mChatBubbleText[iSenderSeatID - 1].text = ((iChatType == 1) ? strChatMessage : ("Whisper:" + strChatMessage));
			}
			mCurBubbleTranY[iSenderSeatID - 1] = 30f;
			mTotalTime[iSenderSeatID - 1] = 0f;
			mIsChatVisable[iSenderSeatID - 1] = true;
			mChatBubblePanel[iSenderSeatID - 1].enabled = true;
			mChatBubbleObj[iSenderSeatID - 1].SetActive(value: true);
			mBubbleTextTran[iSenderSeatID - 1].GetComponent<TweenPosition>().enabled = false;
			localPosition.y = 30f;
			mBubbleTextTran[iSenderSeatID - 1].localPosition = localPosition;
			mFinalBubbleTranY[iSenderSeatID - 1] = _caculateBubbleFinalY(mChatBubbleText[iSenderSeatID - 1]);
		}
		string empty = string.Empty;
		mChatRecordText.Add(empty);
	}

	public void Restart()
	{
		mChatRecordText.Clear();
		for (int i = 0; i < 8; i++)
		{
			mChatBubbleObj[i].SetActive(value: false);
			mChatBubblePanel[i].enabled = false;
		}
	}

	public void IsForbidChat(bool bIsForbidChat)
	{
		mIsShutUp = bIsForbidChat;
		mPublicSendBtnCol.enabled = (!bIsForbidChat && mIsPublicChatActive);
		mPrivateSendBtnCol.enabled = (!bIsForbidChat && mIsPublicChatActive);
		if (!bIsForbidChat && mIsPublicChatActive)
		{
			mPublicSendBg.color = new Color(1f, 1f, 1f);
			mPrivateSendBg.color = new Color(1f, 1f, 1f);
		}
		else
		{
			mPublicSendBg.color = new Color(0.5f, 0.5f, 0.5f);
			mPrivateSendBg.color = new Color(0.5f, 0.5f, 0.5f);
		}
	}

	public void _onClickPublicInput()
	{
		mPublicChatTipObj.SetActive(value: false);
	}

	public void _onClickPublicSendBtn()
	{
		if (mPublicInput.text.CompareTo(string.Empty) == 0)
		{
			mPublicChatTipObj.SetActive(value: true);
			TweenAlpha[] componentsInChildren = mPublicChatTipObj.GetComponentsInChildren<TweenAlpha>();
			foreach (TweenAlpha tweenAlpha in componentsInChildren)
			{
				tweenAlpha.Play(forward: true);
			}
		}
		else if (!JSYS_LL_MyTest.TEST)
		{
			JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendSendChat(1, mChatReceiverKeyId, mPublicInput.text);
			mPublicInput.text = string.Empty;
		}
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
	}

	public void _onClickPrivateInput()
	{
		mPrivateChatTipObj.SetActive(value: false);
	}

	public void _onClickPrivateSendBtn()
	{
		if (mPrivateInput.text.CompareTo(string.Empty) == 0)
		{
			mPrivateChatTipObj.SetActive(value: true);
			TweenAlpha[] componentsInChildren = mPrivateChatTipObj.GetComponentsInChildren<TweenAlpha>();
			foreach (TweenAlpha tweenAlpha in componentsInChildren)
			{
				tweenAlpha.Play(forward: true);
			}
		}
		else
		{
			if (!JSYS_LL_MyTest.TEST)
			{
				JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendSendChat(0, mChatReceiverKeyId, mPrivateInput.text);
				mPrivateInput.text = string.Empty;
			}
			HidePrivateChatWindow();
		}
	}

	public void _onClickPrivateCancelBtn()
	{
		mPrivateInput.text = string.Empty;
		HidePrivateChatWindow();
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
	}
}
