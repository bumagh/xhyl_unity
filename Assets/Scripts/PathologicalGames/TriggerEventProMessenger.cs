using System;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Messenger")]
	public class TriggerEventProMessenger : MonoBehaviour
	{
		[Flags]
		public enum COMPONENTS
		{
			FireController = 0x1,
			EventTrigger = 0x2,
			Targetable = 0x4,
			TargetTracker = 0x8
		}

		public enum MESSAGE_MODE
		{
			Send,
			Broadcast
		}

		public COMPONENTS forComponent;

		public MESSAGE_MODE messageMode;

		public GameObject otherTarget;

		public DEBUG_LEVELS debugLevel;

		public bool targetTracker_OnPostSort;

		public bool targetTracker_OnNewDetected;

		public bool targetTracker_OnTargetsChanged;

		public bool fireController_OnStart;

		public bool fireController_OnUpdate;

		public bool fireController_OnTargetUpdate;

		public bool fireController_OnIdleUpdate;

		public bool fireController_OnPreFire;

		public bool fireController_OnFire;

		public bool fireController_OnStop;

		public bool eventTrigger_OnListenStart;

		public bool eventTrigger_OnListenUpdate;

		public bool eventTrigger_OnFire;

		public bool eventTrigger_OnFireUpdate;

		public bool eventTrigger_OnHitTarget;

		public bool targetable_OnHit;

		public bool targetable_OnHitCollider;

		public bool targetable_OnDetected;

		public bool targetable_OnNotDetected;

		protected void Awake()
		{
			RegisterTargetTracker();
			RegisterFireController();
			RegisterEventTrigger();
			RegisterTargetable();
		}

		protected void handleMsg(string msg)
		{
			GameObject gameObject = (!(otherTarget == null)) ? otherTarget : base.gameObject;
			if (debugLevel > DEBUG_LEVELS.Off)
			{
				UnityEngine.Debug.Log($"Sending message '{msg}' to '{gameObject}'");
			}
			if (messageMode == MESSAGE_MODE.Send)
			{
				gameObject.SendMessage(msg, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				gameObject.BroadcastMessage(msg, SendMessageOptions.DontRequireReceiver);
			}
		}

		protected void handleMsg<T>(string msg, T arg)
		{
			GameObject gameObject = (!(otherTarget == null)) ? otherTarget : base.gameObject;
			if (debugLevel > DEBUG_LEVELS.Off)
			{
				UnityEngine.Debug.Log($"Sending message '{msg}' to '{gameObject}' with argument {arg}");
			}
			if (messageMode == MESSAGE_MODE.Send)
			{
				gameObject.SendMessage(msg, arg, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				gameObject.BroadcastMessage(msg, arg, SendMessageOptions.DontRequireReceiver);
			}
		}

		protected void RegisterTargetTracker()
		{
			TargetTracker component = GetComponent<TargetTracker>();
			if (!(component == null))
			{
				component.AddOnPostSortDelegate(OnPostSortDelegate);
				component.AddOnNewDetectedDelegate(OnNewDetectedDelegate);
				component.AddOnTargetsChangedDelegate(OnTargetsChangedDelegate);
			}
		}

		protected void OnPostSortDelegate(TargetTracker source, TargetList targets)
		{
			if (targetTracker_OnPostSort)
			{
				MessageData_TargetTrackerEvent arg = new MessageData_TargetTrackerEvent(source, targets);
				handleMsg("TargetTracker_OnPostSort", arg);
			}
		}

		protected bool OnNewDetectedDelegate(TargetTracker source, Target target)
		{
			if (!targetTracker_OnNewDetected)
			{
				return true;
			}
			MessageData_TargetTrackerEvent arg = new MessageData_TargetTrackerEvent(source, target);
			handleMsg("TargetTracker_OnNewDetected", arg);
			return true;
		}

		protected void OnTargetsChangedDelegate(TargetTracker source)
		{
			if (targetTracker_OnTargetsChanged)
			{
				handleMsg("TargetTracker_OnTargetsChanged", source);
			}
		}

		protected void RegisterFireController()
		{
			EventFireController component = GetComponent<EventFireController>();
			if (!(component == null))
			{
				component.AddOnStartDelegate(OnStartDelegate);
				component.AddOnUpdateDelegate(OnUpdateDelegate);
				component.AddOnTargetUpdateDelegate(OnTargetUpdateDelegate);
				component.AddOnIdleUpdateDelegate(OnIdleUpdateDelegate);
				component.AddOnPreFireDelegate(OnPreFireDelegate);
				component.AddOnFireDelegate(OnFireDelegate);
				component.AddOnStopDelegate(OnStopDelegate);
			}
		}

		protected void OnStartDelegate()
		{
			if (fireController_OnStart)
			{
				handleMsg("FireController_OnStart");
			}
		}

		protected void OnUpdateDelegate()
		{
			if (fireController_OnUpdate)
			{
				handleMsg("FireController_OnUpdate");
			}
		}

		protected void OnTargetUpdateDelegate(TargetList targets)
		{
			if (fireController_OnTargetUpdate)
			{
				handleMsg("FireController_OnTargetUpdate", targets);
			}
		}

		protected void OnIdleUpdateDelegate()
		{
			if (fireController_OnIdleUpdate)
			{
				handleMsg("FireController_OnIdleUpdate");
			}
		}

		protected void OnPreFireDelegate(TargetList targets)
		{
			if (fireController_OnPreFire)
			{
				handleMsg("FireController_OnPreFire", targets);
			}
		}

		protected void OnFireDelegate(TargetList targets)
		{
			if (fireController_OnFire)
			{
				handleMsg("FireController_OnFire", targets);
			}
		}

		protected void OnStopDelegate()
		{
			if (fireController_OnStop)
			{
				handleMsg("FireController_OnStop");
			}
		}

		protected void RegisterEventTrigger()
		{
			EventTrigger component = GetComponent<EventTrigger>();
			if (!(component == null))
			{
				component.AddOnListenStartDelegate(OnListenStartDelegate);
				component.AddOnListenUpdateDelegate(OnListenUpdateDelegate);
				component.AddOnFireDelegate(EventTrigger_OnFireDelegate);
				component.AddOnFireUpdateDelegate(OnFireUpdateDelegate);
				component.AddOnHitTargetDelegate(OnHitTargetDelegate);
			}
		}

		protected void OnListenStartDelegate()
		{
			if (eventTrigger_OnListenStart)
			{
				handleMsg("EventTrigger_OnListenStart");
			}
		}

		protected void OnListenUpdateDelegate()
		{
			if (eventTrigger_OnListenUpdate)
			{
				handleMsg("EventTrigger_OnListenUpdate");
			}
		}

		protected void EventTrigger_OnFireDelegate(TargetList targets)
		{
			if (eventTrigger_OnFire)
			{
				handleMsg("EventTrigger_OnFire", targets);
			}
		}

		protected void OnFireUpdateDelegate(float progress)
		{
			if (eventTrigger_OnFireUpdate)
			{
				handleMsg("EventTrigger_OnFireUpdate", progress);
			}
		}

		protected void OnHitTargetDelegate(Target target)
		{
			if (eventTrigger_OnHitTarget)
			{
				handleMsg("EventTrigger_OnHitTarget", target);
			}
		}

		protected void RegisterTargetable()
		{
			Targetable component = GetComponent<Targetable>();
			if (!(component == null))
			{
				component.AddOnHitDelegate(OnHitDelegate);
				component.AddOnDetectedDelegate(OnDetectedDelegate);
				component.AddOnNotDetectedDelegate(OnNotDetectedDelegate);
			}
		}

		protected void OnHitDelegate(EventInfoList eventInfoList, Target target)
		{
			if (targetable_OnHit)
			{
				MessageData_TargetableOnHit arg = new MessageData_TargetableOnHit(eventInfoList, target);
				handleMsg("Targetable_OnHit", arg);
			}
		}

		protected void OnHitColliderDelegate(EventInfoList eventInfoList, Target target, Collider other)
		{
			if (targetable_OnHit)
			{
				MessageData_TargetableOnHit arg = new MessageData_TargetableOnHit(eventInfoList, target, other);
				handleMsg("Targetable_OnHitCollider", arg);
			}
		}

		protected void OnDetectedDelegate(TargetTracker source)
		{
			if (targetable_OnDetected)
			{
				handleMsg("Targetable_OnDetected", source);
			}
		}

		protected void OnNotDetectedDelegate(TargetTracker source)
		{
			if (targetable_OnNotDetected)
			{
				handleMsg("Targetable_OnNotDetected", source);
			}
		}
	}
}
