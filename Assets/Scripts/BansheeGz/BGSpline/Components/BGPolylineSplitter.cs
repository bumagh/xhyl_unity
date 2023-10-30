using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	public class BGPolylineSplitter
	{
		public class Config
		{
			public bool DoNotOptimizeStraightLines;

			public BGCcSplitterPolyline.SplitModeEnum SplitMode;

			public int PartsTotal;

			public int PartsPerSection;

			public bool UseLocal;

			public Transform Transform;

			public float DistanceMin;

			public float DistanceMax;
		}

		public abstract class PositionsProvider
		{
			protected BGCcMath Math;

			private float distanceMin = -1f;

			private float distanceMax = -1f;

			protected bool LastPointAdded;

			protected bool DistanceMinConstrained;

			protected bool DistanceMaxConstrained;

			protected bool calculatingTangents;

			public float DistanceMin
			{
				get
				{
					return distanceMin;
				}
				set
				{
					distanceMin = value;
					DistanceMinConstrained = (value > 0f);
				}
			}

			public float DistanceMax
			{
				get
				{
					return distanceMax;
				}
				set
				{
					distanceMax = value;
					DistanceMaxConstrained = (value > 0f);
				}
			}

			public virtual void Init(BGCcMath math)
			{
				Math = math;
				LastPointAdded = false;
				calculatingTangents = Math.IsCalculated(BGCurveBaseMath.Field.Tangent);
			}

			public abstract bool Comply(BGCcSplitterPolyline.SplitModeEnum splitMode);

			public virtual void Build(List<Vector3> positions, int straightLinesCount, bool[] straightBits, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				BGCurveBaseMath math = Math.Math;
				List<BGCurveBaseMath.SectionInfo> sectionInfos = math.SectionInfos;
				int count = sectionInfos.Count;
				if (!DistanceMinConstrained)
				{
					BGCurveBaseMath.SectionPointInfo sectionPointInfo = math[0][0];
					positions.Add(sectionPointInfo.Position);
					points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo.Position, 0f, sectionPointInfo.Tangent));
				}
				for (int i = 0; i < count; i++)
				{
					BGCurveBaseMath.SectionInfo sectionInfo = sectionInfos[i];
					if ((DistanceMinConstrained && sectionInfo.DistanceFromEndToOrigin < DistanceMin) || (DistanceMaxConstrained && sectionInfo.DistanceFromStartToOrigin > DistanceMax))
					{
						continue;
					}
					if (straightLinesCount != 0 && straightBits[i])
					{
						BGCurveBaseMath.SectionPointInfo sectionPointInfo2 = sectionInfo[sectionInfo.PointsCount - 1];
						BGCurveBaseMath.SectionPointInfo previousPoint = sectionInfo[sectionInfo.PointsCount - 2];
						if (DistanceMinConstrained && positions.Count == 0)
						{
							AddFirstPointIfNeeded(positions, sectionInfo, sectionPointInfo2, previousPoint, points);
						}
						if (DistanceMaxConstrained && !LastPointAdded && sectionPointInfo2.DistanceToSectionStart + sectionInfo.DistanceFromStartToOrigin > DistanceMax && AddLastPointIfNeeded(positions, sectionInfo, sectionPointInfo2, previousPoint, points))
						{
							break;
						}
						positions.Add(sectionPointInfo2.Position);
						points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo2.Position, sectionInfo.DistanceFromEndToOrigin, sectionPointInfo2.Tangent));
					}
					else
					{
						FillInSplitSection(sectionInfo, positions, points);
					}
				}
			}

			protected void AddFirstPointIfNeeded(List<Vector3> positions, BGCurveBaseMath.SectionInfo section, BGCurveBaseMath.SectionPointInfo firstPointInRange, BGCurveBaseMath.SectionPointInfo previousPoint, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				float num = firstPointInRange.DistanceToSectionStart + section.DistanceFromStartToOrigin;
				float num2 = num - DistanceMin;
				if (num2 > 1E-05f)
				{
					float ratio = 1f - num2 / (firstPointInRange.DistanceToSectionStart - previousPoint.DistanceToSectionStart);
					Add(section, positions, points, previousPoint, firstPointInRange, ratio);
				}
			}

			protected bool AddLastPointIfNeeded(List<Vector3> positions, BGCurveBaseMath.SectionInfo section, BGCurveBaseMath.SectionPointInfo currentPoint, BGCurveBaseMath.SectionPointInfo previousPoint, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				float num = currentPoint.DistanceToSectionStart + section.DistanceFromStartToOrigin;
				if (!(num > DistanceMax))
				{
					return false;
				}
				float num2 = Vector3.SqrMagnitude(positions[positions.Count - 1] - currentPoint.Position);
				float num3 = Vector3.SqrMagnitude(previousPoint.Position - currentPoint.Position);
				if (num2 > num3)
				{
					float num4 = num - DistanceMax;
					LastPointAdded = true;
					float ratio = num4 / (currentPoint.DistanceToSectionStart - previousPoint.DistanceToSectionStart);
					Add(section, positions, points, previousPoint, currentPoint, ratio);
				}
				else
				{
					float num5 = Mathf.Sqrt(num2);
					float num6 = num - DistanceMax;
					float ratio2 = 1f - num6 / num5;
					Add(section, positions, points, points[positions.Count - 1], currentPoint, ratio2);
				}
				return true;
			}

			private void Add(BGCurveBaseMath.SectionInfo section, List<Vector3> positions, List<BGCcSplitterPolyline.PolylinePoint> points, BGCcSplitterPolyline.PolylinePoint previousPoint, BGCurveBaseMath.SectionPointInfo nextPoint, float ratio)
			{
				Vector3 vector = Vector3.Lerp(previousPoint.Position, nextPoint.Position, ratio);
				positions.Add(vector);
				points.Add(new BGCcSplitterPolyline.PolylinePoint(vector, Mathf.Lerp(previousPoint.Distance, section.DistanceFromStartToOrigin + nextPoint.DistanceToSectionStart, ratio), (!calculatingTangents) ? Vector3.zero : Vector3.Lerp(previousPoint.Tangent, nextPoint.Tangent, ratio)));
			}

			private void Add(BGCurveBaseMath.SectionInfo section, List<Vector3> positions, List<BGCcSplitterPolyline.PolylinePoint> points, BGCurveBaseMath.SectionPointInfo previousPoint, BGCurveBaseMath.SectionPointInfo nextPoint, float ratio)
			{
				Vector3 vector = Vector3.Lerp(previousPoint.Position, nextPoint.Position, ratio);
				positions.Add(vector);
				points.Add(new BGCcSplitterPolyline.PolylinePoint(vector, section.DistanceFromStartToOrigin + Mathf.Lerp(previousPoint.DistanceToSectionStart, nextPoint.DistanceToSectionStart, ratio), (!calculatingTangents) ? Vector3.zero : Vector3.Lerp(previousPoint.Tangent, nextPoint.Tangent, ratio)));
			}

			protected void FillIn(BGCurveBaseMath.SectionInfo section, List<Vector3> result, int parts, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				float num = section.Distance / (float)parts;
				for (int i = 1; i <= parts; i++)
				{
					float num2 = num * (float)i;
					section.CalcByDistance(num2, out Vector3 position, out Vector3 tangent, calculatePosition: true, calculatingTangents);
					result.Add(position);
					points.Add(new BGCcSplitterPolyline.PolylinePoint(position, section.DistanceFromStartToOrigin + num2, tangent));
				}
			}

			protected abstract void FillInSplitSection(BGCurveBaseMath.SectionInfo section, List<Vector3> result, List<BGCcSplitterPolyline.PolylinePoint> points);
		}

		public sealed class PositionsProviderTotalParts : PositionsProvider
		{
			private int parts;

			private int reminderForCurved;

			private int partsPerSectionFloor;

			public void Init(BGCcMath math, int parts)
			{
				base.Init(math);
				this.parts = parts;
			}

			public override bool Comply(BGCcSplitterPolyline.SplitModeEnum splitMode)
			{
				return splitMode == BGCcSplitterPolyline.SplitModeEnum.PartsTotal;
			}

			public override void Build(List<Vector3> positions, int straightLinesCount, bool[] straightBits, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				BGCurve curve = Math.Curve;
				List<BGCurveBaseMath.SectionInfo> sectionInfos = Math.Math.SectionInfos;
				int count = sectionInfos.Count;
				float f = (float)(parts - straightLinesCount) / (float)(count - straightLinesCount);
				reminderForCurved = (int)((float)(parts - straightLinesCount) % (float)(count - straightLinesCount));
				partsPerSectionFloor = Mathf.FloorToInt(f);
				if (parts < count)
				{
					float distance = Math.GetDistance();
					if (parts == 1)
					{
						BGCurveBaseMath.SectionPointInfo sectionPointInfo = sectionInfos[0][0];
						positions.Add(sectionPointInfo.Position);
						points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo.Position, 0f, sectionPointInfo.Tangent));
						if (curve.Closed)
						{
							Vector3 tangent;
							Vector3 vector = Math.CalcByDistanceRatio(0.5f, out tangent);
							positions.Add(vector);
							points.Add(new BGCcSplitterPolyline.PolylinePoint(vector, distance * 0.5f, tangent));
						}
						else
						{
							BGCurveBaseMath.SectionInfo sectionInfo = sectionInfos[count - 1];
							BGCurveBaseMath.SectionPointInfo sectionPointInfo2 = sectionInfo[sectionInfo.PointsCount - 1];
							positions.Add(sectionPointInfo2.Position);
							points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo2.Position, sectionInfo.DistanceFromEndToOrigin, sectionPointInfo2.Tangent));
						}
					}
					else if (parts == 2 && curve.Closed)
					{
						BGCurveBaseMath.SectionPointInfo sectionPointInfo3 = sectionInfos[0][0];
						positions.Add(sectionPointInfo3.Position);
						points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo3.Position, 0f, sectionPointInfo3.Tangent));
						float num = 0.333333343f;
						Vector3 tangent2;
						Vector3 vector2 = Math.CalcByDistanceRatio(num, out tangent2);
						positions.Add(vector2);
						points.Add(new BGCcSplitterPolyline.PolylinePoint(vector2, distance * num, tangent2));
						float num2 = 2f / 3f;
						Vector3 tangent3;
						Vector3 vector3 = Math.CalcByDistanceRatio(num2, out tangent3);
						positions.Add(vector3);
						points.Add(new BGCcSplitterPolyline.PolylinePoint(vector3, distance * num2, tangent3));
					}
					else
					{
						for (int i = 0; i <= parts; i++)
						{
							float num3 = (float)i / (float)parts;
							Vector3 tangent4;
							Vector3 vector4 = Math.CalcByDistanceRatio(num3, out tangent4);
							positions.Add(vector4);
							points.Add(new BGCcSplitterPolyline.PolylinePoint(vector4, distance * num3, tangent4));
						}
					}
				}
				else
				{
					base.Build(positions, straightLinesCount, straightBits, points);
				}
			}

			protected override void FillInSplitSection(BGCurveBaseMath.SectionInfo section, List<Vector3> result, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				int num = partsPerSectionFloor;
				if (reminderForCurved > 0)
				{
					num++;
					reminderForCurved--;
				}
				FillIn(section, result, num, points);
			}
		}

		public sealed class PositionsProviderPartsPerSection : PositionsProvider
		{
			private int parts;

			public void Init(BGCcMath math, int partsPerSection)
			{
				base.Init(math);
				parts = partsPerSection;
			}

			public override bool Comply(BGCcSplitterPolyline.SplitModeEnum splitMode)
			{
				return splitMode == BGCcSplitterPolyline.SplitModeEnum.PartsPerSection;
			}

			protected override void FillInSplitSection(BGCurveBaseMath.SectionInfo section, List<Vector3> result, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				FillIn(section, result, parts, points);
			}
		}

		public sealed class PositionsProviderMath : PositionsProvider
		{
			public override bool Comply(BGCcSplitterPolyline.SplitModeEnum splitMode)
			{
				return splitMode == BGCcSplitterPolyline.SplitModeEnum.UseMathData;
			}

			protected override void FillInSplitSection(BGCurveBaseMath.SectionInfo section, List<Vector3> result, List<BGCcSplitterPolyline.PolylinePoint> points)
			{
				if (LastPointAdded)
				{
					return;
				}
				List<BGCurveBaseMath.SectionPointInfo> points2 = section.Points;
				int count = points2.Count;
				if (!DistanceMaxConstrained)
				{
					if (result.Capacity < count)
					{
						result.Capacity = count;
					}
					if (points.Capacity < count)
					{
						points.Capacity = count;
					}
				}
				for (int i = 1; i < count; i++)
				{
					BGCurveBaseMath.SectionPointInfo sectionPointInfo = points2[i];
					if (DistanceMinConstrained && result.Count == 0)
					{
						AddFirstPointIfNeeded(result, section, sectionPointInfo, points2[i - 1], points);
					}
					if (DistanceMaxConstrained && AddLastPointIfNeeded(result, section, sectionPointInfo, points2[i - 1], points))
					{
						break;
					}
					result.Add(sectionPointInfo.Position);
					points.Add(new BGCcSplitterPolyline.PolylinePoint(sectionPointInfo.Position, section.DistanceFromStartToOrigin + sectionPointInfo.DistanceToSectionStart, (!calculatingTangents) ? Vector3.zero : sectionPointInfo.Tangent));
				}
			}
		}

		private PositionsProvider positionsProvider;

		private bool[] straightBits;

		private PositionsProviderMath providerMath;

		private PositionsProviderPartsPerSection providerPartsPerSection;

		private PositionsProviderTotalParts providerTotalParts;

		public void Bind(List<Vector3> positions, BGCcMath math, Config config, List<BGCcSplitterPolyline.PolylinePoint> points)
		{
			positions.Clear();
			points.Clear();
			bool flag = math.IsCalculated(BGCurveBaseMath.Field.Tangent);
			BGCurveBaseMath math2 = math.Math;
			int sectionsCount = math2.SectionsCount;
			int straightLinesCount = 0;
			if (!config.DoNotOptimizeStraightLines)
			{
				if (straightBits == null || straightBits.Length < sectionsCount)
				{
					Array.Resize(ref straightBits, sectionsCount);
				}
				straightLinesCount = CountStraightLines(math2, straightBits);
			}
			InitProvider(ref positionsProvider, math, config).Build(positions, straightLinesCount, straightBits, points);
			if (config.UseLocal)
			{
				Matrix4x4 worldToLocalMatrix = config.Transform.worldToLocalMatrix;
				int count = positions.Count;
				for (int i = 0; i < count; i++)
				{
					Vector3 position = positions[i] = worldToLocalMatrix.MultiplyPoint(positions[i]);
					BGCcSplitterPolyline.PolylinePoint polylinePoint = points[i];
					points[i] = new BGCcSplitterPolyline.PolylinePoint(position, polylinePoint.Distance, (!flag) ? Vector3.zero : config.Transform.InverseTransformDirection(polylinePoint.Tangent));
				}
			}
		}

		public static int CountStraightLines(BGCurveBaseMath math, bool[] straight)
		{
			BGCurve curve = math.Curve;
			BGCurvePointI[] points = curve.Points;
			if (points.Length == 0)
			{
				return 0;
			}
			List<BGCurveBaseMath.SectionInfo> sectionInfos = math.SectionInfos;
			int count = sectionInfos.Count;
			bool flag = straight != null;
			int num = 0;
			bool flag2 = points[0].ControlType == BGCurvePoint.ControlTypeEnum.Absent;
			for (int i = 0; i < count; i++)
			{
				BGCurvePointI bGCurvePointI = (!curve.Closed || i != count - 1) ? points[i + 1] : points[0];
				bool flag3 = bGCurvePointI.ControlType == BGCurvePoint.ControlTypeEnum.Absent;
				if (flag2 && flag3)
				{
					if (flag)
					{
						straight[i] = true;
					}
					num++;
				}
				else if (flag)
				{
					straight[i] = false;
				}
				flag2 = flag3;
			}
			return num;
		}

		private PositionsProvider InitProvider(ref PositionsProvider positionsProvider, BGCcMath math, Config config)
		{
			BGCcSplitterPolyline.SplitModeEnum splitMode = config.SplitMode;
			bool flag = positionsProvider == null || !positionsProvider.Comply(splitMode);
			switch (splitMode)
			{
			case BGCcSplitterPolyline.SplitModeEnum.PartsTotal:
				if (flag)
				{
					if (providerTotalParts == null)
					{
						providerTotalParts = new PositionsProviderTotalParts();
					}
					positionsProvider = providerTotalParts;
				}
				providerTotalParts.Init(math, config.PartsTotal);
				break;
			case BGCcSplitterPolyline.SplitModeEnum.PartsPerSection:
				if (flag)
				{
					if (providerPartsPerSection == null)
					{
						providerPartsPerSection = new PositionsProviderPartsPerSection();
					}
					positionsProvider = providerPartsPerSection;
				}
				providerPartsPerSection.Init(math, config.PartsPerSection);
				break;
			default:
				if (flag)
				{
					if (providerMath == null)
					{
						providerMath = new PositionsProviderMath();
					}
					positionsProvider = providerMath;
				}
				providerMath.Init(math);
				break;
			}
			if (config.DistanceMin > 0f || config.DistanceMax > 0f)
			{
				if (splitMode != 0)
				{
					throw new Exception("DistanceMin and DistanceMax supported by SplitModeEnum.UseMathData mode only");
				}
				positionsProvider.DistanceMin = config.DistanceMin;
				positionsProvider.DistanceMax = config.DistanceMax;
			}
			return positionsProvider;
		}
	}
}
