using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(BGCurve))]
	public class BGTestCurveDynamic : MonoBehaviour
	{
		private const int TimeToMoveUp = 3;

		public GameObject ObjectToMove;

		private BGCurve curve;

		private BGCurveBaseMath curveBaseMath;

		private float started;

		private float ratio;

		private LineRenderer lineRenderer;

		private void Start()
		{
			curve = GetComponent<BGCurve>();
			lineRenderer = GetComponent<LineRenderer>();
			curveBaseMath = new BGCurveBaseMath(curve);
			started = Time.time;
			ResetLineRenderer();
			curve.Changed += delegate
			{
				ResetLineRenderer();
			};
		}

		private void ResetLineRenderer()
		{
			Vector3[] array = new Vector3[50];
			for (int i = 0; i < 50; i++)
			{
				array[i] = curveBaseMath.CalcPositionByDistanceRatio((float)i / 49f);
			}
			lineRenderer.positionCount = 50;
			lineRenderer.SetPositions(array);
		}

		private void Update()
		{
			base.transform.RotateAround(Vector3.zero, Vector3.up, 40f * Time.deltaTime);
			ratio = (Time.time - started) / 3f;
			if (ratio >= 1f)
			{
				started = Time.time;
				ratio = 0f;
			}
			else
			{
				ObjectToMove.transform.position = curveBaseMath.CalcPositionByDistanceRatio(ratio);
			}
		}
	}
}
