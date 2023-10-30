using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public interface FK3_IEquation
	{
		float FillPoints(List<Vector4> points, int segmentCount, Transform parentTransform, bool closePath);

		Vector3 GetTangent(FK3_EndType type, Transform parentTransform);
	}
}
