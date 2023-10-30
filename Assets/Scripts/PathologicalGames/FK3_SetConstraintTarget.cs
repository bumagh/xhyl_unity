using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Utility - Set Unity Constraint FK3_Target")]
	public class FK3_SetConstraintTarget : MonoBehaviour
	{
		public FK3_ConstraintBaseClass unityConstraint;

		[SerializeField]
		protected Component _targetSource;

		protected FK3_EventFireController fireCtrl;

		protected FK3_EventTrigger eventTrigger;

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
				fireCtrl = (value as FK3_EventFireController);
				if (fireCtrl != null)
				{
					_targetSource = fireCtrl;
					fireCtrl.AddOnTargetUpdateDelegate(OnFireCtrlTargetUpdate);
					fireCtrl.AddOnIdleUpdateDelegate(OnOnFireCtrlIdleUpdate);
					return;
				}
				eventTrigger = (value as FK3_EventTrigger);
				if (eventTrigger != null)
				{
					_targetSource = eventTrigger;
					eventTrigger.AddOnListenUpdateDelegate(OnEventTriggerListenStart);
					return;
				}
				throw new InvalidCastException("Component not a supported type. Use a FireController or FK3_EventTrigger.");
			}
		}

		protected void Awake()
		{
			if (unityConstraint == null)
			{
				unityConstraint = GetComponent<FK3_ConstraintBaseClass>();
				if (unityConstraint == null)
				{
					throw new MissingComponentException($"No UnityConstraint was found on GameObject '{base.name}'");
				}
			}
			if (_targetSource == null)
			{
				FK3_EventFireController component = GetComponent<FK3_EventFireController>();
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

		protected void OnFireCtrlTargetUpdate(List<FK3_Target> targets)
		{
			FK3_ConstraintBaseClass fK3_ConstraintBaseClass = unityConstraint;
			FK3_Target fK3_Target = targets[0];
			fK3_ConstraintBaseClass.target = fK3_Target.transform;
		}

		protected void OnOnFireCtrlIdleUpdate()
		{
			unityConstraint.target = null;
		}
	}
}
