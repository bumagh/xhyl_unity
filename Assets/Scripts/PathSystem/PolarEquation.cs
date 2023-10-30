using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public abstract class PolarEquation : IEquation
	{
		public float startAngle;

		public float endAngle = 360f;

		public abstract float Evaluate(float theta);

		public virtual float FillPoints(List<Vector4> points, int segmentCount, Transform parentTransform, bool closePath)
		{
			if (segmentCount <= 1)
			{
				return 0f;
			}
			float num = (float)Math.PI / 180f * startAngle;
			float num2 = (float)Math.PI / 180f * endAngle;
			float num3 = (num2 - num) / (float)segmentCount;
			float num4 = 0f;
			float r = Evaluate(num);
			Vector3 vector = parentTransform.TransformPoint(Utils.PolarToCartesian(r, num));
			points.Add(vector);
			num += num3;
			int num5 = 1;
			while (num5 < segmentCount)
			{
				float r2 = Evaluate(num);
				points.Add(parentTransform.TransformPoint(Utils.PolarToCartesian(r2, num)));
				num4 += Vector3.Distance(points[num5 - 1], points[num5]);
				num5++;
				num += num3;
			}
			Vector3 vector2 = parentTransform.TransformPoint(Utils.PolarToCartesian(Evaluate(num2), num2));
			points.Add(vector2);
			num4 += Vector3.Distance(points[num5 - 1], points[num5]);
			if (closePath)
			{
				float num6 = Vector3.Distance(vector, vector2);
				if (num6 < 0.1f)
				{
					points.RemoveAt(points.Count - 1);
				}
				else
				{
					num4 += num6;
				}
			}
			return num4;
		}

		public Vector3 GetTangent(EndType type, Transform parentTransform)
		{
			float num = (type == EndType.Start) ? ((float)Math.PI / 180f * startAngle) : ((float)Math.PI / 180f * endAngle);
			float theta = num - 0.01f;
			float theta2 = num + 0.01f;
			Vector3 b = parentTransform.TransformPoint(Utils.PolarToCartesian(Evaluate(theta), theta));
			Vector3 a = parentTransform.TransformPoint(Utils.PolarToCartesian(Evaluate(theta2), theta2));
			return Vector3.Normalize(a - b);
		}
	}
}
