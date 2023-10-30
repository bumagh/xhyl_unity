using UnityEngine;

namespace PathSystem
{
	public class FK3_Sqrt : FK3_CartesianEquation
	{
		public float amplitude = 1f;

		public override float Evaluate(float x)
		{
			return amplitude * Mathf.Sqrt(x);
		}
	}
}
