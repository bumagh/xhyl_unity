using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	[RequireComponent(typeof(BGCcMath))]
	public class BGTestPerformance : MonoBehaviour
	{
		public enum ControlTypeForNewPoints
		{
			Random,
			Absent,
			Bezier
		}

		private const float SpeedRange = 5f;

		private const int Period = 10;

		[Tooltip("Object's prefab")]
		public GameObject ObjectToMove;

		[Tooltip("Limits for points positions and transitions")]
		public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one);

		[Tooltip("Number of points to spawn")]
		[Range(2f, 2000f)]
		public int PointsCount = 100;

		[Range(1f, 500f)]
		[Tooltip("Number of objects to spawn")]
		public int ObjectsCount = 100;

		[Tooltip("Control Type")]
		public ControlTypeForNewPoints ControlType;

		private float startTime = -1000f;

		private BGCurve curve;

		private BGCurveBaseMath math;

		private Vector3[] oldPos;

		private Vector3[] newPos;

		private GameObject[] objects;

		private float[] speed;

		private float[] distances;

		private float oldDistance;

		private void Start()
		{
			curve = GetComponent<BGCurve>();
			BGCcMath component = GetComponent<BGCcMath>();
			math = component.Math;
			curve = component.Curve;
			oldPos = new Vector3[PointsCount];
			newPos = new Vector3[PointsCount];
			speed = new float[ObjectsCount];
			distances = new float[ObjectsCount];
			objects = new GameObject[ObjectsCount];
			for (int i = 0; i < PointsCount; i++)
			{
				BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
				switch (ControlType)
				{
				case ControlTypeForNewPoints.Absent:
					controlType = BGCurvePoint.ControlTypeEnum.Absent;
					break;
				case ControlTypeForNewPoints.Random:
					controlType = (BGCurvePoint.ControlTypeEnum)UnityEngine.Random.Range(0, 3);
					break;
				}
				curve.AddPoint(new BGCurvePoint(curve, RandomVector(), controlType, RandomVector(), RandomVector()));
			}
			math.Recalculate();
			if (ObjectToMove != null)
			{
				float max = oldDistance = math.GetDistance();
				for (int j = 0; j < ObjectsCount; j++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(ObjectToMove, Vector3.zero, Quaternion.identity);
					gameObject.transform.parent = base.transform;
					objects[j] = gameObject;
					distances[j] = UnityEngine.Random.Range(0f, max);
				}
				ObjectToMove.SetActive(value: false);
				for (int k = 0; k < ObjectsCount; k++)
				{
					speed[k] = ((UnityEngine.Random.Range(0, 2) != 0) ? UnityEngine.Random.Range(1.5f, 5f) : UnityEngine.Random.Range(-5f, -1.5f));
				}
			}
		}

		private void Update()
		{
			if (Time.time - startTime > 10f)
			{
				startTime = Time.time;
				for (int i = 0; i < PointsCount; i++)
				{
					oldPos[i] = newPos[i];
					newPos[i] = RandomVector();
				}
			}
			float t = (Time.time - startTime) / 10f;
			BGCurvePointI[] points = curve.Points;
			for (int j = 0; j < PointsCount; j++)
			{
				points[j].PositionLocal = Vector3.Lerp(oldPos[j], newPos[j], t);
			}
			float distance = math.GetDistance();
			if (ObjectToMove != null)
			{
				float num = distance / oldDistance;
				for (int k = 0; k < ObjectsCount; k++)
				{
					float num2 = distances[k];
					num2 *= num;
					num2 += speed[k] * Time.deltaTime;
					if (num2 < 0f)
					{
						speed[k] = 0f - speed[k];
						num2 = 0f;
					}
					else if (num2 > distance)
					{
						speed[k] = 0f - speed[k];
						num2 = distance;
					}
					distances[k] = num2;
					objects[k].transform.position = math.CalcByDistance(BGCurveBaseMath.Field.Position, num2);
				}
			}
			oldDistance = distance;
		}

		private Vector3 RandomVector()
		{
			return new Vector3(Range(0), Range(1), Range(2));
		}

		private float Range(int index)
		{
			return UnityEngine.Random.Range(Bounds.min[index], Bounds.max[index]);
		}
	}
}
