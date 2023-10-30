using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class FoldoutPanel : BaseGUI
	{
		private bool value;

		public bool Value
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

		public FoldoutPanel(string title = null, bool boldTitle = true, bool value = false)
			: base(title, boldTitle)
		{
			this.value = value;
		}

		public bool Show(Action function)
		{
			string text = (value ? "▼ " : "► ") + title;
			GUIStyle style = GUILayout.FoldoutPanelStyle;
			if (boldTitle)
			{
				style = GUILayout.FoldoutPanelBoldStyle;
			}
			bool flag = UnityEngine.GUILayout.Button(text, style);
			value = (value != flag);
			if (value)
			{
				function();
			}
			return value;
		}

		public override void Display()
		{
		}
	}
}
