using BansheeGz.BGSpline.Curve;
using FullInspector;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_SineWaveClip : FK3_CurveClip
	{
		[InspectorRange(0f, 360f, float.NaN)]
		public int startPhaseDegree;

		public int phaseNumber = 2;

		public const float basicQuarter = (float)Math.PI / 2f;

		public float quarter = (float)Math.PI / 2f;

		public float amplitude = 1f;

		public float startAngle;

		public bool keepPrevTail = true;

		[ShowInInspector]
		public float freq => quarter / ((float)Math.PI / 2f);

		public override void MakePoints()
		{
			Vector3 startPosition = GetStartPosition();
			Vector3 startTangent = GetStartTangent();
			int num = phaseNumber + 1;
			BGCurvePoint[] array = new BGCurvePoint[num];
			BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
			int num2 = startIndex;
			Vector3 position = Vector3.zero;
			Vector3 right = Vector3.right;
			int num3 = startPhaseDegree / 90 % 4;
			Vector3 toDirection = FK3_MathUtils.V2Rotate(startTangent, startAngle).normalized;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.right, toDirection);
			float height = GetHeight(num3);
			for (int i = 0; i < num; i++)
			{
				bool flag = i == 0;
				bool flag2 = i == num - 1;
				position = startPosition + rotation * new Vector3((float)i * quarter, (GetHeight(num3) - height) * amplitude);
				Vector3 vector = GetControl(num3);
				vector.y *= amplitude;
				vector.x = vector.x * quarter / ((float)Math.PI / 2f);
				Vector3 point = vector * -1f;
				point = (flag ? Vector3.zero : (rotation * point));
				vector = (flag2 ? Vector3.zero : (rotation * vector));
				num3 = (num3 + 1) % 4;
				array[i] = new BGCurvePoint(curve, position, controlType, point, vector);
				num2++;
			}
			curve.AddPoints(array, startIndex);
			_pointCount = num;
			BGCurvePoint point2 = array[num - 1];
			_tailInfo.index = num2;
			_tailInfo.point = point2;
			_tailInfo.position = position;
			_tailInfo.tangent = (keepPrevTail ? startTangent.normalized : right.normalized);
		}

		private Vector3 GetControl(int phase)
		{
			phase %= 4;
			switch (phase)
			{
			case 0:
				return new Vector3(0.5122866f, 0.5122866f, 0f);
			case 1:
				return new Vector3(0.5122866f, 0f, 0f);
			case 2:
				return new Vector3(0.5122866f, -0.5122866f, 0f);
			case 3:
				return new Vector3(0.5122866f, 0f, 0f);
			default:
				return new Vector3(0.5122866f, 0f, 0f);
			}
		}

		private float GetHeight(int phase)
		{
			switch (phase)
			{
			case 1:
				return 1f;
			case 3:
				return -1f;
			default:
				return 0f;
			}
		}

		public override void CheckAndOnValidate()
		{
			base.CheckAndOnValidate();
			_usePrevTail = !isHeadClip;
			phaseNumber = ((phaseNumber <= 1) ? 1 : phaseNumber);
			startPhaseDegree = startPhaseDegree / 90 * 90;
		}

		public override FK3_CurveClip Clone()
		{
			FK3_SineWaveClip fK3_SineWaveClip = new FK3_SineWaveClip();
			fK3_SineWaveClip.startPhaseDegree = startPhaseDegree;
			fK3_SineWaveClip.phaseNumber = phaseNumber;
			fK3_SineWaveClip.quarter = quarter;
			fK3_SineWaveClip.amplitude = amplitude;
			fK3_SineWaveClip.startAngle = startAngle;
			return fK3_SineWaveClip;
		}
	}
}
