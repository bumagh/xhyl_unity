using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class LinkageCurveData
	{
		public Transform root;

		public Transform node;

		public BGCurve curve;

		public LinkageCurveData master;

		public Func<BGCurve> nodeProvider;

		[HideInInspector]
		public bool isMaster;

		public bool isFixed;

		public LinkageCurveData AddSlave()
		{
			BGCurve bGCurve = nodeProvider();
			LinkageCurveData linkageCurveData = new LinkageCurveData();
			linkageCurveData.root = root;
			linkageCurveData.curve = bGCurve;
			BGCurveUtils.CopyBGCurveBasic(bGCurve, curve);
			linkageCurveData.node = bGCurve.transform;
			linkageCurveData.master = master;
			linkageCurveData.isMaster = false;
			return linkageCurveData;
		}
	}
}
