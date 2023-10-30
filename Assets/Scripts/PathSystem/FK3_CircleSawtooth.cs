using System;

namespace PathSystem
{
	public class FK3_CircleSawtooth : FK3_PolarEquation
	{
		public float count = 3f;

		public float amplitude = 0.6f;

		public override float Evaluate(float theta)
		{
			return amplitude * (theta % ((float)Math.PI * 2f / count) * count);
		}
	}
}
