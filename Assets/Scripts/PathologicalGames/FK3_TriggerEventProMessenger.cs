using System;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Messenger")]
	public class FK3_TriggerEventProMessenger : MonoBehaviour
	{
		[Flags]
		public enum COMPONENTS
		{
			FireController = 0x1,
			FK3_EventTrigger = 0x2,
			FK3_Targetable = 0x4,
			FK3_TargetTracker = 0x8
		}

		public enum MESSAGE_MODE
		{
			Send,
			Broadcast
		}

		public COMPONENTS forComponent;

		public MESSAGE_MODE messageMode;

		public GameObject otherTarget;

		public FK3_DEBUG_LEVELS debugLevel;

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
			if (debugLevel > FK3_DEBUG_LEVELS.Off)
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
			if (debugLevel > FK3_DEBUG_LEVELS.Off)
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
			FK3_TargetTracker component = GetComponent<FK3_TargetTracker>();
			if (!(component == null))
			{
				component.AddOnPostSortDelegate(FK3_OnPostSortDelegate);
				component.AddOnNewDetectedDelegate(FK3_OnNewDetectedDelegate);
				component.AddOnTargetsChangedDelegate(FK3_OnTargetsChangedDelegate);
			}
		}

		protected void FK3_OnPostSortDelegate(FK3_TargetTracker source, FK3_TargetList targets)
		{
			if (targetTracker_OnPostSort)
			{
				FK3_MessageData_TargetTrackerEvent arg = new FK3_MessageData_TargetTrackerEvent(source, targets);
				handleMsg("TargetTracker_OnPostSort", arg);
			}
		}

		protected bool FK3_OnNewDetectedDelegate(FK3_TargetTracker source, FK3_Target target)
		{
			if (!targetTracker_OnNewDetected)
			{
				return true;
			}
			FK3_MessageData_TargetTrackerEvent arg = new FK3_MessageData_TargetTrackerEvent(source, target);
			handleMsg("TargetTracker_OnNewDetected", arg);
			return true;
		}

		protected void FK3_OnTargetsChangedDelegate(FK3_TargetTracker source)
		{
			if (targetTracker_OnTargetsChanged)
			{
				handleMsg("TargetTracker_OnTargetsChanged", source);
			}
		}

		protected void RegisterFireController()
		{
			FK3_EventFireController component = GetComponent<FK3_EventFireController>();
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

		protected void OnTargetUpdateDelegate(FK3_TargetList targets)
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

		protected void OnPreFireDelegate(FK3_TargetList targets)
		{
			if (fireController_OnPreFire)
			{
				handleMsg("FireController_OnPreFire", targets);
			}
		}

		protected void OnFireDelegate(FK3_TargetList targets)
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
			FK3_EventTrigger component = GetComponent<FK3_EventTrigger>();
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

		protected void EventTrigger_OnFireDelegate(FK3_TargetList targets)
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

		protected void OnHitTargetDelegate(FK3_Target target)
		{
			if (eventTrigger_OnHitTarget)
			{
				handleMsg("EventTrigger_OnHitTarget", target);
			}
		}

		protected void RegisterTargetable()
		{
			FK3_Targetable component = GetComponent<FK3_Targetable>();
			if (!(component == null))
			{
				component.AddOnHitDelegate(OnHitDelegate);
				component.AddOnDetectedDelegate(OnDetectedDelegate);
				component.AddOnNotDetectedDelegate(OnNotDetectedDelegate);
			}
		}

		protected void OnHitDelegate(FK3_EventInfoList eventInfoList, FK3_Target target)
		{
			if (targetable_OnHit)
			{
				FK3_MessageData_TargetableOnHit arg = new FK3_MessageData_TargetableOnHit(eventInfoList, target);
				handleMsg("Targetable_OnHit", arg);
			}
		}

		protected void OnHitColliderDelegate(FK3_EventInfoList eventInfoList, FK3_Target target, Collider other)
		{
			if (targetable_OnHit)
			{
				FK3_MessageData_TargetableOnHit arg = new FK3_MessageData_TargetableOnHit(eventInfoList, target, other);
				handleMsg("Targetable_OnHitCollider", arg);
			}
		}

		protected void OnDetectedDelegate(FK3_TargetTracker source)
		{
			if (targetable_OnDetected)
			{
				handleMsg("Targetable_OnDetected", source);
			}
		}

		protected void OnNotDetectedDelegate(FK3_TargetTracker source)
		{
			if (targetable_OnNotDetected)
			{
				handleMsg("Targetable_OnNotDetected", source);
			}
		}
	}
}
