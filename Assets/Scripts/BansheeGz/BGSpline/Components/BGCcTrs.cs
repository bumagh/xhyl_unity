using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcTrs")]
	[CcDescriptor(Description = "Translate + rotate + scale an object with one single component. It's 5 components in one (Cursor+CursorChangeLinear+MoveByCursor+RotateByCursor+ScaleByCursor) with basic functionality", Name = "TRS", Icon = "BGCcTrs123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcTrs")]
	public class BGCcTrs : BGCcCursor
	{
		public enum OverflowControlEnum
		{
			Cycle,
			PingPong,
			Stop
		}

		public enum CursorChangeModeEnum
		{
			Constant,
			LinearField,
			LinearFieldInterpolate
		}

		public enum RotationInterpolationEnum
		{
			None,
			Lerp,
			Slerp
		}

		public enum RotationUpEnum
		{
			WorldUp,
			WorldDown,
			WorldRight,
			WorldLeft,
			WorldForward,
			WorldBack
		}

		[Tooltip("Object to manipulate.\r\n")]
		[SerializeField]
		private Transform objectToManipulate;

		[SerializeField]
		[Tooltip("Modes for changing cursor position.\n1)Constant- speed value is constant.\n2)LinearField- each point has its own speed value.\n3)LinearFieldInterpolate- each point has its own speed value and the final speed is linear interpolation based on the distance between 2 points values")]
		private CursorChangeModeEnum cursorChangeMode;

		[Tooltip("Constant movement speed along the curve (Speed * Time.deltaTime). You can override this value for each point with speedField")]
		[SerializeField]
		private float speed = 5f;

		[Tooltip("Field to store the speed between each point. It should be a float field.")]
		[SerializeField]
		private BGCurvePointField speedField;

		[Tooltip("Cursor will be moved in FixedUpdate instead of Update")]
		[SerializeField]
		private bool useFixedUpdate;

		[SerializeField]
		[Tooltip("How to change speed, when curve reaches the end.")]
		private OverflowControlEnum overflowControl;

		[SerializeField]
		[Tooltip("Object should be translated.\r\n")]
		private bool moveObject = true;

		[SerializeField]
		[Tooltip("Object should be rotated.\r\n")]
		private bool rotateObject;

		[SerializeField]
		[Tooltip("Rotation interpolation mode.\r\n")]
		private RotationInterpolationEnum rotationInterpolation;

		[Tooltip("Rotation Lerp rotationSpeed. (Quaternion.Lerp(from,to, lerpSpeed * Time.deltaTime)) ")]
		[SerializeField]
		private float lerpSpeed = 5f;

		[Tooltip("Rotation Slerp rotationSpeed. (Quaternion.Slerp(from,to, slerpSpeed * Time.deltaTime)) ")]
		[SerializeField]
		private float slerpSpeed = 5f;

		[Tooltip("Angle to add to final result.")]
		[SerializeField]
		private Vector3 offsetAngle;

		[Tooltip("Up vector to be used with Quaternion.LookRotation to determine rotation")]
		[SerializeField]
		private RotationUpEnum upVector;

		[Tooltip("Field to store the rotation between each point. It should be a Quaternion field.")]
		[SerializeField]
		private BGCurvePointField rotationField;

		[SerializeField]
		[Tooltip("Object should be scaled.\r\n")]
		private bool scaleObject;

		[SerializeField]
		[Tooltip("Field to store the scale value at points. It should be a Vector3 field.")]
		private BGCurvePointField scaleField;

		public bool SpeedIsReversed
		{
			get;
			set;
		}

		public override string Error
		{
			get
			{
				if (objectToManipulate == null)
				{
					return "Object To Manipulate is not set.";
				}
				CursorChangeModeEnum cursorChangeModeEnum = cursorChangeMode;
				if (cursorChangeModeEnum == CursorChangeModeEnum.LinearField || cursorChangeModeEnum == CursorChangeModeEnum.LinearFieldInterpolate)
				{
					if (speedField == null)
					{
						return "Speed field is not set.";
					}
					if (speedField.Type != BGCurvePointField.TypeEnum.Float)
					{
						return "Speed field should have float type.";
					}
				}
				if (rotateObject)
				{
					BGCcMath math = base.Math;
					if (math == null || !math.IsCalculated(BGCurveBaseMath.Field.Tangent))
					{
						return "Math does not calculate tangents.";
					}
					if (rotationField != null && rotationField.Type != BGCurvePointField.TypeEnum.Quaternion)
					{
						return "Rotate field should have Quaternion type.";
					}
				}
				if (scaleObject)
				{
					if (scaleField == null)
					{
						return "Scale field is not set.";
					}
					if (scaleField.Type != BGCurvePointField.TypeEnum.Vector3)
					{
						return "Scale field should have Vector3 type.";
					}
				}
				return null;
			}
		}

		public OverflowControlEnum OverflowControl
		{
			get
			{
				return overflowControl;
			}
			set
			{
				ParamChanged(ref overflowControl, value);
			}
		}

		public CursorChangeModeEnum CursorChangeMode
		{
			get
			{
				return cursorChangeMode;
			}
			set
			{
				if (cursorChangeMode != value)
				{
					cursorChangeMode = value;
					FireChangedParams();
				}
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				ParamChanged(ref speed, value);
			}
		}

		public BGCurvePointField SpeedField
		{
			get
			{
				return speedField;
			}
			set
			{
				ParamChanged(ref speedField, value);
			}
		}

		public Transform ObjectToManipulate
		{
			get
			{
				return objectToManipulate;
			}
			set
			{
				ParamChanged(ref objectToManipulate, value);
			}
		}

		public bool UseFixedUpdate
		{
			get
			{
				return useFixedUpdate;
			}
			set
			{
				ParamChanged(ref useFixedUpdate, value);
			}
		}

		public bool MoveObject
		{
			get
			{
				return moveObject;
			}
			set
			{
				ParamChanged(ref moveObject, value);
			}
		}

		public bool RotateObject
		{
			get
			{
				return rotateObject;
			}
			set
			{
				ParamChanged(ref rotateObject, value);
			}
		}

		public bool ScaleObject
		{
			get
			{
				return scaleObject;
			}
			set
			{
				ParamChanged(ref scaleObject, value);
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

		public RotationUpEnum UpVector
		{
			get
			{
				return upVector;
			}
			set
			{
				ParamChanged(ref upVector, value);
			}
		}

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

		public BGCurvePointField ScaleField
		{
			get
			{
				return scaleField;
			}
			set
			{
				ParamChanged(ref scaleField, value);
			}
		}

		private void Update()
		{
			if (!useFixedUpdate)
			{
				Step();
			}
		}

		private void FixedUpdate()
		{
			if (useFixedUpdate)
			{
				Step();
			}
		}

		private void Step()
		{
			if ((cursorChangeMode == CursorChangeModeEnum.Constant && System.Math.Abs(speed) < 1E-05f) || base.Curve.PointsCount < 2 || Error != null)
			{
				return;
			}
			int sectionIndex = -1;
			float num;
			switch (cursorChangeMode)
			{
			case CursorChangeModeEnum.Constant:
				num = speed * Time.deltaTime;
				break;
			case CursorChangeModeEnum.LinearField:
				sectionIndex = CalculateSectionIndex();
				num = base.Curve[sectionIndex].GetFloat(speedField.FieldName) * Time.deltaTime;
				break;
			case CursorChangeModeEnum.LinearFieldInterpolate:
				FillInterpolationInfo(ref sectionIndex, out BGCurvePointI fromPoint, out BGCurvePointI toPoint, out float ratio);
				num = Mathf.Lerp(fromPoint.GetFloat(speedField.FieldName), toPoint.GetFloat(speedField.FieldName), ratio) * Time.deltaTime;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (SpeedIsReversed)
			{
				num = 0f - num;
			}
			base.distance += num;
			if (base.distance < 0f)
			{
				switch (overflowControl)
				{
				case OverflowControlEnum.Cycle:
					base.distance = base.Math.GetDistance();
					break;
				case OverflowControlEnum.PingPong:
					SpeedIsReversed = !SpeedIsReversed;
					base.distance = 0f;
					break;
				case OverflowControlEnum.Stop:
					Speed = 0f;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				float distance = base.Math.GetDistance();
				if (base.distance > distance)
				{
					switch (overflowControl)
					{
					case OverflowControlEnum.Cycle:
						base.distance = 0f;
						break;
					case OverflowControlEnum.PingPong:
						SpeedIsReversed = !SpeedIsReversed;
						base.distance = distance;
						break;
					case OverflowControlEnum.Stop:
						Speed = 0f;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
			Trs(sectionIndex);
		}

		public void Trs(int sectionIndex = -1)
		{
			if (objectToManipulate == null)
			{
				return;
			}
			if (moveObject)
			{
				objectToManipulate.position = base.Math.CalcPositionByDistance(distance);
			}
			if (rotateObject)
			{
				Quaternion lhs;
				if (rotationField == null)
				{
					Vector3 upwards;
					switch (upVector)
					{
					case RotationUpEnum.WorldUp:
						upwards = Vector3.up;
						break;
					case RotationUpEnum.WorldDown:
						upwards = Vector3.down;
						break;
					case RotationUpEnum.WorldRight:
						upwards = Vector3.right;
						break;
					case RotationUpEnum.WorldLeft:
						upwards = Vector3.left;
						break;
					case RotationUpEnum.WorldForward:
						upwards = Vector3.forward;
						break;
					case RotationUpEnum.WorldBack:
						upwards = Vector3.back;
						break;
					default:
						throw new ArgumentOutOfRangeException("upVector");
					}
					lhs = Quaternion.LookRotation(CalculateTangent(), upwards);
				}
				else
				{
					lhs = LerpQuaternion(ref sectionIndex, rotationField.FieldName);
				}
				if (lhs.x != 0f || lhs.y != 0f || lhs.z != 0f || lhs.w != 0f)
				{
					lhs *= Quaternion.Euler(offsetAngle);
					switch (rotationInterpolation)
					{
					case RotationInterpolationEnum.None:
						ObjectToManipulate.rotation = lhs;
						break;
					case RotationInterpolationEnum.Lerp:
						ObjectToManipulate.rotation = Quaternion.Lerp(ObjectToManipulate.rotation, lhs, lerpSpeed * Time.deltaTime);
						break;
					case RotationInterpolationEnum.Slerp:
						ObjectToManipulate.rotation = Quaternion.Slerp(ObjectToManipulate.rotation, lhs, slerpSpeed * Time.deltaTime);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
			if (scaleObject && scaleField != null)
			{
				ObjectToManipulate.localScale = LerpVector3(ref sectionIndex, scaleField.FieldName);
			}
		}

		private Vector3 LerpVector3(ref int sectionIndex, string fieldName)
		{
			FillInterpolationInfo(ref sectionIndex, out BGCurvePointI fromPoint, out BGCurvePointI toPoint, out float ratio);
			return Vector3.Lerp(fromPoint.GetVector3(fieldName), toPoint.GetVector3(fieldName), ratio);
		}

		private Quaternion LerpQuaternion(ref int sectionIndex, string fieldName)
		{
			FillInterpolationInfo(ref sectionIndex, out BGCurvePointI fromPoint, out BGCurvePointI toPoint, out float ratio);
			return Quaternion.Lerp(fromPoint.GetQuaternion(fieldName), toPoint.GetQuaternion(fieldName), ratio);
		}

		private void FillInterpolationInfo(ref int sectionIndex, out BGCurvePointI fromPoint, out BGCurvePointI toPoint, out float ratio)
		{
			if (sectionIndex == -1)
			{
				sectionIndex = CalculateSectionIndex();
			}
			fromPoint = base.Curve[sectionIndex];
			toPoint = ((sectionIndex != base.Curve.PointsCount - 1) ? base.Curve[sectionIndex + 1] : base.Curve[0]);
			BGCurveBaseMath.SectionInfo sectionInfo = base.Math[sectionIndex];
			ratio = (base.Distance - sectionInfo.DistanceFromStartToOrigin) / (sectionInfo.DistanceFromEndToOrigin - sectionInfo.DistanceFromStartToOrigin);
		}

		private void OnDrawGizmosSelected()
		{
			if (!Application.isPlaying && !(objectToManipulate == null))
			{
				Trs();
			}
		}
	}
}
