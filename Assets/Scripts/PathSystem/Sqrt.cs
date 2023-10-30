using UnityEngine;

namespace PathSystem
{
	public class Sqrt : CartesianEquation
	{
		public float amplitude = 1f;

		public override float Evaluate(float x)
		{
			return amplitude * Mathf.Sqrt(x);
		}
	}
}
