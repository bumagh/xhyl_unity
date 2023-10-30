using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class PathGroup : Path
	{
		[SerializeField]
		protected List<Path> _pathList = new List<Path>();

		[ShowInInspector]
		[InspectorCollapsedFoldout]
		[NotSerialized]
		public List<int> allEventPoints => GetEventPoints(new List<int>());

		public List<Path> pathList => _pathList;

		public PathGroup()
		{
			_hideEquation = true;
		}

		public override List<int> GetEventPoints(List<int> dest, int evtIndexOffset = 0)
		{
			if (_eventPoints == null)
			{
				return dest;
			}
			for (int i = 0; i < _eventPoints.Count; i++)
			{
				dest.Add(_eventPoints[i] + evtIndexOffset);
			}
			List<Path> pathList = _pathList;
			for (int j = 0; j < pathList.Count; j++)
			{
				Path path = pathList[j];
				if (!(path == null))
				{
					path.GetEventPoints(dest, evtIndexOffset);
					evtIndexOffset += path.GetPoints().Count;
				}
			}
			return dest;
		}

		public void AddChild(Path path, bool updatePath = true)
		{
			_pathList.Add(path);
			if (updatePath)
			{
				_updatePath();
			}
		}

		public void RemoveAll()
		{
			_pathList.Clear();
			if (_points != null)
			{
				_points.Clear();
			}
		}

		public override Vector3 GetTangent(EndType type)
		{
			if (_pathList.Count == 0)
			{
				UnityEngine.Debug.Log("PathGroup.GetTangent failed, the _pathList is empty!");
				return Vector3.forward;
			}
			if (type == EndType.Start)
			{
				return _pathList[0].GetTangent(type);
			}
			return _pathList[_pathList.Count - 1].GetTangent(type);
		}

		public override void UpdatePath()
		{
			foreach (Path path in _pathList)
			{
				if (!(path == null))
				{
					path.UpdatePath();
				}
			}
			_updatePath();
		}

		protected override void _onAfterValidate()
		{
		}

		protected override void _onDrawGizmos()
		{
			Gizmos.color = gizmoColor;
			if (_pathList.Count > 1)
			{
				for (int i = 0; i < _pathList.Count - 1; i++)
				{
					Path path = _pathList[i];
					if (path == null)
					{
						continue;
					}
					Path path2 = _pathList[i + 1];
					while (path2 == null)
					{
						if (++i >= _pathList.Count - 1)
						{
							return;
						}
						path2 = _pathList[i + 1];
					}
					if (path.pointCount > 0 && path2.pointCount > 0)
					{
						Vector3 point = path.GetPoint(EndType.End);
						Vector3 point2 = path2.GetPoint(EndType.Start);
						Gizmos.DrawLine(point, point2);
					}
				}
				if (closed)
				{
					Path path3 = _pathList[_pathList.Count - 1];
					Path path4 = _pathList[0];
					if (path3.pointCount > 0 && path4.pointCount > 0)
					{
						Gizmos.DrawLine(path3.GetPoint(EndType.End), path4.GetPoint(EndType.Start));
					}
				}
			}
			if (_eventPoints == null)
			{
				return;
			}
			for (int j = 0; j < _eventPoints.Count; j++)
			{
				int num = _eventPoints[j];
				if (num < _points.Count)
				{
					Gizmos.DrawWireSphere(_points[num], 0.1f);
				}
			}
		}

		protected override void _updatePath()
		{
			if (_pathList == null || _pathList.Count == 0)
			{
				_clearPath();
				return;
			}
			_points = new List<Vector4>();
			_length = 0f;
			for (int i = 0; i < _pathList.Count; i++)
			{
				Path path = _pathList[i];
				if (!(path == null))
				{
					_length += path.FillPoints(_points);
				}
			}
			_updateEventData(_eventPoints, _points);
		}
	}
}
