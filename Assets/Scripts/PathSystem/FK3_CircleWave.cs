using UnityEngine;

namespace PathSystem
{
	public class FK3_CircleWave : FK3_PolarEquation
	{
		public float radius = 3f;

		public float amplitude = 1f;

		public float count = 6f;

		public override float Evaluate(float theta)
		{
			return radius + amplitude + amplitude * Mathf.Sin(count * theta);
		}
	}
}
