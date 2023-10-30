using UnityEngine;

namespace PathSystem
{
	public class FK3_Sin : FK3_CartesianEquation
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
