using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class Vector4GUI : ArrayControllableGUI
	{
		private FloatGUI floatGUIX;

		private FloatGUI floatGUIY;

		private FloatGUI floatGUIZ;

		private FloatGUI floatGUIW;

		public Vector4 MinValue
		{
			get
			{
				return new Vector4(floatGUIX.MinValue, floatGUIY.MinValue, floatGUIZ.MinValue, floatGUIW.MinValue);
			}
			set
			{
				floatGUIX.MinValue = value.x;
				floatGUIY.MinValue = value.y;
				floatGUIZ.MinValue = value.z;
				floatGUIW.MinValue = value.w;
			}
		}

		public Vector4 MaxValue
		{
			get
			{
				return new Vector4(floatGUIX.MaxValue, floatGUIY.MaxValue, floatGUIZ.MaxValue, floatGUIW.MaxValue);
			}
			set
			{
				floatGUIX.MaxValue = value.x;
				floatGUIY.MaxValue = value.y;
				floatGUIZ.MaxValue = value.z;
				floatGUIW.MaxValue = value.w;
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
				floatGUIZ.DecimalPlaces = value;
				floatGUIW.DecimalPlaces = value;
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
				floatGUIZ.TextFieldWidth = value;
				floatGUIW.TextFieldWidth = value;
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
				floatGUIZ.WithSlider = value;
				floatGUIW.WithSlider = value;
			}
		}

		public Vector4 Value
		{
			get
			{
				return new Vector4(floatGUIX.Value, floatGUIY.Value, floatGUIZ.Value, floatGUIW.Value);
			}
			set
			{
				floatGUIX.Value = value.x;
				floatGUIY.Value = value.y;
				floatGUIZ.Value = value.z;
				floatGUIW.Value = value.w;
			}
		}

		public Vector4GUI(string title = null, bool boldTitle = false, Vector4? value = default(Vector4?), Vector4? minValue = default(Vector4?), Vector4? maxValue = default(Vector4?), int decimalPlaces = 2, float textFieldWidth = 0f, bool withSlider = false, bool horizontalArray = true)
			: base(title, boldTitle, horizontalArray)
		{
			if (!value.HasValue)
			{
				value = new Vector4(0f, 0f, 0f, 0f);
			}
			if (!minValue.HasValue)
			{
				minValue = new Vector4(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
			}
			if (!maxValue.HasValue)
			{
				maxValue = new Vector4(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
			}
			Vector4 value2 = value.Value;
			Vector4 value3 = minValue.Value;
			Vector4 value4 = maxValue.Value;
			floatGUIX = new FloatGUI("X", boldTitle: false, value2.x, value3.x, value4.x, decimalPlaces, textFieldWidth, withSlider);
			floatGUIY = new FloatGUI("Y", boldTitle: false, value2.y, value3.y, value4.y, decimalPlaces, textFieldWidth, withSlider);
			floatGUIZ = new FloatGUI("Z", boldTitle: false, value2.z, value3.z, value4.z, decimalPlaces, textFieldWidth, withSlider);
			floatGUIW = new FloatGUI("W", boldTitle: false, value2.w, value3.w, value4.w, decimalPlaces, textFieldWidth, withSlider);
		}

		public Vector4 Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			if (base.HorizontalArray)
			{
				UnityEngine.GUILayout.BeginHorizontal();
				floatGUIX.Show();
				floatGUIY.Show();
				floatGUIZ.Show();
				floatGUIW.Show();
				UnityEngine.GUILayout.EndHorizontal();
			}
			else
			{
				floatGUIX.Show();
				floatGUIY.Show();
				floatGUIZ.Show();
				floatGUIW.Show();
			}
			UnityEngine.GUILayout.EndVertical();
			return Value;
		}
	}
}
