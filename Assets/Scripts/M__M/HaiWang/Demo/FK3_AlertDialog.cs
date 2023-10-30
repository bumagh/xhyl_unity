using M__M.HaiWang.UIDefine;
using System;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_AlertDialog : FK3_BaseUIForm
	{
		public static FK3_AlertDialog instance;

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
			uiType.uiFormType = FK3_UIFormTypes.Popup;
		}

		private void OnEnable()
		{
			m_textContent.gameObject.SetActive(value: false);
			Invoke("ShowContent", 0.3f);
			instance = this;
		}

		private void ShowContent()
		{
			m_textContent.gameObject.SetActive(value: true);
		}

		public static FK3_AlertDialog Get()
		{
			return instance;
		}

		public static FK3_AlertDialog GetInstance()
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
			FK3_Demo_UI_State curState = FK3_GVars.m_curState;
			if (m_goContainer.activeSelf)
			{
				Hide();
				if (m_goBtnConfirm.activeSelf && _onOkCallback != null)
				{
					_onOkCallback();
				}
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

		private void Show(FK3_Demo_UI_State showState)
		{
			if (showState == FK3_Demo_UI_State.InGame)
			{
				FK3_UIIngameManager.GetInstance().OpenUI("AlertDialog2");
			}
			else
			{
				FK3_UIRoomManager.GetInstance().OpenUI("AlertDialog");
			}
		}

		private void Hide()
		{
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame)
			{
				FK3_UIIngameManager.GetInstance().CloseUI("AlertDialog2");
				return;
			}
			FK3_UIRoomManager.GetInstance().CloseUI("AlertDialog");
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.RoomSelection)
			{
				FK3_UIRoomManager.GetInstance().OpenUI("Room");
			}
		}

		public void OnBtnOK_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			Hide();
			if (_onOkCallback != null)
			{
				_onOkCallback();
			}
		}

		public void OnBtnCancel_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			Hide();
		}
	}
}
