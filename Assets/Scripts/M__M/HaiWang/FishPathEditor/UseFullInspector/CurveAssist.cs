using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[RequireComponent(typeof(BGCurve), typeof(BGCcMath))]
	public class CurveAssist : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		private BGCurve curve;

		private BGCcMath math;

		[SerializeField]
		private bool _realtimeEditor;

		[SerializeField]
		[InspectorCollapsedFoldout]
		private List<CurveClip> _clips;

		private int _clipsCount;

		public bool log;

		public static string rotateFieldName = "rotate";

		public bool addOffsetRotate;

		[SerializeField]
		[InspectorCollapsedFoldout]
		[InspectorShowIf("addOffsetRotate")]
		private List<OffsetRotateItem> _rotateItems;

		private float _validateTime;

		private int _validateCount;

		private Stopwatch _validateWatch = new Stopwatch();

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
			if (curve.Mode2D != BGCurve.Mode2DEnum.XY)
			{
				curve.Mode2D = BGCurve.Mode2DEnum.XY;
			}
			if (math == null)
			{
				math = curve.gameObject.AddComponent<BGCcMath>();
			}
			if (math.Fields != BGCurveBaseMath.Fields.PositionAndTangent)
			{
				math.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
			}
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
			if (log)
			{
				UnityEngine.Debug.Log("CurveAssist.OnValidate() once");
			}
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (!base.gameObject.activeInHierarchy || !base.isActiveAndEnabled)
			{
				return;
			}
			WatchValidate();
			Check();
			if (_clips != null)
			{
				int count = _clips.Count;
				if (count != _clipsCount)
				{
					_clipsCount = count;
				}
				int num = 0;
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
					curveClip.CheckAndOnValidate();
					if (!curveClip.active)
					{
						continue;
					}
					if (num == 0)
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
					num++;
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
			if (_clips == null)
			{
				_clips = new List<CurveClip>();
			}
			int clipsPointCount = GetClipsPointCount();
			LineClip item = new LineClip(curve, math);
			_clips.Add(item);
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
					_clips.Add(curveClip2);
				}
			}
		}

		[InspectorButton]
		[InspectorName("clone尾部clip 10次")]
		private void CloneLastClipX10()
		{
			for (int i = 0; i < 10; i++)
			{
				CloneLastClip();
			}
		}

		[InspectorButton]
		[InspectorName("全部移除，并清理点")]
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
			TailInfo prevTailInfo = null;
			curve.Clear();
			for (int i = 0; i < count; i++)
			{
				CurveClip curveClip = _clips[i];
				if (CheckClipValid(curveClip))
				{
					curveClip.prevTailInfo = prevTailInfo;
					curveClip.CheckAndOnValidate();
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
				if (log)
				{
					UnityEngine.Debug.Log($"valid clips [{num}/{count}]. making curve successful!");
				}
				ApplyOffsetRotateField();
			}
		}

		[InspectorButton]
		[InspectorName("应用OffsetRotateField")]
		private void ApplyOffsetRotateField()
		{
			if (addOffsetRotate)
			{
				if (!curve.HasField(rotateFieldName))
				{
					curve.AddField(rotateFieldName, BGCurvePointField.TypeEnum.Float);
				}
				BGCurvePointI[] points = curve.Points;
				foreach (BGCurvePointI bGCurvePointI in points)
				{
					bGCurvePointI.SetField(rotateFieldName, 0f);
				}
				int j = 0;
				for (int count = _rotateItems.Count; j < count; j++)
				{
					OffsetRotateItem offsetRotateItem = _rotateItems[j];
					if (offsetRotateItem.index <= curve.PointsCount - 1)
					{
						UnityEngine.Debug.Log($"add rotate> index:{offsetRotateItem.index}, angle:{offsetRotateItem.angle}");
						BGCurvePointI bGCurvePointI2 = curve[offsetRotateItem.index];
						bGCurvePointI2.SetField(rotateFieldName, offsetRotateItem.angle);
					}
				}
			}
			else if (curve.HasField(rotateFieldName))
			{
				curve.DeleteField(curve.Fields[curve.IndexOfFieldValue(rotateFieldName)]);
			}
			curve.PrivateUpdateFieldsValuesIndexes();
		}

		private bool CheckClipValid(CurveClip clip)
		{
			return clip != null && clip.active && clip.CheckValid();
		}

		private void AdjustClips()
		{
			if (_clips != null)
			{
				int num = 0;
				for (int i = 0; i < _clips.Count; i++)
				{
					CurveClip curveClip = _clips[i];
					curveClip.startIndex = num;
					num += curveClip.pointCount;
				}
			}
		}

		private int GetClipsPointCount()
		{
			if (_clips == null)
			{
				return 0;
			}
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
