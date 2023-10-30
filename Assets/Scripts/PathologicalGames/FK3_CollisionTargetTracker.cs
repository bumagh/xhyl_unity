using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Collision FK3_TargetTracker")]
	public class FK3_CollisionTargetTracker : FK3_TargetTracker
	{
		protected FK3_TargetList allTargets = new FK3_TargetList();

		public Collider coll;

		public Collider2D coll2D;

		public override FK3_TargetList targets
		{
			get
			{
				_targets.Clear();
				if (numberOfTargets == 0)
				{
					return _targets;
				}
				List<FK3_Target> list = new List<FK3_Target>(allTargets);
				foreach (FK3_Target allTarget in allTargets)
				{
					FK3_Target current = allTarget;
					if (current.gameObject == null || !current.gameObject.activeInHierarchy)
					{
						list.Remove(current);
					}
				}
				if (list.Count == 0)
				{
					return _targets;
				}
				if (numberOfTargets == -1)
				{
					_targets.AddRange(list);
				}
				else
				{
					int num = Mathf.Clamp(numberOfTargets, 0, list.Count);
					for (int i = 0; i < num; i++)
					{
						_targets.Add(list[i]);
					}
				}
				if (onPostSortDelegates != null)
				{
					onPostSortDelegates(this, _targets);
				}
				return _targets;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			coll = GetComponent<Collider>();
			coll2D = GetComponent<Collider2D>();
			if (coll == null && coll2D == null)
			{
				string message = "No 2D or 3D collider or compound (child) collider found.";
				throw new MissingComponentException(message);
			}
			if ((coll != null && coll.isTrigger) || (coll2D != null && coll2D.isTrigger))
			{
				throw new Exception("CollisionTargetTrackers do not work with trigger colliders.It is designed to work with Physics OnCollider events only.");
			}
		}

		protected void OnCollisionEnter(Collision collisionInfo)
		{
			OnCollisionEnterAny(collisionInfo.gameObject);
		}

		protected void OnCollisionExit(Collision collisionInfo)
		{
			OnCollisionExitAny(collisionInfo.gameObject);
		}

		protected void OnCollisionEnter2D(Collision2D collisionInfo)
		{
			OnCollisionEnterAny(collisionInfo.gameObject);
		}

		protected void OnCollisionExit2D(Collision2D collisionInfo)
		{
			OnCollisionExitAny(collisionInfo.gameObject);
		}

		protected void OnCollisionEnterAny(GameObject otherGo)
		{
			if (IsInLayerMask(otherGo))
			{
				FK3_Target fK3_Target = new FK3_Target(otherGo.transform, this);
				if (!(fK3_Target == FK3_Target.Null) && fK3_Target.targetable.isTargetable && !IsIgnoreTargetOnNewDetectedDelegatesExecute(fK3_Target) && !allTargets.Contains(fK3_Target))
				{
					allTargets.Add(fK3_Target);
				}
			}
		}

		protected void OnCollisionExitAny(GameObject otherGO)
		{
			FK3_Target fK3_Target = default(FK3_Target);
			foreach (FK3_Target allTarget in allTargets)
			{
				FK3_Target current = allTarget;
				if (current.gameObject == otherGO)
				{
					fK3_Target = current;
				}
			}
			if (!(fK3_Target == FK3_Target.Null))
			{
				StartCoroutine(DelayRemove(fK3_Target));
			}
		}

		protected IEnumerator DelayRemove(FK3_Target target)
		{
			yield return null;
			if (allTargets.Contains(target))
			{
				allTargets.Remove(target);
			}
		}

		protected bool IsInLayerMask(GameObject obj)
		{
			LayerMask layerMask = 1 << obj.layer;
			LayerMask targetLayers = base.targetLayers;
			return (targetLayers.value & layerMask.value) != 0;
		}
	}
}
