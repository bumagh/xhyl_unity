using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public interface BGCurveMathI
	{
		Vector3 CalcByDistanceRatio(BGCurveBaseMath.Field field, float distanceRatio, bool useLocal = false);

		Vector3 CalcByDistance(BGCurveBaseMath.Field field, float distance, bool useLocal = false);

		Vector3 CalcByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false);

		Vector3 CalcByDistance(float distance, out Vector3 tangent, bool useLocal = false);

		Vector3 CalcPositionAndTangentByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false);

		Vector3 CalcPositionAndTangentByDistance(float distance, out Vector3 tangent, bool useLocal = false);

		Vector3 CalcPositionByDistanceRatio(float distanceRatio, bool useLocal = false);

		Vector3 CalcPositionByDistance(float distance, bool useLocal = false);

		Vector3 CalcTangentByDistanceRatio(float distanceRatio, bool useLocal = false);

		Vector3 CalcTangentByDistance(float distance, bool useLocal = false);

		int CalcSectionIndexByDistance(float distance);

		int CalcSectionIndexByDistanceRatio(float ratio);

		Vector3 CalcPositionByClosestPoint(Vector3 point, bool skipSectionsOptimization = false, bool skipPointsOptimization = false);

		Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, bool skipSectionsOptimization = false, bool skipPointsOptimization = false);

		Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, out Vector3 tangent, bool skipSectionsOptimization = false, bool skipPointsOptimization = false);

		float GetDistance(int pointIndex = -1);

		void Recalculate(bool force = false);
	}
}
