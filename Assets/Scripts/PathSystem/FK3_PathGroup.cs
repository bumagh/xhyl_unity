using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class FK3_PathGroup : FK3_Path
	{
		[SerializeField]
		protected List<FK3_Path> _pathList = new List<FK3_Path>();

		[InspectorCollapsedFoldout]
		[ShowInInspector]
		[NotSerialized]
		public List<int> allEventPoints => GetEventPoints(new List<int>());

		public List<FK3_Path> pathList => _pathList;

		public FK3_PathGroup()
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
			List<FK3_Path> pathList = _pathList;
			for (int j = 0; j < pathList.Count; j++)
			{
				FK3_Path fK3_Path = pathList[j];
				if (!(fK3_Path == null))
				{
					fK3_Path.GetEventPoints(dest, evtIndexOffset);
					evtIndexOffset += fK3_Path.GetPoints().Count;
				}
			}
			return dest;
		}

		public void AddChild(FK3_Path path, bool updatePath = true)
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

		public override Vector3 GetTangent(FK3_EndType type)
		{
			if (_pathList.Count == 0)
			{
				UnityEngine.Debug.Log("FK3_PathGroup.GetTangent failed, the _pathList is empty!");
				return Vector3.forward;
			}
			if (type == FK3_EndType.Start)
			{
				return _pathList[0].GetTangent(type);
			}
			return _pathList[_pathList.Count - 1].GetTangent(type);
		}

		public override void UpdatePath()
		{
			foreach (FK3_Path path in _pathList)
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
					FK3_Path fK3_Path = _pathList[i];
					if (fK3_Path == null)
					{
						continue;
					}
					FK3_Path fK3_Path2 = _pathList[i + 1];
					while (fK3_Path2 == null)
					{
						if (++i >= _pathList.Count - 1)
						{
							return;
						}
						fK3_Path2 = _pathList[i + 1];
					}
					if (fK3_Path.pointCount > 0 && fK3_Path2.pointCount > 0)
					{
						Vector3 point = fK3_Path.GetPoint(FK3_EndType.End);
						Vector3 point2 = fK3_Path2.GetPoint(FK3_EndType.Start);
						Gizmos.DrawLine(point, point2);
					}
				}
				if (closed)
				{
					FK3_Path fK3_Path3 = _pathList[_pathList.Count - 1];
					FK3_Path fK3_Path4 = _pathList[0];
					if (fK3_Path3.pointCount > 0 && fK3_Path4.pointCount > 0)
					{
						Gizmos.DrawLine(fK3_Path3.GetPoint(FK3_EndType.End), fK3_Path4.GetPoint(FK3_EndType.Start));
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
				FK3_Path fK3_Path = _pathList[i];
				if (!(fK3_Path == null))
				{
					_length += fK3_Path.FillPoints(_points);
				}
			}
			_updateEventData(_eventPoints, _points);
		}
	}
}
