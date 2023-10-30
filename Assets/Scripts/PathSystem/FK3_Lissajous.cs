using UnityEngine;

namespace PathSystem
{
	public class FK3_Lissajous : FK3_ParametricEquation
	{
		public float a = 5f;

		public float b = 5f;

		public float kX = 3f;

		public float kY = 2f;

		public override Vector3 Evaluate(float t)
		{
			return new Vector3(a * Mathf.Cos(kX * t), b * Mathf.Sin(kY * t), 0f);
		}
	}
}
