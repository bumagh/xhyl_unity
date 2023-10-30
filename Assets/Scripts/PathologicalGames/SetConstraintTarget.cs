using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Utility - Set Unity Constraint Target")]
	public class SetConstraintTarget : MonoBehaviour
	{
		public ConstraintBaseClass unityConstraint;

		[SerializeField]
		protected Component _targetSource;

		protected EventFireController fireCtrl;

		protected EventTrigger eventTrigger;

		public Component targetSource
		{
			get
			{
				return _targetSource;
			}
			set
			{
				if (fireCtrl != null)
				{
					fireCtrl.RemoveOnTargetUpdateDelegate(OnFireCtrlTargetUpdate);
					fireCtrl.RemoveOnIdleUpdateDelegate(OnOnFireCtrlIdleUpdate);
				}
				if (eventTrigger != null)
				{
					eventTrigger.RemoveOnListenUpdateDelegate(OnEventTriggerListenStart);
				}
				_targetSource = null;
				eventTrigger = null;
				fireCtrl = null;
				if (value == null)
				{
					return;
				}
				fireCtrl = (value as EventFireController);
				if (fireCtrl != null)
				{
					_targetSource = fireCtrl;
					fireCtrl.AddOnTargetUpdateDelegate(OnFireCtrlTargetUpdate);
					fireCtrl.AddOnIdleUpdateDelegate(OnOnFireCtrlIdleUpdate);
					return;
				}
				eventTrigger = (value as EventTrigger);
				if (eventTrigger != null)
				{
					_targetSource = eventTrigger;
					eventTrigger.AddOnListenUpdateDelegate(OnEventTriggerListenStart);
					return;
				}
				throw new InvalidCastException("Component not a supported type. Use a FireController or EventTrigger.");
			}
		}

		protected void Awake()
		{
			if (unityConstraint == null)
			{
				unityConstraint = GetComponent<ConstraintBaseClass>();
				if (unityConstraint == null)
				{
					throw new MissingComponentException($"No UnityConstraint was found on GameObject '{base.name}'");
				}
			}
			if (_targetSource == null)
			{
				EventFireController component = GetComponent<EventFireController>();
				if (component != null)
				{
					targetSource = component;
				}
			}
			else
			{
				targetSource = _targetSource;
			}
		}

		protected void OnEventTriggerListenStart()
		{
			if (eventTrigger.target.isSpawned)
			{
				unityConstraint.target = eventTrigger.target.transform;
			}
			else
			{
				unityConstraint.target = null;
			}
		}

		protected void OnFireCtrlTargetUpdate(List<Target> targets)
		{
			ConstraintBaseClass constraintBaseClass = unityConstraint;
			Target target = targets[0];
			constraintBaseClass.target = target.transform;
		}

		protected void OnOnFireCtrlIdleUpdate()
		{
			unityConstraint.target = null;
		}
	}
}
