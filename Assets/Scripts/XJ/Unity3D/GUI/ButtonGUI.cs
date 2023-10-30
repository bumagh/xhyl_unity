using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class ButtonGUI : BaseGUI
	{
		private Rect m_position;

		private IMGUI_RectM m_rectM;

		private bool m_useRectM;

		private bool m_isLayoutElement = true;

		private Action m_onClick;

		public ButtonGUI(string title = null, bool isLayout = true, Action onClick = null, bool boldTitle = false, bool value = false)
			: base(title, boldTitle)
		{
			m_onClick = onClick;
			SetPosition(new Rect(0f, 0f, 100f, 20f));
			m_isLayoutElement = isLayout;
		}

		public void Show()
		{
		}

		public void GUI_Show()
		{
			if (m_isLayoutElement)
			{
				UnityEngine.GUILayout.Button(title);
				return;
			}
			Rect position = m_useRectM ? m_rectM.GetRect() : m_position;
			if (UnityEngine.GUI.Button(position, title) && m_onClick != null)
			{
				m_onClick();
			}
		}

		public ButtonGUI SetPosition(Rect position)
		{
			m_position = position;
			m_isLayoutElement = false;
			m_useRectM = false;
			return this;
		}

		public ButtonGUI SetPosition(IMGUI_RectM rectM)
		{
			m_rectM = rectM;
			m_isLayoutElement = false;
			m_useRectM = true;
			return this;
		}

		public Rect GetRect()
		{
			return m_position;
		}

		public IMGUI_RectM GetRectM()
		{
			return m_rectM;
		}

		public ButtonGUI SetAction(Action onClick)
		{
			m_onClick = onClick;
			return this;
		}

		public override void Display()
		{
			GUI_Show();
		}

		public override void Update()
		{
			if (m_useRectM)
			{
				m_rectM.UpdateScreen();
			}
		}
	}
}
