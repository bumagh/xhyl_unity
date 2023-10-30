using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EMailTalkUI : MonoBehaviour
{
	private InputField msgInput;

	private Button sendBtn;

	private float sendTime;

	public RectTransform talkItemRoot;

	public EMailTalkItemUI talkItemUI;

	private List<EMailTalkItemUI> talkItemUIList = new List<EMailTalkItemUI>();

	private Action<EMailTalkItemUI> _onClickEvent;

	public EMailTalkItemMenuUI talkItemMenuUI;

	public Text talkPlayerNameText;

	private float sendImageTime = -100f;

	private TalkPlayer curTalkUser
	{
		get;
		set;
	}

	private void Awake()
	{
		msgInput = base.transform.Find("InputField").GetComponent<InputField>();
		sendBtn = base.transform.Find("SendButton").GetComponent<Button>();
		sendBtn.onClick.AddListener(SendMsg);
		talkItemUIList.Add(talkItemUI);
		talkItemUI.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		talkItemUI.SetOnClickEvent(OnTalkItemOnClick);
	}

	private void OnTalkItemOnClick(EMailTalkItemUI item)
	{
		talkItemMenuUI.Show(curTalkUser, item);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
		{
			SendMsg();
		}
	}

	public void SendMsg()
	{
		if (curTalkUser == null)
		{
		}
		if (string.IsNullOrEmpty(msgInput.text))
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog("发送内容不能为空！");
			return;
		}
		char str = ' ';
		if (HaveSpecialChar(msgInput.text, ref str))
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog("发送内容不能包含特殊字符 " + str);
			return;
		}
		if (Time.time - sendTime < 1f)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog("发送频率过快！");
			return;
		}
		NewWebSocket.GetInstance().SenServiceMsg(ZH2_GVars.user.id.ToString(), ZH2_GVars.user.promoterId.ToString(), msgInput.text, GetDateTime(), OnSendMsgFinish);
		sendTime = Time.time;
		msgInput.text = string.Empty;
		TalkData.ReadOperMsg(curTalkUser);
		TalkData.ReorderPlayeList(curTalkUser);
	}

	public void SendImage()
	{
		if (Time.time - sendImageTime < 10f)
		{
			int num = Mathf.CeilToInt(10f - (Time.time - sendImageTime));
			MB_Singleton<AlertDialog>.Get().ShowDialog("操作过于频繁," + num + "秒后才可以发送图片!");
		}
		else
		{
			sendImageTime = Time.time;
			ServiceTools.PickImage(delegate(Texture2D image)
			{
				SendImageData(image);
			});
		}
	}

	private void SendImageData(Texture2D image)
	{
		if (image != null)
		{
			string text = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + DateTime.Now.Millisecond.ToString("000");
			string text2 = ZH2_GVars.user.id + "TalkImage" + text + ".png";
			NewWebSocket.GetInstance().SenServiceIma(image, 1024, OnSendMsgFinish);
			TalkData.ReorderPlayeList(curTalkUser);
		}
	}

	public void UpdateTalkItemUI()
	{
		TalkPlayer curTalkUser = this.curTalkUser;
		if (curTalkUser == null)
		{
			return;
		}
		talkPlayerNameText.text = curTalkUser.username;
		List<TalkMsg> list = new List<TalkMsg>();
		if (TalkData.talkMsg.ContainsKey(curTalkUser))
		{
			list = TalkData.talkMsg[curTalkUser];
		}
		while (talkItemUIList.Count < list.Count)
		{
			EMailTalkItemUI component = UnityEngine.Object.Instantiate(talkItemUI.gameObject, talkItemRoot).GetComponent<EMailTalkItemUI>();
			component.gameObject.SetActive(value: true);
			component.SetOnClickEvent(OnTalkItemOnClick);
			talkItemUIList.Add(component);
		}
		bool flag = false;
		for (int i = 0; i < talkItemUIList.Count; i++)
		{
			if (i < list.Count)
			{
				talkItemUIList[i].gameObject.SetActive(value: true);
				if (talkItemUIList[i].SetData(list[i]))
				{
					flag = true;
				}
			}
			else
			{
				talkItemUIList[i].gameObject.SetActive(value: false);
			}
			talkItemUIList[i].transform.SetSiblingIndex(i);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(talkItemRoot);
		TalkData.ReadOperMsg(curTalkUser);
	}

	private void OnSendMsgFinish(TalkMsg msg)
	{
		if (TalkData.operatorInfos.ContainsKey(msg.targetUsername))
		{
			TalkPlayer oper = TalkData.operatorInfos[msg.targetUsername];
			TalkData.AddMsg(oper, msg);
			TalkData.SaveMsg(oper);
			UpdateTalkItemUI();
			msgInput.text = string.Empty;
		}
	}

	public string GetDateTime()
	{
		string empty = string.Empty;
		DateTime now = DateTime.Now;
		return now.Year + "-" + now.Month + "-" + now.Day + " " + now.Hour + ":" + now.Minute + ":" + now.Second;
	}

	public bool HaveSpecialChar(string str, ref char str2)
	{
		string text = "`#$%^&*~/";
		foreach (char c in text)
		{
			if (str.IndexOf(c) >= 0)
			{
				str2 = c;
				return true;
			}
		}
		return false;
	}
}
