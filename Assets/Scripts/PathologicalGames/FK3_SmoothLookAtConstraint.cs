using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/FK3_UnityConstraints/Constraint - Look At - Smooth")]
	public class FK3_SmoothLookAtConstraint : FK3_LookAtConstraint
	{
		public FK3_UnityConstraints.INTERP_OPTIONS interpolation = FK3_UnityConstraints.INTERP_OPTIONS.Spherical;

		public float speed = 1f;

		public FK3_UnityConstraints.OUTPUT_ROT_OPTIONS output;

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
			FK3_UnityConstraints.InterpolateLocalRotationTo(base.transform, Quaternion.identity, interpolation, speed);
		}

		private void OutputTowards(Quaternion destRot)
		{
			FK3_UnityConstraints.InterpolateRotationTo(base.transform, destRot, interpolation, speed);
			FK3_UnityConstraints.MaskOutputRotations(base.transform, output);
		}
	}
}
