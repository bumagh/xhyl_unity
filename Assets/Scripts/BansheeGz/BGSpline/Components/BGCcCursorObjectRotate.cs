using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcRotateObject")]
	[ExecuteInEditMode]
	[CcDescriptor(Description = "Align the object's rotation with curve's tangent or 'rotation' field values at the point, the Cursor provides.", Name = "Rotate Object By Cursor", Icon = "BGCcCursorObjectRotate123")]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCursorObjectRotate")]
	public class BGCcCursorObjectRotate : BGCcWithCursorObject
	{
		public enum RotationInterpolationEnum
		{
			None,
			Lerp,
			Slerp
		}

		public enum RotationUpEnum
		{
			WorldUp,
			WorldCustom,
			LocalUp,
			LocalCustom,
			TargetParentUp,
			TargetParentUpCustom
		}

		[Tooltip("Rotation interpolation mode.")]
		[SerializeField]
		private RotationInterpolationEnum rotationInterpolation;

		[SerializeField]
		[Tooltip("Rotation Lerp rotationSpeed. (Quaternion.Lerp(from,to, lerpSpeed * Time.deltaTime)) ")]
		private float lerpSpeed = 5f;

		[Tooltip("Rotation Slerp rotationSpeed. (Quaternion.Slerp(from,to, slerpSpeed * Time.deltaTime)) ")]
		[SerializeField]
		private float slerpSpeed = 5f;

		[Tooltip("Angle to add to final result.")]
		[SerializeField]
		private Vector3 offsetAngle;

		[Tooltip("Up mode for tangent Quaternion.LookRotation. It's used only if rotationField is not assigned.\r\n1) WorldUp - use Vector.up in world coordinates\r\n2) WorldCustom - use custom Vector in world coordinates\r\n3) LocalUp - use Vector.up in local coordinates \r\n4) LocalCustom - use custom Vector in local coordinates\r\n5) TargetParentUp - use Vector.up in target object parent's local coordinates\r\n6) TargetParentUpCustom- use custom Vector in target object parent's local coordinates")]
		[SerializeField]
		private RotationUpEnum upMode;

		[Tooltip("Custom Up vector for tangent Quaternion.LookRotation. It's used only if rotationField is not assigned.")]
		[SerializeField]
		private Vector3 upCustom = Vector3.up;

		[SerializeField]
		[Tooltip("Field to store the rotation between each point. It should be a Quaternion field.")]
		private BGCurvePointField rotationField;

		[SerializeField]
		[Tooltip("Additional 360 degree revolutions around tangent. It's used only if rotationField is assigned. It can be overriden with 'int' revolutionsAroundTangentField field.")]
		private int revolutionsAroundTangent;

		[Tooltip("Field to store additional 360 degree revolutions around tangent for each point. It's used only if rotationField is assigned. It should be an int field.")]
		[SerializeField]
		private BGCurvePointField revolutionsAroundTangentField;

		[SerializeField]
		[Tooltip("By default revolutions around tangent is counter-clockwise. Set it to true to reverse direction. It's used only if rotationField is assigned.It can be overriden with bool field")]
		private bool revolutionsClockwise;

		[SerializeField]
		[Tooltip("Field to store direction for revolutions around tangent. It should be an bool field.  It's used only if rotationField is assigned.")]
		private BGCurvePointField revolutionsClockwiseField;

		private Quaternion rotation = Quaternion.identity;

		public RotationInterpolationEnum RotationInterpolation
		{
			get
			{
				return rotationInterpolation;
			}
			set
			{
				ParamChanged(ref rotationInterpolation, value);
			}
		}

		public float LerpSpeed
		{
			get
			{
				return lerpSpeed;
			}
			set
			{
				ParamChanged(ref lerpSpeed, value);
			}
		}

		public float SlerpSpeed
		{
			get
			{
				return slerpSpeed;
			}
			set
			{
				ParamChanged(ref slerpSpeed, value);
			}
		}

		public Vector3 UpCustom
		{
			get
			{
				return upCustom;
			}
			set
			{
				ParamChanged(ref upCustom, value);
			}
		}

		public RotationUpEnum UpMode
		{
			get
			{
				return upMode;
			}
			set
			{
				ParamChanged(ref upMode, value);
			}
		}

		public BGCurvePointField RotationField
		{
			get
			{
				return rotationField;
			}
			set
			{
				ParamChanged(ref rotationField, value);
			}
		}

		public BGCurvePointField RevolutionsAroundTangentField
		{
			get
			{
				return revolutionsAroundTangentField;
			}
			set
			{
				ParamChanged(ref revolutionsAroundTangentField, value);
			}
		}

		public int RevolutionsAroundTangent
		{
			get
			{
				return revolutionsAroundTangent;
			}
			set
			{
				ParamChanged(ref revolutionsAroundTangent, value);
			}
		}

		public BGCurvePointField RevolutionsClockwiseField
		{
			get
			{
				return revolutionsClockwiseField;
			}
			set
			{
				ParamChanged(ref revolutionsClockwiseField, value);
			}
		}

		public bool RevolutionsClockwise
		{
			get
			{
				return revolutionsClockwise;
			}
			set
			{
				ParamChanged(ref revolutionsClockwise, value);
			}
		}

		public Vector3 OffsetAngle
		{
			get
			{
				return offsetAngle;
			}
			set
			{
				ParamChanged(ref offsetAngle, value);
			}
		}

		public override string Error => ChoseMessage(base.Error, delegate
		{
			if (!base.Cursor.Math.IsCalculated(BGCurveBaseMath.Field.Tangent))
			{
				if (rotationField == null)
				{
					return "Math should calculate tangents if rotation field is null.";
				}
				if (RevolutionsAroundTangent != 0 || RevolutionsAroundTangentField != null)
				{
					return "Math should calculate tangents if revolutions are used.";
				}
			}
			return null;
		});

		public override string Warning => (!(rotationField == null) || (upMode != RotationUpEnum.TargetParentUp && upMode != RotationUpEnum.TargetParentUpCustom) || !(base.ObjectToManipulate != null) || !(base.ObjectToManipulate.parent == null)) ? null : ("Up Mode is set to " + upMode + ", however object's parent is null");

		public override bool SupportHandles => true;

		public override bool SupportHandlesSettings => true;

		public Quaternion Rotation => rotation;

		public event EventHandler ChangedObjectRotation;

		public void ForceUpdate()
		{
			Update();
		}

		private void Update()
		{
			if (base.Curve.PointsCount == 0)
			{
				return;
			}
			Transform objectToManipulate = base.ObjectToManipulate;
			if (!(objectToManipulate == null) && TryToCalculateRotation(ref rotation))
			{
				objectToManipulate.rotation = rotation;
				if (this.ChangedObjectRotation != null)
				{
					this.ChangedObjectRotation(this, null);
				}
			}
		}

		public bool TryToCalculateRotation(ref Quaternion result)
		{
			int pointsCount = base.Curve.PointsCount;
			if (pointsCount == 0)
			{
				return false;
			}
			BGCcCursor cursor = base.Cursor;
			BGCcMath math = cursor.Math;
			if (rotationField == null)
			{
				if (math == null || !math.IsCalculated(BGCurveBaseMath.Field.Tangent))
				{
					return false;
				}
				if (pointsCount == 1)
				{
					result = Quaternion.identity;
				}
				else
				{
					Vector3 vector = cursor.CalculateTangent();
					if ((double)Vector3.SqrMagnitude(vector) < 0.01)
					{
						return false;
					}
					Vector3 upwards;
					switch (upMode)
					{
					case RotationUpEnum.WorldUp:
						upwards = Vector3.up;
						break;
					case RotationUpEnum.WorldCustom:
						upwards = upCustom;
						break;
					case RotationUpEnum.LocalUp:
						upwards = base.transform.InverseTransformDirection(Vector3.up);
						break;
					case RotationUpEnum.LocalCustom:
						upwards = base.transform.InverseTransformDirection(upCustom);
						break;
					case RotationUpEnum.TargetParentUp:
					case RotationUpEnum.TargetParentUpCustom:
					{
						Transform objectToManipulate = base.ObjectToManipulate;
						upwards = ((!(objectToManipulate.parent != null)) ? ((upMode != RotationUpEnum.TargetParentUp) ? upCustom : Vector3.up) : objectToManipulate.parent.InverseTransformDirection((upMode != RotationUpEnum.TargetParentUp) ? upCustom : Vector3.up));
						break;
					}
					default:
						throw new Exception("Unsupported upMode:" + upMode);
					}
					result = Quaternion.LookRotation(vector, upwards);
				}
			}
			else if (pointsCount == 1)
			{
				result = base.Curve[0].GetQuaternion(rotationField.FieldName);
			}
			else if (revolutionsAroundTangentField == null && revolutionsAroundTangent == 0)
			{
				result = LerpQuaternion(rotationField.FieldName);
			}
			else
			{
				int num = (!(revolutionsAroundTangentField != null) && !(revolutionsClockwiseField != null)) ? (-1) : cursor.CalculateSectionIndex();
				result = LerpQuaternion(rotationField.FieldName, num);
				int num2 = Mathf.Clamp((!(revolutionsAroundTangentField != null)) ? revolutionsAroundTangent : base.Curve[num].GetInt(revolutionsAroundTangentField.FieldName), 0, int.MaxValue);
				if (num2 > 0 && math.IsCalculated(BGCurveBaseMath.Field.Tangent))
				{
					Vector3 vector2 = cursor.CalculateTangent();
					if ((double)Vector3.SqrMagnitude(vector2) > 0.01)
					{
						int num3 = 360 * num2;
						if ((!(revolutionsClockwiseField != null)) ? revolutionsClockwise : base.Curve[num].GetBool(revolutionsClockwiseField.FieldName))
						{
							num3 = -num3;
						}
						int indexFrom;
						int indexTo;
						float t = GetT(out indexFrom, out indexTo, num);
						float angle = Mathf.Lerp(0f, num3, t);
						result *= Quaternion.AngleAxis(angle, vector2);
					}
				}
			}
			result *= Quaternion.Euler(offsetAngle);
			switch (rotationInterpolation)
			{
			case RotationInterpolationEnum.Lerp:
				result = Quaternion.Lerp(base.ObjectToManipulate.rotation, rotation, lerpSpeed * Time.deltaTime);
				break;
			case RotationInterpolationEnum.Slerp:
				result = Quaternion.Slerp(base.ObjectToManipulate.rotation, rotation, slerpSpeed * Time.deltaTime);
				break;
			}
			return true;
		}
	}
}
