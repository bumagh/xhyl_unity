using System;
using UnityEngine;

namespace PathologicalGames
{
	public struct Target : IComparable<Target>
	{
		public GameObject gameObject;

		public Transform transform;

		public Targetable targetable;

		public TargetTracker targetTracker;

		public EventFireController fireController;

		public EventTrigger eventTrigger;

		public Collider collider;

		public Collider2D collider2D;

		private static Target _Null = default(Target);

		public static Target Null => _Null;

		public bool isSpawned => !(gameObject == null) && gameObject.activeInHierarchy;

		public Target(Transform transform, TargetTracker targetTracker)
		{
			gameObject = transform.gameObject;
			this.transform = transform;
			targetable = transform.GetComponent<Targetable>();
			this.targetTracker = targetTracker;
			eventTrigger = null;
			collider = null;
			collider2D = null;
			eventTrigger = (targetTracker as EventTrigger);
			if (eventTrigger != null)
			{
				collider = eventTrigger.coll;
				collider2D = eventTrigger.coll2D;
			}
			fireController = null;
		}

		public Target(Targetable targetable, TargetTracker targetTracker)
		{
			gameObject = targetable.go;
			transform = targetable.transform;
			this.targetable = targetable;
			this.targetTracker = targetTracker;
			eventTrigger = null;
			collider = null;
			collider2D = null;
			eventTrigger = (targetTracker as EventTrigger);
			if (eventTrigger != null)
			{
				collider = eventTrigger.coll;
				collider2D = eventTrigger.coll2D;
			}
			fireController = null;
		}

		public Target(Target otherTarget)
		{
			gameObject = otherTarget.gameObject;
			transform = otherTarget.transform;
			targetable = otherTarget.targetable;
			targetTracker = otherTarget.targetTracker;
			fireController = otherTarget.fireController;
			eventTrigger = otherTarget.eventTrigger;
			collider = otherTarget.collider;
			collider2D = otherTarget.collider2D;
		}

		public static bool operator ==(Target tA, Target tB)
		{
			return tA.gameObject == tB.gameObject;
		}

		public static bool operator !=(Target tA, Target tB)
		{
			return tA.gameObject != tB.gameObject;
		}

		public override int GetHashCode()
		{
			return ((ValueType)this).GetHashCode();
		}

		public override bool Equals(object other)
		{
			return other != null && this == (Target)other;
		}

		public int CompareTo(Target obj)
		{
			return (gameObject == obj.gameObject) ? 1 : 0;
		}
	}
}
