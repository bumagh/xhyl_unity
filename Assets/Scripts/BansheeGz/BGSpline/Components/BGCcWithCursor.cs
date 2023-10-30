using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[RequireComponent(typeof(BGCcCursor))]
	public abstract class BGCcWithCursor : BGCc
	{
		private BGCcCursor cursor;

		public BGCcCursor Cursor
		{
			get
			{
				if (cursor == null)
				{
					cursor = GetParent<BGCcCursor>();
				}
				return cursor;
			}
			set
			{
				if (!(value == null))
				{
					cursor = value;
					SetParent(value);
				}
			}
		}

		public override string Error => (!(Cursor == null)) ? null : "Cursor is null";

		public Quaternion LerpQuaternion(string fieldName, int currentSection = -1)
		{
			int indexFrom;
			int indexTo;
			float t = GetT(out indexFrom, out indexTo, currentSection);
			Quaternion a = base.Curve[indexFrom].GetQuaternion(fieldName);
			Quaternion b = base.Curve[indexTo].GetQuaternion(fieldName);
			if (a.x == 0f && a.y == 0f && a.z == 0f && a.w == 0f)
			{
				a = Quaternion.identity;
			}
			if (b.x == 0f && b.y == 0f && b.z == 0f && b.w == 0f)
			{
				b = Quaternion.identity;
			}
			Quaternion quaternion = Quaternion.Lerp(a, b, t);
			return (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y) && !float.IsNaN(quaternion.z) && !float.IsNaN(quaternion.w)) ? quaternion : Quaternion.identity;
		}

		public Vector3 LerpVector(string name, int currentSection = -1)
		{
			int indexFrom;
			int indexTo;
			float t = GetT(out indexFrom, out indexTo, currentSection);
			Vector3 vector = base.Curve[indexFrom].GetVector3(name);
			Vector3 vector2 = base.Curve[indexTo].GetVector3(name);
			return Vector3.Lerp(vector, vector2, t);
		}

		public float GetT(out int indexFrom, out int indexTo, int currentSection = -1)
		{
			BGCurveBaseMath math = Cursor.Math.Math;
			float distance = Cursor.Distance;
			GetFromToIndexes(out indexFrom, out indexTo, currentSection);
			BGCurveBaseMath.SectionInfo sectionInfo = math[indexFrom];
			return (distance - sectionInfo.DistanceFromStartToOrigin) / sectionInfo.Distance;
		}

		protected void GetFromToIndexes(out int indexFrom, out int indexTo, int currentSection = -1)
		{
			indexFrom = ((currentSection >= 0) ? currentSection : Cursor.CalculateSectionIndex());
			indexTo = ((indexFrom != base.Curve.PointsCount - 1) ? (indexFrom + 1) : 0);
		}
	}
}
