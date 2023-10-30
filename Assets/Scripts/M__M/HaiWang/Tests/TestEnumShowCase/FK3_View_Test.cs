using System;
using System.Collections.Generic;
using UnityEngine;
using XJ.Unity3D.GUI;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_View_Test : FK3_ViewBase
	{
		private List<ButtonGUI> btns;

		private FK3_IMGUI_RectM rectM;

		public FK3_View_Test()
		{
			rectM = new FK3_IMGUI_RectM();
			rectM.SetSize(100f, 20f);
			btns = new List<ButtonGUI>();
			KeyValuePair<string, FK3_IMGUI_Layout_Type> keyValuePair = new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Top Left", FK3_IMGUI_Layout_Type.TopLeft);
			KeyValuePair<string, FK3_IMGUI_Layout_Type>[] array = new KeyValuePair<string, FK3_IMGUI_Layout_Type>[9]
			{
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Top Left", FK3_IMGUI_Layout_Type.TopLeft),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Top Right", FK3_IMGUI_Layout_Type.TopRight),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Top Center", FK3_IMGUI_Layout_Type.TopCenter),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Middle Left", FK3_IMGUI_Layout_Type.MiddleLeft),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Middle Right", FK3_IMGUI_Layout_Type.MiddleRight),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Middle Center", FK3_IMGUI_Layout_Type.MiddleCenter),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("BottomLeft", FK3_IMGUI_Layout_Type.BottomLeft),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Bottom Right", FK3_IMGUI_Layout_Type.BottomRight),
				new KeyValuePair<string, FK3_IMGUI_Layout_Type>("Bottom Center", FK3_IMGUI_Layout_Type.BottomCenter)
			};
			Action<string> onClick = delegate(string name)
			{
				UnityEngine.Debug.Log($"btn[{name}] clicked");
			};
			KeyValuePair<string, FK3_IMGUI_Layout_Type>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				KeyValuePair<string, FK3_IMGUI_Layout_Type> pair = array2[i];
				ButtonGUI buttonGUI = new ButtonGUI(pair.Key);
				buttonGUI.SetPosition(rectM.GetRect(pair.Value));
				buttonGUI.SetAction(delegate
				{
					onClick(pair.Key);
				});
				btns.Add(buttonGUI);
			}
		}

		public override void Display()
		{
			foreach (ButtonGUI btn in btns)
			{
				btn.Display();
			}
		}
	}
}
