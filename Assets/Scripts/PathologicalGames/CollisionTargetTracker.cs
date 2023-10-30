using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Collision TargetTracker")]
	public class CollisionTargetTracker : TargetTracker
	{
		protected TargetList allTargets = new TargetList();

		public Collider coll;

		public Collider2D coll2D;

		public override TargetList targets
		{
			get
			{
				_targets.Clear();
				if (numberOfTargets == 0)
				{
					return _targets;
				}
				List<Target> list = new List<Target>(allTargets);
				foreach (Target allTarget in allTargets)
				{
					Target current = allTarget;
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
				Target target = new Target(otherGo.transform, this);
				if (!(target == Target.Null) && target.targetable.isTargetable && !IsIgnoreTargetOnNewDetectedDelegatesExecute(target) && !allTargets.Contains(target))
				{
					allTargets.Add(target);
				}
			}
		}

		protected void OnCollisionExitAny(GameObject otherGO)
		{
			Target target = default(Target);
			foreach (Target allTarget in allTargets)
			{
				Target current = allTarget;
				if (current.gameObject == otherGO)
				{
					target = current;
				}
			}
			if (!(target == Target.Null))
			{
				StartCoroutine(DelayRemove(target));
			}
		}

		protected IEnumerator DelayRemove(Target target)
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
