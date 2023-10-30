using System;
using UnityEngine;
using XJ.Unity3D.GUI;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_View_Test_Selector : FK3_ViewBase
	{
		public ButtonGUI leftBtn;

		public ButtonGUI rightBtn;

		public ButtonGUI analyzeBtn;

		public string fishName = string.Empty;

		public string[] parameters = new string[0];

		public string[] clips = new string[0];

		public Action<int> onParameterClick;

		public Action<int> onClipClick;

		private FK3_IMGUI_RectM rectM;

		private Action displayAction;

		private Action onChangeParameters;

		private Action onChangeClips;

		public FK3_View_Test_Selector()
		{
			rectM = new FK3_IMGUI_RectM();
			rectM.SetSize(100f, 20f).SetMarginLeft(50f).SetMarginRight(50f);
			leftBtn = new ButtonGUI("previous");
			rightBtn = new ButtonGUI("next");
			analyzeBtn = new ButtonGUI("analyze");
			UnityEngine.Debug.LogError("================被我注销了==============");
			parameters = new string[3]
			{
				"1",
				"2",
				"3"
			};
			clips = new string[3]
			{
				"a",
				"b",
				"c"
			};
			Init_Fish_Prefab_Info();
		}

		public void ChangeParameters(string[] parameters)
		{
			if (onChangeParameters != null)
			{
				onChangeParameters();
			}
		}

		public void ChangeClips(string[] clips)
		{
			if (onChangeClips != null)
			{
				onChangeClips();
			}
		}

		private void Init_Fish_Prefab_Info()
		{
			FlexibleWindow window = new FlexibleWindow("FishInfo", 0f, 0f, 300f, 200f, 500f, 500f, enableDrag: true, isVisible: true);
			Toolbar toolbar_parameters = new Toolbar("parameters", boldTitle: false, 0, parameters);
			Toolbar toolbar_clips = new Toolbar("clips", boldTitle: false, 0, clips);
			onParameterClick = delegate(int index)
			{
				UnityEngine.Debug.Log(index);
				UnityEngine.Debug.Log($"Parameter[{parameters[index]}] selected");
			};
			onClipClick = delegate(int index)
			{
				UnityEngine.Debug.Log($"Clip[{clips[index]}] selected");
			};
			toolbar_parameters.OnChanged = delegate(int newValue, int oldValue)
			{
				if (onParameterClick != null)
				{
					onParameterClick(newValue);
				}
			};
			toolbar_clips.OnChanged = delegate(int newValue, int oldValue)
			{
				if (onClipClick != null)
				{
					onClipClick(newValue);
				}
			};
			GUI.WindowFunction controlWindow = delegate
			{
				UnityEngine.GUILayout.Label(fishName);
				toolbar_parameters.Show();
				toolbar_clips.Show();
			};
			displayAction = delegate
			{
				toolbar_parameters.Labels = parameters;
				toolbar_clips.Labels = clips;
				window.Show(controlWindow);
			};
		}

		public override void Display()
		{
			leftBtn.Display();
			rightBtn.Display();
			analyzeBtn.Display();
			Rect offsetRect = rectM.Clone().SetSize(200f, 25f).SetLayout(FK3_IMGUI_Layout_Type.BottomCenter)
				.GetOffsetRect(0f, -20f);
			GUI.Box(offsetRect, fishName);
			if (displayAction != null)
			{
				displayAction();
			}
		}

		public override void Update()
		{
			leftBtn.Update();
			rightBtn.Update();
			analyzeBtn.Update();
		}
	}
}
