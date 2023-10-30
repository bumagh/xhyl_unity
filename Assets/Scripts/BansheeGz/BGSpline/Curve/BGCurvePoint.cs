using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[Serializable]
	public class BGCurvePoint : BGCurvePointI
	{
		public enum ControlTypeEnum
		{
			Absent,
			BezierSymmetrical,
			BezierIndependant
		}

		public enum FieldEnum
		{
			PositionWorld,
			PositionLocal,
			ControlFirstWorld,
			ControlFirstLocal,
			ControlSecondWorld,
			ControlSecondLocal
		}

		[Serializable]
		public sealed class FieldsValues
		{
			[SerializeField]
			public bool[] boolValues;

			[SerializeField]
			public int[] intValues;

			[SerializeField]
			public float[] floatValues;

			[SerializeField]
			public string[] stringValues;

			[SerializeField]
			public Vector3[] vector3Values;

			[SerializeField]
			public Bounds[] boundsValues;

			[SerializeField]
			public Color[] colorValues;

			[SerializeField]
			public Quaternion[] quaternionValues;

			[SerializeField]
			public AnimationCurve[] animationCurveValues;

			[SerializeField]
			public GameObject[] gameObjectValues;

			[SerializeField]
			public Component[] componentValues;

			[SerializeField]
			public BGCurve[] bgCurveValues;

			[SerializeField]
			public BGCurvePointComponent[] bgCurvePointComponentValues;

			[SerializeField]
			public BGCurvePointGO[] bgCurvePointGOValues;
		}

		public static class FieldTypes
		{
			private static readonly Dictionary<Type, Func<FieldsValues, int, object>> type2fieldGetter;

			private static readonly Dictionary<Type, Action<FieldsValues, int, object>> type2fieldSetter;

			private static readonly Dictionary<BGCurvePointField.TypeEnum, Type> type2Type;

			static FieldTypes()
			{
				type2fieldGetter = new Dictionary<Type, Func<FieldsValues, int, object>>();
				type2fieldSetter = new Dictionary<Type, Action<FieldsValues, int, object>>();
				type2Type = new Dictionary<BGCurvePointField.TypeEnum, Type>();
				Register(BGCurvePointField.TypeEnum.Bool, typeof(bool), (FieldsValues value, int index) => value.boolValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.boolValues[index] = Convert.ToBoolean(o);
				});
				Register(BGCurvePointField.TypeEnum.Int, typeof(int), (FieldsValues value, int index) => value.intValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.intValues[index] = Convert.ToInt32(o);
				});
				Register(BGCurvePointField.TypeEnum.Float, typeof(float), (FieldsValues value, int index) => value.floatValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.floatValues[index] = Convert.ToSingle(o);
				});
				Register(BGCurvePointField.TypeEnum.String, typeof(string), (FieldsValues value, int index) => value.stringValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.stringValues[index] = (string)o;
				});
				Register(BGCurvePointField.TypeEnum.Vector3, typeof(Vector3), (FieldsValues value, int index) => value.vector3Values[index], delegate(FieldsValues value, int index, object o)
				{
					value.vector3Values[index] = (Vector3)o;
				});
				Register(BGCurvePointField.TypeEnum.Bounds, typeof(Bounds), delegate(FieldsValues value, int index)
				{
					Bounds bounds = value.boundsValues[index];
					return bounds;
				}, delegate(FieldsValues value, int index, object o)
				{
					value.boundsValues[index] = (Bounds)o;
				});
				Register(BGCurvePointField.TypeEnum.Quaternion, typeof(Quaternion), (FieldsValues value, int index) => value.quaternionValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.quaternionValues[index] = (Quaternion)o;
				});
				Register(BGCurvePointField.TypeEnum.Color, typeof(Color), (FieldsValues value, int index) => value.colorValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.colorValues[index] = (Color)o;
				});
				Register(BGCurvePointField.TypeEnum.AnimationCurve, typeof(AnimationCurve), (FieldsValues value, int index) => value.animationCurveValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.animationCurveValues[index] = (AnimationCurve)o;
				});
				Register(BGCurvePointField.TypeEnum.GameObject, typeof(GameObject), (FieldsValues value, int index) => value.gameObjectValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.gameObjectValues[index] = (GameObject)o;
				});
				Register(BGCurvePointField.TypeEnum.Component, typeof(Component), (FieldsValues value, int index) => value.componentValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.componentValues[index] = (Component)o;
				});
				Register(BGCurvePointField.TypeEnum.BGCurve, typeof(BGCurve), (FieldsValues value, int index) => value.bgCurveValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.bgCurveValues[index] = (BGCurve)o;
				});
				Register(BGCurvePointField.TypeEnum.BGCurvePointComponent, typeof(BGCurvePointComponent), (FieldsValues value, int index) => value.bgCurvePointComponentValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.bgCurvePointComponentValues[index] = (BGCurvePointComponent)o;
				});
				Register(BGCurvePointField.TypeEnum.BGCurvePointGO, typeof(BGCurvePointGO), (FieldsValues value, int index) => value.bgCurvePointGOValues[index], delegate(FieldsValues value, int index, object o)
				{
					value.bgCurvePointGOValues[index] = (BGCurvePointGO)o;
				});
			}

			private static void Register(BGCurvePointField.TypeEnum typeEnum, Type type, Func<FieldsValues, int, object> getter, Action<FieldsValues, int, object> setter)
			{
				type2Type[typeEnum] = type;
				type2fieldGetter[type] = getter;
				type2fieldSetter[type] = setter;
			}

			public static Type GetType(BGCurvePointField.TypeEnum type)
			{
				return type2Type[type];
			}

			public static object GetField(BGCurve curve, Type type, string name, FieldsValues values)
			{
				if (!type2fieldGetter.TryGetValue(type, out Func<FieldsValues, int, object> value))
				{
					throw new UnityException("Unsupported type for a field, type= " + type);
				}
				return value(values, IndexOfFieldRelative(curve, name));
			}

			public static void SetField(BGCurve curve, Type type, string name, object value, FieldsValues values)
			{
				if (!type2fieldSetter.TryGetValue(type, out Action<FieldsValues, int, object> value2))
				{
					throw new UnityException("Unsupported type for a field, type= " + type);
				}
				value2(values, IndexOfFieldRelative(curve, name), value);
			}

			private static int IndexOfFieldRelative(BGCurve curve, string name)
			{
				int num = curve.IndexOfFieldValue(name);
				if (num < 0)
				{
					throw new UnityException("Can not find a field with name " + name);
				}
				return num;
			}
		}

		[SerializeField]
		private ControlTypeEnum controlType;

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
		private FieldsValues[] fieldsValues;

		public BGCurve Curve => curve;

		public FieldsValues PrivateValuesForFields
		{
			get
			{
				if (fieldsValues == null || fieldsValues.Length < 1 || fieldsValues[0] == null)
				{
					fieldsValues = new FieldsValues[1]
					{
						new FieldsValues()
					};
				}
				return fieldsValues[0];
			}
			set
			{
				if (fieldsValues == null || fieldsValues.Length < 1 || fieldsValues[0] == null)
				{
					fieldsValues = new FieldsValues[1]
					{
						new FieldsValues()
					};
				}
				fieldsValues[0] = value;
			}
		}

		public Vector3 PositionLocal
		{
			get
			{
				return (!(pointTransform == null)) ? curve.transform.InverseTransformPoint(pointTransform.position) : positionLocal;
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
				return (!(pointTransform == null)) ? (pointTransform.position - curve.transform.position) : (curve.transform.TransformPoint(positionLocal) - curve.transform.position);
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
				return (!(pointTransform == null)) ? pointTransform.position : curve.transform.TransformPoint(positionLocal);
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
				return ((!(pointTransform == null)) ? pointTransform : curve.transform).TransformVector(controlFirstLocal);
			}
			set
			{
				Transform transform = (!(pointTransform == null)) ? pointTransform : curve.transform;
				SetControlFirstLocal(transform.InverseTransformVector(value));
			}
		}

		public Vector3 ControlFirstWorld
		{
			get
			{
				if (pointTransform == null)
				{
					return curve.transform.TransformPoint(new Vector3(positionLocal.x + controlFirstLocal.x, positionLocal.y + controlFirstLocal.y, positionLocal.z + controlFirstLocal.z));
				}
				return pointTransform.position + pointTransform.TransformVector(controlFirstLocal);
			}
			set
			{
				Vector3 vector = (!(pointTransform == null)) ? pointTransform.InverseTransformVector(value - pointTransform.position) : (curve.transform.InverseTransformPoint(value) - positionLocal);
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
				return ((!(pointTransform == null)) ? pointTransform : curve.transform).TransformVector(controlSecondLocal);
			}
			set
			{
				Transform transform = (!(pointTransform == null)) ? pointTransform : curve.transform;
				SetControlSecondLocal(transform.InverseTransformVector(value));
			}
		}

		public Vector3 ControlSecondWorld
		{
			get
			{
				if (pointTransform == null)
				{
					return curve.transform.TransformPoint(new Vector3(positionLocal.x + controlSecondLocal.x, positionLocal.y + controlSecondLocal.y, positionLocal.z + controlSecondLocal.z));
				}
				return pointTransform.position + pointTransform.TransformVector(controlSecondLocal);
			}
			set
			{
				Vector3 vector = (!(pointTransform == null)) ? pointTransform.InverseTransformVector(value - pointTransform.position) : (curve.transform.InverseTransformPoint(value) - positionLocal);
				SetControlSecondLocal(vector);
			}
		}

		public ControlTypeEnum ControlType
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
					if (controlType == ControlTypeEnum.BezierSymmetrical)
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
				if (!(pointTransform == value))
				{
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
						positionLocal = curve.transform.InverseTransformPoint(positionWorld);
						controlFirstLocal = curve.transform.InverseTransformVector(controlFirstLocalTransformed);
						controlSecondLocal = curve.transform.InverseTransformVector(controlSecondLocalTransformed);
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
		}

		public BGCurvePoint(BGCurve curve, Vector3 position, bool useWorldCoordinates = false)
			: this(curve, position, ControlTypeEnum.Absent, useWorldCoordinates)
		{
		}

		public BGCurvePoint(BGCurve curve, Vector3 position, ControlTypeEnum controlType, bool useWorldCoordinates = false)
			: this(curve, position, controlType, Vector3.zero, Vector3.zero, useWorldCoordinates)
		{
		}

		public BGCurvePoint(BGCurve curve, Vector3 position, ControlTypeEnum controlType, Vector3 controlFirst, Vector3 controlSecond, bool useWorldCoordinates = false)
			: this(curve, null, position, controlType, controlFirst, controlSecond, useWorldCoordinates)
		{
		}

		public BGCurvePoint(BGCurve curve, Transform pointTransform, Vector3 position, ControlTypeEnum controlType, Vector3 controlFirst, Vector3 controlSecond, bool useWorldCoordinates = false)
		{
			this.curve = curve;
			this.controlType = controlType;
			this.pointTransform = pointTransform;
			if (useWorldCoordinates)
			{
				positionLocal = curve.transform.InverseTransformPoint(position);
				controlFirstLocal = curve.transform.InverseTransformDirection(controlFirst - position);
				controlSecondLocal = curve.transform.InverseTransformDirection(controlSecond - position);
			}
			else
			{
				positionLocal = position;
				controlFirstLocal = controlFirst;
				controlSecondLocal = controlSecond;
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
			return FieldTypes.GetField(curve, type, name, PrivateValuesForFields);
		}

		public void SetField<T>(string name, T value)
		{
			SetField(name, value, typeof(T));
		}

		public void SetField(string name, object value, Type type)
		{
			curve.FireBeforeChange("point field value is changed");
			FieldTypes.SetField(curve, type, name, value, PrivateValuesForFields);
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

		public Vector3 Get(FieldEnum field)
		{
			switch (field)
			{
			case FieldEnum.PositionWorld:
				return PositionWorld;
			case FieldEnum.PositionLocal:
				return positionLocal;
			case FieldEnum.ControlFirstWorld:
				return ControlFirstWorld;
			case FieldEnum.ControlFirstLocal:
				return controlFirstLocal;
			case FieldEnum.ControlSecondWorld:
				return ControlSecondWorld;
			default:
				return controlSecondLocal;
			}
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
			if (pointTransform == null)
			{
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
			}
			else
			{
				if (curve.Mode2D != 0)
				{
					value = curve.Apply2D(value);
				}
				pointTransform.position = ((!worldSpaceIsUsed) ? curve.transform.TransformPoint(value) : value);
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
			if (controlType == ControlTypeEnum.BezierSymmetrical)
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
			if (controlType == ControlTypeEnum.BezierSymmetrical)
			{
				controlFirstLocal = -value;
			}
			controlSecondLocal = value;
			curve.FireChange((!curve.UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(Curve, this, "point control is changed"), ignoreEventsGrouping: false, this);
		}

		public static void PrivateFieldDeleted(BGCurvePointField field, int indexOfField, FieldsValues fieldsValues)
		{
			switch (field.Type)
			{
			case BGCurvePointField.TypeEnum.Bool:
				Ensure(ref fieldsValues.boolValues);
				fieldsValues.boolValues = BGCurve.Remove(fieldsValues.boolValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Int:
				Ensure(ref fieldsValues.intValues);
				fieldsValues.intValues = BGCurve.Remove(fieldsValues.intValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Float:
				Ensure(ref fieldsValues.floatValues);
				fieldsValues.floatValues = BGCurve.Remove(fieldsValues.floatValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Vector3:
				Ensure(ref fieldsValues.vector3Values);
				fieldsValues.vector3Values = BGCurve.Remove(fieldsValues.vector3Values, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Bounds:
				Ensure(ref fieldsValues.boundsValues);
				fieldsValues.boundsValues = BGCurve.Remove(fieldsValues.boundsValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Color:
				Ensure(ref fieldsValues.colorValues);
				fieldsValues.colorValues = BGCurve.Remove(fieldsValues.colorValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.String:
				Ensure(ref fieldsValues.stringValues);
				fieldsValues.stringValues = BGCurve.Remove(fieldsValues.stringValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Quaternion:
				Ensure(ref fieldsValues.quaternionValues);
				fieldsValues.quaternionValues = BGCurve.Remove(fieldsValues.quaternionValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.AnimationCurve:
				Ensure(ref fieldsValues.animationCurveValues);
				fieldsValues.animationCurveValues = BGCurve.Remove(fieldsValues.animationCurveValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.GameObject:
				Ensure(ref fieldsValues.gameObjectValues);
				fieldsValues.gameObjectValues = BGCurve.Remove(fieldsValues.gameObjectValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.Component:
				Ensure(ref fieldsValues.componentValues);
				fieldsValues.componentValues = BGCurve.Remove(fieldsValues.componentValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.BGCurve:
				Ensure(ref fieldsValues.bgCurveValues);
				fieldsValues.bgCurveValues = BGCurve.Remove(fieldsValues.bgCurveValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.BGCurvePointComponent:
				Ensure(ref fieldsValues.bgCurvePointComponentValues);
				fieldsValues.bgCurvePointComponentValues = BGCurve.Remove(fieldsValues.bgCurvePointComponentValues, indexOfField);
				break;
			case BGCurvePointField.TypeEnum.BGCurvePointGO:
				Ensure(ref fieldsValues.bgCurvePointGOValues);
				fieldsValues.bgCurvePointGOValues = BGCurve.Remove(fieldsValues.bgCurvePointGOValues, indexOfField);
				break;
			default:
				throw new ArgumentOutOfRangeException("field.Type", field.Type, "Unsupported type " + field.Type);
			}
		}

		public static void PrivateFieldAdded(BGCurvePointField field, FieldsValues fieldsValues)
		{
			Type type = FieldTypes.GetType(field.Type);
			object obj = (!BGReflectionAdapter.IsValueType(type)) ? null : Activator.CreateInstance(type);
			switch (field.Type)
			{
			case BGCurvePointField.TypeEnum.Bool:
				Ensure(ref fieldsValues.boolValues);
				fieldsValues.boolValues = BGCurve.Insert(fieldsValues.boolValues, fieldsValues.boolValues.Length, (bool)obj);
				break;
			case BGCurvePointField.TypeEnum.Int:
				Ensure(ref fieldsValues.intValues);
				fieldsValues.intValues = BGCurve.Insert(fieldsValues.intValues, fieldsValues.intValues.Length, (int)obj);
				break;
			case BGCurvePointField.TypeEnum.Float:
				Ensure(ref fieldsValues.floatValues);
				fieldsValues.floatValues = BGCurve.Insert(fieldsValues.floatValues, fieldsValues.floatValues.Length, (float)obj);
				break;
			case BGCurvePointField.TypeEnum.Vector3:
				Ensure(ref fieldsValues.vector3Values);
				fieldsValues.vector3Values = BGCurve.Insert(fieldsValues.vector3Values, fieldsValues.vector3Values.Length, (Vector3)obj);
				break;
			case BGCurvePointField.TypeEnum.Bounds:
				Ensure(ref fieldsValues.boundsValues);
				fieldsValues.boundsValues = BGCurve.Insert(fieldsValues.boundsValues, fieldsValues.boundsValues.Length, (Bounds)obj);
				break;
			case BGCurvePointField.TypeEnum.Color:
				Ensure(ref fieldsValues.colorValues);
				fieldsValues.colorValues = BGCurve.Insert(fieldsValues.colorValues, fieldsValues.colorValues.Length, (Color)obj);
				break;
			case BGCurvePointField.TypeEnum.String:
				Ensure(ref fieldsValues.stringValues);
				fieldsValues.stringValues = BGCurve.Insert(fieldsValues.stringValues, fieldsValues.stringValues.Length, (string)obj);
				break;
			case BGCurvePointField.TypeEnum.Quaternion:
				Ensure(ref fieldsValues.quaternionValues);
				fieldsValues.quaternionValues = BGCurve.Insert(fieldsValues.quaternionValues, fieldsValues.quaternionValues.Length, (Quaternion)obj);
				break;
			case BGCurvePointField.TypeEnum.AnimationCurve:
				Ensure(ref fieldsValues.animationCurveValues);
				fieldsValues.animationCurveValues = BGCurve.Insert(fieldsValues.animationCurveValues, fieldsValues.animationCurveValues.Length, (AnimationCurve)obj);
				break;
			case BGCurvePointField.TypeEnum.GameObject:
				Ensure(ref fieldsValues.gameObjectValues);
				fieldsValues.gameObjectValues = BGCurve.Insert(fieldsValues.gameObjectValues, fieldsValues.gameObjectValues.Length, (GameObject)obj);
				break;
			case BGCurvePointField.TypeEnum.Component:
				Ensure(ref fieldsValues.componentValues);
				fieldsValues.componentValues = BGCurve.Insert(fieldsValues.componentValues, fieldsValues.componentValues.Length, (Component)obj);
				break;
			case BGCurvePointField.TypeEnum.BGCurve:
				Ensure(ref fieldsValues.bgCurveValues);
				fieldsValues.bgCurveValues = BGCurve.Insert(fieldsValues.bgCurveValues, fieldsValues.bgCurveValues.Length, (BGCurve)obj);
				break;
			case BGCurvePointField.TypeEnum.BGCurvePointComponent:
				Ensure(ref fieldsValues.bgCurvePointComponentValues);
				fieldsValues.bgCurvePointComponentValues = BGCurve.Insert(fieldsValues.bgCurvePointComponentValues, fieldsValues.bgCurvePointComponentValues.Length, (BGCurvePointComponent)obj);
				break;
			case BGCurvePointField.TypeEnum.BGCurvePointGO:
				Ensure(ref fieldsValues.bgCurvePointGOValues);
				fieldsValues.bgCurvePointGOValues = BGCurve.Insert(fieldsValues.bgCurvePointGOValues, fieldsValues.bgCurvePointGOValues.Length, (BGCurvePointGO)obj);
				break;
			default:
				throw new ArgumentOutOfRangeException("field.Type", field.Type, "Unsupported type " + field.Type);
			}
		}

		private static void Ensure<T>(ref T[] array)
		{
			if (array == null)
			{
				array = new T[0];
			}
		}
	}
}
