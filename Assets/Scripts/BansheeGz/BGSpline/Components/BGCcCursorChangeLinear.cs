using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BansheeGz.BGSpline.Components
{
	[CcDescriptor(Description = "Change cursor position linearly", Name = "Cursor Change Linear", Icon = "BGCcCursorChangeLinear123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCursorChangeLinear")]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCursorChangeLinear")]
	public class BGCcCursorChangeLinear : BGCcWithCursor
	{
		public enum OverflowControlEnum
		{
			Cycle,
			PingPong,
			Stop
		}

		[Serializable]
		public class PointReachedEvent : UnityEvent<int>
		{
		}

		public class PointReachedArgs : EventArgs
		{
			private static readonly PointReachedArgs Instance = new PointReachedArgs();

			public int PointIndex
			{
				get;
				private set;
			}

			private PointReachedArgs()
			{
			}

			public static PointReachedArgs GetInstance(int index)
			{
				Instance.PointIndex = index;
				return Instance;
			}
		}

		[SerializeField]
		private bool checkStartDelay;

		public const float SpeedThreshold = 1E-05f;

		[SerializeField]
		private float coerceStartDelay;

		[SerializeField]
		[Tooltip("Cursor will be moved in FixedUpdate instead of Update")]
		private bool useFixedUpdate;

		[SerializeField]
		[Tooltip("Constant movement speed along the curve (Speed * Time.deltaTime).You can override this value for each point with speedField")]
		private float speed = 5f;

		[Tooltip("How to change speed, then curve's end reached.")]
		[SerializeField]
		private OverflowControlEnum overflowControl;

		[SerializeField]
		[Tooltip("If curve's length changed, cursor position be adjusted with curve's length to ensure visually constant speed along the curve. ")]
		private bool adjustByTotalLength;

		[SerializeField]
		[Tooltip("Field to store the speed between each point. It should be a float field.")]
		private BGCurvePointField speedField;

		[SerializeField]
		[Tooltip("Delay at each point. You can override this value for each point with delayField")]
		private float delay;

		[SerializeField]
		[Tooltip("Field to store the delays at points. It should be a float field.")]
		private BGCurvePointField delayField;

		[Tooltip("Event is fired, then point is reached")]
		[SerializeField]
		private PointReachedEvent pointReachedEvent = new PointReachedEvent();

		private float oldLength;

		private bool speedReversed;

		private int currentSectionIndex;

		private float delayStarted = -1f;

		private bool speedWasPositiveWhileDelayed;

		private bool skipZeroPoint;

		public float CoerceStartDelay
		{
			get
			{
				return coerceStartDelay;
			}
			set
			{
				ParamChanged(ref coerceStartDelay, value);
			}
		}

		public bool CheckStartDelay
		{
			get
			{
				return checkStartDelay;
			}
			set
			{
				ParamChanged(ref checkStartDelay, value);
			}
		}

		public OverflowControlEnum OverflowControl
		{
			get
			{
				return overflowControl;
			}
			set
			{
				if (ParamChanged(ref overflowControl, value))
				{
					Stopped = false;
				}
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				ParamChanged(ref speed, value);
			}
		}

		public bool AdjustByTotalLength
		{
			get
			{
				return adjustByTotalLength;
			}
			set
			{
				ParamChanged(ref adjustByTotalLength, value);
			}
		}

		public BGCurvePointField SpeedField
		{
			get
			{
				return speedField;
			}
			set
			{
				ParamChanged(ref speedField, value);
			}
		}

		public float Delay
		{
			get
			{
				return delay;
			}
			set
			{
				ParamChanged(ref delay, value);
			}
		}

		public BGCurvePointField DelayField
		{
			get
			{
				return delayField;
			}
			set
			{
				ParamChanged(ref delayField, value);
			}
		}

		public bool UseFixedUpdate
		{
			get
			{
				return useFixedUpdate;
			}
			set
			{
				ParamChanged(ref useFixedUpdate, value);
			}
		}

		public bool Stopped
		{
			get;
			set;
		}

		public bool SpeedReversed => speedReversed;

		public float CurrentSpeed
		{
			get
			{
				if (base.Curve.PointsCount < 2)
				{
					return 0f;
				}
				if (speedField == null)
				{
					return speed;
				}
				float @float = base.Curve[base.Cursor.CalculateSectionIndex()].GetFloat(speedField.FieldName);
				return (!speedReversed) ? @float : (0f - @float);
			}
		}

		public event EventHandler<PointReachedArgs> PointReached;

		public void FirePointReachedStartPoint()
		{
			FirePointReachedEvent(0);
		}

		public override void Start()
		{
			oldLength = base.Cursor.Math.GetDistance();
			if (Application.isPlaying && base.Curve.PointsCount > 1 && (delay > 0f || delayField != null || pointReachedEvent.GetPersistentEventCount() > 0 || this.PointReached != null))
			{
				currentSectionIndex = base.Cursor.Math.Math.CalcSectionIndexByDistance(base.Cursor.Distance);
			}
		}

		private void Update()
		{
			if (!useFixedUpdate)
			{
				Step();
			}
		}

		private void FixedUpdate()
		{
			if (useFixedUpdate)
			{
				Step();
			}
		}

		private void Step()
		{
			if (Stopped || (speedField == null && Mathf.Abs(speed) < 1E-05f))
			{
				return;
			}
			int pointsCount = base.Curve.PointsCount;
			if (pointsCount < 2)
			{
				return;
			}
			BGCcCursor cursor = base.Cursor;
			BGCurveBaseMath math = cursor.Math.Math;
			bool isPlaying = Application.isPlaying;
			if ((isPlaying && delayStarted >= 0f && !CheckIfDelayIsOver(math, cursor)) || delayStarted >= 0f)
			{
				return;
			}
			float num = cursor.Distance;
			float num2 = 0f;
			if (adjustByTotalLength)
			{
				num2 = math.GetDistance();
				if (Math.Abs(num2) > 1E-05f && Math.Abs(oldLength) > 1E-05f && Math.Abs(num2 - oldLength) > 1E-05f)
				{
					num = num * num2 / oldLength;
				}
			}
			int newSectionIndex = -1;
			bool flag = pointReachedEvent.GetPersistentEventCount() > 0 || this.PointReached != null;
			bool flag2 = isPlaying && (delay > 0f || delayField != null);
			if ((flag2 || flag) && CheckForNewDelay(math, num, ref newSectionIndex, flag2, flag))
			{
				return;
			}
			float num3 = speed;
			if (speedField != null)
			{
				if (newSectionIndex == -1)
				{
					newSectionIndex = math.CalcSectionIndexByDistance(num);
				}
				num3 = base.Curve[newSectionIndex].GetFloat(speedField.FieldName);
				if (speedReversed)
				{
					num3 = 0f - num3;
				}
			}
			float newDistance = num + num3 * Time.deltaTime;
			if (newDistance < 0f || newDistance > math.GetDistance())
			{
				Overflow(math, ref newDistance, num3 >= 0f, flag2, flag);
			}
			cursor.Distance = newDistance;
			oldLength = num2;
		}

		public float GetDelayAtPoint(int point)
		{
			return (!(delayField == null)) ? base.Curve[point].GetFloat(delayField.FieldName) : delay;
		}

		public float GetSpeedAtPoint(int point)
		{
			return (!(speedField == null)) ? base.Curve[point].GetFloat(speedField.FieldName) : speed;
		}

		private bool IsDelayRequired(int pointIndex)
		{
			bool flag = delayField != null;
			return (!flag && delay > 0f) || (flag && base.Curve[pointIndex].GetFloat(delayField.FieldName) > 1E-05f);
		}

		private void StartDelay(bool speedIsPositive)
		{
			delayStarted = Time.time;
			speedWasPositiveWhileDelayed = speedIsPositive;
		}

		private bool CheckForNewDelay(BGCurveBaseMath math, float distance, ref int newSectionIndex, bool checkDelay, bool firingEvents)
		{
			if (currentSectionIndex == 0 && skipZeroPoint)
			{
				return false;
			}
			if (!math.Curve.Closed && currentSectionIndex == math.Curve.PointsCount - 1)
			{
				return false;
			}
			newSectionIndex = math.CalcSectionIndexByDistance(distance);
			if (currentSectionIndex != newSectionIndex)
			{
				bool flag;
				if (speedField == null)
				{
					flag = (speed > 0f);
				}
				else
				{
					flag = (base.Curve[currentSectionIndex].GetFloat(speedField.FieldName) > 0f);
					if (speedReversed)
					{
						flag = !flag;
					}
				}
				if (CheckDelayAtSectionChanged(newSectionIndex, checkDelay, firingEvents, flag))
				{
					return true;
				}
			}
			delayStarted = -1f;
			return false;
		}

		private bool CheckDelayAtSectionChanged(int newSectionIndex, bool checkDelay, bool firingEvents, bool speedPositive)
		{
			BGCcCursor cursor = base.Cursor;
			BGCurveBaseMath math = cursor.Math.Math;
			int num = base.Curve.PointsCount - 1;
			if (speedPositive)
			{
				if (newSectionIndex > currentSectionIndex)
				{
					for (int i = currentSectionIndex + 1; i <= newSectionIndex; i++)
					{
						if (firingEvents)
						{
							FirePointReachedEvent(i);
						}
						if (checkDelay && CheckDelayAtPoint(math, cursor, i, speedPositive))
						{
							return true;
						}
					}
				}
			}
			else
			{
				if (currentSectionIndex == 0 && !base.Curve.Closed)
				{
					currentSectionIndex = num;
				}
				if (newSectionIndex < currentSectionIndex)
				{
					for (int num2 = currentSectionIndex; num2 > newSectionIndex; num2--)
					{
						if (firingEvents)
						{
							FirePointReachedEvent(num2);
						}
						if (checkDelay && CheckDelayAtPoint(math, cursor, num2, speedPositive))
						{
							return true;
						}
					}
				}
			}
			currentSectionIndex = newSectionIndex;
			return false;
		}

		private bool CheckDelayAtPoint(BGCurveBaseMath math, BGCcCursor cursor, int pointIndex, bool speedPositive)
		{
			if (IsDelayRequired(pointIndex))
			{
				currentSectionIndex = pointIndex;
				cursor.Distance = ((base.Curve.PointsCount - 1 != pointIndex || base.Curve.Closed) ? math[pointIndex].DistanceFromStartToOrigin : math.GetDistance());
				StartDelay(speedPositive);
				return true;
			}
			return false;
		}

		private void Overflow(BGCurveBaseMath math, ref float newDistance, bool currentSpeedPositive, bool checkDelay, bool firingEvents)
		{
			bool flag = newDistance < 0f;
			float distance = math.GetDistance();
			int num = base.Curve.PointsCount - 1;
			if (checkDelay || firingEvents)
			{
				if (currentSpeedPositive)
				{
					int num2 = num;
					if (currentSectionIndex != num2 && CheckDelayAtSectionChanged(num2, checkDelay, firingEvents, speedPositive: true))
					{
						return;
					}
				}
				else
				{
					if (currentSectionIndex > 0 && CheckDelayAtSectionChanged(0, checkDelay, firingEvents, speedPositive: false))
					{
						return;
					}
					if (!skipZeroPoint && checkDelay && CheckDelayAtPoint(math, base.Cursor, 0, speedPositive: false))
					{
						if (firingEvents)
						{
							FirePointReachedEvent(0);
						}
						skipZeroPoint = true;
						return;
					}
				}
			}
			switch (overflowControl)
			{
			case OverflowControlEnum.Stop:
				newDistance = ((!flag) ? distance : 0f);
				Stopped = true;
				break;
			case OverflowControlEnum.Cycle:
				newDistance = ((!flag) ? (newDistance - distance) : (distance + newDistance));
				break;
			case OverflowControlEnum.PingPong:
				if (speedField == null)
				{
					speed = 0f - speed;
				}
				speedReversed = !speedReversed;
				currentSpeedPositive = !currentSpeedPositive;
				newDistance = ((!flag) ? (distance * 2f - newDistance) : (0f - newDistance));
				break;
			}
			if (newDistance < 0f)
			{
				newDistance = 0f;
			}
			else if (newDistance > distance)
			{
				newDistance = distance;
			}
			if (!checkDelay && !firingEvents)
			{
				return;
			}
			if (base.Curve.Closed)
			{
				if (skipZeroPoint)
				{
					skipZeroPoint = false;
					return;
				}
				if (firingEvents)
				{
					FirePointReachedEvent(0);
				}
				currentSectionIndex = ((!currentSpeedPositive) ? num : 0);
				if (checkDelay && !CheckDelayAtPoint(math, base.Cursor, 0, currentSpeedPositive))
				{
				}
			}
			else if (flag)
			{
				if (skipZeroPoint)
				{
					skipZeroPoint = false;
					return;
				}
				currentSectionIndex = 0;
				if (firingEvents)
				{
					FirePointReachedEvent(0);
				}
				if (checkDelay && !CheckDelayAtPoint(math, base.Cursor, 0, currentSpeedPositive))
				{
				}
			}
			else
			{
				if (base.Curve.Closed)
				{
					return;
				}
				if (currentSpeedPositive)
				{
					currentSectionIndex = 0;
					if (firingEvents)
					{
						FirePointReachedEvent(0);
					}
					if (checkDelay && !CheckDelayAtPoint(math, base.Cursor, 0, currentSpeedPositive))
					{
					}
				}
				else
				{
					currentSectionIndex = num - 1;
				}
			}
		}

		private bool CheckIfDelayIsOver(BGCurveBaseMath math, BGCcCursor cursor)
		{
			int num = base.Curve.PointsCount - 1;
			if (adjustByTotalLength)
			{
				oldLength = math.GetDistance();
			}
			cursor.Distance = ((base.Curve.Closed || currentSectionIndex != num) ? math[currentSectionIndex].DistanceFromStartToOrigin : math.GetDistance());
			float delayAtPoint = GetDelayAtPoint(currentSectionIndex);
			if (!(Time.time - delayStarted > delayAtPoint))
			{
				return false;
			}
			float @float = speed;
			if (speedField != null)
			{
				@float = base.Curve[currentSectionIndex].GetFloat(speedField.FieldName);
			}
			delayStarted = -1f;
			if (speedWasPositiveWhileDelayed)
			{
				cursor.Distance += Mathf.Abs(@float * Time.deltaTime);
			}
			else if (currentSectionIndex > 0)
			{
				currentSectionIndex--;
				cursor.Distance -= Mathf.Abs(@float * Time.deltaTime);
			}
			else if (!skipZeroPoint)
			{
				currentSectionIndex = num;
				cursor.Distance = math.GetDistance() - Mathf.Abs(@float * Time.deltaTime);
			}
			return true;
		}

		private void FirePointReachedEvent(int pointIndex)
		{
			if (this.PointReached != null)
			{
				this.PointReached(this, PointReachedArgs.GetInstance(pointIndex));
			}
			if (pointReachedEvent.GetPersistentEventCount() > 0)
			{
				pointReachedEvent.Invoke(pointIndex);
			}
		}
	}
}
