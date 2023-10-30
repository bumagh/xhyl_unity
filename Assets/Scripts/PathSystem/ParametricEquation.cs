using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public abstract class ParametricEquation : IEquation
	{
		public float startT;

		public float endT = (float)Math.PI * 2f;

		public abstract Vector3 Evaluate(float t);

		public virtual float FillPoints(List<Vector4> points, int segmentCount, Transform parentTransform, bool closePath)
		{
			if (segmentCount < 1)
			{
				return 0f;
			}
			float num = startT;
			float num2 = (endT - startT) / (float)segmentCount;
			float num3 = 0f;
			Vector3 vector = parentTransform.TransformPoint(Evaluate(num));
			points.Add(vector);
			num += num2;
			int num4 = 1;
			while (num4 < segmentCount)
			{
				points.Add(parentTransform.TransformPoint(Evaluate(num)));
				num3 += Vector3.Distance(points[num4 - 1], points[num4]);
				num4++;
				num += num2;
			}
			Vector3 vector2 = parentTransform.TransformPoint(Evaluate(endT));
			points.Add(vector2);
			num3 += Vector3.Distance(points[num4 - 1], points[num4]);
			if (closePath)
			{
				float num5 = Vector3.Distance(vector, vector2);
				if (num5 < 0.1f)
				{
					points.RemoveAt(points.Count - 1);
				}
				else
				{
					num3 += num5;
				}
			}
			return num3;
		}

		public Vector3 GetTangent(EndType type, Transform parentTransform)
		{
			float num = (type == EndType.Start) ? startT : endT;
			float t = num - 0.01f;
			float t2 = num + 0.01f;
			Vector3 b = parentTransform.TransformPoint(Evaluate(t));
			Vector3 a = parentTransform.TransformPoint(Evaluate(t2));
			return Vector3.Normalize(a - b);
		}
	}
}
