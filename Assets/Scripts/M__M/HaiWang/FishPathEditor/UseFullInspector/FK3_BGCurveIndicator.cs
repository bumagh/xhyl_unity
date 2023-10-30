using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_BGCurveIndicator
	{
		[InspectorDisabled]
		public int index;

		[HideInInspector]
		public BGCurve curve;

		[InspectorButton]
		public void Select()
		{
		}
	}
}
