using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCursor")]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCursor")]
	[CcDescriptor(Description = "Identify location on the curve by distance.", Name = "Cursor", Icon = "BGCcCursor123")]
	public class BGCcCursor : BGCcWithMath
	{
		[Tooltip("Distance from start of the curve.")]
		[SerializeField]
		protected float distance;

		[Range(0.5f, 1.5f)]
		[SerializeField]
		private float handlesScale = 1f;

		[SerializeField]
		private Color handlesColor = Color.white;

		public float Distance
		{
			get
			{
				return distance;
			}
			set
			{
				distance = base.Math.ClampDistance(value);
				FireChangedParams();
			}
		}

		public float DistanceRatio
		{
			get
			{
				float num = base.Math.GetDistance();
				return (num != 0f) ? Mathf.Clamp01(distance / num) : 0f;
			}
			set
			{
				distance = base.Math.GetDistance() * Mathf.Clamp01(value);
				FireChangedParams();
			}
		}

		public override bool SupportHandles => true;

		public override bool SupportHandlesSettings => true;

		public float HandlesScale
		{
			get
			{
				return handlesScale;
			}
			set
			{
				handlesScale = value;
			}
		}

		public Color HandlesColor
		{
			get
			{
				return handlesColor;
			}
			set
			{
				handlesColor = value;
			}
		}

		public Vector3 CalculateTangent()
		{
			return base.Math.CalcByDistance(BGCurveBaseMath.Field.Tangent, distance);
		}

		public Vector3 CalculatePosition()
		{
			return base.Math.CalcByDistance(BGCurveBaseMath.Field.Position, distance);
		}

		public int CalculateSectionIndex()
		{
			return base.Math.CalcSectionIndexByDistance(distance);
		}

		public TR Lerp<T, TR>(string fieldName, Func<T, T, float, TR> lerpFunction)
		{
			if (base.Curve.PointsCount == 0)
			{
				return lerpFunction(default(T), default(T), 0f);
			}
			T fromValue;
			T toValue;
			float adjacentFieldValues = GetAdjacentFieldValues(fieldName, out fromValue, out toValue);
			return lerpFunction(fromValue, toValue, adjacentFieldValues);
		}

		public Quaternion LerpQuaternion(string fieldName, Func<Quaternion, Quaternion, float, Quaternion> customLerp = null)
		{
			if (base.Curve.PointsCount == 0)
			{
				return Quaternion.identity;
			}
			Quaternion fromValue;
			Quaternion toValue;
			float adjacentFieldValues = GetAdjacentFieldValues(fieldName, out fromValue, out toValue);
			if (fromValue.x == 0f && fromValue.y == 0f && fromValue.z == 0f && fromValue.w == 0f)
			{
				fromValue = Quaternion.identity;
			}
			if (toValue.x == 0f && toValue.y == 0f && toValue.z == 0f && toValue.w == 0f)
			{
				toValue = Quaternion.identity;
			}
			Quaternion quaternion = customLerp?.Invoke(fromValue, toValue, adjacentFieldValues) ?? Quaternion.Lerp(fromValue, toValue, adjacentFieldValues);
			return (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y) && !float.IsNaN(quaternion.z) && !float.IsNaN(quaternion.w)) ? quaternion : Quaternion.identity;
		}

		public Vector3 LerpVector(string fieldName, Func<Vector3, Vector3, float, Vector3> customLerp = null)
		{
			if (base.Curve.PointsCount == 0)
			{
				return Vector3.zero;
			}
			Vector3 fromValue;
			Vector3 toValue;
			float adjacentFieldValues = GetAdjacentFieldValues(fieldName, out fromValue, out toValue);
			return customLerp?.Invoke(fromValue, toValue, adjacentFieldValues) ?? Vector3.Lerp(fromValue, toValue, adjacentFieldValues);
		}

		public float LerpFloat(string fieldName, Func<float, float, float, float> customLerp = null)
		{
			if (base.Curve.PointsCount == 0)
			{
				return 0f;
			}
			float fromValue;
			float toValue;
			float adjacentFieldValues = GetAdjacentFieldValues(fieldName, out fromValue, out toValue);
			return customLerp?.Invoke(fromValue, toValue, adjacentFieldValues) ?? Mathf.Lerp(fromValue, toValue, adjacentFieldValues);
		}

		public Color LerpColor(string fieldName, Func<Color, Color, float, Color> customLerp = null)
		{
			if (base.Curve.PointsCount == 0)
			{
				return Color.clear;
			}
			Color fromValue;
			Color toValue;
			float adjacentFieldValues = GetAdjacentFieldValues(fieldName, out fromValue, out toValue);
			return customLerp?.Invoke(fromValue, toValue, adjacentFieldValues) ?? Color.Lerp(fromValue, toValue, adjacentFieldValues);
		}

		public float GetAdjacentFieldValues<T>(string fieldName, out T fromValue, out T toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetField<T>(fieldName);
			toValue = base.Curve[indexTo].GetField<T>(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out float fromValue, out float toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetFloat(fieldName);
			toValue = base.Curve[indexTo].GetFloat(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out int fromValue, out int toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetInt(fieldName);
			toValue = base.Curve[indexTo].GetInt(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out bool fromValue, out bool toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetBool(fieldName);
			toValue = base.Curve[indexTo].GetBool(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out Bounds fromValue, out Bounds toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetBounds(fieldName);
			toValue = base.Curve[indexTo].GetBounds(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out Color fromValue, out Color toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetColor(fieldName);
			toValue = base.Curve[indexTo].GetColor(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out Quaternion fromValue, out Quaternion toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetQuaternion(fieldName);
			toValue = base.Curve[indexTo].GetQuaternion(fieldName);
			return tForLerp;
		}

		public float GetAdjacentFieldValues(string fieldName, out Vector3 fromValue, out Vector3 toValue)
		{
			int indexFrom;
			int indexTo;
			float tForLerp = GetTForLerp(out indexFrom, out indexTo);
			fromValue = base.Curve[indexFrom].GetVector3(fieldName);
			toValue = base.Curve[indexTo].GetVector3(fieldName);
			return tForLerp;
		}

		public float GetTForLerp(out int indexFrom, out int indexTo)
		{
			GetAdjacentPointIndexes(out indexFrom, out indexTo);
			BGCurveBaseMath.SectionInfo sectionInfo = base.Math[indexFrom];
			return (distance - sectionInfo.DistanceFromStartToOrigin) / sectionInfo.Distance;
		}

		public void GetAdjacentPointIndexes(out int indexFrom, out int indexTo)
		{
			indexFrom = CalculateSectionIndex();
			indexTo = ((indexFrom != base.Curve.PointsCount - 1) ? (indexFrom + 1) : 0);
		}

		public override void Start()
		{
			Distance = distance;
		}
	}
}
