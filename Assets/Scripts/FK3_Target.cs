using System;
using UnityEngine;

namespace PathologicalGames
{
	public struct FK3_Target : IComparable<FK3_Target>
	{
		public GameObject gameObject;

		public Transform transform;

		public FK3_Targetable targetable;

		public FK3_TargetTracker targetTracker;

		public FK3_EventFireController fireController;

		public FK3_EventTrigger eventTrigger;

		public Collider collider;

		public Collider2D collider2D;

		private static FK3_Target _Null = default(FK3_Target);

		public static FK3_Target Null => _Null;

		public bool isSpawned => !(gameObject == null) && gameObject.activeInHierarchy;

		public FK3_Target(Transform transform, FK3_TargetTracker targetTracker)
		{
			gameObject = transform.gameObject;
			this.transform = transform;
			targetable = transform.GetComponent<FK3_Targetable>();
			this.targetTracker = targetTracker;
			eventTrigger = null;
			collider = null;
			collider2D = null;
			eventTrigger = (targetTracker as FK3_EventTrigger);
			if (eventTrigger != null)
			{
				collider = eventTrigger.coll;
				collider2D = eventTrigger.coll2D;
			}
			fireController = null;
		}

		public FK3_Target(FK3_Targetable targetable, FK3_TargetTracker targetTracker)
		{
			gameObject = targetable.go;
			transform = targetable.transform;
			this.targetable = targetable;
			this.targetTracker = targetTracker;
			eventTrigger = null;
			collider = null;
			collider2D = null;
			eventTrigger = (targetTracker as FK3_EventTrigger);
			if (eventTrigger != null)
			{
				collider = eventTrigger.coll;
				collider2D = eventTrigger.coll2D;
			}
			fireController = null;
		}

		public FK3_Target(FK3_Target otherTarget)
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

		public static bool operator ==(FK3_Target tA, FK3_Target tB)
		{
			return tA.gameObject == tB.gameObject;
		}

		public static bool operator !=(FK3_Target tA, FK3_Target tB)
		{
			return tA.gameObject != tB.gameObject;
		}

		public override int GetHashCode()
		{
			return ((ValueType)this).GetHashCode();
		}

		public override bool Equals(object other)
		{
			return other != null && this == (FK3_Target)other;
		}

		public int CompareTo(FK3_Target obj)
		{
			return (gameObject == obj.gameObject) ? 1 : 0;
		}
	}
}
