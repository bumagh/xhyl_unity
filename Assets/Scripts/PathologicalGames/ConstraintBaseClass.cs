using System.Collections;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("")]
	public class ConstraintBaseClass : ConstraintFrameworkBaseClass
	{
		public Transform _target;

		protected Transform _internalTarget;

		public UnityConstraints.NO_TARGET_OPTIONS _noTargetMode = UnityConstraints.NO_TARGET_OPTIONS.DoNothing;

		public UnityConstraints.MODE_OPTIONS _mode = UnityConstraints.MODE_OPTIONS.Constrain;

		protected Vector3 pos;

		protected Vector3 scl;

		protected Quaternion rot;

		public Transform target
		{
			get
			{
				if (noTargetMode == UnityConstraints.NO_TARGET_OPTIONS.SetByScript)
				{
					return internalTarget;
				}
				return _target;
			}
			set
			{
				_target = value;
			}
		}

		protected virtual Transform internalTarget
		{
			get
			{
				if (_internalTarget != null)
				{
					return _internalTarget;
				}
				_internalTarget = new GameObject(base.name + "_InternalConstraintTarget")
				{
					hideFlags = HideFlags.HideInHierarchy
				}.transform;
				_internalTarget.position = base.transform.position;
				_internalTarget.rotation = base.transform.rotation;
				_internalTarget.localScale = base.transform.localScale;
				return _internalTarget;
			}
		}

		public Vector3 position
		{
			get
			{
				return internalTarget.position;
			}
			set
			{
				internalTarget.position = value;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return internalTarget.rotation;
			}
			set
			{
				internalTarget.rotation = value;
			}
		}

		public Vector3 scale
		{
			get
			{
				return internalTarget.localScale;
			}
			set
			{
				internalTarget.localScale = value;
			}
		}

		public UnityConstraints.NO_TARGET_OPTIONS noTargetMode
		{
			get
			{
				return _noTargetMode;
			}
			set
			{
				_noTargetMode = value;
			}
		}

		public UnityConstraints.MODE_OPTIONS mode
		{
			get
			{
				return _mode;
			}
			set
			{
				_mode = value;
				InitConstraint();
			}
		}

		private void OnDestroy()
		{
			if (_internalTarget != null)
			{
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(_internalTarget.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(_internalTarget.gameObject);
				}
			}
		}

		protected sealed override void InitConstraint()
		{
			switch (mode)
			{
			case UnityConstraints.MODE_OPTIONS.Constrain:
				StartCoroutine("Constrain");
				break;
			case UnityConstraints.MODE_OPTIONS.Align:
				OnOnce();
				break;
			}
		}

		protected sealed override IEnumerator Constrain()
		{
			while (mode == UnityConstraints.MODE_OPTIONS.Constrain)
			{
				if (target == null)
				{
					switch (noTargetMode)
					{
					case UnityConstraints.NO_TARGET_OPTIONS.Error:
					{
						string msg = $"No target provided. \n{GetType().Name} on {base.transform.name}";
						UnityEngine.Debug.LogError(msg);
						yield return null;
						continue;
					}
					case UnityConstraints.NO_TARGET_OPTIONS.DoNothing:
						yield return null;
						continue;
					case UnityConstraints.NO_TARGET_OPTIONS.ReturnToDefault:
						NoTargetDefault();
						yield return null;
						continue;
					}
				}
				if (!(target == null))
				{
					OnConstraintUpdate();
					yield return null;
				}
			}
		}

		protected virtual void NoTargetDefault()
		{
		}

		private void OnOnce()
		{
			OnConstraintUpdate();
		}
	}
}
