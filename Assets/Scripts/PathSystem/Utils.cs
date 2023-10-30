using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class Utils
	{
		public static Vector3 PolarToCartesian(float r, float theta)
		{
			return new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), 0f);
		}

		public static void DrawLine(List<Vector4> points, bool closed)
		{
			int i;
			for (i = 0; i < points.Count - 1; i++)
			{
				Gizmos.DrawLine(points[i], points[i + 1]);
			}
			if (closed)
			{
				Gizmos.DrawLine(points[i], points[0]);
			}
		}
	}
}
