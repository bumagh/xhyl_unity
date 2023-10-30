using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public abstract class CartesianEquation : IEquation
	{
		public float startX;

		public float endX = 10f;

		public abstract float Evaluate(float x);

		public virtual float FillPoints(List<Vector4> points, int segmentCount, Transform parentTransform, bool closePath)
		{
			if (segmentCount < 1)
			{
				return 0f;
			}
			float num = startX;
			float num2 = (endX - startX) / (float)segmentCount;
			float num3 = 0f;
			float y = Evaluate(num);
			Vector3 vector = parentTransform.TransformPoint(new Vector3(num, y, 0f));
			points.Add(vector);
			num += num2;
			int num4 = 1;
			while (num4 < segmentCount)
			{
				float y2 = Evaluate(num);
				points.Add(parentTransform.TransformPoint(new Vector3(num, y2, 0f)));
				num3 += Vector3.Distance(points[num4 - 1], points[num4]);
				num4++;
				num += num2;
			}
			Vector3 vector2 = parentTransform.TransformPoint(new Vector3(endX, Evaluate(endX), 0f));
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
			float num = (type == EndType.Start) ? startX : endX;
			float x = num - 0.01f;
			float x2 = num + 0.01f;
			Vector3 b = parentTransform.TransformPoint(new Vector3(x, Evaluate(x), 0f));
			Vector3 a = parentTransform.TransformPoint(new Vector3(x2, Evaluate(x2), 0f));
			return Vector3.Normalize(a - b);
		}
	}
}
