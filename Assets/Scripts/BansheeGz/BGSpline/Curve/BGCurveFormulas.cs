using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public static class BGCurveFormulas
	{
		public static Vector3 BezierCubic(float t, Vector3 from, Vector3 fromControl, Vector3 toControl, Vector3 to)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return num * num2 * from + 3f * num2 * t * fromControl + 3f * num * num3 * toControl + t * num3 * to;
		}

		public static Vector3 BezierQuadratic(float t, Vector3 from, Vector3 control, Vector3 to)
		{
			float num = 1f - t;
			float d = num * num;
			float d2 = t * t;
			return d * from + 2f * num * t * control + d2 * to;
		}

		public static Vector3 BezierCubicDerivative(float t, Vector3 from, Vector3 fromControl, Vector3 toControl, Vector3 to)
		{
			float num = 1f - t;
			return 3f * (num * num) * (fromControl - from) + 6f * num * t * (toControl - fromControl) + 3f * (t * t) * (to - toControl);
		}

		public static Vector3 BezierQuadraticDerivative(float t, Vector3 from, Vector3 control, Vector3 to)
		{
			float num = 1f - t;
			return 2f * num * (control - from) + 2f * t * (to - control);
		}
	}
}
