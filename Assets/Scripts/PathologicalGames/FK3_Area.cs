using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("")]
	public class FK3_Area : MonoBehaviour, IList<FK3_Target>, IEnumerable, ICollection<FK3_Target>, IEnumerable<FK3_Target>
	{
		public FK3_AreaTargetTracker targetTracker;

		internal FK3_TargetList targets = new FK3_TargetList();

		public GameObject go;

		public Collider coll;

		public Rigidbody rbd;

		public Collider2D coll2D;

		public Rigidbody2D rbd2D;

		public FK3_Target this[int index]
		{
			get
			{
				return targets[index];
			}
			set
			{
				throw new NotImplementedException("Read-only.");
			}
		}

		public bool IsReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public int Count => targets.Count;

		protected void Awake()
		{
			go = base.gameObject;
			if (GetComponent<Rigidbody>() == null && GetComponent<Rigidbody2D>() == null)
			{
				string message = "Areas must have a Rigidbody or Rigidbody2D.";
				throw new MissingComponentException(message);
			}
			rbd2D = GetComponent<Rigidbody2D>();
			coll2D = GetComponent<Collider2D>();
			rbd = GetComponent<Rigidbody>();
			coll = GetComponent<Collider>();
		}

		public bool IsInRange(FK3_Targetable targetable)
		{
			if (coll != null)
			{
				Vector3 size = coll.bounds.size;
				float radius = Mathf.Max(Mathf.Max(size.x, size.y), size.z);
				List<Collider> list = new List<Collider>(Physics.OverlapSphere(base.transform.position, radius));
				return list.Contains(coll);
			}
			if (coll2D != null)
			{
				List<Collider2D> list2 = new List<Collider2D>();
				BoxCollider2D boxCollider2D = coll2D as BoxCollider2D;
				if (boxCollider2D != null)
				{
					Vector3 position = base.transform.position;
					float x = position.x;
					Vector3 position2 = base.transform.position;
					Vector2 b = new Vector2(x, position2.y);
					Vector2 a = boxCollider2D.offset + b;
					Vector2 b2 = boxCollider2D.size * 0.5f;
					Vector2 pointA = a + b2;
					Vector2 pointB = a - b2;
					list2.AddRange(Physics2D.OverlapAreaAll(pointA, pointB));
				}
				else
				{
					CircleCollider2D circleCollider2D = coll2D as CircleCollider2D;
					if (circleCollider2D != null)
					{
						list2.AddRange(Physics2D.OverlapCircleAll(base.transform.position, circleCollider2D.radius * 2f));
					}
				}
				return list2.Contains(coll2D);
			}
			UnityEngine.Debug.Log("IsInRange returning false due to no collider set. This may be fine.");
			return false;
		}

		protected void OnTriggerEnter(Collider other)
		{
			OnTriggerEntered(other);
		}

		protected void OnTriggerExit(Collider other)
		{
			OnTriggerExited(other);
		}

		protected void OnTriggerEnter2D(Collider2D other)
		{
			OnTriggerEntered(other);
		}

		protected void OnTriggerExit2D(Collider2D other)
		{
			OnTriggerExited(other);
		}

		protected void OnTriggerEntered(Component other)
		{
			FK3_Targetable component = other.GetComponent<FK3_Targetable>();
			if (!(component == null))
			{
				Add(component);
			}
		}

		protected void OnTriggerExited(Component other)
		{
			Remove(other.transform);
		}

		public void Add(FK3_Target target)
		{
			if (target.targetable.isTargetable && go.activeInHierarchy && !targets.Contains(target) && IsInLayerMask(target.targetable.go) && !targetTracker.IsIgnoreTargetOnNewDetectedDelegatesExecute(target))
			{
				targets.Add(target);
				target.targetable.OnDetected(targetTracker);
				targetTracker.targets = targets;
			}
		}

		public void Add(FK3_Targetable targetable)
		{
			FK3_Target target = new FK3_Target(targetable, targetTracker);
			Add(target);
		}

		public bool Remove(Transform xform)
		{
			return Remove(new FK3_Target(xform, targetTracker));
		}

		public bool Remove(FK3_Targetable targetable)
		{
			return Remove(new FK3_Target(targetable, targetTracker));
		}

		public bool Remove(FK3_Target target)
		{
			if (!targets.Remove(target))
			{
				return false;
			}
			if (target.transform == null || base.transform == null || base.transform.parent == null)
			{
				return false;
			}
			target.targetable.OnNotDetected(targetTracker);
			targetTracker.targets = targets;
			return true;
		}

		public void Clear()
		{
			foreach (FK3_Target target in targets)
			{
				FK3_Target current = target;
				current.targetable.OnNotDetected(targetTracker);
			}
			targets.Clear();
			targetTracker.targets = targets;
		}

		public bool Contains(Transform transform)
		{
			return targets.Contains(new FK3_Target(transform, targetTracker));
		}

		public bool Contains(FK3_Target target)
		{
			return targets.Contains(target);
		}

		public IEnumerator<FK3_Target> GetEnumerator()
		{
			for (int i = 0; i < targets.Count; i++)
			{
				yield return targets[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < targets.Count; i++)
			{
				yield return targets[i];
			}
		}

		public void CopyTo(FK3_Target[] array, int arrayIndex)
		{
			targets.CopyTo(array, arrayIndex);
		}

		public int IndexOf(FK3_Target item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, FK3_Target item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			string[] array = new string[targets.Count];
			int num = 0;
			foreach (FK3_Target target in targets)
			{
				FK3_Target current = target;
				if (current.transform == null)
				{
					array[num] = "null";
					num++;
				}
				else
				{
					string text = $"{current.transform.name}:Layer={LayerMask.LayerToName(current.gameObject.layer)}";
					switch (targetTracker.sortingStyle)
					{
					case FK3_TargetTracker.SORTING_STYLES.Nearest:
					case FK3_TargetTracker.SORTING_STYLES.Farthest:
					{
						float sqrDistToPos = current.targetable.GetSqrDistToPos(base.transform.position);
						text += $",Dist={sqrDistToPos}";
						break;
					}
					case FK3_TargetTracker.SORTING_STYLES.NearestToDestination:
					case FK3_TargetTracker.SORTING_STYLES.FarthestFromDestination:
						text += $",DistToDest={current.targetable.distToDest}";
						break;
					}
					array[num] = text;
					num++;
				}
			}
			return string.Format("[{0}]", string.Join(", ", array));
		}

		protected bool IsInLayerMask(GameObject obj)
		{
			LayerMask layerMask = 1 << obj.layer;
			LayerMask targetLayers = targetTracker.targetLayers;
			return (targetLayers.value & layerMask.value) != 0;
		}
	}
}
