using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[Serializable]
	[DisallowMultipleComponent]
	[AddComponentMenu("BansheeGz/BGCurve/BGCurve")]
	[ExecuteInEditMode]
	[HelpURL("http://www.bansheegz.com/BGCurve/")]
	public class BGCurve : MonoBehaviour
	{
		public enum Mode2DEnum
		{
			Off,
			XY,
			XZ,
			YZ
		}

		public enum SnapTypeEnum
		{
			Off,
			Points,
			Curve
		}

		public enum SnapAxisEnum
		{
			X,
			Y,
			Z
		}

		public enum EventModeEnum
		{
			Update,
			LateUpdate,
			NoEvents
		}

		public enum ForceChangedEventModeEnum
		{
			Off,
			EditorOnly,
			EditorAndRuntime
		}

		public enum PointsModeEnum
		{
			Inlined,
			Components,
			GameObjectsNoTransform,
			GameObjectsTransform
		}

		public delegate void IterationCallback(BGCurvePointI point, int index, int count);

		private sealed class FieldsTree
		{
			private readonly Dictionary<string, int> fieldName2Index = new Dictionary<string, int>();

			public bool Comply(BGCurvePointField[] fields)
			{
				return (fields != null) ? (fieldName2Index.Count == fields.Length) : (fieldName2Index.Count == 0);
			}

			public int GetIndex(string name)
			{
				if (fieldName2Index.TryGetValue(name, out int value))
				{
					return value;
				}
				throw new UnityException("Can not find a index of field " + name);
			}

			public void Update(BGCurvePointField[] fields)
			{
				fieldName2Index.Clear();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				int num12 = 0;
				int num13 = 0;
				int num14 = 0;
				foreach (BGCurvePointField bGCurvePointField in fields)
				{
					int value;
					switch (bGCurvePointField.Type)
					{
					case BGCurvePointField.TypeEnum.Bool:
						value = num++;
						break;
					case BGCurvePointField.TypeEnum.Int:
						value = num2++;
						break;
					case BGCurvePointField.TypeEnum.Float:
						value = num3++;
						break;
					case BGCurvePointField.TypeEnum.Vector3:
						value = num4++;
						break;
					case BGCurvePointField.TypeEnum.Bounds:
						value = num5++;
						break;
					case BGCurvePointField.TypeEnum.Color:
						value = num6++;
						break;
					case BGCurvePointField.TypeEnum.String:
						value = num7++;
						break;
					case BGCurvePointField.TypeEnum.Quaternion:
						value = num8++;
						break;
					case BGCurvePointField.TypeEnum.AnimationCurve:
						value = num9++;
						break;
					case BGCurvePointField.TypeEnum.GameObject:
						value = num10++;
						break;
					case BGCurvePointField.TypeEnum.Component:
						value = num11++;
						break;
					case BGCurvePointField.TypeEnum.BGCurve:
						value = num12++;
						break;
					case BGCurvePointField.TypeEnum.BGCurvePointComponent:
						value = num13++;
						break;
					case BGCurvePointField.TypeEnum.BGCurvePointGO:
						value = num14++;
						break;
					default:
						throw new UnityException("Unknown type " + bGCurvePointField.Type);
					}
					fieldName2Index[bGCurvePointField.FieldName] = value;
				}
			}
		}

		public const float Version = 1.25f;

		public const float Epsilon = 1E-05f;

		public const float MinSnapDistance = 0.1f;

		public const float MaxSnapDistance = 100f;

		public const string MethodAddPoint = "AddPoint";

		public const string MethodDeletePoint = "Delete";

		public const string MethodSetPointsNames = "SetPointsNames";

		public const string MethodAddField = "AddField";

		public const string MethodDeleteField = "DeleteField";

		public const string MethodConvertPoints = "ConvertPoints";

		public const string EventClosed = "closed is changed";

		public const string EventSnapType = "snapType is changed";

		public const string EventSnapAxis = "snapAxis is changed";

		public const string EventSnapDistance = "snapDistance is changed";

		public const string EventSnapTrigger = "snapTriggerInteraction is changed";

		public const string EventSnapBackfaces = "snapToBackFaces is changed";

		public const string EventSnapLayerMask = "snapLayerMask is changed";

		public const string EventSnapMonitoring = "snapMonitoring is changed";

		public const string EventAddField = "add a field";

		public const string EventDeleteField = "delete a field";

		public const string EventFieldName = "field name is changed";

		public const string Event2D = "2d mode is changed";

		public const string EventForceUpdate = "force update is changed";

		public const string EventPointsMode = "points mode is changed";

		public const string EventClearAllPoints = "clear all points";

		public const string EventAddPoint = "add a point";

		public const string EventAddPoints = "add points";

		public const string EventDeletePoints = "delete points";

		public const string EventSwapPoints = "swap points";

		public const string EventReversePoints = "reverse points";

		public const string EventTransaction = "changes in transaction";

		public const string EventTransform = "transform is changed";

		public const string EventForcedUpdate = "forced update";

		public const string EventPointPosition = "point position is changed";

		public const string EventPointTransform = "point transform is changed";

		public const string EventPointControl = "point control is changed";

		public const string EventPointControlType = "point control type is changed";

		public const string EventPointField = "point field value is changed";

		private static readonly RaycastHit[] raycastHitArray = new RaycastHit[50];

		private static readonly BGCurvePointI[] pointArray = new BGCurvePointI[1];

		private static readonly List<BGCurvePointI> pointsList = new List<BGCurvePointI>();

		private static readonly List<int> pointsIndexesList = new List<int>();

		[Tooltip("2d Mode for a curve. In 2d mode, only 2 coordinates matter, the third will always be 0 (including controls). Handles in Editor will also be switched to 2d mode")]
		[SerializeField]
		private Mode2DEnum mode2D;

		[Tooltip("If curve is closed")]
		[SerializeField]
		private bool closed;

		[SerializeField]
		private BGCurvePoint[] points = new BGCurvePoint[0];

		[SerializeField]
		private BGCurvePointComponent[] pointsComponents = new BGCurvePointComponent[0];

		[SerializeField]
		private BGCurvePointGO[] pointsGameObjects = new BGCurvePointGO[0];

		[SerializeField]
		private BGCurvePointField[] fields = new BGCurvePointField[0];

		[SerializeField]
		[Tooltip("Snap type. A collider should exists for points to snap to.\r\n 1) Off - snaping is off\r\n 2) Points - only curve's points will be snapped.\r\n 3) Curve - both curve's points and split points will be snapped. With 'Curve' mode Base Math type gives better results, than Adaptive Math, cause snapping occurs after approximation.Also, 'Curve' mode can add a huge overhead if you are changing curve's points at runtime.")]
		private SnapTypeEnum snapType;

		[Tooltip("Axis for snapping points")]
		[SerializeField]
		private SnapAxisEnum snapAxis = SnapAxisEnum.Y;

		[SerializeField]
		[Range(0.1f, 100f)]
		[Tooltip("Snapping distance.")]
		private float snapDistance = 10f;

		[Tooltip("Layer mask for snapping")]
		[SerializeField]
		private LayerMask snapLayerMask = -1;

		[Tooltip("Should snapping takes triggers into account")]
		[SerializeField]
		private QueryTriggerInteraction snapTriggerInteraction;

		[SerializeField]
		[Tooltip("Should snapping takes backfaces of colliders into account")]
		private bool snapToBackFaces;

		[SerializeField]
		[Tooltip("Should curve monitor surrounding environment every frame. This is super costly in terms of performance (especially for Curve snap mode)")]
		private bool snapMonitoring;

		[SerializeField]
		[Tooltip("Event mode for runtime")]
		private EventModeEnum eventMode;

		[SerializeField]
		[Tooltip("Points mode, how points are stored. \r\n 1) Inline - points stored inlined with the curve's component.\r\n 2) Component - points are stored as MonoBehaviour scripts attached to the curve's GameObject.\r\n 3) GameObject - points are stored as MonoBehaviour scripts attached to separate GameObject for each point.")]
		private PointsModeEnum pointsMode;

		[SerializeField]
		[Tooltip("Force firing of Changed event. This can be useful if you use Unity's Animation. Do not use it unless you really need it.")]
		private ForceChangedEventModeEnum forceChangedEventMode;

		private int transactionLevel;

		private List<BGCurveChangedArgs> changeList;

		private FieldsTree fieldsTree;

		private bool changed;

		private bool immediateChangeEvents;

		private EventModeEnum eventModeOld;

		private BGCurveChangedArgs.ChangeTypeEnum lastEventType;

		private string lastEventMessage;

		private List<int> pointsWithTransforms;

		public BGCurvePointI[] Points
		{
			get
			{
				switch (pointsMode)
				{
				case PointsModeEnum.Inlined:
					return points;
				case PointsModeEnum.Components:
					return pointsComponents;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					return pointsGameObjects;
				default:
					throw new ArgumentOutOfRangeException("pointsMode");
				}
			}
		}

		public BGCurvePoint[] bgPoints
		{
			get
			{
				if (pointsMode == PointsModeEnum.Inlined)
				{
					return points;
				}
				return null;
			}
		}

		public int PointsCount
		{
			get
			{
				switch (pointsMode)
				{
				case PointsModeEnum.Inlined:
					return points.Length;
				case PointsModeEnum.Components:
					return pointsComponents.Length;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					return pointsGameObjects.Length;
				default:
					throw new ArgumentOutOfRangeException("pointsMode");
				}
			}
		}

		public BGCurvePointField[] Fields => fields;

		public int FieldsCount => fields.Length;

		public bool Closed
		{
			get
			{
				return closed;
			}
			set
			{
				if (value != closed)
				{
					FireBeforeChange("closed is changed");
					closed = value;
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "closed is changed"));
				}
			}
		}

		public PointsModeEnum PointsMode
		{
			get
			{
				return pointsMode;
			}
			set
			{
				if (pointsMode != value)
				{
					ConvertPoints(value);
				}
			}
		}

		public Mode2DEnum Mode2D
		{
			get
			{
				return mode2D;
			}
			set
			{
				if (mode2D != value)
				{
					Apply2D(value);
				}
			}
		}

		public bool Mode2DOn => mode2D != Mode2DEnum.Off;

		public SnapTypeEnum SnapType
		{
			get
			{
				return snapType;
			}
			set
			{
				if (snapType != value)
				{
					FireBeforeChange("snapType is changed");
					snapType = value;
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapType is changed"));
				}
			}
		}

		public SnapAxisEnum SnapAxis
		{
			get
			{
				return snapAxis;
			}
			set
			{
				if (snapAxis != value)
				{
					FireBeforeChange("snapAxis is changed");
					snapAxis = value;
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapAxis is changed"));
				}
			}
		}

		public float SnapDistance
		{
			get
			{
				return snapDistance;
			}
			set
			{
				if (!(Math.Abs(snapDistance - value) < 1E-05f))
				{
					FireBeforeChange("snapDistance is changed");
					snapDistance = Mathf.Clamp(value, 0.1f, 100f);
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapDistance is changed"));
				}
			}
		}

		public QueryTriggerInteraction SnapTriggerInteraction
		{
			get
			{
				return snapTriggerInteraction;
			}
			set
			{
				if (snapTriggerInteraction != value)
				{
					FireBeforeChange("snapTriggerInteraction is changed");
					snapTriggerInteraction = value;
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapTriggerInteraction is changed"));
				}
			}
		}

		public bool SnapToBackFaces
		{
			get
			{
				return snapToBackFaces;
			}
			set
			{
				if (snapToBackFaces != value)
				{
					FireBeforeChange("snapToBackFaces is changed");
					snapToBackFaces = value;
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapToBackFaces is changed"));
				}
			}
		}

		public LayerMask SnapLayerMask
		{
			get
			{
				return snapLayerMask;
			}
			set
			{
				if ((int)snapLayerMask != (int)value)
				{
					FireBeforeChange("snapLayerMask is changed");
					snapLayerMask = value;
					if (snapType != 0)
					{
						ApplySnapping();
					}
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapLayerMask is changed"));
				}
			}
		}

		public bool SnapMonitoring
		{
			get
			{
				return snapMonitoring;
			}
			set
			{
				if (snapMonitoring != value)
				{
					FireBeforeChange("snapMonitoring is changed");
					snapMonitoring = value;
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Snap, "snapMonitoring is changed"));
				}
			}
		}

		public ForceChangedEventModeEnum ForceChangedEventMode
		{
			get
			{
				return forceChangedEventMode;
			}
			set
			{
				if (forceChangedEventMode != value)
				{
					FireBeforeChange("force update is changed");
					forceChangedEventMode = value;
					FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Curve, "force update is changed"));
				}
			}
		}

		[Obsolete("It is not used anymore and should be removed")]
		public bool TraceChanges
		{
			get
			{
				return this.Changed != null;
			}
			set
			{
			}
		}

		public bool SupressEvents
		{
			get
			{
				return eventMode == EventModeEnum.NoEvents;
			}
			set
			{
				if (value && eventMode != EventModeEnum.NoEvents)
				{
					eventModeOld = eventMode;
				}
				eventMode = ((!value) ? eventModeOld : EventModeEnum.NoEvents);
			}
		}

		public bool UseEventsArgs
		{
			get;
			set;
		}

		public EventModeEnum EventMode
		{
			get
			{
				return eventMode;
			}
			set
			{
				eventMode = value;
			}
		}

		public bool ImmediateChangeEvents
		{
			get
			{
				return immediateChangeEvents;
			}
			set
			{
				immediateChangeEvents = value;
			}
		}

		private List<BGCurveChangedArgs> ChangeList => changeList ?? (changeList = new List<BGCurveChangedArgs>());

		public BGCurvePointI this[int i]
		{
			get
			{
				switch (pointsMode)
				{
				case PointsModeEnum.Inlined:
					return points[i];
				case PointsModeEnum.Components:
					return pointsComponents[i];
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					return pointsGameObjects[i];
				default:
					throw new ArgumentOutOfRangeException("pointsMode");
				}
			}
			set
			{
				switch (pointsMode)
				{
				case PointsModeEnum.Inlined:
					points[i] = (BGCurvePoint)value;
					break;
				case PointsModeEnum.Components:
					pointsComponents[i] = (BGCurvePointComponent)value;
					break;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					pointsGameObjects[i] = (BGCurvePointGO)value;
					break;
				default:
					throw new ArgumentOutOfRangeException("pointsMode");
				}
			}
		}

		public int TransactionLevel => transactionLevel;

		public event EventHandler<BGCurveChangedArgs> Changed;

		public event EventHandler<BGCurveChangedArgs.BeforeChange> BeforeChange;

		public BGCurvePoint CreatePointFromWorldPosition(Vector3 worldPos, BGCurvePoint.ControlTypeEnum controlType)
		{
			return new BGCurvePoint(this, worldPos, controlType, useWorldCoordinates: true);
		}

		public BGCurvePoint CreatePointFromWorldPosition(Vector3 worldPos, BGCurvePoint.ControlTypeEnum controlType, Vector3 control1WorldPos, Vector3 control2WorldPos)
		{
			return new BGCurvePoint(this, worldPos, controlType, control1WorldPos, control2WorldPos, useWorldCoordinates: true);
		}

		public BGCurvePoint CreatePointFromLocalPosition(Vector3 localPos, BGCurvePoint.ControlTypeEnum controlType)
		{
			return new BGCurvePoint(this, localPos, controlType);
		}

		public BGCurvePoint CreatePointFromLocalPosition(Vector3 localPos, BGCurvePoint.ControlTypeEnum controlType, Vector3 control1LocalPos, Vector3 control2LocalPos)
		{
			return new BGCurvePoint(this, localPos, controlType, control1LocalPos, control2LocalPos);
		}

		public void Clear()
		{
			int pointsCount = PointsCount;
			if (pointsCount == 0)
			{
				return;
			}
			FireBeforeChange("clear all points");
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				points = new BGCurvePoint[0];
				break;
			case PointsModeEnum.Components:
				if (pointsCount > 0)
				{
					for (int num2 = pointsCount - 1; num2 >= 0; num2--)
					{
						DestroyIt(pointsComponents[num2]);
					}
				}
				pointsComponents = new BGCurvePointComponent[0];
				break;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				if (pointsCount > 0)
				{
					for (int num = pointsCount - 1; num >= 0; num--)
					{
						DestroyIt(pointsGameObjects[num].gameObject);
					}
				}
				pointsGameObjects = new BGCurvePointGO[0];
				break;
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "clear all points"));
		}

		public int IndexOf(BGCurvePointI point)
		{
			return IndexOf(Points, point);
		}

		public BGCurvePointI AddPoint(BGCurvePoint point)
		{
			return AddPoint(point, PointsCount, null);
		}

		public BGCurvePointI AddPoint(BGCurvePoint point, int index)
		{
			return AddPoint(point, index, null);
		}

		public void AddPoints(BGCurvePoint[] points)
		{
			AddPoints(points, PointsCount, skipFieldsProcessing: false, null);
		}

		public void AddPoints(BGCurvePoint[] points, int index)
		{
			AddPoints(points, index, skipFieldsProcessing: false, null);
		}

		public void Delete(BGCurvePointI point)
		{
			Delete(IndexOf(point), null);
		}

		public void Delete(int index)
		{
			Delete(index, null);
		}

		public void Delete(BGCurvePointI[] points)
		{
			Delete(points, null);
		}

		public void Swap(int index1, int index2)
		{
			if (index1 < 0 || index1 >= PointsCount || index2 < 0 || index2 >= PointsCount)
			{
				throw new UnityException("Unable to remove a point. Invalid indexes: " + index1 + ", " + index2);
			}
			FireBeforeChange("swap points");
			BGCurvePointI[] array = Points;
			BGCurvePointI bGCurvePointI = array[index1];
			BGCurvePointI bGCurvePointI2 = array[index2];
			bool flag = bGCurvePointI.PointTransform != null || bGCurvePointI2.PointTransform != null;
			array[index2] = bGCurvePointI;
			array[index1] = bGCurvePointI2;
			if (IsGoMode(pointsMode))
			{
				SetPointsNames();
			}
			if (flag)
			{
				CachePointsWithTransforms();
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "swap points"));
		}

		public void Reverse()
		{
			int pointsCount = PointsCount;
			if (pointsCount < 2)
			{
				return;
			}
			FireBeforeChange("reverse points");
			BGCurvePointI[] array = Points;
			bool flag = FieldsCount > 0;
			PointsModeEnum pointsModeEnum = PointsMode;
			int num = pointsCount >> 1;
			int num2 = pointsCount - 1;
			for (int i = 0; i < num; i++)
			{
				BGCurvePointI bGCurvePointI = array[i];
				BGCurvePointI bGCurvePointI2 = array[num2 - i];
				Vector3 positionLocal = bGCurvePointI2.PositionLocal;
				BGCurvePoint.ControlTypeEnum controlType = bGCurvePointI2.ControlType;
				Vector3 controlFirstLocal = bGCurvePointI2.ControlFirstLocal;
				Vector3 controlSecondLocal = bGCurvePointI2.ControlSecondLocal;
				BGCurvePoint.FieldsValues fieldsValues = (!flag) ? null : GetFieldsValues(bGCurvePointI2, pointsModeEnum);
				bGCurvePointI2.PositionLocal = bGCurvePointI.PositionLocal;
				bGCurvePointI2.ControlType = bGCurvePointI.ControlType;
				bGCurvePointI2.ControlFirstLocal = bGCurvePointI.ControlSecondLocal;
				bGCurvePointI2.ControlSecondLocal = bGCurvePointI.ControlFirstLocal;
				if (flag)
				{
					SetFieldsValues(bGCurvePointI2, pointsModeEnum, GetFieldsValues(bGCurvePointI, pointsModeEnum));
				}
				bGCurvePointI.PositionLocal = positionLocal;
				bGCurvePointI.ControlType = controlType;
				bGCurvePointI.ControlFirstLocal = controlSecondLocal;
				bGCurvePointI.ControlSecondLocal = controlFirstLocal;
				if (flag)
				{
					SetFieldsValues(bGCurvePointI, pointsModeEnum, fieldsValues);
				}
			}
			if (pointsCount % 2 != 0)
			{
				BGCurvePointI bGCurvePointI3 = array[num];
				Vector3 controlFirstLocal2 = bGCurvePointI3.ControlFirstLocal;
				bGCurvePointI3.ControlFirstLocal = bGCurvePointI3.ControlSecondLocal;
				bGCurvePointI3.ControlSecondLocal = controlFirstLocal2;
			}
			if (IsGoMode(pointsModeEnum))
			{
				SetPointsNames();
			}
			CachePointsWithTransforms();
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "reverse points"));
		}

		public BGCurvePointField AddField(string name, BGCurvePointField.TypeEnum type)
		{
			return AddField(name, type, null);
		}

		public void DeleteField(BGCurvePointField field)
		{
			DeleteField(field, null);
		}

		public int IndexOf(BGCurvePointField field)
		{
			return IndexOf(fields, field);
		}

		public BGCurvePointField GetField(string name)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				BGCurvePointField bGCurvePointField = fields[i];
				if (string.Equals(bGCurvePointField.FieldName, name))
				{
					return bGCurvePointField;
				}
			}
			return null;
		}

		public bool HasField(string name)
		{
			if (FieldsCount == 0)
			{
				return false;
			}
			BGCurvePointField[] array = fields;
			foreach (BGCurvePointField bGCurvePointField in array)
			{
				if (string.Equals(name, bGCurvePointField.FieldName))
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOfFieldValue(string name)
		{
			if (fieldsTree == null || !fieldsTree.Comply(fields))
			{
				PrivateUpdateFieldsValuesIndexes();
			}
			return fieldsTree.GetIndex(name);
		}

		public void PrivateUpdateFieldsValuesIndexes()
		{
			fieldsTree = (fieldsTree ?? new FieldsTree());
			fieldsTree.Update(fields);
		}

		public void Apply2D(Mode2DEnum value)
		{
			FireBeforeChange("2d mode is changed");
			mode2D = value;
			if (mode2D != 0 && PointsCount > 0)
			{
				Transaction(delegate
				{
					BGCurvePointI[] array = Points;
					int num = array.Length;
					for (int i = 0; i < num; i++)
					{
						Apply2D(array[i]);
					}
				});
			}
			else
			{
				FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "2d mode is changed"));
			}
		}

		public virtual void Apply2D(BGCurvePointI point)
		{
			point.PositionLocal = point.PositionLocal;
			point.ControlFirstLocal = point.ControlFirstLocal;
			point.ControlSecondLocal = point.ControlSecondLocal;
		}

		public virtual Vector3 Apply2D(Vector3 point)
		{
			switch (mode2D)
			{
			case Mode2DEnum.XY:
				return new Vector3(point.x, point.y, 0f);
			case Mode2DEnum.XZ:
				return new Vector3(point.x, 0f, point.z);
			case Mode2DEnum.YZ:
				return new Vector3(0f, point.y, point.z);
			default:
				return point;
			}
		}

		public bool ApplySnapping()
		{
			if (snapType == SnapTypeEnum.Off)
			{
				return false;
			}
			BGCurvePointI[] array = Points;
			int num = array.Length;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				flag |= ApplySnapping(array[i]);
			}
			return flag;
		}

		public bool ApplySnapping(BGCurvePointI point)
		{
			if (snapType == SnapTypeEnum.Off)
			{
				return false;
			}
			Vector3 pos = point.PositionWorld;
			bool flag = ApplySnapping(ref pos);
			if (flag)
			{
				point.PositionWorld = pos;
			}
			return flag;
		}

		public bool ApplySnapping(ref Vector3 pos)
		{
			if (snapType == SnapTypeEnum.Off)
			{
				return false;
			}
			Vector3 vector;
			switch (snapAxis)
			{
			case SnapAxisEnum.Y:
				vector = Vector3.up;
				break;
			case SnapAxisEnum.X:
				vector = Vector3.right;
				break;
			default:
				vector = Vector3.forward;
				break;
			}
			Vector3 vector2 = default(Vector3);
			float num = -1f;
			Ray ray = default(Ray);
			for (int i = 0; i < 2; i++)
			{
				ray = new Ray(pos, (i != 0) ? (-vector) : vector);
				if (Physics.Raycast(ray, out RaycastHit hitInfo, snapDistance, snapLayerMask, snapTriggerInteraction) && (num < 0f || num > hitInfo.distance))
				{
					num = hitInfo.distance;
					vector2 = hitInfo.point;
				}
			}
			if (snapToBackFaces)
			{
				for (int j = 0; j < 2; j++)
				{
					int num2 = Physics.RaycastNonAlloc((j != 0) ? new Ray(new Vector3(pos.x - vector.x * snapDistance, pos.y - vector.y * snapDistance, pos.z - vector.z * snapDistance), vector) : new Ray(new Vector3(pos.x + vector.x * snapDistance, pos.y + vector.y * snapDistance, pos.z + vector.z * snapDistance), -vector), raycastHitArray, snapDistance, snapLayerMask, snapTriggerInteraction);
					if (num2 == 0)
					{
						continue;
					}
					for (int k = 0; k < num2; k++)
					{
						RaycastHit raycastHit = raycastHitArray[k];
						float num3 = snapDistance - raycastHit.distance;
						if (num < 0f || num > num3)
						{
							num = num3;
							vector2 = raycastHit.point;
						}
					}
				}
			}
			if (num < 0f)
			{
				return false;
			}
			pos = vector2;
			return true;
		}

		private void SnapIt()
		{
			switch (snapType)
			{
			case SnapTypeEnum.Off:
				break;
			case SnapTypeEnum.Points:
				ApplySnapping();
				break;
			case SnapTypeEnum.Curve:
				if (!ApplySnapping())
				{
					if (immediateChangeEvents && forceChangedEventMode != ForceChangedEventModeEnum.EditorAndRuntime)
					{
						FireChange((!UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(this, lastEventType, lastEventMessage), ignoreEventsGrouping: true);
					}
					else
					{
						changed = true;
					}
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void Transaction(Action action)
		{
			FireBeforeChange("changes in transaction");
			transactionLevel++;
			if (UseEventsArgs && transactionLevel == 1)
			{
				ChangeList.Clear();
			}
			try
			{
				action();
			}
			finally
			{
				transactionLevel--;
				if (transactionLevel == 0)
				{
					FireChange((!UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(this, ChangeList.ToArray(), "changes in transaction"));
					if (UseEventsArgs)
					{
						ChangeList.Clear();
					}
				}
			}
		}

		protected internal static int IndexOf<T>(T[] array, T item)
		{
			return Array.IndexOf(array, item);
		}

		public void FireBeforeChange(string operation)
		{
			if (eventMode != EventModeEnum.NoEvents && transactionLevel <= 0 && this.BeforeChange != null)
			{
				this.BeforeChange(this, (!UseEventsArgs) ? null : BGCurveChangedArgs.BeforeChange.GetInstance(operation));
			}
		}

		public void FireChange(BGCurveChangedArgs change, bool ignoreEventsGrouping = false, object sender = null)
		{
			if (eventMode == EventModeEnum.NoEvents || this.Changed == null)
			{
				return;
			}
			if (transactionLevel > 0 || (!immediateChangeEvents && !ignoreEventsGrouping))
			{
				changed = true;
				if (change != null)
				{
					lastEventType = change.ChangeType;
					lastEventMessage = change.Message;
				}
				if (UseEventsArgs && !ChangeList.Contains(change))
				{
					ChangeList.Add((BGCurveChangedArgs)change.Clone());
				}
			}
			else
			{
				this.Changed(sender ?? this, change);
			}
		}

		public void Start()
		{
			CachePointsWithTransforms();
		}

		protected void Update()
		{
			if (Time.frameCount == 1)
			{
				base.transform.hasChanged = false;
			}
			if (eventMode == EventModeEnum.Update)
			{
				if (snapMonitoring && snapType != 0)
				{
					SnapIt();
				}
				if (this.Changed != null)
				{
					FireFinalEvent();
				}
			}
		}

		protected void LateUpdate()
		{
			if (eventMode == EventModeEnum.LateUpdate)
			{
				if (snapMonitoring && snapType != 0)
				{
					SnapIt();
				}
				if (this.Changed != null)
				{
					FireFinalEvent();
				}
			}
		}

		public Vector3 ToLocal(Vector3 worldPoint)
		{
			return base.transform.InverseTransformPoint(worldPoint);
		}

		public Vector3 ToWorld(Vector3 localPoint)
		{
			return base.transform.TransformPoint(localPoint);
		}

		public Vector3 ToLocalDirection(Vector3 direction)
		{
			return base.transform.InverseTransformDirection(direction);
		}

		public Vector3 ToWorldDirection(Vector3 direction)
		{
			return base.transform.TransformDirection(direction);
		}

		public void ForEach(IterationCallback iterationCallback)
		{
			for (int i = 0; i < PointsCount; i++)
			{
				iterationCallback(Points[i], i, PointsCount);
			}
		}

		private void SetPointsNames()
		{
			try
			{
				if (base.gameObject == null)
				{
					return;
				}
			}
			catch (MissingReferenceException)
			{
				return;
			}
			string name = base.gameObject.name;
			int num = pointsGameObjects.Length;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = pointsGameObjects[i].gameObject;
				gameObject.name = name + "[" + i + "]";
				gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
			}
		}

		public static T[] Insert<T>(T[] oldArray, int index, T[] newElements)
		{
			T[] array = new T[oldArray.Length + newElements.Length];
			if (index > 0)
			{
				Array.Copy(oldArray, array, index);
			}
			if (index < oldArray.Length)
			{
				Array.Copy(oldArray, index, array, index + newElements.Length, oldArray.Length - index);
			}
			Array.Copy(newElements, 0, array, index, newElements.Length);
			return array;
		}

		public static T[] Insert<T>(T[] oldArray, int index, T newElement)
		{
			T[] array = new T[oldArray.Length + 1];
			if (index > 0)
			{
				Array.Copy(oldArray, array, index);
			}
			if (index < oldArray.Length)
			{
				Array.Copy(oldArray, index, array, index + 1, oldArray.Length - index);
			}
			array[index] = newElement;
			return array;
		}

		public static T[] Remove<T>(T[] oldArray, int index)
		{
			T[] array = new T[oldArray.Length - 1];
			if (index > 0)
			{
				Array.Copy(oldArray, array, index);
			}
			if (index < oldArray.Length - 1)
			{
				Array.Copy(oldArray, index + 1, array, index, oldArray.Length - 1 - index);
			}
			return array;
		}

		public string ToString()
		{
			return "BGCurve [id=" + GetInstanceID() + "], points=" + PointsCount;
		}

		public static bool IsGoMode(PointsModeEnum pointsMode)
		{
			return pointsMode == PointsModeEnum.GameObjectsNoTransform || pointsMode == PointsModeEnum.GameObjectsTransform;
		}

		public void PrivateTransformForPointAdded(int index)
		{
			if (index >= 0 && PointsCount < index)
			{
				if (pointsWithTransforms == null)
				{
					pointsWithTransforms = new List<int>();
				}
				if (pointsWithTransforms.IndexOf(index) == -1)
				{
					pointsWithTransforms.Add(index);
				}
			}
		}

		public void PrivateTransformForPointRemoved(int index)
		{
			if (pointsWithTransforms != null && pointsWithTransforms.IndexOf(index) != -1)
			{
				pointsWithTransforms.Remove(index);
			}
		}

		private BGCurvePointI AddPoint(BGCurvePoint point, int index, Func<BGCurvePointI> provider = null)
		{
			if (index < 0 || index > PointsCount)
			{
				throw new UnityException("Unable to add a point. Invalid index: " + index);
			}
			FireBeforeChange("add a point");
			BGCurvePointI bGCurvePointI;
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				points = Insert(points, index, point);
				bGCurvePointI = point;
				break;
			case PointsModeEnum.Components:
			{
				BGCurvePointComponent bGCurvePointComponent = (BGCurvePointComponent)Convert(point, PointsModeEnum.Inlined, pointsMode, provider);
				pointsComponents = Insert(pointsComponents, index, bGCurvePointComponent);
				bGCurvePointI = bGCurvePointComponent;
				break;
			}
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
			{
				BGCurvePointGO bGCurvePointGO = (BGCurvePointGO)Convert(point, PointsModeEnum.Inlined, pointsMode, provider);
				pointsGameObjects = Insert(pointsGameObjects, index, bGCurvePointGO);
				SetPointsNames();
				bGCurvePointI = bGCurvePointGO;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
			if (FieldsCount > 0)
			{
				BGCurvePoint.FieldsValues fieldsValues = GetFieldsValues(bGCurvePointI, pointsMode);
				BGCurvePointField[] array = fields;
				foreach (BGCurvePointField field in array)
				{
					BGCurvePoint.PrivateFieldAdded(field, fieldsValues);
				}
			}
			if (point.PointTransform != null || (pointsWithTransforms != null && pointsWithTransforms.Count > 0))
			{
				CachePointsWithTransforms();
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Point, "add a point"));
			return bGCurvePointI;
		}

		private BGCurvePoint.FieldsValues GetFieldsValues(BGCurvePointI point, PointsModeEnum pointsMode)
		{
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				return ((BGCurvePoint)point).PrivateValuesForFields;
			case PointsModeEnum.Components:
				return ((BGCurvePointComponent)point).Point.PrivateValuesForFields;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				return ((BGCurvePointGO)point).PrivateValuesForFields;
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
		}

		private void SetFieldsValues(BGCurvePointI point, PointsModeEnum pointsMode, BGCurvePoint.FieldsValues fieldsValues)
		{
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				((BGCurvePoint)point).PrivateValuesForFields = fieldsValues;
				break;
			case PointsModeEnum.Components:
				((BGCurvePointComponent)point).Point.PrivateValuesForFields = fieldsValues;
				break;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				((BGCurvePointGO)point).PrivateValuesForFields = fieldsValues;
				break;
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
		}

		private void AddPoints(BGCurvePoint[] points, int index, bool skipFieldsProcessing = false, Func<BGCurvePointI> provider = null)
		{
			if (points == null)
			{
				return;
			}
			int num = points.Length;
			if (num == 0)
			{
				return;
			}
			if (index < 0 || index > PointsCount)
			{
				throw new UnityException("Unable to add points. Invalid index: " + index);
			}
			FireBeforeChange("add points");
			bool flag = pointsWithTransforms != null && pointsWithTransforms.Count > 0;
			BGCurvePointI[] addedPoints;
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				this.points = Insert(this.points, index, points);
				if (!flag)
				{
					for (int k = 0; k < num; k++)
					{
						if (!(points[k].PointTransform == null))
						{
							flag = true;
							break;
						}
					}
				}
				addedPoints = points;
				break;
			case PointsModeEnum.Components:
			{
				BGCurvePointComponent[] array2 = new BGCurvePointComponent[num];
				for (int j = 0; j < num; j++)
				{
					BGCurvePoint bGCurvePoint2 = points[j];
					flag = (flag || bGCurvePoint2.PointTransform != null);
					array2[j] = (BGCurvePointComponent)Convert(bGCurvePoint2, PointsModeEnum.Inlined, pointsMode, provider);
				}
				pointsComponents = Insert(pointsComponents, index, array2);
				addedPoints = points;
				break;
			}
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
			{
				BGCurvePointGO[] array = new BGCurvePointGO[num];
				for (int i = 0; i < num; i++)
				{
					BGCurvePoint bGCurvePoint = points[i];
					flag = (flag || bGCurvePoint.PointTransform != null);
					array[i] = (BGCurvePointGO)Convert(bGCurvePoint, PointsModeEnum.Inlined, pointsMode, provider);
				}
				pointsGameObjects = Insert(pointsGameObjects, index, array);
				SetPointsNames();
				addedPoints = array;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
			if (!skipFieldsProcessing && FieldsCount > 0)
			{
				AddFields(pointsMode, addedPoints);
			}
			if (flag)
			{
				CachePointsWithTransforms();
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "add points"));
		}

		private void Delete(int index, Action<BGCurvePointI> destroyer)
		{
			if (index < 0 || index >= PointsCount)
			{
				throw new UnityException("Unable to remove a point. Invalid index: " + index);
			}
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				pointArray[0] = points[index];
				break;
			case PointsModeEnum.Components:
				pointArray[0] = pointsComponents[index];
				break;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				pointArray[0] = pointsGameObjects[index];
				break;
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
			Delete(pointArray, destroyer);
		}

		private void Delete(BGCurvePointI[] pointsToDelete, Action<BGCurvePointI> destroyer)
		{
			if (pointsToDelete == null || pointsToDelete.Length == 0 || PointsCount == 0)
			{
				return;
			}
			pointsList.Clear();
			pointsIndexesList.Clear();
			BGCurvePointI[] array = Points;
			int num = pointsToDelete.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = Array.IndexOf(array, pointsToDelete[i]);
				if (num2 >= 0)
				{
					pointsIndexesList.Add(num2);
				}
			}
			if (pointsIndexesList.Count == 0)
			{
				return;
			}
			FireBeforeChange("delete points");
			int num3 = array.Length - pointsIndexesList.Count;
			BGCurvePointI[] destinationArray;
			switch (pointsMode)
			{
			case PointsModeEnum.Inlined:
				points = new BGCurvePoint[num3];
				destinationArray = points;
				break;
			case PointsModeEnum.Components:
				pointsComponents = new BGCurvePointComponent[num3];
				destinationArray = pointsComponents;
				break;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				pointsGameObjects = new BGCurvePointGO[num3];
				destinationArray = pointsGameObjects;
				break;
			default:
				throw new ArgumentOutOfRangeException("pointsMode");
			}
			pointsIndexesList.Sort();
			int num4 = 0;
			int count = pointsIndexesList.Count;
			for (int j = 0; j < count; j++)
			{
				int num5 = pointsIndexesList[j];
				if (num5 > num4)
				{
					Array.Copy(array, num4, destinationArray, num4 - j, num5 - num4);
				}
				num4 = num5 + 1;
				PointsModeEnum pointsModeEnum = pointsMode;
				if (pointsModeEnum == PointsModeEnum.Components || pointsModeEnum == PointsModeEnum.GameObjectsNoTransform || pointsModeEnum == PointsModeEnum.GameObjectsTransform)
				{
					pointsList.Add(array[num5]);
				}
			}
			if (num4 < array.Length)
			{
				Array.Copy(array, num4, destinationArray, num4 - count, array.Length - num4);
			}
			PointsModeEnum pointsModeEnum2 = pointsMode;
			if (pointsModeEnum2 == PointsModeEnum.GameObjectsNoTransform || pointsModeEnum2 == PointsModeEnum.GameObjectsTransform)
			{
				SetPointsNames();
			}
			if (pointsList.Count > 0)
			{
				int count2 = pointsList.Count;
				for (int k = 0; k < count2; k++)
				{
					BGCurvePointI bGCurvePointI = pointsList[k];
					if (destroyer != null)
					{
						destroyer(bGCurvePointI);
						continue;
					}
					switch (pointsMode)
					{
					case PointsModeEnum.Components:
						DestroyIt((UnityEngine.Object)bGCurvePointI);
						break;
					case PointsModeEnum.GameObjectsNoTransform:
					case PointsModeEnum.GameObjectsTransform:
						DestroyIt(((BGCurvePointGO)bGCurvePointI).gameObject);
						break;
					default:
						throw new ArgumentOutOfRangeException("pointsMode");
					}
				}
			}
			CachePointsWithTransforms();
			pointsList.Clear();
			pointsIndexesList.Clear();
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "delete points"));
		}

		private void ConvertPoints(PointsModeEnum pointsMode, Func<BGCurvePointI> provider = null, Action<BGCurvePointI> destroyer = null)
		{
			PointsModeEnum pointsModeEnum = PointsMode;
			if (pointsModeEnum == pointsMode)
			{
				return;
			}
			FireBeforeChange("points mode is changed");
			int num;
			BGCurvePointI[] array;
			bool flag;
			switch (pointsModeEnum)
			{
			case PointsModeEnum.Inlined:
				if (points.Length <= 0)
				{
					break;
				}
				switch (pointsMode)
				{
				case PointsModeEnum.Components:
				{
					BGCurvePointComponent[] array7 = new BGCurvePointComponent[points.Length];
					for (int num3 = 0; num3 < points.Length; num3++)
					{
						array7[num3] = (BGCurvePointComponent)Convert(points[num3], pointsModeEnum, pointsMode, provider);
					}
					this.pointsMode = pointsMode;
					pointsComponents = Insert(pointsComponents, 0, array7);
					break;
				}
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
				{
					BGCurvePointGO[] array6 = new BGCurvePointGO[points.Length];
					for (int num2 = 0; num2 < points.Length; num2++)
					{
						array6[num2] = (BGCurvePointGO)Convert(points[num2], pointsModeEnum, pointsMode, provider);
					}
					this.pointsMode = pointsMode;
					pointsGameObjects = Insert(pointsGameObjects, 0, array6);
					SetPointsNames();
					if (FieldsCount > 0)
					{
						AddFields(pointsMode, array6);
					}
					break;
				}
				}
				points = new BGCurvePoint[0];
				break;
			case PointsModeEnum.Components:
				if (pointsComponents.Length <= 0)
				{
					goto case PointsModeEnum.GameObjectsNoTransform;
				}
				num = 1;
				goto IL_017b;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				num = ((IsGoMode(pointsModeEnum) && pointsGameObjects.Length > 0) ? 1 : 0);
				goto IL_017b;
			default:
				{
					throw new ArgumentOutOfRangeException("pointsMode");
				}
				IL_017b:
				if (num == 0)
				{
					break;
				}
				array = null;
				if (pointsModeEnum == PointsModeEnum.Components)
				{
					switch (pointsMode)
					{
					case PointsModeEnum.Inlined:
					{
						BGCurvePoint[] array3 = new BGCurvePoint[pointsComponents.Length];
						for (int j = 0; j < pointsComponents.Length; j++)
						{
							array3[j] = (BGCurvePoint)Convert(pointsComponents[j], pointsModeEnum, pointsMode, provider);
						}
						points = Insert(points, 0, array3);
						break;
					}
					case PointsModeEnum.GameObjectsNoTransform:
					case PointsModeEnum.GameObjectsTransform:
					{
						BGCurvePointGO[] array2 = new BGCurvePointGO[pointsComponents.Length];
						for (int i = 0; i < pointsComponents.Length; i++)
						{
							array2[i] = (BGCurvePointGO)Convert(pointsComponents[i], pointsModeEnum, pointsMode, provider);
						}
						pointsGameObjects = Insert(pointsGameObjects, 0, array2);
						SetPointsNames();
						if (FieldsCount > 0)
						{
							AddFields(pointsMode, array2);
						}
						break;
					}
					}
					array = pointsComponents;
					pointsComponents = new BGCurvePointComponent[0];
				}
				else
				{
					switch (pointsMode)
					{
					case PointsModeEnum.Inlined:
					{
						BGCurvePoint[] array4 = new BGCurvePoint[pointsGameObjects.Length];
						for (int l = 0; l < pointsGameObjects.Length; l++)
						{
							array4[l] = (BGCurvePoint)Convert(pointsGameObjects[l], pointsModeEnum, pointsMode, provider);
						}
						points = Insert(points, 0, array4);
						array = pointsGameObjects;
						break;
					}
					case PointsModeEnum.Components:
					{
						BGCurvePointComponent[] array5 = new BGCurvePointComponent[pointsGameObjects.Length];
						for (int m = 0; m < pointsGameObjects.Length; m++)
						{
							array5[m] = (BGCurvePointComponent)Convert(pointsGameObjects[m], pointsModeEnum, pointsMode, provider);
						}
						pointsComponents = Insert(pointsComponents, 0, array5);
						array = pointsGameObjects;
						break;
					}
					case PointsModeEnum.GameObjectsNoTransform:
					case PointsModeEnum.GameObjectsTransform:
						for (int k = 0; k < pointsGameObjects.Length; k++)
						{
							Convert(pointsGameObjects[k], pointsModeEnum, pointsMode, provider);
						}
						break;
					}
					if (array != null)
					{
						pointsGameObjects = new BGCurvePointGO[0];
					}
				}
				this.pointsMode = pointsMode;
				if (array == null)
				{
					break;
				}
				flag = (pointsModeEnum == PointsModeEnum.Components);
				foreach (BGCurvePointI bGCurvePointI in array)
				{
					if (destroyer != null)
					{
						destroyer(bGCurvePointI);
					}
					else
					{
						DestroyIt((!flag) ? ((BGCurvePointGO)bGCurvePointI).gameObject : ((UnityEngine.Object)bGCurvePointI));
					}
				}
				break;
			}
			this.pointsMode = pointsMode;
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Points, "points mode is changed"));
		}

		private static BGCurvePointI Convert(BGCurvePointI point, PointsModeEnum from, PointsModeEnum to, Func<BGCurvePointI> provider)
		{
			BGCurvePointI bGCurvePointI;
			switch (from)
			{
			case PointsModeEnum.Inlined:
				switch (to)
				{
				case PointsModeEnum.Components:
					bGCurvePointI = ((provider != null) ? ((BGCurvePointComponent)provider()) : point.Curve.gameObject.AddComponent<BGCurvePointComponent>());
					((BGCurvePointComponent)bGCurvePointI).PrivateInit((BGCurvePoint)point);
					break;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					bGCurvePointI = ConvertInlineToGo((BGCurvePoint)point, to, provider);
					break;
				default:
					throw new ArgumentOutOfRangeException("to", to, null);
				}
				break;
			case PointsModeEnum.Components:
				switch (to)
				{
				case PointsModeEnum.Inlined:
					bGCurvePointI = ((BGCurvePointComponent)point).Point;
					break;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					bGCurvePointI = ConvertInlineToGo(((BGCurvePointComponent)point).Point, to, provider);
					break;
				default:
					throw new ArgumentOutOfRangeException("to", to, null);
				}
				break;
			case PointsModeEnum.GameObjectsNoTransform:
			case PointsModeEnum.GameObjectsTransform:
				switch (to)
				{
				case PointsModeEnum.Inlined:
					bGCurvePointI = ConvertGoToInline((BGCurvePointGO)point, from);
					break;
				case PointsModeEnum.Components:
					bGCurvePointI = ((provider == null) ? point.Curve.gameObject.AddComponent<BGCurvePointComponent>() : ((BGCurvePointComponent)provider()));
					((BGCurvePointComponent)bGCurvePointI).PrivateInit(ConvertGoToInline((BGCurvePointGO)point, from));
					break;
				case PointsModeEnum.GameObjectsNoTransform:
				case PointsModeEnum.GameObjectsTransform:
					((BGCurvePointGO)point).PrivateInit(null, to);
					bGCurvePointI = point;
					break;
				default:
					throw new ArgumentOutOfRangeException("to", to, null);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("from", from, null);
			}
			Transform pointTransform = point.PointTransform;
			if (pointTransform != null)
			{
				BGCurveReferenceToPoint bGCurveReferenceToPoint = null;
				if (from != 0)
				{
					bGCurveReferenceToPoint = BGCurveReferenceToPoint.GetReferenceToPoint(point);
				}
				if (to != 0)
				{
					if (bGCurveReferenceToPoint == null)
					{
						bGCurveReferenceToPoint = pointTransform.gameObject.AddComponent<BGCurveReferenceToPoint>();
					}
					bGCurveReferenceToPoint.Point = bGCurvePointI;
				}
				else if (bGCurveReferenceToPoint != null)
				{
					DestroyIt(bGCurveReferenceToPoint);
				}
			}
			return bGCurvePointI;
		}

		private static BGCurvePointGO ConvertInlineToGo(BGCurvePoint point, PointsModeEnum to, Func<BGCurvePointI> provider)
		{
			BGCurvePointGO bGCurvePointGO;
			if (provider != null)
			{
				bGCurvePointGO = (BGCurvePointGO)provider();
			}
			else
			{
				GameObject gameObject = new GameObject();
				Transform transform = gameObject.transform;
				transform.parent = point.Curve.transform;
				transform.localRotation = Quaternion.identity;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				bGCurvePointGO = gameObject.AddComponent<BGCurvePointGO>();
			}
			bGCurvePointGO.PrivateInit(point, to);
			bGCurvePointGO.PrivateValuesForFields = point.PrivateValuesForFields;
			return bGCurvePointGO;
		}

		private static BGCurvePoint ConvertGoToInline(BGCurvePointGO pointGO, PointsModeEnum from)
		{
			BGCurvePoint bGCurvePoint;
			switch (from)
			{
			case PointsModeEnum.GameObjectsNoTransform:
				bGCurvePoint = new BGCurvePoint(pointGO.Curve, pointGO.PointTransform, pointGO.PositionLocal, pointGO.ControlType, pointGO.ControlFirstLocal, pointGO.ControlSecondLocal);
				break;
			case PointsModeEnum.GameObjectsTransform:
			{
				Transform transform = (!(pointGO.PointTransform != null)) ? pointGO.Curve.transform : pointGO.PointTransform;
				Vector3 controlFirst = transform.InverseTransformVector(pointGO.ControlFirstLocalTransformed);
				Vector3 controlSecond = transform.InverseTransformVector(pointGO.ControlSecondLocalTransformed);
				bGCurvePoint = new BGCurvePoint(pointGO.Curve, pointGO.PointTransform, pointGO.PositionLocal, pointGO.ControlType, controlFirst, controlSecond);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("PointsModeEnum");
			}
			if (pointGO.Curve.FieldsCount > 0)
			{
				bGCurvePoint.PrivateValuesForFields = pointGO.PrivateValuesForFields;
			}
			return bGCurvePoint;
		}

		private void FireFinalEvent()
		{
			bool hasChanged = base.transform.hasChanged;
			bool flag = forceChangedEventMode == ForceChangedEventModeEnum.EditorAndRuntime;
			if (!hasChanged && immediateChangeEvents && !flag)
			{
				return;
			}
			if (pointsMode == PointsModeEnum.GameObjectsTransform)
			{
				BGCurvePointGO[] array = (BGCurvePointGO[])Points;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					BGCurvePointGO bGCurvePointGO = array[i];
					Transform transform = bGCurvePointGO.gameObject.transform;
					if (transform.hasChanged)
					{
						transform.hasChanged = false;
						changed = true;
						lastEventType = BGCurveChangedArgs.ChangeTypeEnum.Points;
						lastEventMessage = "point position is changed";
					}
				}
			}
			if (pointsWithTransforms != null)
			{
				int count = pointsWithTransforms.Count;
				if (count > 0)
				{
					BGCurvePointI[] array2 = Points;
					int num2 = array2.Length;
					for (int j = 0; j < count; j++)
					{
						int num3 = pointsWithTransforms[j];
						if (num3 < num2)
						{
							BGCurvePointI bGCurvePointI = array2[num3];
							Transform pointTransform = bGCurvePointI.PointTransform;
							if (!(pointTransform == null) && pointTransform.hasChanged)
							{
								pointTransform.hasChanged = false;
								changed = true;
								lastEventType = BGCurveChangedArgs.ChangeTypeEnum.Points;
								lastEventMessage = "point position is changed";
							}
						}
					}
				}
			}
			if (hasChanged || changed || flag)
			{
				if (changed)
				{
					FireChange((!UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(this, lastEventType, lastEventMessage), ignoreEventsGrouping: true);
				}
				else if (hasChanged)
				{
					FireChange((!UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.CurveTransform, "transform is changed"), ignoreEventsGrouping: true);
				}
				else
				{
					FireChange((!UseEventsArgs) ? null : BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Curve, "forced update"), ignoreEventsGrouping: true);
				}
				base.transform.hasChanged = (changed = false);
			}
		}

		private void AddFields(PointsModeEnum pointsMode, BGCurvePointI[] addedPoints)
		{
			foreach (BGCurvePointI point in addedPoints)
			{
				BGCurvePoint.FieldsValues fieldsValues = GetFieldsValues(point, pointsMode);
				BGCurvePointField[] array = fields;
				foreach (BGCurvePointField field in array)
				{
					BGCurvePoint.PrivateFieldAdded(field, fieldsValues);
				}
			}
		}

		private BGCurvePointField AddField(string name, BGCurvePointField.TypeEnum type, Func<BGCurvePointField> provider = null)
		{
			BGCurvePointField.CheckName(this, name, throwException: true);
			FireBeforeChange("add a field");
			BGCurvePointField bGCurvePointField = (provider != null) ? provider() : base.gameObject.AddComponent<BGCurvePointField>();
			bGCurvePointField.hideFlags = HideFlags.HideInInspector;
			bGCurvePointField.Init(this, name, type);
			fields = Insert(fields, fields.Length, bGCurvePointField);
			PrivateUpdateFieldsValuesIndexes();
			if (PointsCount > 0)
			{
				BGCurvePointI[] array = Points;
				BGCurvePointI[] array2 = array;
				foreach (BGCurvePointI point in array2)
				{
					BGCurvePoint.PrivateFieldAdded(bGCurvePointField, GetFieldsValues(point, pointsMode));
				}
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Fields, "add a field"));
			return bGCurvePointField;
		}

		private void DeleteField(BGCurvePointField field, Action<BGCurvePointField> destroyer = null)
		{
			int num = IndexOf(fields, field);
			if (num < 0 || num >= fields.Length)
			{
				throw new UnityException("Unable to remove a fields. Invalid index: " + num);
			}
			int indexOfField = IndexOfFieldValue(field.FieldName);
			FireBeforeChange("delete a field");
			fields = Remove(fields, num);
			PrivateUpdateFieldsValuesIndexes();
			if (PointsCount > 0)
			{
				BGCurvePointI[] array = Points;
				BGCurvePointI[] array2 = array;
				foreach (BGCurvePointI point in array2)
				{
					BGCurvePoint.PrivateFieldDeleted(field, indexOfField, GetFieldsValues(point, pointsMode));
				}
			}
			if (destroyer == null)
			{
				DestroyIt(field);
			}
			else
			{
				destroyer(field);
			}
			FireChange(BGCurveChangedArgs.GetInstance(this, BGCurveChangedArgs.ChangeTypeEnum.Fields, "delete a field"));
		}

		private void CachePointsWithTransforms()
		{
			if (pointsWithTransforms != null)
			{
				pointsWithTransforms.Clear();
			}
			BGCurvePointI[] array = Points;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (array[i].PointTransform != null)
				{
					if (pointsWithTransforms == null)
					{
						pointsWithTransforms = new List<int>();
					}
					pointsWithTransforms.Add(i);
				}
			}
		}

		public static void DestroyIt(UnityEngine.Object obj)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
