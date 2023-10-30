using UnityEngine;

namespace PathSystem
{
	public class Hypotrochoid : ParametricEquation
	{
		public float bigR = 5f;

		public float smallR = 3f;

		public float d = 5f;

		public override Vector3 Evaluate(float t)
		{
			return new Vector3((bigR - smallR) * Mathf.Cos(t) + d * Mathf.Cos((bigR - smallR) * t / smallR), (bigR - smallR) * Mathf.Sin(t) - d * Mathf.Sin((bigR - smallR) * t / smallR), 0f);
		}
	}
}
