using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.CurveAssit2R
{
	public class CircleClip : CurveClip
	{
		private bool _clockwise = true;

		private float _radius = 1f;

		private float _radiusAngle;

		[ShowInInspector]
		[InspectorDisabled]
		[NotSerialized]
		public Vector3 center = Vector3.zero;

		private CircleCurveMaker maker;

		[SerializeField]
		public bool clockwise
		{
			get
			{
				return _clockwise;
			}
			set
			{
				if (_clockwise != value)
				{
					_clockwise = value;
					FireChanged("clockwise");
				}
			}
		}

		[SerializeField]
		public float radius
		{
			get
			{
				return _radius;
			}
			set
			{
				if (_radius != value)
				{
					_radius = value;
					FireChanged("radius");
				}
			}
		}

		[SerializeField]
		public float radiusAngle
		{
			get
			{
				return _radiusAngle;
			}
			set
			{
				if (_radiusAngle != value)
				{
					_radiusAngle = value;
					FireChanged("radiusAngle");
				}
			}
		}

		public CircleClip()
		{
		}

		public CircleClip(BGCurve curve, BGCcMath math)
			: base(curve, math)
		{
			maker = new CircleCurveMaker(curve);
		}

		public override void MakePoints()
		{
			if (needMakePoints)
			{
				int num = 4;
				BGCurvePoint[] points = maker.MakePoints(num);
				curve.AddPoints(points, startIndex);
			}
		}

		public override void UpdatePoints()
		{
		}
	}
}
