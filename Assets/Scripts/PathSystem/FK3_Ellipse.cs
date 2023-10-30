using UnityEngine;

namespace PathSystem
{
	public class FK3_Ellipse : FK3_ParametricEquation
	{
		public float a = 5f;

		public float b = 3f;

		public override Vector3 Evaluate(float t)
		{
			return new Vector3(a * Mathf.Cos(t), b * Mathf.Sin(t), 0f);
		}
	}
}
