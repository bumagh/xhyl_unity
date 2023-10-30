using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public class FreeClip : CurveClip
	{
		private bool hasPrevPoint => base.pointCount > 0;

		private bool hasLastPoint => base.pointCount > 0;

		public FreeClip()
		{
		}

		public FreeClip(BGCurve curve, BGCcMath math)
			: base(curve, math)
		{
			base.usePrevTail = false;
		}

		[InspectorShowIf("hasPrevPoint")]
		[InspectorButton]
		public void DeleteFirstPoint()
		{
		}

		[InspectorButton]
		[InspectorShowIf("hasLastPoint")]
		public void DeleteLastPoint()
		{
		}

		[InspectorButton]
		[InspectorName("测试选择")]
		public void TrySelectPoint_Test()
		{
		}
	}
}
