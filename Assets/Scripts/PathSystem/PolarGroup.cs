using System.Collections.Generic;

namespace PathSystem
{
	public class PolarGroup : PolarEquation
	{
		public List<PolarEquation> equations;

		public override float Evaluate(float theta)
		{
			int count = equations.Count;
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				if (equations[i] != null)
				{
					num += equations[i].Evaluate(theta);
				}
			}
			return num;
		}
	}
}
