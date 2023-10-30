using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class Path : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorHideIf("_hideEquation")]
		[SerializeField]
		public bool autoSmooth;

		public bool closed;

		public bool drawGizmo = true;

		[InspectorHideIf("_hideEquation")]
		[InspectorRange(1f, 500f, float.NaN)]
		[SerializeField]
		public int segmentCount = 32;

		[SerializeField]
		[InspectorHideIf("_hideEquation")]
		protected IEquation _equation;

		protected bool _hideEquation;

		public Color gizmoColor = Color.green;

		[HideInInspector]
		[SerializeField]
		protected List<Vector4> _points;

		[InspectorDisabled]
		[SerializeField]
		protected float _length;

		[SerializeField]
		[InspectorCollapsedFoldout]
		internal List<int> _eventPoints;

		public int pointCount => (_points != null) ? _points.Count : 0;

		public float length => _length;

		public virtual List<int> GetEventPoints(List<int> dest, int evtIndexOffset = 0)
		{
			if (_eventPoints == null)
			{
				return dest;
			}
			for (int i = 0; i < _eventPoints.Count; i++)
			{
				dest.Add(_eventPoints[i] + evtIndexOffset);
			}
			return dest;
		}

		public virtual Vector3 GetTangent(EndType type)
		{
			return _equation.GetTangent(type, base.transform);
		}

		public virtual bool IsClosed()
		{
			return closed;
		}

		public virtual string GetName()
		{
			return base.name;
		}

		public virtual Vector3 GetPoint(EndType type)
		{
			return _points[(type != 0) ? (_points.Count - 1) : 0];
		}

		public virtual Vector3 GetPoint(int index)
		{
			return _points[index];
		}

		public virtual List<Vector4> GetPoints()
		{
			if (_points == null)
			{
				_updatePath();
			}
			return _points;
		}

		public virtual float FillPoints(List<Vector4> points)
		{
			if (_points == null)
			{
				_updatePath();
			}
			if (_points != null)
			{
				points.AddRange(_points);
			}
			return _length;
		}

		public virtual void UpdatePath()
		{
			_updatePath();
		}

		protected virtual void _updatePath()
		{
			if (_equation == null)
			{
				_clearPath();
				return;
			}
			if (_points == null)
			{
				_points = new List<Vector4>();
			}
			_points.Clear();
			_length = _equation.FillPoints(_points, segmentCount, base.transform, closed);
			_updateEventData(_eventPoints, _points);
		}

		protected virtual void _onAfterValidate()
		{
			if (autoSmooth)
			{
				int num = segmentCount = Mathf.Clamp((int)(_length * 2f), 6, 500);
				_updatePath();
			}
		}

		protected virtual void _onDrawGizmos()
		{
			if (_points == null || _points.Count == 0)
			{
				return;
			}
			Gizmos.color = gizmoColor;
			Gizmos.DrawWireCube(_points[0], Vector3.one / 4f);
			Utils.DrawLine(_points, closed);
			if (closed)
			{
				DrawArrow.ForGizmo(_points[0], GetTangent(EndType.End) / 10f, 0.7f);
			}
			else
			{
				DrawArrow.ForGizmo(_points[_points.Count - 1], GetTangent(EndType.End) / 10f, 0.7f);
			}
			if (_eventPoints == null)
			{
				return;
			}
			for (int i = 0; i < _eventPoints.Count; i++)
			{
				int num = _eventPoints[i];
				if (num < _points.Count)
				{
					Gizmos.DrawWireSphere(_points[num], 0.1f);
				}
			}
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			_updatePath();
			_onAfterValidate();
		}

		protected override void Awake()
		{
			base.Awake();
			if (_points == null)
			{
				UnityEngine.Debug.LogWarning("No points found, will reconstruct point list...");
				_updatePath();
			}
		}

		private void OnDrawGizmos()
		{
			if (base.transform.hasChanged)
			{
				_updatePath();
			}
			if (drawGizmo)
			{
				_onDrawGizmos();
			}
		}

		protected void _clearPath()
		{
			if (_points != null)
			{
				_points.Clear();
			}
			_length = 0f;
		}

		protected void _updateEventData(List<int> eventPoints, List<Vector4> dest, int offset = 0)
		{
			if (eventPoints == null)
			{
				return;
			}
			int count = eventPoints.Count;
			int count2 = _points.Count;
			for (int i = 0; i < count; i++)
			{
				int num = eventPoints[i] + offset;
				if (num < count2)
				{
					Vector4 vector = _points[num];
					dest[num] = new Vector4(vector.x, vector.y, vector.z, 1f);
				}
			}
		}
	}
}
