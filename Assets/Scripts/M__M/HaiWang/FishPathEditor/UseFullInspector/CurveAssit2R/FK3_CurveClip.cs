using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public abstract class FK3_CurveClip
	{
		[InspectorOrder(0.0)]
		public bool active;

		[InspectorOrder(1.0)]
		[InspectorDisabled]
		public BGCurve curve;

		[NotSerialized]
		public BGCcMath math;

		protected int _pointCount;

		[InspectorOrder(3.0)]
		[InspectorDisabled]
		public int startIndex;

		protected bool needMakePoints = true;

		[InspectorDisabled]
		public bool isHeadClip;

		[InspectorDisabled]
		public bool isTailClip;

		[InspectorShowIf("isHeadClip")]
		public Vector3 headStartPoint;

		protected bool _usePrevTail;

		[HideInInspector]
		[SerializeField]
		protected bool _disableUsePrevTail = true;

		public Action<FK3_CurveClip> destroyer;

		public Action<string, object> changed;

		[InspectorHideIf("isHeadClip")]
		[InspectorDisabled]
		[InspectorCollapsedFoldout]
		[SerializeField]
		public FK3_TailInfo prevTailInfo;

		[SerializeField]
		[HideInInspector]
		protected FK3_TailInfo _tailInfo;

		[ShowInInspector]
		[InspectorOrder(2.0)]
		public int pointCount => _pointCount;

		[InspectorDisabledIf("_disableUsePrevTail")]
		[InspectorHideIf("isHeadClip")]
		[SerializeField]
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

		public FK3_CurveClip()
		{
			_tailInfo = new FK3_TailInfo();
		}

		public FK3_CurveClip(BGCurve curve, BGCcMath math)
		{
			_tailInfo = new FK3_TailInfo();
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
				_tailInfo = new FK3_TailInfo();
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

		public virtual FK3_TailInfo GetTailInfo()
		{
			return _tailInfo.Clone();
		}

		protected Vector3 GetStartPosition()
		{
			if (isHeadClip)
			{
				return headStartPoint;
			}
			return (usePrevTail && prevTailInfo != null) ? prevTailInfo.position : FK3_TailInfo.Default.position;
		}

		protected Vector3 GetStartTangent()
		{
			if (isHeadClip)
			{
				return FK3_TailInfo.Default.tangent;
			}
			return (usePrevTail && prevTailInfo != null) ? prevTailInfo.tangent : FK3_TailInfo.Default.tangent;
		}

		public virtual void Refresh()
		{
		}

		public virtual FK3_CurveClip Clone()
		{
			return null;
		}
	}
}
