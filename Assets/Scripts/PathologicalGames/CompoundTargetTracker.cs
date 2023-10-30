using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Compound TargetTracker")]
	public class CompoundTargetTracker : TargetTracker
	{
		public List<TargetTracker> targetTrackers = new List<TargetTracker>();

		protected TargetList combinedTargets = new TargetList();

		public override TargetList targets
		{
			get
			{
				return base.targets;
			}
			set
			{
				combinedTargets.Clear();
				for (int i = 0; i < targetTrackers.Count; i++)
				{
					combinedTargets.AddRange(targetTrackers[i].targets);
				}
				base.targets = combinedTargets;
			}
		}

		public override bool dirty
		{
			get
			{
				return false;
			}
			set
			{
				for (int i = 0; i < targetTrackers.Count; i++)
				{
					targetTrackers[i].AddOnTargetsChangedDelegate(OnTargetsChanged);
					targetTrackers[i].AddOnNewDetectedDelegate(OnTrackersNew);
				}
				base.dirty = value;
			}
		}

		public new LayerMask targetLayers
		{
			get
			{
				string message = "CompoundTargetTrackers do not support targetLayers because they delegate detection to their list of TargetTrackers";
				throw new NotImplementedException(message);
			}
			set
			{
				string message = "CompoundTargetTrackers do not support targetLayers because they delegate detection to their list of TargetTrackers";
				throw new NotImplementedException(message);
			}
		}

		protected override void OnEnable()
		{
			dirty = true;
		}

		protected void OnDisable()
		{
			for (int i = 0; i < targetTrackers.Count; i++)
			{
				targetTrackers[i].RemoveOnTargetsChangedDelegate(OnTargetsChanged);
				targetTrackers[i].RemoveOnNewDetectedDelegate(OnTrackersNew);
			}
		}

		protected void OnTargetsChanged(TargetTracker source)
		{
			base.batchDirty = true;
		}

		protected bool OnTrackersNew(TargetTracker source, Target target)
		{
			return onNewDetectedDelegates == null || onNewDetectedDelegates(this, target);
		}
	}
}
