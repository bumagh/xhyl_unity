using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class Vector2ArrayGUI : BaseGUI
	{
		private Vector2[] values;

		private Vector2GUI[] vector2GUIs;

		private FoldoutPanel foldOutPanel;

		public string[] Titles
		{
			get;
			set;
		}

		public Vector2 MinValue
		{
			get
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				return vector2GUIs[0].MinValue;
			}
			set
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				int num = vector2GUIs.Length;
				for (int i = 0; i < num; i++)
				{
					vector2GUIs[i].MinValue = value;
				}
			}
		}

		public Vector2 MaxValue
		{
			get
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				return vector2GUIs[0].MaxValue;
			}
			set
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				int num = vector2GUIs.Length;
				for (int i = 0; i < num; i++)
				{
					vector2GUIs[i].MaxValue = value;
				}
			}
		}

		public int DecimalPlaces
		{
			get
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				return vector2GUIs[0].DecimalPlaces;
			}
			set
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				int num = vector2GUIs.Length;
				for (int i = 0; i < num; i++)
				{
					vector2GUIs[i].DecimalPlaces = value;
				}
			}
		}

		public float TextFieldWidth
		{
			get
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				return vector2GUIs[0].TextFieldWidth;
			}
			set
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				int num = vector2GUIs.Length;
				for (int i = 0; i < num; i++)
				{
					vector2GUIs[i].TextFieldWidth = value;
				}
			}
		}

		public bool WithSlider
		{
			get
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				return vector2GUIs[0].WithSlider;
			}
			set
			{
				if (vector2GUIs == null)
				{
					throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, Values));
				}
				int num = vector2GUIs.Length;
				for (int i = 0; i < num; i++)
				{
					vector2GUIs[i].WithSlider = value;
				}
			}
		}

		public Vector2[] Values
		{
			get
			{
				return values;
			}
			set
			{
				int num = values.Length;
				if (num != value.Length)
				{
					throw new Exception(string.Format(ErrorMessage.DifferentDataLengthError, "Current Values", "New Values"));
				}
				for (int i = 0; i < num; i++)
				{
					values[i] = value[i];
					vector2GUIs[i].Value = value[i];
				}
			}
		}

		public Vector2ArrayGUI(string title, bool boldTitle, string[] titles, Vector2[] values, Vector2? minValue = default(Vector2?), Vector2? maxValue = default(Vector2?), int decimalPlaces = 2, float textFieldWidth = 0f, bool withSlider = true)
			: base(title, boldTitle)
		{
			if (values == null)
			{
				throw new Exception(string.Format(ErrorMessage.NullOrNoDataError, values));
			}
			if (!minValue.HasValue)
			{
				minValue = new Vector2(float.MinValue, float.MinValue);
			}
			if (!maxValue.HasValue)
			{
				maxValue = new Vector2(float.MaxValue, float.MaxValue);
			}
			foldOutPanel = new FoldoutPanel(base.title, base.boldTitle);
			int num = values.Length;
			vector2GUIs = new Vector2GUI[num];
			for (int i = 0; i < num; i++)
			{
				vector2GUIs[i] = new Vector2GUI();
			}
			this.values = values;
			Values = values;
			Titles = titles;
			MinValue = minValue.Value;
			MaxValue = maxValue.Value;
			DecimalPlaces = decimalPlaces;
			TextFieldWidth = textFieldWidth;
			WithSlider = withSlider;
		}

		public Vector2[] Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			if (title != null)
			{
				foldOutPanel.Show(delegate
				{
					ShowVector2GUIs();
				});
			}
			else
			{
				ShowVector2GUIs();
			}
			UnityEngine.GUILayout.EndVertical();
			return values;
		}

		protected virtual void ShowVector2GUIs()
		{
			int num = values.Length;
			if (Titles != null)
			{
				int num2 = Titles.Length;
				if (num2 < num)
				{
					throw new Exception(string.Format(ErrorMessage.DifferentDataLengthError, values, Titles));
				}
				for (int i = 0; i < num; i++)
				{
					values[i] = vector2GUIs[i].Show();
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					values[j] = vector2GUIs[j].Show();
				}
			}
		}
	}
}
