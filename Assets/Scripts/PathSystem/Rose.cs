using UnityEngine;

namespace PathSystem
{
	public class Rose : PolarEquation
	{
		public float radius = 3f;

		public float count = 3f;

		public Rose()
		{
			endAngle = 180f;
		}

		public override float Evaluate(float theta)
		{
			return radius * Mathf.Sin(count * theta);
		}
	}
}
