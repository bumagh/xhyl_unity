using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO FK3_EventTrigger")]
	public class FK3_EventTrigger : FK3_AreaTargetTracker
	{
		public enum HIT_MODES
		{
			TargetOnly,
			HitLayers
		}

		public enum NOTIFY_TARGET_OPTIONS
		{
			Off,
			Direct,
			PassInfoToEventTrigger,
			PassInfoToEventTriggerOnce,
			UseEventTriggerInfo,
			UseEventTriggerInfoOnce
		}

		public delegate void OnListenStart();

		public delegate void OnListenUpdate();

		public delegate void OnFire(FK3_TargetList targets);

		public delegate void OnFireUpdate(float progress);

		public delegate void OnHitTarget(FK3_Target target);

		public FK3_Target target;

		public List<FK3_EventInfoListGUIBacker> _eventInfoList = new List<FK3_EventInfoListGUIBacker>();

		public bool areaHit = true;

		public bool fireOnRigidBodySleep = true;

		public HIT_MODES hitMode = HIT_MODES.HitLayers;

		public float listenTimeout;

		public float duration;

		public Vector3 startRange = Vector3.zero;

		public Vector3 endRange = Vector3.zero;

		public NOTIFY_TARGET_OPTIONS notifyTargets = NOTIFY_TARGET_OPTIONS.Direct;

		public FK3_EventTrigger eventTriggerPrefab;

		public FK3_EventFireController fireController;

		public bool fireOnSpawn;

		public bool usePooling = true;

		public bool overridePoolName;

		public string eventTriggerPoolName = "EventTriggers";

		public string poolName = "EventTriggers";

		public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

		protected float curTimer;

		protected bool blockNotifications;

		protected FK3_TargetList detectedTargetsCache = new FK3_TargetList();

		public Rigidbody rbd;

		public Collider coll;

		public Rigidbody2D rbd2D;

		public Collider2D coll2D;

		protected OnListenStart OnListenStartDelegates;

		protected OnListenUpdate OnListenUpdateDelegates;

		protected OnFire OnFireDelegates;

		protected OnFireUpdate OnFireUpdateDelegates;

		protected OnHitTarget OnHitTargetDelegates;

		public FK3_EventInfoList eventInfoList
		{
			get
			{
				FK3_EventInfoList fK3_EventInfoList = new FK3_EventInfoList();
				foreach (FK3_EventInfoListGUIBacker eventInfo in _eventInfoList)
				{
					fK3_EventInfoList.Add(new FK3_EventInfo
					{
						name = eventInfo.name,
						value = eventInfo.value,
						duration = eventInfo.duration
					});
				}
				return fK3_EventInfoList;
			}
			set
			{
				_eventInfoList.Clear();
				foreach (FK3_EventInfo item2 in value)
				{
					FK3_EventInfoListGUIBacker item = new FK3_EventInfoListGUIBacker(item2);
					_eventInfoList.Add(item);
				}
			}
		}

		protected override void Awake()
		{
			areaColliderEnabledAtStart = false;
			base.Awake();
			rbd2D = GetComponent<Rigidbody2D>();
			coll2D = GetComponent<Collider2D>();
			rbd = GetComponent<Rigidbody>();
			coll = GetComponent<Collider>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (fireOnSpawn)
			{
				StartCoroutine(Fire());
			}
			else
			{
				StartCoroutine(Listen());
			}
			AddOnNewDetectedDelegate(OnNewTargetHandler);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			target = FK3_Target.Null;
			base.range = endRange;
			RemoveOnNewDetectedDelegate(OnNewTargetHandler);
		}

		protected IEnumerator Listen()
		{
			curTimer = listenTimeout;
			yield return null;
			if (OnListenStartDelegates != null)
			{
				OnListenStartDelegates();
			}
			while (true)
			{
				if (!target.isSpawned)
				{
					target = FK3_Target.Null;
				}
				if (OnListenUpdateDelegates != null)
				{
					OnListenUpdateDelegates();
				}
				if ((fireOnRigidBodySleep && rbd != null && rbd.IsSleeping()) || (rbd2D != null && rbd2D.IsSleeping()))
				{
					break;
				}
				if (listenTimeout > 0f)
				{
					if (curTimer <= 0f)
					{
						break;
					}
					curTimer -= Time.deltaTime;
				}
				yield return null;
			}
			StartCoroutine(Fire());
		}

		protected void OnTriggered(GameObject other)
		{
			switch (hitMode)
			{
			case HIT_MODES.HitLayers:
				if (((1 << other.layer) & (int)targetLayers) == 0)
				{
					break;
				}
				if (!areaHit)
				{
					FK3_Targetable component = other.GetComponent<FK3_Targetable>();
					if (component != null)
					{
						target = new FK3_Target(component, this);
					}
				}
				StartCoroutine(Fire());
				break;
			case HIT_MODES.TargetOnly:
				if (target.isSpawned && target.gameObject == other)
				{
					StartCoroutine(Fire());
				}
				break;
			}
		}

		protected void OnTriggerEnter(Collider other)
		{
			OnTriggered(other.gameObject);
		}

		protected void OnTriggerEnter2D(Collider2D other)
		{
			OnTriggered(other.gameObject);
		}

		protected bool OnNewTargetHandler(FK3_TargetTracker targetTracker, FK3_Target target)
		{
			if (target == FK3_Target.Null)
			{
				return false;
			}
			FK3_Target item = new FK3_Target(target.targetable, this);
			detectedTargetsCache.Add(item);
			if (!blockNotifications)
			{
				HandleNotifyTarget(target);
			}
			return false;
		}

		protected void HandleNotifyTarget(FK3_Target target)
		{
			switch (notifyTargets)
			{
			case NOTIFY_TARGET_OPTIONS.PassInfoToEventTriggerOnce:
				SpawnOnFirePrefab(passInfo: true);
				break;
			case NOTIFY_TARGET_OPTIONS.UseEventTriggerInfoOnce:
				SpawnOnFirePrefab(passInfo: false);
				break;
			case NOTIFY_TARGET_OPTIONS.Direct:
				target.targetable.OnHit(eventInfoList, target);
				if (OnHitTargetDelegates != null)
				{
					OnHitTargetDelegates(target);
				}
				break;
			}
		}

		public virtual IEnumerator Fire()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				yield break;
			}
			if (duration > 0f)
			{
				endRange = base.range;
				base.range = startRange;
			}
			detectedTargetsCache.Clear();
			blockNotifications = true;
			if (areaHit)
			{
				setAreaColliderEnabled(enable: true);
				yield return null;
			}
			else
			{
				OnNewTargetHandler(this, target);
			}
			blockNotifications = false;
			if (OnFireDelegates != null)
			{
				OnFireDelegates(detectedTargetsCache);
			}
			foreach (FK3_Target item in detectedTargetsCache)
			{
				HandleNotifyTarget(item);
			}
			detectedTargetsCache.Clear();
			if (notifyTargets == NOTIFY_TARGET_OPTIONS.PassInfoToEventTrigger)
			{
				SpawnOnFirePrefab(passInfo: true);
			}
			if (duration != 0f)
			{
				if (duration < 0f)
				{
					while (true)
					{
						if (OnFireUpdateDelegates != null)
						{
							OnFireUpdateDelegates(-1f);
						}
						yield return null;
					}
				}
				float timer = 0f;
				float progress = 0f;
				while (progress < 1f)
				{
					if (OnFireUpdateDelegates != null)
					{
						OnFireUpdateDelegates(progress);
					}
					timer += Time.deltaTime;
					progress = timer / duration;
					base.range = endRange * progress;
					yield return null;
				}
				base.range = endRange;
			}
			if (base.gameObject.activeInHierarchy)
			{
				FK3_InstanceManager.Despawn(poolName, base.transform);
				yield return null;
			}
		}

		protected void SpawnOnFirePrefab(bool passInfo)
		{
			if (!(eventTriggerPrefab == null))
			{
				string text = (!usePooling) ? string.Empty : (overridePoolName ? eventTriggerPoolName : eventTriggerPrefab.poolName);
				Transform transform = FK3_InstanceManager.Spawn(text, eventTriggerPrefab.transform, base.transform.position, base.transform.rotation);
				FK3_EventTrigger component = transform.GetComponent<FK3_EventTrigger>();
				component.poolName = text;
				component.fireController = fireController;
				if (passInfo)
				{
					component.areaShape = base.areaShape;
					component.range = base.range;
					component.eventInfoList = eventInfoList;
				}
			}
		}

		public void AddOnListenStartDelegate(OnListenStart del)
		{
			OnListenStartDelegates = (OnListenStart)Delegate.Combine(OnListenStartDelegates, del);
		}

		public void SetOnListenStartDelegate(OnListenStart del)
		{
			OnListenStartDelegates = del;
		}

		public void RemoveOnListenStartDelegate(OnListenStart del)
		{
			OnListenStartDelegates = (OnListenStart)Delegate.Remove(OnListenStartDelegates, del);
		}

		public void AddOnListenUpdateDelegate(OnListenUpdate del)
		{
			OnListenUpdateDelegates = (OnListenUpdate)Delegate.Combine(OnListenUpdateDelegates, del);
		}

		public void SetOnListenUpdateDelegate(OnListenUpdate del)
		{
			OnListenUpdateDelegates = del;
		}

		public void RemoveOnListenUpdateDelegate(OnListenUpdate del)
		{
			OnListenUpdateDelegates = (OnListenUpdate)Delegate.Remove(OnListenUpdateDelegates, del);
		}

		public void AddOnFireDelegate(OnFire del)
		{
			OnFireDelegates = (OnFire)Delegate.Combine(OnFireDelegates, del);
		}

		public void SetOnFireDelegate(OnFire del)
		{
			OnFireDelegates = del;
		}

		public void RemoveOnFireDelegate(OnFire del)
		{
			OnFireDelegates = (OnFire)Delegate.Remove(OnFireDelegates, del);
		}

		public void AddOnFireUpdateDelegate(OnFireUpdate del)
		{
			OnFireUpdateDelegates = (OnFireUpdate)Delegate.Combine(OnFireUpdateDelegates, del);
		}

		public void SetOnFireUpdateDelegate(OnFireUpdate del)
		{
			OnFireUpdateDelegates = del;
		}

		public void RemoveOnFireUpdateDelegate(OnFireUpdate del)
		{
			OnFireUpdateDelegates = (OnFireUpdate)Delegate.Remove(OnFireUpdateDelegates, del);
		}

		public void AddOnHitTargetDelegate(OnHitTarget del)
		{
			OnHitTargetDelegates = (OnHitTarget)Delegate.Combine(OnHitTargetDelegates, del);
		}

		public void SetOnHitTargetDelegate(OnHitTarget del)
		{
			OnHitTargetDelegates = del;
		}

		public void RemoveOnHitTargetDelegate(OnHitTarget del)
		{
			OnHitTargetDelegates = (OnHitTarget)Delegate.Remove(OnHitTargetDelegates, del);
		}
	}
}
