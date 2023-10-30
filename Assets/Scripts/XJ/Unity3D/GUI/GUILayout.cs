using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class GUILayout
	{
		public static readonly RectOffset BaseMargin;

		public static readonly RectOffset BasePadding;

		public static readonly Color BaseTextColor;

		public static readonly Texture2D NormalBackgroundTexture;

		public static readonly Texture2D HoverBackgroundTexture;

		public static readonly Texture2D ActiveBackgroundTexture;

		public static readonly Texture2D FocusedBackgroundTexture;

		private static readonly Color NormalBackgroundColor;

		private static readonly Color HoverBackgroundColor;

		private static readonly Color ActiveBackgroundColor;

		private static readonly Color FocusedBackgroundColor;

		public static readonly GUIStyle TabButtonStyle;

		public static readonly GUIStyle TabPanelStyle;

		public static readonly GUIStyle BoldLabelStyle;

		public static readonly GUIStyle MiddleRightAlignedLabelStyle;

		public static readonly GUIStyle LowerCenterAlignedLabelStyle;

		public static readonly GUIStyle NumericTextBoxStyle;

		public static readonly GUIStyle FoldoutPanelStyle;

		public static readonly GUIStyle FoldoutPanelBoldStyle;

		public static readonly GUIStyle ComboBoxStyle;

		static GUILayout()
		{
			BaseMargin = new RectOffset(5, 5, 5, 5);
			BasePadding = new RectOffset(5, 5, 2, 2);
			BaseTextColor = new Color
			{
				a = 1f,
				r = 1f,
				g = 1f,
				b = 1f
			};
			NormalBackgroundColor = new Color
			{
				a = 0.5f,
				r = 0f,
				g = 0f,
				b = 0f
			};
			HoverBackgroundColor = new Color
			{
				a = 0.5f,
				r = 0.8f,
				g = 0.8f,
				b = 0.8f
			};
			ActiveBackgroundColor = new Color
			{
				a = 0.4f,
				r = 0.8f,
				g = 0.8f,
				b = 0.8f
			};
			FocusedBackgroundColor = new Color
			{
				a = 0.5f,
				r = 1f,
				g = 1f,
				b = 1f
			};
			NormalBackgroundTexture = GenerateBackgroundTexture(NormalBackgroundColor);
			HoverBackgroundTexture = GenerateBackgroundTexture(HoverBackgroundColor);
			ActiveBackgroundTexture = GenerateBackgroundTexture(ActiveBackgroundColor);
			FocusedBackgroundTexture = GenerateBackgroundTexture(FocusedBackgroundColor);
			TabButtonStyle = new GUIStyle();
			TabButtonStyle.margin = new RectOffset(1, 1, BaseMargin.top, 0);
			TabButtonStyle.padding = BasePadding;
			TabButtonStyle.alignment = TextAnchor.MiddleCenter;
			TabButtonStyle.normal.textColor = BaseTextColor;
			TabButtonStyle.hover.textColor = BaseTextColor;
			TabButtonStyle.active.textColor = BaseTextColor;
			TabButtonStyle.onNormal.textColor = BaseTextColor;
			TabButtonStyle.onHover.textColor = BaseTextColor;
			TabButtonStyle.onActive.textColor = BaseTextColor;
			TabButtonStyle.normal.background = NormalBackgroundTexture;
			TabButtonStyle.hover.background = HoverBackgroundTexture;
			TabButtonStyle.active.background = ActiveBackgroundTexture;
			TabButtonStyle.onNormal.background = ActiveBackgroundTexture;
			TabButtonStyle.onHover.background = HoverBackgroundTexture;
			TabButtonStyle.onActive.background = ActiveBackgroundTexture;
			TabPanelStyle = new GUIStyle();
			TabPanelStyle.normal.background = ActiveBackgroundTexture;
			MiddleRightAlignedLabelStyle = new GUIStyle();
			MiddleRightAlignedLabelStyle.margin = BaseMargin;
			MiddleRightAlignedLabelStyle.normal.textColor = BaseTextColor;
			MiddleRightAlignedLabelStyle.alignment = TextAnchor.MiddleRight;
			LowerCenterAlignedLabelStyle = new GUIStyle(MiddleRightAlignedLabelStyle);
			LowerCenterAlignedLabelStyle.alignment = TextAnchor.LowerCenter;
			BoldLabelStyle = new GUIStyle(MiddleRightAlignedLabelStyle);
			BoldLabelStyle.fontStyle = FontStyle.Bold;
			BoldLabelStyle.alignment = TextAnchor.MiddleLeft;
			NumericTextBoxStyle = new GUIStyle();
			NumericTextBoxStyle.alignment = TextAnchor.MiddleRight;
			NumericTextBoxStyle.normal.background = NormalBackgroundTexture;
			NumericTextBoxStyle.normal.textColor = BaseTextColor;
			NumericTextBoxStyle.active.background = NormalBackgroundTexture;
			NumericTextBoxStyle.active.textColor = BaseTextColor;
			NumericTextBoxStyle.focused.background = NormalBackgroundTexture;
			NumericTextBoxStyle.focused.textColor = BaseTextColor;
			NumericTextBoxStyle.margin = BaseMargin;
			NumericTextBoxStyle.padding = BasePadding;
			FoldoutPanelStyle = new GUIStyle();
			FoldoutPanelStyle.margin = BaseMargin;
			FoldoutPanelStyle.normal.textColor = BaseTextColor;
			FoldoutPanelStyle.hover.textColor = BaseTextColor;
			FoldoutPanelStyle.hover.background = HoverBackgroundTexture;
			FoldoutPanelBoldStyle = new GUIStyle(FoldoutPanelStyle);
			FoldoutPanelBoldStyle.fontStyle = FontStyle.Bold;
			ComboBoxStyle = new GUIStyle();
			ComboBoxStyle.alignment = TextAnchor.MiddleCenter;
			ComboBoxStyle.normal.background = NormalBackgroundTexture;
			ComboBoxStyle.hover.background = HoverBackgroundTexture;
			ComboBoxStyle.active.background = ActiveBackgroundTexture;
			ComboBoxStyle.onNormal.background = ActiveBackgroundTexture;
			ComboBoxStyle.onHover.background = HoverBackgroundTexture;
			ComboBoxStyle.onActive.background = ActiveBackgroundTexture;
		}

		public static void BoldLabel(string text)
		{
			UnityEngine.GUILayout.Label(text, BoldLabelStyle);
		}

		private static Texture2D GenerateBackgroundTexture(Color color)
		{
			int num = 1;
			int num2 = 1;
			Color[] array = new Color[num * num2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			Texture2D texture2D = new Texture2D(num, num2);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}
	}
}
