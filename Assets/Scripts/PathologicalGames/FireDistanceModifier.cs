using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(EventFireController))]
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Fire Distance")]
	public class FireDistanceModifier : TriggerEventProModifier
	{
		public enum ORIGIN_MODE
		{
			TargetTracker,
			TargetTrackerArea,
			FireController,
			FireControllerEmitter
		}

		public ORIGIN_MODE originMode = ORIGIN_MODE.FireController;

		public float minDistance;

		public float maxDistance = 1f;

		public DEBUG_LEVELS debugLevel;

		public EventFireController fireCtrl;

		protected Target currentTarget;

		protected List<Target> iterTargets = new List<Target>();

		public Transform origin
		{
			get
			{
				Transform result = base.transform;
				switch (originMode)
				{
				case ORIGIN_MODE.TargetTracker:
					result = fireCtrl.targetTracker.transform;
					break;
				case ORIGIN_MODE.TargetTrackerArea:
				{
					Area area = fireCtrl.targetTracker.area;
					if (area == null)
					{
						throw new MissingReferenceException($"FireController {fireCtrl.name}'s TargetTracker doesn't have an AreaIf by design, such as a CollisionTargetTracker, use the 'TargetTracker' or other Origin Mode option.");
					}
					result = area.transform;
					break;
				}
				case ORIGIN_MODE.FireController:
				{
					Transform transform2 = origin = base.transform;
					result = transform2;
					break;
				}
				case ORIGIN_MODE.FireControllerEmitter:
					if (fireCtrl.spawnEventTriggerAtTransform == null)
					{
						throw new MissingReferenceException($"FireController {fireCtrl.name} doesn't have an emitter set. Add one or use the 'Fire Controller' Origin Mode option.");
					}
					result = fireCtrl.spawnEventTriggerAtTransform;
					break;
				}
				return result;
			}
			set
			{
			}
		}

		protected void Awake()
		{
			fireCtrl = GetComponent<EventFireController>();
		}

		protected void OnEnable()
		{
			fireCtrl.AddOnPreFireDelegate(FilterFireTargetList);
		}

		protected void OnDisable()
		{
			if (fireCtrl != null)
			{
				fireCtrl.RemoveOnPreFireDelegate(FilterFireTargetList);
			}
		}

		protected void FilterFireTargetList(TargetList targets)
		{
			Vector3 position = origin.position;
			iterTargets.Clear();
			iterTargets.AddRange(targets);
			for (int i = 0; i < iterTargets.Count; i++)
			{
				currentTarget = iterTargets[i];
				float distToPos = currentTarget.targetable.GetDistToPos(position);
				if (distToPos <= minDistance || distToPos >= maxDistance)
				{
					targets.Remove(currentTarget);
				}
			}
		}
	}
}
