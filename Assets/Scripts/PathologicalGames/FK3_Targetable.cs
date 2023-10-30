using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO FK3_Targetable")]
	public class FK3_Targetable : MonoBehaviour
	{
		public delegate void OnDetectedDelegate(FK3_TargetTracker source);

		public delegate void OnNotDetectedDelegate(FK3_TargetTracker source);

		public delegate void OnHitDelegate(FK3_EventInfoList eventInfoList, FK3_Target target);

		public bool _isTargetable = true;

		public FK3_DEBUG_LEVELS debugLevel;

		internal List<FK3_TargetTracker> trackers = new List<FK3_TargetTracker>();

		public GameObject go;

		public Collider coll;

		public Collider2D coll2D;

		protected OnDetectedDelegate onDetectedDelegates;

		protected OnNotDetectedDelegate onNotDetectedDelegates;

		protected OnHitDelegate onHitDelegates;

		[HideInInspector]
		public List<Vector3> waypoints = new List<Vector3>();

		public bool isTargetable
		{
			get
			{
				return go.activeInHierarchy && base.enabled && _isTargetable;
			}
			set
			{
				if (_isTargetable == value)
				{
					return;
				}
				_isTargetable = value;
				if (go.activeInHierarchy && base.enabled)
				{
					if (!_isTargetable)
					{
						CleanUp();
					}
					else
					{
						BecomeTargetable();
					}
				}
			}
		}

		public float strength
		{
			get;
			set;
		}

		public float distToDest
		{
			get
			{
				if (waypoints.Count == 0)
				{
					return 0f;
				}
				float num = GetSqrDistToPos(waypoints[0]);
				for (int i = 0; i < waypoints.Count - 2; i++)
				{
					num += (waypoints[i] - waypoints[i + 1]).sqrMagnitude;
				}
				return num;
			}
		}

		protected void Awake()
		{
			go = base.gameObject;
			coll = GetComponent<Collider>();
			coll2D = GetComponent<Collider2D>();
		}

		protected void OnDisable()
		{
			CleanUp();
		}

		protected void OnDestroy()
		{
			CleanUp();
		}

		protected void CleanUp()
		{
			if (Application.isPlaying)
			{
				List<FK3_TargetTracker> list = new List<FK3_TargetTracker>(trackers);
				foreach (FK3_TargetTracker item in list)
				{
					if (!(item == null) && !(item.area == null) && item.area.Count != 0)
					{
						item.area.Remove(this);
					}
				}
				trackers.Clear();
			}
		}

		protected void BecomeTargetable()
		{
			if (coll != null && coll.enabled)
			{
				coll.enabled = false;
				coll.enabled = true;
			}
			else if (coll2D != null && coll2D.enabled)
			{
				coll2D.enabled = false;
				coll2D.enabled = true;
			}
		}

		public void OnHit(FK3_EventInfoList eventInfoList, FK3_Target target)
		{
			eventInfoList = eventInfoList.CopyWithHitTime();
			if (onHitDelegates != null)
			{
				onHitDelegates(eventInfoList, target);
			}
		}

		internal void OnDetected(FK3_TargetTracker source)
		{
			trackers.Add(source);
			if (onDetectedDelegates != null)
			{
				onDetectedDelegates(source);
			}
		}

		internal void OnNotDetected(FK3_TargetTracker source)
		{
			trackers.Remove(source);
			if (onNotDetectedDelegates != null)
			{
				onNotDetectedDelegates(source);
			}
		}

		public float GetSqrDistToPos(Vector3 other)
		{
			return (base.transform.position - other).sqrMagnitude;
		}

		public float GetDistToPos(Vector3 other)
		{
			return (base.transform.position - other).magnitude;
		}

		public void AddOnDetectedDelegate(OnDetectedDelegate del)
		{
			onDetectedDelegates = (OnDetectedDelegate)Delegate.Remove(onDetectedDelegates, del);
			onDetectedDelegates = (OnDetectedDelegate)Delegate.Combine(onDetectedDelegates, del);
		}

		public void SetOnDetectedDelegate(OnDetectedDelegate del)
		{
			onDetectedDelegates = del;
		}

		public void RemoveOnDetectedDelegate(OnDetectedDelegate del)
		{
			onDetectedDelegates = (OnDetectedDelegate)Delegate.Remove(onDetectedDelegates, del);
		}

		public void AddOnNotDetectedDelegate(OnNotDetectedDelegate del)
		{
			onNotDetectedDelegates = (OnNotDetectedDelegate)Delegate.Remove(onNotDetectedDelegates, del);
			onNotDetectedDelegates = (OnNotDetectedDelegate)Delegate.Combine(onNotDetectedDelegates, del);
		}

		public void SetOnNotDetectedDelegate(OnNotDetectedDelegate del)
		{
			onNotDetectedDelegates = del;
		}

		public void RemoveOnNotDetectedDelegate(OnNotDetectedDelegate del)
		{
			onNotDetectedDelegates = (OnNotDetectedDelegate)Delegate.Remove(onNotDetectedDelegates, del);
		}

		public void AddOnHitDelegate(OnHitDelegate del)
		{
			onHitDelegates = (OnHitDelegate)Delegate.Remove(onHitDelegates, del);
			onHitDelegates = (OnHitDelegate)Delegate.Combine(onHitDelegates, del);
		}

		public void SetOnHitDelegate(OnHitDelegate del)
		{
			onHitDelegates = del;
		}

		public void RemoveOnHitDelegate(OnHitDelegate del)
		{
			onHitDelegates = (OnHitDelegate)Delegate.Remove(onHitDelegates, del);
		}
	}
}
