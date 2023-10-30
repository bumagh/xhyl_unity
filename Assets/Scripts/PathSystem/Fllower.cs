using UnityEngine;

namespace PathSystem
{
	public class Fllower : PolarEquation
	{
		public float radius = 3f;

		public float count = 3f;

		public override float Evaluate(float theta)
		{
			return radius + Mathf.Sin(count * theta) * Mathf.Sign(Mathf.Sin(count * theta));
		}
	}
}
