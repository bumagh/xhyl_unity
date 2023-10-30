using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class EnumGUI<T> : BaseGUI
	{
		protected T value;

		protected bool editing;

		protected int buttonWidth;

		public T Value
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

		public EnumGUI(string title = null, bool boldTitle = false, int value = 0, int buttonWidth = 0)
			: base(title, boldTitle)
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enum.");
			}
			this.value = (T)(object)value;
			this.buttonWidth = buttonWidth;
		}

		public T Show()
		{
			UnityEngine.GUILayout.BeginVertical();
			UnityEngine.GUILayout.BeginHorizontal();
			base.ShowTilte();
			if (buttonWidth <= 0)
			{
				if (UnityEngine.GUILayout.Button(value.ToString()))
				{
					editing = !editing;
				}
			}
			else if (UnityEngine.GUILayout.Button(value.ToString(), UnityEngine.GUILayout.Width(buttonWidth)))
			{
				editing = !editing;
			}
			UnityEngine.GUILayout.EndHorizontal();
			if (editing)
			{
				UnityEngine.GUILayout.BeginVertical();
				Array values = Enum.GetValues(typeof(T));
				for (int i = 0; i < values.Length; i++)
				{
					T val = (T)values.GetValue(i);
					if (val.Equals(value))
					{
						continue;
					}
					if (buttonWidth <= 0)
					{
						if (UnityEngine.GUILayout.Button(val.ToString()))
						{
							value = val;
							editing = false;
						}
						continue;
					}
					UnityEngine.GUILayout.BeginHorizontal();
					UnityEngine.GUILayout.FlexibleSpace();
					if (UnityEngine.GUILayout.Button(val.ToString(), UnityEngine.GUILayout.Width(buttonWidth)))
					{
						value = val;
						editing = false;
					}
					UnityEngine.GUILayout.EndHorizontal();
				}
				UnityEngine.GUILayout.EndVertical();
			}
			UnityEngine.GUILayout.EndVertical();
			return value;
		}

		public override void Display()
		{
			Show();
		}
	}
}
