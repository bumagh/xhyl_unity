using M__M.HaiWang.UIDefine;
using System;
using UnityEngine;

namespace M__M.HaiWang.UI
{
	public class InGameUI : SimpleSingletonBehaviour<InGameUI>
	{
		private InGameUIContext m_context = new InGameUIContext();

		public Action EventHandler_UI_PanelCoinInOut_OnHide;

		public bool showCoinInOut;

		public bool showOptionBtn = true;

		public event Action EventHandler_UI_OnBtnBackClick;

		public event Action EventHandler_UI_OnBtnCoinInClick;

		public event Action EventHandler_UI_OnBtnCoinOutClick;

		private void Awake()
		{
			SimpleSingletonBehaviour<InGameUI>.s_instance = this;
		}

		private void Start()
		{
		}

		private void OnGUI()
		{
		}

		private void Display_UI()
		{
			if (showOptionBtn)
			{
				Display_OptionBtn();
			}
			if (showCoinInOut)
			{
				Display_CoinInOut();
			}
		}

		private void Display_OptionBtn()
		{
			if (GUI.Button(new Rect(0f, 40f, 100f, 25f), "上下分"))
			{
				Show_Panel_CoinInOut();
			}
		}

		private void Display_CoinInOut()
		{
			int num = 180;
			int num2 = 25;
			int num3 = num2 * 3;
			int num4 = num + 20;
			int num5 = num3 + 20;
			GUI.Box(new Rect(Screen.width / 2 - num4 / 2, Screen.height / 2 - num5 / 2, num4, num5), string.Empty);
			GUILayout.BeginArea(new Rect(Screen.width / 2 - num / 2, Screen.height / 2 - num3 / 2, num, num3));
			GUILayout.BeginHorizontal();
			GUILayout.Label("金 币：");
			GUILayout.TextField(m_context.curGold.ToString(), GUILayout.Width(100f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("分 数：");
			GUILayout.TextField(m_context.curScore.ToString(), GUILayout.Width(100f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("上分"))
			{
				UnityEngine.Debug.Log("上分");
				SimpleUtil.SafeInvoke(delegate
				{
					if (this.EventHandler_UI_OnBtnCoinInClick != null)
					{
						this.EventHandler_UI_OnBtnCoinInClick();
					}
				});
				return;
			}
			if (GUILayout.Button("下分"))
			{
				UnityEngine.Debug.Log("下分");
				SimpleUtil.SafeInvoke(delegate
				{
					if (this.EventHandler_UI_OnBtnCoinOutClick != null)
					{
						this.EventHandler_UI_OnBtnCoinOutClick();
					}
				});
				return;
			}
			if (GUILayout.Button("X"))
			{
				Hide_Panel_CoinInOut();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void Display_Back()
		{
			if (GUI.Button(new Rect(0f, 0f, 100f, 25f), "返回"))
			{
				SimpleUtil.SafeInvoke(delegate
				{
					if (this.EventHandler_UI_OnBtnBackClick != null)
					{
						this.EventHandler_UI_OnBtnBackClick();
					}
				});
			}
		}

		public InGameUIContext GetContext()
		{
			return m_context;
		}

		public void Show_Panel_CoinInOut()
		{
			showCoinInOut = true;
			showOptionBtn = false;
		}

		public void Hide_Panel_CoinInOut()
		{
			showCoinInOut = false;
			showOptionBtn = true;
			if (EventHandler_UI_PanelCoinInOut_OnHide != null)
			{
				EventHandler_UI_PanelCoinInOut_OnHide();
			}
		}

		public void Reset_EventHandler()
		{
			this.EventHandler_UI_OnBtnBackClick = null;
			this.EventHandler_UI_OnBtnCoinInClick = null;
			this.EventHandler_UI_OnBtnCoinOutClick = null;
			EventHandler_UI_PanelCoinInOut_OnHide = null;
		}
	}
}
