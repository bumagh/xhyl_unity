using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public interface IEquation
	{
		float FillPoints(List<Vector4> points, int segmentCount, Transform parentTransform, bool closePath);

		Vector3 GetTangent(EndType type, Transform parentTransform);
	}
}
