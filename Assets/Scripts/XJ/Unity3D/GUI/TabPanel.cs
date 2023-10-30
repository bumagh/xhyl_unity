using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class TabPanel : BaseGUI
	{
		public string[] labels;

		private int value;

		public string[] Labels
		{
			get
			{
				return labels;
			}
			set
			{
				labels = value;
			}
		}

		public int Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public TabPanel(string title = null, bool boldTitle = true, int value = 0, params string[] labels)
			: base(title, boldTitle)
		{
			this.value = value;
			this.labels = labels;
		}

		public int Show(params Action[] functions)
		{
			if (functions.Length != labels.Length)
			{
				throw new Exception(string.Format(ErrorMessage.DifferentDataLengthError, functions, labels));
			}
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			UnityEngine.GUILayout.BeginHorizontal();
			value = UnityEngine.GUILayout.Toolbar(value, labels, GUILayout.TabButtonStyle);
			UnityEngine.GUILayout.EndHorizontal();
			UnityEngine.GUILayout.BeginVertical(GUILayout.TabPanelStyle);
			functions[value]();
			UnityEngine.GUILayout.EndVertical();
			UnityEngine.GUILayout.EndVertical();
			return value;
		}
	}
}
