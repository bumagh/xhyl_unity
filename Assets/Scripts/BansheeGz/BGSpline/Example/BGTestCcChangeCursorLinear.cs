using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCcChangeCursorLinear : MonoBehaviour
	{
		private sealed class Sequence
		{
			public const float Epsilon = 0.1f;

			private readonly List<ExpectedPoint> expectedPoints = new List<ExpectedPoint>();

			private BGCcCursorChangeLinear changeCursor;

			private int pointCursor;

			private bool valid = true;

			private float lastPoint;

			private float started;

			public BGCurve Curve;

			private float Elapsed => Time.time - started;

			public Sequence(BGCcCursorChangeLinear changeCursor)
			{
				this.changeCursor = changeCursor;
				BGCcCursor cursor = changeCursor.Cursor;
				BGCurve bGCurve = Curve = changeCursor.Curve;
				started = Time.time;
				if (!Curve.gameObject.activeInHierarchy)
				{
					return;
				}
				ThrowIf("Stop overflow control is not supported", changeCursor.OverflowControl == BGCcCursorChangeLinear.OverflowControlEnum.Stop);
				int pointsCount = bGCurve.PointsCount;
				ThrowIf("Curve should have at least 2 points.", pointsCount < 2);
				BGCurveBaseMath math = changeCursor.Cursor.Math.Math;
				int num = cursor.CalculateSectionIndex();
				float currentSpeed = changeCursor.CurrentSpeed;
				if (currentSpeed > 0f)
				{
					if (bGCurve.Closed && num == pointsCount - 1)
					{
						expectedPoints.Add(new ExpectedPoint(0, math.GetDistance() - cursor.Distance, currentSpeed, 0f));
					}
					else if (!bGCurve.Closed && num == pointsCount - 2)
					{
						expectedPoints.Add(new ExpectedPoint(pointsCount - 1, math.GetDistance() - cursor.Distance, currentSpeed, 0f));
					}
					else
					{
						expectedPoints.Add(new ExpectedPoint(num + 1, math[num + 1].DistanceFromStartToOrigin - cursor.Distance, currentSpeed, 0f));
					}
					for (int i = num + 2; i < pointsCount; i++)
					{
						expectedPoints.Add(new ExpectedPoint(i, math[i - 1].Distance, changeCursor.GetSpeedAtPoint(i - 1), changeCursor.GetDelayAtPoint(i - 1)));
					}
					if (bGCurve.Closed && num != pointsCount)
					{
						expectedPoints.Add(new ExpectedPoint(0, math[pointsCount - 1].Distance, changeCursor.GetSpeedAtPoint(pointsCount - 1), changeCursor.GetDelayAtPoint(pointsCount - 1)));
					}
					if (changeCursor.OverflowControl == BGCcCursorChangeLinear.OverflowControlEnum.PingPong)
					{
						if (bGCurve.Closed)
						{
							expectedPoints.Add(new ExpectedPoint(pointsCount - 1, math[pointsCount - 1].Distance, changeCursor.GetSpeedAtPoint(pointsCount - 1), changeCursor.GetDelayAtPoint(0)));
						}
						for (int num2 = pointsCount - 2; num2 >= 0; num2--)
						{
							expectedPoints.Add(new ExpectedPoint(num2, math[num2].Distance, changeCursor.GetSpeedAtPoint(num2), changeCursor.GetDelayAtPoint(num2 + 1)));
						}
					}
					else if (!bGCurve.Closed)
					{
						expectedPoints.Add(new ExpectedPoint(0, math[pointsCount - 2].Distance, 0f, changeCursor.GetDelayAtPoint(pointsCount - 1)));
					}
					for (int j = 1; j <= num; j++)
					{
						expectedPoints.Add(new ExpectedPoint(j, math[j - 1].Distance, changeCursor.GetSpeedAtPoint(j - 1), changeCursor.GetDelayAtPoint(j - 1)));
					}
					expectedPoints.Add(new ExpectedPoint(-1, cursor.Distance - math[num].DistanceFromStartToOrigin, currentSpeed, changeCursor.GetDelayAtPoint(num)));
					return;
				}
				expectedPoints.Add(new ExpectedPoint(num, cursor.Distance - math[num].DistanceFromStartToOrigin, currentSpeed, 0f));
				for (int num3 = num - 1; num3 >= 0; num3--)
				{
					expectedPoints.Add(new ExpectedPoint(num3, math[num3].Distance, changeCursor.GetSpeedAtPoint(num3), changeCursor.GetDelayAtPoint(num3 + 1)));
				}
				if (changeCursor.OverflowControl == BGCcCursorChangeLinear.OverflowControlEnum.PingPong)
				{
					for (int k = 1; k < pointsCount; k++)
					{
						expectedPoints.Add(new ExpectedPoint(k, math[k - 1].Distance, changeCursor.GetSpeedAtPoint(k - 1), changeCursor.GetDelayAtPoint(k - 1)));
					}
					if (bGCurve.Closed)
					{
						expectedPoints.Add(new ExpectedPoint(0, math[pointsCount - 1].Distance, changeCursor.GetSpeedAtPoint(pointsCount - 1), changeCursor.GetDelayAtPoint(pointsCount - 1)));
						expectedPoints.Add(new ExpectedPoint(pointsCount - 1, math[pointsCount - 1].Distance, changeCursor.GetSpeedAtPoint(pointsCount - 1), changeCursor.GetDelayAtPoint(0)));
					}
				}
				else if (bGCurve.Closed)
				{
					expectedPoints.Add(new ExpectedPoint(pointsCount - 1, math[pointsCount - 1].Distance, changeCursor.GetSpeedAtPoint(pointsCount - 1), changeCursor.GetDelayAtPoint(0)));
				}
				else
				{
					expectedPoints.Add(new ExpectedPoint(pointsCount - 1, 0f, 0f, changeCursor.GetDelayAtPoint(0)));
				}
				for (int num4 = pointsCount - 2; num4 > num; num4--)
				{
					expectedPoints.Add(new ExpectedPoint(num4, math[num4].Distance, changeCursor.GetSpeedAtPoint(num4), changeCursor.GetDelayAtPoint(num4 + 1)));
				}
				expectedPoints.Add(new ExpectedPoint(-1, math[num].DistanceFromEndToOrigin - cursor.Distance, changeCursor.GetSpeedAtPoint(num), changeCursor.GetDelayAtPoint(num + 1)));
			}

			private void ThrowIf(string message, bool condition)
			{
				if (condition)
				{
					throw GetException(message);
				}
			}

			private UnityException GetException(string message)
			{
				return new UnityException(message + ". Curve=" + changeCursor.Curve.gameObject.name);
			}

			public void Check()
			{
				if (!valid)
				{
					return;
				}
				ExpectedPoint expectedPoint = expectedPoints[pointCursor];
				if (expectedPoint.PointIndex == -1)
				{
					if (expectedPoint.ExpectedDelay < (double)Elapsed)
					{
						MoveNext();
					}
				}
				else if (expectedPoint.ExpectedDelay < (double)(Elapsed - 0.1f))
				{
					valid = false;
					UnityEngine.Debug.LogException(GetException("Missing event: expected " + expectedPoint + " event did not occur"));
				}
			}

			public void Reached(int point)
			{
				if (!valid)
				{
					return;
				}
				ExpectedPoint expectedPoint = expectedPoints[pointCursor];
				if (expectedPoint.PointIndex >= 0 && expectedPoint.PointIndex != point)
				{
					valid = false;
					UnityEngine.Debug.LogException(GetException("Points indexes mismatch: expected " + expectedPoint.PointIndex + ", actual=" + point));
					return;
				}
				double expectedDelay = expectedPoint.ExpectedDelay;
				float elapsed = Elapsed;
				if (Math.Abs(expectedDelay - (double)elapsed) > 0.10000000149011612)
				{
					valid = false;
					UnityEngine.Debug.LogException(GetException("Timing mismatch at point {" + expectedPoint.PointIndex + "}: expected " + expectedDelay + ", actual=" + elapsed));
				}
				else
				{
					MoveNext();
				}
			}

			private void MoveNext()
			{
				started = Time.time;
				pointCursor = ((pointCursor != expectedPoints.Count - 1) ? (pointCursor + 1) : 0);
			}
		}

		private sealed class ExpectedPoint
		{
			public readonly float Distance;

			public readonly int PointIndex;

			public readonly float Speed;

			public readonly float Delay;

			public double ExpectedDelay
			{
				get
				{
					float num = Math.Abs(Speed);
					float num2 = Mathf.Clamp(Delay, 0f, float.MaxValue);
					return num2 + ((!(num < 0.1f)) ? (Distance / num) : 0.1f);
				}
			}

			public ExpectedPoint(int pointIndex, float distance, float speed, float delay)
			{
				Speed = speed;
				Distance = distance;
				PointIndex = pointIndex;
				Delay = delay;
			}

			public string ToString()
			{
				return "Point " + PointIndex + " after " + ExpectedDelay + " delay.";
			}
		}

		private readonly List<Sequence> sequences = new List<Sequence>(10);

		private void Start()
		{
			BGCcCursorChangeLinear[] componentsInChildren = GetComponentsInChildren<BGCcCursorChangeLinear>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Register(componentsInChildren[i]);
			}
		}

		private void Register(BGCcCursorChangeLinear curve)
		{
			sequences.Add(new Sequence(curve));
		}

		private void Update()
		{
			foreach (Sequence sequence in sequences)
			{
				if (sequence.Curve.gameObject.activeInHierarchy)
				{
					sequence.Check();
				}
			}
		}

		public void PointReached0(int point)
		{
			Process(0, point);
		}

		public void PointReached1(int point)
		{
			Process(1, point);
		}

		public void PointReached2(int point)
		{
			Process(2, point);
		}

		public void PointReached3(int point)
		{
			Process(3, point);
		}

		public void PointReached4(int point)
		{
			Process(4, point);
		}

		public void PointReached5(int point)
		{
			Process(5, point);
		}

		public void PointReached6(int point)
		{
			Process(6, point);
		}

		public void PointReached7(int point)
		{
			Process(7, point);
		}

		public void PointReached8(int point)
		{
			Process(8, point);
		}

		public void PointReached9(int point)
		{
			Process(9, point);
		}

		public void PointReached10(int point)
		{
			Process(10, point);
		}

		public void PointReached11(int point)
		{
			Process(11, point);
		}

		public void PointReached12(int point)
		{
			Process(12, point);
		}

		public void PointReached13(int point)
		{
			Process(13, point);
		}

		public void PointReached14(int point)
		{
			Process(14, point);
		}

		public void PointReached15(int point)
		{
			Process(15, point);
		}

		public void PointReached16(int point)
		{
			Process(16, point);
		}

		public void PointReached17(int point)
		{
			Process(17, point);
		}

		public void PointReached18(int point)
		{
			Process(18, point);
		}

		public void PointReached19(int point)
		{
			Process(19, point);
		}

		public void PointReached20(int point)
		{
			Process(20, point);
		}

		public void PointReached21(int point)
		{
			Process(21, point);
		}

		public void PointReached22(int point)
		{
			Process(22, point);
		}

		public void PointReached23(int point)
		{
			Process(23, point);
		}

		public void PointReached24(int point)
		{
			Process(24, point);
		}

		public void PointReached25(int point)
		{
			Process(25, point);
		}

		public void PointReached26(int point)
		{
			Process(26, point);
		}

		public void PointReached27(int point)
		{
			Process(27, point);
		}

		public void PointReached28(int point)
		{
			Process(28, point);
		}

		public void PointReached29(int point)
		{
			Process(29, point);
		}

		public void PointReached30(int point)
		{
			Process(30, point);
		}

		public void PointReached31(int point)
		{
			Process(31, point);
		}

		public void PointReached32(int point)
		{
			Process(32, point);
		}

		private void Process(int curve, int pointIndex)
		{
			Sequence sequence = sequences[curve];
			if (sequence.Curve.gameObject.activeSelf)
			{
				sequence.Reached(pointIndex);
			}
		}
	}
}
