using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	[RequireComponent(typeof(BGCcMath))]
	public class BGTestCurveRuntime : MonoBehaviour
	{
		private const float OneTierHeight = 2f;

		private const int PointsCountToAdd = 8;

		private const float PointMoveTime = 1.5f;

		private const int MaximumControlSpread = 4;

		public GameObject ObjectToMove;

		private BGCurve curve;

		private BGCcMath math;

		private float started;

		private int counter;

		private Vector3 nextPosition;

		private Vector3 nextControl1;

		private Vector3 nextControl2;

		private void Start()
		{
			curve = GetComponent<BGCurve>();
			math = GetComponent<BGCcMath>();
			Reset();
		}

		private void Reset()
		{
			curve.Clear();
			curve.AddPoint(new BGCurvePoint(curve, Vector3.zero)
			{
				ControlType = BGCurvePoint.ControlTypeEnum.BezierIndependant
			});
			started = Time.time;
			counter = 0;
		}

		private void Update()
		{
			float num = Time.time - started;
			if (num >= 1.5f || curve.PointsCount < 2)
			{
				if (counter == 8)
				{
					Reset();
					return;
				}
				Vector3 positionLocal = curve[curve.PointsCount - 1].PositionLocal;
				curve.AddPoint(new BGCurvePoint(curve, positionLocal)
				{
					ControlType = BGCurvePoint.ControlTypeEnum.BezierIndependant,
					ControlFirstLocal = Vector3.zero,
					ControlSecondLocal = Vector3.zero
				});
				bool flag = counter < 4;
				nextPosition = positionLocal + new Vector3(0f, (!flag) ? (-2f) : 2f, 0f);
				Vector3 b = new Vector3(UnityEngine.Random.Range(-4, 4), ((!flag) ? (-2f) : 2f) * 0.5f, UnityEngine.Random.Range(-4, 4));
				nextControl1 = Vector3.Lerp(positionLocal - nextPosition, b, 0.8f);
				nextControl2 = Vector3.Lerp(nextPosition - positionLocal, b, 0.8f);
				started = Time.time;
				counter++;
				if (curve.PointsCount > 2)
				{
					curve.Delete(0);
				}
			}
			else
			{
				float num2 = num / 1.5f;
				BGCurvePointI bGCurvePointI = curve[curve.PointsCount - 1];
				bGCurvePointI.PositionLocal = Vector3.Lerp(bGCurvePointI.PositionLocal, nextPosition, num2);
				bGCurvePointI.ControlFirstLocal = Vector3.Lerp(bGCurvePointI.PositionLocal, nextControl1, num2);
				bGCurvePointI.ControlFirstLocal = Vector3.Lerp(bGCurvePointI.PositionLocal, nextControl2, num2);
				BGCurveBaseMath.SectionInfo sectionInfo = math.Math[0];
				ObjectToMove.transform.position = math.Math.CalcByDistance(BGCurveBaseMath.Field.Position, sectionInfo.DistanceFromStartToOrigin + sectionInfo.Distance * num2);
			}
		}
	}
}
