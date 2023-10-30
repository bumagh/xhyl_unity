using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/FK3_UnityConstraints/Constraint - Transform - Smooth")]
	public class FK3_SmoothTransformConstraint : FK3_TransformConstraint
	{
		public enum INTERP_OPTIONS_POS
		{
			Linear,
			Damp,
			DampLimited
		}

		public float positionSpeed = 0.1f;

		public float rotationSpeed = 1f;

		public float scaleSpeed = 0.1f;

		public FK3_UnityConstraints.INTERP_OPTIONS interpolation = FK3_UnityConstraints.INTERP_OPTIONS.Spherical;

		public float positionMaxSpeed = 0.1f;

		public INTERP_OPTIONS_POS position_interpolation;

		private Vector3 curDampVelocity = Vector3.zero;

		protected override void OnConstraintUpdate()
		{
			if (constrainScale)
			{
				SetWorldScale(base.target);
			}
			OutputRotationTowards(base.target.rotation);
			OutputPositionTowards(base.target.position);
		}

		protected override void NoTargetDefault()
		{
			if (constrainScale)
			{
				base.transform.localScale = Vector3.one;
			}
			OutputRotationTowards(Quaternion.identity);
			OutputPositionTowards(base.target.position);
		}

		private void OutputPositionTowards(Vector3 destPos)
		{
			if (constrainPosition)
			{
				switch (position_interpolation)
				{
				case INTERP_OPTIONS_POS.DampLimited:
					base.pos = Vector3.SmoothDamp(base.transform.position, destPos, ref curDampVelocity, positionSpeed, positionMaxSpeed);
					break;
				case INTERP_OPTIONS_POS.Damp:
					base.pos = Vector3.SmoothDamp(base.transform.position, destPos, ref curDampVelocity, positionSpeed);
					break;
				case INTERP_OPTIONS_POS.Linear:
					base.pos = Vector3.Lerp(base.transform.position, destPos, positionSpeed);
					break;
				}
				if (!outputPosX)
				{
					ref Vector3 pos = ref base.pos;
					Vector3 position = base.transform.position;
					pos.x = position.x;
				}
				if (!outputPosY)
				{
					ref Vector3 pos2 = ref base.pos;
					Vector3 position2 = base.transform.position;
					pos2.y = position2.y;
				}
				if (!outputPosZ)
				{
					ref Vector3 pos3 = ref base.pos;
					Vector3 position3 = base.transform.position;
					pos3.z = position3.z;
				}
				base.transform.position = base.pos;
			}
		}

		private void OutputRotationTowards(Quaternion destRot)
		{
			if (constrainRotation)
			{
				FK3_UnityConstraints.InterpolateRotationTo(base.transform, destRot, interpolation, rotationSpeed);
				FK3_UnityConstraints.MaskOutputRotations(base.transform, output);
			}
		}

		public override void SetWorldScale(Transform sourceXform)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, GetTargetLocalScale(sourceXform), scaleSpeed);
		}
	}
}
