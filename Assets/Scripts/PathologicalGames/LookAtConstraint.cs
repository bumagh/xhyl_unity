using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/UnityConstraints/Constraint - Look At")]
	public class LookAtConstraint : LookAtBaseClass
	{
		public Transform upTarget;

		protected virtual Vector3 lookVect => base.target.position - base.transform.position;

		protected Vector3 upVect
		{
			get
			{
				if (upTarget == null)
				{
					return Vector3.up;
				}
				return upTarget.position - base.transform.position;
			}
		}

		protected override void OnConstraintUpdate()
		{
			Quaternion lookRot = Quaternion.LookRotation(lookVect, upVect);
			base.transform.rotation = GetUserLookRotation(lookRot);
		}
	}
}
