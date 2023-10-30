using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[DisallowMultipleComponent]
	public class BGCurvePointGO : MonoBehaviour, BGCurvePointI
	{
		[SerializeField]
		private BGCurvePoint.ControlTypeEnum controlType;

		[SerializeField]
		private Vector3 positionLocal;

		[SerializeField]
		private Vector3 controlFirstLocal;

		[SerializeField]
		private Vector3 controlSecondLocal;

		[SerializeField]
		private Transform pointTransform;

		[SerializeField]
		private BGCurve curve;

		[SerializeField]
		private BGCurvePoint.FieldsValues[] fieldsValues;

		public BGCurve Curve => curve;

		public BGCurvePoint.FieldsValues PrivateValuesForFields
		{
			get
			{
				if (fieldsValues == null || fieldsValues.Length < 1 || fieldsValues[0] == null)
				{
					fieldsValues = new BGCurvePoint.FieldsValues[1]
					{
						new BGCurvePoint.FieldsValues()
					};
				}
				return fieldsValues[0];
			}
			set
			{
				if (fieldsValues == null || fieldsValues.Length < 1 || fieldsValues[0] == null)
				{
					fieldsValues = new BGCurvePoint.FieldsValues[1]
					{
						new BGCurvePoint.FieldsValues()
					};
				}
				fieldsValues[0] = value;
			}
		}

		public Vector3 PositionLocal
		{
			get
			{
				if (pointTransform != null)
				{
					return curve.transform.InverseTransformPoint(pointTransform.position);
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return positionLocal;
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return curve.transform.InverseTransformPoint(base.transform.position);
				default:
					throw WrongMode();
				}
			}
			set
			{
				SetPosition(value);
			}
		}

		public Vector3 PositionLocalTransformed
		{
			get
			{
				if (pointTransform != null)
				{
					return pointTransform.position - curve.transform.position;
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return curve.transform.TransformPoint(positionLocal) - curve.transform.position;
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return base.transform.position - curve.transform.position;
				default:
					throw WrongMode();
				}
			}
			set
			{
				SetPosition(value + curve.transform.position, worldSpaceIsUsed: true);
			}
		}

		public Vector3 PositionWorld
		{
			get
			{
				if (pointTransform != null)
				{
					return pointTransform.position;
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return curve.transform.TransformPoint(positionLocal);
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return base.transform.position;
				default:
					throw WrongMode();
				}
			}
			set
			{
				SetPosition(value, worldSpaceIsUsed: true);
			}
		}

		public Vector3 ControlFirstLocal
		{
			get
			{
				return controlFirstLocal;
			}
			set
			{
				SetControlFirstLocal(value);
			}
		}

		public Vector3 ControlFirstLocalTransformed
		{
			get
			{
				return TargetTransform.TransformVector(controlFirstLocal);
			}
			set
			{
				SetControlFirstLocal(TargetTransform.InverseTransformVector(value));
			}
		}

		public Vector3 ControlFirstWorld
		{
			get
			{
				if (pointTransform != null)
				{
					return pointTransform.position + pointTransform.TransformVector(controlFirstLocal);
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return curve.transform.TransformPoint(new Vector3(positionLocal.x + controlFirstLocal.x, positionLocal.y + controlFirstLocal.y, positionLocal.z + controlFirstLocal.z));
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return base.transform.position + base.transform.TransformVector(controlFirstLocal);
				default:
					throw WrongMode();
				}
			}
			set
			{
				Vector3 vector;
				if (pointTransform != null)
				{
					vector = pointTransform.InverseTransformVector(value - pointTransform.position);
				}
				else
				{
					switch (Curve.PointsMode)
					{
					case BGCurve.PointsModeEnum.GameObjectsNoTransform:
						vector = curve.transform.InverseTransformPoint(value) - PositionLocal;
						break;
					case BGCurve.PointsModeEnum.GameObjectsTransform:
						vector = base.transform.InverseTransformVector(value - base.transform.position);
						break;
					default:
						throw WrongMode();
					}
				}
				SetControlFirstLocal(vector);
			}
		}

		public Vector3 ControlSecondLocal
		{
			get
			{
				return controlSecondLocal;
			}
			set
			{
				SetControlSecondLocal(value);
			}
		}

		public Vector3 ControlSecondLocalTransformed
		{
			get
			{
				return TargetTransform.TransformVector(controlSecondLocal);
			}
			set
			{
				SetControlSecondLocal(TargetTransform.InverseTransformVector(value));
			}
		}

		public Vector3 ControlSecondWorld
		{
			get
			{
				if (pointTransform != null)
				{
					return pointTransform.position + pointTransform.TransformVector(controlSecondLocal);
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return curve.transform.TransformPoint(new Vector3(positionLocal.x + controlSecondLocal.x, positionLocal.y + controlSecondLocal.y, positionLocal.z + controlSecondLocal.z));
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return base.transform.position + base.transform.TransformVector(controlSecondLocal);
				default:
					throw WrongMode();
				}
			}
			set
			{
				Vector3 vector;
				if (pointTransform != null)
				{
					vector = pointTransform.InverseTransformVector(value - pointTransform.position);
				}
				else
				{
					switch (Curve.PointsMode)
					{
					case BGCurve.PointsModeEnum.GameObjectsNoTransform:
						vector = curve.transform.InverseTransformPoint(value) - PositionLocal;
						break;
					case BGCurve.PointsModeEnum.GameObjectsTransform:
						vector = base.transform.InverseTransformVector(value - base.transform.position);
						break;
					default:
						throw WrongMode();
					}
				}
				SetControlSecondLocal(vector);
			}
		}

		public BGCurvePoint.ControlTypeEnum ControlType
		{
			get
			{
				return controlType;
			}
			set
			{
				if (controlType != value)
				{
					curve.FireBeforeChange("point control type is changed");
					controlType = value;
					if (controlType == BGCurvePoint.ControlTypeEnum.BezierSymmetrical)
					{
						controlSecondLocal = -controlFirstLocal;
					}
					curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point control type is changed"), ignoreEventsGrouping: false, this);
				}
			}
		}

		public Transform PointTransform
		{
			get
			{
				return pointTransform;
			}
			set
			{
				if (pointTransform == value)
				{
					return;
				}
				curve.FireBeforeChange("point transform is changed");
				bool flag = pointTransform == null && value != null;
				bool flag2 = value == null && pointTransform != null;
				Vector3 controlFirstLocalTransformed = ControlFirstLocalTransformed;
				Vector3 controlSecondLocalTransformed = ControlSecondLocalTransformed;
				Vector3 positionWorld = PositionWorld;
				pointTransform = value;
				if (pointTransform != null)
				{
					pointTransform.position = positionWorld;
					controlFirstLocal = pointTransform.InverseTransformVector(controlFirstLocalTransformed);
					controlSecondLocal = pointTransform.InverseTransformVector(controlSecondLocalTransformed);
				}
				else
				{
					switch (curve.PointsMode)
					{
					case BGCurve.PointsModeEnum.GameObjectsNoTransform:
						positionLocal = curve.transform.InverseTransformPoint(positionWorld);
						controlFirstLocal = curve.transform.InverseTransformVector(controlFirstLocalTransformed);
						controlSecondLocal = curve.transform.InverseTransformVector(controlSecondLocalTransformed);
						break;
					case BGCurve.PointsModeEnum.GameObjectsTransform:
						base.transform.position = positionWorld;
						controlFirstLocal = base.transform.InverseTransformVector(controlFirstLocalTransformed);
						controlSecondLocal = base.transform.InverseTransformVector(controlSecondLocalTransformed);
						break;
					default:
						throw new ArgumentOutOfRangeException("curve.PointsMode");
					}
				}
				if (flag)
				{
					curve.PrivateTransformForPointAdded(curve.IndexOf(this));
				}
				else if (flag2)
				{
					curve.PrivateTransformForPointRemoved(curve.IndexOf(this));
				}
				curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point transform is changed"), ignoreEventsGrouping: false, this);
			}
		}

		private Transform TargetTransform
		{
			get
			{
				if (pointTransform != null)
				{
					return pointTransform;
				}
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					return curve.transform;
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					return base.transform;
				default:
					throw WrongMode();
				}
			}
		}

		public T GetField<T>(string name)
		{
			Type typeFromHandle = typeof(T);
			object field = GetField(name, typeFromHandle);
			return (T)field;
		}

		public float GetFloat(string name)
		{
			return PrivateValuesForFields.floatValues[curve.IndexOfFieldValue(name)];
		}

		public bool GetBool(string name)
		{
			return PrivateValuesForFields.boolValues[curve.IndexOfFieldValue(name)];
		}

		public int GetInt(string name)
		{
			return PrivateValuesForFields.intValues[curve.IndexOfFieldValue(name)];
		}

		public Vector3 GetVector3(string name)
		{
			return PrivateValuesForFields.vector3Values[curve.IndexOfFieldValue(name)];
		}

		public Quaternion GetQuaternion(string name)
		{
			return PrivateValuesForFields.quaternionValues[curve.IndexOfFieldValue(name)];
		}

		public Bounds GetBounds(string name)
		{
			return PrivateValuesForFields.boundsValues[curve.IndexOfFieldValue(name)];
		}

		public Color GetColor(string name)
		{
			return PrivateValuesForFields.colorValues[curve.IndexOfFieldValue(name)];
		}

		public object GetField(string name, Type type)
		{
			return BGCurvePoint.FieldTypes.GetField(curve, type, name, PrivateValuesForFields);
		}

		public void SetField<T>(string name, T value)
		{
			SetField(name, value, typeof(T));
		}

		public void SetField(string name, object value, Type type)
		{
			curve.FireBeforeChange("point field value is changed");
			BGCurvePoint.FieldTypes.SetField(curve, type, name, value, PrivateValuesForFields);
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetFloat(string name, float value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.floatValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetBool(string name, bool value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.boolValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetInt(string name, int value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.intValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetVector3(string name, Vector3 value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.vector3Values[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetQuaternion(string name, Quaternion value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.quaternionValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetBounds(string name, Bounds value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.boundsValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public void SetColor(string name, Color value)
		{
			curve.FireBeforeChange("point field value is changed");
			PrivateValuesForFields.colorValues[curve.IndexOfFieldValue(name)] = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point field value is changed"), ignoreEventsGrouping: false, this);
		}

		public override string ToString()
		{
			return "Point [localPosition=" + positionLocal + "]";
		}

		private void SetPosition(Vector3 value, bool worldSpaceIsUsed = false)
		{
			curve.FireBeforeChange("point position is changed");
			if (curve.SnapType != 0)
			{
				if (worldSpaceIsUsed)
				{
					curve.ApplySnapping(ref value);
				}
				else
				{
					Vector3 pos = curve.transform.TransformPoint(value);
					if (curve.ApplySnapping(ref pos))
					{
						value = curve.transform.InverseTransformPoint(pos);
					}
				}
			}
			if (pointTransform != null)
			{
				if (curve.Mode2D != 0)
				{
					value = curve.Apply2D(value);
				}
				pointTransform.position = ((!worldSpaceIsUsed) ? curve.transform.TransformPoint(value) : value);
			}
			else
			{
				switch (Curve.PointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					if (worldSpaceIsUsed)
					{
						Vector3 point = curve.transform.InverseTransformPoint(value);
						if (curve.Mode2D != 0)
						{
							point = curve.Apply2D(point);
						}
						positionLocal = point;
					}
					else
					{
						if (curve.Mode2D != 0)
						{
							value = curve.Apply2D(value);
						}
						positionLocal = value;
					}
					break;
				case BGCurve.PointsModeEnum.GameObjectsTransform:
					if (worldSpaceIsUsed)
					{
						if (curve.Mode2D != 0)
						{
							value = curve.transform.TransformPoint(curve.Apply2D(curve.transform.InverseTransformPoint(value)));
						}
						base.transform.position = value;
					}
					else
					{
						if (curve.Mode2D != 0)
						{
							value = curve.Apply2D(value);
						}
						base.transform.position = curve.transform.TransformPoint(value);
					}
					break;
				default:
					throw WrongMode();
				}
			}
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point position is changed"), ignoreEventsGrouping: false, this);
		}

		private void SetControlFirstLocal(Vector3 value)
		{
			curve.FireBeforeChange("point control is changed");
			if (curve.Mode2D != 0)
			{
				value = curve.Apply2D(value);
			}
			if (controlType == BGCurvePoint.ControlTypeEnum.BezierSymmetrical)
			{
				controlSecondLocal = -value;
			}
			controlFirstLocal = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point control is changed"), ignoreEventsGrouping: false, this);
		}

		private void SetControlSecondLocal(Vector3 value)
		{
			curve.FireBeforeChange("point control is changed");
			if (curve.Mode2D != 0)
			{
				value = curve.Apply2D(value);
			}
			if (controlType == BGCurvePoint.ControlTypeEnum.BezierSymmetrical)
			{
				controlFirstLocal = -value;
			}
			controlSecondLocal = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point control is changed"), ignoreEventsGrouping: false, this);
		}

		public void PrivateInit(BGCurvePoint point, BGCurve.PointsModeEnum pointsMode)
		{
			if (point != null)
			{
				curve = point.Curve;
				controlType = point.ControlType;
				pointTransform = point.PointTransform;
				switch (pointsMode)
				{
				case BGCurve.PointsModeEnum.GameObjectsNoTransform:
					positionLocal = point.PositionLocal;
					controlFirstLocal = point.ControlFirstLocal;
					controlSecondLocal = point.ControlSecondLocal;
					break;
				case BGCurve.PointsModeEnum.GameObjectsTransform:
				{
					base.transform.localPosition = point.PositionLocal;
					Transform transform = (!(pointTransform != null)) ? base.transform : pointTransform;
					controlFirstLocal = transform.InverseTransformVector(point.ControlFirstLocalTransformed);
					controlSecondLocal = transform.InverseTransformVector(point.ControlSecondLocalTransformed);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException("pointsMode", pointsMode, null);
				}
				return;
			}
			Transform transform2;
			switch (pointsMode)
			{
			case BGCurve.PointsModeEnum.GameObjectsNoTransform:
				if (Curve.PointsMode != BGCurve.PointsModeEnum.GameObjectsTransform)
				{
					throw new ArgumentOutOfRangeException("Curve.PointsMode", "Curve points mode should be equal to GameObjectsTransform");
				}
				positionLocal = base.transform.localPosition;
				transform2 = ((!(pointTransform != null)) ? curve.transform : pointTransform);
				break;
			case BGCurve.PointsModeEnum.GameObjectsTransform:
				if (Curve.PointsMode != BGCurve.PointsModeEnum.GameObjectsNoTransform)
				{
					throw new ArgumentOutOfRangeException("Curve.PointsMode", "Curve points mode should be equal to GameObjectsNoTransform");
				}
				base.transform.position = PositionWorld;
				transform2 = ((!(pointTransform != null)) ? base.transform : pointTransform);
				break;
			default:
				throw new ArgumentOutOfRangeException("pointsMode", pointsMode, null);
			}
			controlFirstLocal = transform2.InverseTransformVector(ControlFirstLocalTransformed);
			controlSecondLocal = transform2.InverseTransformVector(ControlSecondLocalTransformed);
		}

		private static ArgumentOutOfRangeException WrongMode()
		{
			return new ArgumentOutOfRangeException("Curve.PointsMode");
		}
	}
}
