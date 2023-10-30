using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/UnityConstraints/Constraint - Look At - Smooth")]
	public class SmoothLookAtConstraint : LookAtConstraint
	{
		public UnityConstraints.INTERP_OPTIONS interpolation = UnityConstraints.INTERP_OPTIONS.Spherical;

		public float speed = 1f;

		public UnityConstraints.OUTPUT_ROT_OPTIONS output;

		private Quaternion lookRot;

		private Quaternion usrLookRot;

		private Quaternion curRot;

		private Vector3 angles;

		private Vector3 lookVectCache;

		protected override void OnConstraintUpdate()
		{
			lookVectCache = Vector3.zero;
			lookVectCache = lookVect;
			if (!(lookVectCache == Vector3.zero))
			{
				lookRot = Quaternion.LookRotation(lookVectCache, base.upVect);
				usrLookRot = GetUserLookRotation(lookRot);
				OutputTowards(usrLookRot);
			}
		}

		protected override void NoTargetDefault()
		{
			UnityConstraints.InterpolateLocalRotationTo(base.transform, Quaternion.identity, interpolation, speed);
		}

		private void OutputTowards(Quaternion destRot)
		{
			UnityConstraints.InterpolateRotationTo(base.transform, destRot, interpolation, speed);
			UnityConstraints.MaskOutputRotations(base.transform, output);
		}
	}
}
