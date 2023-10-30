using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(FK3_EventFireController))]
	public class FK3_WaitForAlignmentModifier : FK3_TriggerEventProModifier
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

		public float angleTolerance = 5f;

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
			UnityEngine.Debug.LogWarning($"WaitForAlignementModifier on GameObject {base.name} has been deprecated. Replace the component with FK3_AngleLimitModifier (with the filterType set to 'Wait to Fire Event'). You can do this without losing your other settings by switching the Inspector tab to 'Debug' and changing the script field.");
			fireCtrl = GetComponent<FK3_EventFireController>();
			filterType = FILTER_TYPE.WaitToFireEvent;
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
			return Vector3.Angle(from, forward) < angleTolerance;
		}
	}
}
