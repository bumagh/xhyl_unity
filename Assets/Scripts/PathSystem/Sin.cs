using UnityEngine;

namespace PathSystem
{
	public class Sin : CartesianEquation
	{
		public float amplitude = 1f;

		public float frequency = 1f;

		public float phase;

		public override float Evaluate(float x)
		{
			return amplitude * Mathf.Sin(frequency * x + phase);
		}
	}
}
