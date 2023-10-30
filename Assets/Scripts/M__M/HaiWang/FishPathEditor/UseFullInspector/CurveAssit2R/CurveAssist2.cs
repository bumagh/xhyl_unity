using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	[RequireComponent(typeof(BGCurve))]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class CurveAssist2 : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		private BGCurve curve;

		private BGCcMath math;

		[SerializeField]
		private bool _realtimeEditor;

		private List<CurveClip> _clips = new List<CurveClip>();

		private int _clipsCount;

		private float _validateTime;

		private int _validateCount;

		private Stopwatch _validateWatch = new Stopwatch();

		[InspectorCollapsedFoldout]
		[SerializeField]
		private List<CurveClip> clips
		{
			get
			{
				return _clips;
			}
			set
			{
				_clips = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			curve = GetComponent<BGCurve>();
			math = GetComponent<BGCcMath>();
			Check();
		}

		private void Check()
		{
			if (curve == null)
			{
				curve = base.gameObject.AddComponent<BGCurve>();
			}
			curve.Mode2D = BGCurve.Mode2DEnum.XY;
			if (math == null)
			{
				math = base.gameObject.AddComponent<BGCcMath>();
			}
			math.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
		}

		private void Start()
		{
		}

		private void WatchValidate()
		{
			_validateCount++;
			if ((double)_validateWatch.ElapsedMilliseconds > 1000.0)
			{
				_validateWatch.Stop();
				_validateWatch.Reset();
				if (_validateCount > 10)
				{
					UnityEngine.Debug.Log($"CurveAssist.OnValidate() busy. count[{_validateCount}]");
				}
				_validateCount = 0;
			}
			if (!_validateWatch.IsRunning)
			{
				_validateWatch.Start();
			}
			UnityEngine.Debug.Log("CurveAssist.OnValidate() once");
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			WatchValidate();
			Check();
			if (_clips != null)
			{
				int count = _clips.Count;
				if (count != _clipsCount)
				{
					_clipsCount = count;
				}
				for (int i = 0; i < count; i++)
				{
					CurveClip curveClip = _clips[i];
					if (curveClip == null)
					{
						continue;
					}
					if (curveClip.curve == null)
					{
						curveClip.curve = curve;
					}
					if (curveClip.math == null)
					{
						curveClip.math = math;
					}
					curveClip.Check();
					if (i == 0)
					{
						if (curveClip.prevTailInfo != null)
						{
							curveClip.prevTailInfo = null;
						}
						curveClip.isHeadClip = true;
					}
					else
					{
						curveClip.isHeadClip = false;
						curveClip.Refresh();
					}
					if (i == _clipsCount - 1)
					{
						curveClip.isTailClip = true;
					}
				}
			}
			if (_realtimeEditor)
			{
				MakeCurveByClips();
			}
		}

		[InspectorButton]
		[InspectorName("尾部增加直线")]
		private void TailAddLineClip()
		{
			int clipsPointCount = GetClipsPointCount();
			LineClip item = new LineClip(curve, math);
			clips.Add(item);
		}

		[InspectorButton]
		[InspectorName("clone尾部clip")]
		private void CloneLastClip()
		{
			if (_clips == null || _clips.Count <= 0)
			{
				return;
			}
			CurveClip curveClip = _clips[_clips.Count - 1];
			if (curveClip != null)
			{
				CurveClip curveClip2 = curveClip.Clone();
				if (curveClip2 != null)
				{
					clips.Add(curveClip2);
				}
			}
		}

		[InspectorName("全部移除，并清理点")]
		[InspectorButton]
		private void RemoveAllClipsAndClearPoints()
		{
			_clips.Clear();
			curve.Clear();
		}

		[InspectorName("全部移除，保留点")]
		[InspectorButton]
		private void RemoveAllClipsAndKeepPoints()
		{
			_clips.Clear();
		}

		[InspectorName("根据clips生成curve")]
		[InspectorButton]
		private void MakeCurveByClips()
		{
			if (_clips == null)
			{
				return;
			}
			int count = _clips.Count;
			int num = 0;
			int num2 = 0;
			bool flag = false;
			TailInfo prevTailInfo = null;
			curve.Clear();
			for (int i = 0; i < count; i++)
			{
				CurveClip curveClip = _clips[i];
				if (CheckClipValid(curveClip))
				{
					curveClip.prevTailInfo = prevTailInfo;
					curveClip.Check();
					curveClip.Refresh();
					curveClip.startIndex = num2;
					curveClip.MakePoints();
					num2 += curveClip.pointCount;
					prevTailInfo = curveClip.GetTailInfo();
					num++;
				}
			}
			if (!flag)
			{
				UnityEngine.Debug.Log($"valid clips [{num}/{count}]. making curve successful!");
			}
		}

		private bool CheckClipValid(CurveClip clip)
		{
			return clip != null;
		}

		private void AdjustClips()
		{
			int num = 0;
			for (int i = 0; i < _clips.Count; i++)
			{
				CurveClip curveClip = _clips[i];
				curveClip.startIndex = num;
				num += curveClip.pointCount;
			}
		}

		private int GetClipsPointCount()
		{
			int num = 0;
			for (int i = 0; i < _clips.Count; i++)
			{
				CurveClip curveClip = _clips[i];
				if (curveClip != null)
				{
					num += curveClip.pointCount;
				}
			}
			return num;
		}

		private bool CheckClipsPoints(bool log = false)
		{
			int clipsPointCount = GetClipsPointCount();
			int pointsCount = curve.PointsCount;
			if (clipsPointCount == pointsCount)
			{
				return true;
			}
			if (log)
			{
				UnityEngine.Debug.Log($"Curve points:[{pointsCount}], clips points:[{clipsPointCount}]");
			}
			return false;
		}
	}
}
