using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class TailInfo
	{
		public int index;

		[NotSerialized]
		public BGCurvePointI point;

		public Vector3 position;

		public Vector3 tangent;

		public static TailInfo Default = new TailInfo
		{
			position = Vector3.zero,
			tangent = Vector3.right
		};

		public TailInfo Clone()
		{
			TailInfo tailInfo = new TailInfo();
			tailInfo.index = index;
			tailInfo.position = position;
			tailInfo.tangent = tangent;
			tailInfo.point = point;
			return tailInfo;
		}
	}
}
