using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	[RequireComponent(typeof(BGCurve))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class FK3_CurveAssist2 : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		private BGCurve curve;

		private BGCcMath math;

		[SerializeField]
		private bool _realtimeEditor;

		private List<FK3_CurveClip> _clips = new List<FK3_CurveClip>();

		private int _clipsCount;

		private float _validateTime;

		private int _validateCount;

		private Stopwatch _validateWatch = new Stopwatch();

		[SerializeField]
		[InspectorCollapsedFoldout]
		private List<FK3_CurveClip> clips
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
					UnityEngine.Debug.Log($"FK3_CurveAssist.OnValidate() busy. count[{_validateCount}]");
				}
				_validateCount = 0;
			}
			if (!_validateWatch.IsRunning)
			{
				_validateWatch.Start();
			}
			UnityEngine.Debug.Log("FK3_CurveAssist.OnValidate() once");
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
					FK3_CurveClip fK3_CurveClip = _clips[i];
					if (fK3_CurveClip == null)
					{
						continue;
					}
					if (fK3_CurveClip.curve == null)
					{
						fK3_CurveClip.curve = curve;
					}
					if (fK3_CurveClip.math == null)
					{
						fK3_CurveClip.math = math;
					}
					fK3_CurveClip.Check();
					if (i == 0)
					{
						if (fK3_CurveClip.prevTailInfo != null)
						{
							fK3_CurveClip.prevTailInfo = null;
						}
						fK3_CurveClip.isHeadClip = true;
					}
					else
					{
						fK3_CurveClip.isHeadClip = false;
						fK3_CurveClip.Refresh();
					}
					if (i == _clipsCount - 1)
					{
						fK3_CurveClip.isTailClip = true;
					}
				}
			}
			if (_realtimeEditor)
			{
				MakeCurveByClips();
			}
		}

		[InspectorName("尾部增加直线")]
		[InspectorButton]
		private void TailAddLineClip()
		{
			int clipsPointCount = GetClipsPointCount();
			FK3_LineClip item = new FK3_LineClip(curve, math);
			clips.Add(item);
		}

		[InspectorName("clone尾部clip")]
		[InspectorButton]
		private void CloneLastClip()
		{
			if (_clips == null || _clips.Count <= 0)
			{
				return;
			}
			FK3_CurveClip fK3_CurveClip = _clips[_clips.Count - 1];
			if (fK3_CurveClip != null)
			{
				FK3_CurveClip fK3_CurveClip2 = fK3_CurveClip.Clone();
				if (fK3_CurveClip2 != null)
				{
					clips.Add(fK3_CurveClip2);
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

		[InspectorButton]
		[InspectorName("全部移除，保留点")]
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
			FK3_TailInfo prevTailInfo = null;
			curve.Clear();
			for (int i = 0; i < count; i++)
			{
				FK3_CurveClip fK3_CurveClip = _clips[i];
				if (CheckClipValid(fK3_CurveClip))
				{
					fK3_CurveClip.prevTailInfo = prevTailInfo;
					fK3_CurveClip.Check();
					fK3_CurveClip.Refresh();
					fK3_CurveClip.startIndex = num2;
					fK3_CurveClip.MakePoints();
					num2 += fK3_CurveClip.pointCount;
					prevTailInfo = fK3_CurveClip.GetTailInfo();
					num++;
				}
			}
			if (!flag)
			{
				UnityEngine.Debug.Log($"valid clips [{num}/{count}]. making curve successful!");
			}
		}

		private bool CheckClipValid(FK3_CurveClip clip)
		{
			return clip != null;
		}

		private void AdjustClips()
		{
			int num = 0;
			for (int i = 0; i < _clips.Count; i++)
			{
				FK3_CurveClip fK3_CurveClip = _clips[i];
				fK3_CurveClip.startIndex = num;
				num += fK3_CurveClip.pointCount;
			}
		}

		private int GetClipsPointCount()
		{
			int num = 0;
			for (int i = 0; i < _clips.Count; i++)
			{
				FK3_CurveClip fK3_CurveClip = _clips[i];
				if (fK3_CurveClip != null)
				{
					num += fK3_CurveClip.pointCount;
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
