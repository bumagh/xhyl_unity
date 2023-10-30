using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class Vector2GUI : ArrayControllableGUI
	{
		private FloatGUI floatGUIX;

		private FloatGUI floatGUIY;

		public Vector2 MinValue
		{
			get
			{
				return new Vector2(floatGUIX.MinValue, floatGUIY.MinValue);
			}
			set
			{
				floatGUIX.MinValue = value.x;
				floatGUIY.MinValue = value.y;
			}
		}

		public Vector2 MaxValue
		{
			get
			{
				return new Vector2(floatGUIX.MaxValue, floatGUIY.MaxValue);
			}
			set
			{
				floatGUIX.MaxValue = value.x;
				floatGUIY.MaxValue = value.y;
			}
		}

		public int DecimalPlaces
		{
			get
			{
				return floatGUIX.DecimalPlaces;
			}
			set
			{
				floatGUIX.DecimalPlaces = value;
				floatGUIY.DecimalPlaces = value;
			}
		}

		public float TextFieldWidth
		{
			get
			{
				return floatGUIX.TextFieldWidth;
			}
			set
			{
				floatGUIX.TextFieldWidth = value;
				floatGUIY.TextFieldWidth = value;
			}
		}

		public bool WithSlider
		{
			get
			{
				return floatGUIX.WithSlider;
			}
			set
			{
				floatGUIX.WithSlider = value;
				floatGUIY.WithSlider = value;
			}
		}

		public Vector2 Value
		{
			get
			{
				return new Vector2(floatGUIX.Value, floatGUIY.Value);
			}
			set
			{
				floatGUIX.Value = value.x;
				floatGUIY.Value = value.y;
			}
		}

		public Vector2GUI(string title = null, bool boldTitle = false, Vector2? value = default(Vector2?), Vector2? minValue = default(Vector2?), Vector2? maxValue = default(Vector2?), int decimalPlaces = 2, float textFieldWidth = 0f, bool withSlider = false, bool horizontalArray = true)
			: base(title, boldTitle, horizontalArray)
		{
			if (!value.HasValue)
			{
				value = new Vector2(0f, 0f);
			}
			if (!minValue.HasValue)
			{
				minValue = new Vector2(float.MinValue, float.MinValue);
			}
			if (!maxValue.HasValue)
			{
				maxValue = new Vector2(float.MaxValue, float.MaxValue);
			}
			Vector2 value2 = value.Value;
			Vector2 value3 = minValue.Value;
			Vector2 value4 = maxValue.Value;
			floatGUIX = new FloatGUI("X", boldTitle: false, value2.x, value3.x, value4.x, decimalPlaces, textFieldWidth, withSlider);
			floatGUIY = new FloatGUI("Y", boldTitle: false, value2.y, value3.y, value4.y, decimalPlaces, textFieldWidth, withSlider);
		}

		public Vector2 Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			if (base.HorizontalArray)
			{
				UnityEngine.GUILayout.BeginHorizontal();
				floatGUIX.Show();
				floatGUIY.Show();
				UnityEngine.GUILayout.EndHorizontal();
			}
			else
			{
				floatGUIX.Show();
				floatGUIY.Show();
			}
			UnityEngine.GUILayout.EndVertical();
			return Value;
		}
	}
}
