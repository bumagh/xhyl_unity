using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BansheeGz.BGSpline.Components
{
	[DisallowMultipleComponent]
	[CcDescriptor(Description = "Math solver for the curve (position, tangent, total distance, position by closest point). With this component you can use math functions.", Name = "Math", Icon = "BGCcMath123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcMath")]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcMath")]
	public class BGCcMath : BGCc, BGCurveMathI
	{
		public enum MathTypeEnum
		{
			Base,
			Adaptive
		}

		public enum UpdateModeEnum
		{
			Always,
			AabbVisible,
			RendererVisible
		}

		private sealed class VisibilityCheck : MonoBehaviour
		{
			public bool Visible
			{
				get;
				private set;
			}

			public event EventHandler BecameVisible;

			private void OnBecameVisible()
			{
				Visible = true;
				if (this.BecameVisible != null)
				{
					this.BecameVisible(this, null);
				}
			}

			private void OnBecameInvisible()
			{
				Visible = false;
			}
		}

		[Serializable]
		public class MathChangedEvent : UnityEvent
		{
		}

		private const int PartsMax = 100;

		private static readonly Vector3[] EmptyVertices = new Vector3[0];

		[Tooltip("Which fields you want to use.")]
		[SerializeField]
		private BGCurveBaseMath.Fields fields = BGCurveBaseMath.Fields.Position;

		[Tooltip("Math type to use.\r\nBase - uses uniformely split sections;\r\n Adaptive - uses non-uniformely split sections, based on the curvature. Expiremental.")]
		[SerializeField]
		private MathTypeEnum mathType;

		[SerializeField]
		[Range(1f, 100f)]
		[Tooltip("The number of equal parts for each section, used by Base math.")]
		private int sectionParts = 30;

		[Tooltip("Use only 2 points for straight lines. Tangents may be calculated slightly different. Used by Base math.")]
		[SerializeField]
		private bool optimizeStraightLines;

		[Range(0.1f, 0.999975f)]
		[Tooltip("Tolerance, used by Adaptive Math. The bigger the tolerance- the lesser splits. Note: The final tolerance used by Math is based on this value but different.")]
		[SerializeField]
		private float tolerance = 0.2f;

		[Tooltip("Points position will be used for tangent calculation. This can gain some performance")]
		[SerializeField]
		private bool usePositionToCalculateTangents;

		[Tooltip("Updating math takes some resources. You can fine-tune in which cases math is updated.\r\n1) Always- always update\r\n2) AabbVisible- update only if AABB (Axis Aligned Bounding Box) around points and controls is visible\r\n3) RendererVisible- update only if some renderer is visible")]
		[SerializeField]
		private UpdateModeEnum updateMode;

		[Tooltip("Renderer to check for updating math. Math will be updated only if renderer is visible")]
		[SerializeField]
		private Renderer rendererForUpdateCheck;

		[Tooltip("Event is fired, then math is recalculated")]
		[SerializeField]
		private MathChangedEvent mathChangedEvent = new MathChangedEvent();

		private BGCurveBaseMath math;

		private VisibilityCheck visibilityCheck;

		private MeshFilter meshFilter;

		private readonly Vector3[] vertices = new Vector3[2];

		public MathTypeEnum MathType
		{
			get
			{
				return mathType;
			}
			set
			{
				ParamChanged(ref mathType, value);
			}
		}

		public int SectionParts
		{
			get
			{
				return Mathf.Clamp(sectionParts, 1, 100);
			}
			set
			{
				ParamChanged(ref sectionParts, Mathf.Clamp(value, 1, 100));
			}
		}

		public bool OptimizeStraightLines
		{
			get
			{
				return optimizeStraightLines;
			}
			set
			{
				ParamChanged(ref optimizeStraightLines, value);
			}
		}

		public float Tolerance
		{
			get
			{
				return tolerance;
			}
			set
			{
				ParamChanged(ref tolerance, value);
			}
		}

		public BGCurveBaseMath.Fields Fields
		{
			get
			{
				return fields;
			}
			set
			{
				ParamChanged(ref fields, value);
			}
		}

		public bool UsePositionToCalculateTangents
		{
			get
			{
				return usePositionToCalculateTangents;
			}
			set
			{
				ParamChanged(ref usePositionToCalculateTangents, value);
			}
		}

		public UpdateModeEnum UpdateMode
		{
			get
			{
				return updateMode;
			}
			set
			{
				ParamChanged(ref updateMode, value);
			}
		}

		public Renderer RendererForUpdateCheck
		{
			get
			{
				return rendererForUpdateCheck;
			}
			set
			{
				ParamChanged(ref rendererForUpdateCheck, value);
			}
		}

		public string Error => (updateMode != UpdateModeEnum.RendererVisible || !(RendererForUpdateCheck == null)) ? null : ("Update mode is set to " + updateMode + ", however the RendererForUpdateCheck is null");

		public string Warning
		{
			get
			{
				if (base.Curve.SnapType == BGCurve.SnapTypeEnum.Curve && Fields == BGCurveBaseMath.Fields.PositionAndTangent && !UsePositionToCalculateTangents)
				{
					return "Your curve's snap mode is Curve, and you are calculating tangents. However you use formula for tangents, instead of points positions.This may result in wrong tangents. Set UsePositionToCalculateTangents to true.";
				}
				return null;
			}
		}

		public string Info => (Math != null) ? ("Math uses " + Math.PointsCount + " points") : null;

		public bool SupportHandles => true;

		public bool SupportHandlesSettings => true;

		public BGCurveBaseMath Math
		{
			get
			{
				if (math == null)
				{
					InitMath(null, null);
				}
				return math;
			}
		}

		private bool NewMathRequired => math == null || (mathType == MathTypeEnum.Base && math.GetType() != typeof(BGCurveBaseMath)) || (mathType == MathTypeEnum.Adaptive && math.GetType() != typeof(BGCurveAdaptiveMath));

		public BGCurveBaseMath.SectionInfo this[int i] => Math[i];

		public event EventHandler ChangedMath;

		public void Start()
		{
			base.Curve.Changed += SendEventsIfMathIsNotCreated;
		}

		public void OnDestroy()
		{
			if (math != null)
			{
				math.Changed -= MathWasChanged;
				math.ChangeRequested -= MathOnChangeRequested;
				math.Dispose();
			}
			base.ChangedParams -= InitMath;
		}

		public void EnsureMathIsCreated()
		{
			BGCurveBaseMath bGCurveBaseMath = Math;
		}

		public void Recalculate(bool force = false)
		{
			Math.Recalculate(force);
		}

		public bool IsCalculated(BGCurveBaseMath.Field field)
		{
			return Math?.IsCalculated(field) ?? false;
		}

		public float ClampDistance(float distance)
		{
			BGCurveBaseMath bGCurveBaseMath = Math;
			return (distance < 0f) ? 0f : ((!(distance > bGCurveBaseMath.GetDistance())) ? distance : bGCurveBaseMath.GetDistance());
		}

		public float GetDistance(int pointIndex = -1)
		{
			return Math.GetDistance(pointIndex);
		}

		public Vector3 CalcByDistanceRatio(BGCurveBaseMath.Field field, float ratio, bool useLocal = false)
		{
			return Math.CalcByDistanceRatio(field, ratio, useLocal);
		}

		public Vector3 CalcByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false)
		{
			return Math.CalcByDistanceRatio(distanceRatio, out tangent, useLocal);
		}

		public Vector3 CalcPositionByDistanceRatio(float ratio, bool useLocal = false)
		{
			return Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Position, ratio, useLocal);
		}

		public Vector3 CalcTangentByDistanceRatio(float ratio, bool useLocal = false)
		{
			return Math.CalcByDistanceRatio(BGCurveBaseMath.Field.Tangent, ratio, useLocal);
		}

		public Vector3 CalcPositionAndTangentByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false)
		{
			return Math.CalcPositionAndTangentByDistanceRatio(distanceRatio, out tangent, useLocal);
		}

		public Vector3 CalcByDistance(BGCurveBaseMath.Field field, float distance, bool useLocal = false)
		{
			return Math.CalcByDistance(field, distance, useLocal);
		}

		public Vector3 CalcByDistance(float distance, out Vector3 tangent, bool useLocal = false)
		{
			return Math.CalcByDistance(distance, out tangent, useLocal);
		}

		public Vector3 CalcPositionByDistance(float distance, bool useLocal = false)
		{
			return Math.CalcByDistance(BGCurveBaseMath.Field.Position, distance, useLocal);
		}

		public Vector3 CalcTangentByDistance(float distance, bool useLocal = false)
		{
			return Math.CalcByDistance(BGCurveBaseMath.Field.Tangent, distance, useLocal);
		}

		public Vector3 CalcPositionAndTangentByDistance(float distance, out Vector3 tangent, bool useLocal = false)
		{
			return Math.CalcPositionAndTangentByDistance(distance, out tangent, useLocal);
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, out Vector3 tangent, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			return Math.CalcPositionByClosestPoint(point, out distance, out tangent, skipSectionsOptimization, skipPointsOptimization);
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			return Math.CalcPositionByClosestPoint(point, out distance, skipSectionsOptimization, skipPointsOptimization);
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			return Math.CalcPositionByClosestPoint(point, skipSectionsOptimization, skipPointsOptimization);
		}

		public int CalcSectionIndexByDistance(float distance)
		{
			return Math.CalcSectionIndexByDistance(distance);
		}

		public int CalcSectionIndexByDistanceRatio(float distanceRatio)
		{
			return Math.CalcSectionIndexByDistanceRatio(distanceRatio);
		}

		private void SendEventsIfMathIsNotCreated(object sender, BGCurveChangedArgs e)
		{
			if (math != null)
			{
				base.Curve.Changed -= SendEventsIfMathIsNotCreated;
			}
			else
			{
				MathWasChanged(sender, e);
			}
		}

		private void InitMath(object sender, EventArgs e)
		{
			object obj;
			if (mathType == MathTypeEnum.Adaptive)
			{
				BGCurveAdaptiveMath.ConfigAdaptive configAdaptive = new BGCurveAdaptiveMath.ConfigAdaptive(fields);
				configAdaptive.Tolerance = tolerance;
				obj = configAdaptive;
			}
			else
			{
				BGCurveBaseMath.Config config = new BGCurveBaseMath.Config(fields);
				config.Parts = sectionParts;
				obj = config;
			}
			BGCurveBaseMath.Config config2 = (BGCurveBaseMath.Config)obj;
			config2.UsePointPositionsToCalcTangents = usePositionToCalculateTangents;
			config2.OptimizeStraightLines = optimizeStraightLines;
			config2.Fields = fields;
			if (updateMode != 0 && Application.isPlaying)
			{
				switch (updateMode)
				{
				case UpdateModeEnum.AabbVisible:
					InitAabbVisibleBefore(config2);
					break;
				case UpdateModeEnum.RendererVisible:
					InitRendererVisible(config2);
					break;
				}
			}
			if (NewMathRequired)
			{
				bool flag = math == null;
				if (flag)
				{
					base.ChangedParams += InitMath;
				}
				else
				{
					math.ChangeRequested -= MathOnChangeRequested;
					math.Changed -= MathWasChanged;
					math.Dispose();
				}
				if (MathType == MathTypeEnum.Base)
				{
					math = new BGCurveBaseMath(base.Curve, config2);
				}
				else
				{
					math = new BGCurveAdaptiveMath(base.Curve, (BGCurveAdaptiveMath.ConfigAdaptive)config2);
				}
				math.Changed += MathWasChanged;
				if (!flag)
				{
					MathWasChanged(this, null);
				}
			}
			else
			{
				math.ChangeRequested -= MathOnChangeRequested;
				math.Init(config2);
			}
			if (updateMode == UpdateModeEnum.AabbVisible)
			{
				InitAabbVisibleAfter();
			}
		}

		private void InitAabbVisibleBefore(BGCurveBaseMath.Config config)
		{
			if (Application.isPlaying)
			{
				if (meshFilter != null)
				{
					UnityEngine.Object.Destroy(meshFilter.gameObject);
				}
				GameObject gameObject = new GameObject("AabbBox");
				gameObject.transform.parent = base.transform;
				MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
				meshFilter = gameObject.AddComponent<MeshFilter>();
				meshFilter.mesh = new Mesh();
				InitVisibilityCheck(config, renderer);
				MathOnChangeRequested(this, null);
			}
		}

		private void InitAabbVisibleAfter()
		{
			if (Application.isPlaying)
			{
				math.ChangeRequested += MathOnChangeRequested;
			}
		}

		private void MathOnChangeRequested(object sender, EventArgs eventArgs)
		{
			if (!Application.isPlaying || visibilityCheck == null || visibilityCheck.Visible)
			{
				return;
			}
			BGCurvePointI[] points = base.Curve.Points;
			Mesh sharedMesh = meshFilter.sharedMesh;
			switch (points.LongLength)
			{
			case 0L:
				sharedMesh.vertices = EmptyVertices;
				return;
			case 1L:
				vertices[0] = points[0].PositionWorld;
				vertices[1] = vertices[0];
				sharedMesh.vertices = vertices;
				return;
			}
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			Vector3 positionWorld = points[0].PositionWorld;
			int num = points.Length;
			int num2 = num - 1;
			bool closed = base.Curve.Closed;
			Vector3 vector = positionWorld;
			Vector3 vector2 = positionWorld;
			for (int i = 0; i < num; i++)
			{
				BGCurvePointI bGCurvePointI = points[i];
				Vector3 positionLocal = bGCurvePointI.PositionLocal;
				Vector3 vector3 = localToWorldMatrix.MultiplyPoint(positionLocal);
				if (vector.x > vector3.x)
				{
					vector.x = vector3.x;
				}
				if (vector.y > vector3.y)
				{
					vector.y = vector3.y;
				}
				if (vector.z > vector3.z)
				{
					vector.z = vector3.z;
				}
				if (vector2.x < vector3.x)
				{
					vector2.x = vector3.x;
				}
				if (vector2.y < vector3.y)
				{
					vector2.y = vector3.y;
				}
				if (vector2.z < vector3.z)
				{
					vector2.z = vector3.z;
				}
				if (bGCurvePointI.ControlType == BGCurvePoint.ControlTypeEnum.Absent)
				{
					continue;
				}
				if (closed || i != 0)
				{
					Vector3 vector4 = localToWorldMatrix.MultiplyPoint(bGCurvePointI.ControlFirstLocal + positionLocal);
					if (vector.x > vector4.x)
					{
						vector.x = vector4.x;
					}
					if (vector.y > vector4.y)
					{
						vector.y = vector4.y;
					}
					if (vector.z > vector4.z)
					{
						vector.z = vector4.z;
					}
					if (vector2.x < vector4.x)
					{
						vector2.x = vector4.x;
					}
					if (vector2.y < vector4.y)
					{
						vector2.y = vector4.y;
					}
					if (vector2.z < vector4.z)
					{
						vector2.z = vector4.z;
					}
				}
				if (closed || i != num2)
				{
					Vector3 vector5 = localToWorldMatrix.MultiplyPoint(bGCurvePointI.ControlSecondLocal + positionLocal);
					if (vector.x > vector5.x)
					{
						vector.x = vector5.x;
					}
					if (vector.y > vector5.y)
					{
						vector.y = vector5.y;
					}
					if (vector.z > vector5.z)
					{
						vector.z = vector5.z;
					}
					if (vector2.x < vector5.x)
					{
						vector2.x = vector5.x;
					}
					if (vector2.y < vector5.y)
					{
						vector2.y = vector5.y;
					}
					if (vector2.z < vector5.z)
					{
						vector2.z = vector5.z;
					}
				}
			}
			vertices[0] = vector;
			vertices[1] = vector2;
			sharedMesh.vertices = vertices;
		}

		private void InitRendererVisible(BGCurveBaseMath.Config config)
		{
			InitVisibilityCheck(config, RendererForUpdateCheck);
		}

		private void InitVisibilityCheck(BGCurveBaseMath.Config config, Renderer renderer)
		{
			if (visibilityCheck != null)
			{

				UnityEngine.Object.Destroy(visibilityCheck);
			}
			if (!(renderer == null))
			{
				visibilityCheck = renderer.gameObject.AddComponent<VisibilityCheck>();

				config.ShouldUpdate = (() => visibilityCheck.Visible);
			}
		}

		private void BecameVisible(object sender, EventArgs e)
		{
			math.Configuration.FireUpdate();
		}

		private void MathWasChanged(object sender, EventArgs e)
		{
			if (this.ChangedMath != null)
			{
				this.ChangedMath(this, null);
			}
			if (mathChangedEvent.GetPersistentEventCount() > 0)
			{
				mathChangedEvent.Invoke();
			}
		}
	}
}
