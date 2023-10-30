using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_LineClip : FK3_CurveClip
	{
		[Range(-360f, 360f)]
		public float angle;

		public float length = 1f;

		[InspectorDisabled]
		public Vector3 startPos = Vector3.zero;

		public FK3_LineClip()
		{
		}

		public FK3_LineClip(BGCurve curve, BGCcMath math)
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
			Vector3 vector = startPos + (Vector3)FK3_MathUtils.V2Rotate(right, angle).normalized * length;
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

		public override void CheckAndOnValidate()
		{
			base.CheckAndOnValidate();
			_usePrevTail = !isHeadClip;
		}

		public override FK3_CurveClip Clone()
		{
			FK3_LineClip fK3_LineClip = new FK3_LineClip(curve, math);
			fK3_LineClip.angle = angle;
			fK3_LineClip.length = length;
			return fK3_LineClip;
		}
	}
}
