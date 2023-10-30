using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class AgentClip : CurveClip
	{
		public BGCurve agentCurve;

		[Range(-360f, 360f)]
		public float angle;

		public bool useTailAngle;

		public bool flapX;

		public bool flapY;

		public AgentClip()
		{
		}

		public AgentClip(BGCurve curve, BGCcMath math)
			: base(curve, math)
		{
		}

		public override void MakePoints()
		{
			Vector3 startPosition = GetStartPosition();
			Vector3 startTangent = GetStartTangent();
			Vector3 toDirection = MathUtils.V2Rotate(startTangent, angle).normalized;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.right, toDirection);
			int startIndex = base.startIndex;
			int pointsCount = agentCurve.PointsCount;
			int num = base.startIndex;
			int pointsCount2 = agentCurve.PointsCount;
			BGCurvePoint[] array = new BGCurvePoint[pointsCount2];
			Vector3 position = Vector3.zero;
			Vector3 right = Vector3.right;
			BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
			BGCurvePoint.ControlTypeEnum controlTypeEnum = BGCurvePoint.ControlTypeEnum.Absent;
			BGCurvePointI bGCurvePointI = agentCurve.Points[0];
			Vector3 positionLocal = bGCurvePointI.PositionLocal;
			for (int i = 0; i < pointsCount2; i++)
			{
				BGCurvePointI bGCurvePointI2 = agentCurve.Points[i];
				bool flag = i == 0;
				bool flag2 = i == pointsCount2 - 1;
				Vector3 value = bGCurvePointI2.PositionLocal - positionLocal;
				position = startPosition + rotation * FlapV3(value);
				bool flag3 = bGCurvePointI2.ControlType == controlTypeEnum;
				Vector3 vector = flag3 ? Vector3.zero : (rotation * bGCurvePointI2.ControlFirstLocal);
				Vector3 vector2 = flag3 ? Vector3.zero : (rotation * bGCurvePointI2.ControlSecondLocal);
				vector = (flag ? Vector3.zero : vector);
				vector2 = (flag2 ? Vector3.zero : vector2);
				BGCurvePoint bGCurvePoint = array[i] = new BGCurvePoint(curve, position, controlType, FlapV3(vector), FlapV3(vector2));
				num++;
			}
			curve.AddPoints(array, base.startIndex);
			_pointCount = pointsCount2;
			BGCurvePoint point = array[pointsCount2 - 1];
			_tailInfo.index = startIndex;
			_tailInfo.point = point;
			_tailInfo.position = position;
			_tailInfo.tangent = right.normalized;
		}

		private Vector3 FlapV3(Vector3 value)
		{
			return new Vector3(flapX ? (0f - value.x) : value.x, flapY ? (0f - value.y) : value.y, value.z);
		}

		public override CurveClip Clone()
		{
			AgentClip agentClip = new AgentClip(curve, math);
			agentClip.angle = angle;
			agentClip.agentCurve = agentCurve;
			return agentClip;
		}

		public override bool CheckValid()
		{
			return !(agentCurve == null);
		}

		public override void CheckAndOnValidate()
		{
			base.CheckAndOnValidate();
			_usePrevTail = !isHeadClip;
		}

		protected override Vector3 GetStartTangent()
		{
			if (isHeadClip)
			{
				return TailInfo.Default.tangent;
			}
			return (useTailAngle && base.usePrevTail && prevTailInfo != null) ? prevTailInfo.tangent : TailInfo.Default.tangent;
		}
	}
}
