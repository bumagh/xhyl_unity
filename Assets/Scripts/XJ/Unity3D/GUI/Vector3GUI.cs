using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class Vector3GUI : ArrayControllableGUI
	{
		private FloatGUI floatGUIX;

		private FloatGUI floatGUIY;

		private FloatGUI floatGUIZ;

		public Vector3 MinValue
		{
			get
			{
				return new Vector3(floatGUIX.MinValue, floatGUIY.MinValue, floatGUIZ.MinValue);
			}
			set
			{
				floatGUIX.MinValue = value.x;
				floatGUIY.MinValue = value.y;
				floatGUIZ.MinValue = value.z;
			}
		}

		public Vector3 MaxValue
		{
			get
			{
				return new Vector3(floatGUIX.MaxValue, floatGUIY.MaxValue, floatGUIZ.MaxValue);
			}
			set
			{
				floatGUIX.MaxValue = value.x;
				floatGUIY.MaxValue = value.y;
				floatGUIZ.MaxValue = value.z;
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
			}
		}

		public Vector3 Value
		{
			get
			{
				return new Vector3(floatGUIX.Value, floatGUIY.Value, floatGUIZ.Value);
			}
			set
			{
				floatGUIX.Value = value.x;
				floatGUIY.Value = value.y;
				floatGUIZ.Value = value.z;
			}
		}

		public Vector3GUI(string title = null, bool boldTitle = false, Vector3? value = default(Vector3?), Vector3? minValue = default(Vector3?), Vector3? maxValue = default(Vector3?), int decimalPlaces = 2, float textFieldWidth = 0f, bool withSlider = false, bool horizontalArray = true)
			: base(title, boldTitle, horizontalArray)
		{
			if (!value.HasValue)
			{
				value = new Vector3(0f, 0f, 0f);
			}
			if (!minValue.HasValue)
			{
				minValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			}
			if (!maxValue.HasValue)
			{
				maxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			}
			Vector3 value2 = value.Value;
			Vector3 value3 = minValue.Value;
			Vector3 value4 = maxValue.Value;
			floatGUIX = new FloatGUI("X", boldTitle: false, value2.x, value3.x, value4.x, decimalPlaces, textFieldWidth, withSlider);
			floatGUIY = new FloatGUI("Y", boldTitle: false, value2.y, value3.y, value4.y, decimalPlaces, textFieldWidth, withSlider);
			floatGUIZ = new FloatGUI("Z", boldTitle: false, value2.z, value3.z, value4.z, decimalPlaces, textFieldWidth, withSlider);
		}

		public Vector3 Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			base.ShowTilte();
			if (base.HorizontalArray)
			{
				UnityEngine.GUILayout.BeginHorizontal();
				floatGUIX.Show();
				floatGUIY.Show();
				floatGUIZ.Show();
				UnityEngine.GUILayout.EndHorizontal();
			}
			else
			{
				floatGUIX.Show();
				floatGUIY.Show();
				floatGUIZ.Show();
			}
			UnityEngine.GUILayout.EndVertical();
			return Value;
		}
	}
}
