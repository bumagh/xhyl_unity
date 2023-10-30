using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public class BGCurveAdaptiveMath : BGCurveBaseMath
	{
		public class ConfigAdaptive : Config
		{
			public float Tolerance = 0.2f;

			public ConfigAdaptive(Fields fields)
				: base(fields)
			{
			}
		}

		public const float MinTolerance = 0.1f;

		public const float MaxTolerance = 0.999975f;

		public const float DistanceTolerance = 0.01f;

		private const int RecursionLimit = 24;

		private bool ignoreSectionChangedCheckOverride;

		private float toleranceRatio;

		private float toleranceRatioSquared;

		private float tolerance;

		public BGCurveAdaptiveMath(BGCurve curve, ConfigAdaptive config)
			: base(curve, config)
		{
		}

		public override void Init(Config config)
		{
			ConfigAdaptive configAdaptive = (ConfigAdaptive)config;
			tolerance = Mathf.Clamp(configAdaptive.Tolerance, 0.1f, 0.999975f);
			tolerance *= tolerance;
			tolerance *= tolerance;
			toleranceRatio = 1f / (1f - tolerance);
			toleranceRatioSquared = toleranceRatio * toleranceRatio;
			ignoreSectionChangedCheckOverride = (base.config == null || Math.Abs(((ConfigAdaptive)base.config).Tolerance - tolerance) > 1E-05f);
			base.Init(config);
		}

		protected override bool Reset(SectionInfo section, BGCurvePointI from, BGCurvePointI to, int pointsCount)
		{
			return section.Reset(from, to, section.PointsCount, ignoreSectionChangedCheck || ignoreSectionChangedCheckOverride);
		}

		protected override bool IsUseDistanceToAdjustTangents(SectionInfo section, SectionInfo prevSection)
		{
			return true;
		}

		protected override void CalculateSplitSection(SectionInfo section, BGCurvePointI from, BGCurvePointI to)
		{
			bool calcTangents = cacheTangent && !config.UsePointPositionsToCalcTangents;
			List<SectionPointInfo> points = section.points;
			int count = points.Count;
			for (int i = 0; i < count; i++)
			{
				poolPointInfos.Add(points[i]);
			}
			points.Clear();
			Vector3 originalFrom = section.OriginalFrom;
			Vector3 originalFromControl = section.OriginalFromControl;
			Vector3 originalToControl = section.OriginalToControl;
			Vector3 originalTo = section.OriginalTo;
			int num = ((section.OriginalFromControlType != 0) ? 2 : 0) + ((section.OriginalToControlType != 0) ? 1 : 0);
			int num2 = poolPointInfos.Count - 1;
			SectionPointInfo sectionPointInfo;
			if (num2 >= 0)
			{
				sectionPointInfo = poolPointInfos[num2];
				poolPointInfos.RemoveAt(num2);
			}
			else
			{
				sectionPointInfo = new SectionPointInfo();
			}
			sectionPointInfo.Position = originalFrom;
			sectionPointInfo.DistanceToSectionStart = 0f;
			points.Add(sectionPointInfo);
			switch (num)
			{
			case 3:
				RecursiveCubicSplit(section, originalFrom.x, originalFrom.y, originalFrom.z, originalFromControl.x, originalFromControl.y, originalFromControl.z, originalToControl.x, originalToControl.y, originalToControl.z, originalTo.x, originalTo.y, originalTo.z, 0, calcTangents, 0.0, 1.0);
				break;
			case 2:
				RecursiveQuadraticSplit(section, originalFrom.x, originalFrom.y, originalFrom.z, originalFromControl.x, originalFromControl.y, originalFromControl.z, originalTo.x, originalTo.y, originalTo.z, 0, useSecond: false, calcTangents, 0.0, 1.0);
				break;
			case 1:
				RecursiveQuadraticSplit(section, originalFrom.x, originalFrom.y, originalFrom.z, originalToControl.x, originalToControl.y, originalToControl.z, originalTo.x, originalTo.y, originalTo.z, 0, useSecond: true, calcTangents, 0.0, 1.0);
				break;
			}
			num2 = poolPointInfos.Count - 1;
			SectionPointInfo sectionPointInfo2;
			if (num2 >= 0)
			{
				sectionPointInfo2 = poolPointInfos[num2];
				poolPointInfos.RemoveAt(num2);
			}
			else
			{
				sectionPointInfo2 = new SectionPointInfo();
			}
			sectionPointInfo2.Position = originalTo;
			points.Add(sectionPointInfo2);
			calcTangents = (cacheTangent && config.UsePointPositionsToCalcTangents);
			SectionPointInfo sectionPointInfo3 = points[0];
			for (int j = 1; j < points.Count; j++)
			{
				SectionPointInfo sectionPointInfo4 = points[j];
				Vector3 position = sectionPointInfo4.Position;
				Vector3 position2 = sectionPointInfo3.Position;
				double num3 = (double)position.x - (double)position2.x;
				double num4 = (double)position.y - (double)position2.y;
				double num5 = (double)position.z - (double)position2.z;
				sectionPointInfo4.DistanceToSectionStart = sectionPointInfo3.DistanceToSectionStart + (float)Math.Sqrt(num3 * num3 + num4 * num4 + num5 * num5);
				if (calcTangents)
				{
					sectionPointInfo4.Tangent = Vector3.Normalize(position - position2);
				}
				sectionPointInfo3 = sectionPointInfo4;
			}
			if (!cacheTangent)
			{
				return;
			}
			if (config.UsePointPositionsToCalcTangents)
			{
				sectionPointInfo.Tangent = (points[1].Position - sectionPointInfo.Position).normalized;
				sectionPointInfo2.Tangent = points[points.Count - 2].Tangent;
				return;
			}
			switch (num)
			{
			case 0:
				sectionPointInfo.Tangent = (sectionPointInfo2.Tangent = (sectionPointInfo2.Position - sectionPointInfo.Position).normalized);
				break;
			case 1:
				sectionPointInfo.Tangent = Vector3.Normalize(section.OriginalToControl - section.OriginalFrom);
				sectionPointInfo2.Tangent = Vector3.Normalize(section.OriginalTo - section.OriginalToControl);
				break;
			case 2:
				sectionPointInfo.Tangent = Vector3.Normalize(section.OriginalFromControl - section.OriginalFrom);
				sectionPointInfo2.Tangent = Vector3.Normalize(section.OriginalTo - section.OriginalFromControl);
				break;
			case 3:
				sectionPointInfo.Tangent = Vector3.Normalize(section.OriginalFromControl - section.OriginalFrom);
				sectionPointInfo2.Tangent = Vector3.Normalize(section.OriginalTo - section.OriginalToControl);
				break;
			}
		}

		private void RecursiveQuadraticSplit(SectionInfo section, double x0, double y0, double z0, double x1, double y1, double z1, double x2, double y2, double z2, int level, bool useSecond, bool calcTangents, double fromT, double toT)
		{
			if (level > 24)
			{
				return;
			}
			double num = x0 - x1;
			double num2 = y0 - y1;
			double num3 = z0 - z1;
			double num4 = x1 - x2;
			double num5 = y1 - y2;
			double num6 = z1 - z2;
			double num7 = x0 - x2;
			double num8 = y0 - y2;
			double num9 = z0 - z2;
			double num10 = num7 * num7 + num8 * num8 + num9 * num9;
			double num11 = num * num + num2 * num2 + num3 * num3;
			double num12 = num4 * num4 + num5 * num5 + num6 * num6;
			double num13 = num10 * (double)toleranceRatioSquared - num11 - num12;
			if (!(4.0 * num11 * num12 < num13 * num13) && !(num10 + num11 + num12 < 0.0099999997764825821))
			{
				double num14 = (x0 + x1) * 0.5;
				double num15 = (y0 + y1) * 0.5;
				double num16 = (z0 + z1) * 0.5;
				double num17 = (x1 + x2) * 0.5;
				double num18 = (y1 + y2) * 0.5;
				double num19 = (z1 + z2) * 0.5;
				double num20 = (num14 + num17) * 0.5;
				double num21 = (num15 + num18) * 0.5;
				double num22 = (num16 + num19) * 0.5;
				double num23 = (!calcTangents) ? 0.0 : ((fromT + toT) * 0.5);
				Vector3 pos = new Vector3((float)num20, (float)num21, (float)num22);
				if (curve.SnapType == BGCurve.SnapTypeEnum.Curve)
				{
					curve.ApplySnapping(ref pos);
				}
				RecursiveQuadraticSplit(section, x0, y0, z0, num14, num15, num16, num20, num21, num22, level + 1, useSecond, calcTangents, fromT, num23);
				int num24 = poolPointInfos.Count - 1;
				SectionPointInfo sectionPointInfo;
				if (num24 >= 0)
				{
					sectionPointInfo = poolPointInfos[num24];
					poolPointInfos.RemoveAt(num24);
				}
				else
				{
					sectionPointInfo = new SectionPointInfo();
				}
				sectionPointInfo.Position = pos;
				section.points.Add(sectionPointInfo);
				if (calcTangents)
				{
					Vector3 vector = (!useSecond) ? section.OriginalFromControl : section.OriginalToControl;
					sectionPointInfo.Tangent = Vector3.Normalize(2f * (1f - (float)num23) * (vector - section.OriginalFrom) + 2f * (float)num23 * (section.OriginalTo - vector));
				}
				RecursiveQuadraticSplit(section, num20, num21, num22, num17, num18, num19, x2, y2, z2, level + 1, useSecond, calcTangents, num23, toT);
			}
		}

		private void RecursiveCubicSplit(SectionInfo section, double x0, double y0, double z0, double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3, int level, bool calcTangents, double fromT, double toT)
		{
			if (level > 24)
			{
				return;
			}
			double num = x0 - x1;
			double num2 = y0 - y1;
			double num3 = z0 - z1;
			double num4 = x1 - x2;
			double num5 = y1 - y2;
			double num6 = z1 - z2;
			double num7 = x2 - x3;
			double num8 = y2 - y3;
			double num9 = z2 - z3;
			double num10 = x0 - x3;
			double num11 = y0 - y3;
			double num12 = z0 - z3;
			double num13 = num10 * num10 + num11 * num11 + num12 * num12;
			double num14 = num * num + num2 * num2 + num3 * num3;
			double num15 = num4 * num4 + num5 * num5 + num6 * num6;
			double num16 = num7 * num7 + num8 * num8 + num9 * num9;
			if (!(Math.Sqrt(num14 * num15) + Math.Sqrt(num15 * num16) + Math.Sqrt(num14 * num16) < (num13 * (double)toleranceRatioSquared - num14 - num15 - num16) * 0.5) && !(num13 + num14 + num15 + num16 < 0.0099999997764825821))
			{
				double num17 = (x0 + x1) * 0.5;
				double num18 = (y0 + y1) * 0.5;
				double num19 = (z0 + z1) * 0.5;
				double num20 = (x1 + x2) * 0.5;
				double num21 = (y1 + y2) * 0.5;
				double num22 = (z1 + z2) * 0.5;
				double num23 = (x2 + x3) * 0.5;
				double num24 = (y2 + y3) * 0.5;
				double num25 = (z2 + z3) * 0.5;
				double num26 = (num17 + num20) * 0.5;
				double num27 = (num18 + num21) * 0.5;
				double num28 = (num19 + num22) * 0.5;
				double num29 = (num20 + num23) * 0.5;
				double num30 = (num21 + num24) * 0.5;
				double num31 = (num22 + num25) * 0.5;
				double num32 = (num26 + num29) * 0.5;
				double num33 = (num27 + num30) * 0.5;
				double num34 = (num28 + num31) * 0.5;
				double num35 = (!calcTangents) ? 0.0 : ((fromT + toT) * 0.5);
				Vector3 pos = new Vector3((float)num32, (float)num33, (float)num34);
				if (curve.SnapType == BGCurve.SnapTypeEnum.Curve)
				{
					curve.ApplySnapping(ref pos);
				}
				RecursiveCubicSplit(section, x0, y0, z0, num17, num18, num19, num26, num27, num28, num32, num33, num34, level + 1, calcTangents, fromT, num35);
				int num36 = poolPointInfos.Count - 1;
				SectionPointInfo sectionPointInfo;
				if (num36 >= 0)
				{
					sectionPointInfo = poolPointInfos[num36];
					poolPointInfos.RemoveAt(num36);
				}
				else
				{
					sectionPointInfo = new SectionPointInfo();
				}
				sectionPointInfo.Position = pos;
				section.points.Add(sectionPointInfo);
				if (calcTangents)
				{
					double num37 = 1.0 - num35;
					sectionPointInfo.Tangent = Vector3.Normalize(3f * (float)(num37 * num37) * (section.OriginalFromControl - section.OriginalFrom) + 6f * (float)(num37 * num35) * (section.OriginalToControl - section.OriginalFromControl) + 3f * (float)(num35 * num35) * (section.OriginalTo - section.OriginalToControl));
				}
				RecursiveCubicSplit(section, num32, num33, num34, num29, num30, num31, num23, num24, num25, x3, y3, z3, level + 1, calcTangents, num35, toT);
			}
		}

		public override string ToString()
		{
			return "Adaptive Math for curve (" + base.Curve + "), sections=" + base.SectionsCount;
		}
	}
}
