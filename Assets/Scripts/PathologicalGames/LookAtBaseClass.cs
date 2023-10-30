using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("")]
	public class LookAtBaseClass : ConstraintBaseClass
	{
		public Vector3 pointAxis = -Vector3.back;

		public Vector3 upAxis = Vector3.up;

		protected override Transform internalTarget
		{
			get
			{
				if (_internalTarget != null)
				{
					return _internalTarget;
				}
				Transform internalTarget = base.internalTarget;
				internalTarget.position = base.transform.rotation * pointAxis + base.transform.position;
				return _internalTarget;
			}
		}

		protected override void NoTargetDefault()
		{
			base.transform.rotation = Quaternion.identity;
		}

		protected Quaternion GetUserLookRotation(Quaternion lookRot)
		{
			Quaternion rotation = Quaternion.LookRotation(pointAxis, upAxis);
			return lookRot * Quaternion.Inverse(rotation);
		}
	}
}
