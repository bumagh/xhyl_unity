using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class IntGUI : BaseGUI
	{
		protected int minValue;

		protected int maxValue;

		protected float textFieldWidth;

		protected bool withSlider;

		private int value;

		private string text;

		private bool textIsValid;

		public int MinValue
		{
			get
			{
				return minValue;
			}
			set
			{
				minValue = value;
			}
		}

		public int MaxValue
		{
			get
			{
				return maxValue;
			}
			set
			{
				maxValue = value;
			}
		}

		public float TextFieldWidth
		{
			get
			{
				return textFieldWidth;
			}
			set
			{
				textFieldWidth = value;
			}
		}

		public bool WithSlider
		{
			get
			{
				return withSlider;
			}
			set
			{
				withSlider = value;
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
				text = value.ToString();
			}
		}

		public string Text
		{
			get
			{
				if (text == null)
				{
					text = value.ToString();
				}
				return text;
			}
			set
			{
				text = value;
				textIsValid = int.TryParse(text, out int result);
				if (textIsValid)
				{
					this.value = result;
				}
			}
		}

		public IntGUI(string title = null, bool boldTitle = false, int value = 0, int minValue = int.MinValue, int maxValue = int.MaxValue, float textFieldWidth = 0f, bool withSlider = false)
			: base(title, boldTitle)
		{
			Value = value;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.textFieldWidth = textFieldWidth;
			this.withSlider = withSlider;
		}

		public int Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			UnityEngine.GUILayout.BeginHorizontal();
			base.ShowTilte();
			GUILayoutOption gUILayoutOption = (!(TextFieldWidth <= 0f)) ? UnityEngine.GUILayout.Width(TextFieldWidth) : UnityEngine.GUILayout.ExpandWidth(expand: true);
			Text = UnityEngine.GUILayout.TextField(Text, GUILayout.NumericTextBoxStyle, gUILayoutOption);
			UnityEngine.GUILayout.EndHorizontal();
			if (WithSlider)
			{
				int num = (int)UnityEngine.GUILayout.HorizontalSlider(Value, MinValue, MaxValue);
				if (textIsValid)
				{
					Value = num;
				}
				else if (Value != num)
				{
					Value = num;
				}
			}
			UnityEngine.GUILayout.EndVertical();
			return Value;
		}

		public override void Display()
		{
			Show();
		}
	}
}
