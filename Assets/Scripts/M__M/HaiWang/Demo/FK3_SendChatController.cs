using DG.Tweening;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_SendChatController : FK3_SimpleSingletonBehaviour<FK3_SendChatController>
	{
		[SerializeField]
		private GameObject _showChat;

		[SerializeField]
		private InputField _inputSendMessage;

		[SerializeField]
		private GameObject _warn;

		[SerializeField]
		private GameObject _grid;

		[SerializeField]
		private GameObject _btnCommon;

		[SerializeField]
		private GameObject _commonPanel;

		[SerializeField]
		private GameObject _recordingPanel;

		[SerializeField]
		private GameObject _wordItem;

		[SerializeField]
		private GameObject[] _bubbleItem;

		[SerializeField]
		private List<Transform> _commonList;

		private int CurrentChatType = -1;

		private int seatId = -1;

		private List<string> recordingList = new List<string>();

		private string MyHeadStr = "<color=#00FF01FF>我说:</color>";

		private Tween tweenr;

		private void Awake()
		{
			FK3_SimpleSingletonBehaviour<FK3_SendChatController>.s_instance = this;
			recordingList.Clear();
			_showChat.SetActive(value: false);
			InitCommon();
			base.transform.Find("grayBg").GetComponent<Button>().onClick.AddListener(Close);
			_btnCommon.GetComponent<Button>().onClick.AddListener(OnBtnSend_Click);
			base.gameObject.SetActive(value: false);
		}

		private void Start()
		{
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("sendChatPush", HandleNetMsg_SendChatPush);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("sendChat", HandleNetMsg_SendChat);
		}

		private void Close()
		{
			base.gameObject.SetActive(value: false);
		}

		public void CloseChatWindow()
		{
			_showChat.SetActive(value: false);
		}

		private void OnDisable()
		{
			ResetBubble();
			_warn.SetActive(value: false);
		}

		private void SendChat(string msg)
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("聊天界面发送按钮音效");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("聊天界面发送按钮音效", 1f);
			object[] args = new object[3]
			{
				CurrentChatType,
				seatId,
				msg
			};
			UnityEngine.Debug.Log($"chatType:{CurrentChatType},seatId:{seatId},msg:{msg}");
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/sendChat", args);
		}

		public void ShowChatWindow(int seatId)
		{
			UnityEngine.Debug.Log("ShowChatWindow seatId: " + seatId);
			base.gameObject.SetActive(value: true);
			_showChat.SetActive(value: true);
			_commonPanel.SetActive(value: false);
			_inputSendMessage.text = null;
			_btnCommon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
			if (seatId == -1)
			{
				CurrentChatType = 1;
				this.seatId = -1;
			}
			else
			{
				CurrentChatType = 0;
				this.seatId = seatId;
			}
			ShowRecordingMessage();
		}

		public void ClickCommon()
		{
			bool activeSelf = _commonPanel.activeSelf;
			_commonPanel.SetActive(!_commonPanel.activeSelf);
		}

		private void InitCommon()
		{
		}

		public void OnBtnWords_Click(Text words)
		{
			if (FK3_GVars.isShutup || FK3_GVars.isUserShutup)
			{
				ShowWarnTip(ZH2_GVars.ShowTip("禁言状态，无法聊天", "A state of silence, unable to chat", string.Empty));
				return;
			}
			string msg = _inputSendMessage.text + words.text;
			SendChat(msg);
			_showChat.SetActive(value: false);
		}

		public void OnBtnSend_Click()
		{
			if (FK3_GVars.isShutup || FK3_GVars.isUserShutup)
			{
				ShowWarnTip(ZH2_GVars.ShowTip("禁言状态，无法聊天", "A state of silence, unable to chat", string.Empty));
				return;
			}
			if (_inputSendMessage.text.Trim() == string.Empty)
			{
				ShowWarnTip(ZH2_GVars.ShowTip("发送消息不可为空", "The sent message cannot be empty", string.Empty));
				_inputSendMessage.text = " ";
				return;
			}
			string text = _inputSendMessage.text;
			SendChat(text);
			_showChat.SetActive(value: false);
			_inputSendMessage.text = string.Empty;
		}

		public void SwitchPanel(int index)
		{
			_commonPanel.SetActive(index == 1);
			_recordingPanel.SetActive(index == 2);
			if (index == 2)
			{
				ShowRecordingMessage();
			}
		}

		private void ShowChatMessage(int type, int senderSeatId, string chatMessage)
		{
			string str = string.Empty;
			if (type == 0)
			{
				str = ((senderSeatId != FK3_GVars.lobby.curSeatId) ? (" @" + FK3_GVars.user.nickname + " ") : ("@" + FK3_GVars.lobby.inGameSeats[seatId - 1].user.nickname + " "));
			}
			int index = (FK3_GVars.lobby.curSeatId <= 2) ? (senderSeatId - 1) : ((senderSeatId > 2) ? (senderSeatId - 3) : (senderSeatId + 1));
			string msg = str + chatMessage;
			StopAllCoroutines();
			tweenr.Pause();
			tweenr.Kill();
			tweenr = null;
			ResetBubble();
			StartCoroutine(BubbleEffect(index, msg));
		}

		private IEnumerator BubbleEffect(int index, string msg)
		{
			yield return null;
		}

		private void ResetBubble()
		{
		}

		private void HandleNetMsg_SendChat(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int num = (int)dictionary["code"];
			string str = (string)dictionary["message"];
			UnityEngine.Debug.Log("message: " + str);
		}

		private void HandleNetMsg_SendChatPush(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			int num = (int)dictionary["chatType"];
			int num2 = (int)dictionary["senderSeatId"];
			string text = (string)dictionary["chatMessage"];
			if ((!FK3_GVars.isShutChatGroup || num != 1 || FK3_GVars.lobby.curSeatId == num2) && (!FK3_GVars.isShutChatPrivate || num != 0 || FK3_GVars.lobby.curSeatId == num2))
			{
				string str = (FK3_GVars.lobby.curSeatId == num2) ? ("<color=#00FF01FF>" + FK3_GVars.lobby.user.nickname + ":</color>") : ("<color=#ffce00>" + FK3_GVars.lobby.inGameSeats[num2 - 1].user.nickname + ":</color>");
				string empty = string.Empty;
				if (num2 == FK3_GVars.lobby.curSeatId && num == 0)
				{
					str = string.Empty;
					empty = "对<color=#ffce00>" + FK3_GVars.lobby.inGameSeats[seatId - 1].user.nickname + "</color>说: ";
				}
				else
				{
					empty = "对你说: ";
				}
				if (num2 != FK3_GVars.lobby.curSeatId && num == 0)
				{
					str = ((FK3_GVars.lobby.curSeatId == num2) ? ("<color=#00FF01FF>" + FK3_GVars.lobby.user.nickname + "</color>") : ("<color=#ffce00>" + FK3_GVars.lobby.inGameSeats[num2 - 1].user.nickname + "</color>"));
				}
				empty = ((num == 0) ? empty : " ");
				string item = str + empty + text;
				recordingList.Add(item);
				ShowChatMessage(num, num2, text);
			}
		}

		private void ShowRecordingMessage()
		{
		}

		private void ShowWarnTip(string tip)
		{
			_inputSendMessage.text = string.Empty;
			_warn.SetActive(value: true);
			_warn.GetComponentInChildren<Text>().text = tip;
			StartCoroutine(delay(1f, delegate
			{
				_warn.SetActive(value: false);
			}));
		}

		private IEnumerator delay(float delay, Action call)
		{
			yield return new WaitForSeconds(delay);
			call();
		}
	}
}
