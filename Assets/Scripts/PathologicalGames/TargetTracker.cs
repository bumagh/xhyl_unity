using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	public abstract class TargetTracker : MonoBehaviour, ITargetTracker
	{
		public enum SORTING_STYLES
		{
			None,
			Nearest,
			Farthest,
			NearestToDestination,
			FarthestFromDestination,
			MostPowerful,
			LeastPowerful
		}

		public interface iTargetComparer : IComparer<Target>
		{
			new int Compare(Target targetA, Target targetB);
		}

		public class TargetComparer : iTargetComparer, IComparer<Target>
		{
			protected Transform xform;

			protected SORTING_STYLES sortStyle;

			public TargetComparer(SORTING_STYLES sortStyle, Transform xform)
			{
				this.xform = xform;
				this.sortStyle = sortStyle;
			}

			public int Compare(Target targetA, Target targetB)
			{
				switch (sortStyle)
				{
				case SORTING_STYLES.None:
					throw new NotImplementedException("Unexpected option. SORT_OPTIONS.NONE should bypass sorting altogether.");
				case SORTING_STYLES.Nearest:
				{
					float sqrDistToPos2 = targetA.targetable.GetSqrDistToPos(xform.position);
					float sqrDistToPos3 = targetB.targetable.GetSqrDistToPos(xform.position);
					return sqrDistToPos2.CompareTo(sqrDistToPos3);
				}
				case SORTING_STYLES.Farthest:
				{
					float sqrDistToPos = targetA.targetable.GetSqrDistToPos(xform.position);
					return targetB.targetable.GetSqrDistToPos(xform.position).CompareTo(sqrDistToPos);
				}
				case SORTING_STYLES.NearestToDestination:
					return targetA.targetable.distToDest.CompareTo(targetB.targetable.distToDest);
				case SORTING_STYLES.FarthestFromDestination:
					return targetB.targetable.distToDest.CompareTo(targetA.targetable.distToDest);
				case SORTING_STYLES.MostPowerful:
					return targetB.targetable.strength.CompareTo(targetA.targetable.strength);
				case SORTING_STYLES.LeastPowerful:
					return targetA.targetable.strength.CompareTo(targetB.targetable.strength);
				default:
					throw new NotImplementedException($"Unexpected option '{sortStyle}'.");
				}
			}
		}

		public int numberOfTargets = 1;

		public LayerMask targetLayers;

		[SerializeField]
		protected SORTING_STYLES _sortingStyle = SORTING_STYLES.Nearest;

		public float updateInterval = 0.1f;

		protected bool batchDirtyRunning;

		protected TargetList _targets = new TargetList();

		protected bool isUpdateTargetsUpdateRunning;

		protected TargetList _unfilteredTargets = new TargetList();

		public DEBUG_LEVELS debugLevel;

		protected internal OnPostSortDelegate onPostSortDelegates;

		protected internal OnTargetsChangedDelegate onTargetsChangedDelegates;

		protected internal OnNewDetectedDelegate onNewDetectedDelegates;

		public SORTING_STYLES sortingStyle
		{
			get
			{
				return _sortingStyle;
			}
			set
			{
				if (_sortingStyle != value)
				{
					_sortingStyle = value;
					dirty = true;
				}
			}
		}

		public virtual bool dirty
		{
			get
			{
				return false;
			}
			set
			{
				if (value)
				{
					targets = new TargetList(_unfilteredTargets);
				}
			}
		}

		public bool batchDirty
		{
			get
			{
				return batchDirtyRunning;
			}
			set
			{
				if (!batchDirtyRunning)
				{
					StartCoroutine(RunBatchDirty());
				}
			}
		}

		public virtual TargetList targets
		{
			get
			{
				return _targets;
			}
			set
			{
				_unfilteredTargets.Clear();
				_targets.Clear();
				if (numberOfTargets == 0 || value.Count == 0)
				{
					if (onTargetsChangedDelegates != null)
					{
						onTargetsChangedDelegates(this);
					}
					return;
				}
				_unfilteredTargets.AddRange(value);
				_targets.AddRange(_unfilteredTargets);
				if (sortingStyle != 0)
				{
					if (_unfilteredTargets.Count > 1)
					{
						TargetComparer comparer = new TargetComparer(sortingStyle, base.transform);
						_targets.Sort(comparer);
					}
					if (!isUpdateTargetsUpdateRunning && sortingStyle != 0)
					{
						StartCoroutine(UpdateTargets());
					}
				}
				if (onPostSortDelegates != null)
				{
					onPostSortDelegates(this, _targets);
				}
				if (numberOfTargets > -1)
				{
					int count = _targets.Count;
					int num = Mathf.Clamp(numberOfTargets, 0, count);
					_targets.RemoveRange(num, count - num);
				}
				if (onTargetsChangedDelegates != null)
				{
					onTargetsChangedDelegates(this);
				}
			}
		}

		public virtual Area area
		{
			get
			{
				return null;
			}
			protected set
			{
			}
		}

		protected IEnumerator RunBatchDirty()
		{
			batchDirtyRunning = true;
			yield return new WaitForEndOfFrame();
			batchDirtyRunning = false;
			dirty = true;
		}

		protected IEnumerator UpdateTargets()
		{
			isUpdateTargetsUpdateRunning = true;
			while (_unfilteredTargets.Count > 0)
			{
				if (updateInterval == 0f)
				{
					yield return null;
				}
				else
				{
					yield return new WaitForSeconds(updateInterval);
				}
				dirty = true;
			}
			isUpdateTargetsUpdateRunning = false;
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
			dirty = true;
		}

		internal bool IsIgnoreTargetOnNewDetectedDelegatesExecute(Target target)
		{
			if (onNewDetectedDelegates == null)
			{
				return false;
			}
			Delegate[] invocationList = onNewDetectedDelegates.GetInvocationList();
			Delegate[] array = invocationList;
			for (int i = 0; i < array.Length; i++)
			{
				OnNewDetectedDelegate onNewDetectedDelegate = (OnNewDetectedDelegate)array[i];
				if (!onNewDetectedDelegate(this, target))
				{
					return true;
				}
			}
			return false;
		}

		public void AddOnPostSortDelegate(OnPostSortDelegate del)
		{
			onPostSortDelegates = (OnPostSortDelegate)Delegate.Remove(onPostSortDelegates, del);
			onPostSortDelegates = (OnPostSortDelegate)Delegate.Combine(onPostSortDelegates, del);
		}

		public void SetOnPostSortDelegate(OnPostSortDelegate del)
		{
			onPostSortDelegates = del;
		}

		public void RemoveOnPostSortDelegate(OnPostSortDelegate del)
		{
			onPostSortDelegates = (OnPostSortDelegate)Delegate.Remove(onPostSortDelegates, del);
		}

		public void AddOnTargetsChangedDelegate(OnTargetsChangedDelegate del)
		{
			onTargetsChangedDelegates = (OnTargetsChangedDelegate)Delegate.Remove(onTargetsChangedDelegates, del);
			onTargetsChangedDelegates = (OnTargetsChangedDelegate)Delegate.Combine(onTargetsChangedDelegates, del);
		}

		public void SetOnTargetsChangedDelegate(OnTargetsChangedDelegate del)
		{
			onTargetsChangedDelegates = del;
		}

		public void RemoveOnTargetsChangedDelegate(OnTargetsChangedDelegate del)
		{
			onTargetsChangedDelegates = (OnTargetsChangedDelegate)Delegate.Remove(onTargetsChangedDelegates, del);
		}

		public void AddOnNewDetectedDelegate(OnNewDetectedDelegate del)
		{
			onNewDetectedDelegates = (OnNewDetectedDelegate)Delegate.Remove(onNewDetectedDelegates, del);
			onNewDetectedDelegates = (OnNewDetectedDelegate)Delegate.Combine(onNewDetectedDelegates, del);
		}

		public void SetOnNewDetectedDelegate(OnNewDetectedDelegate del)
		{
			onNewDetectedDelegates = del;
		}

		public void RemoveOnNewDetectedDelegate(OnNewDetectedDelegate del)
		{
			onNewDetectedDelegates = (OnNewDetectedDelegate)Delegate.Remove(onNewDetectedDelegates, del);
		}
	}
}
