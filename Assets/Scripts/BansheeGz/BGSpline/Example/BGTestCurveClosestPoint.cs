using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveClosestPoint : MonoBehaviour
	{
		[Tooltip("Line renderer material")]
		public Material LineRendererMaterial;

		[Tooltip("Object to use for point's indication")]
		public GameObject PointIndicator;

		[Range(1f, 100f)]
		[Tooltip("How much points to use with search")]
		public int NumberOfPointsToSeek = 10;

		[Range(2f, 100f)]
		[Tooltip("How much points to add to the curve")]
		public int NumberOfCurvePoints = 100;

		[Range(1f, 30f)]
		[Tooltip("How much sections to use for splitting each curve's segment")]
		public int NumberOfSplits = 30;

		[Tooltip("Transition period")]
		[Range(1f, 5f)]
		public int Period = 4;

		[Tooltip("Use slow check method to validate results")]
		public bool CheckResults;

		private BGCurve curve;

		private BGCcMath math;

		private static Vector3 min = new Vector3(-10f, 0f, -2f);

		private static Vector3 max = new Vector3(10f, 10f, 2f);

		private GameObject[] objects;

		private Vector3[] oldCurvePointPos;

		private Vector3[] newCurvePointPos;

		private Vector3[] oldPointPos;

		private Vector3[] newPointPos;

		private float startTime = -100000f;

		private int ErrorPointIndex = -1;

		private GUIStyle style;

		private bool HasError => ErrorPointIndex >= 0;

		private void Start()
		{
			curve = base.gameObject.AddComponent<BGCurve>();
			curve.Closed = true;
			math = base.gameObject.AddComponent<BGCcMath>();
			base.gameObject.AddComponent<BGCcVisualizationLineRenderer>();
			LineRenderer component = base.gameObject.GetComponent<LineRenderer>();
			component.sharedMaterial = LineRendererMaterial;
			Color color = new Color(0.2f, 0.2f, 0.2f, 1f);
			float num3 = component.startWidth = (component.endWidth = 0.03f);
			Color color4 = component.startColor = (component.endColor = color);
			math.SectionParts = NumberOfSplits;
			for (int i = 0; i < NumberOfCurvePoints; i++)
			{
				int num4 = UnityEngine.Random.Range(0, 3);
				BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.Absent;
				switch (num4)
				{
				case 1:
					controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
					break;
				case 2:
					controlType = BGCurvePoint.ControlTypeEnum.BezierSymmetrical;
					break;
				}
				curve.AddPoint(new BGCurvePoint(curve, Vector3.zero, controlType, RandomVector() * 0.3f, RandomVector() * 0.3f));
			}
			oldPointPos = new Vector3[NumberOfPointsToSeek];
			newPointPos = new Vector3[NumberOfPointsToSeek];
			oldCurvePointPos = new Vector3[NumberOfCurvePoints];
			newCurvePointPos = new Vector3[NumberOfCurvePoints];
			InitArray(newCurvePointPos, oldCurvePointPos);
			InitArray(newPointPos, oldPointPos);
			objects = new GameObject[NumberOfPointsToSeek];
			for (int j = 0; j < NumberOfPointsToSeek; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(PointIndicator);
				gameObject.transform.parent = base.transform;
				objects[j] = gameObject;
			}
			PointIndicator.SetActive(value: false);
			InitCycle();
		}

		private void OnGUI()
		{
			if (style == null)
			{
				style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 20
				};
			}
			GUI.Label(new Rect(0f, 30f, 600f, 30f), "Turn on Gizmos to see Debug lines", style);
		}

		private void InitCycle()
		{
			InitArray(oldCurvePointPos, newCurvePointPos);
			InitArray(oldPointPos, newPointPos);
		}

		private void Update()
		{
			if (HasError)
			{
				Process(ErrorPointIndex, suppressWarning: true);
				return;
			}
			Calculate(null, null);
			float num = Time.time - startTime;
			if (num > (float)Period)
			{
				startTime = Time.time;
				InitCycle();
				num = 0f;
			}
			float t = num / (float)Period;
			for (int i = 0; i < NumberOfCurvePoints; i++)
			{
				curve[i].PositionLocal = Vector3.Lerp(oldCurvePointPos[i], newCurvePointPos[i], t);
			}
			for (int j = 0; j < NumberOfPointsToSeek; j++)
			{
				objects[j].transform.localPosition = Vector3.Lerp(oldPointPos[j], newPointPos[j], t);
			}
		}

		private void Calculate(object sender, EventArgs e)
		{
			for (int i = 0; i < NumberOfPointsToSeek; i++)
			{
				Process(i);
				if (HasError)
				{
					break;
				}
			}
		}

		private void Process(int i, bool suppressWarning = false)
		{
			Vector3 position = objects[i].transform.position;
			float distance;
			Vector3 vector = math.CalcPositionByClosestPoint(position, out distance);
			UnityEngine.Debug.DrawLine(position, vector, Color.yellow);
			if (!CheckResults)
			{
				return;
			}
			float distance2;
			Vector3 vector2 = CalcPositionByClosestPoint(math, position, out distance2);
			UnityEngine.Debug.DrawLine(position, vector2, Color.blue);
			bool flag = Math.Abs(distance - distance2) > 0.01f;
			bool flag2 = Vector3.Magnitude(vector - vector2) > 0.001f;
			if ((!flag && !flag2) || !(Mathf.Abs((position - vector).magnitude - (position - vector2).magnitude) > 1E-05f))
			{
				return;
			}
			ErrorPointIndex = i;
			if (!suppressWarning)
			{
				UnityEngine.Debug.Log("Error detected. Simulation stopped, but erroneous iteration's still running. Use debugger to debug the issue.");
				UnityEngine.Debug.Log("!!! Discrepancy detected while calculating pos by closest point: 1) [Using math] pos=" + vector + ", distance=" + distance + "  2) [Using check method] pos=" + vector2 + ", distance=" + distance2);
				if (flag2)
				{
					UnityEngine.Debug.Log("Reason: Result points varies more than " + 1E-05f + ". Difference=" + Vector3.Magnitude(vector - vector2));
				}
				if (flag)
				{
					UnityEngine.Debug.Log("Reason: Distances varies more than 1cm. Difference=" + Math.Abs(distance - distance2));
				}
				Vector3 a = math.CalcByDistance(BGCurveBaseMath.Field.Position, distance);
				Vector3 a2 = math.CalcByDistance(BGCurveBaseMath.Field.Position, distance2);
				UnityEngine.Debug.Log("Distance check: 1) [Using math] check=" + ((!(Vector3.SqrMagnitude(a - vector) < 1E-05f)) ? "failed" : "passed") + "  2) [Using check method] check=" + ((!(Vector3.SqrMagnitude(a2 - vector2) < 1E-05f)) ? "failed" : "passed"));
				float num = Vector3.Distance(position, vector);
				float num2 = Vector3.Distance(position, vector2);
				UnityEngine.Debug.Log("Actual distance: 1) [Using math] Dist=" + num + "  2) [Using check method] Dist=" + num2 + ((!(Math.Abs(num - num2) > 1E-05f)) ? string.Empty : (". And the winner is " + ((!(num < num2)) ? "check method" : "math"))));
			}
		}

		private static Vector3 CalcPositionByClosestPoint(BGCcMath math, Vector3 targetPoint, out float distance)
		{
			List<BGCurveBaseMath.SectionInfo> sectionInfos = math.Math.SectionInfos;
			int count = sectionInfos.Count;
			Vector3 result = sectionInfos[0][0].Position;
			float num = Vector3.SqrMagnitude(sectionInfos[0][0].Position - targetPoint);
			distance = 0f;
			for (int i = 0; i < count; i++)
			{
				BGCurveBaseMath.SectionInfo sectionInfo = sectionInfos[i];
				List<BGCurveBaseMath.SectionPointInfo> points = sectionInfo.Points;
				int count2 = points.Count;
				for (int j = 1; j < count2; j++)
				{
					BGCurveBaseMath.SectionPointInfo sectionPointInfo = points[j];
					float ratio;
					Vector3 vector = CalcClosestPointToLine(points[j - 1].Position, sectionPointInfo.Position, targetPoint, out ratio);
					float num2 = Vector3.SqrMagnitude(targetPoint - vector);
					if (!(num > num2))
					{
						continue;
					}
					num = num2;
					result = vector;
					if (ratio == 1f)
					{
						int index = i;
						int i2 = j;
						if (j == count2 - 1 && i < count - 1)
						{
							index = i + 1;
							i2 = 0;
						}
						distance = sectionInfos[index].DistanceFromStartToOrigin + sectionInfos[index][i2].DistanceToSectionStart;
					}
					else
					{
						distance = sectionInfos[i].DistanceFromStartToOrigin + Mathf.Lerp(sectionInfo[j - 1].DistanceToSectionStart, sectionPointInfo.DistanceToSectionStart, ratio);
					}
				}
			}
			return result;
		}

		private static Vector3 RandomVector()
		{
			return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
		}

		private static void InitArray(Vector3[] oldArray, Vector3[] newArray)
		{
			for (int i = 0; i < oldArray.Length; i++)
			{
				oldArray[i] = newArray[i];
				newArray[i] = RandomVector();
			}
		}

		private static Vector3 CalcClosestPointToLine(Vector3 a, Vector3 b, Vector3 p, out float ratio)
		{
			Vector3 lhs = p - a;
			Vector3 vector = b - a;
			float sqrMagnitude = vector.sqrMagnitude;
			if (Math.Abs(sqrMagnitude) < 1E-05f)
			{
				ratio = 1f;
				return b;
			}
			float num = Vector3.Dot(lhs, vector) / sqrMagnitude;
			if (num < 0f)
			{
				ratio = 0f;
				return a;
			}
			if (num > 1f)
			{
				ratio = 1f;
				return b;
			}
			ratio = num;
			return a + vector * num;
		}
	}
}
