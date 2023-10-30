using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Modifier - Ignore Tags")]
	[RequireComponent(typeof(FK3_TargetTracker))]
	public class FK3_IgnoreTagModifier : FK3_TriggerEventProModifier
	{
		public List<string> ignoreList = new List<string>();

		public FK3_DEBUG_LEVELS debugLevel;

		protected FK3_TargetTracker tracker;

		protected string currentTag;

		protected void Awake()
		{
			tracker = GetComponent<FK3_TargetTracker>();
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

		protected bool OnNewDetected(FK3_TargetTracker targetTracker, FK3_Target target)
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
