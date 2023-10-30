using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_LinkageCurveData
	{
		public Transform root;

		public Transform node;

		public BGCurve curve;

		public FK3_LinkageCurveData master;

		public Func<BGCurve> nodeProvider;

		[HideInInspector]
		public bool isMaster;

		public bool isFixed;

		public FK3_LinkageCurveData AddSlave()
		{
			BGCurve bGCurve = nodeProvider();
			FK3_LinkageCurveData fK3_LinkageCurveData = new FK3_LinkageCurveData();
			fK3_LinkageCurveData.root = root;
			fK3_LinkageCurveData.curve = bGCurve;
			FK3_BGCurveUtils.CopyBGCurveBasic(bGCurve, curve);
			fK3_LinkageCurveData.node = bGCurve.transform;
			fK3_LinkageCurveData.master = master;
			fK3_LinkageCurveData.isMaster = false;
			return fK3_LinkageCurveData;
		}
	}
}
