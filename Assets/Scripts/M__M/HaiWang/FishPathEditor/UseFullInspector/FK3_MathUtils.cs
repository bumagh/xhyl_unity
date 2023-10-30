using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public static class FK3_MathUtils
	{
		public static Vector2 V2Rotate(Vector2 v, float angle)
		{
			float num = Mathf.Sin(angle * ((float)Math.PI / 180f));
			float num2 = Mathf.Cos(angle * ((float)Math.PI / 180f));
			float x = v.x;
			float y = v.y;
			v.x = num2 * x - num * y;
			v.y = num * x + num2 * y;
			return v;
		}

		public static Vector2 V2Rotated(Vector2 v, float angle)
		{
			Vector2 v2 = new Vector2(v.x, v.y);
			return V2Rotate(v2, angle);
		}
	}
}
