using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_TailInfo
	{
		public int index;

		[NotSerialized]
		public BGCurvePointI point;

		public Vector3 position;

		public Vector3 tangent;

		public static FK3_TailInfo Default = new FK3_TailInfo
		{
			position = Vector3.zero,
			tangent = Vector3.right
		};

		public FK3_TailInfo Clone()
		{
			FK3_TailInfo fK3_TailInfo = new FK3_TailInfo();
			fK3_TailInfo.index = index;
			fK3_TailInfo.position = position;
			fK3_TailInfo.tangent = tangent;
			fK3_TailInfo.point = point;
			return fK3_TailInfo;
		}
	}
}
