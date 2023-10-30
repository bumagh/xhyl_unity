using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Angle Limit Modifier")]
	public class FK3_AngleLimitModifier : FK3_TriggerEventProModifier
	{
		public enum COMPARE_AXIS
		{
			X,
			Y,
			Z
		}

		public enum FILTER_TYPE
		{
			IgnoreTargetTracking,
			WaitToFireEvent
		}

		[SerializeField]
		protected Transform _origin;

		public float angle = 5f;

		public bool flatAngleCompare;

		public COMPARE_AXIS flatCompareAxis = COMPARE_AXIS.Z;

		public FILTER_TYPE filterType = FILTER_TYPE.WaitToFireEvent;

		public FK3_DEBUG_LEVELS debugLevel;

		protected FK3_EventFireController fireCtrl;

		protected FK3_TargetTracker tracker;

		protected FK3_Target currentTarget;

		protected List<FK3_Target> iterTargets = new List<FK3_Target>();

		public Transform origin
		{
			get
			{
				if (_origin == null)
				{
					return base.transform;
				}
				return _origin;
			}
			set
			{
				_origin = value;
			}
		}

		protected void Awake()
		{
			fireCtrl = GetComponent<FK3_EventFireController>();
			if (fireCtrl != null)
			{
				tracker = fireCtrl.targetTracker;
			}
			else
			{
				tracker = GetComponent<FK3_TargetTracker>();
			}
			if (fireCtrl == null && tracker == null)
			{
				throw new MissingComponentException("Must have at least a FK3_TargetTracker or FK3_EventFireController");
			}
			if (fireCtrl == null || tracker == null)
			{
				if (fireCtrl != null)
				{
					filterType = FILTER_TYPE.WaitToFireEvent;
				}
				else
				{
					filterType = FILTER_TYPE.IgnoreTargetTracking;
				}
			}
		}

		protected void OnEnable()
		{
			if (tracker != null)
			{
				tracker.AddOnPostSortDelegate(FilterTrackerTargetList);
			}
			if (fireCtrl != null)
			{
				fireCtrl.AddOnPreFireDelegate(FilterFireCtrlTargetList);
			}
		}

		protected void OnDisable()
		{
			if (tracker != null)
			{
				tracker.RemoveOnPostSortDelegate(FilterTrackerTargetList);
			}
			if (fireCtrl != null)
			{
				fireCtrl.RemoveOnPreFireDelegate(FilterFireCtrlTargetList);
			}
		}

		protected void FilterFireCtrlTargetList(FK3_TargetList targets)
		{
			if (filterType == FILTER_TYPE.WaitToFireEvent)
			{
				FilterTargetList(targets);
			}
		}

		protected void FilterTrackerTargetList(FK3_TargetTracker source, FK3_TargetList targets)
		{
			if (filterType == FILTER_TYPE.IgnoreTargetTracking)
			{
				FilterTargetList(targets);
			}
		}

		protected void FilterTargetList(FK3_TargetList targets)
		{
			iterTargets.Clear();
			iterTargets.AddRange(targets);
			for (int i = 0; i < iterTargets.Count; i++)
			{
				currentTarget = iterTargets[i];
				if (!IsInAngle(currentTarget))
				{
					targets.Remove(currentTarget);
				}
			}
		}

		protected bool IsInAngle(FK3_Target target)
		{
			Vector3 from = target.transform.position - origin.position;
			Vector3 forward = origin.forward;
			if (flatAngleCompare)
			{
				switch (flatCompareAxis)
				{
				case COMPARE_AXIS.Z:
					from.z = (forward.z = 0f);
					break;
				case COMPARE_AXIS.Y:
					from.y = (forward.y = 0f);
					break;
				case COMPARE_AXIS.X:
					from.x = (forward.x = 0f);
					break;
				}
			}
			return Vector3.Angle(from, forward) < angle;
		}
	}
}
