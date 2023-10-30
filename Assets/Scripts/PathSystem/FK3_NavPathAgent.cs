using FullInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PathSystem
{
	[ExecuteInEditMode]
	public class FK3_NavPathAgent : BaseBehavior<FullSerializerSerializer>
	{
		[HideInInspector]
		public object userData;

		public FK3_Path path;

		public int loopCount = 1;

		protected int __remainLoopCount;

		public float speed = 5f;

		public bool smoothRotation = true;

		public float rotationOffset;

		public bool moveOnStart;

		protected FK3_NavPathState _state;

		protected int _curPtIndex;

		private List<Vector4> __pathPoints;

		protected float __targetLen;

		protected float __passedLen;

		protected float __remainSegment;

		protected Vector3 _tailDir;

		protected Vector3 _curDir;

		protected Vector3 _headDir;

		protected Quaternion __startDir;

		protected Quaternion __targetDir;

		[SerializeField]
		protected Dictionary<int, FK3_PointEvent> _pointEventsListeners;

		public FK3_NavPathState state => _state;

		public event Action<FK3_NavPathAgent> OnStart;

		public event Action<FK3_NavPathAgent> OnLoop;

		public event Action<FK3_NavPathAgent> OnComplete;

		public FK3_NavPathAgent()
		{
			_state = FK3_NavPathState.Stopped;
		}

		private void Start()
		{
			if (moveOnStart)
			{
				StartMove();
			}
		}

		public void StartMove(FK3_Path path = null)
		{
			if (path != null)
			{
				this.path = path;
			}
			if (this.path == null)
			{
				UnityEngine.Debug.Log("FK3_NavPathAgent.StartMove failed, path is null!");
				return;
			}
			__pathPoints = this.path.GetPoints();
			if (__pathPoints == null)
			{
				UnityEngine.Debug.Log("FK3_NavPathAgent.StartMove failed, path has no point!");
				return;
			}
			if (this.OnStart != null)
			{
				this.OnStart(this);
			}
			__remainLoopCount = loopCount;
			_doStartMove();
		}

		public void PauseMove()
		{
			if (_state != FK3_NavPathState.Moving)
			{
				UnityEngine.Debug.LogWarning("Current state is not FK3_State.Moving, resume failed!");
			}
			else
			{
				_state = FK3_NavPathState.Paused;
			}
		}

		public void ResumeMove()
		{
			if (_state != FK3_NavPathState.Paused)
			{
				UnityEngine.Debug.LogWarning("Current state is not FK3_State.Paused, resume failed!");
			}
			else
			{
				_state = FK3_NavPathState.Moving;
			}
		}

		public void StopMove()
		{
			_state = FK3_NavPathState.Stopped;
			_curDir = Vector3.zero;
			__targetLen = 0f;
			__passedLen = 0f;
			__remainSegment = 0f;
			__remainLoopCount = 0;
			_curPtIndex = 0;
			__pathPoints = null;
		}

		public void Forward(float time)
		{
			if (_state != FK3_NavPathState.Moving)
			{
				UnityEngine.Debug.LogWarning("Forward is valid in moving state only!");
				return;
			}
			for (float num = _update(time); num > 0f; num = _update(num))
			{
			}
		}

		public void Reverse()
		{
			if (state == FK3_NavPathState.Stopped)
			{
				UnityEngine.Debug.Log("Reverse failed, current state is FK3_State.Stopped, cannot do reversing!");
			}
		}

		public void AddEventListener(int index, UnityAction<int, FK3_NavPathAgent> handler)
		{
			if (_pointEventsListeners == null)
			{
				_pointEventsListeners = new Dictionary<int, FK3_PointEvent>();
			}
			if (!_pointEventsListeners.ContainsKey(index))
			{
				_pointEventsListeners.Add(index, new FK3_PointEvent());
			}
			_pointEventsListeners[index].AddListener(handler);
		}

		public void RemoveEventListener(int index, UnityAction<int, FK3_NavPathAgent> handler)
		{
			if (_pointEventsListeners == null)
			{
				_pointEventsListeners = new Dictionary<int, FK3_PointEvent>();
			}
			if (!_pointEventsListeners.ContainsKey(index))
			{
				UnityEngine.Debug.LogWarning("RemoveEventListener failed, the handler is not registered!");
			}
			else
			{
				_pointEventsListeners[index].RemoveListener(handler);
			}
		}

		public void _doStartMove()
		{
			if (path == null)
			{
				UnityEngine.Debug.LogWarning("StartMove failed, please specify path first!");
				return;
			}
			if (__pathPoints.Count == 0)
			{
				UnityEngine.Debug.LogWarning("Path: " + path.GetName() + " has no point!");
				return;
			}
			if (__pathPoints.Count == 1)
			{
				UnityEngine.Debug.LogWarning("Path: " + path.GetName() + " has only one point!");
				return;
			}
			_state = FK3_NavPathState.Moving;
			_curPtIndex = 0;
			__targetLen = Vector3.Distance(__pathPoints[_curPtIndex + 1], __pathPoints[_curPtIndex]);
			__passedLen = 0f;
			__remainSegment = (path.IsClosed() ? __pathPoints.Count : (__pathPoints.Count - 1));
			Vector4 v = __pathPoints[0];
			base.transform.position = v;
			if (_pointEventsListeners != null && v.w == 1f && _pointEventsListeners.ContainsKey(_curPtIndex))
			{
				_pointEventsListeners[_curPtIndex].Invoke(_curPtIndex, this);
			}
			int count = __pathPoints.Count;
			if (_canSmoothDir())
			{
				if (path.IsClosed())
				{
					_tailDir = Vector3.Normalize(__pathPoints[_curPtIndex] - __pathPoints[_preIndex(_curPtIndex, 1, count)]);
					_curDir = Vector3.Normalize(__pathPoints[_nextIndex(_curPtIndex, 1, count)] - __pathPoints[_curPtIndex]);
					_headDir = Vector3.Normalize(__pathPoints[_nextIndex(_curPtIndex, 2, count)] - __pathPoints[_nextIndex(_curPtIndex, 1, count)]);
					__startDir = _getAverageDir(_tailDir, _curDir);
					__targetDir = _getAverageDir(_curDir, _headDir);
				}
				else
				{
					_curDir = Vector3.Normalize(__pathPoints[_nextIndex(_curPtIndex, 1, count)] - __pathPoints[_curPtIndex]);
					_tailDir = _curDir;
					_headDir = Vector3.Normalize(__pathPoints[_nextIndex(_curPtIndex, 2, count)] - __pathPoints[_nextIndex(_curPtIndex, 1, count)]);
					__startDir = _getAverageDir(_tailDir, _curDir);
					__targetDir = _getAverageDir(_curDir, _headDir);
				}
				base.transform.rotation = __startDir;
			}
			else
			{
				_curDir = Vector3.Normalize(__pathPoints[_curPtIndex + 1] - __pathPoints[_curPtIndex]);
				base.transform.rotation = Quaternion.AngleAxis(57.29578f * Mathf.Atan2(_curDir.y, _curDir.x) + rotationOffset, Vector3.forward);
			}
		}

		private void Update()
		{
			float num = _update(Time.deltaTime);
			int num2 = 0;
			while (num > 0f)
			{
				num2++;
				num = _update(num);
				if (num2 > 10)
				{
					break;
				}
			}
		}

		protected float _update(float deltaTime)
		{
			if (_state != FK3_NavPathState.Moving)
			{
				return 0f;
			}
			float num = deltaTime * speed;
			__passedLen += num;
			if (__passedLen >= __targetLen)
			{
				__remainSegment -= 1f;
				if (__remainSegment == 0f)
				{
					__remainLoopCount--;
					if (__remainLoopCount == 0)
					{
						base.transform.position = _getEndPoint();
						if (smoothRotation)
						{
							base.transform.rotation = __targetDir;
						}
						_state = FK3_NavPathState.Stopped;
						if (this.OnComplete != null)
						{
							this.OnComplete(this);
						}
						return 0f;
					}
					if (this.OnLoop != null)
					{
						this.OnLoop(this);
					}
					if (!path.IsClosed())
					{
						_doStartMove();
						return 0f;
					}
					__remainSegment = __pathPoints.Count;
				}
				int count = __pathPoints.Count;
				_curPtIndex = _nextIndex(_curPtIndex, 1, count);
				float result = (__passedLen - __targetLen) / speed;
				__passedLen = 0f;
				__targetLen = Vector3.Distance(__pathPoints[_curPtIndex], __pathPoints[_nextIndex(_curPtIndex, 1, count)]);
				Vector4 v = __pathPoints[_curPtIndex];
				base.transform.position = v;
				if (_pointEventsListeners != null && v.w == 1f && _pointEventsListeners.ContainsKey(_curPtIndex))
				{
					_pointEventsListeners[_curPtIndex].Invoke(_curPtIndex, this);
				}
				if (_canSmoothDir())
				{
					if (path.IsClosed() || _curPtIndex < count - 2)
					{
						_tailDir = _curDir;
						_curDir = _headDir;
						_headDir = Vector3.Normalize(__pathPoints[_nextIndex(_curPtIndex, 2, count)] - __pathPoints[_nextIndex(_curPtIndex, 1, count)]);
						__startDir = __targetDir;
						__targetDir = _getAverageDir(_curDir, _headDir);
					}
					else
					{
						_tailDir = _curDir;
						_curDir = _headDir;
						__startDir = __targetDir;
						__targetDir = _getDir(_headDir);
					}
					base.transform.rotation = __startDir;
				}
				else
				{
					Vector4 a = __pathPoints[path.IsClosed() ? _nextIndex(_curPtIndex, 1, count) : (_curPtIndex + 1)];
					_curDir = Vector3.Normalize(a - __pathPoints[_curPtIndex]);
					base.transform.rotation = Quaternion.AngleAxis(57.29578f * Mathf.Atan2(_curDir.y, _curDir.x) + rotationOffset, Vector3.forward);
				}
				return result;
			}
			base.transform.position += num * _curDir;
			if (smoothRotation)
			{
				base.transform.rotation = Quaternion.Lerp(__startDir, __targetDir, __passedLen / __targetLen);
			}
			return 0f;
		}

		protected bool _canSmoothDir()
		{
			return smoothRotation && __pathPoints.Count >= 3;
		}

		protected Quaternion _getDir(Vector3 dir)
		{
			return Quaternion.AngleAxis(57.29578f * Mathf.Atan2(dir.y, dir.x) + rotationOffset, Vector3.forward);
		}

		protected Quaternion _getAverageDir(Vector3 dir1, Vector3 dir2)
		{
			Vector3 vector = (dir1 + dir2) / 2f;
			return Quaternion.AngleAxis(57.29578f * Mathf.Atan2(vector.y, vector.x) + rotationOffset, Vector3.forward);
		}

		protected int _preIndex(int index, int amount, int totalCount)
		{
			return (index + totalCount - amount) % totalCount;
		}

		protected int _nextIndex(int index, int amount, int totalCount)
		{
			return (index + amount) % totalCount;
		}

		protected Vector4 _getEndPoint()
		{
			if (path.IsClosed())
			{
				return __pathPoints[0];
			}
			return __pathPoints[__pathPoints.Count - 1];
		}
	}
}
