using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class FK3_ParametricGroup : FK3_ParametricEquation
	{
		public List<FK3_ParametricEquation> equations;

		public override Vector3 Evaluate(float theta)
		{
			int count = equations.Count;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < count; i++)
			{
				if (equations[i] != null)
				{
					vector += equations[i].Evaluate(theta);
				}
			}
			return vector;
		}
	}
}
