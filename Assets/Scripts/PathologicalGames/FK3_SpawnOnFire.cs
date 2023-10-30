using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Utility - Spawn On Fire (FireController)")]
	public class FK3_SpawnOnFire : MonoBehaviour
	{
		public enum SPAWN_AT_MODE
		{
			FireControllerSpawnAt,
			FireControllerTargetTracker,
			Area,
			ThisTransform,
			OtherTransform
		}

		public Transform prefab;

		public Transform spawnAtTransform;

		public string poolName = string.Empty;

		public bool usePooling = true;

		public GameObject altSource;

		public SPAWN_AT_MODE spawnAtMode = SPAWN_AT_MODE.ThisTransform;

		protected FK3_EventFireController fireCtrl;

		protected FK3_EventTrigger eventTrigger;

		public Transform origin
		{
			get
			{
				Transform result = base.transform;
				switch (spawnAtMode)
				{
				case SPAWN_AT_MODE.FireControllerSpawnAt:
					if (fireCtrl == null)
					{
						throw new MissingComponentException("Must have a FireController to use the FireControllerTargetTracker option");
					}
					if (fireCtrl.spawnEventTriggerAtTransform == null)
					{
						throw new MissingReferenceException($"FireController {fireCtrl.name} doesn't have an emitter set. Add one or use the 'Fire Controller' Origin Mode option.");
					}
					result = fireCtrl.spawnEventTriggerAtTransform;
					break;
				case SPAWN_AT_MODE.FireControllerTargetTracker:
					if (fireCtrl == null)
					{
						throw new MissingComponentException("Must have a FireController to use the 'FireControllerTargetTracker' option");
					}
					result = fireCtrl.targetTracker.transform;
					break;
				case SPAWN_AT_MODE.Area:
				{
					FK3_Area area;
					if (fireCtrl != null)
					{
						area = fireCtrl.targetTracker.area;
						if (area == null)
						{
							throw new MissingReferenceException($"FireController {fireCtrl.name}'s FK3_TargetTracker doesn't have a Area. If by design, such as a FK3_CollisionTargetTracker, use the 'FK3_TargetTracker' or other Origin Mode option.");
						}
					}
					else
					{
						if (!eventTrigger.areaHit)
						{
							throw new MissingReferenceException($"FK3_EventTrigger {eventTrigger.name} areaHit is false. Turn this on before using the 'Area' option");
						}
						area = eventTrigger.area;
					}
					result = area.transform;
					break;
				}
				case SPAWN_AT_MODE.ThisTransform:
					result = base.transform;
					break;
				case SPAWN_AT_MODE.OtherTransform:
					result = spawnAtTransform;
					break;
				}
				return result;
			}
			set
			{
			}
		}

		private void Awake()
		{
			GameObject gameObject = (!altSource) ? base.gameObject : altSource;
			FK3_EventFireController component = gameObject.GetComponent<FK3_EventFireController>();
			if (component != null)
			{
				fireCtrl = component;
				fireCtrl.AddOnFireDelegate(OnFire);
			}
			else
			{
				FK3_EventTrigger component2 = gameObject.GetComponent<FK3_EventTrigger>();
				if (component2 != null)
				{
					eventTrigger = component2;
					eventTrigger.AddOnFireDelegate(OnFire);
				}
			}
			if (fireCtrl == null && eventTrigger == null)
			{
				throw new MissingComponentException("Must have either an FK3_EventFireController or FK3_EventTrigger.");
			}
		}

		protected void OnFire(List<FK3_Target> targets)
		{
			if (!(prefab == null))
			{
				FK3_InstanceManager.Spawn(poolName, prefab, origin.position, origin.rotation);
			}
		}
	}
}
