using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_CircleClip : FK3_CurveClip
	{
		private bool _clockwise = true;

		private float _radius = 3f;

		private float _startAngle;

		private int _division = 4;

		private float _radiusAngle = 180f;

		[ShowInInspector]
		[NotSerialized]
		[InspectorDisabled]
		public Vector3 center = Vector3.zero;

		private FK3_CircleCurveMaker maker;

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
		public float startAngle
		{
			get
			{
				return _startAngle;
			}
			set
			{
				if (_startAngle != value)
				{
					_startAngle = value;
					FireChanged("startAngle");
				}
			}
		}

		[SerializeField]
		[InspectorRange(4f, 32f, float.NaN)]
		public int division
		{
			get
			{
				return _division;
			}
			set
			{
				if (_division != value)
				{
					_division = value;
					FireChanged("division");
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

		[InspectorDisabled]
		[NotSerialized]
		public float minAngle => 360f / (float)_division;

		public FK3_CircleClip()
		{
		}

		public FK3_CircleClip(BGCurve curve, BGCcMath math)
			: base(curve, math)
		{
		}

		public override void MakePoints()
		{
			Vector3 startPosition = GetStartPosition();
			Vector3 startTangent = GetStartTangent();
			Vector3 a = FK3_MathUtils.V2Rotate(startTangent, _startAngle).normalized;
			center = startPosition + a * radius;
			int num = Mathf.FloorToInt(_radiusAngle / minAngle) + 1;
			BGCurvePoint[] array = new BGCurvePoint[num];
			BGCurvePoint.ControlTypeEnum controlType = BGCurvePoint.ControlTypeEnum.BezierIndependant;
			float angle = minAngle * (float)((!_clockwise) ? 1 : (-1));
			float controlFactor = GetControlFactor(angle);
			Vector3 vector = -a;
			int num2 = startIndex;
			Vector3 position = Vector3.zero;
			Vector3 a2 = Vector3.right;
			for (int i = 0; i < num; i++)
			{
				position = center + vector * radius;
				a2 = FK3_MathUtils.V2Rotate(new Vector2(vector.x, vector.y), -90f);
				a2.Normalize();
				bool flag = i == 0;
				bool flag2 = i == num - 1;
				Vector3 vector2 = a2 * (controlFactor * radius);
				Vector3 vector3 = vector2 * -1f;
				vector2 = (flag ? Vector3.zero : vector2);
				vector3 = (flag2 ? Vector3.zero : vector3);
				array[i] = new BGCurvePoint(curve, position, controlType, vector2, vector3);
				vector = FK3_MathUtils.V2Rotated(vector, angle);
				num2++;
			}
			curve.AddPoints(array, startIndex);
			_pointCount = num;
			BGCurvePoint point = array[num - 1];
			_tailInfo.index = num2;
			_tailInfo.point = point;
			_tailInfo.position = position;
			_tailInfo.tangent = a2.normalized;
		}

		public override void CheckAndOnValidate()
		{
			base.CheckAndOnValidate();
			_usePrevTail = !isHeadClip;
			_division = Mathf.NextPowerOfTwo(_division);
			_division = Mathf.Clamp(_division, 4, 128);
			_radiusAngle = Mathf.Floor(_radiusAngle / minAngle) * minAngle;
			_radiusAngle = Mathf.Max(_radiusAngle, minAngle);
			if (maker == null)
			{
				maker = new FK3_CircleCurveMaker(curve);
			}
		}

		public static float GetControlFactor(float angle)
		{
			return 4f * Mathf.Tan(angle * ((float)Math.PI / 180f) / 4f) / 3f;
		}

		public override FK3_CurveClip Clone()
		{
			FK3_CircleClip fK3_CircleClip = new FK3_CircleClip(curve, math);
			fK3_CircleClip._clockwise = _clockwise;
			fK3_CircleClip._radius = _radius;
			fK3_CircleClip._startAngle = _startAngle;
			fK3_CircleClip._division = _division;
			fK3_CircleClip._radiusAngle = _radiusAngle;
			return fK3_CircleClip;
		}
	}
}
