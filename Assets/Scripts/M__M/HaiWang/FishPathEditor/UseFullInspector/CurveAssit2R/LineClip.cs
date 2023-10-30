using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public class LineClip : CurveClip
	{
		[Range(-360f, 360f)]
		public float angle;

		public float length = 1f;

		[InspectorDisabled]
		public Vector3 startPos = Vector3.zero;

		public LineClip()
		{
		}

		public LineClip(BGCurve curve, BGCcMath math)
			: base(curve, math)
		{
		}

		public override void MakePoints()
		{
			Vector3 right = Vector3.right;
			int num = startIndex;
			if (isHeadClip)
			{
				startPos = headStartPoint;
				curve.AddPoint(new BGCurvePoint(curve, startPos), num);
				right = Vector3.right;
				num++;
			}
			else
			{
				right = GetStartTangent();
				startPos = GetStartPosition();
			}
			Vector3 vector = startPos + (Vector3)MathUtils.V2Rotate(right, angle).normalized * length;
			BGCurvePoint point = new BGCurvePoint(curve, vector);
			curve.AddPoint(point, num);
			_tailInfo.index = num;
			_tailInfo.point = point;
			_tailInfo.position = vector;
			_tailInfo.tangent = (vector - startPos).normalized;
		}

		public override void Refresh()
		{
			_pointCount = ((!isHeadClip) ? 1 : 2);
		}

		public void RefreshPoints()
		{
		}

		public override void Check()
		{
			base.Check();
			_usePrevTail = !isHeadClip;
		}

		public override CurveClip Clone()
		{
			LineClip lineClip = new LineClip(curve, math);
			lineClip.angle = angle;
			lineClip.length = length;
			return lineClip;
		}
	}
}
