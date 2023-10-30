using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(FK3_TargetTracker))]
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Ignore Targetables")]
	public class FK3_IgnoreModifier : MonoBehaviour
	{
		public List<FK3_Targetable> _ignoreList = new List<FK3_Targetable>();

		public FK3_DEBUG_LEVELS debugLevel;

		protected FK3_TargetTracker tracker;

		protected FK3_Targetable currentTargetable;

		protected void Awake()
		{
			tracker = GetComponent<FK3_TargetTracker>();
		}

		protected void OnEnable()
		{
			tracker.AddOnNewDetectedDelegate(OnNewDetected);
			if (_ignoreList.Count == 0 || tracker.area == null)
			{
				return;
			}
			FK3_TargetList fK3_TargetList = new FK3_TargetList(tracker.area);
			for (int i = 0; i < fK3_TargetList.Count; i++)
			{
				FK3_Target fK3_Target = fK3_TargetList[i];
				currentTargetable = fK3_Target.targetable;
				if (!(currentTargetable == null) && _ignoreList.Contains(currentTargetable))
				{
					tracker.area.Remove(currentTargetable);
				}
			}
		}

		protected void OnDisable()
		{
			if (tracker != null)
			{
				tracker.RemoveOnNewDetectedDelegate(OnNewDetected);
			}
			if (!(tracker.area == null))
			{
				foreach (FK3_Targetable ignore in _ignoreList)
				{
					if (ignore != null && tracker.area != null && tracker.area.IsInRange(ignore))
					{
						tracker.area.Add(ignore);
					}
				}
			}
		}

		protected bool OnNewDetected(FK3_TargetTracker targetTracker, FK3_Target target)
		{
			return !_ignoreList.Contains(target.targetable);
		}

		public void Add(FK3_Targetable targetable)
		{
			if (!(targetable == null))
			{
				if (!_ignoreList.Contains(targetable))
				{
					_ignoreList.Add(targetable);
				}
				if (base.enabled && tracker != null && tracker.area != null && targetable.trackers.Contains(tracker))
				{
					tracker.area.Remove(targetable);
				}
			}
		}

		public void Remove(FK3_Targetable targetable)
		{
			if (!(targetable == null))
			{
				_ignoreList.Remove(targetable);
				if (base.enabled && tracker.area != null)
				{
					tracker.area.Add(targetable);
				}
			}
		}

		public void Clear()
		{
			foreach (FK3_Targetable ignore in _ignoreList)
			{
				Remove(ignore);
			}
		}
	}
}
