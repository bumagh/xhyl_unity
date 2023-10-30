using BansheeGz.BGSpline.Curve;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	public class BGTriangulator2D
	{
		public class Config
		{
			public enum UvModeEnum
			{
				Scale,
				PPU
			}

			public UvModeEnum UvMode;

			public bool Closed;

			public bool Flip;

			public bool AutoFlip;

			public BGCurve.Mode2DEnum Mode2D;

			public bool DoubleSided;

			public Vector2 ScaleUV = Vector2.one;

			public Vector2 OffsetUV = Vector2.zero;

			public Vector2 ScaleBackUV = Vector2.one;

			public Vector2 OffsetBackUV = Vector2.zero;

			public BGPpu TextureSize;

			public BGPpu PixelsPerUnit;

			public BGPpu PixelsPerUnitBack;
		}

		private const float MinUvScale = 1E-06f;

		private const float MaxUvScale = 1000000f;

		private static readonly List<int> V = new List<int>();

		private static readonly List<Vector3> Vertices = new List<Vector3>();

		private static readonly List<Vector2> Points = new List<Vector2>();

		private static readonly List<Vector2> Uvs = new List<Vector2>();

		private static readonly List<int> Triangles = new List<int>();

		public void Bind(Mesh mesh, List<Vector3> positions, Config config)
		{
			int num = positions.Count;
			if (config.Closed)
			{
				num--;
			}
			Clear();
			if (num > 2)
			{
				Vector4 minMax = new Vector4(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = positions[i];
					Vector3 item;
					Vector2 item2;
					switch (config.Mode2D)
					{
					case BGCurve.Mode2DEnum.XY:
						item = new Vector3(vector.x, vector.y);
						item2 = new Vector2(vector.x, vector.y);
						break;
					case BGCurve.Mode2DEnum.XZ:
						item = new Vector3(vector.x, 0f, vector.z);
						item2 = new Vector2(vector.x, vector.z);
						break;
					default:
						item = new Vector3(0f, vector.y, vector.z);
						item2 = new Vector2(vector.y, vector.z);
						break;
					}
					Vertices.Add(item);
					Points.Add(item2);
					if (item2.x < minMax.x)
					{
						minMax.x = item2.x;
					}
					if (item2.y < minMax.y)
					{
						minMax.y = item2.y;
					}
					if (item2.x > minMax.z)
					{
						minMax.z = item2.x;
					}
					if (item2.y > minMax.w)
					{
						minMax.w = item2.y;
					}
				}
				Triangulate(Points, Triangles);
				if (config.AutoFlip)
				{
					Vector3 lhs = Points[Triangles[1]] - Points[Triangles[0]];
					Vector3 rhs = Points[Triangles[2]] - Points[Triangles[0]];
					Vector3 lhs2 = Vector3.Cross(lhs, rhs);
					float num2 = Vector3.Dot(lhs2, Camera.main.transform.forward);
					if (num2 > 0f)
					{
						Triangles.Reverse();
					}
				}
				else if (!config.Flip)
				{
					Triangles.Reverse();
				}
				if (config.UvMode == Config.UvModeEnum.Scale)
				{
					Bind(minMax, config.ScaleUV, config.OffsetUV);
				}
				else
				{
					Bind(minMax, config.PixelsPerUnit, config.TextureSize);
				}
				if (config.DoubleSided)
				{
					int count = Vertices.Count;
					for (int j = 0; j < count; j++)
					{
						Vertices.Add(Vertices[j]);
					}
					int count2 = Triangles.Count;
					for (int num3 = count2 - 1; num3 >= 0; num3--)
					{
						Triangles.Add(Triangles[num3] + count);
					}
					if (config.UvMode == Config.UvModeEnum.Scale)
					{
						Bind(minMax, config.ScaleBackUV, config.OffsetBackUV);
					}
					else
					{
						Bind(minMax, config.PixelsPerUnitBack, config.TextureSize);
					}
				}
			}
			mesh.Clear();
			mesh.SetVertices(Vertices);
			mesh.SetTriangles(Triangles, 0);
			mesh.SetUVs(0, Uvs);
			mesh.RecalculateNormals();
			Clear();
		}

		private void Clear()
		{
			Vertices.Clear();
			Triangles.Clear();
			Uvs.Clear();
			Points.Clear();
		}

		private static void Bind(Vector4 minMax, Vector2 scale, Vector2 offset)
		{
			float num = minMax.z - minMax.x;
			float num2 = minMax.w - minMax.y;
			int count = Points.Count;
			float num3 = Mathf.Clamp(scale.x, 1E-06f, 1000000f);
			float num4 = Mathf.Clamp(scale.y, 1E-06f, 1000000f);
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = Points[i];
				Uvs.Add(new Vector2(offset.x + (vector.x - minMax.x) / num * num3, offset.y + (vector.y - minMax.y) / num2 * num4));
			}
		}

		private void Bind(Vector4 minMax, BGPpu pixelsPerUnit, BGPpu textureSize)
		{
			float num = (float)pixelsPerUnit.X / (float)textureSize.X;
			float num2 = (float)pixelsPerUnit.Y / (float)textureSize.Y;
			int count = Points.Count;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = Points[i];
				float x = (vector.x - minMax.x) * num;
				float y = (vector.y - minMax.y) * num2;
				Uvs.Add(new Vector2(x, y));
			}
		}

		private static void Triangulate(List<Vector2> points, List<int> tris)
		{
			tris.Clear();
			int count = points.Count;
			if (count < 3)
			{
				return;
			}
			V.Clear();
			if (Area(points) > 0f)
			{
				for (int i = 0; i < count; i++)
				{
					V.Add(i);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					V.Add(count - 1 - j);
				}
			}
			int num = count;
			int num2 = 2 * num;
			int num3 = 0;
			int num4 = num - 1;
			while (num > 2 && num2-- > 0)
			{
				int num6 = num4;
				if (num <= num6)
				{
					num6 = 0;
				}
				num4 = num6 + 1;
				if (num <= num4)
				{
					num4 = 0;
				}
				int num7 = num4 + 1;
				if (num <= num7)
				{
					num7 = 0;
				}
				if (Snip(points, num6, num4, num7, num, V))
				{
					int item = V[num6];
					int item2 = V[num4];
					int item3 = V[num7];
					tris.Add(item);
					tris.Add(item2);
					tris.Add(item3);
					num3++;
					int num8 = num4;
					for (int k = num4 + 1; k < num; k++)
					{
						V[num8] = V[k];
						num8++;
					}
					num--;
					num2 = 2 * num;
				}
			}
		}

		private static float Area(List<Vector2> points)
		{
			int count = points.Count;
			float num = 0f;
			int index = count - 1;
			int num2 = 0;
			while (num2 < count)
			{
				Vector2 vector = points[index];
				Vector2 vector2 = points[num2];
				num += vector.x * vector2.y - vector2.x * vector.y;
				index = num2++;
			}
			return num * 0.5f;
		}

		private static bool Snip(List<Vector2> points, int u, int v, int w, int n, List<int> V)
		{
			Vector2 a = points[V[u]];
			Vector2 b = points[V[v]];
			Vector2 c = points[V[w]];
			if (Mathf.Epsilon > (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 p = points[V[i]];
					if (InsideTriangle(a, b, c, p))
					{
						return false;
					}
				}
			}
			return true;
		}

		private static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			float num = C.x - B.x;
			float num2 = C.y - B.y;
			float num3 = A.x - C.x;
			float num4 = A.y - C.y;
			float num5 = B.x - A.x;
			float num6 = B.y - A.y;
			float num7 = P.x - A.x;
			float num8 = P.y - A.y;
			float num9 = P.x - B.x;
			float num10 = P.y - B.y;
			float num11 = P.x - C.x;
			float num12 = P.y - C.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}
	}
}
