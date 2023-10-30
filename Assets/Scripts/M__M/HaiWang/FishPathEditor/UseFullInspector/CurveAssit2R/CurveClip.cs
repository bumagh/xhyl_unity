using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public abstract class CurveClip
	{
		[InspectorOrder(0.0)]
		public bool active;

		[InspectorDisabled]
		[InspectorOrder(1.0)]
		public BGCurve curve;

		[NotSerialized]
		public BGCcMath math;

		protected int _pointCount;

		[InspectorDisabled]
		[InspectorOrder(3.0)]
		public int startIndex;

		protected bool needMakePoints = true;

		[InspectorDisabled]
		public bool isHeadClip;

		[InspectorDisabled]
		public bool isTailClip;

		[InspectorShowIf("isHeadClip")]
		public Vector3 headStartPoint;

		protected bool _usePrevTail;

		[SerializeField]
		[HideInInspector]
		protected bool _disableUsePrevTail = true;

		public Action<CurveClip> destroyer;

		public Action<string, object> changed;

		[InspectorDisabled]
		[InspectorCollapsedFoldout]
		[SerializeField]
		[InspectorHideIf("isHeadClip")]
		public TailInfo prevTailInfo;

		[HideInInspector]
		[SerializeField]
		protected TailInfo _tailInfo;

		[ShowInInspector]
		[InspectorOrder(2.0)]
		public int pointCount => _pointCount;

		[InspectorDisabledIf("_disableUsePrevTail")]
		[SerializeField]
		[InspectorHideIf("isHeadClip")]
		public bool usePrevTail
		{
			get
			{
				return _usePrevTail;
			}
			set
			{
				if (_usePrevTail != value)
				{
					_usePrevTail = value;
					FireChanged("useTail");
				}
			}
		}

		public CurveClip()
		{
			_tailInfo = new TailInfo();
		}

		public CurveClip(BGCurve curve, BGCcMath math)
		{
			_tailInfo = new TailInfo();
			this.curve = curve;
			this.math = math;
		}

		public virtual void Work(BGCurve curve)
		{
			this.curve = curve;
		}

		public virtual void Check()
		{
			bool raiseExceptions = Assert.raiseExceptions;
			Assert.raiseExceptions = true;
			if (_tailInfo == null)
			{
				_tailInfo = new TailInfo();
			}
			Assert.raiseExceptions = raiseExceptions;
		}

		public virtual void MakePoints()
		{
		}

		public virtual void UpdatePoints()
		{
		}

		public virtual void Remove()
		{
			if (destroyer != null)
			{
				destroyer(this);
			}
		}

		protected virtual void FireChanged(string reason, object arg = null)
		{
			if (changed != null)
			{
				changed(reason, arg);
			}
		}

		public virtual TailInfo GetTailInfo()
		{
			return _tailInfo.Clone();
		}

		protected Vector3 GetStartPosition()
		{
			if (isHeadClip)
			{
				return headStartPoint;
			}
			return (usePrevTail && prevTailInfo != null) ? prevTailInfo.position : TailInfo.Default.position;
		}

		protected Vector3 GetStartTangent()
		{
			if (isHeadClip)
			{
				return TailInfo.Default.tangent;
			}
			return (usePrevTail && prevTailInfo != null) ? prevTailInfo.tangent : TailInfo.Default.tangent;
		}

		public virtual void Refresh()
		{
		}

		public virtual CurveClip Clone()
		{
			return null;
		}
	}
}
