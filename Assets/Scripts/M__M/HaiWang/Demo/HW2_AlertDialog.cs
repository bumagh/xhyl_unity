using M__M.HaiWang.UIDefine;
using System;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class HW2_AlertDialog : BaseUIForm
	{
		public static HW2_AlertDialog instance;

		[SerializeField]
		private GameObject m_goContainer;

		[SerializeField]
		private GameObject m_goBtnOkCancelPanel;

		[SerializeField]
		private GameObject m_goBtnConfirm;

		[SerializeField]
		private Text m_textContent;

		private Action _onOkCallback;

		public bool isTouchForbidden;

		private void Awake()
		{
			instance = this;
			uiType.uiFormType = UIFormTypes.Popup;
		}

		private void OnEnable()
		{
			m_textContent.SetActive(active: false);
			Invoke("ShowContent", 0.3f);
			instance = this;
		}

		private void ShowContent()
		{
			m_textContent.SetActive();
		}

		public static HW2_AlertDialog Get()
		{
			return instance;
		}

		public static HW2_AlertDialog GetInstance()
		{
			return instance;
		}

		public void ShowDialog(string content, bool showOkCancel = false, Action callback = null)
		{
			if (string.IsNullOrEmpty(content))
			{
				UnityEngine.Debug.Log("ShowDialog: Content is " + content);
				return;
			}
			UnityEngine.Debug.Log("ShowDialog: " + content);
			Demo_UI_State curState = HW2_GVars.m_curState;
			if (m_goContainer.activeSelf)
			{
				Hide();
				if (m_goBtnConfirm.activeSelf && _onOkCallback != null)
				{
					_onOkCallback();
				}
				UnityEngine.Debug.Log("m_goContainer.activeSelf: " + m_goContainer.activeSelf);
			}
			_showDialog(content, showOkCancel, callback);
			Show(curState);
		}

		private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
		{
			m_textContent.text = content;
			_onOkCallback = callback;
			m_goBtnOkCancelPanel.SetActive(showOkCancel);
			m_goBtnConfirm.SetActive(!showOkCancel);
		}

		private void Show(Demo_UI_State showState)
		{
			UnityEngine.Debug.LogError("显示提示 当前状态: " + showState);
			if (showState == Demo_UI_State.InGame)
			{
				UIIngameManager.GetInstance().OpenUI("AlertDialog2");
			}
			else
			{
				UIRoomManager.GetInstance().OpenUI("AlertDialog");
			}
		}

		private void Hide()
		{
			if (HW2_GVars.m_curState == Demo_UI_State.InGame)
			{
				UIIngameManager.GetInstance().CloseUI("AlertDialog2");
				return;
			}
			UIRoomManager.GetInstance().CloseUI("AlertDialog");
			if (HW2_GVars.m_curState == Demo_UI_State.RoomSelection)
			{
				UnityEngine.Debug.LogError("打开房间");
			}
		}

		public void OnBtnOK_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			Hide();
			if (_onOkCallback != null)
			{
				_onOkCallback();
			}
		}

		public void OnBtnCancel_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			Hide();
		}
	}
}
