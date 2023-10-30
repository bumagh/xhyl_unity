using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcSweep2D")]
	[DisallowMultipleComponent]
	[CcDescriptor(Description = "Sweep a line or 2d spline along another 2d spline.", Name = "Sweep 2D", Icon = "BGCcSweep2d123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcSweep2D")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class BGCcSweep2D : BGCcSplitterPolyline
	{
		public enum ProfileModeEnum
		{
			Line,
			Spline
		}

		private struct PositionWithU
		{
			public Vector3 Position;

			public float U;
		}

		[SerializeField]
		[Tooltip("Profile mode.\r\n StraightLine -use straight line as cross section;\r\n Spline - use 2d spline as cross section;")]
		private ProfileModeEnum profileMode;

		[SerializeField]
		[Tooltip("Line width for StraightLine profile mode")]
		private float lineWidth = 1f;

		[Tooltip("U coordinate for line start")]
		[SerializeField]
		private float uCoordinateStart;

		[SerializeField]
		[Tooltip("U coordinate for line end")]
		private float uCoordinateEnd = 1f;

		[Tooltip("Profile spline for Spline profile mode")]
		[SerializeField]
		private BGCcSplitterPolyline profileSpline;

		[Tooltip("V coordinate multiplier")]
		[SerializeField]
		private float vCoordinateScale = 1f;

		[Tooltip("Swap U with V coordinate")]
		[SerializeField]
		private bool swapUV;

		[SerializeField]
		[Tooltip("Swap mesh normals direction")]
		private bool swapNormals;

		private static readonly List<PositionWithU> crossSectionList = new List<PositionWithU>();

		[NonSerialized]
		private MeshFilter meshFilter;

		private readonly List<Vector3> vertices = new List<Vector3>();

		private readonly List<Vector2> uvs = new List<Vector2>();

		private readonly List<int> triangles = new List<int>();

		public ProfileModeEnum ProfileMode
		{
			get
			{
				return profileMode;
			}
			set
			{
				ParamChanged(ref profileMode, value);
			}
		}

		public float LineWidth
		{
			get
			{
				return lineWidth;
			}
			set
			{
				ParamChanged(ref lineWidth, value);
			}
		}

		public float UCoordinateStart
		{
			get
			{
				return uCoordinateStart;
			}
			set
			{
				ParamChanged(ref uCoordinateStart, value);
			}
		}

		public float UCoordinateEnd
		{
			get
			{
				return uCoordinateEnd;
			}
			set
			{
				ParamChanged(ref uCoordinateEnd, value);
			}
		}

		public BGCcSplitterPolyline ProfileSpline
		{
			get
			{
				return profileSpline;
			}
			set
			{
				ParamChanged(ref profileSpline, value);
			}
		}

		public bool SwapUv
		{
			get
			{
				return swapUV;
			}
			set
			{
				ParamChanged(ref swapUV, value);
			}
		}

		public bool SwapNormals
		{
			get
			{
				return swapNormals;
			}
			set
			{
				ParamChanged(ref swapNormals, value);
			}
		}

		public float VCoordinateScale
		{
			get
			{
				return vCoordinateScale;
			}
			set
			{
				ParamChanged(ref vCoordinateScale, value);
			}
		}

		public override string Error => ChoseMessage(base.Error, delegate
		{
			if (!base.Curve.Mode2DOn)
			{
				return "Curve should be in 2D mode";
			}
			if (profileMode == ProfileModeEnum.Spline)
			{
				if (profileSpline == null)
				{
					return "Profile spline is not set.";
				}
				if (profileSpline.Curve.Mode2D != BGCurve.Mode2DEnum.XY)
				{
					return "Profile spline should be in XY 2D mode.";
				}
				profileSpline.InvalidateData();
				if (profileSpline.PointsCount < 2)
				{
					return "Profile spline should have at least 2 points.";
				}
			}
			int num = (profileMode != 0) ? profileSpline.PointsCount : 2;
			return (base.PointsCount * num > 65534) ? "Vertex count per mesh limit is exceeded ( > 65534)" : null;
		});

		public MeshFilter MeshFilter
		{
			get
			{
				if (meshFilter == null)
				{
					meshFilter = GetComponent<MeshFilter>();
				}
				return meshFilter;
			}
		}

		public override void Start()
		{
			useLocal = true;
			base.Start();
			if (MeshFilter.sharedMesh == null)
			{
				UpdateUI();
			}
		}

		public void UpdateUI()
		{
			if (Error != null)
			{
				return;
			}
			if (!UseLocal)
			{
				useLocal = true;
				dataValid = false;
			}
			List<Vector3> positions = base.Positions;
			if (positions.Count < 2)
			{
				return;
			}
			MeshFilter meshFilter;
			try
			{
				meshFilter = MeshFilter;
			}
			catch (MissingReferenceException)
			{
				RemoveListeners();
				return;
			}
			Mesh mesh = meshFilter.sharedMesh;
			if (mesh == null)
			{
				mesh = (meshFilter.mesh = new Mesh());
			}
			crossSectionList.Clear();
			triangles.Clear();
			uvs.Clear();
			vertices.Clear();
			if (profileMode == ProfileModeEnum.Line)
			{
				crossSectionList.Add(new PositionWithU
				{
					Position = Vector3.left * lineWidth * 0.5f,
					U = uCoordinateStart
				});
				crossSectionList.Add(new PositionWithU
				{
					Position = Vector3.right * lineWidth * 0.5f,
					U = uCoordinateEnd
				});
			}
			else
			{
				List<Vector3> positions2 = profileSpline.Positions;
				for (int i = 0; i < positions2.Count; i++)
				{
					crossSectionList.Add(new PositionWithU
					{
						Position = positions2[i]
					});
				}
			}
			int count = crossSectionList.Count;
			float num = 0f;
			for (int j = 0; j < count - 1; j++)
			{
				float num2 = num;
				PositionWithU positionWithU = crossSectionList[j];
				Vector3 position = positionWithU.Position;
				PositionWithU positionWithU2 = crossSectionList[j + 1];
				num = num2 + Vector3.Distance(position, positionWithU2.Position);
			}
			if (profileMode == ProfileModeEnum.Spline)
			{
				float num3 = 0f;
				for (int k = 0; k < count - 1; k++)
				{
					List<PositionWithU> list = crossSectionList;
					int index = k;
					PositionWithU value = default(PositionWithU);
					PositionWithU positionWithU3 = crossSectionList[k];
					value.Position = positionWithU3.Position;
					value.U = uCoordinateStart + num3 / num * (uCoordinateEnd - uCoordinateStart);
					list[index] = value;
					float num4 = num3;
					PositionWithU positionWithU4 = crossSectionList[k];
					Vector3 position2 = positionWithU4.Position;
					PositionWithU positionWithU5 = crossSectionList[k + 1];
					num3 = num4 + Vector3.Distance(position2, positionWithU5.Position);
				}
				List<PositionWithU> list2 = crossSectionList;
				int index2 = crossSectionList.Count - 1;
				PositionWithU value2 = default(PositionWithU);
				PositionWithU positionWithU6 = crossSectionList[crossSectionList.Count - 1];
				value2.Position = positionWithU6.Position;
				value2.U = uCoordinateEnd;
				list2[index2] = value2;
			}
			Vector3 upwards;
			switch (base.Curve.Mode2D)
			{
			case BGCurve.Mode2DEnum.XY:
				upwards = ((!swapNormals) ? Vector3.forward : Vector3.back);
				break;
			case BGCurve.Mode2DEnum.XZ:
				upwards = ((!swapNormals) ? Vector3.up : Vector3.down);
				break;
			case BGCurve.Mode2DEnum.YZ:
				upwards = ((!swapNormals) ? Vector3.right : Vector3.left);
				break;
			default:
				throw new ArgumentOutOfRangeException("Curve.Mode2D");
			}
			bool closed = base.Curve.Closed;
			Vector3 vector3;
			if (closed)
			{
				Vector3 vector = positions[1] - positions[0];
				float magnitude = vector.magnitude;
				Vector3 vector2 = positions[positions.Count - 1] - positions[positions.Count - 2];
				float magnitude2 = vector2.magnitude;
				float d = magnitude / magnitude2;
				vector3 = vector.normalized + vector2.normalized * d;
			}
			else
			{
				vector3 = positions[1] - positions[0];
			}
			Vector3 vector4 = vector3;
			Vector3 a = vector4.normalized;
			float num5 = (positions[1] - positions[0]).magnitude;
			Matrix4x4 matrix4x = Matrix4x4.TRS(positions[0], Quaternion.LookRotation(vector4, upwards), Vector3.one);
			for (int l = 0; l < count; l++)
			{
				PositionWithU positionWithU7 = crossSectionList[l];
				vertices.Add(matrix4x.MultiplyPoint(positionWithU7.Position));
				uvs.Add((!swapUV) ? new Vector2(positionWithU7.U, 0f) : new Vector2(0f, positionWithU7.U));
			}
			float num6 = num5;
			int count2 = positions.Count;
			for (int m = 1; m < count2; m++)
			{
				Vector3 vector5 = positions[m];
				bool flag = m == count2 - 1;
				Vector3 vector6 = (!flag) ? (positions[m + 1] - vector5) : vector4;
				Vector3 normalized = vector6.normalized;
				float magnitude3 = vector6.magnitude;
				float d2 = magnitude3 / num5;
				Vector3 forward = normalized + a * d2;
				if (flag && closed)
				{
					forward = vector3;
				}
				matrix4x = Matrix4x4.TRS(vector5, Quaternion.LookRotation(forward, upwards), Vector3.one);
				float num7 = num6 / num * vCoordinateScale;
				for (int n = 0; n < count; n++)
				{
					PositionWithU positionWithU8 = crossSectionList[n];
					vertices.Add(matrix4x.MultiplyPoint(positionWithU8.Position));
					uvs.Add((!swapUV) ? new Vector2(positionWithU8.U, num7) : new Vector2(num7, positionWithU8.U));
				}
				int num8 = vertices.Count - count * 2;
				int num9 = vertices.Count - count;
				for (int num10 = 0; num10 < count - 1; num10++)
				{
					triangles.Add(num8 + num10);
					triangles.Add(num9 + num10);
					triangles.Add(num8 + num10 + 1);
					triangles.Add(num8 + num10 + 1);
					triangles.Add(num9 + num10);
					triangles.Add(num9 + num10 + 1);
				}
				num6 += magnitude3;
				vector4 = vector6;
				a = normalized;
				num5 = magnitude3;
			}
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetUVs(0, uvs);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateNormals();
		}

		protected override void UpdateRequested(object sender, EventArgs e)
		{
			base.UpdateRequested(sender, e);
			UpdateUI();
		}
	}
}
