using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[RequireComponent(typeof(TargetTracker))]
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Ignore Targetables")]
	public class IgnoreModifier : MonoBehaviour
	{
		public List<Targetable> _ignoreList = new List<Targetable>();

		public DEBUG_LEVELS debugLevel;

		protected TargetTracker tracker;

		protected Targetable currentTargetable;

		protected void Awake()
		{
			tracker = GetComponent<TargetTracker>();
		}

		protected void OnEnable()
		{
			tracker.AddOnNewDetectedDelegate(OnNewDetected);
			if (_ignoreList.Count == 0 || tracker.area == null)
			{
				return;
			}
			TargetList targetList = new TargetList(tracker.area);
			for (int i = 0; i < targetList.Count; i++)
			{
				Target target = targetList[i];
				currentTargetable = target.targetable;
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
				foreach (Targetable ignore in _ignoreList)
				{
					if (ignore != null && tracker.area != null && tracker.area.IsInRange(ignore))
					{
						tracker.area.Add(ignore);
					}
				}
			}
		}

		protected bool OnNewDetected(TargetTracker targetTracker, Target target)
		{
			return !_ignoreList.Contains(target.targetable);
		}

		public void Add(Targetable targetable)
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

		public void Remove(Targetable targetable)
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
			foreach (Targetable ignore in _ignoreList)
			{
				Remove(ignore);
			}
		}
	}
}
