using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public class BGCurveBaseMath : BGCurveMathI, IDisposable
	{
		public enum Field
		{
			Position = 1,
			Tangent
		}

		public enum Fields
		{
			Position = 1,
			PositionAndTangent = 3
		}

		public class Config
		{
			public Fields Fields = Fields.Position;

			public int Parts = 30;

			public bool UsePointPositionsToCalcTangents;

			public bool OptimizeStraightLines;

			public Func<bool> ShouldUpdate;

			public event EventHandler Update;

			public Config()
			{
			}

			public Config(Fields fields)
			{
				Fields = fields;
			}

			protected bool Equals(Config other)
			{
				return Fields == other.Fields && Parts == other.Parts && UsePointPositionsToCalcTangents == other.UsePointPositionsToCalcTangents && OptimizeStraightLines == other.OptimizeStraightLines;
			}

			public override bool Equals(object obj)
			{
				if (object.ReferenceEquals(null, obj))
				{
					return false;
				}
				if (object.ReferenceEquals(this, obj))
				{
					return true;
				}
				if (obj.GetType() != GetType())
				{
					return false;
				}
				return Equals((Config)obj);
			}

			public override int GetHashCode()
			{
				int fields = (int)Fields;
				fields = ((fields * 397) ^ Parts);
				fields = ((fields * 397) ^ UsePointPositionsToCalcTangents.GetHashCode());
				return (fields * 397) ^ OptimizeStraightLines.GetHashCode();
			}

			public void FireUpdate()
			{
				if (this.Update != null)
				{
					this.Update(this, null);
				}
			}
		}

		public class SectionInfo
		{
			public float DistanceFromStartToOrigin;

			public float DistanceFromEndToOrigin;

			protected internal readonly List<SectionPointInfo> points = new List<SectionPointInfo>();

			public Vector3 OriginalFrom;

			public Vector3 OriginalTo;

			public BGCurvePoint.ControlTypeEnum OriginalFromControlType;

			public BGCurvePoint.ControlTypeEnum OriginalToControlType;

			public Vector3 OriginalFromControl;

			public Vector3 OriginalToControl;

			public Vector3 OriginalFirstPointTangent;

			public Vector3 OriginalLastPointTangent;

			public List<SectionPointInfo> Points => points;

			public int PointsCount => points.Count;

			public float Distance => DistanceFromEndToOrigin - DistanceFromStartToOrigin;

			public SectionPointInfo this[int i]
			{
				get
				{
					return points[i];
				}
				set
				{
					points[i] = value;
				}
			}

			public override string ToString()
			{
				return "Section distance=(" + Distance + ")";
			}

			protected internal bool Reset(BGCurvePointI fromPoint, BGCurvePointI toPoint, int pointsCount, bool skipCheck)
			{
				Vector3 positionWorld = fromPoint.PositionWorld;
				Vector3 positionWorld2 = toPoint.PositionWorld;
				Vector3 controlSecondWorld = fromPoint.ControlSecondWorld;
				Vector3 controlFirstWorld = toPoint.ControlFirstWorld;
				if (!skipCheck && points.Count == pointsCount && OriginalFromControlType == fromPoint.ControlType && OriginalToControlType == toPoint.ControlType && Vector3.SqrMagnitude(new Vector3(OriginalFrom.x - positionWorld.x, OriginalFrom.y - positionWorld.y, OriginalFrom.z - positionWorld.z)) < 1E-06f && Vector3.SqrMagnitude(new Vector3(OriginalTo.x - positionWorld2.x, OriginalTo.y - positionWorld2.y, OriginalTo.z - positionWorld2.z)) < 1E-06f && Vector3.SqrMagnitude(new Vector3(OriginalFromControl.x - controlSecondWorld.x, OriginalFromControl.y - controlSecondWorld.y, OriginalFromControl.z - controlSecondWorld.z)) < 1E-06f && Vector3.SqrMagnitude(new Vector3(OriginalToControl.x - controlFirstWorld.x, OriginalToControl.y - controlFirstWorld.y, OriginalToControl.z - controlFirstWorld.z)) < 1E-06f)
				{
					return false;
				}
				OriginalFrom = positionWorld;
				OriginalTo = positionWorld2;
				OriginalFromControlType = fromPoint.ControlType;
				OriginalToControlType = toPoint.ControlType;
				OriginalFromControl = controlSecondWorld;
				OriginalToControl = controlFirstWorld;
				return true;
			}

			public int FindPointIndexByDistance(float distanceWithinSection)
			{
				int num = points.Count - 1;
				int num2 = 0;
				int num3 = 0;
				int num4 = points.Count;
				int num5 = 0;
				while (num2 < num4)
				{
					num3 = num2 + num4 >> 1;
					SectionPointInfo sectionPointInfo = points[num3];
					if (!(distanceWithinSection < sectionPointInfo.DistanceToSectionStart) && (num3 == num || points[num3 + 1].DistanceToSectionStart >= distanceWithinSection))
					{
						break;
					}
					if (distanceWithinSection < sectionPointInfo.DistanceToSectionStart)
					{
						num4 = num3;
					}
					else
					{
						num2 = num3 + 1;
					}
					if (num5++ > 100)
					{
						throw new UnityException("Something wrong: more than 100 iterations inside BinarySearch");
					}
				}
				return num3;
			}

			public void CalcByDistance(float distanceWithinSection, out Vector3 position, out Vector3 tangent, bool calculatePosition, bool calculateTangent)
			{
				position = Vector3.zero;
				tangent = Vector3.zero;
				if (points.Count == 2)
				{
					SectionPointInfo sectionPointInfo = points[0];
					if (Math.Abs(Distance) < 1E-05f)
					{
						if (calculatePosition)
						{
							position = sectionPointInfo.Position;
						}
						if (calculateTangent)
						{
							tangent = sectionPointInfo.Tangent;
						}
						return;
					}
					float t = distanceWithinSection / Distance;
					SectionPointInfo sectionPointInfo2 = points[1];
					if (calculatePosition)
					{
						position = Vector3.Lerp(sectionPointInfo.Position, sectionPointInfo2.Position, t);
					}
					if (calculateTangent)
					{
						tangent = Vector3.Lerp(sectionPointInfo.Tangent, sectionPointInfo2.Tangent, t);
					}
					return;
				}
				int num = FindPointIndexByDistance(distanceWithinSection);
				SectionPointInfo sectionPointInfo3 = points[num];
				if (num == points.Count - 1)
				{
					if (calculatePosition)
					{
						position = sectionPointInfo3.Position;
					}
					if (calculateTangent)
					{
						tangent = sectionPointInfo3.Tangent;
					}
					return;
				}
				SectionPointInfo sectionPointInfo4 = points[num + 1];
				float num2 = sectionPointInfo4.DistanceToSectionStart - sectionPointInfo3.DistanceToSectionStart;
				float num3 = distanceWithinSection - sectionPointInfo3.DistanceToSectionStart;
				float t2 = (!(Math.Abs(num2) < 1E-05f)) ? (num3 / num2) : 0f;
				if (calculatePosition)
				{
					position = Vector3.Lerp(sectionPointInfo3.Position, sectionPointInfo4.Position, t2);
				}
				if (calculateTangent)
				{
					tangent = Vector3.Lerp(sectionPointInfo3.Tangent, sectionPointInfo4.Tangent, t2);
				}
			}
		}

		public class SectionPointInfo
		{
			public Vector3 Position;

			public float DistanceToSectionStart;

			public Vector3 Tangent;

			internal Vector3 GetField(Field field)
			{
				switch (field)
				{
				case Field.Position:
					return Position;
				case Field.Tangent:
					return Tangent;
				default:
					throw new UnityException("Unknown field=" + field);
				}
			}

			internal Vector3 LerpTo(Field field, SectionPointInfo to, float ratio)
			{
				return Vector3.Lerp(GetField(field), to.GetField(field), ratio);
			}

			public override string ToString()
			{
				return "Point at (" + Position + ")";
			}
		}

		protected readonly BGCurve curve;

		protected Config config;

		protected readonly List<SectionInfo> cachedSectionInfos = new List<SectionInfo>();

		protected readonly List<SectionInfo> poolSectionInfos = new List<SectionInfo>();

		protected readonly List<SectionPointInfo> poolPointInfos = new List<SectionPointInfo>();

		protected float cachedLength;

		protected bool cachePosition;

		protected bool cacheTangent;

		protected BGCurveCalculatorClosestPoint closestPointCalculator;

		private int recalculatedAtFrame = -1;

		private int createdAtFrame;

		protected bool ignoreSectionChangedCheck;

		private double h;

		private double h2;

		private double h3;

		public bool SuppressWarning
		{
			get;
			set;
		}

		public BGCurve Curve => curve;

		public List<SectionInfo> SectionInfos => cachedSectionInfos;

		public SectionInfo this[int i] => cachedSectionInfos[i];

		public int SectionsCount => cachedSectionInfos.Count;

		public Config Configuration => config;

		protected bool NeedTangentFormula => !config.UsePointPositionsToCalcTangents && cacheTangent;

		public int PointsCount
		{
			get
			{
				if (SectionsCount == 0)
				{
					return 0;
				}
				int num = 0;
				int count = cachedSectionInfos.Count;
				for (int i = 0; i < count; i++)
				{
					num += cachedSectionInfos[i].PointsCount;
				}
				return num;
			}
		}

		public event EventHandler ChangeRequested;

		public event EventHandler Changed;

		public BGCurveBaseMath(BGCurve curve)
			: this(curve, new Config(Fields.Position))
		{
		}

		public BGCurveBaseMath(BGCurve curve, Config config)
		{
			this.curve = curve;
			curve.Changed += CurveChanged;
			Init(config ?? new Config(Fields.Position));
		}

		[Obsolete("Use another constructors")]
		public BGCurveBaseMath(BGCurve curve, bool traceChanges, int parts = 30, bool usePointPositionsToCalcTangents = false)
			: this(curve, new Config(Fields.Position)
			{
				Parts = parts,
				UsePointPositionsToCalcTangents = usePointPositionsToCalcTangents
			})
		{
		}

		public virtual void Init(Config config)
		{
			if (this.config != null)
			{
				ignoreSectionChangedCheck = (this.config.Fields == Fields.Position && config.Fields == Fields.PositionAndTangent);
				this.config.Update -= ConfigOnUpdate;
			}
			else
			{
				ignoreSectionChangedCheck = false;
			}
			this.config = config;
			config.Parts = Mathf.Clamp(config.Parts, 1, 1000);
			this.config.Update += ConfigOnUpdate;
			createdAtFrame = Time.frameCount;
			cachePosition = Field.Position.In(config.Fields.Val());
			cacheTangent = Field.Tangent.In(config.Fields.Val());
			if (!cachePosition && !cacheTangent)
			{
				throw new UnityException("No fields were chosen. Create math like this: new BGCurveBaseMath(curve, new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.Position))");
			}
			AfterInit(config);
			Recalculate(force: true);
		}

		protected virtual void AfterInit(Config config)
		{
		}

		public virtual Vector3 CalcPositionByT(BGCurvePoint from, BGCurvePoint to, float t, bool useLocal = false)
		{
			t = Mathf.Clamp01(t);
			Vector3 vector = (!useLocal) ? from.PositionWorld : from.PositionLocal;
			Vector3 vector2 = (!useLocal) ? to.PositionWorld : to.PositionLocal;
			if (from.ControlType == BGCurvePoint.ControlTypeEnum.Absent && to.ControlType == BGCurvePoint.ControlTypeEnum.Absent)
			{
				return vector + (vector2 - vector) * t;
			}
			Vector3 vector3 = (!useLocal) ? from.ControlSecondWorld : (from.ControlSecondLocal + vector);
			Vector3 vector4 = (!useLocal) ? to.ControlFirstWorld : (to.ControlFirstLocal + vector2);
			return (from.ControlType == BGCurvePoint.ControlTypeEnum.Absent || to.ControlType == BGCurvePoint.ControlTypeEnum.Absent) ? BGCurveFormulas.BezierQuadratic(t, vector, (from.ControlType != 0) ? vector3 : vector4, vector2) : BGCurveFormulas.BezierCubic(t, vector, vector3, vector4, vector2);
		}

		public virtual Vector3 CalcTangentByT(BGCurvePoint from, BGCurvePoint to, float t, bool useLocal = false)
		{
			if (Curve.PointsCount < 2)
			{
				return Vector3.zero;
			}
			t = Mathf.Clamp01(t);
			Vector3 vector = (!useLocal) ? from.PositionWorld : from.PositionLocal;
			Vector3 vector2 = (!useLocal) ? to.PositionWorld : to.PositionLocal;
			Vector3 vector3;
			if (from.ControlType == BGCurvePoint.ControlTypeEnum.Absent && to.ControlType == BGCurvePoint.ControlTypeEnum.Absent)
			{
				vector3 = vector2 - vector;
			}
			else
			{
				Vector3 vector4 = (!useLocal) ? from.ControlSecondWorld : (from.ControlSecondLocal + vector);
				Vector3 vector5 = (!useLocal) ? to.ControlFirstWorld : (to.ControlFirstLocal + vector2);
				vector3 = ((from.ControlType == BGCurvePoint.ControlTypeEnum.Absent || to.ControlType == BGCurvePoint.ControlTypeEnum.Absent) ? BGCurveFormulas.BezierQuadraticDerivative(t, vector, (from.ControlType != 0) ? vector4 : vector5, vector2) : BGCurveFormulas.BezierCubicDerivative(t, vector, vector4, vector5, vector2));
			}
			return vector3.normalized;
		}

		public virtual Vector3 CalcByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false)
		{
			return CalcByDistance(cachedLength * distanceRatio, out tangent, useLocal);
		}

		public virtual Vector3 CalcByDistance(float distance, out Vector3 tangent, bool useLocal = false)
		{
			BinarySearchByDistance(distance, out Vector3 position, out tangent, calculatePosition: true, calculateTangent: true);
			if (useLocal)
			{
				position = curve.transform.InverseTransformPoint(position);
				tangent = curve.transform.InverseTransformDirection(tangent);
			}
			return position;
		}

		public virtual Vector3 CalcByDistanceRatio(Field field, float distanceRatio, bool useLocal = false)
		{
			return CalcByDistance(field, cachedLength * distanceRatio, useLocal);
		}

		public virtual Vector3 CalcByDistance(Field field, float distance, bool useLocal = false)
		{
			bool flag = field == Field.Position;
			BinarySearchByDistance(distance, out Vector3 position, out Vector3 tangent, flag, !flag);
			if (useLocal)
			{
				switch (field)
				{
				case Field.Position:
					position = curve.transform.InverseTransformPoint(position);
					break;
				case Field.Tangent:
					tangent = curve.transform.InverseTransformDirection(tangent);
					break;
				}
			}
			return (!flag) ? tangent : position;
		}

		public virtual Vector3 CalcPositionAndTangentByDistanceRatio(float distanceRatio, out Vector3 tangent, bool useLocal = false)
		{
			return CalcByDistanceRatio(distanceRatio, out tangent, useLocal);
		}

		public virtual Vector3 CalcPositionAndTangentByDistance(float distance, out Vector3 tangent, bool useLocal = false)
		{
			return CalcByDistance(distance, out tangent, useLocal);
		}

		public virtual Vector3 CalcPositionByDistanceRatio(float distanceRatio, bool useLocal = false)
		{
			return CalcByDistanceRatio(Field.Position, distanceRatio, useLocal);
		}

		public virtual Vector3 CalcPositionByDistance(float distance, bool useLocal = false)
		{
			return CalcByDistance(Field.Position, distance, useLocal);
		}

		public virtual Vector3 CalcTangentByDistanceRatio(float distanceRatio, bool useLocal = false)
		{
			return CalcByDistanceRatio(Field.Tangent, distanceRatio, useLocal);
		}

		public virtual Vector3 CalcTangentByDistance(float distance, bool useLocal = false)
		{
			return CalcByDistance(Field.Tangent, distance, useLocal);
		}

		public int CalcSectionIndexByDistance(float distance)
		{
			return FindSectionIndexByDistance(ClampDistance(distance));
		}

		public int CalcSectionIndexByDistanceRatio(float ratio)
		{
			return FindSectionIndexByDistance(DistanceByRatio(ratio));
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			if (closestPointCalculator == null)
			{
				closestPointCalculator = new BGCurveCalculatorClosestPoint(this);
			}
			float distance;
			Vector3 tangent;
			return closestPointCalculator.CalcPositionByClosestPoint(point, out distance, out tangent, skipSectionsOptimization, skipPointsOptimization);
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			if (closestPointCalculator == null)
			{
				closestPointCalculator = new BGCurveCalculatorClosestPoint(this);
			}
			Vector3 tangent;
			return closestPointCalculator.CalcPositionByClosestPoint(point, out distance, out tangent, skipSectionsOptimization, skipPointsOptimization);
		}

		public Vector3 CalcPositionByClosestPoint(Vector3 point, out float distance, out Vector3 tangent, bool skipSectionsOptimization = false, bool skipPointsOptimization = false)
		{
			if (closestPointCalculator == null)
			{
				closestPointCalculator = new BGCurveCalculatorClosestPoint(this);
			}
			return closestPointCalculator.CalcPositionByClosestPoint(point, out distance, out tangent, skipSectionsOptimization, skipPointsOptimization);
		}

		public virtual float GetDistance(int pointIndex = -1)
		{
			if (pointIndex < 0)
			{
				return cachedLength;
			}
			if (pointIndex == 0)
			{
				return 0f;
			}
			return cachedSectionInfos[pointIndex - 1].DistanceFromEndToOrigin;
		}

		public Vector3 GetPosition(int pointIndex)
		{
			int count = cachedSectionInfos.Count;
			if (count == 0 || count <= pointIndex)
			{
				return curve[pointIndex].PositionWorld;
			}
			return (pointIndex >= count) ? cachedSectionInfos[pointIndex - 1].OriginalTo : cachedSectionInfos[pointIndex].OriginalFrom;
		}

		public Vector3 GetControlFirst(int pointIndex)
		{
			int count = cachedSectionInfos.Count;
			if (count == 0)
			{
				return curve[pointIndex].ControlFirstWorld;
			}
			if (pointIndex == 0)
			{
				return (!curve.Closed) ? curve[0].ControlFirstWorld : cachedSectionInfos[count - 1].OriginalToControl;
			}
			return cachedSectionInfos[pointIndex - 1].OriginalToControl;
		}

		public Vector3 GetControlSecond(int pointIndex)
		{
			int count = cachedSectionInfos.Count;
			if (count == 0)
			{
				return curve[pointIndex].ControlSecondWorld;
			}
			if (pointIndex == count)
			{
				return (!curve.Closed) ? curve[pointIndex].ControlSecondWorld : cachedSectionInfos[count - 1].OriginalFromControl;
			}
			return cachedSectionInfos[pointIndex].OriginalFromControl;
		}

		public virtual bool IsCalculated(Field field)
		{
			return ((int)field & (int)config.Fields) != 0;
		}

		public virtual void Dispose()
		{
			curve.Changed -= CurveChanged;
			config.Update -= ConfigOnUpdate;
			cachedSectionInfos.Clear();
			poolSectionInfos.Clear();
		}

		public Bounds GetBoundingBox(int sectionIndex, SectionInfo section)
		{
			bool flag = section.OriginalFromControlType == BGCurvePoint.ControlTypeEnum.Absent;
			bool flag2 = section.OriginalToControlType == BGCurvePoint.ControlTypeEnum.Absent;
			Vector3 originalFrom = section.OriginalFrom;
			Vector3 originalTo = section.OriginalTo;
			Vector3 originalToControl = section.OriginalToControl;
			float num = (!(originalFrom.x > originalTo.x)) ? originalFrom.x : originalTo.x;
			float num2 = (!(originalFrom.y > originalTo.y)) ? originalFrom.y : originalTo.y;
			float num3 = (!(originalFrom.z > originalTo.z)) ? originalFrom.z : originalTo.z;
			float num4 = (!(originalFrom.x < originalTo.x)) ? originalFrom.x : originalTo.x;
			float num5 = (!(originalFrom.y < originalTo.y)) ? originalFrom.y : originalTo.y;
			float num6 = (!(originalFrom.z < originalTo.z)) ? originalFrom.z : originalTo.z;
			float num7;
			float num8;
			float num9;
			float num10;
			float num11;
			float num12;
			if (flag)
			{
				if (flag2)
				{
					num7 = num;
					num8 = num2;
					num9 = num3;
					num10 = num4;
					num11 = num5;
					num12 = num6;
				}
				else
				{
					num7 = ((!(num > originalToControl.x)) ? num : originalToControl.x);
					num8 = ((!(num2 > originalToControl.y)) ? num2 : originalToControl.y);
					num9 = ((!(num3 > originalToControl.z)) ? num3 : originalToControl.z);
					num10 = ((!(num4 < originalToControl.x)) ? num4 : originalToControl.x);
					num11 = ((!(num5 < originalToControl.y)) ? num5 : originalToControl.y);
					num12 = ((!(num6 < originalToControl.z)) ? num6 : originalToControl.z);
				}
			}
			else
			{
				Vector3 originalFromControl = section.OriginalFromControl;
				if (flag2)
				{
					num7 = ((!(num > originalFromControl.x)) ? num : originalFromControl.x);
					num8 = ((!(num2 > originalFromControl.y)) ? num2 : originalFromControl.y);
					num9 = ((!(num3 > originalFromControl.z)) ? num3 : originalFromControl.z);
					num10 = ((!(num4 < originalFromControl.x)) ? num4 : originalFromControl.x);
					num11 = ((!(num5 < originalFromControl.y)) ? num5 : originalFromControl.y);
					num12 = ((!(num6 < originalFromControl.z)) ? num6 : originalFromControl.z);
				}
				else
				{
					float num13 = (!(num > originalToControl.x)) ? num : originalToControl.x;
					float num14 = (!(num2 > originalToControl.y)) ? num2 : originalToControl.y;
					float num15 = (!(num3 > originalToControl.z)) ? num3 : originalToControl.z;
					float num16 = (!(num4 < originalToControl.x)) ? num4 : originalToControl.x;
					float num17 = (!(num5 < originalToControl.y)) ? num5 : originalToControl.y;
					float num18 = (!(num6 < originalToControl.z)) ? num6 : originalToControl.z;
					num7 = ((!(num13 > originalFromControl.x)) ? num13 : originalFromControl.x);
					num8 = ((!(num14 > originalFromControl.y)) ? num14 : originalFromControl.y);
					num9 = ((!(num15 > originalFromControl.z)) ? num15 : originalFromControl.z);
					num10 = ((!(num16 < originalFromControl.x)) ? num16 : originalFromControl.x);
					num11 = ((!(num17 < originalFromControl.y)) ? num17 : originalFromControl.y);
					num12 = ((!(num18 < originalFromControl.z)) ? num18 : originalFromControl.z);
				}
			}
			float num19 = num10 - num7;
			float num20 = num11 - num8;
			float num21 = num12 - num9;
			Vector3 extents = new Vector3(num19 * 0.5f, num20 * 0.5f, num21 * 0.5f);
			Bounds result = default(Bounds);
			result.extents = extents;
			result.center = new Vector3(num7 + extents.x, num8 + extents.y, num9 + extents.z);
			return result;
		}

		public virtual void Recalculate(bool force = false)
		{
			if (this.ChangeRequested != null)
			{
				this.ChangeRequested(this, null);
			}
			force = (force || Curve.SnapType == BGCurve.SnapTypeEnum.Curve);
			if (!force && config.ShouldUpdate != null && !config.ShouldUpdate())
			{
				return;
			}
			int count = cachedSectionInfos.Count;
			if (curve.PointsCount < 2)
			{
				cachedLength = 0f;
				if (count > 0)
				{
					cachedSectionInfos.Clear();
				}
				if (this.Changed != null)
				{
					this.Changed(this, null);
				}
				return;
			}
			int frameCount = Time.frameCount;
			if (recalculatedAtFrame == frameCount && frameCount != createdAtFrame)
			{
				Warning("We noticed you are updating math more than once per frame. This is not optimal. If you use curve.ImmediateChangeEvents by some reason, try to use curve.Transaction to wrap all the changes to one single event.");
			}
			recalculatedAtFrame = frameCount;
			int pointsCount = curve.PointsCount;
			int num = (!curve.Closed) ? (pointsCount - 1) : pointsCount;
			h = 1.0 / (double)config.Parts;
			h2 = h * h;
			h3 = h2 * h;
			if (count != num)
			{
				if (count < num)
				{
					int num2 = num - count;
					int count2 = poolSectionInfos.Count;
					int num3 = num2;
					int num4 = count2 - 1;
					while (num4 >= 0 && num2 > 0)
					{
						cachedSectionInfos.Add(poolSectionInfos[num4]);
						num4--;
						num2--;
					}
					int num5 = num3 - num2;
					if (num5 != 0)
					{
						poolSectionInfos.RemoveRange(poolSectionInfos.Count - num5, num5);
					}
					if (num2 > 0)
					{
						for (int i = 0; i < num2; i++)
						{
							cachedSectionInfos.Add(new SectionInfo());
						}
					}
				}
				else
				{
					int num6 = count - num;
					for (int j = num; j < count; j++)
					{
						poolSectionInfos.Add(cachedSectionInfos[j]);
					}
					cachedSectionInfos.RemoveRange(count - num6, num6);
				}
			}
			for (int k = 0; k < pointsCount - 1; k++)
			{
				CalculateSection(k, cachedSectionInfos[k], (k != 0) ? cachedSectionInfos[k - 1] : null, curve[k], curve[k + 1]);
			}
			SectionInfo sectionInfo = cachedSectionInfos[num - 1];
			if (curve.Closed)
			{
				CalculateSection(num - 1, sectionInfo, cachedSectionInfos[num - 2], curve[pointsCount - 1], curve[0]);
				if (cacheTangent)
				{
					AdjustBoundaryPointsTangents(cachedSectionInfos[0], sectionInfo);
				}
			}
			cachedLength = sectionInfo.DistanceFromEndToOrigin;
			if (this.Changed != null)
			{
				this.Changed(this, null);
			}
		}

		protected virtual void Warning(string message, bool condition = true, Action callback = null)
		{
			if (condition && Application.isPlaying)
			{
				if (!SuppressWarning)
				{
					UnityEngine.Debug.Log("BGCurve[BGCurveBaseMath] Warning! " + message + ". You can suppress all warnings by using BGCurveBaseMath.SuppressWarning=true;");
				}
				callback?.Invoke();
			}
		}

		protected virtual void CalculateSection(int index, SectionInfo section, SectionInfo prevSection, BGCurvePointI from, BGCurvePointI to)
		{
			if (section == null)
			{
				section = new SectionInfo();
			}
			section.DistanceFromStartToOrigin = (prevSection?.DistanceFromEndToOrigin ?? 0f);
			bool flag = config.OptimizeStraightLines && from.ControlType == BGCurvePoint.ControlTypeEnum.Absent && to.ControlType == BGCurvePoint.ControlTypeEnum.Absent;
			int pointsCount = (!flag) ? (config.Parts + 1) : 2;
			if (Reset(section, from, to, pointsCount) || Curve.SnapType == BGCurve.SnapTypeEnum.Curve)
			{
				if (flag)
				{
					Resize(section.points, 2);
					SectionPointInfo sectionPointInfo = section.points[0];
					SectionPointInfo sectionPointInfo2 = section.points[1];
					sectionPointInfo.Position = section.OriginalFrom;
					sectionPointInfo2.Position = section.OriginalTo;
					sectionPointInfo2.DistanceToSectionStart = Vector3.Distance(section.OriginalFrom, section.OriginalTo);
					if (cacheTangent)
					{
						sectionPointInfo.Tangent = (sectionPointInfo2.Tangent = (sectionPointInfo2.Position - sectionPointInfo.Position).normalized);
					}
				}
				else
				{
					CalculateSplitSection(section, from, to);
				}
				if (cacheTangent)
				{
					section.OriginalFirstPointTangent = section[0].Tangent;
					section.OriginalLastPointTangent = section[section.PointsCount - 1].Tangent;
				}
			}
			if (cacheTangent && prevSection != null)
			{
				AdjustBoundaryPointsTangents(section, prevSection);
			}
			section.DistanceFromEndToOrigin = section.DistanceFromStartToOrigin + section[section.PointsCount - 1].DistanceToSectionStart;
		}

		private void AdjustBoundaryPointsTangents(SectionInfo section, SectionInfo prevSection)
		{
			if (IsUseDistanceToAdjustTangents(section, prevSection))
			{
				float num = Vector3.SqrMagnitude(section[0].Position - section[1].Position);
				float num2 = Vector3.SqrMagnitude(prevSection[prevSection.PointsCount - 1].Position - prevSection[prevSection.PointsCount - 2].Position);
				float num3 = num + num2;
				if (!(Math.Abs(num3) < 1E-05f))
				{
					float num4 = num / num3;
					float num5 = 1f - num4;
					section[0].Tangent = (prevSection[prevSection.PointsCount - 1].Tangent = Vector3.Normalize(new Vector3(section.OriginalFirstPointTangent.x * num4 + prevSection.OriginalLastPointTangent.x * num5, section.OriginalFirstPointTangent.y * num4 + prevSection.OriginalLastPointTangent.y * num5, section.OriginalFirstPointTangent.z * num4 + prevSection.OriginalLastPointTangent.z * num5)));
				}
			}
			else
			{
				section[0].Tangent = (prevSection[prevSection.PointsCount - 1].Tangent = Vector3.Normalize(new Vector3(section.OriginalFirstPointTangent.x + prevSection.OriginalLastPointTangent.x, section.OriginalFirstPointTangent.y + prevSection.OriginalLastPointTangent.y, section.OriginalFirstPointTangent.z + prevSection.OriginalLastPointTangent.z)));
			}
		}

		protected virtual bool IsUseDistanceToAdjustTangents(SectionInfo section, SectionInfo prevSection)
		{
			return config.OptimizeStraightLines && section.OriginalFromControlType == BGCurvePoint.ControlTypeEnum.Absent && (section.OriginalToControlType == BGCurvePoint.ControlTypeEnum.Absent || prevSection.OriginalFromControlType == BGCurvePoint.ControlTypeEnum.Absent);
		}

		protected virtual bool Reset(SectionInfo section, BGCurvePointI from, BGCurvePointI to, int pointsCount)
		{
			return section.Reset(from, to, pointsCount, ignoreSectionChangedCheck);
		}

		protected virtual void CalculateSplitSection(SectionInfo section, BGCurvePointI from, BGCurvePointI to)
		{
			int parts = config.Parts;
			Resize(section.points, parts + 1);
			List<SectionPointInfo> points = section.points;
			Vector3 originalFrom = section.OriginalFrom;
			Vector3 originalTo = section.OriginalTo;
			Vector3 vector = section.OriginalFromControl;
			Vector3 originalToControl = section.OriginalToControl;
			bool flag = from.ControlType == BGCurvePoint.ControlTypeEnum.Absent;
			bool flag2 = to.ControlType == BGCurvePoint.ControlTypeEnum.Absent;
			bool flag3 = flag && flag2;
			bool flag4 = !flag && !flag2;
			if (!flag3 && !flag4 && flag)
			{
				vector = originalToControl;
			}
			bool flag5 = curve.SnapType == BGCurve.SnapTypeEnum.Curve;
			object sectionPointInfo = points[0];
			if (sectionPointInfo == null)
			{
				SectionPointInfo sectionPointInfo2 = new SectionPointInfo();
				section.points[0] = sectionPointInfo2;
				sectionPointInfo = sectionPointInfo2;
			}
			SectionPointInfo sectionPointInfo3 = (SectionPointInfo)sectionPointInfo;
			sectionPointInfo3.Position = originalFrom;
			object sectionPointInfo4 = points[parts];
			if (sectionPointInfo4 == null)
			{
				SectionPointInfo sectionPointInfo2 = new SectionPointInfo();
				section.points[parts] = sectionPointInfo2;
				sectionPointInfo4 = sectionPointInfo2;
			}
			SectionPointInfo sectionPointInfo5 = (SectionPointInfo)sectionPointInfo4;
			sectionPointInfo5.Position = originalTo;
			if (flag3)
			{
				double num = originalFrom.x;
				double num2 = originalFrom.y;
				double num3 = originalFrom.z;
				double num4 = ((double)originalTo.x - (double)originalFrom.x) / (double)parts;
				double num5 = ((double)originalTo.y - (double)originalFrom.y) / (double)parts;
				double num6 = ((double)originalTo.z - (double)originalFrom.z) / (double)parts;
				Vector3 tangent = Vector3.zero;
				if (cacheTangent)
				{
					tangent = (originalTo - originalFrom).normalized;
				}
				sectionPointInfo5.DistanceToSectionStart = Vector3.Distance(originalTo, originalFrom);
				float num7 = sectionPointInfo5.DistanceToSectionStart / (float)parts;
				for (int i = 1; i < parts; i++)
				{
					SectionPointInfo sectionPointInfo6 = points[i];
					num += num4;
					num2 += num5;
					num3 += num6;
					Vector3 pos = new Vector3((float)num, (float)num2, (float)num3);
					if (flag5)
					{
						curve.ApplySnapping(ref pos);
					}
					sectionPointInfo6.Position = pos;
					if (cacheTangent)
					{
						if (config.UsePointPositionsToCalcTangents)
						{
							SectionPointInfo sectionPointInfo7 = section[i - 1];
							Vector3 position = sectionPointInfo7.Position;
							Vector3 vector2 = new Vector3(pos.x - position.x, pos.y - position.y, pos.z - position.z);
							float num8 = (float)Math.Sqrt((double)vector2.x * (double)vector2.x + (double)vector2.y * (double)vector2.y + (double)vector2.z * (double)vector2.z);
							vector2 = ((!((double)num8 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector2.x / num8, vector2.y / num8, vector2.z / num8));
							sectionPointInfo7.Tangent = (sectionPointInfo6.Tangent = vector2);
						}
						else
						{
							sectionPointInfo6.Tangent = tangent;
						}
					}
					sectionPointInfo6.DistanceToSectionStart = num7 * (float)i;
				}
				if (cacheTangent)
				{
					sectionPointInfo3.Tangent = (sectionPointInfo5.Tangent = tangent);
				}
			}
			else
			{
				double num9 = 0.0;
				double num10 = 0.0;
				double num11 = 0.0;
				double num12 = 0.0;
				double num13 = 0.0;
				double num14 = 0.0;
				if (flag4)
				{
					double num15 = 3.0 * ((double)vector.x - (double)originalFrom.x);
					double num16 = 3.0 * ((double)vector.y - (double)originalFrom.y);
					double num17 = 3.0 * ((double)vector.z - (double)originalFrom.z);
					double num18 = 3.0 * ((double)originalToControl.x - (double)vector.x) - num15;
					double num19 = 3.0 * ((double)originalToControl.y - (double)vector.y) - num16;
					double num20 = 3.0 * ((double)originalToControl.z - (double)vector.z) - num17;
					double num21 = (double)originalTo.x - (double)originalFrom.x - num15 - num18;
					double num22 = (double)originalTo.y - (double)originalFrom.y - num16 - num19;
					double num23 = (double)originalTo.z - (double)originalFrom.z - num17 - num20;
					double num24 = originalFrom.x;
					double num25 = originalFrom.y;
					double num26 = originalFrom.z;
					double num27 = num21 * h3;
					double num28 = 6.0 * num27;
					double num29 = num22 * h3;
					double num30 = 6.0 * num29;
					double num31 = num23 * h3;
					double num32 = 6.0 * num31;
					double num33 = num18 * h2;
					double num34 = num19 * h2;
					double num35 = num20 * h2;
					double num36 = num27 + num33 + num15 * h;
					double num37 = num29 + num34 + num16 * h;
					double num38 = num31 + num35 + num17 * h;
					double num39 = num28 + 2.0 * num33;
					double num40 = num30 + 2.0 * num34;
					double num41 = num32 + 2.0 * num35;
					double num42 = num28;
					double num43 = num30;
					double num44 = num32;
					double num45 = 0.0;
					double num46 = 0.0;
					double num47 = 0.0;
					if (cacheTangent && !config.UsePointPositionsToCalcTangents)
					{
						double num48 = 6.0 * ((double)originalFrom.x - (double)(2f * vector.x) + (double)originalToControl.x);
						double num49 = 6.0 * ((double)originalFrom.y - (double)(2f * vector.y) + (double)originalToControl.y);
						double num50 = 6.0 * ((double)originalFrom.z - (double)(2f * vector.z) + (double)originalToControl.z);
						double num51 = 3.0 * ((double)(0f - originalFrom.x) + (double)(3f * vector.x) - (double)(3f * originalToControl.x) + (double)originalTo.x);
						double num52 = 3.0 * ((double)(0f - originalFrom.y) + (double)(3f * vector.y) - (double)(3f * originalToControl.y) + (double)originalTo.y);
						double num53 = 3.0 * ((double)(0f - originalFrom.z) + (double)(3f * vector.z) - (double)(3f * originalToControl.z) + (double)originalTo.z);
						double num54 = num51 * h2;
						double num55 = num52 * h2;
						double num56 = num53 * h2;
						num12 = num54 + num48 * h;
						num13 = num55 + num49 * h;
						num14 = num56 + num50 * h;
						num45 = 2.0 * num54;
						num46 = 2.0 * num55;
						num47 = 2.0 * num56;
						num9 = num15;
						num10 = num16;
						num11 = num17;
						double num57 = Math.Sqrt(num9 * num9 + num10 * num10 + num11 * num11);
						sectionPointInfo3.Tangent = ((!(num57 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3((float)(num9 / num57), (float)(num10 / num57), (float)(num11 / num57)));
					}
					for (int j = 1; j < parts; j++)
					{
						SectionPointInfo sectionPointInfo8 = points[j];
						num24 += num36;
						num25 += num37;
						num26 += num38;
						num36 += num39;
						num37 += num40;
						num38 += num41;
						num39 += num42;
						num40 += num43;
						num41 += num44;
						Vector3 pos2 = new Vector3((float)num24, (float)num25, (float)num26);
						if (flag5)
						{
							curve.ApplySnapping(ref pos2);
						}
						sectionPointInfo8.Position = pos2;
						if (cacheTangent)
						{
							if (config.UsePointPositionsToCalcTangents)
							{
								SectionPointInfo sectionPointInfo9 = section[j - 1];
								Vector3 position2 = sectionPointInfo9.Position;
								Vector3 vector3 = new Vector3(pos2.x - position2.x, pos2.y - position2.y, pos2.z - position2.z);
								float num58 = (float)Math.Sqrt((double)vector3.x * (double)vector3.x + (double)vector3.y * (double)vector3.y + (double)vector3.z * (double)vector3.z);
								vector3 = ((!((double)num58 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector3.x / num58, vector3.y / num58, vector3.z / num58));
								sectionPointInfo9.Tangent = (sectionPointInfo8.Tangent = vector3);
							}
							else
							{
								num9 += num12;
								num10 += num13;
								num11 += num14;
								num12 += num45;
								num13 += num46;
								num14 += num47;
								double num59 = Math.Sqrt(num9 * num9 + num10 * num10 + num11 * num11);
								sectionPointInfo8.Tangent = ((!(num59 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3((float)(num9 / num59), (float)(num10 / num59), (float)(num11 / num59)));
							}
						}
						Vector3 position3 = section[j - 1].Position;
						double num60 = pos2.x - position3.x;
						double num61 = pos2.y - position3.y;
						double num62 = pos2.z - position3.z;
						sectionPointInfo8.DistanceToSectionStart = section[j - 1].DistanceToSectionStart + (float)Math.Sqrt(num60 * num60 + num61 * num61 + num62 * num62);
					}
				}
				else
				{
					double num63 = 2.0 * ((double)vector.x - (double)originalFrom.x);
					double num64 = 2.0 * ((double)vector.y - (double)originalFrom.y);
					double num65 = 2.0 * ((double)vector.z - (double)originalFrom.z);
					double num66 = (double)originalFrom.x - (double)(2f * vector.x) + (double)originalTo.x;
					double num67 = (double)originalFrom.y - (double)(2f * vector.y) + (double)originalTo.y;
					double num68 = (double)originalFrom.z - (double)(2f * vector.z) + (double)originalTo.z;
					double num69 = num66 * h2 + num63 * h;
					double num70 = num67 * h2 + num64 * h;
					double num71 = num68 * h2 + num65 * h;
					double num72 = 2.0 * num66 * h2;
					double num73 = 2.0 * num67 * h2;
					double num74 = 2.0 * num68 * h2;
					double num75 = originalFrom.x;
					double num76 = originalFrom.y;
					double num77 = originalFrom.z;
					if (cacheTangent && !config.UsePointPositionsToCalcTangents)
					{
						double num78 = 2.0 * ((double)originalFrom.x - (double)(2f * vector.x) + (double)originalTo.x);
						double num79 = 2.0 * ((double)originalFrom.y - (double)(2f * vector.y) + (double)originalTo.y);
						double num80 = 2.0 * ((double)originalFrom.z - (double)(2f * vector.z) + (double)originalTo.z);
						num12 = num78 * h;
						num13 = num79 * h;
						num14 = num80 * h;
						num9 = 2.0 * ((double)vector.x - (double)originalFrom.x);
						num10 = 2.0 * ((double)vector.y - (double)originalFrom.y);
						num11 = 2.0 * ((double)vector.z - (double)originalFrom.z);
						double num81 = Math.Sqrt(num9 * num9 + num10 * num10 + num11 * num11);
						sectionPointInfo3.Tangent = ((!(num81 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3((float)(num9 / num81), (float)(num10 / num81), (float)(num11 / num81)));
					}
					for (int k = 1; k < parts; k++)
					{
						SectionPointInfo sectionPointInfo10 = points[k];
						num75 += num69;
						num76 += num70;
						num77 += num71;
						num69 += num72;
						num70 += num73;
						num71 += num74;
						Vector3 pos3 = new Vector3((float)num75, (float)num76, (float)num77);
						if (flag5)
						{
							curve.ApplySnapping(ref pos3);
						}
						sectionPointInfo10.Position = pos3;
						if (cacheTangent)
						{
							if (config.UsePointPositionsToCalcTangents)
							{
								SectionPointInfo sectionPointInfo11 = section[k - 1];
								Vector3 position4 = sectionPointInfo11.Position;
								Vector3 vector4 = new Vector3(pos3.x - position4.x, pos3.y - position4.y, pos3.z - position4.z);
								float num82 = (float)Math.Sqrt((double)vector4.x * (double)vector4.x + (double)vector4.y * (double)vector4.y + (double)vector4.z * (double)vector4.z);
								vector4 = ((!((double)num82 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector4.x / num82, vector4.y / num82, vector4.z / num82));
								sectionPointInfo11.Tangent = (sectionPointInfo10.Tangent = vector4);
							}
							else
							{
								num9 += num12;
								num10 += num13;
								num11 += num14;
								double num83 = Math.Sqrt(num9 * num9 + num10 * num10 + num11 * num11);
								sectionPointInfo10.Tangent = ((!(num83 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3((float)(num9 / num83), (float)(num10 / num83), (float)(num11 / num83)));
							}
						}
						Vector3 position5 = section[k - 1].Position;
						double num84 = pos3.x - position5.x;
						double num85 = pos3.y - position5.y;
						double num86 = pos3.z - position5.z;
						sectionPointInfo10.DistanceToSectionStart = section[k - 1].DistanceToSectionStart + (float)Math.Sqrt(num84 * num84 + num85 * num85 + num86 * num86);
					}
				}
				if (cacheTangent && !config.UsePointPositionsToCalcTangents)
				{
					num9 += num12;
					num10 += num13;
					num11 += num14;
					double num87 = Math.Sqrt(num9 * num9 + num10 * num10 + num11 * num11);
					sectionPointInfo5.Tangent = ((!(num87 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3((float)(num9 / num87), (float)(num10 / num87), (float)(num11 / num87)));
				}
			}
			SectionPointInfo sectionPointInfo12 = section[parts - 1];
			Vector3 position6 = sectionPointInfo12.Position;
			Vector3 position7 = sectionPointInfo5.Position;
			double num88 = position7.x - position6.x;
			double num89 = position7.y - position6.y;
			double num90 = position7.z - position6.z;
			sectionPointInfo5.DistanceToSectionStart = sectionPointInfo12.DistanceToSectionStart + (float)Math.Sqrt(num88 * num88 + num89 * num89 + num90 * num90);
			if (cacheTangent && config.UsePointPositionsToCalcTangents)
			{
				Vector3 vector5 = new Vector3((float)num88, (float)num89, (float)num90);
				float num91 = (float)Math.Sqrt((double)vector5.x * (double)vector5.x + (double)vector5.y * (double)vector5.y + (double)vector5.z * (double)vector5.z);
				vector5 = (sectionPointInfo5.Tangent = ((!((double)num91 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector5.x / num91, vector5.y / num91, vector5.z / num91)));
			}
		}

		protected virtual void BinarySearchByDistance(float distance, out Vector3 position, out Vector3 tangent, bool calculatePosition, bool calculateTangent)
		{
			switch (curve.PointsCount)
			{
			case 0:
				position = Vector3.zero;
				tangent = Vector3.zero;
				return;
			case 1:
				position = curve[0].PositionWorld;
				tangent = Vector3.zero;
				return;
			}
			if (cachedSectionInfos.Count == 0)
			{
				position = Vector3.zero;
				tangent = Vector3.zero;
				return;
			}
			if (distance < 0f)
			{
				distance = 0f;
			}
			else if (distance > cachedLength)
			{
				distance = cachedLength;
			}
			if (calculateTangent && ((Fields)2 & config.Fields) == (Fields)0)
			{
				throw new UnityException("Can not calculate tangent, cause it was not included in the 'fields' constructor parameter. For example, use new BGCurveBaseMath(curve, new BGCurveBaseMath.Config(BGCurveBaseMath.Fields.PositionAndTangent))to calculate world's position and tangent");
			}
			SectionInfo sectionInfo = cachedSectionInfos[FindSectionIndexByDistance(distance)];
			sectionInfo.CalcByDistance(distance - sectionInfo.DistanceFromStartToOrigin, out position, out tangent, calculatePosition, calculateTangent);
		}

		protected int FindSectionIndexByDistance(float distance)
		{
			int num = 0;
			int num2 = 0;
			int num3 = cachedSectionInfos.Count;
			int num4 = 0;
			while (num < num3)
			{
				num2 = num + num3 >> 1;
				SectionInfo sectionInfo = cachedSectionInfos[num2];
				if (distance >= sectionInfo.DistanceFromStartToOrigin && distance <= sectionInfo.DistanceFromEndToOrigin)
				{
					break;
				}
				if (distance < sectionInfo.DistanceFromStartToOrigin)
				{
					num3 = num2;
				}
				else
				{
					num = num2 + 1;
				}
				if (num4++ > 100)
				{
					throw new UnityException("Something wrong: more than 100 iterations inside BinarySearch");
				}
			}
			return num2;
		}

		protected float DistanceByRatio(float distanceRatio)
		{
			return GetDistance() * Mathf.Clamp01(distanceRatio);
		}

		protected float ClampDistance(float distance)
		{
			return Mathf.Clamp(distance, 0f, GetDistance());
		}

		public override string ToString()
		{
			return "Base Math for curve (" + Curve + "), sections=" + SectionsCount;
		}

		protected void Resize(List<SectionPointInfo> points, int size)
		{
			int count = points.Count;
			if (count == size)
			{
				return;
			}
			if (count < size)
			{
				int num = poolPointInfos.Count - 1;
				for (int i = count; i < size; i++)
				{
					points.Add((num < 0) ? new SectionPointInfo() : poolPointInfos[num--]);
				}
				if (num != poolPointInfos.Count - 1)
				{
					poolPointInfos.RemoveRange(num + 1, poolPointInfos.Count - 1 - num);
				}
			}
			else
			{
				for (int j = size; j < count; j++)
				{
					poolPointInfos.Add(points[j]);
				}
				points.RemoveRange(size, count - size);
			}
		}

		private void CurveChanged(object sender, BGCurveChangedArgs e)
		{
			ignoreSectionChangedCheck = (e != null && e.ChangeType == BGCurveChangedArgs.ChangeTypeEnum.Snap);
			Recalculate();
			ignoreSectionChangedCheck = false;
		}

		private void ConfigOnUpdate(object sender, EventArgs eventArgs)
		{
			Recalculate(force: true);
		}
	}
}
