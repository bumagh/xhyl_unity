using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	[RequireComponent(typeof(BGCurve), typeof(BGCcMath))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class FK3_CurveAssist : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		private BGCurve curve;

		private BGCcMath math;

		[SerializeField]
		private bool _realtimeEditor;

		[SerializeField]
		[InspectorCollapsedFoldout]
		private List<FK3_CurveClip> _clips;

		private int _clipsCount;

		public bool log;

		public static string rotateFieldName = "rotate";

		public bool addOffsetRotate;

		[InspectorCollapsedFoldout]
		[SerializeField]
		[InspectorShowIf("addOffsetRotate")]
		private List<FK3_OffsetRotateItem> _rotateItems;

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
					UnityEngine.Debug.Log($"FK3_CurveAssist.OnValidate() busy. count[{_validateCount}]");
				}
				_validateCount = 0;
			}
			if (!_validateWatch.IsRunning)
			{
				_validateWatch.Start();
			}
			if (log)
			{
				UnityEngine.Debug.Log("FK3_CurveAssist.OnValidate() once");
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
					fK3_CurveClip.CheckAndOnValidate();
					if (!fK3_CurveClip.active)
					{
						continue;
					}
					if (num == 0)
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
				_clips = new List<FK3_CurveClip>();
			}
			int clipsPointCount = GetClipsPointCount();
			FK3_LineClip item = new FK3_LineClip(curve, math);
			_clips.Add(item);
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
					_clips.Add(fK3_CurveClip2);
				}
			}
		}

		[InspectorName("clone尾部clip 10次")]
		[InspectorButton]
		private void CloneLastClipX10()
		{
			for (int i = 0; i < 10; i++)
			{
				CloneLastClip();
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

		[InspectorButton]
		[InspectorName("根据clips生成curve")]
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
					fK3_CurveClip.CheckAndOnValidate();
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
					FK3_OffsetRotateItem fK3_OffsetRotateItem = _rotateItems[j];
					if (fK3_OffsetRotateItem.index <= curve.PointsCount - 1)
					{
						UnityEngine.Debug.Log($"add rotate> index:{fK3_OffsetRotateItem.index}, angle:{fK3_OffsetRotateItem.angle}");
						BGCurvePointI bGCurvePointI2 = curve[fK3_OffsetRotateItem.index];
						bGCurvePointI2.SetField(rotateFieldName, fK3_OffsetRotateItem.angle);
					}
				}
			}
			else if (curve.HasField(rotateFieldName))
			{
				curve.DeleteField(curve.Fields[curve.IndexOfFieldValue(rotateFieldName)]);
			}
			curve.PrivateUpdateFieldsValuesIndexes();
		}

		private bool CheckClipValid(FK3_CurveClip clip)
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
					FK3_CurveClip fK3_CurveClip = _clips[i];
					fK3_CurveClip.startIndex = num;
					num += fK3_CurveClip.pointCount;
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
