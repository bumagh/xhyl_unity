using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Ignore Tags")]
	[RequireComponent(typeof(TargetTracker))]
	public class IgnoreTagModifier : TriggerEventProModifier
	{
		public List<string> ignoreList = new List<string>();

		public DEBUG_LEVELS debugLevel;

		protected TargetTracker tracker;

		protected string currentTag;

		protected void Awake()
		{
			tracker = GetComponent<TargetTracker>();
		}

		protected void OnEnable()
		{
			tracker.AddOnNewDetectedDelegate(OnNewDetected);
		}

		protected void OnDisable()
		{
			if (tracker != null)
			{
				tracker.RemoveOnNewDetectedDelegate(OnNewDetected);
			}
		}

		protected bool OnNewDetected(TargetTracker targetTracker, Target target)
		{
			for (int i = 0; i < ignoreList.Count; i++)
			{
				currentTag = ignoreList[i];
				if (target.gameObject.tag == currentTag)
				{
					return false;
				}
			}
			return true;
		}
	}
}
