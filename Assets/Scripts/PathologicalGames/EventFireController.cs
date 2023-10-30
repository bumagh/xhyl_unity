using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO EventFireController")]
	public class EventFireController : MonoBehaviour
	{
		public enum NOTIFY_TARGET_OPTIONS
		{
			Off,
			Direct,
			PassInfoToEventTrigger,
			UseEventTriggerInfo
		}

		public delegate void OnStartDelegate();

		public delegate void OnUpdateDelegate();

		public delegate void OnTargetUpdateDelegate(TargetList targets);

		public delegate void OnIdleUpdateDelegate();

		public delegate void OnStopDelegate();

		public delegate void OnPreFireDelegate(TargetList targets);

		public delegate void OnFireDelegate(TargetList targets);

		public delegate void OnEventTriggerSpawnedDelegate(EventTrigger eventTrigger);

		public float interval;

		public bool initIntervalCountdownAtZero = true;

		public NOTIFY_TARGET_OPTIONS notifyTargets = NOTIFY_TARGET_OPTIONS.Direct;

		public EventTrigger eventTriggerPrefab;

		public bool usePooling = true;

		public string eventTriggerPoolName = "EventTriggers";

		public bool overridePoolName;

		public List<EventInfoListGUIBacker> _eventInfoList = new List<EventInfoListGUIBacker>();

		[SerializeField]
		protected Transform _spawnEventTriggerAtTransform;

		public DEBUG_LEVELS debugLevel;

		public float fireIntervalCounter = 99999f;

		public TargetTracker targetTracker;

		public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

		protected TargetList targets = new TargetList();

		protected TargetList targetsCopy = new TargetList();

		protected OnStartDelegate onStartDelegates;

		protected OnUpdateDelegate onUpdateDelegates;

		protected OnTargetUpdateDelegate onTargetUpdateDelegates;

		protected OnIdleUpdateDelegate onIdleUpdateDelegates;

		protected OnStopDelegate onStopDelegates;

		protected OnPreFireDelegate onPreFireDelegates;

		protected OnFireDelegate onFireDelegates;

		protected OnEventTriggerSpawnedDelegate onEventTriggerSpawnedDelegates;

		protected bool keepFiring;

		public EventInfoList eventInfoList
		{
			get
			{
				EventInfoList eventInfoList = new EventInfoList();
				foreach (EventInfoListGUIBacker eventInfo in _eventInfoList)
				{
					eventInfoList.Add(new EventInfo
					{
						name = eventInfo.name,
						value = eventInfo.value,
						duration = eventInfo.duration
					});
				}
				return eventInfoList;
			}
			set
			{
				_eventInfoList.Clear();
				foreach (EventInfo item2 in value)
				{
					EventInfoListGUIBacker item = new EventInfoListGUIBacker(item2);
					_eventInfoList.Add(item);
				}
			}
		}

		public Transform spawnEventTriggerAtTransform
		{
			get
			{
				if (_spawnEventTriggerAtTransform == null)
				{
					return base.transform;
				}
				return _spawnEventTriggerAtTransform;
			}
			set
			{
				_spawnEventTriggerAtTransform = value;
			}
		}

		protected void OnEnable()
		{
			StartCoroutine(FiringSystem());
		}

		protected void OnDisable()
		{
			OnStop();
		}

		protected void OnStart()
		{
			if (onStartDelegates != null)
			{
				onStartDelegates();
			}
		}

		protected void OnUpdate()
		{
			if (onUpdateDelegates != null)
			{
				onUpdateDelegates();
			}
		}

		protected void OnTargetUpdate(TargetList targets)
		{
			if (onTargetUpdateDelegates != null)
			{
				onTargetUpdateDelegates(targets);
			}
		}

		protected void OnIdleUpdate()
		{
			if (onIdleUpdateDelegates != null)
			{
				onIdleUpdateDelegates();
			}
		}

		protected void OnStop()
		{
			if (onStopDelegates != null)
			{
				onStopDelegates();
			}
			keepFiring = false;
		}

		protected void Fire()
		{
			TargetList targetList = new TargetList();
			foreach (Target target in targets)
			{
				targetList.Add(new Target(target)
				{
					fireController = this
				});
			}
			targets = targetList;
			foreach (Target target2 in targets)
			{
				Target current2 = target2;
				switch (notifyTargets)
				{
				case NOTIFY_TARGET_OPTIONS.UseEventTriggerInfo:
					SpawnEventTrigger(current2, passInfo: false);
					break;
				case NOTIFY_TARGET_OPTIONS.PassInfoToEventTrigger:
					SpawnEventTrigger(current2, passInfo: true);
					break;
				case NOTIFY_TARGET_OPTIONS.Direct:
					current2.targetable.OnHit(eventInfoList, current2);
					break;
				}
			}
			if (onFireDelegates != null)
			{
				onFireDelegates(targets);
			}
		}

		protected void SpawnEventTrigger(Target target, bool passInfo)
		{
			if (!(eventTriggerPrefab == null))
			{
				string poolName = (!usePooling) ? string.Empty : (overridePoolName ? eventTriggerPoolName : eventTriggerPrefab.poolName);
				Transform transform = InstanceManager.Spawn(poolName, eventTriggerPrefab.transform, spawnEventTriggerAtTransform.position, spawnEventTriggerAtTransform.rotation);
				EventTrigger component = transform.GetComponent<EventTrigger>();
				component.fireController = this;
				component.target = target;
				component.poolName = poolName;
				if (passInfo)
				{
					component.eventInfoList = eventInfoList;
				}
				if (onEventTriggerSpawnedDelegates != null)
				{
					onEventTriggerSpawnedDelegates(component);
				}
			}
		}

		public void AddOnStartDelegate(OnStartDelegate del)
		{
			onStartDelegates = (OnStartDelegate)Delegate.Remove(onStartDelegates, del);
			onStartDelegates = (OnStartDelegate)Delegate.Combine(onStartDelegates, del);
		}

		public void SetOnStartDelegate(OnStartDelegate del)
		{
			onStartDelegates = del;
		}

		public void RemoveOnStartDelegate(OnStartDelegate del)
		{
			onStartDelegates = (OnStartDelegate)Delegate.Remove(onStartDelegates, del);
		}

		public void AddOnUpdateDelegate(OnUpdateDelegate del)
		{
			onUpdateDelegates = (OnUpdateDelegate)Delegate.Remove(onUpdateDelegates, del);
			onUpdateDelegates = (OnUpdateDelegate)Delegate.Combine(onUpdateDelegates, del);
		}

		public void SetOnUpdateDelegate(OnUpdateDelegate del)
		{
			onUpdateDelegates = del;
		}

		public void RemoveOnUpdateDelegate(OnUpdateDelegate del)
		{
			onUpdateDelegates = (OnUpdateDelegate)Delegate.Remove(onUpdateDelegates, del);
		}

		public void AddOnTargetUpdateDelegate(OnTargetUpdateDelegate del)
		{
			onTargetUpdateDelegates = (OnTargetUpdateDelegate)Delegate.Remove(onTargetUpdateDelegates, del);
			onTargetUpdateDelegates = (OnTargetUpdateDelegate)Delegate.Combine(onTargetUpdateDelegates, del);
		}

		public void SetOnTargetUpdateDelegate(OnTargetUpdateDelegate del)
		{
			onTargetUpdateDelegates = del;
		}

		public void RemoveOnTargetUpdateDelegate(OnTargetUpdateDelegate del)
		{
			onTargetUpdateDelegates = (OnTargetUpdateDelegate)Delegate.Remove(onTargetUpdateDelegates, del);
		}

		public void AddOnIdleUpdateDelegate(OnIdleUpdateDelegate del)
		{
			onIdleUpdateDelegates = (OnIdleUpdateDelegate)Delegate.Remove(onIdleUpdateDelegates, del);
			onIdleUpdateDelegates = (OnIdleUpdateDelegate)Delegate.Combine(onIdleUpdateDelegates, del);
		}

		public void SetOnIdleUpdateDelegate(OnIdleUpdateDelegate del)
		{
			onIdleUpdateDelegates = del;
		}

		public void RemoveOnIdleUpdateDelegate(OnIdleUpdateDelegate del)
		{
			onIdleUpdateDelegates = (OnIdleUpdateDelegate)Delegate.Remove(onIdleUpdateDelegates, del);
		}

		public void AddOnStopDelegate(OnStopDelegate del)
		{
			onStopDelegates = (OnStopDelegate)Delegate.Remove(onStopDelegates, del);
			onStopDelegates = (OnStopDelegate)Delegate.Combine(onStopDelegates, del);
		}

		public void SetOnStopDelegate(OnStopDelegate del)
		{
			onStopDelegates = del;
		}

		public void RemoveOnStopDelegate(OnStopDelegate del)
		{
			onStopDelegates = (OnStopDelegate)Delegate.Remove(onStopDelegates, del);
		}

		public void AddOnPreFireDelegate(OnPreFireDelegate del)
		{
			onPreFireDelegates = (OnPreFireDelegate)Delegate.Remove(onPreFireDelegates, del);
			onPreFireDelegates = (OnPreFireDelegate)Delegate.Combine(onPreFireDelegates, del);
		}

		public void SetOnPreFireDelegate(OnPreFireDelegate del)
		{
			onPreFireDelegates = del;
		}

		public void RemoveOnPreFireDelegate(OnPreFireDelegate del)
		{
			onPreFireDelegates = (OnPreFireDelegate)Delegate.Remove(onPreFireDelegates, del);
		}

		public void AddOnFireDelegate(OnFireDelegate del)
		{
			onFireDelegates = (OnFireDelegate)Delegate.Remove(onFireDelegates, del);
			onFireDelegates = (OnFireDelegate)Delegate.Combine(onFireDelegates, del);
		}

		public void SetOnFireDelegate(OnFireDelegate del)
		{
			onFireDelegates = del;
		}

		public void RemoveOnFireDelegate(OnFireDelegate del)
		{
			onFireDelegates = (OnFireDelegate)Delegate.Remove(onFireDelegates, del);
		}

		public void AddOnEventTriggerSpawnedDelegate(OnEventTriggerSpawnedDelegate del)
		{
			onEventTriggerSpawnedDelegates = (OnEventTriggerSpawnedDelegate)Delegate.Remove(onEventTriggerSpawnedDelegates, del);
			onEventTriggerSpawnedDelegates = (OnEventTriggerSpawnedDelegate)Delegate.Combine(onEventTriggerSpawnedDelegates, del);
		}

		public void SetOnEventTriggerSpawnedDelegate(OnEventTriggerSpawnedDelegate del)
		{
			onEventTriggerSpawnedDelegates = del;
		}

		public void RemoveOnEventTriggerSpawnedDelegate(OnEventTriggerSpawnedDelegate del)
		{
			onEventTriggerSpawnedDelegates = (OnEventTriggerSpawnedDelegate)Delegate.Remove(onEventTriggerSpawnedDelegates, del);
		}

		public void FireImmediately(bool resetIntervalCounter)
		{
			if (resetIntervalCounter)
			{
				fireIntervalCounter = interval;
			}
			if (onPreFireDelegates != null)
			{
				onPreFireDelegates(targets);
			}
			Fire();
		}

		protected IEnumerator FiringSystem()
		{
			if (targetTracker == null)
			{
				targetTracker = GetComponent<TargetTracker>();
				if (targetTracker == null)
				{
					yield return null;
					if (targetTracker == null)
					{
						throw new MissingComponentException("FireControllers must be on the same GameObject as a TargetTracker or have it's targetTracker property set by code or drag-and-drop in the inspector.");
					}
				}
			}
			if (initIntervalCountdownAtZero)
			{
				fireIntervalCounter = 0f;
			}
			else
			{
				fireIntervalCounter = interval;
			}
			targets.Clear();
			OnStart();
			keepFiring = true;
			while (keepFiring)
			{
				targets = new TargetList(targetTracker.targets);
				if (targets.Count != 0)
				{
					if (fireIntervalCounter <= 0f)
					{
						targetsCopy.Clear();
						targetsCopy.AddRange(targets);
						if (targetsCopy.Count != 0 && onPreFireDelegates != null)
						{
							onPreFireDelegates(targetsCopy);
						}
						if (targetsCopy.Count != 0)
						{
							Fire();
							fireIntervalCounter = interval;
						}
					}
					OnTargetUpdate(targets);
				}
				else
				{
					OnIdleUpdate();
				}
				fireIntervalCounter -= Time.deltaTime;
				OnUpdate();
				yield return null;
			}
			targets.Clear();
		}
	}
}
