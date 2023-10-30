using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class ButtonTeamGUI : BaseGUI
	{
		private string[] m_btnTitles;

		private string m_title;

		private Action<int> onClick;

		public string[] btnTitles
		{
			get
			{
				return m_btnTitles;
			}
			set
			{
				m_btnTitles = value;
			}
		}

		private new string title
		{
			get
			{
				return m_title;
			}
			set
			{
				m_title = value;
			}
		}

		public bool isHorizontal
		{
			get;
			private set;
		}

		public ButtonTeamGUI(string[] btnTitles, string title = null, Action<int> onClick = null, bool isHorizontal = true)
		{
			this.btnTitles = btnTitles;
			this.title = title;
			this.onClick = onClick;
			this.isHorizontal = isHorizontal;
		}

		public override void Display()
		{
			if (string.IsNullOrEmpty(m_title))
			{
				UnityEngine.GUILayout.Label(m_title);
			}
			if (isHorizontal)
			{
				UnityEngine.GUILayout.BeginHorizontal();
				for (int i = 0; i < m_btnTitles.Length; i++)
				{
					if (UnityEngine.GUILayout.Button(m_btnTitles[i]))
					{
						onClick(i);
					}
				}
				UnityEngine.GUILayout.EndHorizontal();
				return;
			}
			for (int j = 0; j < btnTitles.Length; j++)
			{
				if (UnityEngine.GUILayout.Button(m_btnTitles[j]))
				{
					onClick(j);
				}
			}
		}
	}
}
