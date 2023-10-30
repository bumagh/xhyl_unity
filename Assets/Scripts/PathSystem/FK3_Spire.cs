using FullInspector;
using UnityEngine;

namespace PathSystem
{
	public class FK3_Spire : FK3_PolarEquation
	{
		[InspectorRange(0.0001f, 20f, float.NaN)]
		public float scale1 = 1f;

		public float scale2 = 1f;

		[InspectorRange(1f, 20f, float.NaN)]
		public float amplitude = 1f;

		public override float Evaluate(float theta)
		{
			return scale1 * (scale2 * theta + (amplitude - 1f) * Mathf.Pow(theta * amplitude, 2f));
		}
	}
}
