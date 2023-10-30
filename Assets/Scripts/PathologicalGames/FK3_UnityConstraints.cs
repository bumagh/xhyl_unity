using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("")]
	public static class FK3_UnityConstraints
	{
		public enum MODE_OPTIONS
		{
			Align,
			Constrain
		}

		public enum NO_TARGET_OPTIONS
		{
			Error,
			DoNothing,
			ReturnToDefault,
			SetByScript
		}

		public enum INTERP_OPTIONS
		{
			Linear,
			Spherical,
			SphericalLimited
		}

		public enum OUTPUT_ROT_OPTIONS
		{
			WorldAll,
			WorldX,
			WorldY,
			WorldZ,
			LocalX,
			LocalY,
			LocalZ
		}

		private static float lastRealtimeSinceStartup;

		public static Quaternion GetInterpolateRotationTo(Quaternion currentRot, Quaternion targetRot, INTERP_OPTIONS interpolation, float speed)
		{
			Quaternion result = Quaternion.identity;
			float deltaTime = Time.deltaTime;
			switch (interpolation)
			{
			case INTERP_OPTIONS.SphericalLimited:
				result = Quaternion.RotateTowards(currentRot, targetRot, speed * Time.timeScale);
				break;
			case INTERP_OPTIONS.Spherical:
				result = Quaternion.Slerp(currentRot, targetRot, deltaTime * speed);
				break;
			case INTERP_OPTIONS.Linear:
				result = Quaternion.Lerp(currentRot, targetRot, deltaTime * speed);
				break;
			}
			return result;
		}

		public static void InterpolateRotationTo(Transform xform, Quaternion targetRot, INTERP_OPTIONS interpolation, float speed)
		{
			xform.rotation = GetInterpolateRotationTo(xform.rotation, targetRot, interpolation, speed);
		}

		public static void InterpolateLocalRotationTo(Transform xform, Quaternion targetRot, INTERP_OPTIONS interpolation, float speed)
		{
			xform.localRotation = GetInterpolateRotationTo(xform.localRotation, targetRot, interpolation, speed);
		}

		public static void MaskOutputRotations(Transform xform, OUTPUT_ROT_OPTIONS option)
		{
			switch (option)
			{
			case OUTPUT_ROT_OPTIONS.WorldX:
			{
				Vector3 eulerAngles3 = xform.eulerAngles;
				eulerAngles3.y = 0f;
				eulerAngles3.z = 0f;
				xform.eulerAngles = eulerAngles3;
				break;
			}
			case OUTPUT_ROT_OPTIONS.WorldY:
			{
				Vector3 eulerAngles2 = xform.eulerAngles;
				eulerAngles2.x = 0f;
				eulerAngles2.z = 0f;
				xform.eulerAngles = eulerAngles2;
				break;
			}
			case OUTPUT_ROT_OPTIONS.WorldZ:
			{
				Vector3 eulerAngles = xform.eulerAngles;
				eulerAngles.x = 0f;
				eulerAngles.y = 0f;
				xform.eulerAngles = eulerAngles;
				break;
			}
			case OUTPUT_ROT_OPTIONS.LocalX:
			{
				Vector3 localEulerAngles3 = xform.localEulerAngles;
				localEulerAngles3.y = 0f;
				localEulerAngles3.z = 0f;
				xform.localEulerAngles = localEulerAngles3;
				break;
			}
			case OUTPUT_ROT_OPTIONS.LocalY:
			{
				Vector3 localEulerAngles2 = xform.localEulerAngles;
				localEulerAngles2.x = 0f;
				localEulerAngles2.z = 0f;
				xform.localEulerAngles = localEulerAngles2;
				break;
			}
			case OUTPUT_ROT_OPTIONS.LocalZ:
			{
				Vector3 localEulerAngles = xform.localEulerAngles;
				localEulerAngles.x = 0f;
				localEulerAngles.y = 0f;
				xform.localEulerAngles = localEulerAngles;
				break;
			}
			}
		}
	}
}
