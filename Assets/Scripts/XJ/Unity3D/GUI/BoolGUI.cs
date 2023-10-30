using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class BoolGUI : BaseGUI
	{
		protected bool value;

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

		public BoolGUI(string title = null, bool boldTitle = false, bool value = false)
			: base(title, boldTitle)
		{
			Value = value;
		}

		public bool Show()
		{
			UnityEngine.GUILayout.BeginHorizontal();
			base.ShowTilte();
			UnityEngine.GUILayout.FlexibleSpace();
			Value = UnityEngine.GUILayout.Toggle(Value, string.Empty);
			UnityEngine.GUILayout.EndHorizontal();
			return Value;
		}

		public override void Display()
		{
			Show();
		}
	}
}
