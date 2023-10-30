using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public class FK3_CircleCurveMaker
	{
		public float startAngle;

		public float endAngle = 360f;

		public float radius = 3f;

		private const float Factor = 0.5522848f;

		public BGCurve curve;

		public Vector3 center = Vector3.zero;

		private BGCurvePoint[] points;

		public FK3_CircleCurveMaker(BGCurve curve)
		{
			this.curve = curve;
		}

		public BGCurvePoint[] MakePoints(int num = 4)
		{
			BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
			Vector3 right = Vector3.right;
			float num2 = 360 / num;
			float controlFactor = GetControlFactor(num2);
			UnityEngine.Debug.Log(controlFactor);
			points = new BGCurvePoint[num + 1];
			for (int i = 0; i < num + 1; i++)
			{
				Vector3 vector = EvaluatePoint((float)i * num2);
				Vector3 vector2 = vector - center;
				Vector3 a = FK3_MathUtils.V2Rotate(new Vector2(vector2.x, vector2.y), -90f);
				a.Normalize();
				Vector3 vector3 = a * (controlFactor * radius);
				Vector3 controlSecond = vector3 * -1f;
				points[i] = new BGCurvePoint(curve, vector, controlType, vector3, controlSecond);
			}
			return points;
		}

		public float Evaluate(float theta)
		{
			return radius;
		}

		public Vector3 EvaluatePoint(float theta)
		{
			return PolarToCartesianXY(Evaluate(theta), theta, center);
		}

		public static Vector3 PolarToCartesianXY(float r, float theta, Vector3 center)
		{
			return new Vector3(r * Mathf.Cos(theta * ((float)Math.PI / 180f)), r * Mathf.Sin(theta * ((float)Math.PI / 180f)), 0f) + center;
		}

		public static float GetControlFactor(float angle)
		{
			return 4f * Mathf.Tan(angle * ((float)Math.PI / 180f) / 4f) / 3f;
		}
	}
}
