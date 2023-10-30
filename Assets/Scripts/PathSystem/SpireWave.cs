using UnityEngine;

namespace PathSystem
{
	public class SpireWave : PolarEquation
	{
		public float frequency = 10f;

		public float scale = 1f;

		public float amplitude = 1f;

		public override float Evaluate(float theta)
		{
			return theta * scale + 0.1f * theta * amplitude * Mathf.Sin(frequency * theta);
		}
	}
}
