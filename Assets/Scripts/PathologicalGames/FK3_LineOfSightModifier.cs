using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Line of Sight")]
	public class FK3_LineOfSightModifier : FK3_TriggerEventProModifier
	{
		public enum TEST_MODE
		{
			SinglePoint,
			BoundingBox
		}

		public LayerMask targetTrackerLayerMask;

		public LayerMask fireControllerLayerMask;

		public TEST_MODE testMode;

		public float radius = 1f;

		public FK3_DEBUG_LEVELS debugLevel;

		public FK3_TargetTracker tracker;

		public FK3_EventFireController fireCtrl;

		protected void Awake()
		{
			tracker = GetComponent<FK3_TargetTracker>();
			fireCtrl = GetComponent<FK3_EventFireController>();
		}

		protected void OnEnable()
		{
			if (tracker != null)
			{
				tracker.AddOnPostSortDelegate(FilterTrackerTargetList);
			}
			if (fireCtrl != null)
			{
				fireCtrl.AddOnPreFireDelegate(FilterFireTargetList);
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
				fireCtrl.RemoveOnPreFireDelegate(FilterFireTargetList);
			}
		}

		protected void FilterTrackerTargetList(FK3_TargetTracker source, FK3_TargetList targets)
		{
			if (targetTrackerLayerMask.value != 0)
			{
				Vector3 fromPos = (!(tracker.area != null)) ? tracker.transform.position : tracker.area.transform.position;
				LayerMask mask = targetTrackerLayerMask;
				FilterTargetList(targets, mask, fromPos, Color.red);
			}
		}

		protected void FilterFireTargetList(FK3_TargetList targets)
		{
			if (fireControllerLayerMask.value != 0)
			{
				Vector3 fromPos = (!(fireCtrl.spawnEventTriggerAtTransform != null)) ? fireCtrl.transform.position : fireCtrl.spawnEventTriggerAtTransform.position;
				LayerMask mask = fireControllerLayerMask;
				FilterTargetList(targets, mask, fromPos, Color.yellow);
			}
		}

		protected void FilterTargetList(FK3_TargetList targets, LayerMask mask, Vector3 fromPos, Color debugLineColor)
		{
			List<FK3_Target> list = new List<FK3_Target>(targets);
			foreach (FK3_Target item in list)
			{
				FK3_Target current = item;
				bool flag = false;
				if (testMode == TEST_MODE.BoundingBox)
				{
					Collider coll = current.targetable.coll;
					Matrix4x4 localToWorldMatrix = current.targetable.transform.localToWorldMatrix;
					Vector3 vector = coll.bounds.extents * 0.5f;
					Vector3[] array = new Vector3[8]
					{
						localToWorldMatrix.MultiplyPoint3x4(vector),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f - vector.x, vector.y, vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, vector.y, 0f - vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f - vector.x, vector.y, 0f - vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, 0f - vector.y, vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(0f - vector.x, 0f - vector.y, vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(new Vector3(vector.x, 0f - vector.y, 0f - vector.z)),
						localToWorldMatrix.MultiplyPoint3x4(-vector)
					};
					for (int i = 0; i < array.Length; i++)
					{
						flag = Physics.Linecast(fromPos, array[i], mask);
						if (!flag)
						{
							break;
						}
					}
				}
				else
				{
					Vector3 position = current.targetable.transform.position;
					flag = Physics.Linecast(fromPos, position, mask);
				}
				if (flag)
				{
					targets.Remove(current);
				}
			}
		}
	}
}
