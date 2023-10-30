using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[Obsolete("Use BGCurveBaseMath. This class is for testing purpose only")]
	public class BGCurveFormulaMath : BGCurveBaseMath
	{
		private float[] bakedT;

		private float[] bakedT2;

		private float[] bakedTr2;

		private float[] bakedT3;

		private float[] bakedTr3;

		private float[] bakedTr2xTx3;

		private float[] bakedT2xTrx3;

		private float[] bakedTxTrx2;

		private float[] bakedTr2x3;

		private float[] bakedTxTrx6;

		private float[] bakedT2x3;

		private float[] bakedTx2;

		private float[] bakedTrx2;

		public BGCurveFormulaMath(BGCurve curve, Config config)
			: base(curve, config)
		{
		}

		protected override void AfterInit(Config config)
		{
			int parts = config.Parts;
			int newSize = parts + 1;
			Array.Resize(ref bakedT, newSize);
			Array.Resize(ref bakedT2, newSize);
			Array.Resize(ref bakedTr2, newSize);
			Array.Resize(ref bakedT3, newSize);
			Array.Resize(ref bakedTr3, newSize);
			Array.Resize(ref bakedTr2xTx3, newSize);
			Array.Resize(ref bakedT2xTrx3, newSize);
			Array.Resize(ref bakedTxTrx2, newSize);
			if (base.NeedTangentFormula)
			{
				Array.Resize(ref bakedTr2x3, newSize);
				Array.Resize(ref bakedTxTrx6, newSize);
				Array.Resize(ref bakedT2x3, newSize);
				Array.Resize(ref bakedTx2, newSize);
				Array.Resize(ref bakedTrx2, newSize);
			}
			for (int i = 0; i <= parts; i++)
			{
				float num = (float)i / (float)parts;
				float num2 = 1f - num;
				float num3 = num * num;
				float num4 = num2 * num2;
				bakedT[i] = num;
				bakedT2[i] = num3;
				bakedTr2[i] = num4;
				bakedT3[i] = num3 * num;
				bakedTr3[i] = num4 * num2;
				bakedTr2xTx3[i] = 3f * num4 * num;
				bakedT2xTrx3[i] = 3f * num2 * num3;
				bakedTxTrx2[i] = 2f * num2 * num;
				if (base.NeedTangentFormula)
				{
					bakedTr2x3[i] = 3f * num4;
					bakedTxTrx6[i] = 6f * num2 * num;
					bakedT2x3[i] = 3f * num3;
					bakedTx2[i] = 2f * num;
					bakedTrx2[i] = 2f * num2;
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			bakedTrx2 = (bakedTx2 = (bakedT2x3 = (bakedTxTrx6 = (bakedTr2x3 = (bakedTxTrx2 = (bakedT2xTrx3 = (bakedTr2xTx3 = (bakedTr3 = (bakedT3 = (bakedTr2 = (bakedT2 = (bakedT = new float[0]))))))))))));
		}

		protected override void CalculateSplitSection(SectionInfo section, BGCurvePointI from, BGCurvePointI to)
		{
			Resize(section.points, config.Parts + 1);
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
			Vector3 vector2 = Vector3.zero;
			Vector3 tangent = Vector3.zero;
			if (flag3)
			{
				vector2 = originalTo - originalFrom;
				if (cacheTangent)
				{
					tangent = (to.PositionWorld - from.PositionWorld).normalized;
				}
			}
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = Vector3.zero;
			Vector3 vector5 = Vector3.zero;
			Vector3 vector6 = Vector3.zero;
			if (!config.UsePointPositionsToCalcTangents && cacheTangent)
			{
				vector3 = vector - originalFrom;
				if (flag4)
				{
					vector4 = originalToControl - vector;
					vector5 = originalTo - originalToControl;
				}
				else
				{
					vector6 = originalTo - vector;
				}
			}
			int num = bakedT.Length;
			for (int i = 0; i < num; i++)
			{
				object sectionPointInfo = section.points[i];
				if (sectionPointInfo == null)
				{
					SectionPointInfo sectionPointInfo2 = new SectionPointInfo();
					section.points[i] = sectionPointInfo2;
					sectionPointInfo = sectionPointInfo2;
				}
				SectionPointInfo sectionPointInfo3 = (SectionPointInfo)sectionPointInfo;
				Vector3 pos;
				if (flag3)
				{
					float num2 = bakedT[i];
					pos = new Vector3(originalFrom.x + vector2.x * num2, originalFrom.y + vector2.y * num2, originalFrom.z + vector2.z * num2);
					if (flag5)
					{
						curve.ApplySnapping(ref pos);
					}
					sectionPointInfo3.Position = pos;
					if (cacheTangent)
					{
						sectionPointInfo3.Tangent = tangent;
					}
				}
				else
				{
					if (flag4)
					{
						float num3 = bakedTr3[i];
						float num4 = bakedTr2xTx3[i];
						float num5 = bakedT2xTrx3[i];
						float num6 = bakedT3[i];
						pos = new Vector3(num3 * originalFrom.x + num4 * vector.x + num5 * originalToControl.x + num6 * originalTo.x, num3 * originalFrom.y + num4 * vector.y + num5 * originalToControl.y + num6 * originalTo.y, num3 * originalFrom.z + num4 * vector.z + num5 * originalToControl.z + num6 * originalTo.z);
					}
					else
					{
						float num7 = bakedTr2[i];
						float num8 = bakedTxTrx2[i];
						float num9 = bakedT2[i];
						pos = new Vector3(num7 * originalFrom.x + num8 * vector.x + num9 * originalTo.x, num7 * originalFrom.y + num8 * vector.y + num9 * originalTo.y, num7 * originalFrom.z + num8 * vector.z + num9 * originalTo.z);
					}
					if (flag5)
					{
						curve.ApplySnapping(ref pos);
					}
					sectionPointInfo3.Position = pos;
					if (cacheTangent)
					{
						if (config.UsePointPositionsToCalcTangents)
						{
							if (i != 0)
							{
								SectionPointInfo sectionPointInfo4 = section[i - 1];
								Vector3 position = sectionPointInfo4.Position;
								Vector3 vector7 = new Vector3(pos.x - position.x, pos.y - position.y, pos.z - position.z);
								float num10 = (float)Math.Sqrt((double)vector7.x * (double)vector7.x + (double)vector7.y * (double)vector7.y + (double)vector7.z * (double)vector7.z);
								vector7 = (sectionPointInfo4.Tangent = ((!((double)num10 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector7.x / num10, vector7.y / num10, vector7.z / num10)));
								if (i == config.Parts)
								{
									sectionPointInfo3.Tangent = sectionPointInfo4.Tangent;
								}
							}
						}
						else
						{
							Vector3 vector8;
							if (flag4)
							{
								float num11 = bakedTr2x3[i];
								float num12 = bakedTxTrx6[i];
								float num13 = bakedT2x3[i];
								vector8 = new Vector3(num11 * vector3.x + num12 * vector4.x + num13 * vector5.x, num11 * vector3.y + num12 * vector4.y + num13 * vector5.y, num11 * vector3.z + num12 * vector4.z + num13 * vector5.z);
							}
							else
							{
								float num14 = bakedTrx2[i];
								float num15 = bakedTx2[i];
								vector8 = new Vector3(num14 * vector3.x + num15 * vector6.x, num14 * vector3.y + num15 * vector6.y, num14 * vector3.z + num15 * vector6.z);
							}
							float num16 = (float)Math.Sqrt((double)vector8.x * (double)vector8.x + (double)vector8.y * (double)vector8.y + (double)vector8.z * (double)vector8.z);
							vector8 = (sectionPointInfo3.Tangent = ((!((double)num16 > 9.99999974737875E-06)) ? Vector3.zero : new Vector3(vector8.x / num16, vector8.y / num16, vector8.z / num16)));
						}
					}
				}
				if (i != 0)
				{
					Vector3 position2 = section[i - 1].Position;
					double num17 = pos.x - position2.x;
					double num18 = pos.y - position2.y;
					double num19 = pos.z - position2.z;
					sectionPointInfo3.DistanceToSectionStart = section[i - 1].DistanceToSectionStart + (float)Math.Sqrt(num17 * num17 + num18 * num18 + num19 * num19);
				}
			}
		}
	}
}
