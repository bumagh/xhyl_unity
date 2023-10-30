using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Fire Distance")]
	[RequireComponent(typeof(FK3_EventFireController))]
	public class FK3_FireDistanceModifier : FK3_TriggerEventProModifier
	{
		public enum ORIGIN_MODE
		{
			FK3_TargetTracker,
			TargetTrackerArea,
			FireController,
			FireControllerEmitter
		}

		public ORIGIN_MODE originMode = ORIGIN_MODE.FireController;

		public float minDistance;

		public float maxDistance = 1f;

		public FK3_DEBUG_LEVELS debugLevel;

		public FK3_EventFireController fireCtrl;

		protected FK3_Target currentTarget;

		protected List<FK3_Target> iterTargets = new List<FK3_Target>();

		public Transform origin
		{
			get
			{
				Transform result = base.transform;
				switch (originMode)
				{
				case ORIGIN_MODE.FK3_TargetTracker:
					result = fireCtrl.targetTracker.transform;
					break;
				case ORIGIN_MODE.TargetTrackerArea:
				{
					FK3_Area area = fireCtrl.targetTracker.area;
					if (area == null)
					{
						throw new MissingReferenceException($"FireController {fireCtrl.name}'s FK3_TargetTracker doesn't have an AreaIf by design, such as a FK3_CollisionTargetTracker, use the 'FK3_TargetTracker' or other Origin Mode option.");
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
			fireCtrl = GetComponent<FK3_EventFireController>();
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

		protected void FilterFireTargetList(FK3_TargetList targets)
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
