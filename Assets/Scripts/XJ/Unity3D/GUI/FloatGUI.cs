using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class FloatGUI : BaseGUI
	{
		protected float minValue;

		protected float maxValue;

		protected int decimalPlaces;

		protected float textFieldWidth;

		protected bool withSlider;

		private float value;

		private string text;

		private bool textIsValid;

		public float MinValue
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

		public float MaxValue
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

		public int DecimalPlaces
		{
			get
			{
				return decimalPlaces;
			}
			set
			{
				decimalPlaces = value;
			}
		}

		public float TextFieldWidth
		{
			get
			{
				return TextFieldWidth;
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

		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = (float)Math.Round(value, decimalPlaces);
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
				textIsValid = (float.TryParse(text, out float result) && !text.EndsWith("."));
				if (textIsValid)
				{
					result = (this.value = (float)Math.Round(result, decimalPlaces));
					text = result.ToString();
				}
			}
		}

		public FloatGUI(string title = null, bool boldTitle = false, float value = 0f, float minValue = float.MinValue, float maxValue = float.MaxValue, int decimalPlaces = 2, float textFieldWidth = 0f, bool withSlider = false)
			: base(title, boldTitle)
		{
			Value = value;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.decimalPlaces = decimalPlaces;
			this.textFieldWidth = textFieldWidth;
			this.withSlider = withSlider;
		}

		public float Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			UnityEngine.GUILayout.BeginHorizontal();
			base.ShowTilte();
			GUILayoutOption gUILayoutOption = (!(textFieldWidth <= 0f)) ? UnityEngine.GUILayout.Width(textFieldWidth) : UnityEngine.GUILayout.ExpandWidth(expand: true);
			Text = UnityEngine.GUILayout.TextField(Text, GUILayout.NumericTextBoxStyle, gUILayoutOption);
			UnityEngine.GUILayout.EndHorizontal();
			if (withSlider)
			{
				float num = UnityEngine.GUILayout.HorizontalSlider(Value, minValue, maxValue);
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
