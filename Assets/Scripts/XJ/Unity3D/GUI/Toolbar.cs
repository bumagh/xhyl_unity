using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class Toolbar : BaseGUI
	{
		public delegate void OnChangedFun(int newValue, int oldValue);

		public string[] labels;

		private int value;

		public OnChangedFun OnChanged;

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
				if (value != this.value && OnChanged != null)
				{
					OnChanged(value, this.value);
				}
				this.value = value;
			}
		}

		public Toolbar(string title = null, bool boldTitle = true, int value = 0, params string[] labels)
			: base(title, boldTitle)
		{
			this.value = value;
			this.labels = labels;
		}

		public int Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			UnityEngine.GUILayout.BeginHorizontal();
			Value = UnityEngine.GUILayout.Toolbar(value, labels);
			UnityEngine.GUILayout.EndHorizontal();
			UnityEngine.GUILayout.EndVertical();
			return value;
		}
	}
}
